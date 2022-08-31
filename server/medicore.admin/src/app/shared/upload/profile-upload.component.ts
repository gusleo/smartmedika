import { Component, ViewChild, Input, Output, EventEmitter, ElementRef, Renderer } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'profile-picture-uploader',
  template: require('./profile-upload.component.html')
})
export class ProfileUploadComponent {

    @Input() picture:string = '';
    @Input() uploaderOptions:any = {};
    @Output() otImage = new EventEmitter();
    
    onUpload:EventEmitter<any> = new EventEmitter();
    onUploadCompleted:EventEmitter<any> = new EventEmitter();

    @ViewChild('fileProfileUpload') protected _fileUpload:ElementRef;

    public uploadInProgress:boolean = false;
    public _uploader:FileUploader = new FileUploader({url: this.uploaderOptions});
    public camera:any =  '../assets/img/camera.png';


    constructor(private renderer:Renderer) {}
    
    public bringFileSelector():boolean {
        
        this.renderer.invokeElementMethod(this._fileUpload.nativeElement, 'click');
        return false;
        
    }

    public onFiles(data):void {
        let files = this._fileUpload.nativeElement.files;

        if (files.length) {
        const file = files[0];
        this._changePicture(file);

            if (this._canUploadOnServer()) {
                this.otImage.emit(file);
                //this._uploader.addFilesToQueue(files);
            }
            //console.log(files);
        }           
    }        

    protected _changePicture(file:File):void {
        const reader = new FileReader();
        reader.addEventListener('load', (event:Event) => {
            this.picture = (<any> event.target).result;
        }, false);
        reader.readAsDataURL(file);
    }

    protected _canUploadOnServer():boolean {
        return !!this.uploaderOptions['url'];
    }
}