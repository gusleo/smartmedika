import { NgModule }                     from '@angular/core';

//shared module for component, directives
import { SharedModule }                 from '../../shared/shared.module';

//Routing
import { RegisterDoctorRoutingModule }  from './register-doctor-routing.module';

import { RegisterDoctorComponent }      from './register-doctor.component';
import { SpesialisService }     from '../../services';

@NgModule({
    imports: [
        SharedModule,
        RegisterDoctorRoutingModule
    ],
    declarations: [
        RegisterDoctorComponent
    ]
})
export class RegisterDoctorModule { }