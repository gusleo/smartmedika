import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient} from '@angular/common/http';
import { StringHelper } from '../../libs';

//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { PaginationEntity, HospitalOperatorModel, UserRoleViewModel } from '../model';

@Injectable()
export class RolesService{

    private _baseUri: string;
    private slugAssignToUser: string = 'AssignToUser';

    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "roles";
    }        

    getAll(): Observable<Array<UserRoleViewModel>> {
        let uri = StringHelper.concat("/", this._baseUri, "roles");
        return this.http.get<Array<UserRoleViewModel>>(uri);        
    }

    getDetail(id: number): Observable<any[]> {
        let uri = StringHelper.concat("/", this._baseUri, "detail", id);
        return this.http.get<Array<any[]>>(uri);        
    }

    assignToUser(userId: number, roles: string[]){
        let uri = StringHelper.concat("/", this._baseUri, this.slugAssignToUser, userId);
        return this.http.put(uri, roles);
    }
}