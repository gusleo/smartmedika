import { IEntityBase } from './IEntityBase';
import { MedicalStaffModel } from './medicalStaffModel';
import { MedicalStaffSpesialisModel } from './medicalStaffSpesialisModel';

export class MedicalStaffSpecialistMapModel implements IEntityBase {
    id:number;
    medicalStaffId:number;
    medicalStaffSpecialistId:number;
    medicalStaff: MedicalStaffModel;    
    medicalStaffSpecialist: MedicalStaffSpesialisModel;
    
    constructor()
    constructor(id:number, medicalStaffId:number, medicalStaffSpecialistId:number)
    constructor(id?: number, medicalStaffId?: number, medicalStaffSpecialistId?: number){
        this.id = id;
        this.medicalStaffId = medicalStaffId;
        this.medicalStaffSpecialistId = medicalStaffSpecialistId;
        this.medicalStaff = null;
        this.medicalStaffSpecialist = null;
    }

}