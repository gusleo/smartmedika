import { NgModule }                     from '@angular/core';
import { CommonModule }                 from '@angular/common';
import { AuthService } from '../auth';


//Routing
import { UnauthorizedRoutingModule }  from './unauthorized-routing.module';

import { UnauthorizedComponent }      from './unauthorized.component';

@NgModule({
    imports: [
        UnauthorizedRoutingModule
    ],
    providers: [AuthService],
    declarations: [
        UnauthorizedComponent
    ]
})
export class UnauthorizedModule { }