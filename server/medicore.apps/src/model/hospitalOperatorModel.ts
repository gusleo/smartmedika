import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { UserModel } from './userModel';
import { HospitalModel } from './hospitalModel';
import { HospitalStaffStatus } from './enumModel';

export class HospitalOperatorModel implements IEntityBase {
    id: number;
    hospitalId: number;
    userId: number;
    status: HospitalStaffStatus;

    //region model
    @Type(() => UserModel)
    user: UserModel;
    @Type(() => HospitalModel)
    hospital: HospitalModel;
}

