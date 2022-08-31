import { Type } from 'class-transformer';
import { PolyClinicToHospitalMapModel, RegencyModel, IEntityBase, HospitalStaffOperatingHoursModel, HospitalImageModel } from '../model';
import { HospitalStatus } from './enumModel';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class HospitalModel extends WriteHistoryBaseModel implements IEntityBase {
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

    //model
    @Type(() => RegencyModel)
    regency: RegencyModel;
    @Type(() => HospitalStaffOperatingHoursModel)
    operatingHours: HospitalStaffOperatingHoursModel[];
    @Type(() => PolyClinicToHospitalMapModel)
    polyClinicMaps: PolyClinicToHospitalMapModel[];
    @Type(() => HospitalImageModel)
    images: HospitalImageModel[];

    constructor() {
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
        this.primaryPhone = null;
        this.secondaryPhone = null;
        this.polyClinicMaps = null;
        this.images = null;
    }
}