import { NgModule }                     from '@angular/core';
import { CommonModule }                 from '@angular/common';


//Routing
import { BookingDoctorRoutingModule }  from './booking-doctor-routing.module';

import { BookingDoctorComponent }      from './booking-doctor.component';

@NgModule({
    imports: [
        BookingDoctorRoutingModule
    ],
    declarations: [
        BookingDoctorComponent
    ]
})
export class BookingDoctortModule { }