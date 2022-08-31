import { NgModule }                 from '@angular/core';
import { Routes,
         RouterModule }             from '@angular/router';

import { ListDoctorComponent }      from './list-doctor.component';

const routes: Routes = [
    {
        path: '',
        component: ListDoctorComponent,
        data: {
            title: 'Daftar Dokter'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ListDoctorRoutingModule {}
