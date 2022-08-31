import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule, MdNativeDateModule, DateAdapter, MD_DATE_FORMATS } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { AppointmentRoutingModule } from './appointment-routing.module';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { IndonesiaDateAdapter, IDN_DATE_FORMATS} from '../../libs';

import { StaffService, GeoLocationService, AppoitmentService } from '../services';
import { ListAppointmentComponent, RegisterAppointmentComponent, FormAppointmentDialog,
  NextAppointmentDialog, TableAppointmentComponent } from '../appointment';

@NgModule({
  imports: [
    CommonModule,
    AppointmentRoutingModule,
    MaterialModule,
    MdNativeDateModule, 
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    NgxDatatableModule
  ],
  declarations: [RegisterAppointmentComponent, ListAppointmentComponent, FormAppointmentDialog, NextAppointmentDialog, TableAppointmentComponent],
  providers: [StaffService, AppoitmentService, DatePipe,     
    {provide: DateAdapter, useClass: IndonesiaDateAdapter},
    {provide: MD_DATE_FORMATS, useValue: IDN_DATE_FORMATS}
  ],
  entryComponents: [FormAppointmentDialog, NextAppointmentDialog]
})
export class AppointmentModule { }
