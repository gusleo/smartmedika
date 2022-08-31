import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegionRegisterDialog } from './region-register.dialog';

describe('RegionRegisterDialog', () => {
  let component: RegionRegisterDialog;
  let fixture: ComponentFixture<RegionRegisterDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegionRegisterDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegionRegisterDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
