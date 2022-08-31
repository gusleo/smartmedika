import { Component, Inject } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'operating-houes-dialog',
   templateUrl: './operatinghours.dialog.html',
  styleUrls: ['./operatinghours.dialog.scss']
})

export class OperatingHoursDialog{
    title: string;
    startHour:number;
    startMinute:number;
    endHour:number;
    endMinute:number;
    day:number;
    constructor(public dialogRef: MdDialogRef<OperatingHoursDialog>, @Inject(MD_DIALOG_DATA) public data: any){
        this.title = data.title;
        this.day = data.day;
        this.setTime(data.startTime, data.endTime);
    }

    setTime(startTime: string, endTime:string){
        if(startTime != undefined){
            let startTimes:string[] = startTime.split(":");
            this.startHour = Number(startTimes[0]);
            this.startMinute = Number(startTimes[1]);
        }
        
        if(endTime != undefined){
             let enTimes:string[] = endTime.split(":");
            this.endHour = Number(enTimes[0]);
            this.endMinute = Number(enTimes[1]);
        }

    }

    close(){
        this.dialogRef.close();
    }
    save(){
        let formatStartTime: string = this.format(this.startHour) +  ":" + this.format(this.endMinute);
        let formarEndTime: string = this.format(this.endHour) + ":" + this.format(this.endMinute);
        this.dialogRef.close({
            day: this.day,
            startTime: formatStartTime,
            endTime: formarEndTime
        });
    }
    format(value: number):string{
        if (value < 10){
            return "0" + String(value);
        }else{
            return String(value);
        }
    }
    
}