import { IEntityBase }              from  './IEntityBase';
import { MetaType }                 from  './enumModel';
import { WriteHistoryBaseModel }    from './writeHistoryBaseModel';

export class HospitalMetadataModel extends WriteHistoryBaseModel implements IEntityBase{

    id: number;
    hospitalId: number;
    metaType: MetaType;
    jsonValue: string;   

    constructor(){
        super();
        this.id = 0;
        this.hospitalId = 0;
        this.metaType = null;
        this.jsonValue = null;
    } 
}

