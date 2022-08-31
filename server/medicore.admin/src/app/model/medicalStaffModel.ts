import { Type }                                         from 'class-transformer';
import { IEntityBase }                                  from './IEntityBase';
import { RegionModel }                                  from './regionModel';
import { MedicalStaffType, MedicalStaffStatus }         from './enumModel';
import { UserModel }                                    from './userModel';
import { WriteHistoryBaseModel }                        from './writeHistoryBaseModel';

export class MedicalStaffModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;   
    title: string;
    firstName: string;
    lastName: string;
    medicalStaffType: MedicalStaffType;
    medicalStaffRegisteredNumber: string;
    phoneCodeArea: string;
    phoneNumber: string;
    associatedToUserId: number;
    //user model
    @Type(() => UserModel)
    users: UserModel;
    status: MedicalStaffStatus;

    constructor(){
        super();
        this.id = 0;
        this.title = null;
        this.firstName = null;
        this.lastName = null;
        this.medicalStaffType = null;
        this.medicalStaffRegisteredNumber = null;
        this.phoneCodeArea = null;
        this.phoneNumber = null;
        this.associatedToUserId = 0;
        this.users = null;
        this.status = null;
    }
}