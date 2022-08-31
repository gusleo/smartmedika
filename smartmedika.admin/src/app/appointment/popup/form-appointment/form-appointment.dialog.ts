import { Component, OnInit, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';
//config API & Grab everything with import model;
import { HospitalAppointmentModel, MedicalStaffModel, MedicalStaffSpecialistMapModel, HospitalMedicalStaffModel } from '../../../model';
import { AppoitmentService, StaffService } from '../../../services';
import { DateHelper, Message} from '../../../../libs';

@Component({
  selector: 'app-form-appointmentdialog',
  templateUrl: './form-appointment.dialog.html',
  styleUrls: ['./form-appointment.dialog.scss']
})
export class FormAppointmentDialog implements OnInit{
    
    form: FormGroup;    
    model = new MedicalStaffModel();
    appointmentModel = new HospitalAppointmentModel();
    hospitalMedicalStaff = new HospitalMedicalStaffModel();
    hospitalId: number;
    onRequest:boolean;
    aDate: Date = new Date;
    patientProblems: string = '';

    constructor(public dialogRef: MdDialogRef<FormAppointmentDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
                private appoitmentService: AppoitmentService, private formBuilder: FormBuilder, private datePipe: DatePipe, private staffService: StaffService) {
        this.model = data.staff;
        this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));
 
        if(this.model == undefined) {
            this.model = new MedicalStaffModel();            
        }           

        console.log(this.model.medicalStaffClinics[0].operatingHours);
        this.loadOperatingHours();
    }

    ngOnInit() {
        // setting for validation
        this.form = this.formBuilder.group({
            doctor: [''],
            patientName: [null, Validators.compose([Validators.required])],
            noTelp: [null, Validators.compose([Validators.required])],
            patientProblems: [''],
            aDate: ['']
        });
    }        
    async loadOperatingHours() {
        
        let res = await this.staffService.getOperatingHours(this.hospitalId, this.model.id).toPromise();
        this.hospitalMedicalStaff = res;
    }    

    getDayName(day:number):string{    
        let res:string = new DateHelper().getDayName(day); 
        return res;
    }
   
    settingEnDate(dates: string) {
        let res: string;
        
        res = this.datePipe.transform(dates, 'MM/dd/yyyy HH:mm');
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

    close(){
        this.dialogRef.close();
    }
    
    async save() {
        this.onRequest = true;
        let response:any;
        let resAdate: any = this.settingEnDate(this.aDate.toString());
        
        this.appointmentModel.patientProblems = this.patientProblems;
        this.appointmentModel.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));
        this.appointmentModel.medicalStaffId = this.model.id;
        this.appointmentModel.appointmentDate = resAdate;
        this.appointmentModel.appointmentStatus = 2;
        this.appointmentModel.patientProblems = this.patientProblems;
        console.log(this.appointmentModel);
        response = await this.appoitmentService.save(this.appointmentModel).toPromise();

        this.onRequest = false;
        
        this.dialogRef.close({
            model: response
        });               
    }
}