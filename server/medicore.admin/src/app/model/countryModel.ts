import { Type }                 from 'class-transformer';
import { IEntityBase }          from './IEntityBase';
import { RegionModel }          from './regionModel';

export class CountryModel implements IEntityBase{
    id: number;   
    name: string;
    code: string;
    //region model
    @Type(() => RegionModel)
    regionList: RegionModel[];   

    constructor(){
        this.id = 0;
        this.name = null;
        this.code = null;
        this.regionList = null;
    }
}