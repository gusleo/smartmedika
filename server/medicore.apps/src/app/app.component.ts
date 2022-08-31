import { Component, ViewChild } from '@angular/core';
import { Nav, Platform, MenuController, Events, AlertController } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { Geolocation } from '@ionic-native/geolocation';
import { Commons } from '../util/commons';
import { OperatingSystem } from '../model';
import { Diagnostic } from '@ionic-native/diagnostic';

import { HomepagePage } from '../pages/homepage/homepage';
// import { HomePage } from '../pages/home/home';
// import { ListPage } from '../pages/list/list';
// import { Splash } from '../pages/splash/splash';
// import { LoginPage } from '../pages/login/login';
import { WizardPage } from '../pages/wizard/wizard';
import { LoginPage } from '../pages/login/login';
// import { RegisterPage } from '../pages/register/register';
// import { CardPage } from '../pages/card/card';
import { RateAppPage } from '../pages/rate-app/rate-app';
import { InviteFriendAndFamilyPage } from '../pages/invite-friend-and-family/invite-friend-and-family';
import { SettingsPage } from '../pages/settings/settings';
import { AboutPage } from '../pages/about/about';
import { TestingPage } from '../pages/testing/testing';
import { NoNetworkPage } from '../pages/no-network/no-network';

import { UniqueDeviceID } from '@ionic-native/unique-device-id';
import { UserData } from '../providers/user-data';
import { Storage } from '@ionic/storage';
import { AuthService } from '../providers/authService';
import * as Constant from '../util/constants';

import {
  Push,
  PushToken
} from '@ionic/cloud-angular';

@Component({
  templateUrl: 'app.html'
})
export class MyApp {
  @ViewChild(Nav) nav: Nav;

  rootPage: any;

  pages: Array<any>;

  constructor(public platform: Platform, public statusBar: StatusBar, public splashScreen: SplashScreen, public push: Push,
    private geolocation: Geolocation, public common: Commons, private uniqueDeviceID: UniqueDeviceID, public userData: UserData,
    public menu: MenuController, public events: Events, public storage: Storage, private authService: AuthService,
    private diagnostic: Diagnostic, private alertCtrl: AlertController) {

    this.common.getToken();

    // Check if the user has already seen the tutorial
    this.storage.get('hasSeenTutorial')
      .then((hasSeenTutorial) => {
        if (hasSeenTutorial) {
          if (navigator.onLine === false) {
            this.rootPage = NoNetworkPage;
          } else {
            // decide which menu items should be hidden by current login status stored in local storage
            this.userData.hasLoggedIn().then((hasLoggedIn) => {
              if (hasLoggedIn) {
                this.authService.silentRenewToken().subscribe((message: any) => {
                  if (message === Constant.ALLOWED) {
                    setTimeout(() => {
                      console.log("Refresh token succeed!");
                      this.enableMenu(hasLoggedIn === true);
                      this.rootPage = HomepagePage;
                    });
                  } else {
                    console.log('silent renew token error.');
                    this.rootPage = LoginPage;
                  }
                },
                  (error: any) => {
                    console.log('silent renew token error any.');
                    this.common.showError(error);
                  });
              } else {
                this.rootPage = LoginPage;
              }
            });
          }
        } else {
          this.rootPage = WizardPage;
        }
        this.initializeApp();
      })

    // used for an example of ngFor and navigation
    this.pages = [
      { "title": "Home", "theme": "parallax", "icon": "icon-home", "component": HomepagePage },
      { "title": "Rate SmartMedika App", "theme": "parallax", "icon": "icon-star", "component": RateAppPage },
      { "title": "Invite Friends & Family", "theme": "parallax", "icon": "icon-share", "component": InviteFriendAndFamilyPage },
      { "title": "Settings", "theme": "parallax", "icon": "icon-settings", "component": SettingsPage },
      { "title": "About", "theme": "parallax", "icon": "icon-information", "component": AboutPage },
      { "title": "Show Tutorial", "theme": "parallax", "icon": "icon-human", "component": WizardPage },
      { "title": "Testing Page", "theme": "parallax", "icon": "icon-help", "component": TestingPage },
      // { "title": "Card", "theme": "parallax", "icon":"icon-format-align-justify", "component": CardPage },
      // { "title": "Login", "theme": "parallax", "icon":"icon-format-line-spacing", "component": LoginPage },
      // { "title": "Wizard", "theme": "parallax", "icon":"icon-format-line-spacing", "component": WizardPage },
      // { "title": "Register", "theme": "parallax", "icon":"icon-format-line-spacing", "component": RegisterPage }
    ];

    this.listenToLoginEvents();
  }

