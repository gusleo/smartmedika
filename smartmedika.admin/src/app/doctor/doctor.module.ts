import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { DoctorRoutingModule } from './doctor-routing.module';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { Message } from '../../libs';
import { OperatingHoursDialog } from './popup/operatinghours.dialog';
import { ListDoctorComponent } from './list-doctor/list-doctor.component';
import { RegisterDoctorComponent } from './register-doctor/register-doctor.component';
import { ScheduleDoctorComponent } from './schedule/schedule.component';
import { ListRegisterDoctorComponent } from './list-register-doctor/list-register-doctor.component';
import { ListDoctorAdminComponent } from './list-doctor-admin/list-doctor-admin.component';
import { StaffService, GeoLocationService, SpecialistService } from '../services';

@NgModule({
  imports: [
    CommonModule,
    DoctorRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    NgxDatatableModule
  ],
  declarations: [ListDoctorComponent, RegisterDoctorComponent, ListRegisterDoctorComponent, ListDoctorAdminComponent, ScheduleDoctorComponent, OperatingHoursDialog ],
  providers: [StaffService, GeoLocationService, SpecialistService, Message],
  entryComponents: [ OperatingHoursDialog]
})
export class DoctorModule { }
