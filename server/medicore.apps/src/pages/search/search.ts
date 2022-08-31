import { Component, Input } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { SearchSortPage } from '../search-sort/search-sort';
import { SearchFilterPage } from '../search-filter/search-filter';
import { SharePage } from '../share/share';
import * as Constant from '../../util/constants';
import { Commons } from '../../util/commons';
import { NearestDoctorOrHospitalModel, MedicalStaffModel, MedicalStaffSpecialistMapModel, MedicalStaffImageModel, HospitalModel, HospitalImageModel } from '../../model';
import { MedicalStaffService, HospitalService } from '../../providers';
import { AppointmentPage } from '../appointment/appointment';

/**
 * Generated class for the SearchPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-search',
  templateUrl: 'search.html',
  providers: [MedicalStaffService, HospitalService]
})
export class SearchPage {

  layanan: string = "Dokter";
  public clinicId;
  nearestDoctor: NearestDoctorOrHospitalModel;
  nearestClinic: NearestDoctorOrHospitalModel;
  nearestDoctorList: MedicalStaffModel[];
  nearestClinicList: HospitalModel[];
  @Input() events: any;
  constructor(public navCtrl: NavController, public navParams: NavParams, private common: Commons, private medicalStaffService: MedicalStaffService,
    private hospitalService: HospitalService) {
    this.clinicId = navParams.get("clinicId");
    console.log('clinicId', this.clinicId);
    this.getNearestDoctor();
    this.getNearestClinic();

    this.events = {
      'onItemClick': (item: any) => {
        console.log(item);
      },
      'onExplore': (item: any) => {
        console.log("Explore");
      },
      'onBooking': (item: any) => {
        this.navCtrl.push(AppointmentPage, {
          doctorDetails: item
        })
      },
      'onLike': (item: any) => {
        console.log("onLike");
      },
      'onFavorite': (item: any) => {
        console.log("onFavorite");
      },
      'onFab': (item: any) => {
        console.log("Fab");
      },
    };
  }

  onEvent(event: string, item: any, e: any) {
    if (e) {
      e.stopPropagation();
    }
    if (this.events[event]) {
      this.events[event](item);
    }
  }

  onClickSort() {
    this.navCtrl.push(SearchSortPage);
  }

  onClickFilter() {
    this.navCtrl.push(SearchFilterPage);
  }

  onClickShare() {
    this.navCtrl.push(SharePage);
  }

  displaySpecialistTitle(param: MedicalStaffSpecialistMapModel[]) {
    return this.common.displayDoctorSpecialistTitle(param);
  }

  displaySpecialistDescription(param: MedicalStaffSpecialistMapModel[]) {
    return this.common.displayDoctorSpecialistDescription(param);
  }

  displayAvatar(param: MedicalStaffImageModel[]) {
    return this.common.displayDoctorAvatar(param);
  }

  displayClinicAvatar(param: HospitalImageModel[]) {
    let item = '';
    param.forEach(element => {
      item = element.image.imageUrl;
    });

    return item;
  }

  // isClinicOpen(param: HospitalOperatingHoursModel) {
  //   if (param != null || param.length > 0) {
  //     return true;
  //   }

  //   return false;
  // }

  getNearestDoctor() {
    this.nearestDoctor = new NearestDoctorOrHospitalModel();
    this.nearestDoctor.latitude = this.common.latitude;
    this.nearestDoctor.longitude = this.common.longitude;
    this.nearestDoctor.polyClinicIds = [this.clinicId];
    this.nearestDoctor.clue = "";
    this.nearestDoctor.radius = Constant.RADIUS;
    this.nearestDoctor.pageIndex = 1;
    this.nearestDoctor.pageSize = Constant.PAGE_SIZE;

    this.common.showLoading('Mencari Dokter terdekat...', true);
    this.medicalStaffService.getNearestDoctor(this.nearestDoctor).subscribe(
      res => {
        console.log('Doctor data:', res.items);
        // this.common.hideLoading();
        this.nearestDoctorList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

  getNearestClinic() {
    this.nearestClinic = new NearestDoctorOrHospitalModel();
    this.nearestClinic.latitude = this.common.latitude;
    this.nearestClinic.longitude = this.common.longitude;
    this.nearestClinic.polyClinicIds = [this.clinicId];
    this.nearestClinic.clue = "";
    this.nearestClinic.radius = Constant.RADIUS;
    this.nearestClinic.pageIndex = 1;
    this.nearestClinic.pageSize = Constant.PAGE_SIZE;

    // this.common.showLoading('Mencari Dokter terdekat...', true);
    this.hospitalService.getHospitalList(this.nearestClinic).subscribe(
      res => {
        // console.log(res.items);
        // this.common.hideLoading();
        this.nearestClinicList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

}
