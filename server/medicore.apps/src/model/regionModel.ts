import { Type }                 from 'class-transformer';
import { IEntityBase }          from './IEntityBase';
import { CountryModel }         from './countryModel';
import { UtcBaseModel }         from './utcBaseModel';
import { RegencyModel }         from './regencyModel';
import { Status }               from './enumModel';

export class RegionModel implements IEntityBase{
    
    id: number;
    name: string;
    countryId: number;
    utcId: number;
    status: Status;
    //country model 
    @Type(() => CountryModel)
    country: CountryModel;    
    //utc model
    @Type(() => UtcBaseModel)
    utcTime: UtcBaseModel;        
    //regency model
    @Type(() => RegencyModel)
    regencyList: RegencyModel[];   

    constructor(){
        this.id = 0;
        this.name = null;
        this.countryId = 0;
        this.utcId - 0;
        this.country = null
        this.utcId = 0;
        this.country = null
        this.utcTime = null
        this.regencyList = null
    }     

}