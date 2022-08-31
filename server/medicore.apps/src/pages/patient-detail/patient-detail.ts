import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { PatientModel } from '../../model';
import { Commons } from '../../util/commons';
import { PatientService } from '../../providers';

/**
 * Generated class for the PatientDetailPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-patient-detail',
  templateUrl: 'patient-detail.html',
  providers: [PatientService]
})
export class PatientDetailPage {

  data: any = {};
  patient: PatientModel = new PatientModel();
  patientId: any;
  patientDetail: PatientModel;
  callback: any;
  constructor(public navCtrl: NavController, public navParams: NavParams, private common: Commons, private service: PatientService) {
    this.data = {
        "name"        : "Nama Pasien",
        "savebutton"        : "Simpan",
        "logo": "assets/images/logo/logo-blue-02-1.png",
        "laki": "Laki",
        "perempuan": "Perempuan",
        "tanggallahir": "Tanggal Lahir"
    }

    this.patientId = navParams.get('patientId');
    console.log('patientId', this.patientId);
    this.getPatientDetail(this.patientId);
    this.callback = this.navParams.get("callback");
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PatientDetailPage');
  }

  getPatientDetail(patientId: number) {
    this.common.showLoading('Mengunduh data...', true);
    this.service.getPatientDetail(patientId).subscribe(
      res => {
        this.common.hideLoading();
        console.log('patient detail', res);
        this.patientDetail = res;
        this.patient.asociatedUserId = this.patientDetail.asociatedUserId;
        this.patient.associatedUser = this.patientDetail.associatedUser;
        this.patient.createdById = this.patientDetail.createdById;
        this.patient.createdByUser = this.patientDetail.createdByUser;
        this.patient.createdDate = this.patientDetail.createdDate;
        this.patient.dateOfBirth = this.patientDetail.dateOfBirth;
        this.patient.gender = this.patientDetail.gender;
        this.patient.id = this.patientDetail.id;
        this.patient.patientName = this.patientDetail.patientName;
        this.patient.patientStatus = this.patientDetail.patientStatus;
        this.patient.relationshipStatus = this.patientDetail.relationshipStatus;
        this.patient.updatedById = this.patientDetail.updatedById;
        this.patient.updatedByUser = this.patientDetail.updatedByUser;
        this.patient.updatedDate = this.patientDetail.updatedDate;
      }, error => {
        console.error(error);
      }
    )
  }

  onUpdate(patient: PatientModel) {
    // set it patientId in order to update
    patient.id = this.patientId;
    console.log('patient to update:', JSON.stringify(patient));
    this.common.showLoading('Memutakhirkan data...', true);
    this.service.updatePatient(patient).subscribe(
      res => {
        this.common.hideLoading();
        this.callback(patient).then(()=>{
          this.navCtrl.pop();
        })
      }, error => {
        console.error(error);
      }
    )
  }

}
