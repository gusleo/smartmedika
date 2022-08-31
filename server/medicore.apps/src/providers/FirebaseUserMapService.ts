import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { ConfigService } from './configService';
import { Storage } from '@ionic/storage';
import { AuthService } from '../providers/authService';

//Grab everything with import model;
import { FirebaseUserMapModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class FirebaseUserMapService {
    private _baseUri: string;
    constructor(protected http: Http, public storage: Storage, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "firebase";
    }

    signupFirebase(params: FirebaseUserMapModel): Observable<FirebaseUserMapModel> {
        let uri = this._baseUri;
        let headers = this.auth.getAuthToken();
        let body = JSON.stringify(params);

        return this.http.post(uri, body, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let item = plainToClass(FirebaseUserMapModel, object as Object);
                return item; 
            });
    }
}