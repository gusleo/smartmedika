import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { MedicalStaffType, MedicalStaffStatus } from './enumModel';
import { UserModel, RegencyModel, MedicalStaffSpecialistMapModel, HospitalMedicalStaffModel, MedicalStaffImageModel} from '../model';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class MedicalStaffModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    title: string;
    firstName: string;
    lastName: string;
    medicalStaffType: MedicalStaffType;
    medicalStaffRegisteredNumber: string;
    email: string;
    primaryPhone: string;
    secondaryPhone: string;
    associatedToUserId?: number;
    address: string;
    regencyId: number;
    status: MedicalStaffStatus;
    //user model
    @Type(() => UserModel)
    associatedToUser: UserModel;
    @Type(() => RegencyModel)
    regency: RegencyModel;
    @Type(() => MedicalStaffSpecialistMapModel)
    medicalStaffSpecialists: MedicalStaffSpecialistMapModel;
    @Type(() => HospitalMedicalStaffModel)
    medicalStaffClinics: HospitalMedicalStaffModel;
    @Type(() => MedicalStaffImageModel)
    images: MedicalStaffImageModel;

    constructor() {
        super();
        this.id = 0;
        this.title = null;
        this.firstName = null;
        this.lastName = null;
        this.medicalStaffType = null;
        this.medicalStaffRegisteredNumber = null;
        this.email = null;
        this.primaryPhone = null;
        this.secondaryPhone = null;
        this.associatedToUserId = 0;
        this.address = null;
        this.regencyId = 0;
        this.status = null;
        this.associatedToUser = null;
        this.regency = null;
        this.medicalStaffSpecialists = null;
        this.medicalStaffClinics = null;
        this.images = null;
    }
}