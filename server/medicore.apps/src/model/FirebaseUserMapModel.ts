import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { OperatingSystem, UserModel} from '../model';

export class FirebaseUserMapModel implements IEntityBase {
    id: number;
    userId: number;
    deviceId: string;
    deviceToken: string;
    operatingSystem: OperatingSystem;

    //user model
    @Type(() => UserModel)
    user: UserModel;

    constructor() {
        this.id = 0;
        this.userId = 0;
        this.deviceId = null;
        this.deviceToken = null;
        this.operatingSystem = null;
        this.user = null;
    }
}