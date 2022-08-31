import { Component } from '@angular/core';
import { NavController, Platform, AlertController, Alert } from 'ionic-angular';
import { TabPage1 } from '../tab-page-1/tab-page-1';
import { TabPage2 } from '../tab-page-2/tab-page-2';
import { TabPage3 } from '../tab-page-3/tab-page-3';
import { SearchPage } from '../search/search';
import { NotificationPage } from '../notification/notification';
import { Commons } from '../../util/commons';

@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {

  params: any = {}
  public unregisterBackButtonAction: any;
  alert: Alert;
  constructor(public navCtrl: NavController, private platform: Platform, private alertCtrl: AlertController
    ,public common: Commons) {
    this.params.data = [
      { page: TabPage1, icon: "home" },
      { page: TabPage2, icon: "paper" },
      { page: TabPage3, icon: "person" }
    ];

    // get promise of latitude and longitude before executed on tab1
    this.common.getLatitude();
    this.common.getLongitude();

    // get the token in async await way
    this.common.getToken();
  }

  onClickSearch() {
    this.navCtrl.push(SearchPage);
  }

  onClickNotifications() {
    this.navCtrl.push(NotificationPage);
  }

  ionViewDidEnter() {
    this.initializeBackButtonCustomHandler();
  }

  ionViewWillLeave() {
    // Unregister the custom back button action for this page
    this.unregisterBackButtonAction && this.unregisterBackButtonAction();
  }

  public initializeBackButtonCustomHandler(): void {
    this.unregisterBackButtonAction = this.platform.registerBackButtonAction(() => {
      this.customHandleBackButton();
    }, 10);
  }

  private customHandleBackButton(): void {
    // do what you need to do here ...
    if(this.alert) {
      this.alert.dismiss();
      this.alert = null;
    } else {
      this.presentConfirm();
    }
}

presentConfirm() {
  this.alert = this.alertCtrl.create({
    title: '',
    message: 'Yakin keluar aplikasi SmartMedika?',
    buttons: [
      {
        text: 'Tidak',
        role: 'cancel',
        handler: () => {
          console.log('Cancel clicked');
          this.alert = null
        }
      },
      {
        text: 'Iya',
        handler: () => {
          console.log('Yes clicked');
          this.platform.exitApp();
        }
      }
    ]
  });
  this.alert.present();
}

}
