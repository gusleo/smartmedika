import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { HospitalModel, ImageModel } from '../model';

export class HospitalImageModel implements IEntityBase {
    id: number;
    hospitalId: number;
    imageId: number;
    @Type(() => HospitalModel)
    hospital: HospitalModel;
    @Type(() => ImageModel)
    image: ImageModel;

    constructor() {
        this.id = 0;
        this.hospitalId = 0;
        this.hospital = null;
        this.image = null;
    }
}

