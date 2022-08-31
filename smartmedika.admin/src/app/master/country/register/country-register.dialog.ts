import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { CountryModel } from '../../../model';
import { GeoLocationService} from '../../../services';

@Component({
  selector: 'country-register-dialog',
  templateUrl: './country-register.dialog.html',
  styleUrls: ['./country-register.dialog.scss']
})

export class CountryRegisterDialog{
    country:CountryModel
    onRequest: boolean;
    form: FormGroup;       
    constructor(public dialogRef: MdDialogRef<CountryRegisterDialog>, private geoServices:GeoLocationService, @Inject(MD_DIALOG_DATA) public data: any, private formBuilder: FormBuilder){
        this.country = data.country;   
        if(this.country == undefined)
            this.country = new CountryModel();
        
       this.form = this.formBuilder.group({
          nameMessage: [null, Validators.compose([Validators.required])],
          codeMessage: [null, Validators.compose([Validators.required])]
      });        
    }


    close(){
        this.dialogRef.close();
    }
    async save(){
        let response:any;
        this.onRequest = true;

        if(this.country.id == 0){
            response = await this.geoServices.addCountry(this.country).toPromise();
        }else{
            response = await this.geoServices.editCountry(this.country.id, this.country).toPromise();
        }

        this.onRequest = false;
        this.dialogRef.close({
            country: response
        });

    }   
    
}