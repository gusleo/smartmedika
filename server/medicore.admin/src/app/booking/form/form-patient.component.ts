import { Component }                from '@angular/core';
import { ActivatedRoute, Router }   from '@angular/router';
import { Response }                 from '@angular/http';

@Component({
    templateUrl: 'form-patient.component.html'
})
export class FormPatientComponent {
    
    sub: any;
    getNameDoctor: string;

    constructor(private route: ActivatedRoute, private router: Router) { }

    ngOnInit(){
        this.sub = this.route.params.subscribe(params => {
            let id = Number.parseInt(params['id']);
            if(id == 1){
                this.getNameDoctor = 'Prof. Hendra Santosa';
            }else if(id == 2) {
                this.getNameDoctor = 'Dr. Upadana';
            }else if(id == 3) {
                this.getNameDoctor = 'Dr. Suyasa';
            }else if(id == 4) {
                this.getNameDoctor = 'dr. Wirama';
            }else if(id == 5) {
                this.getNameDoctor = 'dr. Surya';                
            }else if(id == 6) {
                this.getNameDoctor = 'Dr. Leo';
            }
        });
    }
}
