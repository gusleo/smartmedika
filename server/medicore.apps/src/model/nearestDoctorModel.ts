import { IEntityBase } from './IEntityBase';

export class NearestDoctorOrHospitalModel implements IEntityBase {
    id: number;
    longitude: number;
    latitude: number;
    polyClinicIds: number[];
    clue: string;
    radius: number;
    pageIndex: number;
    pageSize: number;

    constructor(){
        this.id = 0;
        this.longitude = null;
        this.latitude = null;
        this.polyClinicIds = null;
        this.clue = null
        this.radius = 0;
        this.pageIndex = 1;
        this.pageSize = 20;
    }
}

