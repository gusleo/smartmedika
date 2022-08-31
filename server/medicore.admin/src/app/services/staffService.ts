import { Injectable }                   from '@angular/core';
import { Http, Response, Headers }      from '@angular/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';
import { Observer }                     from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { FileUploader } 				from 'ng2-file-upload';
//config API & Grab everything with import model;
import { ConfigService }                    from '../services/configService';
import { AuthService }                      from '../auth';
import { PaginationEntity }                 from '../model/paginationEntity';
import { MedicalStaffModel }                from '../model';
import { plainToClass }                     from 'class-transformer';

@Injectable()
export class StaffService{
    private _baseUri: string;
    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();
    }

    getAll(): Observable<PaginationEntity<MedicalStaffModel>>
    {
        let uri = this._baseUri + "staff";
            return this._auth.AuthGet(uri)
                    .map((object: any) => {
                        let pagination: PaginationEntity<MedicalStaffModel> = new 
                        PaginationEntity<MedicalStaffModel>(object.page, object.totalPages, 
                        object.totalCount, object.count); 
                        pagination.items = plainToClass(MedicalStaffModel, object.items);
                        return pagination;  
                    })
                    .catch((error:any) => Observable.throw(error.json().error || 'Server error'));    
    }
    save (body: MedicalStaffModel){
        let uri = this._baseUri + "staff";
            return this._auth.AuthPost(uri, body)
                .map((res:Response) => res.json())
                .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    update (body: MedicalStaffModel, id: number){
        let uri = this._baseUri + "staff/" + id;
            return this._auth.AuthPut(uri, body)
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    delete(id: number): Observable<string>
    {
        let uri = this._baseUri + "staff/" + id;
        return this._auth.AuthDelete(uri)
                    .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
            
    }  
}