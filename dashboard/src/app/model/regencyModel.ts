import { IEntityBase }          from './IEntityBase';
import { RegionModel }         from './regionModel';

export class RegencyModel implements IEntityBase{

    id: number;
    name: string;
    regionId: number;
    longitude: number;
    latitude:number;
    region: RegionModel;

    constructor(){
        this.id = 0;
        this.name = null;
        this.regionId = 0;
        this.region = null;
    }
}