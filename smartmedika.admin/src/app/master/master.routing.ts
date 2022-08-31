import { Routes } from '@angular/router';

import { ListPolyClinicComponent }      from './polyclinic/list/list-polyclinic.component';
//import { RegisterPolyClinicComponent }  from './polyclinic/register/register-polyclinic.component';
import { ListSpecialistComponent }      from './specialist';
//import { RegisterSpecialistComponent }  from './specialist/register/register-specialist.component';
import { CountryListComponent } from './country';
import { RegencyListComponent } from './regency';
import { RegionListComponent } from './region';
import { MenuListComponent } from './menu';

export const MasterRoutes: Routes = [
  {
    path: '',
    children: [{
      path: 'polyclinic',
      component: ListPolyClinicComponent
    }/*,{
      path: 'polyclinic/register/:id',
      component: RegisterPolyClinicComponent      
    }*/,{
      path: 'specialist',
      component: ListSpecialistComponent      
    }/*, {
      path: 'specialist/register/:id',
      component: RegisterSpecialistComponent
    }*/, {
      path: 'country',
      component: CountryListComponent
    }, {
      path: 'region',
      component: RegionListComponent
    }, {
      path: 'regency',
      component: RegencyListComponent
    },{
      path: 'menu',
      component: MenuListComponent
    }
    ]
  }
];