import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { ConfigService } from './configService';
import { AuthService } from '../providers';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';

//Grab everything with import model;
import { MedicalStaffModel, PaginationEntity, NearestDoctorOrHospitalModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class MedicalStaffService {
    private _baseUri: string;
    constructor(protected http: Http, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "staff/doctor";
    }

    getNearestDoctor(params: NearestDoctorOrHospitalModel): Observable<PaginationEntity<MedicalStaffModel>> {
        let uri = this._baseUri + "/nearest/"+ params.longitude + "/" + params.latitude + "/" + params.radius + "/" + params.polyClinicIds + "/" + params.pageIndex + "/" + params.pageSize;
        let headers = this.auth.getAuthToken();

        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let pagination: PaginationEntity<MedicalStaffModel> = new
                PaginationEntity<MedicalStaffModel>(object.page, object.pageSize, object.totalPages,
                object.totalCount, object.count);
                pagination.items = plainToClass(MedicalStaffModel, object.items);
                return pagination; 
            });
    }
}