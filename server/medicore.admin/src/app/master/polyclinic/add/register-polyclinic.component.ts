import { Component, ViewChild, OnInit }                     from '@angular/core';
import { ActivatedRoute, Router }                           from '@angular/router';
import { FormGroup, FormBuilder, Validators }               from '@angular/forms';
import { ToasterModule, ToasterService, ToasterConfig }     from 'angular2-toaster/angular2-toaster';
import { Observable }                                       from 'rxjs/Observable';

//grap api, service, model
import { AppConstant }                                      from '../../../';
import { PolyClinicModel }	                                from '../../../model';
import { PolyClinicService, ValidationService }             from '../../../services';

@Component({
    templateUrl: 'register-polyclinic.component.html'
})

export class RegisterPolyClinicComponent{
    
    private toasterService: ToasterService;

    public toasterconfig : ToasterConfig =
        new ToasterConfig({
            tapToDismiss: true,
            timeout: 5000
        });
    
    poliForm: any;
    private model = new PolyClinicModel();
 	constructor(private route:ActivatedRoute, private router: Router, private service: PolyClinicService, toasterService: ToasterService, private formBuilder: FormBuilder) {
    
            this.toasterService = toasterService;
            this.route.params.subscribe(params => {
                this.model.id =  Number.parseInt(params['id']);                
            });
            this.poliForm = this.formBuilder.group({
                'name': ['', Validators.required]
            });
      }

     ngOnInit() {
         if(this.model.id != 0){
            this.service.getById(this.model.id).subscribe(
                res =>{
                    this.model = res;
                    console.log(this.model.id);
                }, error => {
                    this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat ditampilkan'); 
                    console.error('Error: ' + error);     
                });
         }
     }

     submitForm()
     {
        if (this.poliForm.dirty && this.poliForm.valid) {         
            if(this.model.id == 0){
                this.service.save(this.model).subscribe(
                    res =>{
                        this.toasterService.pop('success', 'Konfirmasi', 'Data berhasil disimpan');
                        this.reset();
                    },
                    error => {
                        console.error('Error: ' + error);
                        this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat tersimpan'); 
                    });            
            }else{
                this.service.update(this.model, this.model.id).subscribe(
                    res =>{
                        this.toasterService.pop('success', 'Konfirmasi', 'Data berhasil disimpan');
                    },
                    error => {
                        console.error('Error: ' + error);
                        this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat tersimpan'); 
                    });            

            }
        }
     }

     reset()
     {
         this.model.id = 0;
         this.model.name = '';
     }
}