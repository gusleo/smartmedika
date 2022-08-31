import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HospitalOperatorService } from '../../services';
import { PaginationEntity, HospitalOperatorModel, UserModel } from '../../model';
import { MdSnackBar, MdSnackBarConfig, MdDialog, MdDialogRef } from '@angular/material';
import { DialogsService } from '../../shared/popup/dialogs.service';

@Component({
    selector: 'app-list-verification-users',
    templateUrl: './list-verification-user.component.html',
    styleUrls: ['./list-verification-user.component.scss']
  })
export class ListVerificationUserComponent implements OnInit {

    results: UserModel[];
    model: HospitalOperatorModel = new HospitalOperatorModel();
    status:object[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    hospitalId: number;
    keyword: string = "";
    form:FormGroup;

    constructor(private services: HospitalOperatorService, public snackBar: MdSnackBar, private router: Router, private formBuilder: FormBuilder, 
    private dialog:DialogsService) {
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
        let res = await this.services.getOperatorNonRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
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
        let res = await this.services.getOperatorNonRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();    
        this.results = res.items;
        this.total = res.totalCount;
    }    

    onAssign(row?: UserModel, usernm?: string) {
        this.dialog
        .confirm('Confirm Dialog', 'Apakah anda yakin menverifikasi '+usernm)
        .subscribe(res => {
            if(res){
                var data = new HospitalOperatorModel();
                data.id = 0;
                data.hospitalId = this.hospitalId;
                data.userId = row.id;
                data.status = 1;
                this.verification(data);
            }
        });        
    }

    async verification(data) {
        var res = await this.services.addassignOperator(data).toPromise();
        this.infoWindow("Data telah terverfikasi.");
        this.getAll();        
    }

    infoWindow(message:string) {
        const config = new MdSnackBarConfig();
        config.duration = 3000;
        config.extraClasses = null;
        this.snackBar.open(message, "", config);
      }    
}  