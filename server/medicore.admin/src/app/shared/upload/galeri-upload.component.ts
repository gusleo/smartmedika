import { Component, ViewChild, Input, Output, EventEmitter, ElementRef, Renderer } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'galeri-picture-uploader',
  styles: [require('./galeri-upload.component.scss')],
  template: require('./galeri-upload.component.html')
})
export class GaleriUploadComponent {

  @Input() defaultGaleri:string = '';
  @Input() galeriPhoto:string = '';

  @Input() uploaderURL:any = {};
  @Input() canDelete:boolean = true;
  @Output() otGalery = new EventEmitter();

  //@Input() URL: string = "";

  onUpload:EventEmitter<any> = new EventEmitter();
  onUploadCompleted:EventEmitter<any> = new EventEmitter();

  @ViewChild('fileUpload') protected _fileUpload:ElementRef;

  public uploadInProgress:boolean = false;
  public _uploader:FileUploader = new FileUploader({url: this.uploaderURL});

  constructor(private renderer:Renderer) {}

  public ngOnInit():void {
    if (this._canUploadOnServer()) {
      setTimeout(() => {
        this._uploader;
        //this._uploader.setOptions(this.uploaderURL);
      });

      //this._uploader._emitter.subscribe((data) => {
        //this._onUpload(data);
      //});
    } else {
      console.warn('Please specify url parameter to be able to upload the file on the back-end');
    }
  }

  public onFiles():void {
    let files = this._fileUpload.nativeElement.files;

    if (files.length) {
      const file = files[0];
      this._changePicture(file);

      if (this._canUploadOnServer()) {
        this.uploadInProgress = false;
        //this._uploader;
        this.otGalery.emit(file);
        //this._uploader.addFilesToQueue(files);
      }
    }
  }

  public bringFileSelector():boolean {
    this.renderer.invokeElementMethod(this._fileUpload.nativeElement, 'click');
    return false;
  }

  public removePicture():boolean {
    this.galeriPhoto = '';
    return false;
  }

  protected _changePicture(file:File):void {
    const reader = new FileReader();
    reader.addEventListener('load', (event:Event) => {
      this.galeriPhoto = (<any> event.target).result;
    }, false);
    reader.readAsDataURL(file);
  }

  protected _onUpload(data):void {
    if (data['done'] || data['abort'] || data['error']) {
      this._onUploadCompleted(data);
    } else {
      this.onUpload.emit(data);
    }
  }

  protected _onUploadCompleted(data):void {
    this.uploadInProgress = false;
    this.onUploadCompleted.emit(data);
  }

  protected _canUploadOnServer():boolean {
    return !!this.uploaderURL['url'];
  }
}
