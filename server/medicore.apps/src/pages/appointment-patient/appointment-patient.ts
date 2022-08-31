import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { Commons } from '../../util/commons';
import * as Constant from '../../util/constants';
import { PatientService, StaffService, AppointmentService } from '../../providers';
import { PatientModel, UserAppointmentViewModel, UserPatientAppointmentModel, HospitalMedicalStaffModel } from '../../model';
import { AppointmentInfoPage } from '../appointment-info/appointment-info';

/**
 * Generated class for the AppointmentPatientPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-appointment-patient',
  templateUrl: 'appointment-patient.html',
  providers: [PatientService, StaffService, AppointmentService]
})
export class AppointmentPatientPage {

  patientList: PatientModel[];
  dokter: any = {};
  userAppointment: UserAppointmentViewModel;
  patientProblem: UserPatientAppointmentModel[];
  // stringArr: string[];
  hospitalMedStaff: HospitalMedicalStaffModel[];
  selectedDate: Date;
  medStaffId: number;
  constructor(public navCtrl: NavController, public navParams: NavParams, private common: Commons, 
      private service: PatientService, public staffService: StaffService, public appointmentService: AppointmentService) {
    this.getAllPatient();
    this.hospitalMedStaff = navParams.get("hospitalMedStaff");
    this.selectedDate = navParams.get("selectedDate");
    this.medStaffId = navParams.get("medicalStaffId");
    console.log('hospital Medical Staff', this.hospitalMedStaff);
    console.log('selected date', this.selectedDate);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad AppointmentPatientPage');
  }

  getAllPatient() {
    let pageIndex = 1;
    let pageSize = Constant.PAGE_SIZE;
    this.common.showLoading('Mengunduh data pasien...', true);
    this.service.getAllPatient(pageIndex, pageSize).subscribe(
      res => {
        this.common.hideLoading();
        console.log(res.items);
        this.patientList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

  onChangeItem(id: any) {
    console.log('On change item:' + id);
  }

  toggleGroup(group: any) {
    group.show = !group.show;
    // console.log('group:', group);
  }

  isGroupShown(group: any) {
    return group.show;
  }

  onBookingClick(param: PatientModel[]) {
    // validate input
    for (let index = 0; index < param.length; index++) {
      let element = param[index];
      let isSelected = param.filter((element) => element.isChecked === true)
      if(isSelected.length <= 0) {
        this.common.showInfo('Pilih Pasien', 'Silahkan pilih Pasien');

        return false;
      } else if (element.patientProblem === null || element.patientProblem === '') {
        this.common.showInfo('Masukkan Keluhan', 'Keluhan harus diisi');
        
        return false;
      }
    }

    this.userAppointment = new UserAppointmentViewModel();
    
    this.hospitalMedStaff.forEach(element => {
      // get hospital id
      this.userAppointment.hospitalId = element.hospital.id;
    });

    this.userAppointment.medicalStaffId = this.medStaffId;

    // get appointment data
    this.userAppointment.appointmentDate = this.selectedDate;
    this.userAppointment.status = 0;

    this.userAppointment.patientProblems = new Array<UserPatientAppointmentModel>();

    for (let index = 0; index < param.length; index++) {
      let element = param[index];
      if (element.isChecked) {
        this.userAppointment.patientProblems.push({ "patientId": element.id, "problems":element.patientProblem});
      }
    }

    console.log('onBooking', this.userAppointment);
    this.bookAnAppointment(this.userAppointment);
  }

  bookAnAppointment(param: UserAppointmentViewModel) {
    this.navCtrl.push(AppointmentInfoPage, {
      userAppointment: param
    })
  }
 
}
