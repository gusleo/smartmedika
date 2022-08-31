import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';
import { AuthService } from '../providers/authService';

//Grab everything with import model;
import { HospitalMedicalStaffModel, PaginationEntity, UserAppointmentViewModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class StaffService {
    private _baseUri: string;
    constructor(protected http: Http, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "staff";
    }

    getDoctorOperatingHoursByDayOfWeek(userAppointment: UserAppointmentViewModel, dayOfWeek: number): Observable<PaginationEntity<HospitalMedicalStaffModel>> {
        let uri = this._baseUri + "/doctor/operatinghours/" + userAppointment.id + "/" + dayOfWeek;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let page: PaginationEntity<HospitalMedicalStaffModel> = new PaginationEntity<HospitalMedicalStaffModel>(object.page, object.pageSize, object.totalPages, object.totalCount, object.count);
                let items: Array<HospitalMedicalStaffModel> = plainToClass(HospitalMedicalStaffModel, object);
                page.items = items;
                return page;
            });
    }
}