import {Injectable} from '@angular/core';
import {Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import * as Constant from '../util/constants';

@Injectable()
export class HttpService extends Http {

  constructor (backend: XHRBackend, options: RequestOptions) {
    // let token = localStorage.getItem(Constant.ACCESS_TOKEN);; // your custom token getter function here
    // options.headers.set(Constant.AUTHORIZATION, `Bearer ${token}`);
    options.headers.set(Constant.ACCEPT, Constant.APPLICATION_JSON);
    options.headers.set(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_JSON);

    super(backend, options);
  }

  request(url: string|Request, options?: RequestOptionsArgs): Observable<Response> {
    // let token = localStorage.getItem('auth_token');
    if (typeof url === 'string') { // meaning we have to add the token to the options, not in url
      if (!options) {
        // let's make option object
        options = {headers: new Headers()};
      }
    //   options.headers.set('Authorization', `Bearer ${token}`);
        options.headers.set(Constant.ACCEPT, Constant.APPLICATION_JSON);
        options.headers.set(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_JSON);
    } else if (url.url === Constant.BACKEND_URL_TOKEN) {
        // options = {headers: new Headers()};
        // options.headers.set(Constant.ACCEPT, Constant.APPLICATION_JSON);
        // options.headers.set(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_URLENCODED);
        // options.headers.set(Constant.AUTHORIZATION, Constant.AUTHORIZATION_BASIC);
    } else {
    // we have to add the token to the url object
    //   url.headers.set('Authorization', `Bearer ${token}`);
      url.headers.set(Constant.ACCEPT, Constant.APPLICATION_JSON);
      url.headers.set(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_JSON);
    }
    return super.request(url, options).catch(this.catchAuthError(this));
  }

  private catchAuthError (self: HttpService) {
    // we have to pass HttpService's own instance here as `self`
    return (res: Response) => {
      console.log(res);
      if (res.status === 401 || res.status === 403) {
        // if not authenticated
        console.log(res);
      }
      return Observable.throw(res);
    };
  }
}