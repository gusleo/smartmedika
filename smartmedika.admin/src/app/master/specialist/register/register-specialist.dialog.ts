import { Component, OnInit, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
import { Observable } from 'rxjs/Observable';
// config API & Grab everything with import model;
import { MedicalStaffSpesialisModel, PolyClinicModel,
         MedicalStaffType } from '../../../model';
import { SpecialistService, PolyClinicService } from '../../../services';

@Component({
  selector: 'app-registerspecialistdialog',
  templateUrl: './register-specialist.dialog.html',
  styleUrls: ['./register-specialist.dialog.scss']
})
export class RegisterSpecialistDialog implements OnInit {

    form: FormGroup;    
    model = new MedicalStaffSpesialisModel();
    currentSpecialist: any;
    polyObservable: Observable<PolyClinicModel[]>;
    polyContent: PolyClinicModel[] = [];
    resultStaff: any[] = [];
    resultPoly: Array<PolyClinicModel>;
    onRequest:boolean;
    //autocomplete for polyclinic
    polyCtrl: FormControl;    
    listPoly: any;
    resPoly: PolyClinicModel[] = [];
 
    namePoly: string;
    currentPolyclinic: any[] = [];
    
    constructor(public dialogRef: MdDialogRef<RegisterSpecialistDialog>, @Inject(MD_DIALOG_DATA) public data: any, 
    public polyClinicService: PolyClinicService, public specialistSevice: SpecialistService, private formBuilder: FormBuilder) {

        this.model = data.staff;
        this.namePoly = data.polysName;
        if(this.model == undefined) {
            this.model = new MedicalStaffSpesialisModel();        
        } 
        this.loadPoliklinik();
        this.loadTipeStaf();
        this.onLoadAutoCompletePoly();
    }
  
    async loadPoliklinik() {
       let res: Array<PolyClinicModel> = await this.polyClinicService.getAll().toPromise();
   
       this.resultPoly = res;
       res.forEach(el => {
           /*if(this.model.polyClinicId != null){
               if(this.model.polyClinicId == el.id){
                    //this.namePoly = el.name;
               }
           }*/
            this.resPoly.push({id: el.id, name: el.name});            
       });
    }

    ngOnInit() {
        // setting for validation
        this.form = this.formBuilder.group({
            nameMessage: [null, Validators.compose([Validators.required])],
            aliasMessage: [null, Validators.compose([Validators.required])],
            predikatMessage: [null, Validators.compose([Validators.required])]
            //polyCtrl: [null, Validators.compose([Validators.required])]
        });
    }
    
    onLoadAutoCompletePoly() {
        this.polyCtrl = new FormControl({id: this.model.polyClinicId, name: this.namePoly});
        this.listPoly = this.polyCtrl.valueChanges
            .startWith(this.polyCtrl.value)
            .map(val => this.displayPolyclinic(val))
            .map(name => this.filterPolyclinic(name));
    }

  displayPolyclinic(value: any): string {
    return value && typeof value === 'object' ? value.name : value;
  }

  filterPolyclinic(val: String) {
    if (val) {
      if(this.polyCtrl.value.id != undefined){
        this.setModelPoly(this.polyCtrl.value.id);
      }      
      const filterValue = val.toLowerCase();
      return this.resPoly.filter(item => item.name.toLowerCase().startsWith(filterValue));
    }    
    return this.resPoly;
  }     

  setModelPoly(id: number) {
      this.model.polyClinicId = id;
  }

    initPolyclinicAutoComplete() {
        this.polyObservable = this.form.get('polyclinics').valueChanges
        .debounceTime(400)
        .do(value => {
            let exist = this.polyContent.findIndex(t => t.name === value)
            if (exist > -1) return;
        })
    }

    close(){
        this.dialogRef.close();
    }

    onStaffTypeChange(event){
      this.model.staffType = event;
    }

    loadTipeStaf() {
        this.resultStaff.push({id: 1, name: 'Dokter'});
        this.resultStaff.push({id: 2, name: 'Perawat'});
        this.resultStaff.push({id: 3, name: 'Bidan'});
        this.resultStaff.push({id: 4, name: 'Terapis'});

        return this.resultStaff;
    }

    async save() {
        this.onRequest = true;
        let response:any;
        if(this.model.id == 0){
            response = await this.specialistSevice.save(this.model).toPromise();
        }else{
            response = await this.specialistSevice.update(this.model, this.model.id).toPromise();
        }
         this.onRequest = false;
        
        this.dialogRef.close({
            model: response
        });       

    }
}