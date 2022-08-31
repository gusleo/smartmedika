import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { CountryModel, RegionModel, UTCTimeBaseModel  } from '../../../model';
import { GeoLocationService } from '../../../services';

@Component({
  selector: 'app-regionregisterdialog',
  templateUrl: './region-register.dialog.html',
  styleUrls: ['./region-register.dialog.scss']
})
export class RegionRegisterDialog implements OnInit {
   
    countries:CountryModel[];
    region:RegionModel;
    utcs:UTCTimeBaseModel[];
    onRequest:boolean;
    form: FormGroup;
    constructor(public dialogRef: MdDialogRef<RegionRegisterDialog>, private geoService: GeoLocationService , @Inject(MD_DIALOG_DATA) public data: any, private formBuilder: FormBuilder){
        this.region = data.region;   
        if(this.region == undefined){
          this.region = new RegionModel();
          this.region.countryId = 1;
        }    
       this.form = this.formBuilder.group({
          nameMessage: [null, Validators.compose([Validators.required])],
          countryMessage: [null, Validators.compose([Validators.required])],
          utcMessage: [null, Validators.compose([Validators.required])]          
      });        
    }

    async ngOnInit(){
      let result = await this.geoService.getAllCountry(1, 10000).toPromise();
      this.countries = result.items;      
      this.getUtcTimes();
    }

    onCountryChange(event){
      this.getUtcTimes();
    }

    async getUtcTimes(){
      let countryId = this.region.countryId == 0 ? 1 : this.region.countryId;
      let res = await this.geoService.getAllUtcByCountry(countryId, true, 1, 10000).toPromise();
      this.utcs = res.items;
    }

    

    close(){
        this.dialogRef.close();
    }
    async save(){
        this.onRequest = true;
        let response:any;
        if(this.region.id == 0){
            response = await this.geoService.addRegion(this.region).toPromise();
        }else{
            response = await this.geoService.editRegion(this.region.id, this.region).toPromise();
        }
        this.onRequest = false;
        
        this.dialogRef.close({
            region: response
        });
    }  

}
