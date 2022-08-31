import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { RelationshipStatus, PatientStatus, Gender } from './enumModel';
import { UserModel } from '../model';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class PatientModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    patientName: string;
    relationshipStatus: RelationshipStatus;
    patientStatus: PatientStatus;
    dateOfBirth?: Date;
    gender: Gender;
    isChecked: boolean;
    patientProblem: string;

    //if patient is user
    asociatedUserId?: number;

    //region model
    @Type(() => UserModel)
    associatedUser: UserModel;

    constructor() {
        super();
        this.id = 0;
        this.patientName = null;
        this.relationshipStatus = null;
        this.patientStatus = null;
        this.dateOfBirth = null;
        this.gender = null;
        this.isChecked = false;
        this.patientProblem = null;
        this.asociatedUserId = null;
        this.associatedUser = null;
    }
}