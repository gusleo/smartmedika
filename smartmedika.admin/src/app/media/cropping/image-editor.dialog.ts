import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import {ImageCropperComponent, CropperSettings, Bounds} from 'ng2-img-cropper';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'cropping-image',
  templateUrl: './image-editor.dialog.html',
  styleUrls: ['./image-editor.dialog.scss']
})
export class ImageEditorDialog implements OnInit, AfterViewInit {
  imgData:any;
  cropperSettings:CropperSettings;
  base64:string;

  @ViewChild('cropper', undefined) cropper:ImageCropperComponent;

  constructor(public dialogRef: MdDialogRef<ImageEditorDialog>, @Inject(MD_DIALOG_DATA) public data: any) {
      if(data != undefined)
        this.base64 = data.base64;

      this.setCroppedSetting();
      this.imgData = {};
   }


  ngOnInit() {

  }
   ngAfterViewInit() {
        this.fillImageComponent(this.base64);
    }

  setCroppedSetting(){
    this.cropperSettings = new CropperSettings();
    this.cropperSettings.width = 200;
    this.cropperSettings.height = 200;
    this.cropperSettings.keepAspect = true;

    this.cropperSettings.croppedWidth = 200;
    this.cropperSettings.croppedHeight = 200;

    this.cropperSettings.canvasWidth = 500;
    this.cropperSettings.canvasHeight = 300;

    this.cropperSettings.minWidth = 100;
    this.cropperSettings.minHeight = 100;

    this.cropperSettings.rounded = false;
    this.cropperSettings.minWithRelativeToResolution = false;

    this.cropperSettings.cropperDrawSettings.strokeColor = 'rgba(255,255,255,1)';
    this.cropperSettings.cropperDrawSettings.strokeWidth = 2;
    this.cropperSettings.noFileInput = true;
  }

  fillImageComponent(base64:string){
    var image:any = new Image();
    image.src = base64;
    this.cropper.setImage(image);
    this.base64 = base64;
  }

  cropped(bounds:Bounds) {
      //console.log(bounds);
  }

  /**
   * Used to send image to second cropper
   * @param $event
   */
  fileChangeListener($event) {        
      var file:File = $event.target.files[0];
      var myReader:FileReader = new FileReader();
      var that = this;
      myReader.onloadend = function (loadEvent:any) {            
          that.fillImageComponent(loadEvent.target.result);
      };

      myReader.readAsDataURL(file);
  }
    
  close(){
      this.dialogRef.close();
  }
  crop(){
      this.dialogRef.close({
          base64: this.imgData.image
      });
  }

}
