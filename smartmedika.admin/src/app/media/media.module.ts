import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListMediaComponent } from './list-media/list-media.component';
import { AddMediaComponent } from './add-media/add-media.component';
import { GalleryDialog } from './popup/gallery.dialog';
import { ImageEditorDialog } from './cropping/image-editor.dialog';
import { ImageCropperComponent } from 'ng2-img-cropper';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [ListMediaComponent, AddMediaComponent, GalleryDialog, ImageEditorDialog]
})
export class MediaModule { }

