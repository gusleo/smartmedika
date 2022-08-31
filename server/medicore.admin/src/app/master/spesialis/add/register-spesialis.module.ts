import { NgModule }                                     from '@angular/core';
import { CommonModule }                                 from '@angular/common';

//shared module for component, directives
import { SharedModule }                                 from '../../../shared/shared.module';

// Notifications
import { ToasterModule, ToasterService}                 from 'angular2-toaster/angular2-toaster';

//Routing
import { RegisterSpesialisRoutingModule }              from './register-spesialis-routing.module';

import { RegisterSpesialisComponent }                             from './register-spesialis.component';
import { ControlMessagesComponent }                               from '../../../shared/control-messages.component';
import { ConfigService, SpesialisService, PolyClinicService,
         ValidationService }                                      from '../../../services';

@NgModule({
  imports: [
    CommonModule,
    RegisterSpesialisRoutingModule,
    SharedModule,
    ToasterModule
  ],
  declarations: [
    RegisterSpesialisComponent,
    ControlMessagesComponent
  ],
  providers:[
    ConfigService,
    SpesialisService,
    PolyClinicService,
    ValidationService
  ]
})
export class RegisterSpesialisModule { }
