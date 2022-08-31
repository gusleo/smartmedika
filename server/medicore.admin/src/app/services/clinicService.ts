import { Injectable }                   from '@angular/core';
import { Http, Response, Headers, RequestOptions }      from '@angular/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';
import { Observer }                     from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

//config API & Grab everything with import model;
import { ConfigService }                from '../services/configService';
import { AuthService }                  from '../auth';

import { PhotoModel, PhoneModel, 
    OperatingHourModel, HospitalModel, PaginationEntity }                from '../model';

import { plainToClass, classToPlain }                 from 'class-transformer';


@Injectable()
export class ClinicService {
    private _baseUri: string;
    private headers: Headers;
    constructor(protected http: Http, private _auth: AuthService){
        this._baseUri = new ConfigService().getApiUri() + "hospital";
    }
    
    getAllHospitalByPaging(page: number, itemPerpage: number): Observable<PaginationEntity<HospitalModel>>
    {
        
        let uri = this._baseUri + "/" + page + "/" + itemPerpage;
        return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();
                    }).map((object: any) => {
                        let pagination: PaginationEntity<HospitalModel> = new 
                        PaginationEntity<HospitalModel>(object.page, object.totalPages, 
                        object.totalCount, object.count); 
                        pagination.items = plainToClass(HospitalModel, object.items);
                        return pagination;
                    });
    }

    getHospitalBySearching(page: number, itemPerpage: number, regionId: number, clue: string): Observable<PaginationEntity<HospitalModel>>
    {
        let uri = this._baseUri + "/" + page + "/" + itemPerpage + "/" + regionId + "/" + clue  ;
        return this._auth.AuthGet(uri)
                    .map((responseData: Response) => {
                        return responseData.json();
                    }).map((object: any) => {
                        let pagination: PaginationEntity<HospitalModel> = new 
                        PaginationEntity<HospitalModel>(object.page, object.totalPages, 
                        object.totalCount, object.count); 
                        pagination.items = plainToClass(HospitalModel, object.items);
                        return pagination;
                    });        
    }

    hospitalDelete(id: number): Observable<HospitalModel[]>
    {
        let uri = this._baseUri + "/" + id;
        return this._auth.AuthDelete(uri)
            .map((res: Response) => {
                return res.json();
            });
            
    }
    create(obj: HospitalModel){
        let uri = this._baseUri;        
        return this._auth.AuthPost(uri, obj)
            .map((res:Response) => {
                return res.json();
            })
    }

    getDetail(id:number):Observable<HospitalModel>{
        let uri = this._baseUri + "/" + id;
        return this._auth.AuthGet(uri)
            .map((res: Response) =>{
                return res.json();
            })
            .map((obj: any) => {
                return plainToClass(HospitalModel, obj as Object);               
            });
    }
    getPhotos(photos: any): Observable<PhotoModel>
    {
        
        return ;
    }
}