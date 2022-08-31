import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import "reflect-metadata";
import "es6-shim"
import { Commons } from '../util/commons';
import { IonicStorageModule } from '@ionic/storage';
import { NgCalendarModule  } from 'ionic2-calendar';
import { Geolocation } from '@ionic-native/geolocation';
import { AuthService } from '../providers';
import { UniqueDeviceID } from '@ionic-native/unique-device-id';
import { UserData } from '../providers/user-data';
import { RequestOptions, XHRBackend, Http } from '@angular/http';
import { HttpService } from '../providers/http.service';
import { Diagnostic } from '@ionic-native/diagnostic';

// Components
import { MyApp } from './app.component';
import { SplashScreenLayout1 } from '../components/splash-screen/layout-1/splash-screen-layout-1';
import { LoginLayout2 } from '../components/login/layout-2/login-layout-2';
import { WizardLayout1 } from '../components/wizard/layout-1/wizard-layout-1';
import { RegisterLayout2 } from '../components/register/layout-2/register-layout-2';
import { TabsLayout2 } from '../components/tabs/layout-2/tabs-layout-2';
import { GoogleCardLayout1 } from '../components/list-view/google-card/layout-1/google-card-layout-1';
import { SearchBarLayout1 } from '../components/search-bar/layout-1/search-bar-layout-1';
import { LoginLayout3 } from '../components/login/layout-3/login-layout-3';
// For patient create
import { OtherLayout1 } from '../components/other/layout-1/other-layout-1';

// Pages
import { HomePage } from '../pages/home/home';
import { ListPage } from '../pages/list/list';
import { SearchPage } from '../pages/search/search';
import { NotificationPage } from '../pages/notification/notification';
import { Splash } from '../pages/splash/splash';
import { LoginPage } from '../pages/login/login';
import { WizardPage } from '../pages/wizard/wizard';
import { RegisterPage } from '../pages/register/register';
import { TabPage1 } from '../pages/tab-page-1/tab-page-1';
import { TabPage2 } from '../pages/tab-page-2/tab-page-2';
import { TabPage3 } from '../pages/tab-page-3/tab-page-3';
import { CardPage } from '../pages/card/card';
import { RateAppPage } from '../pages/rate-app/rate-app';
import { InviteFriendAndFamilyPage } from '../pages/invite-friend-and-family/invite-friend-and-family';
import { SettingsPage } from '../pages/settings/settings';
import { AboutPage } from '../pages/about/about';
import { TestingPage } from '../pages/testing/testing';
import { LoginVerificationPage } from '../pages/login-verification/login-verification';
import { LoginWithEmailPage } from '../pages/login-with-email/login-with-email';
import { PatientCreatePage } from '../pages/patient-create/patient-create';
import { PatientListPage } from '../pages/patient-list/patient-list';
import { AppointmentPage } from '../pages/appointment/appointment';
import { ClinicDetailPage } from '../pages/clinic-detail/clinic-detail';
import { DoctorDetailPage } from '../pages/doctor-detail/doctor-detail';
import { NoNetworkPage } from '../pages/no-network/no-network';
import { SearchSortPage } from '../pages/search-sort/search-sort';
import { SearchFilterPage } from '../pages/search-filter/search-filter';
import { SharePage } from '../pages/share/share';
import { PatientDetailPage } from '../pages/patient-detail/patient-detail';
import { ArticleDetailPage } from '../pages/article-detail/article-detail';
import { NotificationDetailPage } from '../pages/notification-detail/notification-detail';
import { AppointmentPatientPage } from '../pages/appointment-patient/appointment-patient';
import { AppointmentInfoPage } from '../pages/appointment-info/appointment-info';
import { HomepagePage } from '../pages/homepage/homepage';
import { SearchDoctorPage } from '../pages/search-doctor/search-doctor';

// Firebase
import { HttpModule } from '@angular/http';
import { AngularFireModule } from 'angularfire2';
import { AngularFireDatabaseModule } from 'angularfire2/database';
import { AngularFireAuthModule } from 'angularfire2/auth';
import { FirebaseServiceProvider } from '../providers/firebase-service/firebase-service';

