import { NgModule }                 from '@angular/core';
import { Routes,
         RouterModule }             from '@angular/router';

import { FormPatientComponent }      from './form-patient.component';

const routes: Routes = [
    {
        path: '',
        component: FormPatientComponent,
        data: {
            title: 'Form Pasien'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FormPatientRoutingModule {}
