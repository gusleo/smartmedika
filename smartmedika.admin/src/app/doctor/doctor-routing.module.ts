import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListDoctorComponent } from './list-doctor/list-doctor.component';
import { RegisterDoctorComponent } from './register-doctor/register-doctor.component';
import { ScheduleDoctorComponent } from './schedule/schedule.component';
import { ListRegisterDoctorComponent } from './list-register-doctor/list-register-doctor.component';
import { ListDoctorAdminComponent } from './list-doctor-admin/list-doctor-admin.component';

const routes: Routes = [
  {
    path: '',
    children: [{
      path: 'list',
      component: ListDoctorComponent
    }, {
      path: 'register',
      component: RegisterDoctorComponent      
    }, {
      path: 'register/:id',
      component: RegisterDoctorComponent
    }, {
      path: 'schedule/:id/:hospitalId',
      component: ScheduleDoctorComponent
    }, {
      path: 'list-register',
      component: ListRegisterDoctorComponent      
    }, {
      path: 'list-doctor-admin',
      component: ListDoctorAdminComponent      
    }     
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoctorRoutingModule { }
