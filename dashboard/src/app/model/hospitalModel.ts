
import { PolyClinicToHospitalMapModel }  from './polyClinicToHospitalMapModel';
import { HospitalMetadataModel }      from './hospitalMetadataModel';
import { RegencyModel }         from './regencyModel';
import { HospitalStatus }       from './enumModel';
import { IEntityBase }          from './IEntityBase';
import { WriteHistoryBaseModel }    from './writeHistoryBaseModel';
import { HospitalOpratingHoursModel } from './hospitalOperatingHoursModel';
import { HospitalImage } from './hospitalImageModel';

export class HospitalModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    name: string;
    address: string;
    zipCode: string;
    latitude?: number;
    longitude?: number;
    regencyId: number;
    isHaveAmbulance: boolean;    
    
    status: HospitalStatus;
    description: string;
    website: string;
    primaryPhone: string;
    secondaryPhone: string;
    primaryEmail: string;
    secondaryEmail:string;

    //regency model
   
    regency: RegencyModel;

    //clinic meta  model
   
    operatingHours: HospitalOpratingHoursModel[];

    //poly clinic maps  model
    
    polyClinicMaps: PolyClinicToHospitalMapModel[];    

    
    images: HospitalImage[];

    constructor(){
        super();     
        this.id = 0; 
        this.primaryEmail = '';
        //this.secondaryEmail = '';  
        this.operatingHours = null;
        this.polyClinicMaps = null;
        this.regency = null;

    }
}

export class HospitalViewModel{
    id: number;
    name: string;

    constructor() {
        this.id = 0;
        this.name = null;
    }    
}