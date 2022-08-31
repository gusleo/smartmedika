import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Observable } from 'rxjs/Observable';
import { StaffService, GeoLocationService } from '../../services';
import { PaginationEntity, MedicalStaffModel, HospitalStatus, RegionModel, MedicalStaffSpesialisModel, 
  MedicalStaffSpecialistMapModel, MedicalStaffStatus, MedicalStaffImageModel } from '../../model';
import { EnumValues } from 'enum-values';

@Component({
  selector: 'app-list-doctor-admin',
  templateUrl: './list-doctor-admin.component.html',
  styleUrls: ['./list-doctor-admin.component.scss']
})
export class ListDoctorAdminComponent implements OnInit {
  results: MedicalStaffModel[];
  status:object[];
  page:number = 0;
  PAGE_SIZE:number = 20; 
  total:number = 0;
  form:FormGroup;
  keyword: string = "";

  constructor(private staffService: StaffService, public snackBar: MdSnackBar, private geoServices: GeoLocationService, private router: Router, 
    private formBuilder: FormBuilder) {
    this.status = EnumValues.getNamesAndValues(MedicalStaffStatus);   
    this.createForm();   
  }

  ngOnInit() {
    this.getAllDoctor();
  }  

  async getAllDoctor() {
      let res = await this.staffService.getDoctorAdminBypaging((this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();     
      this.results = res.items;
      this.total = res.totalCount;      
  }

  createForm() {
      this.form = this.formBuilder.group({
          keyword: ['']
      });  
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

  async searchDoctor() {
    this.page = 0;
    let res = await this.staffService.getDoctorAdminBypaging((this.page + 1), this.PAGE_SIZE, this.keyword).toPromise();
    this.results = res.items;
    this.total = res.totalCount;
  }

  onPage(event) {
    this.page = event.offset;
    this.getAllDoctor();
  }  

  add() {
      this.router.navigate(['/doctor/register-doctor/0']);
  } 

  onStatusChange(id:number, event){
    let status = event;
    this.staffService.changeStatus(id, status).subscribe(res =>{
      confirm("Sukses mengganti status");
    }, error =>{
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
