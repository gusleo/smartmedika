import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { ConfigService } from './configService';
import { Storage } from '@ionic/storage';
import { AuthService } from '../providers/authService';
// import { HttpService } from '../providers/http.service';

//Grab everything with import model;
import { PatientModel, PaginationEntity, Status } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class PatientService {
    private _baseUri: string;
    constructor(protected http: Http, public storage: Storage, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "patient";
    }

    getAllPatient(pageIndex: number, pageSize: number): Observable<PaginationEntity<PatientModel>> {
        console.log('patient service - get all patient');
        let uri = this._baseUri + "/" + pageIndex + "/" + pageSize;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                console.log('data:', responseData.json());
                return responseData.json();
            })
            .map((object: any) => {
                let pagination: PaginationEntity<PatientModel> = new
                    PaginationEntity<PatientModel>(object.page, object.pageSize, object.totalPages,
                    object.totalCount, object.count);
                pagination.items = plainToClass(PatientModel, object.items);
                console.log('pagination:', pagination);
                return pagination;
            });
    }

    getPatientDetail(patientId: number): Observable<PatientModel> {
        let uri = this._baseUri + "/" + patientId;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let item: PatientModel = plainToClass(PatientModel, object as Object);
                return item;
            });
    }

    createPatient(params: PatientModel): Observable<PatientModel> {
        let uri = this._baseUri;
        let headers = this.auth.getAuthToken();
        params.patientStatus = Status.Active.valueOf();
        let body = JSON.stringify(params);

        return this.http.post(uri, body, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let item = plainToClass(PatientModel, object as Object);
                return item; 
            });
    }

    updatePatient(patient: PatientModel): Observable<PatientModel> {
        let uri = this._baseUri + "/" + patient.id;
        let headers = this.auth.getAuthToken();
        let body = JSON.stringify(patient);

        return this.http.put(uri, body, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let item = plainToClass(PatientModel, object as Object);
                return item; 
            });
    }
}