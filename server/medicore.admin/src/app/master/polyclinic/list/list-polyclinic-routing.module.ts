import { NgModule }         from '@angular/core';
import { Routes,
         RouterModule }     from '@angular/router';

import { ListPolyClinicComponent }  from './list-polyclinic-component';

const routes: Routes = [
  {
    path: '',
    component: ListPolyClinicComponent,
    data: {
      title: 'Poliklinik'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ListPolyClinicRoutingModule {}