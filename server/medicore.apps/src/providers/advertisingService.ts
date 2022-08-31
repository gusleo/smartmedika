import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';

//Grab everything with import model;
import { AdvertisingModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class AdvertisingService{
    private _baseUri: string;
    constructor(protected http: Http) {
        this._baseUri = new ConfigService().getApiUri() + "advertising/slider";
    }  

    getAllAdvertisement(): Observable<Array<AdvertisingModel>> {
        let uri = this._baseUri;
        return this.http.get(uri)
                .map((responseData:Response) => {
                  return responseData.json();  
                }).map((object:any) => {
                    let items: Array<AdvertisingModel> = plainToClass(AdvertisingModel, object);
                    return items;
                });  
    }
    
}