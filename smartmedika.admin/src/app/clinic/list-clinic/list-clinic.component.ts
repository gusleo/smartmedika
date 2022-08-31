import { Component, OnInit } from '@angular/core';
import { ClinicService } from '../../services';
import { PaginationEntity, HospitalModel, HospitalStatus } from '../../model';
import { EnumValues } from 'enum-values';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

@Component({
  selector: 'app-list-clinic',
  templateUrl: './list-clinic.component.html',
  styleUrls: ['./list-clinic.component.scss']
})
export class ListClinicComponent implements OnInit {

  results: HospitalModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  numberOfpage: number =0; 
  constructor(private clinicService: ClinicService, public snackBar: MdSnackBar) { 
    this.status = EnumValues.getNamesAndValues(HospitalStatus);
  }

  ngOnInit() {
    this.getAllHospital();
  }

  getAllHospital(){
    this.clinicService.getHospitalBySearching((this.page + 1), this.PAGE_SIZE).subscribe(res =>{     
      this.results = res.items;
      this.total = res.totalCount;
      this.numberOfpage = Math.max(this.page * this.PAGE_SIZE, 0);
      
    });
  }

  getStatusName(status:number){
    let name = HospitalStatus[status];
    return name;
  }

  onStatusChange(id:number, event){
    let status = event;
    this.clinicService.changeStatus(id, status).subscribe(res =>{
      console.log(res);
      confirm("Sukses mengganti status");
    }, error =>{
      confirm(error.statusText);
    });
  }

  confirm(message:string) {
    const config = new MdSnackBarConfig();
    config.duration = 3000;
    config.extraClasses = null;
    this.snackBar.open(message, "", config);
  }

  onPage(event) {
    this.page = event.offset;
    this.getAllHospital();
  }


}
