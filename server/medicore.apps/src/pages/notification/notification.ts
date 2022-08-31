import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';

/**
 * Generated class for the NotificationPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-notification',
  templateUrl: 'notification.html',
})
export class NotificationPage {

  notifications:any = {};
  constructor(public navCtrl: NavController, public navParams: NavParams) {
      this.notifications = [
        {
          title: 'Trouble in poop - & Foods at your rescue',
          timeline: 'Fri, 24 Feb at 1:30 PM'
        },
        {
          title: 'Trouble in poop - & Foods at your rescue',
          timeline: 'Fri, 24 Feb at 1:30 PM'
        }
      ]
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad NotificationPage');
  }

}
