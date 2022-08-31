import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { PatientService } from '../../providers';
import { Commons } from '../../util/commons';

/**
 * Generated class for the PatientCreatePage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-patient-create',
  templateUrl: 'patient-create.html',
  providers: [PatientService]
})
export class PatientCreatePage {

  params:any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams, private service: PatientService, private common: Commons) {
    this.params.data = {
        "name"        : "Nama Pasien",
        "savebutton"        : "Simpan",
        "logo": "assets/images/logo/logo-blue-02-1.png",
        "laki": "Laki",
        "perempuan": "Perempuan",
        "tanggallahir": "Tanggal Lahir"
      };

    this.params.events = {
      onSave: (params) => {
        console.log('onSave:' + JSON.stringify(params));
        this.common.showLoading('Menambahkan data...', true);
        this.service.createPatient(params).subscribe(
          res => {
            this.common.hideLoading();
            this.navCtrl.pop();
          }, error => {
            console.error(error);
          }
        )
      }
    }
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PatientCreatePage');
  }

}
