import { Component, OnInit, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';

//config API & Grab everything with import model;
import { MedicalStaffSpesialisModel } from '../../../../model';
import { SpecialistService } from '../../../../services';

@Component({
  selector: 'master-deletespecialistdialog',
  templateUrl: './deletespecialist.dialog.html',
  styleUrls: ['./deletespecialist.dialog.scss']
})
export class DeleteSpecialistDialog{
  model = new MedicalStaffSpesialisModel();  
  onRequest:boolean;
  
  constructor(public dialogRef: MdDialogRef<DeleteSpecialistDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
    public specialistService: SpecialistService) {

        this.model = data.staff;
        if(this.model == undefined) {
            this.model = new MedicalStaffSpesialisModel();
        }    
    }  

  close(){
      this.dialogRef.close();
  }  
    
  async delete() {
      this.onRequest = true;
      let response:any;

      response = await this.specialistService.delete(this.model.id).toPromise();
      this.onRequest = false;
      this.dialogRef.close({
          model: response
      });
  }
}