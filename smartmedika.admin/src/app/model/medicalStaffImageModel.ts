import { IEntityBase } from './IEntityBase';
import { MedicalStaffModel } from './medicalStaffModel';
import { ImageModel } from './imageModel';

export class MedicalStaffImageModel implements IEntityBase{
    id:number
    medicalStaffId:number;
    imageId:number;
    medicalStaff:MedicalStaffModel;
    image:ImageModel;
}