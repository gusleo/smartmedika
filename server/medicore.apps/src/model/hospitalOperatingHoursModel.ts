import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { HospitalModel } from '../model';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
 
export class HospitalOperatingHoursModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    day: number;
    hospitalId: number;
    startTime: string;
    endTime: string;
    isClosed: boolean;
    @Type(() => HospitalModel)
    hospital: HospitalModel;

    constructor() {
        super();
        this.id = 0;
        this.day = 0;
        this.hospitalId = 0;
        this.startTime = null;
        this.endTime = null;
        this.isClosed = false;
        this.hospital = null;
    }
}

