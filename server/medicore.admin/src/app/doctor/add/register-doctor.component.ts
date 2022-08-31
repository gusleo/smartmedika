import { Component, ViewChild, ElementRef, Renderer }        from '@angular/core';
import { SebmGoogleMap }                                     from 'angular2-google-maps/core';

@Component({
    templateUrl: 'register-doctor.component.html'
})
export class RegisterDoctorComponent {

	// start value for profile picture
    public picture:any =  '../assets/img/avatars/headshot.png';
    public uploaderOptions:any = {
		// url: 'http://website.com/upload'
	};	
    
    constructor(private renderer:Renderer) { }


      

}
