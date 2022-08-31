import { Component, Input } from '@angular/core';
import { Commons } from '../../util/commons';
import { AdvertisingService, MedicalStaffService, FirebaseUserMapService } from '../../providers';
import * as Constant from '../../util/constants';
import { SearchPage } from '../search/search';
import { NavController, NavParams } from 'ionic-angular';
import { NearestDoctorOrHospitalModel, FirebaseUserMapModel } from '../../model';

@Component({
  templateUrl: 'tab-page-1.html',
  providers: [AdvertisingService, MedicalStaffService, FirebaseUserMapService]
})

export class TabPage1 {
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

  constructor(public service: AdvertisingService, private common: Commons,
    public navCtrl: NavController, public navParams: NavParams,
    public medicalStaffService: MedicalStaffService, private firebaseUserMapService: FirebaseUserMapService) {
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
        this.advertisementList = res;
        // this.common.hideLoading();
      }, error => {
        console.error(error);
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
    )
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
      this.navCtrl.push(SearchPage, {
        clinicId: id
      })
    } else if (id === Constant.ID_OBGYN.toString()) {
      this.navCtrl.push(SearchPage, {
        clinicId: id
      })
    } else {
      console.log("to be determined");
    }
  }
}
