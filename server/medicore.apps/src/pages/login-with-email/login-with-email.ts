import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import * as Constant from '../../util/constants';
import { Commons } from '../../util/commons';
import { AuthService } from '../../providers';
import { HomePage } from '../home/home';

/**
 * Generated class for the LoginWithEmailPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-login-with-email',
  templateUrl: 'login-with-email.html',
})
export class LoginWithEmailPage {

  params: any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams, private commons: Commons, private auth: AuthService) {
    this.params.data = {
      "email": "Email",
      "password": "Password",
      "login": "Login",
      "logo": "assets/images/logo/logo-blue-02-1.png"
    };

    this.params.events = {
      onLogin: (params) => {
        console.log('onLogin:' + JSON.stringify(params));
        if (navigator.onLine === false) {
          this.commons.showNoNetworkMessage();
        }
        else {
          this.commons.showLoading('Logging in...', false);
          this.auth.login(params.email, params.password).subscribe((message: any) => {
            if (message === Constant.ALLOWED) {
              setTimeout(() => {
                this.commons.loading.dismiss();
                this.navCtrl.setRoot(HomePage)
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
    };
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad LoginWithEmailPage');
  }

}
