import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';
import { AuthService } from '../providers/authService';

//Grab everything with import model;
import { HospitalAppointmentModel, HospitalAppointmentDetailModel, PaginationEntity, UserAppointmentViewModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class AppointmentService {
    private _baseUri: string;
    constructor(protected http: Http, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "appointment";
    }

    getAppointmentList(pageIndex: number, pageSize: number): Observable<PaginationEntity<HospitalAppointmentModel>> {
        let uri = this._baseUri + "/" + pageIndex + "/" + pageSize;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let page: PaginationEntity<HospitalAppointmentModel> = new PaginationEntity<HospitalAppointmentModel>(object.page, object.pageSize, object.totalPages, object.totalCount, object.count);
                let items: Array<HospitalAppointmentModel> = plainToClass(HospitalAppointmentModel, object);
                page.items = items;
                return page;
            });
    }

    getAppointmentDetail(appointmentId: number): Observable<HospitalAppointmentDetailModel> {
        let uri = this._baseUri + "/" + appointmentId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let item: HospitalAppointmentDetailModel = plainToClass(HospitalAppointmentDetailModel, object as Object);
                return item;
            });
    }

    postAnAppointment(params: UserAppointmentViewModel): Observable<HospitalAppointmentModel> {
        let uri = this._baseUri;
        let headers = this.auth.getAuthToken();
        let body = JSON.stringify(params);

        return this.http.post(uri, body, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let item = plainToClass(HospitalAppointmentModel, object as Object);
                return item; 
            });
    }
}