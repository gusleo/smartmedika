import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { HomePage } from '../home/home';
import { LoginPage } from '../login/login';
import { Commons } from '../../util/commons';


/**
 * Generated class for the Splash page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-splash',
  templateUrl: 'splash.html',
})
export class Splash {

  params:any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams, private common: Commons) {
    this.params.data = {
      "duration" : 3000,
      "backgroundImage" : 'assets/images/background/31.jpg',
      "logo" : 'assets/images/logo/smlogo.png',
      "title" : "SmartMedika"
   };

   this.params.events = {
      "onRedirect" : () => {
         console.log('redirect')
        if(this.common.hasLoggedIn === true) {
          this.navCtrl.setRoot(HomePage)
        } else {
          this.navCtrl.setRoot(LoginPage)
        }
      }
   }

  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad Splash');
  }

}
