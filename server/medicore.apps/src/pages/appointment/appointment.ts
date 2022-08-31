import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { StaffService } from '../../providers';
import { Commons } from '../../util/commons';
import { MedicalStaffModel, MedicalStaffSpecialistMapModel, MedicalStaffImageModel, UserAppointmentViewModel, HospitalMedicalStaffModel } from '../../model';
import { AppointmentPatientPage } from '../appointment-patient/appointment-patient';

/**
 * Generated class for the AppointmentPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-appointment',
  templateUrl: 'appointment.html',
  providers: [StaffService]
})
export class AppointmentPage {

  shownGroup: any = null;
  public kalenderPetugas: boolean = true; //Whatever you want to initialise it as
  public jadwalPetugas: boolean = true;

  eventSource: any;
  viewTitle: any;
  isToday: boolean;
  calendar = {
    mode: 'month',
    currentDate: new Date()
  }; // these are the variable used by the calendar.

  doctorDetails: MedicalStaffModel;
  userAppointment: UserAppointmentViewModel;
  staffId: number;
  scheduleList: HospitalMedicalStaffModel[];
  selectedDate: Date;
  constructor(public navCtrl: NavController, public navParams: NavParams, public service: StaffService, private common: Commons) {
    this.doctorDetails = navParams.get('doctorDetails');
    this.staffId = this.doctorDetails.id;
    console.log('doctorDetails:', this.doctorDetails);

    let date = new Date();
    this.getDoctorOperatingHours(date);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad AppointmentPage');
  }

  public onButtonClick(id: any) {
    if (id === 0) {
      this.kalenderPetugas = !this.kalenderPetugas;
    } else {
      this.jadwalPetugas = !this.jadwalPetugas;
    }
  }

  getDateNow() {
    let tzoffset = (new Date()).getTimezoneOffset() * 60000;
    let localISOTime = (new Date(Date.now() - tzoffset)).toISOString().slice(0, -1);
    return localISOTime;
  }

  loadEvents() {
    this.eventSource = this.createRandomEvents();
  }
  onViewTitleChanged(title: any) {
    this.viewTitle = title;
  }
  onEventSelected(event: any) {
    console.log('Event selected:' + event.startTime + '-' + event.endTime + ',' + event.title);
  }
  changeMode(mode: any) {
    this.calendar.mode = mode;
  }
  today() {
    this.calendar.currentDate = new Date();
  }
  onTimeSelected(ev: any) {
    console.log('Selected time: ' + ev.selectedTime + ', hasEvents: ' +
      (ev.events !== undefined && ev.events.length !== 0) + ', disabled: ' + ev.disabled);
   this.selectedDate = ev.selectedTime;
   this.getDoctorOperatingHours(this.selectedDate);
  }
  onCurrentDateChanged(event: Date) {
    var today = new Date();
    today.setHours(0, 0, 0, 0);
    event.setHours(0, 0, 0, 0);
    this.isToday = today.getTime() === event.getTime();
  }
  createRandomEvents() {
    let events: any = [];
    for (var i = 0; i < 50; i += 1) {
      var date = new Date();
      var eventType = Math.floor(Math.random() * 2);
      var startDay = Math.floor(Math.random() * 90) - 45;
      var endDay = Math.floor(Math.random() * 2) + startDay;
      let startTime: any;
      let endTime: any;
      if (eventType === 0) {
        startTime = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate() + startDay));
        if (endDay === startDay) {
          endDay += 1;
        }
        endTime = new Date(Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate() + endDay));
        events.push({
          title: 'All Day - ' + i,
          startTime: startTime,
          endTime: endTime,
          allDay: true
        });
      } else {
        var startMinute = Math.floor(Math.random() * 24 * 60);
        var endMinute = Math.floor(Math.random() * 180) + startMinute;
        startTime = new Date(date.getFullYear(), date.getMonth(), date.getDate() + startDay, 0, date.getMinutes() + startMinute);
        endTime = new Date(date.getFullYear(), date.getMonth(), date.getDate() + endDay, 0, date.getMinutes() + endMinute);
        events.push({
          title: 'Event - ' + i,
          startTime: startTime,
          endTime: endTime,
          allDay: false
        });
      }
    }
    return events;
  }

  onRangeChanged(ev: any) {
    console.log('range changed: startTime: ' + ev.startTime + ', endTime: ' + ev.endTime);
  }

  markDisabled = (date: Date) => {
    var current = new Date();
    current.setHours(0, 0, 0);
    return date < current;
  };

  onBooking(hospitalMedicalStaff: HospitalMedicalStaffModel, selDate: Date) {
    this.navCtrl.push(AppointmentPatientPage, {
      hospitalMedStaff: hospitalMedicalStaff,
      selectedDate: selDate,
      medicalStaffId: this.staffId
    });

    console.log('hospitalMedicalStaff', hospitalMedicalStaff);
    console.log('selectedDate', selDate);
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

  getDoctorOperatingHours(date: Date) {
    // let date = new Date();
    let dayOfWeek = date.getDay();
    this.selectedDate =  date;
    console.log('date', date);
    console.log('dayOfWeek', dayOfWeek);
    this.userAppointment = new UserAppointmentViewModel();
    this.userAppointment.id = this.staffId;
    console.log('staffId', this.userAppointment.id);
    this.common.showLoading('Mencari jadwal praktek...', true);
    this.service.getDoctorOperatingHoursByDayOfWeek(this.userAppointment, dayOfWeek).subscribe(
      res => {
        console.log('jadwal dokter', res.items);
        this.common.hideLoading();
        this.scheduleList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

}
