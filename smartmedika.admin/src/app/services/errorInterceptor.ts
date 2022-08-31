import 'rxjs/add/operator/mergeMap';
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs/observable';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router/src/router';


@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router:Router){
      
  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      
      return next.handle(req).catch((err, caught) =>{
        if(err instanceof HttpErrorResponse){
            if(err.status == 401){
                this.router.navigate(['session/unauthorized']);
            }
        }
        return Observable.throw(err);
      })
  }
}