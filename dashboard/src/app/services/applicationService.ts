import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


//config API & Grab everything with import model;
import { ServiceConfiguration } from "../app.config";
import { StringHelper } from '../libs/stringHelper';
import { MenuType, MenuItemModel, TreeMenuModel, PaginationEntity } from "../model";



@Injectable()
export class ApplicationService {
    private _baseUri: string;
    private _menuSlug:string = "Menu";

    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "application";
    }

    getMenuByType(type:MenuType): Observable<Array<MenuItemModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this._menuSlug, "Tree", type); 
        return this.http.get<Array<MenuItemModel>>(uri);
               
    }
    getParentMenuByType(type:MenuType): Observable<Array<TreeMenuModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this._menuSlug, "Parent", type); 
        return this.http.get<Array<TreeMenuModel>>(uri);
               
    }
    getMenuByTypePaged(type:MenuType, pageIndex:number, pageSize:number):Observable<PaginationEntity<TreeMenuModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this._menuSlug, type, pageIndex, pageSize); 
        return this.http.get<PaginationEntity<TreeMenuModel>>(uri);
    }

    saveMenu(param: TreeMenuModel):Observable<TreeMenuModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._menuSlug); 
        if(param.id == 0){
            return this.http.post<TreeMenuModel>(uri, param);
        }else{
            uri = uri + '/' + param.id;
            return this.http.put<TreeMenuModel>(uri, param);
        }
    }
    deleteMenu(id:number):Observable<TreeMenuModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._menuSlug, id); 
        return this.http.delete<TreeMenuModel>(uri);
    }

    
    
}