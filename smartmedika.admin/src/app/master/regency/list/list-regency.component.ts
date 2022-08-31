import { Component, OnInit } from '@angular/core';
import { GeoLocationService} from '../../../services';
import { RegionModel, RegencyModel, CountryModel, PaginationEntity, Status } from '../../../model';
import { EnumValues } from 'enum-values';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { RegisterRegencyDialog } from '../register/regency-register.dialog';
import { DialogConfig } from '../../../';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

@Component({
  selector: 'app-regency',
  templateUrl: './list-regency.component.html',
  styleUrls: ['./list-regency.component.scss']
})
export class RegencyListComponent implements OnInit {

  countries: CountryModel[];
  selectedCountry: CountryModel;
  results: RegencyModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  search:string = "";
  autoHide = 3000;
  includeActive: boolean = true;
  registerDialog: MdDialogRef<RegisterRegencyDialog>

  constructor(private geoServices: GeoLocationService, private dialog:MdDialog, private message:MdSnackBar) { 
    this.status = EnumValues.getNamesAndValues(Status);
  }

  async ngOnInit() {
    await this.getAllCountry();
    this.getAllRegency();
  }

  onPage(event) {
    this.page = event.offset;
    this.getAllRegency();
    
  }

  async getAllCountry(){
    let res: PaginationEntity<CountryModel> = await this.geoServices.getAllCountry(1, 1000).toPromise();
    this.countries = res.items;
    this.selectedCountry = this.countries[0];

  }
  async getAllRegency(){
    let res:PaginationEntity<RegencyModel> = await this.geoServices.getRegencyByClue(this.page + 1, this.PAGE_SIZE, this.includeActive, this.search).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }

  onStatusChange(id:number, event){
    let status = event;
    this.geoServices.changeRegencyStatus(id, status).subscribe(res =>{
      console.log(res);
      confirm("Sukses mengganti status");
    }, error =>{
      confirm(error.statusText);
    });
  }

  openRegisterDialog(regency?:RegencyModel){
    let value:RegencyModel
    if(regency == undefined){
      value = new RegencyModel();
    }else{
      value = Object.assign(new RegencyModel(), regency);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
    
    let config:any = DialogConfig;    
    config.data = {     
      regency: value
    }
    this.registerDialog = this.dialog.open(RegisterRegencyDialog, config);
    this.registerDialog.afterClosed().subscribe(result => {
      if(result != undefined){
        let data = result.response;
        if(value.id == 0){
           this.message.open("Berhasil menambahkan data", "", messageConfig);
           this.page = 0;          
        }else{
          this.message.open("Berhasil mengedit data", "", messageConfig);
        }
        this.getAllRegency();
      }
      this.registerDialog = null;
    });
  }

}
