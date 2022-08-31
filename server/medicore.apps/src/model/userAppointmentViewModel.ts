import { AppointmentStatus, UserPatientAppointmentModel } from '../model';
import { IEntityBase } from './IEntityBase';

export class UserAppointmentViewModel implements IEntityBase {
    id: number;
    status: AppointmentStatus;
    appointmentDate: Date;
    hospitalId: number;
    medicalStaffId: number;
    patientProblems: UserPatientAppointmentModel[];

    constructor() {
        this.id = 0;
        this.status = null;
        this.appointmentDate = null;
        this.hospitalId = 0;
        this.medicalStaffId = 0;
        this.patientProblems = null;
    }
}

