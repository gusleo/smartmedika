import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { StaffService, GeoLocationService } from '../../services';
import { PaginationEntity, MedicalStaffModel, HospitalStatus, RegionModel, MedicalStaffSpesialisModel, 
  MedicalStaffSpecialistMapModel, RegencyModel, MedicalHospitalStaffViewModel, MedicalStaffImageModel } from '../../model';
import { EnumValues } from 'enum-values';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';

import { DummyData } from '../dummyData';

@Component({
  selector: 'app-list-register-doctor',
  templateUrl: './list-register-doctor.component.html',
  styleUrls: ['./list-register-doctor.component.scss']
})
export class ListRegisterDoctorComponent implements OnInit {
  results: MedicalStaffModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 15; 
  total:number = 0;
  hospitalId: number;
  keyword: string = "";
  form:FormGroup;
  hospitalStaff = new MedicalHospitalStaffViewModel();

  constructor(private staffService: StaffService, public snackBar: MdSnackBar, private geoServices: GeoLocationService, private router: Router, private formBuilder: FormBuilder) {
    this.status = EnumValues.getNamesAndValues(HospitalStatus);   
    this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));
    this.createForm();   
  }

  async ngOnInit() {
    await this.getAllDoctor();
  }  

  async getAllDoctor() {
    let res = await this.staffService.getDoctorNonRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
    this.results = res.items;
    this.total = res.totalCount;    
  }
  
  createForm() {
    this.form = this.formBuilder.group({
        keyword: ['']
    });  
  }

  async searchDoctor() {
    this.page = 0;
    let res = await this.staffService.getDoctorNonRegiteredByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();    
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

  async onPage(event) {
    this.page = event.offset;
    await this.getAllDoctor();
  }  

  add() {
      this.router.navigate(['/doctor/register-doctor/0']);
  }  

  onAssignToHospital(staffIds:number, hosId: number){
    this.hospitalStaff = {hospitalId: hosId, staffIds: [staffIds], isDeleteFromHospital: false};    
    this.staffService.assignToHospital(this.hospitalStaff).subscribe(res => {
        confirm("Dokter telah sukses terverifikasi");
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
