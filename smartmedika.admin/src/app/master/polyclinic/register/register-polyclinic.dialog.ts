import { Component, OnInit, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
//import { CustomValidators } from 'ng2-validation';
//config API & Grab everything with import model;
import { PolyClinicModel }	from '../../../model';
import { PolyClinicService } from '../../../services';

@Component({
  selector: 'master-registerpolyclinicdialog',
  templateUrl: './register-polyclinic.dialog.html',
  styleUrls: ['./register-polyclinic.dialog.scss']
})
export class RegisterPolyClinicDialog implements OnInit{

  model = new PolyClinicModel();  
  form: FormGroup;
  onRequest:boolean;

  constructor(public dialogRef: MdDialogRef<RegisterPolyClinicDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
    public polyClinicService: PolyClinicService, private formBuilder: FormBuilder) {

        this.model = data.poly;
        if(this.model == undefined) {
            this.model = new PolyClinicModel();
        }    
     
  }

  ngOnInit() {
       this.form = this.formBuilder.group({
          nameMessage: [null, Validators.compose([Validators.required])]
      });    
  }

  close(){
      this.dialogRef.close();
  }  

  async save() {
      this.onRequest = true;
      let response:any;
      if(this.model.id == 0){
          response = await this.polyClinicService.save(this.model).toPromise();
      }else{
          response = await this.polyClinicService.update(this.model, this.model.id).toPromise();
      }
      this.onRequest = false;
      this.dialogRef.close({
          model: response
      });
  }  
}