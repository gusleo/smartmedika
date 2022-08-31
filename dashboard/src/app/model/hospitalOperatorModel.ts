import { HospitalStaffStatus } from './enumModel';
import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { HospitalModel } from './hospitalModel';
import { UserModel } from './userModel';


export class HospitalOperatorModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    hospitalId: number;
    userId: number;
    status: HospitalStaffStatus;
    user: UserModel[];
    hospital: HospitalModel[];

    constructor(){
        super();
        this.id = 0;
        this.hospitalId = 0;
        this.userId = 0;
        this.user = null;
        this.hospital = null;
    }
}
