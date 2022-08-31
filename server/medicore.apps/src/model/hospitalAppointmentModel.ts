import { Type } from 'class-transformer';
import { IEntityBase } from './IEntityBase';
import { AppointmentStatus } from './enumModel';
import { UserModel } from './userModel';
import { HospitalModel } from './hospitalModel';
import { MedicalStaffModel } from './medicalStaffModel';
import { HospitalAppointmentDetailModel } from './hospitalAppointmentDetailModel';

export class HospitalAppointmentModel implements IEntityBase {
    id: number;
    hospitalId: number;
    medicalStaffId: number;
    appointmentDate: Date;
    appointmentStatus: AppointmentStatus;
    queueNumber: number;

    //if appointment create the by the logged user then fill it
    userId: number;

    //if not registered user then use their PhoneNumber
    phoneNumber: string;

    //if not registered user then he/she use PatientName   
    patientName: string;

    //if not registered user fill on PatientProblems
    //if registered user, fill on HospitalAppointmentDetail
    patientProblems: string;

    //if user cancelled, fill the reason
    //cancelled status is from AppointmentStatus
    cancelledReason: string;

    //region model
    @Type(() => UserModel)
    user: UserModel;
    @Type(() => HospitalModel)
    hospital: HospitalModel;
    @Type(() => MedicalStaffModel)
    medicalStaff: MedicalStaffModel;
    @Type(() => HospitalAppointmentDetailModel)
    appointmentDetails: Array<HospitalAppointmentDetailModel>;

    constructor() {
        this.id = 0;
        this.hospitalId = 0;
        this.medicalStaffId = 0;
        this.appointmentDate = null;
        this.appointmentStatus = null;
        this.queueNumber = 0;
        this.userId = 0;
        this.phoneNumber = null;
        this.patientName = null;
        this.patientProblems = null;
        this.cancelledReason = null;
        this.user = null;
        this.hospital = null;
        this.medicalStaff = null;
        this.appointmentDetails = null;
    }
}