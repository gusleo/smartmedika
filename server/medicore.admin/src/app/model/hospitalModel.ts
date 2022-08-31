import { Type }                 from 'class-transformer';
import { PolyClinicToHospitalMapModel }  from './polyClinicToHospitalMapModel';
import { HospitalMetadataModel }      from './hospitalMetadataModel';
import { RegencyModel }         from './regencyModel';
import { HospitalStatus }       from './enumModel';
import { IEntityBase }          from './IEntityBase';
import { WriteHistoryBaseModel }    from './writeHistoryBaseModel';

export class HospitalModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    name: string;
    address: string;
    zipCode: string;
    latitude: number;
    longitude: number;
    regencyId: number;
    isHaveAmbulance: boolean;
    //regency model
    @Type(() => RegencyModel)
    regency: RegencyModel;
    status: HospitalStatus;
    description: string;
    website: string;
    //clinic meta  model
    @Type(() => HospitalMetadataModel)
    clinicMetas: HospitalMetadataModel[];
    //poly clinic maps  model
    @Type(() => PolyClinicToHospitalMapModel)
    polyClinicMaps: PolyClinicToHospitalMapModel[];    

    constructor(){
        super();
        this.id = 0;
        this.address = null;
        this.zipCode = null;
        this.longitude = null;
        this.latitude = null;
        this.regencyId = 0;
        this.isHaveAmbulance = false;
        this.status = HospitalStatus.Active;
        this.description = null;
        this.website = null;
        this.clinicMetas = null;
        this.polyClinicMaps = null;

    }
}

