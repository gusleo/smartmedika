import { NgModule }                     from '@angular/core';
import { CommonModule }                 from '@angular/common';


//Routing
import { FormPatientRoutingModule }      from './form-patient-routing.module';

import { FormPatientComponent }          from './form-patient.component';

@NgModule({
    imports: [
        FormPatientRoutingModule
    ],
    declarations: [
        FormPatientComponent
    ]
})
export class FormPatientModule { }