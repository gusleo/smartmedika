import { Type }                 from 'class-transformer';
import { IEntityBase }          from './IEntityBase';
import { PolyClinicModel }      from './polyClinicModel';
import { MedicalStaffType }     from './enumModel';

export class MedicalStaffSpesialisModel implements IEntityBase{
    id: number;   
    name: string;
    alias: string;
    description: string;
    staffType: MedicalStaffType;
    polyClinicId:number;
    @Type(() => PolyClinicModel)
    polyclinicList: PolyClinicModel[];   

    constructor(){
        this.id = 0;
        this.name = null;
        this.alias = null;
        this.description = null;
        this.polyClinicId = null;
        this.staffType = MedicalStaffType.Doctor;        
        this.polyclinicList = null;
    }
}