import { Injectable, EventEmitter } from '@angular/core';
import { Location } from '@angular/common';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { Router } from '@angular/router';
import { UserManager, Log, MetadataService, User } from 'oidc-client';
import { environment } from '../../environments/environment';

@Injectable()
export class AuthService {
  mgr: UserManager = new UserManager(settings);
  userLoadededEvent: EventEmitter<User> = new EventEmitter<User>();
  currentUser:User;
  loggedIn: boolean = false;

  authHeaders: Headers;


  constructor(private http: Http, private router:Router, private location:Location) {
    this.mgr.getUser()
      .then((user) => {
        if (user) {
          this.loggedIn = true;
          this.currentUser = user;         
          this.userLoadededEvent.emit(user);
        }
        else {
          this.loggedIn = false;
        }
      })
      .catch((err) => {
        this.loggedIn = false;
      });
    this.mgr.events.addUserUnloaded((e) => {
      if (!environment.production) {
        console.log("user unloaded");
      }
      this.loggedIn = false;
    });
  }

  getCurrentUser():Observable<User>{
    return Observable.fromPromise(this.mgr.getUser())
  }
  clearState() {
    this.mgr.clearStaleState().then(function () {
      console.log("clearStateState success");
    }).catch(function (e) {
      console.log("clearStateState error", e.message);
    });
  }

  getUser() {
    this.mgr.getUser().then((user) => {
      console.log("got user", user);
      this.userLoadededEvent.emit(user);
    }).catch(function (err) {
      console.log(err);
    });
  }

  StafremoveUser() {
    this.mgr.removeUser().then(() => {
      this.userLoadededEvent.emit(null);
      console.log("user removed");
    }).catch(function (err) {
      console.log(err);
    });
  }

  startSigninMainWindow() {
    this.mgr.signinRedirect({ data: 'some data' }).then(function () {
      console.log("signinRedirect done");
    }).catch(function (err) {
      console.log(err);
    });
  }
  endSigninMainWindow() {
    this.mgr.signinRedirectCallback().then(function (user) {
      console.log("signed in", user);
    }).catch(function (err) {
      console.log(err);
    });
  }

  startSignoutMainWindow() {
    this.mgr.signoutRedirect().then(function (resp) {
      console.log("signed out", resp);
      setTimeout(5000, () => {
        console.log("testing to see if fired...");

      })
    }).catch(function (err) {
      console.log(err);
    });
  };

  endSignoutMainWindow() {
    this.mgr.signoutRedirectCallback().then(function (resp) {
      console.log("signed out", resp);
    }).catch(function (err) {
      console.log(err);
    });
  };
  isLogin():Promise<boolean> {
    return new Promise((resolve, reject)=>{
        this.mgr.getUser().then((user) =>{
          if(user){          
            this._setAuthHeaders(user);  
            resolve(true);
          }else{
            resolve(false);
          }
      }).catch((err) => {
          resolve(false);
      });
    });
     
  }
  
  /**
   * Example of how you can make auth request using angulars http methods.
   * @param options if options are not supplied the default content type is application/json
   */
  get(url: string, options?: RequestOptions): Observable<Response> {

    if (options) {
      options = this._setRequestOptions(options);
    }
    else {
      options = this._setRequestOptions();
    }
    return this.intercept(this.http.get(url, options));
  }
  /**
   * @param options if options are not supplied the default content type is application/json
   */
  put(url: string, data: any, options?: RequestOptions): Observable<Response> {

    let body;
    if(data instanceof FormData){
      body = data;
    }else{
     body = JSON.stringify(data);
    }

    if (options) {
      options = this._setRequestOptions(options);
    }
    else {
      options = this._setRequestOptions();
    }
    return this.intercept(this.http.put(url, body, options));
  }
  /**
   * @param options if options are not supplied the default content type is application/json
   */
  delete(url: string, options?: RequestOptions): Observable<Response> {

    if (options) {
      options = this._setRequestOptions(options);
    }
    else {
      options = this._setRequestOptions();
    }
    return this.intercept(this.http.delete(url, options));
  }
  /**
   * @param options if options are not supplied the default content type is application/json
   */
  post(url: string, data: any, options?: RequestOptions): Observable<Response> {

    let body;
    if(data instanceof FormData){
      body = data;
    }else{
     body = JSON.stringify(data);
    }

    if (options) {
      options = this._setRequestOptions(options);
    }
    else {
      options = this._setRequestOptions();
    }
    return this.intercept(this.http.post(url, body, options));
  }


  private _setAuthHeaders(user: any) {
    this.authHeaders = new Headers();
    this.authHeaders.append('Authorization', user.token_type + " " + user.access_token);
    this.authHeaders.append('Content-Type', 'application/json');
  }
  private _setRequestOptions(options?: RequestOptions) {
    
    if (options) {
      let token = this.authHeaders.get('Authorization');
      options.headers.append('Authorization', token);
    }
    else {
      options = new RequestOptions({ headers: this.authHeaders});
    }

    return options;
  }

  intercept(observable: Observable<Response>): Observable<Response> {
        return observable.catch((err, source) => {
            if (err.status  == 401 || err.status  == 403) {
               this.router.navigate(['session/unauthorized']);
                return Observable.empty();
            } else {
                return Observable.throw(err.json().error || 'Server error');
            }
        });
 
    }

}

/**
 * change authority url to 
 * (LIVE) http://auth.smartmedika.com 
 * (DEV) http://localhost:5000
 */

const settings: any = {
    //authority: 'http://localhost:5000',
    authority: 'http://52.87.109.107:5000',
    client_id: 'js',
    redirect_uri: 'http://localhost:9999/auth.html',
    // if we choose to use popup window instead for logins
    //popup_redirect_uri: window.location.origin + "/popup.html",
    //popupWindowFeatures: "menubar=yes,location=yes,toolbar=yes,width=1200,height=800,left=100,top=100;resizable=yes",
    post_logout_redirect_uri: 'http://localhost:9999/',
    response_type: 'id_token token',
    scope: 'openid profile medicore.full_access',   
    silent_redirect_uri: 'http://localhost:9999/silent.html',
    automaticSilentRenew: true,
    monitorSession : true,
    //revokeAccessTokenOnSignout: true,
    filterProtocolClaims: true,
    loadUserInfo: true/*,
    accessTokenExpiringNotificationTime: 1500*/
};
