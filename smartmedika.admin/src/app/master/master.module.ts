import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import 'hammerjs';

import { MasterRoutes } from './master.routing';

import { ListPolyClinicComponent, RegisterPolyClinicDialog, DeletePolyclinicDialog } from './polyclinic';
import { ListSpecialistComponent, RegisterSpecialistDialog, DeleteSpecialistDialog } from './specialist';
import { PolyClinicService, ApplicationService,
         SpecialistService, GeoLocationService }    from '../services';
import { CountryListComponent, CountryRegisterDialog } from './country';
import { RegencyListComponent, RegisterRegencyDialog } from './regency';
import { RegionListComponent, RegionRegisterDialog } from './region';
import { MenuListComponent, MenuRegisterDialog } from './menu'

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(MasterRoutes),
    MaterialModule,
    HttpModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    NgxDatatableModule
  ],
  declarations: [
    ListPolyClinicComponent,
    RegisterPolyClinicDialog,
    ListSpecialistComponent,
    RegisterSpecialistDialog,
    CountryListComponent,
    RegencyListComponent,
    RegionListComponent,
    CountryRegisterDialog,
    RegionRegisterDialog,
    RegisterRegencyDialog,
    DeletePolyclinicDialog,
    DeleteSpecialistDialog,
    MenuListComponent,
    MenuRegisterDialog    
  ],
  providers: [
    PolyClinicService,
    SpecialistService,
    GeoLocationService,
    ApplicationService
  ],
  entryComponents: [CountryRegisterDialog, RegionRegisterDialog, RegisterRegencyDialog, RegisterSpecialistDialog, 
    RegisterPolyClinicDialog, DeletePolyclinicDialog, DeleteSpecialistDialog, MenuRegisterDialog]     
})
export class MasterModule { }