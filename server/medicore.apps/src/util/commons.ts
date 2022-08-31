import { Injectable } from '@angular/core';
import { AlertController, LoadingController, Loading } from 'ionic-angular';
import { Storage } from '@ionic/storage';
import * as Constant from './constants';
import { MedicalStaffSpecialistMapModel, MedicalStaffImageModel } from '../model';

@Injectable()
export class Commons {
    loading: Loading;
    userToken: any;
    hasLoggedIn: boolean;
    latitude: number;
    longitude: number;
    refreshToken: any;
    username: any;
    constructor(private loadingCtrl: LoadingController,
        private alertCtrl: AlertController,
        public storage: Storage) {
        // this.isLoggedIn();
    }

    showLoading(message: string, withDuration: boolean) {
        if (withDuration) {
            this.loading = this.loadingCtrl.create({
                content: message,
                duration: 2500
            });
        } else {
            this.loading = this.loadingCtrl.create({
                content: message
            });
        }

        this.loading.present();
    }

    hideLoading() {
        this.loading.dismiss();
    }

    showError(text: string) {
        setTimeout(() => {
            this.loading.dismiss();
        });

        let alert = this.alertCtrl.create({
            title: 'Failed',
            subTitle: text,
            buttons: ['OK']
        });
        alert.present();
    }

    setUserToken(token: string) {
        this.storage.set(Constant.ACCESS_TOKEN, token);
    };

    // setUserLoginStatus() {
    //     this.storage.set(Constant.HAS_LOGGED_IN, true);
    // };

    // getUserToken() {
    //     return this.storage.get(Constant.ACCESS_TOKEN);
    // };

    getUserTokenStorage() {
        return this.storage.get(Constant.ACCESS_TOKEN);
    }

    async getUserToken() {
        const val =  await this.getUserTokenStorage();
        this.userToken = val;
    }

    setUserRefreshToken(token: string) {
        this.storage.set(Constant.REFRESH_TOKEN, token);
    };

    getUserRefreshTokenStorage() {
        return this.storage.get(Constant.REFRESH_TOKEN);
    }

    async getUserRefreshToken() {
        const val = await this.getUserRefreshTokenStorage();
        this.refreshToken = val;
    };

    getToken() {
        this.getUserToken();
        this.getUserRefreshToken();
    }

    async getFromStorageAsync(param: string) {
        return await this.storage.get(param);
    }

    // isLoggedIn() {
    //     this.storage.get(Constant.HAS_LOGGED_IN).then((value) => {
    //         return this.hasLoggedIn = value;
    //     });
    // };

    showNoNetworkMessage() {
        let alert = this.alertCtrl.create({
            title: 'Tidak ada jaringan',
            subTitle: 'Mohon periksa jaringan internet Anda.',
            buttons: ['OK']
        });
        alert.present();
    }

    showInfo(title:string, message: string) {
        let alert = this.alertCtrl.create({
            title: title,
            subTitle: message,
            buttons: ['OK']
        });
        alert.present();
    }

    setLatitude(latitude: number) {
        this.storage.set(Constant.LATITUDE, latitude);
    }

    getLatitude() {
        this.storage.get(Constant.LATITUDE).then((value) => {
            this.latitude = value;
        });
    }

    setLongitude(longitude: number) {
        this.storage.set(Constant.LONGITUDE, longitude);
    }

    getLongitude() {
        this.storage.get(Constant.LONGITUDE).then((value) => {
            this.longitude = value;
        });
    }

    setDeviceId(deviceId: number) {
        this.storage.set(Constant.DEVICE_ID, deviceId);
    }

    getDeviceId() {
        return this.storage.get(Constant.DEVICE_ID);
    }

    setDeviceToken(deviceToken: string) {
        this.storage.set(Constant.DEVICE_TOKEN, deviceToken);
    }

    getDeviceToken() {
        return this.storage.get(Constant.DEVICE_TOKEN);
    }

    setOperatingSystem(operatingSystem: number) {
        this.storage.set(Constant.DEVICE_OS, operatingSystem);
    }

    getOperatingSystem() {
        return this.storage.get(Constant.DEVICE_OS);
    }

    setUsername(username: number) {
        this.storage.set(Constant.USERNAME, username);
    }

    getUsername() {
        this.storage.get(Constant.USERNAME).then((value) => {
            this.username = value;
        });
    }

    displayDoctorSpecialistTitle(param: MedicalStaffSpecialistMapModel[]) {
        let item = '';
        param.forEach(element => {
          item += element.medicalStaffSpecialist.name + ", ";
        });
    
        return item.substring(0, item.length-2);
    }

    displayDoctorSpecialistDescription(param: MedicalStaffSpecialistMapModel[]) {
        let item = '';
        param.forEach(element => {
          item += element.medicalStaffSpecialist.alias + ", ";
        });
    
        return item.substring(0, item.length-2);
    }

    displayDoctorAvatar(param: MedicalStaffImageModel[]) {
        let item = '';
        param.forEach(element => {
          item = element.image.imageUrl;
        });

        if(!item || item.length === 0) {
            item = 'assets/images/logo/login-icon.png';
        }
    
        return item;
    }

}