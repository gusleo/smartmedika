import { IEntityBase, HospitalModel, ImageModel } from './';

export class HospitalImage implements IEntityBase{
    id:number;
    hospitalId:number;
    imageId:number;

    hospital: HospitalModel;
    image: ImageModel;
    constructor()
    constructor(imageId:number, hospitalId:number)
    constructor(imageId?:number, hospitalId?:number){
        this.hospitalId = hospitalId;
        this.imageId = imageId;
        this.hospital = null;
        this.image = null;
    }
}