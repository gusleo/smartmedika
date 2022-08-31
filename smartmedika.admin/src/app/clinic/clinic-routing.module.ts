import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListClinicComponent } from './list-clinic/list-clinic.component';
import { RegisterClinicComponent } from './register-clinic/register-clinic.component';

const routes: Routes = [
  {
    path: '',
    children: [{
      path: 'list',
      component: ListClinicComponent
    },{
      path: 'register',
      component: RegisterClinicComponent      
    }
    ,{
      path: 'register/:id',
      component: RegisterClinicComponent      
    }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClinicRoutingModule { }
