import { Component, OnInit, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

//config API & Grab everything with import model;
import { HospitalAppointmentModel, MedicalStaffModel, MedicalStaffSpecialistMapModel } from '../../../model';
import { AppoitmentService, StaffService } from '../../../services';

@Component({
  selector: 'app-next-appointmentdialog',
  templateUrl: './next-appointment.dialog.html',
  styleUrls: ['./next-appointment.dialog.scss']
})
export class NextAppointmentDialog implements OnInit{

    form: FormGroup;    
    model = new MedicalStaffModel();  
    appointmentModel = new HospitalAppointmentModel();
    //results = new MedicalStaffModel();
    onRequest:boolean;
    aDate: Date = new Date;
    problemsPatients: string;

    constructor(public dialogRef: MdDialogRef<NextAppointmentDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
                private appoitmentService: AppoitmentService, private formBuilder: FormBuilder, private staffService: StaffService, private datePipe: DatePipe) {
        this.appointmentModel = data.appointments;
        console.log(this.appointmentModel);
        this.appointmentModel.appointmentDate = new Date;
        //this.loadStaff(this.appointmentModel.medicalStaffId);
    }    

    ngOnInit() {
        this.form = this.formBuilder.group({
            aDate: [null, Validators.compose([Validators.required])],
            problemsPatients: [null, Validators.compose([Validators.required])]
        });
    }    

    async loadStaff(id: number) {
        let res = await this.staffService.getDetail(id).toPromise();
        this.model = res;
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

    settingEnDate(dates: string) {
        let res: string;
        
        res = this.datePipe.transform(dates, 'MM/dd/yyyy HH:mm');
        return res;
    }

    saving() {
        this.onRequest = true;
        let resAdate: any = this.settingEnDate(this.aDate.toString());        
        let response:any;
        this.appointmentModel.appointmentDate = resAdate;
        this.appointmentModel.patientProblems = this.problemsPatients;

        this.appointmentModel.appointmentDetails = null;        
        this.appointmentModel.id = 0;
        this.appointmentModel.queueNumber = 0;
        this.appointmentModel.appointmentStatus = 2;
        
        console.log(this.appointmentModel);
        this.appoitmentService.save(this.appointmentModel).subscribe(res => {
            console.log('sukses buat janji baru');                
        }, error => {
            console.log(error.statusText);
        });             
        
        this.onRequest = false;
        
        response = new HospitalAppointmentModel();
        this.dialogRef.close({
            model: response
        });               
    }

    async save() {
        this.onRequest = true;
        let responseNext:any;
        let response:any;
        
        //this.appointmentModel.appointmentStatus = 2;
        //response = await this.appoitmentService.save(this.appointmentModel).toPromise();
        
        this.appointmentModel.appointmentStatus = 1;        
        responseNext = await this.appoitmentService.save(this.appointmentModel).toPromise();

        this.onRequest = false;
        
        this.dialogRef.close({
            model: response
        });       
        
    }    
}