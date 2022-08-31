import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterSpecialistDialog } from './register-specialist.dialog';

describe('RegionRegisterDialog', () => {
  let component: RegisterSpecialistDialog;
  let fixture: ComponentFixture<RegisterSpecialistDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisterSpecialistDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterSpecialistDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
