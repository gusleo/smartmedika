import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { HospitalModel } from './hospitalModel';
import { HospitalMedicalStaffModel } from './hospitalMedicalStaffModel';

export class HospitalStaffOperatingHoursModel extends WriteHistoryBaseModel implements IEntityBase{
    id:number;
    day: number;
    hospitalMedicalStaffId: number;
    startTime: string;
    endTime: string;
    isClossed: boolean;
    hospitalStaff: HospitalMedicalStaffModel;

    constructor()
    constructor(day:number, isClossed:boolean)
    constructor(day:number, isClossed:boolean, startTime: string, endTime: string)
    constructor(day?:number, isClossed?:boolean, startTime?:string, endTime?:string){       
        super();
        this.day = day;
        this.startTime = startTime;
        this.endTime = endTime;        
        this.isClossed = isClossed;
        this.hospitalStaff = null;
       
    }
}