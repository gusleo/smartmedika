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
import { MedicalStaffSpesialisModel }       from '../model/medicalStaffSpesialisModel';
import { plainToClass }                     from 'class-transformer';

@Injectable()
export class SpesialisService {
    private _baseUri: string;
    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();
    }

    getAll(): Observable<Array<MedicalStaffSpesialisModel>>
    {
        let uri = this._baseUri + "specialist";
            return this._auth.AuthGet(uri)
            .map((res:Response) =>{
                return res.json();
            })
            .map((object: any) => {
               return plainToClass(MedicalStaffSpesialisModel, object);   
            });     
    }

    getById(id: number): Observable<MedicalStaffSpesialisModel>
    {
        let uri = this._baseUri + "specialist/" + id;
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        let items = new MedicalStaffSpesialisModel();
                        return items = responseData.json();
                    });
    }

    save (body: MedicalStaffSpesialisModel){
        let uri = this._baseUri + "specialist";
            return this._auth.AuthPost(uri, body)
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    update (body: MedicalStaffSpesialisModel, id: number){
        let uri = this._baseUri + "specialist/" + id;
            return this._auth.AuthPut(uri, body)
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    delete(id: number): Observable<MedicalStaffSpesialisModel[]>
    {
        let uri = this._baseUri + "specialist/" + id;
        return this._auth.AuthDelete(uri)
                    .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
            
    }    

    import(files: any){
        let urlImport = this._baseUri + "specialist/importdata" + files;
        let _uploader:FileUploader = new FileUploader({url: urlImport});    
        
        //_uploader.addToQueue(files);
    }    
}