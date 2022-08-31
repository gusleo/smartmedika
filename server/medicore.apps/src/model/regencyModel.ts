import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { RegionModel } from './regionModel';
import { Status } from './enumModel';

export class RegencyModel implements IEntityBase {
    id: number;
    name: string;
    regionId: number;
    status: Status;
    latitude: number;
    longitude: number;
    @Type(() => RegionModel)
    region: RegionModel;

    constructor() {
        this.id = 0;
        this.name = null;
        this.regionId = 0;
        this.latitude = 0;
        this.longitude = 0;
        this.region = null;
    }
}