import { Component, OnInit } from '@angular/core';
import { GeoLocationService} from '../../../services';
import { CountryModel, PaginationEntity, Status } from '../../../model';
import { EnumValues } from 'enum-values';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { CountryRegisterDialog } from '../register/country-register.dialog';
import { DialogConfig } from '../../../';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

@Component({
  selector: 'list-country',
  templateUrl: './list-country.component.html',
  styleUrls: ['./list-country.component.scss']
})
export class CountryListComponent implements OnInit {

  results: CountryModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  autoHide = 3000;
  registerDialog: MdDialogRef<CountryRegisterDialog>

  constructor(private geoServices: GeoLocationService, private dialog:MdDialog, private message:MdSnackBar) { 
    this.status = EnumValues.getNamesAndValues(Status);
  }

  ngOnInit() {
    this.getAllCountry();
  }

  onPage(event) {
    this.page = event.offset;
    this.getAllCountry();
  }
  async getAllCountry(){
    let res:PaginationEntity<CountryModel> = await this.geoServices.getAllCountry(this.page + 1, this.PAGE_SIZE).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }
  onStatusChange(id:number, event){
    let status = event;
    this.geoServices.changeCountryStatus(id, status).subscribe(res =>{
      console.log(res);
      confirm("Sukses mengganti status");
    }, error =>{
      confirm(error.statusText);
    });
  }

  openRegisterDialog(country?:CountryModel){
    let value:CountryModel
    if(country == undefined){
      value = new CountryModel();
    }else{
      value = Object.assign(new CountryModel(), country);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
    
    let config:any = DialogConfig;
    config.data = {     
      country: value
    }

    this.registerDialog = this.dialog.open(CountryRegisterDialog, config);
    this.registerDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
        if(value.id == 0){
         this.page = 0;
          this.message.open("Berhasil menambahkan data", "", messageConfig);
        }else{
          this.message.open("Berhasil mengedit data", "", messageConfig);
        }
        this.getAllCountry();
        
      }
      this.registerDialog = null;
    });
  }

}
