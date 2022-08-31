import { NgModule }                                     from '@angular/core';
//import { CommonModule }                                 from '@angular/common';
//google map module
import { AgmCoreModule }                                from 'angular2-google-maps/core';

//shared module for component, directives
import { SharedModule } from '../../shared/shared.module';

//ng2bootstrap module
import { ModalModule } from 'ng2-bootstrap/modal';
import { TabsModule } from 'ng2-bootstrap/tabs';
import { TimepickerModule }  from 'ng2-bootstrap/timepicker';
import { SelectModule } from 'ng2-select';

//Routing
import { RegisterClinicRoutingModule } from './register-clinic-routing.module';

import { RegisterClinicComponent } from './register-clinic.component';
import { ClinicService, PolyClinicService, UploadImageService,
         RegionService}                                         from '../../services';

@NgModule({
  imports: [
    RegisterClinicRoutingModule,
    SharedModule,
    TabsModule,
    TimepickerModule.forRoot(),
    ModalModule.forRoot(),
    AgmCoreModule.forRoot({
        libraries: ["places"],
        apiKey: 'AIzaSyDevzr0WmMftGbTxYKNjHGsr5Rq9xkV--0'
    }),
    SelectModule
  ],
  declarations: [
    RegisterClinicComponent
  ],
  providers:[
    ClinicService,
    PolyClinicService,
    UploadImageService,
    RegionService
  ]
})
export class RegisterClinicModule { }
