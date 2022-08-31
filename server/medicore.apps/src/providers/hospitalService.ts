import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';
import { AuthService } from '../providers';

//Grab everything with import model;
import { HospitalModel, PaginationEntity, NearestDoctorOrHospitalModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class HospitalService {
    private _baseUri: string;
    constructor(protected http: Http, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "hospital";
    }

    getHospitalList(params: NearestDoctorOrHospitalModel): Observable<PaginationEntity<HospitalModel>> {
        let uri = this._baseUri + "/nearest/"+ params.longitude + "/" + params.latitude + "/" + params.radius + "/" + params.polyClinicIds + "/" + params.pageIndex + "/" + params.pageSize;
        let headers = this.auth.getAuthToken();

        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let pagination: PaginationEntity<HospitalModel> = new
                    PaginationEntity<HospitalModel>(object.page, object.pageSize, object.totalPages,
                    object.totalCount, object.count);
                pagination.items = plainToClass(HospitalModel, object.items);
                return pagination;
            });
    }

    getHospitalDetail(hospitalId: number): Observable<HospitalModel> {
        let uri = this._baseUri + "/" + hospitalId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let item: HospitalModel = plainToClass(HospitalModel, object as Object);
                return item;
            });
    }
}