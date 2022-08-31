import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

//Layouts
import { FullLayoutComponent } from './layouts/full-layout.component';
import { SimpleLayoutComponent } from './layouts/simple-layout.component';
import { UnauthorizedComponent } from './unauthorized';
import { AuthGuardService } from './auth';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'dashboard',
    pathMatch: 'full',
  }, 
  {
    path: 'unauthorized',
    component: UnauthorizedComponent
  }, 
  
  {
    path: '',
    component: FullLayoutComponent,    
    canActivate: [AuthGuardService],
    data: {
      title: 'Home'
    },
    children: [
       
        {
            path: 'dashboard',
            loadChildren: './dashboard/dashboard.module#DashboardModule'
        },
        {
            path: 'clinic',
            loadChildren: './clinic/list/list-klinik.module#ListKlinikModule'
        },
        {
            path: 'clinic/register-clinic/:id',
            loadChildren: './clinic/add/register-clinic.module#RegisterClinicModule'
        },
        {
            path: 'doctor',
            loadChildren: './doctor/list/list-doctor.module#ListDoctorModule'
        },
        {
            path: 'doctor/register-doctor',
            loadChildren: './doctor/add/register-doctor.module#RegisterDoctorModule'
        },
        {
            path: 'booking-doctor',
            loadChildren: './booking/doctor/booking-doctor.module#BookingDoctortModule'
        },
        {
            path: 'booking-doctor/:id',
            loadChildren: './booking/form/form-patient.module#FormPatientModule'
        },
        {
            path: 'master/polyclinic',
            loadChildren: './master/polyclinic/list/list-polyclinic.module#ListPolyClinicModule'
        },
        {
            path: 'master/polyclinic/register-polyclinic/:id',
            loadChildren: './master/polyclinic/add/register-polyclinic.module#RegisterPolyClinicModule'
        },
        {
            path: 'master/spesialis',
            loadChildren: './master/spesialis/list/list-spesialis.module#ListSpesialisModule'
        },
        {
            path: 'master/spesialis/register-spesialis/:id',
            loadChildren: './master/spesialis/add/register-spesialis.module#RegisterSpesialisModule'
        }        
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
