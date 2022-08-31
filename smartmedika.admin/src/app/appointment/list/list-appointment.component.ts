import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig, DateAdapter } from '@angular/material';
import { StaffService } from '../../services';
import { PaginationEntity, MedicalStaffModel, MedicalStaffSpecialistMapModel, HospitalAppointmentModel, 
  MedicalStaffImageModel, PostAppointmentViewModel } from '../../model';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogConfig } from '../../';

@Component({
  selector: 'app-list-appointment',
  templateUrl: './list-appointment.component.html',
  styleUrls: ['./list-appointment.component.scss']
})
export class ListAppointmentComponent implements OnInit {

    results: MedicalStaffModel[];
  //status:object[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    hospitalId: number = 2;

    constructor(private staffService: StaffService, public snackBar: MdSnackBar, private router: Router, 
      public dialog: MdDialog) {
        this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));       
    }   
    
    async ngOnInit() {
      await this.getAllDoctor();
    }  

    async getAllDoctor() {
      let res = await this.staffService.getAllDoctorByPaging(this.hospitalId, (this.page + 1), this.PAGE_SIZE).toPromise();
      this.results = res.items;
   }    

   showImages(images:MedicalStaffImageModel[]) {
      let res: string = "";
      let img = images.filter(image => image.image.isPrimary == true);
      if(img.length > 0){
        res = img[0].image.imageUrl;
      }else{
        res = "assets/images/headshots.png";       
      }
      return res;     
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
}