import { PolyClinicModel }  from './polyClinicModel';
import { IEntityBase }      from './IEntityBase';

export class PolyClinicToHospitalMapModel implements IEntityBase{
    id: number;
    hospitalId: number;
    polyClinicId: number;
    polyClinic: PolyClinicModel;

    constructor(id?:number, hospitalId?:number, polyClinicId?:number){
        this.id = id;
        this.hospitalId = hospitalId;
        this.polyClinicId = polyClinicId;
        
        this.polyClinic = null;
    }
}   