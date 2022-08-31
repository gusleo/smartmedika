import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { HospitalOperatorService } from '../../services';
import { PaginationEntity, HospitalOperatorModel, UserModel } from '../../model';
import { MdSnackBar, MdSnackBarConfig, MdDialog, MdDialogRef } from '@angular/material';
import { DialogsService } from '../../shared/popup/dialogs.service';

@Component({
    selector: 'app-list-cancel-verification',
    templateUrl: './list-cancel-verification.component.html',
    styleUrls: ['./list-cancel-verification.component.scss']
  })
export class ListCancelVerificationComponent implements OnInit {

    results: HospitalOperatorModel[];
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
        let res = await this.services.getOperatorRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
        this.results = res.items;
        this.total = res.totalCount;
        console.log(this.results);
    }

    async onPage(event) {
        this.page = event.offset;
        await this.getAll();
    }      

    async searchStaff() {
        this.page = 0;
        let res = await this.services.getOperatorRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();    
        this.results = res.items;
        this.total = res.totalCount;
    }    

    onAssign(row?: HospitalOperatorModel, usernm?: string) {
        this.model = row;
        this.model.status = 0;
        this.dialog
        .confirm('Confirm Dialog', 'Apakah anda yakin menghapus '+usernm)
        .subscribe(res => {
            if(res){
                this.delete();
            }
        });        
    }

    async delete() {
        await this.services.assignOperator(this.model.id, this.model).toPromise();
        this.infoWindow("Data berhasil dihapus dari verfikasi.");
        this.getAll();        
    }

    infoWindow(message:string) {
        const config = new MdSnackBarConfig();
        config.duration = 3000;
        config.extraClasses = null;
        this.snackBar.open(message, "", config);
      }    
}  