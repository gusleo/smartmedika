import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StringHelper, Base64Helper } from '../libs';

import { ServiceConfiguration } from '../app.config';

import { PhotoModel, HospitalImage,
    HospitalModel, PaginationEntity, HospitalStatus } from '../model';


@Injectable()
export class ClinicService {
    private _baseUri: string;
    private headers: Headers;
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "hospital";
    }
    
    getHospitalBySearching(page: number, itemPerpage: number, regionId: number = 0, clue: string = ""): Observable<PaginationEntity<HospitalModel>>
    {       
        let uri = StringHelper.concat("/", this._baseUri, page, itemPerpage, regionId, clue);
        return this.http.get<PaginationEntity<HospitalModel>>(uri);
    }

    hospitalDelete(id: number): Observable<HospitalModel>
    {        
        let uri = StringHelper.concat("/", this._baseUri,id);
        return this.http.delete<HospitalModel>(uri);
            
    }

    save (param:HospitalModel):Observable<HospitalModel>{
        let uri = this._baseUri;
        if(param.id == 0){
            return this.http.post<HospitalModel>(uri, param);
        }else{
            uri = uri + '/' + param.id;        
            return this.http.put<HospitalModel>(uri, param);
        }
    }
    
    getDetail(id:number):Observable<HospitalModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.get<HospitalModel>(uri);
    }

    changeStatus(id:number, status:HospitalStatus):Observable<HospitalModel>{
        let uri = StringHelper.concat("/", this._baseUri, "status", id);
        return this.http.put<HospitalModel>(uri, status);
    }
    CoverImage(hospitalId:number, filename:string, base64:string):Observable<HospitalImage>{
        var blob = Base64Helper.toBlob(base64);
        var formData = new FormData();       
        formData.append('file', blob, filename);
       
        let uri = StringHelper.concat("/", this._baseUri, "CoverImage", hospitalId);
        return this.http.put<HospitalImage>(uri, formData);
            
    }
    /*getPhotos(photos: any): Observable<PhotoModel>
    {
        
        return ;
    }*/

    getHospitalAsscoiateUser(): Observable<HospitalModel> {
        var uri = StringHelper.concat("/", this._baseUri, "associateduser");
        return this.http.get<HospitalModel>(uri);
    }
}