import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl }  from '@angular/forms';
import { CustomValidators }  from 'ng2-validation';
import { OperatingHoursDialog } from '../popup/operatinghours.dialog';
import { DummyData } from '../dummyData';
import { DialogConfig } from '../../';
import { DateHelper, Message} from '../../../libs';

import { HospitalStaffOperatingHoursModel, HospitalMedicalStaffModel } from '../../model';
import { StaffService } from '../../services';

@Component({
  selector: 'app-schedule-doctor',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleDoctorComponent implements OnInit {

    //operatingHours: Array<HospitalStaffOperatingHoursModel>;    
    hospitalMedicalStaff = new HospitalMedicalStaffModel();
    hospitalMedicalOperatingHours: HospitalMedicalStaffModel[];
    dummy:DummyData = new DummyData();
    //dialog
    operatingHoursDialog: MdDialogRef<OperatingHoursDialog>
    titleForm: string;
    id: number;
    hospitalId: number;
    onRequest:boolean = false;
    estimateTime: number = 0;
    
    constructor(public dialog: MdDialog, private router: Router, private route: ActivatedRoute, private staffService: StaffService, private message:Message) {
        this.titleForm = 'Atur Jadwal';
        this.route.params.subscribe(params => {
             this.id =  params['id'];
             this.hospitalId = params['hospitalId'];
        });
    }

    ngOnInit() {
      this.loadOperatingHours();
    }

    async loadOperatingHours() {
        let res = await this.staffService.getOperatingHours(this.hospitalId, this.id).toPromise();
        this.hospitalMedicalStaff = res;
        if(this.hospitalMedicalStaff.estimateTimeForPatient == null)
          this.hospitalMedicalStaff.estimateTimeForPatient = 10;

        //console.log(res);
        if(this.hospitalMedicalStaff.operatingHours.length == 0){
          this.hospitalMedicalStaff.operatingHours = this.dummy.getDefaultOperatingHour();          
        }
    }

    openCloseClinic(day:number, isClossed:boolean){
      let index:number = this.hospitalMedicalStaff.operatingHours.findIndex(x => x.day == day);
      this.hospitalMedicalStaff.operatingHours[index].isClossed = isClossed;
    }
    openOperatingHours(day:number, startTime: string, endTime: string){
      let config:any = DialogConfig;
      config.data = {
        title: "Jadwal Praktek Dokter Hari " + this.getDayName(day),
        day: day,
        startTime: startTime,
        endTime: endTime
      }
      this.operatingHoursDialog = this.dialog.open(OperatingHoursDialog, config);
      this.operatingHoursDialog.afterClosed().subscribe(result => {
        if(result != undefined){
          let index:number = this.hospitalMedicalStaff.operatingHours.findIndex(x => x.day == result.day);
          this.hospitalMedicalStaff.operatingHours[index].startTime = result.startTime;
          this.hospitalMedicalStaff.operatingHours[index].endTime = result.endTime;
          this.hospitalMedicalStaff.operatingHours[index].isClossed = false;
        }
        this.operatingHoursDialog = null;
      });
    }    

    getDayName(day:number):string{    
      let res:string = new DateHelper().getDayName(day); 
      return res;

    }

    async saveOperatingHours() {    
        let message = "Sukses merubah jadwal dokter";    
        this.onRequest = true;        
        var result = await this.staffService.updateOpertingHours(this.hospitalMedicalStaff, this.hospitalId, this.id).toPromise();
        //console.log("valids: "+result[2].day);
        this.onRequest = false;
        this.hospitalMedicalStaff.operatingHours = result;
        
        this.message.open(message);
    }

    backClicked() {
      this.router.navigate(['/doctor/list'],{skipLocationChange:true});    
    }

}