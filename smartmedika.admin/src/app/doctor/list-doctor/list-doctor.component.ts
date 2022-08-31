import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { StaffService, GeoLocationService } from '../../services';
import { PaginationEntity, MedicalStaffModel, HospitalStatus, MedicalStaffSpesialisModel, 
  MedicalStaffSpecialistMapModel, MedicalStaffStatus, MedicalHospitalStaffViewModel, MedicalStaffImageModel } from '../../model';
import { EnumValues } from 'enum-values';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { DummyData } from '../dummyData';

@Component({
  selector: 'app-list-doctor',
  templateUrl: './list-doctor.component.html',
  styleUrls: ['./list-doctor.component.scss']
})
export class ListDoctorComponent implements OnInit {
  results: MedicalStaffModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  hospitalId: number;
  keyword: string = "";
  hospitalStaff = new MedicalHospitalStaffViewModel();
  //rootUrl: string = ConfigService.ImageRoot;

  constructor(private staffService: StaffService, public snackBar: MdSnackBar, private router: Router) {
    this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));
  }

  async ngOnInit() {
    await this.getAllDoctor();
  }  

  async getAllDoctor() {
    let res = await this.staffService.getAllDoctorBySearchPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }

  async onPage(event) {
    this.page = event.offset;
    await this.getAllDoctor();
  }
  
  async searchDoctor() {
    this.page = 0;
    let res = await this.staffService.getAllDoctorBySearchPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }
 
  displaySpecialist(specialist: MedicalStaffSpecialistMapModel[]): string{
    let value = "";
    if(specialist != null && specialist.length > 0){
      specialist.forEach(el =>{
          value += el.medicalStaffSpecialist.name + ", ";
        });

        value = value.substr(0, value.length -2);
    }  
    return value;
  }

  add() {
      this.router.navigate(['/doctor/register-doctor/0']);
  }  

  onAssignToHospital(staffIds:number, hosId: number){
    this.hospitalStaff = {hospitalId: hosId, staffIds: [staffIds], isDeleteFromHospital: true};
    this.staffService.assignToHospital(this.hospitalStaff).subscribe(res => {
        confirm("Dokter telah terhapus dari klinik");
        this.getAllDoctor();
    }, error => {
      confirm(error.statusText);        
    });    
  }  

  showImages(images:MedicalStaffImageModel[]) {
    let res: string = "";
    let img = images.filter(image => image.image.isPrimary == true);
    if(img.length > 0){
      res = img[0].image.imageUrl;
    }else{
      res = "assets/images/headshot.png"; 
    }
    return res;

  }
}
