import { NgModule }         from '@angular/core';
import { Routes,
         RouterModule }     from '@angular/router';

import { RegisterPolyClinicComponent }  from './register-polyclinic.component';

const routes: Routes = [
  {
    path: '',
    component: RegisterPolyClinicComponent,
    data: {
      title: 'Registrasi Poliklinik'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegisterPolyClinicRoutingModule {}