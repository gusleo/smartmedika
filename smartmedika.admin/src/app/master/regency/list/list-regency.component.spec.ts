import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegencyListComponent } from './list-regency.component';

describe('RegencyComponent', () => {
  let component: RegencyListComponent;
  let fixture: ComponentFixture<RegencyListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RegencyListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegencyListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
