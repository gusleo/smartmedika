import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { TreeMenuModel, MenuType, RolesMenuModel, IconModel, IconCollection } from '../../../model';
import { ApplicationService} from '../../../services';

@Component({
  selector: 'menu-register-dialog',
  templateUrl: './menu-register.dialog.html',
  styleUrls: ['./menu-register.dialog.scss']
})

export class MenuRegisterDialog implements OnInit{
    menu:TreeMenuModel
    onRequest: boolean;
    form: FormGroup;  
    parents: Array<TreeMenuModel> = new Array<TreeMenuModel>();   
    roles:Array<RolesMenuModel>;  
    icons:Array<IconModel>;
    menutTypeList: any[] = [];

    constructor(public dialogRef: MdDialogRef<MenuRegisterDialog>, private appServices:ApplicationService, @Inject(MD_DIALOG_DATA) public data: any, private formBuilder: FormBuilder){
        this.menu = data.menu; 
        this.roles  = TreeMenuModel.getRoles();        
        this.assignRoles(data.menu);
        this.getMenuType();

       this.form = this.formBuilder.group({
          displayNameMessage: [null, Validators.compose([Validators.required])],
          linkMessage: [null, Validators.compose([Validators.required])],
          parentMenuMessage: [null, Validators.compose([Validators.required])],
          displayIconMessage: [null],
          menuTypeMessage: [null, Validators.compose([Validators.required])], 
          orderMenuMessage: [null, Validators.compose([Validators.required])]         
      });        
    }

    getMenuType() {
        this.menutTypeList.push({code: 0, name: 'Client'});
        this.menutTypeList.push({code: 1, name: 'Admin'});
    }

    assignRoles(menu: TreeMenuModel){
        if(menu.id > 0){
            let role:string[] = menu.roles.split(",");

            var rolex = this.roles;
            role.forEach(element => {
              let index:number = this.roles.findIndex(x => x.roleName == element);
                if(index > -1){
                    this.roles[index].checked = true;
                }
            });
        }
    }
    onParentChange(event){
        let menu = event;
    }
    onDisplayIconChange(event){
        let icon = event;
    }
    async ngOnInit(){
        this.icons = IconCollection.getIconList();
        let icon = new IconModel("Silahkan pilih icon", "");
        this.icons.unshift(icon);
        await this.getParentMenu();
    }
    close(){
        this.dialogRef.close();
    }
    async getParentMenu(){
        let res: TreeMenuModel[] = await this.appServices.getParentMenuByType(MenuType.Admin).toPromise();
        this.parents = res;
        let menu = new TreeMenuModel();
        menu.id = 0
        menu.displayName = "Silahkan pilih menu induk";
        this.parents.unshift(menu);
    
      }
    async save(){
        let response:any;
        this.onRequest = true; 
        //find the checked, get the name and join with "," as string 
        this.menu.roles = this.roles.filter(x => x.checked == true).map(y => y.roleName).join(",");
        this.menu.menuType = 1;

        response = await this.appServices.saveMenu(this.menu).toPromise();       
        
        this.onRequest = false;
        this.dialogRef.close({
            menu: response
        });
    }
    
    onMenuTipeChange(event){
        this.menu.menuType = event;
    }
    
}