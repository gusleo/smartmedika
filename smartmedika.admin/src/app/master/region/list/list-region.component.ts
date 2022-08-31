import { Component, OnInit } from '@angular/core';
import { GeoLocationService} from '../../../services';
import { RegionModel, PaginationEntity, Status } from '../../../model';
import { EnumValues } from 'enum-values';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { RegionRegisterDialog } from '../register/region-register.dialog';
import { DialogConfig } from '../../../';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

@Component({
  selector: 'app-region',
  templateUrl: './list-region.component.html',
  styleUrls: ['./list-region.component.scss']
})
export class RegionListComponent implements OnInit {

  results: RegionModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  search:string = "";
  autoHide = 3000;
  registerDialog: MdDialogRef<RegionRegisterDialog>

  constructor(private geoServices: GeoLocationService, private dialog:MdDialog, private message:MdSnackBar) { 
    this.status = EnumValues.getNamesAndValues(Status);
  }

  ngOnInit() {
    this.getAllRegion();
  }

  onPage(event) {
    this.page = event.offset;
    this.getAllRegion();
  }

  async getAllRegion(){
    let res:PaginationEntity<RegionModel> = await this.geoServices.getAllRegion(true, this.page + 1, this.PAGE_SIZE, this.search).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }

  onStatusChange(id:number, event){
    let status = event;
    this.geoServices.changeRegionStatus(id, status).subscribe(res =>{
      console.log(res);
      confirm("Sukses mengganti status");
    }, error =>{
      confirm(error.statusText);
    });
  }

  openRegisterDialog(region?:RegionModel){
    let value:RegionModel
    if(region == undefined){
      value = new RegionModel();
    }else{
      value = Object.assign(new RegionModel(), region);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
    
    let config:any = DialogConfig;
    config.data = {     
      region: value
    }

    this.registerDialog = this.dialog.open(RegionRegisterDialog, config);
    this.registerDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
        if(value.id == 0){
          this.message.open("Berhasil menambahkan data", "", messageConfig);
          this.page = 0;
        }else{
            this.message.open("Berhasil mengedit data", "", messageConfig);      
        }
        this.getAllRegion();
      }
      this.registerDialog = null;
    });
  }

}
