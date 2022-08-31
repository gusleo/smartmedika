import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { HospitalMedicalStaffModel } from '../model';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
 
export class HospitalStaffOperatingHoursModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    day: number;
    hospitalMedicalStaffId: number;
    startTime: string;
    endTime: string;
    isClosed: boolean;
    @Type(() => HospitalMedicalStaffModel)
    hospitalStaff: HospitalMedicalStaffModel;

    constructor() {
        super();
        this.id = 0;
        this.day = 0;
        this.hospitalMedicalStaffId = 0;
        this.startTime = null;
        this.endTime = null;
        this.isClosed = false;
        this.hospitalStaff = null;
    }
}

