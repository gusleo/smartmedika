import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
// config API & Grab everything with import model;
import { HospitalOperatorModel, UserRoleViewModel, UserModel } from '../../model';
import { HospitalOperatorService, RolesService } from '../../services';
import { element } from 'protractor';


@Component({
    selector: 'app-registerrulesdialog',
    templateUrl: './register-rules.dialog.html',
    styleUrls: ['./register-rules.dialog.scss']
})
export class RegisterRulesDialog implements OnInit {
    
    model = new UserModel();
    rolesModel: Array<UserRoleViewModel>;
    onRequest: boolean = false;
    dataRoles: any[] = [];
    resultRoles: any[] = [];

    constructor(public dialogRef: MdDialogRef<RegisterRulesDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
                public services: HospitalOperatorService, private rolesService: RolesService) {
        this.model = data.operator;


        console.log(this.model);
    }

    ngOnInit(){
        this.loadRoles();        
    }

    close(){
        this.dialogRef.close();
    }
        
    async loadRoles(){
        let checked: boolean = false;
        let res = await this.rolesService.getAll().toPromise();
        this.resultRoles = await this.rolesService.getDetail(this.model.id).toPromise();
        console.log(this.resultRoles);
        this.rolesModel = new Array<UserRoleViewModel>();
        var model = this.model;
        res.forEach(element => {
            checked = false;
            this.resultRoles.forEach(el => {
                if(element.name == el){
                    checked = true;                                   
                }
            });
            /*if(model.id == element.id){
                checked = true;                
            }else{
                checked = false;
            }*/
            this.rolesModel.push({
            id: element.id,
            name: element.name,
            normalizedName: element.normalizedName,
            isChecked: checked
          });  
        });

    }

    async save(){
        this.onRequest = true;   
        let roles: string = '';
        let num: number = 0;
        let userId: number = 0;
        let response:any;

        this.rolesModel.forEach(element => {                        
            if(element.isChecked == true){
                /*num = num + 1;
                if(num == 1){                    
                    roles += element.name;                                    
                }else{
                    roles += ', '+element.name;                                    
                }  */
                
                this.dataRoles.push(element.name);
            }
        });

        userId = this.model.id;
        //this.dataRoles.push(roles); 
        var hasil = await this.rolesService.assignToUser(userId, this.dataRoles).toPromise();
        this.onRequest = false;

        console.log(this.dataRoles);
        this.dialogRef.close({
            model: response
        });        
    }

}