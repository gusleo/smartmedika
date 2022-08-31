import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {

    _apiUri: string;

    constructor() {
        this._apiUri = 'https://smartmedika.herokuapp.com/';
    }

    getApiUri() {
        return this._apiUri;
    }
}