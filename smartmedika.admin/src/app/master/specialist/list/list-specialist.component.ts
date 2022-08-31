import { Component, OnInit, ViewChild, EventEmitter, 
         ElementRef, Renderer, TemplateRef } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogConfig } from '../../../';
import { RegisterSpecialistDialog } from '../register/register-specialist.dialog';
import { DeleteSpecialistDialog } from '../popup/deletespecialist/deletespecialist.dialog';
//config API & Grab everything with import model;
import { MedicalStaffSpesialisModel, PolyClinicModel} from '../../../model';
import { SpecialistService, PolyClinicService } from '../../../services';

@Component({
  selector: 'master-list-specialist',
  templateUrl: './list-specialist.component.html',
  styleUrls: ['./list-specialist.component.scss']
})
export class ListSpecialistComponent{

  public dialogResult: any;
  result: Array<MedicalStaffSpesialisModel>;
  registerSpecialistDialog: MdDialogRef<RegisterSpecialistDialog>;
  deleteSpecialistDialog: MdDialogRef<DeleteSpecialistDialog>;
  currentPoly = new PolyClinicModel();
  dataNamePolyclinic: string = '';
  autoHide = 3000;

  constructor(public snackBar: MdSnackBar, public dialog: MdDialog,
     public specialistService: SpecialistService, public polyClinicService: PolyClinicService) {}

	ngOnInit() {
		this.loadSpecialist();
	}

  async getByIdPolyclinic(id: number) {
    this.currentPoly = await this.polyClinicService.getById(id).toPromise();
    //console.log(this.currentPoly);
    if(this.currentPoly != null){
      this.dataNamePolyclinic = this.currentPoly.name;      
    }else{
      this.dataNamePolyclinic = null;
    }
  } 

  //load specialist from api service
  async loadSpecialist() {
    let res: Array<MedicalStaffSpesialisModel> = await this.specialistService.getAll().toPromise();
    this.result = res;
  }       

  async openRegisterDialog(staff?:MedicalStaffSpesialisModel, polysName?:string) {
    let value:MedicalStaffSpesialisModel;
    let polynames: string;
    if(staff == undefined){
      value = new MedicalStaffSpesialisModel();
      this.dataNamePolyclinic = null;
    }else{
      value = Object.assign(new MedicalStaffSpesialisModel(), staff);
      await this.getByIdPolyclinic(staff.polyClinicId);
    }
    
    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
    
    let config:any = DialogConfig;
    config.data = {     
      staff: value,
      polysName: this.dataNamePolyclinic
    }

    this.registerSpecialistDialog = this.dialog.open(RegisterSpecialistDialog, config);
    this.registerSpecialistDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
        if(value.id == 0){
          this.snackBar.open("Berhasil menambahkan data", "", messageConfig);
        }else{
            this.snackBar.open("Berhasil mengedit data", "", messageConfig);      
        }
        this.loadSpecialist();
      }
      this.registerSpecialistDialog = null;
    });
  }

  openDeleteDialog(staff?:MedicalStaffSpesialisModel) {
    let value: MedicalStaffSpesialisModel;
    
    if(staff == undefined){
      value = new MedicalStaffSpesialisModel();      
    }else{
      value = Object.assign(new MedicalStaffSpesialisModel, staff);   
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;

    let config: any = DialogConfig;
    config.data = {
      staff: value  
    }
    this.deleteSpecialistDialog = this.dialog.open(DeleteSpecialistDialog, config);
    this.deleteSpecialistDialog.afterClosed().subscribe(result => {
      if(result != undefined){
        this.snackBar.open("Berhasil menghapus data", "", messageConfig);
      }
      this.loadSpecialist();
      this.deleteSpecialistDialog = null;
    });

  }

}