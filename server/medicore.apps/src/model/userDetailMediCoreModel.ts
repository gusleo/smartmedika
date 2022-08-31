import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { RegencyModel } from './regencyModel';

export class UserDetailMediCoreModel implements IEntityBase {
    id: number;
    longitude?: number;
    latitude?: number;
    address: string;
    regencyId: number;

    @Type(() => RegencyModel)
    regency: RegencyModel;
}