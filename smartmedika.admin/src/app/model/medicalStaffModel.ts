import { IEntityBase } from './IEntityBase';
import { RegionModel } from './regionModel';
import { MedicalStaffType, MedicalStaffStatus } from './enumModel';
import { UserModel } from './userModel';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { MedicalStaffSpecialistMapModel } from './medicalStaffSpecialistMapModel';
import { HospitalMedicalStaffModel }  from './hospitalMedicalStaffModel';
import { RegencyModel } from './regencyModel';
import { MedicalStaffImageModel } from './medicalStaffImageModel';

export class MedicalStaffModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    title: string;
    firstName: string;
    lastName: string;
    staffType: MedicalStaffType;
    medicalStaffRegisteredNumber: string;
    email: string;
    primaryPhone: string;
    secondaryPhone: string;
    associatedToUserId?: number;
    address: string;
    regencyId: number; 
    status: MedicalStaffStatus;

    associatedToUser: UserModel;
    regency: RegencyModel;
    medicalStaffSpecialists: MedicalStaffSpecialistMapModel[];
    medicalStaffClinics: HospitalMedicalStaffModel[];
    images: MedicalStaffImageModel[];

    constructor() {
        super();
        this.id = 0;
        this.title = null;
        this.firstName = null;
        this.lastName = null;
        this.staffType = null;
        this.medicalStaffRegisteredNumber = null;
        this.email = null;
        this.primaryPhone = null;
        this.secondaryPhone = null;    
        this.status = null;
        this.address = null;
        this.regencyId = 0;

        this.associatedToUser = null;
        this.medicalStaffClinics = null;
        this.medicalStaffSpecialists = null;
        this.regency = null;
        this.images = null;
    
    }
}

export class MedicalHospitalStaffViewModel {
    hospitalId: number;
    staffIds: any[];
    isDeleteFromHospital: boolean;
    
}