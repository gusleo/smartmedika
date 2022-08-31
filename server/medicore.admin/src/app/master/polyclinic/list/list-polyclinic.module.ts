import { NgModule }                             from '@angular/core';
import { CommonModule }                         from '@angular/common';

import { PaginationModule }                     from 'ng2-bootstrap/pagination';

//shared module for component, directives
import { SharedModule }                         from '../../../shared/shared.module';

// Notifications
import { ToasterModule, ToasterService}         from 'angular2-toaster/angular2-toaster';
//ng2bootstrap module
import { ModalModule }                          from 'ng2-bootstrap/modal';

//Routing
import { ListPolyClinicRoutingModule }          from './list-polyclinic-routing.module';

import { ListPolyClinicComponent }              from './list-polyclinic-component';
import { ConfigService, PolyClinicService }     from '../../../services';


@NgModule({
    imports: [
        CommonModule,         
        SharedModule,               
        ListPolyClinicRoutingModule,
        ModalModule.forRoot(),        
        PaginationModule.forRoot(),
        ToasterModule
    ],
    declarations: [
        ListPolyClinicComponent
    ],
    providers: [
        ConfigService,
        PolyClinicService 
    ]
})
export class ListPolyClinicModule { }