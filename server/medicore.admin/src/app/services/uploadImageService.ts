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
import { plainToClass }                     from 'class-transformer';

@Injectable()
export class UploadImageService {

    private _baseUri: string;
    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();
    }

    upload(image: any, galeri1: any, galeri2: any, galeri3: any)
    {
        let body = [];
        body = [image, galeri1, galeri2, galeri3];
        let uri = this._baseUri + "file/images";
            return this._auth.AuthPost(uri, body)
                        .catch((error:any) => Observable.throw(error.json().error || 'Server error'));        
    }
}