import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//Grab everything with import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { StringHelper, Base64Helper } from '../../libs';

//config API & Grab everything with import model;
import { AuthService } from '../services';
import { ServiceConfiguration } from '../';
import { PaginationEntity, PostAppointmentViewModel, HospitalAppointmentModel } from '../model';


@Injectable()
export class AppoitmentService{
    private _baseUri: string;
    private slugDoctor: string = "doctor";
    private slugRegistered: string = "registered";    
    private slugNonRegistered: string = "nonregistered";
    private slugBookingDoctor: string = "bookingdoctor";  
    private slugProcess: string = "process";  
    private slugFinish: string = "finish";      
    
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "appointment";
    }

    getAllAppointmentByPaging(model: PostAppointmentViewModel): Observable<PaginationEntity<HospitalAppointmentModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor); 
        return this.http.post<PaginationEntity<HospitalAppointmentModel>>(uri, model);
               
    }

    save(model: HospitalAppointmentModel): Observable<HospitalAppointmentModel> {
        let uri = StringHelper.concat("/", this._baseUri, this.slugBookingDoctor);
            return this.http.post<HospitalAppointmentModel>(uri, model);
    }

    saveByStatusProcess(appointmentId: number):Observable<HospitalAppointmentModel> {
        let uri = StringHelper.concat("/", this._baseUri, this.slugProcess, appointmentId);
        return this.http.get<HospitalAppointmentModel>(uri);
    }

    saveByStatusFinish(appointmentId: number):Observable<HospitalAppointmentModel> {
        let uri = StringHelper.concat("/", this._baseUri, this.slugFinish, appointmentId);
        return this.http.get<HospitalAppointmentModel>(uri);
    }    
}