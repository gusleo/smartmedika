import { Injectable }                   from '@angular/core';
import { Http, Response, Headers, RequestOptions }      from '@angular/http';
//Grab everything with import 'rxjs/Rx';
import { Observable }                   from 'rxjs/Observable';
import { Observer }                     from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

//config API & Grab everything with import model;
import { ConfigService }                from '../services/configService';
import { PaginationEntity }             from '../model/paginationEntity';
import { plainToClass }                 from 'class-transformer';


@Injectable()
export class ApplicationService {
    private _baseUri: string;
    private headers: Headers;
    constructor(protected http: Http){
        this._baseUri = new ConfigService().getApiUri() + "hospital";
    }

    
    
}