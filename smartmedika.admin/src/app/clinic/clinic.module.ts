import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ClinicRoutingModule } from './clinic-routing.module';
import { ListClinicComponent } from './list-clinic/list-clinic.component';
import { RegisterClinicComponent } from './register-clinic/register-clinic.component';
import { OperatingHoursDialog } from './popup';
import { AgmCoreModule } from '@agm/core';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { GooglePlacesService, Message } from '../../libs';
import { PolyClinicService, ClinicService, GeoLocationService } from '../services';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    ClinicRoutingModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    AgmCoreModule,
    NgxDatatableModule,
    SharedModule,
    
  ],
  declarations: [ListClinicComponent, RegisterClinicComponent, OperatingHoursDialog],
  providers:[GooglePlacesService, PolyClinicService, ClinicService, GeoLocationService, Message],
  entryComponents: [ OperatingHoursDialog ]
})
export class ClinicModule { }
