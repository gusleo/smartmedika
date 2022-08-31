import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { StaffService, GeoLocationService } from '../../services';
import { PaginationEntity, MedicalStaffModel, MedicalStaffSpecialistMapModel, MedicalStaffImageModel } from '../../model';

import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogConfig } from '../../';
import { FormAppointmentDialog } from '../popup/form-appointment/form-appointment.dialog';
import { DialogsService } from '../../shared/popup/dialogs.service';

@Component({
  selector: 'app-register-appointment',
  templateUrl: './register-appointment.component.html',
  styleUrls: ['./register-appointment.component.scss']
})
export class RegisterAppointmentComponent {


    registerAppointmentDialog: MdDialogRef<FormAppointmentDialog>;
    results: MedicalStaffModel[];
   //status:object[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    hospitalId: number = 2;

    constructor(private staffService: StaffService, public snackBar: MdSnackBar, private router: Router, public dialog: MdDialog, private dialogService:DialogsService) {
      this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));  
      this.getAllDoctor();        
    }   
    
    getAllDoctor() {
      this.staffService.getDoctorAndOpertingHour(this.hospitalId, (this.page + 1), this.PAGE_SIZE, '').subscribe(res =>{     
        this.results = res.items;
        //this.total = res.totalCount;      
        console.log(this.results);
      });
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
    
    openRegisterDialog(staff?: MedicalStaffModel) {
      let value: MedicalStaffModel;
      console.log(staff.medicalStaffClinics[0].operatingHours.length);
      if(staff.medicalStaffClinics[0].operatingHours.length <= 0){
        let doctorName = staff.title +' '+ staff.firstName +' '+ staff.lastName +' '+ this.displaySpecialist(staff.medicalStaffSpecialists);
        this.confirm(staff.id, doctorName);
      }else{
        if(staff == undefined){
          value = new MedicalStaffModel();
        }else{
          value = Object.assign(new MedicalStaffModel(), staff);
        }
   
        let config:any = DialogConfig;
        config.width = '600px';
        config.data = {     
          staff: value
        }        
   
        this.registerAppointmentDialog = this.dialog.open(FormAppointmentDialog, config);
        this.registerAppointmentDialog.afterClosed().subscribe(result => {
          if(result != undefined){    
            this.snackBar.open("Berhasil menambahkan data", "CLOSE");            
          }          
              this.registerAppointmentDialog = null;
        });
      }

   } 

   confirm(id:number, name: string){
    var deletedId = id;
    this.dialogService
        .confirm('Confirm Dialog', 'Silahkan atur jadwal terlebih dahulu?')
        .subscribe(res => {
            if(res){
              this.router.navigate(['/doctor/schedule', id, this.hospitalId]);
            }
        });
    
}   
}