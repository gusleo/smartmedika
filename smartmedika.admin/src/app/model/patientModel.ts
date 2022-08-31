import { IEntityBase } from './IEntityBase';
import { PatientStatus, RelationshipStatus, Gender } from './enumModel';
import { UserModel } from '../model';

export class PatientModel implements IEntityBase{
    id: number;
    patientName: string;
    relationshipStatus: RelationshipStatus;
    patientStatus: PatientStatus;
    gender: Gender;
    dateOfBirth?: string; 
    associatedUserId: number;
    associatedToUser: UserModel;    

    constructor(id: number, patientName: string, dateOfBirth?: string ) {
        this.id = 0;
        this.patientName = patientName;
        this.dateOfBirth = dateOfBirth;
        //this.patientStatus = null;
    }
}