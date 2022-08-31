import { NavController, Platform, AlertController, Alert, NavParams } from 'ionic-angular';
import { SearchDoctorPage } from '../search-doctor/search-doctor';
import { NotificationPage } from '../notification/notification';
import { Commons } from '../../util/commons';
import { Component, Input } from '@angular/core';
import { AdvertisingService, MedicalStaffService, FirebaseUserMapService } from '../../providers';
import * as Constant from '../../util/constants';
import { NearestDoctorOrHospitalModel, FirebaseUserMapModel } from '../../model';

/**
 * Generated class for the HomepagePage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-homepage',
  templateUrl: 'homepage.html',
  providers: [AdvertisingService, MedicalStaffService, FirebaseUserMapService]
})
export class HomepagePage {

  params: any = {}
  public unregisterBackButtonAction: any;
  alert: Alert;
  serviceList: any = {};
  advertisementList: any;
  @Input() events: any;
  latitude:any;
  longitude:any;
  nearestDoctor:NearestDoctorOrHospitalModel;
  firebaseUserMap:FirebaseUserMapModel;
  deviceId: string;
  deviceToken: string;
  operatingSystem: number;

  constructor(public navCtrl: NavController, private platform: Platform, private alertCtrl: AlertController
    , public common: Commons,
    public service: AdvertisingService, public navParams: NavParams,
    public medicalStaffService: MedicalStaffService, private firebaseUserMapService: FirebaseUserMapService) {

    // get promise of latitude and longitude before executed on tab1
    this.common.getLatitude();
    this.common.getLongitude();

    // get the token in async await way
    this.common.getToken();

    // from service
    if (navigator.onLine === false) {
      this.common.showNoNetworkMessage();
    } else {
      this.common.showLoading('Sedang mengambil informasi penting...', true);
      this.getAdvertisement();
    }

    this.serviceList = [
      {
        id: Constant.ID_PEDIATRICIAN.toString(),
        url: 'assets/images/avatar/home-02.png',
        title: 'DOKTER ANAK',
        description: 'Atasi segera penyakit anak Anda segera dengan dokter yang tepat'
      },
      {
        id: Constant.ID_OBGYN.toString(),
        url: 'assets/images/avatar/home-03.png',
        title: 'DOKTER KANDUNGAN',
        description: 'Konsultasi mengenai kandungan dengan dokter yang bijak'
      },
      {
        id: '20',
        url: 'assets/images/avatar/home-04.png',
        title: 'DIREKTORI KLINIK',
        description: 'Daftar Klinik terdekat dari lokasi Anda'
      },
    ]
  }

  getAdvertisement() {
    this.service.getAllAdvertisement().subscribe(
      res => {
        // this.common.hideLoading();
        if(res.length > 0) {
          this.advertisementList = res;
        } else {
          this.loadDefaultAdImages();
        }
      }, error => {
        console.error(error);
        this.loadDefaultAdImages();
      }
    )
  }

  loadDefaultAdImages() {
    this.advertisementList = [
      {
        "image": {
          "imageUrl": 'assets/images/background/a01.jpg'
        }
      },
      {
        "image": {
          "imageUrl": 'assets/images/background/a02.jpg'
        }
      },
      {
        "image": {
          "imageUrl": 'assets/images/background/a03.jpg'
        }
      },
    ]
  }

  ionViewDidEnter() {
    // doing firebase user map, these three below, please keep in order
    this.getDeviceId();
    this.getDeviceToken();
    this.getOperatingSystem();
  }

  getFireBaseUserMap() {
    this.firebaseUserMap = new FirebaseUserMapModel();
    this.firebaseUserMap.deviceId = this.deviceId;
    this.firebaseUserMap.deviceToken = this.deviceToken;
    this.firebaseUserMap.operatingSystem = this.operatingSystem;
    console.log('Firebase user map model:', this.firebaseUserMap);
    this.firebaseUserMapService.signupFirebase(this.firebaseUserMap).subscribe(
      res => {
        console.log("Firebase user map sent!");
        // this.common.hideLoading();
      }, error => {
        console.error(error);
      }
    )
  }

  async getDeviceId() {
    const val = await this.common.getDeviceId();
    this.deviceId = val;
  }

  async getDeviceToken() {
    const val = await this.common.getDeviceToken();
    this.deviceToken = val;
  }

  async getOperatingSystem() {
    const os = await this.common.getOperatingSystem();
    this.operatingSystem = os;

    this.getFireBaseUserMap();
  }

  onClick(id: any) {
    if (id === Constant.ID_PEDIATRICIAN.toString()) {
      this.navCtrl.push(SearchDoctorPage, {
        clinicId: id
      })
    } else if (id === Constant.ID_OBGYN.toString()) {
      this.navCtrl.push(SearchDoctorPage, {
        clinicId: id
      })
    } else {
      console.log("to be determined");
    }
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad HomepagePage');
  }

  onClickSearch() {
    this.navCtrl.push(SearchDoctorPage);
  }

  onClickNotifications() {
    this.navCtrl.push(NotificationPage);
  }

  public initializeBackButtonCustomHandler(): void {
    this.unregisterBackButtonAction = this.platform.registerBackButtonAction(() => {
      this.customHandleBackButton();
    }, 10);
  }

  private customHandleBackButton(): void {
    // do what you need to do here ...
    if (this.alert) {
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
