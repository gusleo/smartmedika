import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';

/**
 * Generated class for the RegisterPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-register',
  templateUrl: 'register.html',
})
export class RegisterPage {

  params: any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.params.data = {
      "logo": "assets/images/logo/logo-01.png",
      "iconName": "icon-account",
      "name": "Nama",
      "iconPhone": "icon-phone",
      "phone": "Handphone",
      "submit": "Verifikasi"
    };

    this.params.events = {
      onRegister: (params) => {
        console.log('onRegister:' + JSON.stringify(params));
      },
      onSkip: (params) => {
        console.log('onSkip:' + JSON.stringify(params));
      }
    };
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad RegisterPage');
  }

}
