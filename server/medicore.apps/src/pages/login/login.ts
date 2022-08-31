import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { LoginWithEmailPage } from '../login-with-email/login-with-email';

/**
 * Generated class for the LoginPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-login',
  templateUrl: 'login.html',
})
export class LoginPage {

  params: any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.params.data = {
      "phone": "Nomor handphone",
      "login": "Login",
      "loginwithemail": "Login dengan Email",
      "logo": "assets/images/logo/logo-blue-02-1.png"
    };

    this.params.events = {
      onLogin: (params) => {
        console.log('onLogin:' + JSON.stringify(params));
      },
      onLoginWithEmail: (params) => {
        console.log('onLoginWithEmail:' + JSON.stringify(params));
        this.navCtrl.push(LoginWithEmailPage);
      }
    };
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad LoginPage');
  }

}
