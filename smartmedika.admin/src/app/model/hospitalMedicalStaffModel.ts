import { IEntityBase } from './IEntityBase';
import { HospitalStaffStatus } from './enumModel';
import { HospitalModel } from './hospitalModel';
import { MedicalStaffModel } from './medicalStaffModel';
import { HospitalStaffOperatingHoursModel } from './hospitalStaffOperatingHoursModel';

export class HospitalMedicalStaffModel implements IEntityBase {
    id:number;
    estimateTimeForPatient: number;   
    hospitalId: number;
    medicalStaffId: number;
    status:HospitalStaffStatus;
    
    hospital:HospitalModel;
    medicalStaff: MedicalStaffModel;
    operatingHours: HospitalStaffOperatingHoursModel[]

}