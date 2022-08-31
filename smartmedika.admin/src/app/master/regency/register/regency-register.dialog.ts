import { Component, Inject, OnInit } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { RegencyModel, RegionModel, UTCTimeBaseModel  } from '../../../model';
import { GeoLocationService } from '../../../services';

@Component({
  selector: 'app-register',
  templateUrl: './regency-register.dialog.html',
  styleUrls: ['./regency-register.dialog.scss']
})
export class RegisterRegencyDialog implements OnInit {

    regions:RegionModel[];
    regency:RegencyModel;
    onRequest:boolean;
    form: FormGroup;   
    
    constructor(public dialogRef: MdDialogRef<RegisterRegencyDialog>, private geoService: GeoLocationService , @Inject(MD_DIALOG_DATA) public data: any, private formBuilder: FormBuilder){
      

      this.regency = data.regency;   
        if(this.regency.id == 0){
          this.regency = new RegencyModel();          
        }     
        
        this.buildFormValidation();
    }

    async ngOnInit() {
      let countryId = this.regency.region == undefined ? 1 : this.regency.region.countryId;
      let result = await this.geoService.getAllRegion(false,1, 1000, "", countryId).toPromise();
      this.regions = result.items;                
    }
    
    buildFormValidation(){
      this.form = this.formBuilder.group({
          nameMessage: [null, Validators.compose([Validators.required])],
          longitudeMessage: [null, Validators.compose([Validators.required])],
          latitudeMessage: [null, Validators.compose([Validators.required])],
          regionMessage: [null, Validators.compose([Validators.required])]
      });
    }
    onRegionChange(event){
      this.regency.regionId = event;     
    }
    
    close(){
        this.dialogRef.close();
    }
    async save(){
      this.onRequest = true;
      let response:any;
      if(this.regency.id == 0){
        response = await this.geoService.addRegency(this.regency).toPromise();
      }else{
        response = await this.geoService.editRegency(this.regency.id, this.regency).toPromise();
      }
      this.onRequest = false;
      this.dialogRef.close({
          response: response
      });
    }  

}
