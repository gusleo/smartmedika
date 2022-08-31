import { NgModule }                 from '@angular/core';
import { Routes,
         RouterModule }             from '@angular/router';

import { RegisterDoctorComponent }      from './register-doctor.component';

const routes: Routes = [
    {
        path: '',
        component: RegisterDoctorComponent,
        data: {
            title: 'Register Dokter'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RegisterDoctorRoutingModule {}
