import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TableAppointmentComponent, RegisterAppointmentComponent, ListAppointmentComponent } from '../appointment';

const routes: Routes = [
  {
    path: '',
    children: [{
      path: 'register',
      component: RegisterAppointmentComponent
    }, {
      path: 'schedules/:id',
      component: TableAppointmentComponent
    }, {
      path: 'list',
      component: ListAppointmentComponent
    }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentRoutingModule { }
