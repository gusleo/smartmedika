import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StringHelper } from '../libs';

// config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { PolyClinicModel } from '../model/polyClinicModel';

@Injectable()
export class PolyClinicService {
    private _baseUri: string;

    constructor(protected http: HttpClient) {
        this._baseUri = new ServiceConfiguration().getApiUri() + "polyclinic";
    }

    getAll(): Observable<Array<PolyClinicModel>> {
        let uri = StringHelper.concat("/", this._baseUri);
        return this.http.get<Array<PolyClinicModel>>(uri);
    }

    getById(id: number): Observable<PolyClinicModel> {
        let uri = StringHelper.concat("/", this._baseUri, "detail", id);
        return this.http.get<PolyClinicModel>(uri);
    }

    getByClue(clue: string): Observable<PolyClinicModel>{

        let uri = StringHelper.concat("/", this._baseUri, clue);
        return this.http.get<PolyClinicModel>(uri);
    }    

    save(model: PolyClinicModel): Observable<PolyClinicModel>{
        let uri = StringHelper.concat("/", this._baseUri);
        return this.http.post<PolyClinicModel>(uri, model);  
    }

    update(model: PolyClinicModel, id: number): Observable<PolyClinicModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.put<PolyClinicModel>(uri, model);
            
    }

    delete(id: number): Observable<PolyClinicModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.delete<PolyClinicModel>(uri);
    }
}
