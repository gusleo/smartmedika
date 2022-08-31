import { Type } from 'class-transformer';
import { UserModel } from './userModel';
import { IEntityBase } from './IEntityBase';

export class UserDetailModel implements IEntityBase {
    id: number;
    userId: number;
    firstName: string;
    lastName: string;

    //region model
    @Type(() => UserModel)
    user: UserModel;
}

