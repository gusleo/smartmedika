import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';

//Grab everything with import model;
import { MedicalStaffModel, PaginationEntity } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class DoctorService {
    private _baseUri: string;
    constructor(protected http: Http) {
        this._baseUri = new ConfigService().getApiUri() + "doctor";
    }

    getDoctorByHospitalId(hospitalId: number, pageIndex: number): Observable<PaginationEntity<MedicalStaffModel>> {
        let uri = this._baseUri + "/" + hospitalId + "/" + pageIndex;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let page: PaginationEntity<MedicalStaffModel> = new PaginationEntity<MedicalStaffModel>(object.page, object.pageSize, object.totalPages, object.totalCount, object.count);
                let items: Array<MedicalStaffModel> = plainToClass(MedicalStaffModel, object);
                page.items = items;
                return page;
            });
    }

    getDoctorDetail(doctorId: number): Observable<MedicalStaffModel> {
        let uri = this._baseUri + "/" + doctorId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let item: MedicalStaffModel = plainToClass(MedicalStaffModel, object as Object);
                return item;
            });
    }
}