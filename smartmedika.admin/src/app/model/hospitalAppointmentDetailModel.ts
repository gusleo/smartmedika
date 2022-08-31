import { IEntityBase } from './IEntityBase';

import { HospitalModel } from './hospitalModel';
import { PatientModel} from './patientModel';
import { HospitalAppointmentModel } from './hospitalAppointmentModel';

export class HospitalAppointmentDetailModel implements IEntityBase{
    id: number;
    hospitalAppointmentId: number;
    problem: string;
 
    hospital: HospitalModel;
    patient : PatientModel;    
    hospitalAppointment: HospitalAppointmentModel;    
    

    constructor(id:number, hospitalAppointmentId: number, patient: PatientModel ) {
        this.id = 0;
        this.hospitalAppointmentId = hospitalAppointmentId;
        //this.problem = '';
        this.patient = patient;
    }
}