import { Component }        from '@angular/core';
import { Router }           from '@angular/router';

import { PaginationEntity, Pagination, MedicalStaffModel, MedicalStaffSpesialisModel}  		from '../../model';
import { SpesialisService, StaffService }                       from '../../services';

@Component({
    templateUrl: 'list-doctor.component.html'
})
export class ListDoctorComponent extends Pagination {

    public specialists:MedicalStaffSpesialisModel[];
    
    constructor(private router: Router, protected specialistService:SpesialisService, protected staffService:StaffService) 
    {
        super();
    }

    loadSpecialist(){
        
    }
    register()
    {
        this.router.navigate(['/doctor/register-doctor']);
    }
}
