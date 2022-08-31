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
import { PolyClinicModel }                  from '../model/polyClinicModel';
import { plainToClass }                     from 'class-transformer';

@Injectable()
export class PolyClinicService {
    private _baseUri: string;
  
    
    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();
    }

    getAll(): Observable<Array<PolyClinicModel>>
    {
        let uri = this._baseUri + "polyclinic";
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();
                    }).map((object: any) => {
                        return plainToClass(PolyClinicModel, object);
                       
                    });
    }

    getById(id: number): Observable<PolyClinicModel>
    {
        let uri = this._baseUri + "polyclinic/" + id;
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        let items = new PolyClinicModel();
                        return items = responseData.json();
                    });
    }

    save (body: PolyClinicModel){
        let uri = this._baseUri + "polyclinic";
            return this._auth.AuthPost(uri, body)
                        //.map((res:Response) => res.json())
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    update (body: PolyClinicModel, id: number){
        let uri = this._baseUri + "polyclinic/" + id;
            return this._auth.AuthPut(uri, body)
                        //.map((res:Response) => res.json())
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
    }    

    delete(id: number)
    {
        let uri = this._baseUri + "polyclinic/" + id;
        return this._auth.AuthDelete(uri)
            //.map((res: Response) => {
                //return res.json();
            .catch((error:any) => Observable.throw(error.json().error || 'Server error'));
            
    }

    import(files: any){
        let urlImport = this._baseUri + "polyclinic/importdata" + files;
        let _uploader:FileUploader = new FileUploader({url: urlImport});          
        
       // _uploader.addToQueue(files);
    }

}