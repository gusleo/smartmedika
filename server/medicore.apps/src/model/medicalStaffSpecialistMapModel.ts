import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { MedicalStaffModel } from './medicalStaffModel';
import { MedicalStaffSpecialistModel } from './medicalStaffSpecialistModel'

export class MedicalStaffSpecialistMapModel implements IEntityBase {
    id: number;
    medicalStaffId: number;
    medicalStaffSpecialistId: number;

    @Type(() => MedicalStaffModel)
    medicalStaff: MedicalStaffModel;
    @Type(() => MedicalStaffSpecialistModel)
    medicalStaffSpecialist: MedicalStaffSpecialistModel;
}