import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HospitalOperatorService } from '../../services';
import { PaginationEntity, HospitalOperatorModel, UserModel, UserHospitalModel } from '../../model';
import { MdSnackBar, MdSnackBarConfig, MdDialog, MdDialogRef } from '@angular/material';
import { DialogsService } from '../../shared/popup/dialogs.service';
import { RegisterRulesDialog } from '../register-rules/register-rules.dialog';
import { DialogConfig } from '../../';

@Component({
    selector: 'app-set-rule',
    templateUrl: './set-rules.component.html',
    styleUrls: ['./set-rules.component.scss']
  })
export class SetRulesComponent implements OnInit {

    results: UserHospitalModel[];
    model: HospitalOperatorModel = new HospitalOperatorModel();
    status:object[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    hospitalId: number;
    keyword: string = "";
    form:FormGroup;
    registerRulesDialog: MdDialogRef<RegisterRulesDialog>;
    autoHide = 3000;

    constructor(private services: HospitalOperatorService, public snackBar: MdSnackBar, private router: Router, private formBuilder: FormBuilder, 
    private dialog:DialogsService, public dialogRegister: MdDialog) {
        this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));
        this.createForm(); 
    }

    async ngOnInit() {
        await this.getAll();
    }

    createForm() {
        this.form = this.formBuilder.group({
            keyword: ['']
        });  
    }    
    
    async getAll() {
        let res = await this.services.getSetRules((this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
        this.results = res.items;
        console.log(this.results);
        this.total = res.totalCount;
    }

    async onPage(event) {
        this.page = event.offset;
        await this.getAll();
    }      

    async searchStaff() {
        this.page = 0;
        let res = await this.services.getSetRules((this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();    
        this.results = res.items;
        this.total = res.totalCount;
    }    

    openRegisterDialog(operator?: UserModel) {
        let value: UserModel;

        if(operator == undefined){
            value = new UserModel();      
        }else{
            value = Object.assign(new UserModel, operator);   
        }
        var messageConfig = new MdSnackBarConfig();
        messageConfig.duration = this.autoHide;
        
        let config:any = DialogConfig;
        config.data = {     
            operator: value
        }
    
        this.registerRulesDialog = this.dialogRegister.open(RegisterRulesDialog, config);
        this.registerRulesDialog.afterClosed().subscribe(result => {
          if(result != undefined){       
            if(value.id == 0){
              this.snackBar.open("Berhasil merubah data", "", messageConfig);
            }else{
                this.snackBar.open("Berhasil merubah data", "", messageConfig);      
            }
            this.getAll();
          }
          this.registerRulesDialog = null;
        });        
    }

    infoWindow(message:string) {
        const config = new MdSnackBarConfig();
        config.duration = 3000;
        config.extraClasses = null;
        this.snackBar.open(message, "", config);
      }    
}  