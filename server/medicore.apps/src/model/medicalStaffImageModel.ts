import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { MedicalStaffModel, ImageModel } from '../model';

export class MedicalStaffImageModel implements IEntityBase {
    id: number;
    medicalStaffId: number;
    imageId: number;
    //model
    @Type(() => MedicalStaffModel)
    medicalStaff: MedicalStaffModel;
    @Type(() => ImageModel)
    image: ImageModel;

    constructor() {
        this.id = 0;
        this.medicalStaffId = 0;
        this.imageId = 0;
        this.medicalStaff = null;
        this.image = null;
    }
}