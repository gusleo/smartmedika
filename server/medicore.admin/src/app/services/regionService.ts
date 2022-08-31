import { Injectable }                   from '@angular/core';
import { Http, Response, Headers }      from '@angular/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';
import { Observer }                     from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

//config API & Grab everything with import model;
import { ConfigService }                from '../services/configService';
import { AuthService }                  from '../auth';
import { RegionModel, PaginationEntity, 
         RegencyModel }                 from '../model';
import { plainToClass }                 from 'class-transformer';

@Injectable()
export class RegionService {
    private _baseUri: string;
    private _regionWithRegency: string = "geoLocation/regionwithregency";
    private _region: string = "geoLocation/region";

    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri();
    }

    getRegionWithRegency(): Observable<Array<RegionModel>>
    {
        let defaultValue = 1;
        let uri = this._baseUri + "/" + this._regionWithRegency + "/"+ defaultValue;
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();   
                    });     
    }

    getRegion(): Observable<Array<RegionModel>>
    {
        let defaultValue = 1;
        let uri = this._baseUri + "/" + this._region + "/"+ defaultValue;
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();   
                    });     
    }

    getRegencyByClue(): Observable<Array<RegencyModel>>
    {
        let clue = 'a';
        let defaultValue = 1;        
        let uri = this._baseUri + "/geoLocation/regency/" + defaultValue + "/" + clue;
            return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();   
                    });     
    }    
}