  initializeApp() {
    this.platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      this.statusBar.styleDefault();
      this.splashScreen.hide();

      this.getPosition();
      this.getPlatform();
      this.getUniqueDeviceID();
    });
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }

  getPosition() {
    this.geolocation.getCurrentPosition().then((resp) => {
      // resp.coords.latitude
      console.log("latitude:", resp.coords.latitude);
      this.common.setLatitude(resp.coords.latitude);
      // resp.coords.longitude
      console.log("longitude:", resp.coords.longitude);
      this.common.setLongitude(resp.coords.longitude);
    }).catch((error) => {
      console.log('Error getting location', error);
    });
  }

  getUniqueDeviceID() {
    this.uniqueDeviceID.get()
      .then((uuid: any) => {
        console.log("uuid:", uuid)
        this.common.setDeviceId(uuid);
      })
      .catch((error: any) => console.log("uuid error:", error));
  }

  getPlatform() {
    if (this.platform.is('cordova')) {
      this.push.register().then((t: PushToken) => {
        return this.push.saveToken(t);
      }).then((t: PushToken) => {
        this.common.setDeviceToken(t.token);
        console.log('Token saved:', t.token);
      });

      this.push.rx.notification()
        .subscribe((msg) => {
          alert(msg.title + ': ' + msg.text);
        });

      // check if someone has their GPS turned off in Ionic/Cordova
      this.checkGPS();
    }

    if (this.platform.is('ios')) {
      // This will only print when on iOS
      this.common.setOperatingSystem(OperatingSystem.iOS.valueOf());
      console.log('I am an iOS device!');
    } else if (this.platform.is('android')) {
      // This will only print when on Android
      this.common.setOperatingSystem(OperatingSystem.Android.valueOf());
      // this.storage.set('operatingSystem', OperatingSystem.Android);
      console.log('I am an Android device!');
    } else if (this.platform.is('windows')) {
      // This will only print when on Windows phone
      this.common.setOperatingSystem(OperatingSystem.WindowsPhone.valueOf());
      console.log('I am an Windows phone device!');
    } else if (this.platform.is('mobileweb')) {
      // This will only print when on Mobile web browser
      this.common.setOperatingSystem(OperatingSystem.Browser.valueOf());
      console.log('I am an Mobile web browser!');
    } else if (this.platform.is('core')) {
      // This will only print when on Desktop browser
      this.common.setOperatingSystem(OperatingSystem.Browser.valueOf());
      console.log('I am an Desktop browser!');
    }

    // this.common.getOperatingSystem();
  }

  enableMenu(loggedIn: boolean) {
    this.menu.enable(loggedIn, 'loggedInMenu');
    this.menu.enable(!loggedIn, 'loggedOutMenu');
  }

  listenToLoginEvents() {
    this.events.subscribe('user:login', () => {
      this.enableMenu(true);
    });

    this.events.subscribe('user:signup', () => {
      this.enableMenu(true);
    });

    this.events.subscribe('user:logout', () => {
      this.enableMenu(false);
    });
  }

  checkGPS() {
    let successCallback = (isAvailable) => {
      console.log('Is available? ' + isAvailable);
      if (isAvailable) {
        console.log('GPS idup coy');
      } else {
        console.log('GPS mati coy');
        let confirm = this.alertCtrl.create({
          title: '<b>Lokasi</b>',
          message: 'Informasi lokasi tidak tersedia. Hidupkan lokasi di pengaturan.',
          buttons: [

            {
              text: 'Ke Pengaturan',
              handler: () => {
                this.diagnostic.switchToLocationSettings();
              }
            }
          ]
        });
        confirm.present();
      }
    };
    let errorCallback = (error: any) => console.log("GPS error:", error);

    this.diagnostic.isLocationEnabled()
      .then(successCallback)
      .catch(errorCallback);
  }

}
