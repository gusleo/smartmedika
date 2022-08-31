import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageEditorDialog } from './image-editor.dialog';

describe('CroppingImageDialog', () => {
  let component: ImageEditorDialog;
  let fixture: ComponentFixture<ImageEditorDialog>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageEditorDialog ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageEditorDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
