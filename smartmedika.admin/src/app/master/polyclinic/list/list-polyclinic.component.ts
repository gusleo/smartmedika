import { Component, OnInit, ViewChild, EventEmitter, 
         ElementRef, Renderer, TemplateRef } from '@angular/core';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { DialogConfig } from '../../../';
//call component
import { RegisterPolyClinicDialog } from '../register/register-polyclinic.dialog';
import { DeletePolyclinicDialog } from '../popup/deletepolyclinic/deletepolyclinic.dialog';
//config API & Grab everything with import model;
import { PolyClinicModel}	from '../../../model';
import { PolyClinicService } from '../../../services';

@Component({
  selector: 'master-list-polyclinic',
  templateUrl: './list-polyclinic.component.html',
  styleUrls: ['./list-polyclinic.component.scss']
})
export class ListPolyClinicComponent{

  //array for each model
  result: Array<PolyClinicModel>;
  registerPolyClinicDialog: MdDialogRef<RegisterPolyClinicDialog>;
  deletePolyclinicDialog: MdDialogRef<DeletePolyclinicDialog>;
  autoHide = 3000;

  constructor(public snackBar: MdSnackBar, public dialog: MdDialog, 
    public polyClinicService: PolyClinicService, private renderer:Renderer) {}

	ngOnInit() {
		this.loadPolyClinic();
	}

  //load polyclinic from api  service
  async loadPolyClinic() {
     let res:Array<PolyClinicModel> = await this.polyClinicService.getAll().toPromise();
     this.result = res;
  }  

  openRegisterDialog(poly?:PolyClinicModel) {
    let value:PolyClinicModel
    if(poly == undefined){
      value = new PolyClinicModel();
    }else{
      value = Object.assign(new PolyClinicModel(), poly);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
    
    let config:any = DialogConfig;
    config.data = {     
      poly: value
    }

    this.registerPolyClinicDialog = this.dialog.open(RegisterPolyClinicDialog, config);
    this.registerPolyClinicDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
        if(value.id == 0){
          this.snackBar.open("Berhasil menambahkan data", "", );messageConfig
        }else{
            this.snackBar.open("Berhasil mengedit data", "", messageConfig);      
        }
        this.loadPolyClinic();
      }
      this.registerPolyClinicDialog = null;
    });
  }  

  openDeleteDialog(poly?:PolyClinicModel) {
    let value: PolyClinicModel;
    if(poly == undefined){
      value = new PolyClinicModel();
    }else{
      value = Object.assign(new PolyClinicModel(), poly);
    }

    var messageConfig = new MdSnackBarConfig();
    messageConfig.duration = this.autoHide;
        
    let config: any = DialogConfig;
    config.data = {
      poly: value
    }
    this.deletePolyclinicDialog = this.dialog.open(DeletePolyclinicDialog, config);
    this.deletePolyclinicDialog.afterClosed().subscribe(result => {
      if(result != undefined){
          this.snackBar.open("Berhasil menghapus data", "", messageConfig);
      }
      this.loadPolyClinic();
      this.deletePolyclinicDialog = null;
    });    
  }
}

