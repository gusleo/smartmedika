import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { StaffRoutingModule } from './staff-routing.module';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { Message } from '../../libs';
import { ListVerificationUserComponent, ListCancelVerificationComponent, SetRulesComponent, RegisterRulesDialog } from '../staff';
import { HospitalOperatorService, RolesService } from '../services';

@NgModule({
    imports: [
      CommonModule,
      StaffRoutingModule,
      MaterialModule,
      FormsModule,
      ReactiveFormsModule,
      FlexLayoutModule,
      NgxDatatableModule
    ],
    declarations: [ListVerificationUserComponent, ListCancelVerificationComponent, SetRulesComponent, RegisterRulesDialog],
    providers: [HospitalOperatorService, Message, RolesService],
    entryComponents: [RegisterRulesDialog]
  })
  export class StaffModule { }