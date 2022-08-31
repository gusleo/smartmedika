import { Component, Input } from '@angular/core';
import { PatientModel } from '../../../model';

@Component({
    selector: 'other-layout-1',
    templateUrl: 'other.html'
})

export class OtherLayout1 {
  @Input() data: any;
  @Input() events: any;

  patient: PatientModel = new PatientModel();
  constructor() {}

  onEvent = (event: string): void => {
    if (this.events[event]) {
        this.events[event]({
            'patientName' : this.patient.patientName,
            'gender': this.patient.gender,
            'dateOfBirth': this.patient.dateOfBirth,
            'relationshipStatus': this.patient.relationshipStatus
        });
    }
  }
}