import { CloudSettings, CloudModule } from '@ionic/cloud-angular';
const cloudSettings: CloudSettings = {
  'core': {
    'app_id': 'c4829f3b'
  },
  'push': {
    'sender_id': '118905408746',
    'pluginConfig': {
      'ios': {
        'badge': true,
        'sound': true
      },
      'android': {
        'iconColor': '#343434'
      }
    }
  }
};

export const firebaseConfig = {
  apiKey: "AIzaSyAxX0IsVF1VdIiYDFxVsENkj1XXfXMJPS8",
  authDomain: "ionicsmartmedika.firebaseapp.com",
  databaseURL: "https://ionicsmartmedika.firebaseio.com",
  projectId: "ionicsmartmedika",
  storageBucket: "ionicsmartmedika.appspot.com",
  messagingSenderId: "118905408746"
};

@NgModule({
  declarations: [
    MyApp,
    HomePage,
    ListPage,
    SplashScreenLayout1,
    Splash,
    LoginLayout2,
    LoginPage,
    WizardLayout1,
    WizardPage,
    RegisterLayout2,
    RegisterPage,
    TabsLayout2,
    TabPage1,
    TabPage2,
    TabPage3,
    GoogleCardLayout1,
    CardPage,
    SearchPage,
    NotificationPage,
    SearchBarLayout1,
    RateAppPage,
    InviteFriendAndFamilyPage,
    SettingsPage,
    AboutPage,
    TestingPage,
    LoginVerificationPage,
    LoginWithEmailPage,
    PatientCreatePage,
    PatientListPage,
    AppointmentPage,
    ClinicDetailPage,
    DoctorDetailPage,
    NoNetworkPage,
    LoginLayout3,
    OtherLayout1,
    SearchSortPage,
    SearchFilterPage,
    SharePage,
    PatientDetailPage,
    ArticleDetailPage,
    NotificationDetailPage,
    AppointmentPatientPage,
    AppointmentInfoPage,
    HomepagePage,
    SearchDoctorPage
  ],
  imports: [
    BrowserModule,
    IonicModule.forRoot(MyApp),
    HttpModule,
    AngularFireModule.initializeApp(firebaseConfig), AngularFireDatabaseModule, AngularFireAuthModule,
    IonicStorageModule.forRoot(),
    NgCalendarModule,
    CloudModule.forRoot(cloudSettings)
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp,
    HomePage,
    ListPage,
    SplashScreenLayout1,
    Splash,
    LoginLayout2,
    LoginPage,
    WizardLayout1,
    WizardPage,
    RegisterLayout2,
    RegisterPage,
    TabsLayout2,
    TabPage1,
    TabPage2,
    TabPage3,
    GoogleCardLayout1,
    CardPage,
    SearchPage,
    NotificationPage,
    SearchBarLayout1,
    RateAppPage,
    InviteFriendAndFamilyPage,
    SettingsPage,
    AboutPage,
    TestingPage,
    LoginVerificationPage,
    LoginWithEmailPage,
    PatientCreatePage,
    PatientListPage,
    AppointmentPage,
    ClinicDetailPage,
    DoctorDetailPage,
    NoNetworkPage,
    LoginLayout3,
    OtherLayout1,
    SearchSortPage,
    SearchFilterPage,
    SharePage,
    PatientDetailPage,
    ArticleDetailPage,
    NotificationDetailPage,
    AppointmentPatientPage,
    AppointmentInfoPage,
    HomepagePage,
    SearchDoctorPage
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    FirebaseServiceProvider,
    Commons,
    Geolocation,
    AuthService,
    UniqueDeviceID,
    Diagnostic,
    UserData,
    {
      provide: Http,
      useFactory: (backend: XHRBackend, options: RequestOptions) => {
        return new HttpService(backend, options);
      },
      deps: [XHRBackend, RequestOptions]
    }
  ]
})
export class AppModule {}
