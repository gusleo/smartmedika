import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { PatientCreatePage } from '../patient-create/patient-create';
import { SearchPage } from '../search/search';
import { NotificationPage } from '../notification/notification';
import { PatientService } from '../../providers';
import { PatientModel } from '../../model';
import { Commons } from '../../util/commons';
import * as Constant from '../../util/constants';
import { PatientDetailPage } from '../patient-detail/patient-detail';

/**
 * Generated class for the PatientListPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-patient-list',
  templateUrl: 'patient-list.html',
  providers: [PatientService]
})
export class PatientListPage {
  patientList: PatientModel[]
  constructor(public navCtrl: NavController, public navParams: NavParams, private service: PatientService, private common: Commons) {
    this.getAllPatient();
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad PatientListPage');
  }

  onClickAddPatient() {
    this.navCtrl.push(PatientCreatePage);
  }

  onClickSearch() {
    this.navCtrl.push(SearchPage);
  }

  onClickNotifications() {
    this.navCtrl.push(NotificationPage);
  }

  getAllPatient() {
    let pageIndex = 1;
    let pageSize = Constant.PAGE_SIZE;
    this.common.showLoading('Mengunduh data pasien...', true);
    this.service.getAllPatient(pageIndex, pageSize).subscribe(
      res => {
        this.common.hideLoading();
        console.log(res.items);
        this.patientList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

  onItemClicked(id: any) {
    console.log('patientId:', id)
    this.navCtrl.push(PatientDetailPage, {
      patientId: id,
      callback: this.myCallbackFunction
    })
  }

  myCallbackFunction = function (_params) {
    return new Promise((resolve, reject) => {
      resolve();
    });
  }

}
