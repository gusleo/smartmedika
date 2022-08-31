import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListVerificationUserComponent, ListCancelVerificationComponent, SetRulesComponent } from '../staff';


const routes: Routes = [ 
    {
        path: '',
        children: [{
            path: 'list-verification-user',
            component: ListVerificationUserComponent
        }, {
            path: 'list-cancel-verification',
            component: ListCancelVerificationComponent            
        }, {
            path: 'set-rules',
            component: SetRulesComponent            
        }
        ]
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
  })
  export class StaffRoutingModule { }