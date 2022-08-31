import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//Grab everything with import 'rxjs/Rx';
import { Observable } from 'rxjs';

//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { MedicalStaffSpesialisModel } from '../model';
import { StringHelper } from '../libs';

@Injectable()
export class SpecialistService {
    private _baseUri: string;
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "specialist";
    }

    getAll(): Observable<Array<MedicalStaffSpesialisModel>>{

        let uri = StringHelper.concat("/", this._baseUri);
        return this.http.get<Array<MedicalStaffSpesialisModel>>(uri);
    }

    getById(id: number): Observable<MedicalStaffSpesialisModel>{

        let uri = StringHelper.concat("/", this._baseUri, "detail", id);
        return this.http.get<MedicalStaffSpesialisModel>(uri);
    }

    getByClue(clue: string): Observable<MedicalStaffSpesialisModel>{

        let uri = StringHelper.concat("/", this._baseUri, clue);
        return this.http.get<MedicalStaffSpesialisModel>(uri);
    }    

    save(model: MedicalStaffSpesialisModel): Observable<MedicalStaffSpesialisModel>{
        let uri = StringHelper.concat("/", this._baseUri);
        return this.http.post<MedicalStaffSpesialisModel>(uri, model);
    }    
        
    update(model: MedicalStaffSpesialisModel, id: number): Observable<MedicalStaffSpesialisModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.put<MedicalStaffSpesialisModel>(uri, model);
    }    

    delete(id: number): Observable<MedicalStaffSpesialisModel>{
        let uri = StringHelper.concat(",", this._baseUri, id);
        return this.http.delete<MedicalStaffSpesialisModel>(uri);
    }
}