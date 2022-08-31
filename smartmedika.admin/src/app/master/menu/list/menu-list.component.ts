import { Component, OnInit } from '@angular/core';
import { ApplicationService} from '../../../services';
import { TreeMenuModel, PaginationEntity, MenuType } from '../../../model';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { MenuRegisterDialog } from '../register/menu-register.dialog';
import { DialogConfig } from '../../../';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogsService } from '../../../shared/popup/dialogs.service';

@Component({
  selector: 'list-menu',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.scss']
})
export class MenuListComponent implements OnInit {

  results: TreeMenuModel[];
  parents: Array<TreeMenuModel> = new Array<TreeMenuModel>();
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  autoHide = 3000;
  registerDialog: MdDialogRef<MenuRegisterDialog>

  constructor(private appServices: ApplicationService, private dialog:MdDialog, private message:MdSnackBar, private confirm:DialogsService) { 
    
  }

  async ngOnInit() {
    await this.getParentMenu();
    this.getAllMenu();
  }

  async onPage(event) {
    this.page = event.offset;
    await this.getAllMenu();
  }
  async getAllMenu(){
    let res:PaginationEntity<TreeMenuModel> = await this.appServices.getMenuByTypePaged(MenuType.Admin, this.page + 1, this.PAGE_SIZE).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }
  async getParentMenu(){
    let res: TreeMenuModel[] = await this.appServices.getParentMenuByType(MenuType.Admin).toPromise();
    this.parents = res;
  }
 
  displayParent(id:number):string{
    let parent = this.parents.filter(item => item.id == id);
    if(parent.length > 0)
      return parent[0].displayName;
    else
      return "-";
  }

  onDeleteMenu(menu:TreeMenuModel){
    var deletedId = menu.id;
    this.confirm
        .confirm('Confirm Dialog', 'Apakah anda yakin menghapus menu ' + menu.displayName)
        .subscribe(res => {
            if(res){
                this.delete(deletedId);
            }
        });
  }
  async delete(id:number){
    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;

    let menu = await this.appServices.deleteMenu(id).toPromise();
    this.message.open("Menu " + menu.displayName + " berhasil dihapus.", "", messageConfig);
    this.getAllMenu();
  }
  openRegisterDialog(menu?:TreeMenuModel){
    let value:TreeMenuModel
    if(menu == undefined){
      value = new TreeMenuModel();
      value.menuType = MenuType.Admin;
    }else{
      value = Object.assign(new TreeMenuModel(), menu);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;

    let config:any = DialogConfig;
    config.data = {     
      menu: value
    }

    this.registerDialog = this.dialog.open(MenuRegisterDialog, config);
    this.registerDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
        if(value.id == 0){
         this.page = 0;
            this.message.open("Berhasil menambahkan data", "", messageConfig); 
        }else{
            this.message.open("Berhasil mengedit data", "", messageConfig);            
        }
        this.getAllMenu();
        
      }
      this.registerDialog = null;
    });
  }

}
