import { Injectable }                   from '@angular/core';
import { Http, Response, Headers }      from '@angular/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';
import { Observer }                     from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

//config API & Grab everything with import model;
import { ConfigService }    from './configService';
import { AuthService }      from '../auth'


@Injectable()
export class IdentityService {
    private _baseUri: string;


    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();        
    }


    getUserId(): Observable<number>
    {
        
        let uri = this._baseUri + 'identity';
        return this._auth.AuthGet(uri).map((responseData: Response) =>{
            return responseData.json();
        });
        
    }

    
}