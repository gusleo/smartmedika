import { NgModule }         from '@angular/core';
import { Routes,
         RouterModule }     from '@angular/router';

import { RegisterSpesialisComponent }  from './register-spesialis.component';

const routes: Routes = [
  {
    path: '',
    component: RegisterSpesialisComponent,
    data: {
      title: 'Registrasi Spesialis'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegisterSpesialisRoutingModule {}