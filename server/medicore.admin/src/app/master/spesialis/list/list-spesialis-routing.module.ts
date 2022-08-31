import { NgModule }         from '@angular/core';
import { Routes,
         RouterModule }     from '@angular/router';

import { ListSpesialisComponent }  from './list-spesialis.component';

const routes: Routes = [
  {
    path: '',
    component: ListSpesialisComponent,
    data: {
      title: 'Spesialis'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ListSpesialisRoutingModule {}