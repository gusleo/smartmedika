import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
//Grab everything with import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { FileUploader } from 'ng2-file-upload';
//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { BlogCategoryModel, BlogModel, ArticleStatus, PaginationEntity } from '../model';
import { StringHelper, Base64Helper } from '../../libs';

@Injectable()
export class BlogService{
    
    private _baseUri: string;
    private _categorySlug: string = "category";
    
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() +"article"
    }

    getBlogCategory(page:number, itemPerpage:number):Observable<PaginationEntity<BlogCategoryModel>>{
        let uri = StringHelper.concat("/", this._baseUri, this._categorySlug, page, itemPerpage);
        return this.http.get<PaginationEntity<BlogCategoryModel>>(uri);
    }

    getCategoryDetail(id:number):Observable<BlogCategoryModel>{
        let uri = StringHelper.concat("/", this._baseUri, this._categorySlug, id);
        return this.http.get<BlogCategoryModel>(uri);
    }

    categoryDelete(id: number): Observable<BlogCategoryModel>
    {        
        let uri = StringHelper.concat("/", this._baseUri, this._categorySlug, id);
        return this.http.delete<BlogCategoryModel>(uri);
            
    }

    categorySave (param:BlogCategoryModel):Observable<BlogCategoryModel>{
        if(param.slug == null){
            param.slug = param.name.replace(/ /g, "-").toLowerCase();
        }
        let uri = StringHelper.concat("/", this._baseUri, this._categorySlug);
        if(param.id == 0){
            return this.http.post<BlogCategoryModel>(uri, param);
        }else{
            uri = StringHelper.concat("/", uri, param.id);        
            return this.http.put<BlogCategoryModel>(uri, param);
        }
    }

    getBlog(page:number, itemPerpage:number):Observable<PaginationEntity<BlogModel>>{
        let uri = StringHelper.concat("/", this._baseUri, page, itemPerpage);
        return this.http.get<PaginationEntity<BlogModel>>(uri);
    }

    getBlogDetail(id:number):Observable<BlogModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.get<BlogModel>(uri);
    }

    blogDelete(id: number): Observable<BlogModel>
    {        
        let uri = StringHelper.concat("/", this._baseUri, "status", id);
        return this.http.put<BlogModel>(uri, ArticleStatus.Archive);
            
    }
    blogPublish(id: number, isPublish:boolean): Observable<BlogModel>
    {        
        let uri = StringHelper.concat("/", this._baseUri, "status", id);
        return this.http.put<BlogModel>(uri, isPublish ? ArticleStatus.Confirmed : ArticleStatus.UnConfirmed);
            
    }

    blogSave (param:BlogModel):Observable<BlogModel>{
        if(param.slug == null){
            param.slug = param.title.replace(/ /g, "-").toLowerCase();
        }
        let uri = StringHelper.concat("/", this._baseUri);
        if(param.id == 0){
            return this.http.post<BlogModel>(uri, param);
        }else{
            uri = StringHelper.concat("/", uri, param.id);        
            return this.http.put<BlogModel>(uri, param);
        }
    }

}