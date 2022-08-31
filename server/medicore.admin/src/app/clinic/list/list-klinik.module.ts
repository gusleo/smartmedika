import { NgModule }                         from '@angular/core';
import { CommonModule }                     from '@angular/common';

import { PaginationModule }                 from 'ng2-bootstrap/pagination';

//shared module for component, directives
import { SharedModule }                     from '../../shared/shared.module';

//ng2bootstrap module
import { ModalModule }                      from 'ng2-bootstrap/modal';

//Routing
import { ListKlinikRoutingModule }          from './list-klinik-routing.module';

import { ListKlinikComponent }              from './list-klinik.component';
import { ClinicService, ConfigService, RegionService }  from '../../services';


@NgModule({
    imports: [
        CommonModule,         
        SharedModule,               
        ListKlinikRoutingModule,
        ModalModule.forRoot(),        
        PaginationModule.forRoot()
    ],
    declarations: [
        ListKlinikComponent
    ],
    providers: [
        ConfigService,
        ClinicService,
        RegionService
    ]
})
export class ListKlinikModule { }