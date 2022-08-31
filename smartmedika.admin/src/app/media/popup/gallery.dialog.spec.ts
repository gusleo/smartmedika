import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GalleryDialog } from './gallery.dialog';

describe('GalleryDialog', () => {
  let component: GalleryDialog;
  let fixture: ComponentFixture<GalleryDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GalleryDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GalleryDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
