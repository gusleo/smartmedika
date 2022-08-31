import 'rxjs/add/operator/mergeMap';
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable, mergeMap } from 'rxjs';
import { AuthService} from './auth.service';
import { User } from 'oidc-client';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth:AuthService){
      
  }
  intercept (request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.auth.getCurrentUser()
        .pipe(
            mergeMap((user: User | null) => {
                if (user) {
                    // clone and modify the request
                    request = request.clone({headers: request.headers.set('Authorization', user.token_type + " " + user.access_token)})
                }
                if(request.body instanceof FormData == false){
                    if (!request.headers.has('Content-Type')) {
                        request = request.clone({ headers: request.headers.set('Content-Type', 'application/json') });
                    }
                }
                
                return next.handle(request);
            })
        )
        
    }
}