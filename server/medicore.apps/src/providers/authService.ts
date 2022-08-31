import { Injectable } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { Storage } from '@ionic/storage';
import * as Constant from '../util/constants';
import { Commons } from '../util/commons';
import { UserData } from '../providers/user-data';

export class User {
    email: string;
    password: string;
    access_token: string;

    constructor(email: string, password: string, access_token: string) {
        this.email = email;
        this.password = password;
        this.access_token = access_token;
    }
}

@Injectable()
export class AuthService {
    currentUser: User;
    constructor(private http: Http, public storage: Storage, private common: Commons, public userData: UserData) {
    }

    public login(email: any, password: any) {
        return Observable.create((observer: Observer<any>) => {
            let headers = this.newAuthHeader();
            let body = "grant_type=password&username=" + email + "&scope=openid profile offline_access medicore.basic&password=" + password;

            this.http.post(Constant.BACKEND_URL_TOKEN, body, {headers: headers})
                .map(res => res.json())
                .subscribe(data => {
                    console.log('login data:', data);
                    if (data.access_token !== null) {
                        //get access token
                        let token = data.access_token;
                        let refreshToken = data.refresh_token;

                        // add userdata username - important
                        this.userData.login(email);

                        //simpan di local storage
                        this.common.setUserToken(token);
                        this.common.setUserRefreshToken(refreshToken);

                        observer.next(Constant.ALLOWED);
                        observer.complete();
                    } else {
                        observer.next(Constant.NOT_ALLOWED);
                        observer.complete();
                    }
                }, error => {
                    console.error(error);
                    observer.next(error.statusText);
                    observer.complete();
                })
        });
    }

    public silentRenewToken() {
        return Observable.create((observer: Observer<any>) => {
            let headers = this.newAuthHeader();
            let body = "grant_type=refresh_token&client_id=medicore&secret=FuckGoverment&refresh_token=" + this.common.refreshToken;

            this.http.post(Constant.BACKEND_URL_TOKEN, body, {headers: headers})
                .map(res => res.json())
                .subscribe(data => {
                    console.log(data);
                    if (data.access_token !== null) {
                        //get access token
                        let token = data.access_token;
                        let refreshToken = data.refresh_token;

                        //simpan di local storage
                        this.common.setUserToken(token);
                        this.common.setUserRefreshToken(refreshToken);

                        observer.next(Constant.ALLOWED);
                        observer.complete();
                    } else {
                        observer.next(Constant.NOT_ALLOWED);
                        observer.complete();
                    }
                }, error => {
                    console.error(error);
                    observer.next(error.statusText);
                    observer.complete();
                })
        });
    }

    private newAuthHeader() {
        let headers = new Headers();
        headers.append(Constant.ACCEPT, Constant.APPLICATION_JSON);
        headers.append(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_URLENCODED);
        headers.append(Constant.AUTHORIZATION, Constant.AUTHORIZATION_BASIC);

        return headers;
    }

    public register(credentials: any) {
        if (credentials.email === null || credentials.password === null) {
            return Observable.throw("Please insert credentials");
        } else {
            // At this point store the credentials to your backend!
            return Observable.create((observer: Observer<any>) => {
                observer.next(true);
                observer.complete();
            });
        }
    }

    public getUserInfo(): User {
        return this.currentUser;
    }

    public logout() {
        return Observable.create((observer: Observer<any>) => {
            this.currentUser = null;
            observer.next(true);
            observer.complete();
        });
    }

    // private readJwt(token: string): User {
    //     let tokens: Array<any> = token.split('.');
    //     let tokenPayload: any = JSON.parse(atob(tokens[1]));

    //     // let user:User = new User();
    //     // user.lastConnection = new Date();
    //     // user.id = parseInt(tokenPayload.iss);
    //     // user.email = tokenPayload.sub;
    //     // user.firstName = tokenPayload.firstName;
    //     // user.lastName = tokenPayload.lastName;
    //     // user.roles = tokenPayload.role;

    //     let user: User;

    //     return user;
    // }

    // public getHeaders() {
    //     let auth = "Bearer " + this.common.userToken;
    //     console.log('authService getHeader auth:', auth);
    //     let headers = new Headers();

    //     headers.append(Constant.ACCEPT, Constant.APPLICATION_JSON);
    //     headers.append(Constant.CONTENT_TYPE_HEADER, Constant.APPLICATION_JSON);
    //     headers.append(Constant.AUTHORIZATION, auth);

    //     return headers;
    // }

    public getAuthToken() {
        let auth = "Bearer " + this.common.userToken;
        console.log('authService getHeader auth:', auth);
        let headers = new Headers();

        headers.append(Constant.AUTHORIZATION, auth);

        return headers;
    }
}