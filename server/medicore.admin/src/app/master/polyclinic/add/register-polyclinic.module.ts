import { NgModule }                                     from '@angular/core';
import { CommonModule }                                 from '@angular/common';

//shared module for component, directives
import { SharedModule }                                 from '../../../shared/shared.module';

// Notifications
import { ToasterModule, ToasterService}                 from 'angular2-toaster/angular2-toaster';

//Routing
import { RegisterPolyClinicRoutingModule }              from './register-polyclinic-routing.module';

import { RegisterPolyClinicComponent }                  from './register-polyclinic.component';
import { ControlMessagesComponent }                     from '../../../shared/control-messages.component';
import { ConfigService, PolyClinicService, ValidationService }  from '../../../services';

@NgModule({
  imports: [
    CommonModule,
    RegisterPolyClinicRoutingModule,
    SharedModule,
    ToasterModule
  ],
  declarations: [
    RegisterPolyClinicComponent,
    ControlMessagesComponent
  ],
  providers:[
    ConfigService,
    PolyClinicService,
    ValidationService    
  ]
})
export class RegisterPolyClinicModule { }
