import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { MedicalStaffType } from './enumModel';
import { PolyClinicModel } from './polyClinicModel';

export class MedicalStaffSpecialistModel implements IEntityBase {
    id: number;
    name: string;
    alias: string;
    polyClinicId?: number;
    description: string;
    staffType: MedicalStaffType;

    @Type(() => PolyClinicModel)
    polyClinic: PolyClinicModel;
}