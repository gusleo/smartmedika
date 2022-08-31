import { IEntityBase }          from './IEntityBase';
import { CountryModel }         from './countryModel';
import { UTCTimeBaseModel }     from './utcTimeBaseModel';
import { RegencyModel }         from './regencyModel';

export class RegionModel implements IEntityBase{
    
    id: number;
    name: string;
    countryId: number;
    utcId: number;
    

   
    country: CountryModel;
    utcTime: UTCTimeBaseModel; 
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