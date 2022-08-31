import { NgModule }                 from '@angular/core';
import { Routes,
         RouterModule }             from '@angular/router';

import { ListKlinikComponent }      from './list-klinik.component';

const routes: Routes = [
    {
        path: '',
        component: ListKlinikComponent,
        data: {
            title: 'Daftar Klinik'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ListKlinikRoutingModule {}
