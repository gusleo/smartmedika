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
import { ListSpesialisRoutingModule }          from './list-spesialis-routing.module';

import { ListSpesialisComponent }              from './list-spesialis.component';
import { ConfigService, SpesialisService }     from '../../../services';


@NgModule({
    imports: [
        CommonModule,         
        SharedModule,               
        ListSpesialisRoutingModule,
        ModalModule.forRoot(),        
        PaginationModule.forRoot(),
        ToasterModule
    ],
    declarations: [
        ListSpesialisComponent
    ],
    providers: [
        ConfigService,
        SpesialisService 
    ]
})
export class ListSpesialisModule { }