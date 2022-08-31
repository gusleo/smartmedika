import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from './configService';
import { AuthService } from '../providers/authService';

//Grab everything with import model;
import { ArticleModel, PaginationEntity } from '../model';
//Json Seriliaze
import { plainToClass } from 'class-transformer';

@Injectable()
export class ArticleService {
    private _baseUri: string;
    constructor(protected http: Http, private auth: AuthService) {
        this._baseUri = new ConfigService().getApiUri() + "article";
    }

    getArticleList(pageIndex: number, pageSize: number): Observable<PaginationEntity<ArticleModel>> {
        let uri = this._baseUri + "/" + pageIndex + "/" + pageSize;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let pagination: PaginationEntity<ArticleModel> = new
                    PaginationEntity<ArticleModel>(object.page, object.pageSize, object.totalPages,
                    object.totalCount, object.count);
                pagination.items = plainToClass(ArticleModel, object.items);
                return pagination;
            });
    }

    getArticleDetail(articleId: number): Observable<ArticleModel> {
        let uri = this._baseUri + "/" + articleId;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            }).map((object: any) => {
                let item: ArticleModel = plainToClass(ArticleModel, object as Object);
                return item;
            });
    }

    getArticleStaffById(id: number, pageIndex: number, pageSize: number): Observable<PaginationEntity<ArticleModel>> {
        let uri = this._baseUri + "/" + "staff" +"/" +id+ "/" + pageIndex + "/" + pageSize;
        let headers = this.auth.getAuthToken();
        return this.http.get(uri, { headers: headers })
            .map((responseData: Response) => {
                return responseData.json();
            })
            .map((object: any) => {
                let pagination: PaginationEntity<ArticleModel> = new
                    PaginationEntity<ArticleModel>(object.page, object.pageSize, object.totalPages,
                    object.totalCount, object.count);
                pagination.items = plainToClass(ArticleModel, object.items);
                return pagination;
            });
    }
}