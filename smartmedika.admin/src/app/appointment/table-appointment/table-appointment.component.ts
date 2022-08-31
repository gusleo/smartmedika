import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AppoitmentService } from '../../services';
import { PaginationEntity, HospitalAppointmentModel, PostAppointmentViewModel, AppointmentStatus } from '../../model';
import { MdDialog, MdDialogRef, MdDialogConfig, MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogConfig, DATETIME_FORMAT } from '../../';
import { NextAppointmentDialog } from '../popup/next-appointment/next-appointment.dialog';

@Component({
  selector: 'app-table-appointment',
  templateUrl: './table-appointment.component.html',
  styleUrls: ['./table-appointment.component.scss']
})
export class TableAppointmentComponent implements OnInit {
    
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    numberOfpage: number = 0; 
    staffId: number;
    hospitalId: number;
    varAppointment = new PostAppointmentViewModel();
    results: HospitalAppointmentModel[];
    sDate: Date = new Date;
    eDate: Date = new Date;
    statusName: string = 'Menunggu';
    statusColor: string = 'primary';
    statusDisabled: boolean = true;

    nextAppointmentDialog: MdDialogRef<NextAppointmentDialog>;
    
    constructor(private appoitmentService: AppoitmentService, private router: Router, private route: ActivatedRoute, public snackBar: MdSnackBar, public dialog: MdDialog,
        private formBuilder: FormBuilder, private datePipe: DatePipe) {
            
        this.route.params.subscribe(params => {
            let id =  Number.parseInt(params['id']);
            if(Number.isNaN(id)){
                this.staffId = 0;
            }else{
                this.staffId = id
            }              
        });  
        this.hospitalId = parseInt(localStorage.getItem('globalHospitalId'));    
        this.varAppointment = {hospitalId: this.hospitalId, staffId: this.staffId, pageIndex: this.page + 1, pageSize: this.PAGE_SIZE, startDate: new Date, endDate: new Date, filter: [2]};  
    }    

    async ngOnInit() {
        await this.getAppoitnment();
    }

    async getAppoitnment() {
        let res = await this.appoitmentService.getAllAppointmentByPaging(this.varAppointment).toPromise();
        this.results = res.items;
        this.total = res.totalCount;
        this.numberOfpage = Math.max(this.page * this.PAGE_SIZE, 0);
        console.log(this.results);
    }
    
    getStatusappointment(status:number){
        let name = AppointmentStatus[status];
        return name;
    }    

    settingDate(dates: string) {       
        let res = this.datePipe.transform(dates, DATETIME_FORMAT);
        return res;
    }

    settingEnDate(dates: string) {
        let res: string;
        
        res = this.datePipe.transform(dates, 'MM/dd/yyyy HH:mm');
        return res;
    }

    updateStartDate(newDate) {
        this.sDate = newDate;
    }

    updateEndDate(newDate) {
        this.eDate = newDate;
    }    
    
    async searchAppointment() {
        let resSdate: any = this.settingEnDate(this.sDate.toString());
        let resEdate: any = this.settingEnDate(this.eDate.toString());
        
        this.varAppointment.startDate = resSdate;
        this.varAppointment.endDate = resEdate;
        
        let res = await this.appoitmentService.getAllAppointmentByPaging(this.varAppointment).toPromise();
        this.results = res.items;
        this.total = res.totalCount;
        this.numberOfpage = Math.max(this.page * this.PAGE_SIZE, 0);
    }

    openRegisterDialog(appointments?: HospitalAppointmentModel) {
        if(appointments.appointmentStatus == 2){
            appointments.appointmentStatus = 3;
            
            this.appoitmentService.saveByStatusProcess(appointments.id).subscribe(res => {
                console.log('sukses simpan status proses');
            }, error => {
                console.log(error.statusText);
            })            
        }else{
            appointments.appointmentStatus = 1;
                    
            this.appoitmentService.saveByStatusFinish(appointments.id).subscribe(res => {
                console.log('sukses simpan status finish');                
            }, error => {
                console.log(error.statusText);
            });

            this.disabledStatus(appointments.appointmentStatus);

            let value: HospitalAppointmentModel;
            
            if(appointments == undefined){
                value = new HospitalAppointmentModel();
            }else{
                value = Object.assign(new HospitalAppointmentModel(), appointments);
            }
            
            let config:any = DialogConfig;
            config.width = '600px';
            config.data = {     
                appointments: value
            }    
    
            this.nextAppointmentDialog = this.dialog.open(NextAppointmentDialog, config);
            this.nextAppointmentDialog.afterClosed().subscribe(result => {
                if(result != undefined){    
                this.snackBar.open("Berhasil menambahkan data", "CLOSE");            
                }          
                    this.nextAppointmentDialog = null;
            });                    
        }        
    }

    nameStatus(statusChoice: any) {
        if(statusChoice == 0){
            this.statusName = 'Batal';            
        }else if(statusChoice == 1){
            this.statusName = 'Selesai';            
        }else if(statusChoice == 2){
            this.statusName = 'Menunggu';            
        }else if(statusChoice == 3){
            this.statusName = 'Proses';            
        }

        return this.statusName;
    }

    colorStatus(statusChoice: any){
        
        if(statusChoice == 0){
            this.statusColor = '';            
        }else if(statusChoice == 1){
            this.statusColor = '';            
        }else if(statusChoice == 2){
            this.statusColor = 'primary';            
        }else if(statusChoice == 3){
            this.statusColor = 'warn';            
        }

        return this.statusColor;
    }

    disabledStatus(statusChoice: any){
        
        if(statusChoice == 0){
            this.statusDisabled = true;                        
        }else if(statusChoice == 1){
            this.statusDisabled = true;             
        }else if(statusChoice == 2){
            this.statusDisabled = false;             
        }else if(statusChoice == 3){
            this.statusDisabled = false;             
        }

        return this.statusDisabled;        
    }

    onPage(event) {
        this.page = event.offset;
        this.getAppoitnment();
      }    
}