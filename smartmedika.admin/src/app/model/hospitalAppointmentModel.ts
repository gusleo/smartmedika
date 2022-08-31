import { IEntityBase } from './IEntityBase';
import { AppointmentStatus } from './enumModel';
import { UserModel } from './userModel';
import { HospitalModel } from './hospitalModel';
import { MedicalStaffModel } from './medicalStaffModel';
import { HospitalAppointmentDetailModel } from './hospitalAppointmentDetailModel';

export class HospitalAppointmentModel implements IEntityBase{
    id: number;
    hospitalId: number;
    medicalStaffId: number;
    appointmentDate: Date;
    appointmentStatus: AppointmentStatus;
    queueNumber: number;
    //startTime?: Date;
    //endTime?: Date;
    userId?: number;
    phoneNumber: string;
    patientName : string;
    patientProblems: string;
    cancelledReason: string;
    deviceId: string;

    user: UserModel;    
    hospital: HospitalModel;
    medicalStaff: MedicalStaffModel;
    appointmentDetails: HospitalAppointmentDetailModel;        

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
        this.deviceId = null;       
        this.appointmentDetails = null;
    }
}

export class PostAppointmentViewModel {
    hospitalId: number;
    pageIndex: number;
    pageSize: number;
    staffId: number;
    startDate: Date;
    endDate: Date;
    filter: any[];
}