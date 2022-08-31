
import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { MedicalStaffSpesialisModel } from '../model';
export class DoctorModel implements IEntityBase {

    id: number;
    firstName: string;
    lastName: string;
    specialist: string;
    noSip: string;
    email: string;
    noTelp: string;
    alamat: string;
    city: string;
    expired: string;
    status: number;

    constructor() {
        this.id = 0;
        this.firstName = null;
        this.lastName = null;
        this.specialist = null;
        this.noSip = null;
        this.email = null;
        this.noTelp = null;
        this.alamat = null;
        this.city = null;
        this.expired = null;
        this.status = null;
    }
}
