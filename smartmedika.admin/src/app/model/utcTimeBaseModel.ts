import { IEntityBase }          from './IEntityBase';
import { CountryModel }         from './countryModel';
import { Status } from './enumModel';

export class UTCTimeBaseModel implements IEntityBase{
    id: number;
    countryId: number;
    name: string;
    code: string;
    utc: number;
    status: Status;
    country: CountryModel;

    constructor(){
        this.id = 0;
        this.countryId = 0;
        this.name = null;
        this.utc = 0;
        this.country = null;
    }
}