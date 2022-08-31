import { Type }                 from 'class-transformer';
import { IEntityBase }          from './IEntityBase';
import { RegionModel }          from './regionModel';
import { Status }               from './enumModel';

export class CountryModel implements IEntityBase{
    id: number;   
    name: string;
    code: string;
    status: Status;
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