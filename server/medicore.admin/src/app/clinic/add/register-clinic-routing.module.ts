import { NgModule }         from '@angular/core';
import { Routes,
         RouterModule }     from '@angular/router';

import { RegisterClinicComponent }  from './register-clinic.component';

const routes: Routes = [
  {
    path: '',
    component: RegisterClinicComponent,
    data: {
      title: 'Profile'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegisterClinicRoutingModule {}