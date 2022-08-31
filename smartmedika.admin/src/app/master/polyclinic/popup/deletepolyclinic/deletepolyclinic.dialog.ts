import { Component, OnInit, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
//import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
//config API & Grab everything with import model;
import { PolyClinicModel }	from '../../../../model';
import { PolyClinicService } from '../../../../services';

@Component({
  selector: 'master-deletepolyclinicdialog',
  templateUrl: './deletepolyclinic.dialog.html',
  styleUrls: ['./deletepolyclinic.dialog.scss']
})
export class DeletePolyclinicDialog{
  model = new PolyClinicModel();  
  onRequest:boolean;
  
  constructor(public dialogRef: MdDialogRef<DeletePolyclinicDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
    public polyClinicService: PolyClinicService) {

        this.model = data.poly;
        if(this.model == undefined) {
            this.model = new PolyClinicModel();
        }    
    }  

  close(){
      this.dialogRef.close();
  }  
    
  async delete() {
      this.onRequest = true;
      let response:any;

      response = await this.polyClinicService.delete(this.model.id).toPromise();
      this.onRequest = false;
      this.dialogRef.close({
          model: response
      });
  }
}