import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterRegencyDialog } from './regency-register.dialog';

describe('RegisterComponent', () => {
  let component: RegisterRegencyDialog;
  let fixture: ComponentFixture<RegisterRegencyDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegisterRegencyDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterRegencyDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
