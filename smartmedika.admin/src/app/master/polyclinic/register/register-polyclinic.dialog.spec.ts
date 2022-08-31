import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterPolyClinicDialog } from './register-polyclinic.dialog';

describe('RegionRegisterDialog', () => {
  let component: RegisterPolyClinicDialog;
  let fixture: ComponentFixture<RegisterPolyClinicDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisterPolyClinicDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterPolyClinicDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
