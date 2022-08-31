import { NgModule }                 from '@angular/core';
import { Routes,
         RouterModule }             from '@angular/router';

import { BookingDoctorComponent }      from './booking-doctor.component';

const routes: Routes = [
    {
        path: '',
        component: BookingDoctorComponent,
        data: {
            title: 'Register Pasien'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BookingDoctorRoutingModule {}
