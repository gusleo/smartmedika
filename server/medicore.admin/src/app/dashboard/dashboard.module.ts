import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts/ng2-charts';
import { DropdownModule } from 'ng2-bootstrap/dropdown';

import { DashboardComponent } from './dashboard.component';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { IdentityService } from '../services';

@NgModule({
  imports: [
    DashboardRoutingModule,
    ChartsModule,
    DropdownModule
  ],
  declarations: [ DashboardComponent ],
  providers:[IdentityService]
})
export class DashboardModule { }
