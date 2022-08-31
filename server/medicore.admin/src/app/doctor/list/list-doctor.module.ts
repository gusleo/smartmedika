import { NgModule }                     from '@angular/core';
import { CommonModule }                 from '@angular/common';


//Routing
import { ListDoctorRoutingModule }  from './list-doctor-routing.module';

import { ListDoctorComponent }      from './list-doctor.component';

import {ClinicService, SpesialisService} from '../../services';

@NgModule({
    imports: [
        ListDoctorRoutingModule
    ],
    declarations: [
        ListDoctorComponent
    ],
    providers:[
        ClinicService,
        SpesialisService
    ]
})
export class ListDoctorModule { }