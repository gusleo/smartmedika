import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { LoginPage } from '../login/login';
import { RegisterPage } from '../register/register';
import { LoginVerificationPage } from '../login-verification/login-verification';
import { LoginWithEmailPage } from '../login-with-email/login-with-email';
import { PatientListPage } from '../patient-list/patient-list';
import { NoNetworkPage } from '../no-network/no-network';
import { AppointmentPage } from '../appointment/appointment';
import { SearchPage } from '../search/search';
import { ClinicDetailPage } from '../clinic-detail/clinic-detail';
import { DoctorDetailPage } from '../doctor-detail/doctor-detail';
import { AppointmentPatientPage } from '../appointment-patient/appointment-patient';
import { AuthService } from '../../providers';
import { Commons } from '../../util/commons';
import * as Constant from '../../util/constants';
import { HomepagePage } from '../homepage/homepage';
import { AppointmentInfoPage } from '../appointment-info/appointment-info';

/**
 * Generated class for the TestingPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-testing',
  templateUrl: 'testing.html',
})
export class TestingPage {

  constructor(public navCtrl: NavController, public navParams: NavParams, public auth: AuthService, private commons: Commons) {
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad TestingPage');
  }

  onClickLogin() {
    this.navCtrl.push(LoginPage);
  }

  onClickLoginWithEmail() {
    this.navCtrl.push(LoginWithEmailPage);
  }

  onClickLoginVerification() {
    this.navCtrl.push(LoginVerificationPage);
  }

  onClickPatient(){
    this.navCtrl.push(PatientListPage);
  }

  onClickRegister(){
    this.navCtrl.push(RegisterPage);
  }

  onClickTemp() {
    this.navCtrl.push(NoNetworkPage);
  }

  onClickAppointment() {
    this.navCtrl.push(AppointmentPage);
  }

  onClickSearch() {
    this.navCtrl.push(SearchPage);
  }

  onClickClinicDetail() {
    this.navCtrl.push(ClinicDetailPage);
  }

  onClickDoctorDetail() {
    this.navCtrl.push(DoctorDetailPage);
  }

  onClickAppointmentPatient() {
    this.navCtrl.push(AppointmentPatientPage);
  }

  onClickRenewToken() {
      if (navigator.onLine === false) {
        this.commons.showNoNetworkMessage();
      }
      else {
        this.commons.showLoading('Renewing token...', false);
        this.auth.login('suarkadipa@gmail.com', 'Test12345!').subscribe((message: any) => {
          if (message === Constant.ALLOWED) {
            setTimeout(() => {
              this.commons.loading.dismiss();
              this.navCtrl.push(HomepagePage);
            });
          } else {
            this.commons.showError(message);
          }
        },
          (error: any) => {
            this.commons.showError(error);
          });
      }
  }

  onClickAppointmentInfo() {
    this.navCtrl.push(AppointmentInfoPage);
  }

}
