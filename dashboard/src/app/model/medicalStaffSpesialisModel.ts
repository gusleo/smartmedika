import { IEntityBase } from './IEntityBase';
import { PolyClinicModel } from './polyClinicModel';
import { MedicalStaffType } from './enumModel';

export class MedicalStaffSpesialisModel implements IEntityBase {
    id: number;
    name: string;
    alias: string;
    description: string;
    staffType: MedicalStaffType;
   
    polyClinicId: number;
    polyclinicList: PolyClinicModel[];

    constructor() {
        this.id = 0;
        this.name = null;
        this.alias = null;
        this.description = null;
        this.polyClinicId = null;
        this.staffType = null;
        this.polyclinicList = null;
    }
}

export class MedicalSpecialistViewModel{
    id: number;
    name: string;
    alias: string;

    constructor() {
        this.id = 0;
        this.name = null;
        this.alias = null;
    }
    
}

export class MedicalStaffSpesialisViewModel extends MedicalSpecialistViewModel{
    isChecked: boolean;
}
