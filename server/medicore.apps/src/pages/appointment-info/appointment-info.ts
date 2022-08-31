import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { AppointmentService } from '../../providers';
import { UserAppointmentViewModel, HospitalAppointmentModel, HospitalModel, MedicalStaffModel, MedicalStaffSpecialistModel, MedicalStaffSpecialistMapModel } from '../../model';
import { Commons } from '../../util/commons';

/**
 * Generated class for the AppointmentInfoPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-appointment-info',
  templateUrl: 'appointment-info.html',
  providers: [AppointmentService]
})
export class AppointmentInfoPage {

  userAppointment: UserAppointmentViewModel;
  hospitalAppointment: HospitalAppointmentModel = new HospitalAppointmentModel();
  hospital: HospitalModel = new HospitalModel();
  medicalStaff: MedicalStaffModel = new MedicalStaffModel();
  medicalStaffSpecialist: MedicalStaffSpecialistMapModel =  new MedicalStaffSpecialistMapModel();
  medStaffSpecialits: MedicalStaffSpecialistModel = new MedicalStaffSpecialistModel();
  medicalStaffTitle: string;
  constructor(public navCtrl: NavController, public navParams: NavParams, public service: AppointmentService, private common: Commons) {
    this.userAppointment = navParams.get("userAppointment");
    console.log('user appointment', this.userAppointment);
    this.bookAnAppointment(this.userAppointment);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad AppointmentInfoPage');
  }

  bookAnAppointment(appointment: UserAppointmentViewModel) {
    let date = new Date();
    let dayOfWeek = date.getDay();
    console.log('onBooking', dayOfWeek);
    // console.log('onSave:' + JSON.stringify(params));
    this.common.showLoading('Memesan tempat...', true);
    this.service.postAnAppointment(appointment).subscribe(
      res => {
        this.common.hideLoading();
        this.hospitalAppointment = res;
        this.hospital = res.hospital;
        this.medicalStaff = res.medicalStaff;
        this.medicalStaffSpecialist = res.medicalStaff.medicalStaffSpecialists;
        this.medicalStaffTitle = res.medicalStaff.medicalStaffSpecialists[0].medicalStaffSpecialist.name;

        // let stringify = JSON.stringify(res);
        // let parse = JSON.parse(stringify);
        // this.hospitalAppointment = parse;
        // console.log('json', this.hospitalAppointment);
      }, error => {
        console.error(error);
      }
    )
  }

  displaySpecialistTitle(param: MedicalStaffSpecialistMapModel[]) {
    return this.common.displayDoctorSpecialistTitle(param);
  }

}
