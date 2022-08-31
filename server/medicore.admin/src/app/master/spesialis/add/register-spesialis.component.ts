import { Component, ViewChild, OnInit }                     from '@angular/core';
import { ActivatedRoute, Router }                           from '@angular/router';
import { FormGroup, FormBuilder, Validators }               from '@angular/forms';
import { ToasterModule, ToasterService, ToasterConfig }     from 'angular2-toaster/angular2-toaster';
import { Observable }                                       from 'rxjs/Observable';

//grap api, service, model
import { AppConstant }                                      from '../../../';
import { MedicalStaffSpesialisModel, PolyClinicModel, PaginationEntity, MedicalStaffType }	    from '../../../model';
import { SpesialisService, PolyClinicService }              from '../../../services';

@Component({
    templateUrl: 'register-spesialis.component.html'
})

export class RegisterSpesialisComponent{
    
    private toasterService: ToasterService;

    public toasterconfig : ToasterConfig =
        new ToasterConfig({
            tapToDismiss: true,
            timeout: 5000
        });
    
    
    spesialisForm: any;
    private resultStaff: any[] = [];
    private resultPoli: Array<PolyClinicModel>;
    private model = new MedicalStaffSpesialisModel();
 	constructor(private route:ActivatedRoute, private router: Router, private service: SpesialisService, private poliService: PolyClinicService,
    toasterService: ToasterService, private formBuilder: FormBuilder) {

            this.toasterService = toasterService;
            this.route.params.subscribe(params => {
                this.model.id =  Number.parseInt(params['id']);                
            });

            this.spesialisForm = this.formBuilder.group({
                'name': ['', Validators.required],
                'fungsi': ['', Validators.required],
                'deskripsi': ['', Validators.required],
                'tipe': ['', Validators.required],
                'poli': ['', Validators.required]
            }); 
    }
    
    ngOnInit() {
        if(this.model.id != 0){
            this.service.getById(this.model.id).subscribe(
                res =>{
                    this.model = res;
                }, error => {
                    this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat ditampilkan'); 
                    console.error('Error: ' + error);     
                });            
        }
        this.loadPoliklinik();
        this.loadTipeStaf();
    }

    loadPoliklinik()
    {
        this.poliService.getAll().subscribe( 
            res => {
                this.resultPoli = res;
			}, error => {
				this.toasterService.pop('error', 'Konfirmasi', 'Data tidak menampilkan poliklinik'); 
				console.error('Error: ' + error);                
           });
    }

	 

    loadTipeStaf()
    {   
        this.resultStaff.push({id: 1, name: 'Dokter'});
        this.resultStaff.push({id: 2, name: 'Perawat'});
        this.resultStaff.push({id: 3, name: 'Bidan'});
        this.resultStaff.push({id: 4, name: 'Terapis'});

        return this.resultStaff;       
    }

     submitForm()
     {
        if (this.spesialisForm.dirty && this.spesialisForm.valid) {          
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
        this.model.alias = '';
        this.model.description = '';
        this.model.staffType = MedicalStaffType.Doctor;        
        this.model.polyclinicList = null;         
     }    
}