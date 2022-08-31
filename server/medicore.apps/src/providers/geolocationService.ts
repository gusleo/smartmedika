import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';

//Grab everything with import model;
import { CountryModel, RegencyModel, RegionModel } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class GeoLocationService {
    private _baseUri: string;
    constructor(protected http: Http) {
        this._baseUri = new ConfigService().getApiUri() + "geolocation";
    }

    getCountryList(): Observable<Array<CountryModel>> {
        let uri = this._baseUri;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let items: Array<CountryModel> = plainToClass(CountryModel, object);
                return items;
            });
    }

    getRegencyList(regionId: number): Observable<Array<RegencyModel>> {
        let uri = this._baseUri + "/" + regionId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let items: Array<RegencyModel> = plainToClass(RegencyModel, object);
                return items;
            });
    }

    getRegionList(countryId: number): Observable<Array<RegionModel>> {
        let uri = this._baseUri + "/" + countryId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let items: Array<RegionModel> = plainToClass(RegionModel, object);
                return items;
            });
    }

    getRegionWithRegencyList(countryId: number): Observable<Array<RegionModel>> {
        let uri = this._baseUri + "/" + countryId;
        return this.http.get(uri)
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let items: Array<RegionModel> = plainToClass(RegionModel, object);
                return items;
            });
    }
}