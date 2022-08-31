import { Component } from '@angular/core';
import { NavController, NavParams, MenuController } from 'ionic-angular';
import { Storage } from '@ionic/storage';
import { LoginPage } from '../login/login';
import { HomePage } from '../home/home';
import { UserData } from '../../providers';

/**
 * Generated class for the WizardPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-wizard',
  templateUrl: 'wizard.html',
})
export class WizardPage {

  params:any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams, public menu: MenuController, public storage: Storage, public userData: UserData) {

      // define data from service
      this.params.data = {
          'toolBarTitle': 'Simple + icon',
          'btnPrev': 'Previous',
          'btnNext': 'Next',
          'btnFinish': 'Finish',
          'items': [
              {
                iconSlider: 'icon-star-outline',
                title: 'Fragment Example 1',
                description: 'Text for Fragment Example 1 Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.'
              },
              {
                iconSlider: 'icon-star-half',
                title: 'Fragment Example 2',
                description: 'Text for Fragment Example 2 Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.'
              },
              {
                iconSlider: 'icon-star',
                title: 'Fragment Example 3',
                description: 'Text for Fragment Example 3 Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.'
              }
          ]
      };

      // define service from service
      this.params.events = {
        'onFinish': (event: any) => {
          console.log('Finish');
          this.userData.hasLoggedIn().then((hasLoggedIn) => {
            if (hasLoggedIn) {
              this.navCtrl.push(HomePage).then(() => {
                this.storage.set('hasSeenTutorial', 'true');
              })
            } else {
              this.navCtrl.push(LoginPage).then(() => {
                this.storage.set('hasSeenTutorial', 'true');
              })
            }
          });
        }
      };
  }

  ionViewDidEnter() {
    // the root left menu should be disabled on the tutorial page
    this.menu.enable(false);
  }

  ionViewDidLeave() {
    // enable the root left menu when leaving the tutorial page
    this.menu.enable(true);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad WizardPage');
  }

}
