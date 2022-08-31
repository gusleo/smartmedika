import { IEntityBase}  from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { HospitalModel } from './hospitalModel'

export class HospitalOpratingHoursModel extends WriteHistoryBaseModel implements IEntityBase{
    id:number;
    day:number;
    startTime: string;
    endTime:string;
    isClossed:boolean;
    hospitalId:number;

    hospital:HospitalModel;

    constructor()
    constructor(day:number, isClossed:boolean)
    constructor(day:number, isClossed:boolean, startTime: string, endTime: string)
    constructor(day?:number, isClossed?:boolean, startTime?:string, endTime?:string){
        super();
        this.day = day;
        this.isClossed = isClossed;
        this.startTime = startTime;
        this.endTime = endTime;
        this.hospital = null;
        
    }
}