import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient} from '@angular/common/http';
import { StringHelper } from '../../libs';

//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { PaginationEntity, HospitalOperatorModel, UserModel, UserHospitalModel } from '../model';

@Injectable()
export class HospitalOperatorService{

    private _baseUri: string;
    private _newbaseUri: string;

    private slugNonRegistered: string = "nonregistered"; 

    constructor(protected http: HttpClient){
        this._newbaseUri = new ServiceConfiguration().getApiUri() + "account";
        this._baseUri = new ServiceConfiguration().getApiUri() + "hospitaloperator";
    }    

    getOperatorRegiteredByPaging(hospitalId: number, page: number, itemPerpage: number, clue: string) : Observable<PaginationEntity<HospitalOperatorModel>>{
        let uri = StringHelper.concat("/", this._baseUri, hospitalId, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<HospitalOperatorModel>>(uri);
    }

    getOperatorNonRegiteredByPaging(hospitalId: number, page: number, itemPerpage: number, clue: string) : Observable<PaginationEntity<UserModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this.slugNonRegistered, hospitalId, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<UserModel>>(uri);
    }    

    assignOperator(id: number, model:HospitalOperatorModel):Observable<HospitalOperatorModel> {
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.put<HospitalOperatorModel>(uri, model);       
    }

    addassignOperator(model:HospitalOperatorModel) {
        let uri = StringHelper.concat("/", this._baseUri);
        return this.http.post<HospitalOperatorModel>(uri, model);  
    }

    getSetRules(page: number, itemPerpage: number, clue: string): Observable<PaginationEntity<UserHospitalModel>>{
        let uri = StringHelper.concat("/", this._newbaseUri, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<UserHospitalModel>>(uri);        
    }
}