import { IEntityBase } from './IEntityBase';
import { RegionModel } from './regionModel';
import { UserModel } from './userModel';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { RegencyModel } from './regencyModel';


export class UserHospitalModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    latitude?: number;
    longitude?: number;
    address: string;
    regencyId: number; 
    regency: RegencyModel;
    userId: number;
    firstName: string;
    lastName: string;
    user: UserModel[];

    constructor() {
        super();
        this.id = 0;
        this.latitude = null;
        this.longitude = null;
        this.address = null;  
        this.regencyId = 0;
        this.regency = null;
        this.userId = 0;
        this.firstName = null;
        this.lastName = null;
        this.user = null;
    }
}