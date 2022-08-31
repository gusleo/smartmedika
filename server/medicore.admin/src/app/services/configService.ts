import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
    public OAuthServer: string = "http://auth.smartmedika.com/";
    public AngularUrl: string = "http://localhost:4200/";
    private _localApi = 'http://localhost:5001/admin/';
    private _liveApi = 'http://api.smartmedika.com/admin/';

    _apiUri: string;

    constructor() {
        this._apiUri = this._liveApi;
    }

    getApiUri() {
        return this._apiUri;
    }

}