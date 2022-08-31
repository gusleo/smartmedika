import { Injectable }                   from '@angular/core';
import { HttpClient }      from '@angular/common/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';

//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { RegionModel, PaginationEntity, 
         RegencyModel, CountryModel, Status, UTCTimeBaseModel }   from '../model';
import { StringHelper } from '../../libs';

@Injectable()
export class GeoLocationService {
    private readonly _regionWithRegency: string = "regionwithregency";
    private readonly _region: string = "region";
    private readonly _regency: string = "regency";
    private readonly _country:string = "country";

    private _baseUri: string;     
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "geolocation";
    }

    getAllRegion(includeInActive:boolean, page:number, itemPerpage:number, search:string, countryId?:number,):Observable<PaginationEntity<RegionModel>>
    {
       
        countryId = this.getCountryId(countryId);
        let uri = StringHelper.concat("/", this._baseUri, this._region, countryId, includeInActive, page, itemPerpage, search);
        return this.http.get<PaginationEntity<RegionModel>>(uri);
    }

    changeRegionStatus(id:number, status:Status):Observable<RegionModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._region, "status", id);
        return this.http.put<RegionModel>(uri, status);
    }

    addRegion(model:RegionModel):Observable<RegionModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._region);
        return this.http.post<RegionModel>(uri, model);
    }

    editRegion(id:number, model:RegionModel):Observable<RegionModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._region, id);
        return this.http.put<RegionModel>(uri, model);
    }

    deleteRegion(id:number):Observable<RegionModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._region, id);
        return this.http.delete<RegionModel>(uri);
    }
    getRegionWithRegency(countryId?:number): Observable<Array<RegionModel>>
    {
        countryId = this.getCountryId();
        let uri = StringHelper.concat("/", this._baseUri, this._regionWithRegency, countryId);
        return this.http.get<Array<RegionModel>>(uri);
    }

    getRegion(countryId?:number): Observable<Array<RegionModel>>
    {
        countryId = this.getCountryId();
        let uri = StringHelper.concat("/", this._baseUri, this._region, countryId);
        return this.http.get<Array<RegionModel>>(uri);
    }


    getRegencyByClue(page:number, itemPerPage:number, includeInActive:boolean, clue:string, countryId?:number): Observable<PaginationEntity<RegencyModel>>
    {       
        countryId = this.getCountryId(countryId);
        let uri = StringHelper.concat("/", this._baseUri, this._regency, countryId, includeInActive, page, itemPerPage, clue);
            return this.http.get<PaginationEntity<RegencyModel>>(uri);
    } 
s
    private getCountryId(countryId?:number){
         countryId = countryId == null ? 1 : countryId;
         if(countryId == 0)
            countryId = 1;
         return countryId;
    }

    /**
     * Get all country, include active/inactive
     * @param page current page index
     * @param itemPerpage page width
     */
    getAllCountry(page:number, itemPerpage:number):Observable<PaginationEntity<CountryModel>>
    {
        let uri = StringHelper.concat("/", this._baseUri, this._country, "all", page, itemPerpage);
        return this.http.get<PaginationEntity<CountryModel>>(uri);
    }  

    /**
     * Get active country,
     * used for show data as dropdown
     * @param page current page index
     * @param itemPerpage page width
     */
    getActiveCountry(page:number, itemPerpage:number = 1000 ):Observable<PaginationEntity<CountryModel>>
    {
        let uri = StringHelper.concat("/", this._baseUri, this._country, "Active", page, itemPerpage);
        return this.http.get<PaginationEntity<CountryModel>>(uri);
    } 

    addCountry(model:CountryModel):Observable<CountryModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._country);
        return this.http.post<CountryModel>(uri, model);
    }
    editCountry(id:number, model:CountryModel):Observable<CountryModel>{
       let uri = StringHelper.concat("/", this._baseUri, this._country, id);
        return this.http.put<CountryModel>(uri, model);
    }
    deleteCountry(id:number):Observable<CountryModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._country, id);
        return this.http.delete<CountryModel>(uri);
    }
    changeCountryStatus(id:number, status:Status):Observable<CountryModel>{
       let uri = StringHelper.concat("/", this._baseUri, this._country, "status", id);
        return this.http.put<CountryModel>(uri, status);
    }

    getAllUtcByCountry(countryId:number, includeInActive:boolean, page:number, pageSize:number):Observable<PaginationEntity<UTCTimeBaseModel>>{
        let uri = StringHelper.concat("/", this._baseUri, "utc", countryId, includeInActive, page, pageSize);
        return this.http.get<PaginationEntity<UTCTimeBaseModel>>(uri);
    }

    changeRegencyStatus(id:number, status:Status):Observable<RegencyModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._regency, "status", id);
        return this.http.put<RegencyModel>(uri, status);
    }
    addRegency(model:RegencyModel):Observable<RegencyModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._regency);
        return this.http.post<RegencyModel>(uri, model);
    }

    editRegency(id:number, model:RegencyModel):Observable<RegencyModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._regency, id);
        return this.http.put<RegencyModel>(uri, model);
    }

    deleteRegency(id:number):Observable<RegencyModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._regency, id);
        return this.http.delete<RegencyModel>(uri);
    }
}