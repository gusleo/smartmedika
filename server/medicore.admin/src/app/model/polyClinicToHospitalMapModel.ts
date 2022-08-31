import { Type }             from 'class-transformer';
import { PolyClinicModel }  from './polyClinicModel';
import { IEntityBase }      from './IEntityBase';

export class PolyClinicToHospitalMapModel implements IEntityBase{
    id: number;
    hospitalId: number;
    polyClinicId: number;

    @Type(() => PolyClinicModel)
    polyClinic: PolyClinicModel;

    constructor(){
        this.id = 0;
        this.hospitalId = 0;
        this.polyClinicId = 0;
        this.polyClinic = null;
    }
}   