import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { AuthService } from './auth.service';

@Injectable()
export class AuthGuardService implements CanActivate {

    constructor(private authService: AuthService, private router: Router) { }
    canActivate() {       
        return this.authService.isLogin().then((isLogin) =>{
            if(isLogin)
                return true;
                
            this.router.navigate(['session/unauthorized']);
            return false;
        })
        

    }

}
