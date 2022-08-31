import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { HospitalStaffStatus } from './enumModel';
import { HospitalModel } from './hospitalModel';
import { MedicalStaffModel, HospitalStaffOperatingHoursModel } from '../model';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class HospitalMedicalStaffModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    hospitalId: number;
    medicalStaffId: number;

    //user hospital model
    @Type(() => HospitalModel)
    hospital: HospitalModel;
    @Type(() => MedicalStaffModel)
    medicalStaff: MedicalStaffModel;
    status: HospitalStaffStatus;
    estimateTimeForPatient?: number;
    @Type(() => HospitalStaffOperatingHoursModel)
    operatingHours: HospitalStaffOperatingHoursModel[];
    
    constructor() {
        super();
        this.id = 0;
        this.hospitalId = 0;
        this.hospital = null;
        this.medicalStaff = null;
        this.status = null;
        this.estimateTimeForPatient = 0;
        this.operatingHours = null;
    }
}