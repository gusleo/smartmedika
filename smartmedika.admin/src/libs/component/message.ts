import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { Injectable } from '@angular/core';

@Injectable()
export class Message{
    constructor(private snackBar:MdSnackBar){

    }
    open(message:string, title?:string){
        const config = new MdSnackBarConfig();
        config.duration = 3000;
        config.extraClasses = null;
        this.snackBar.open(message, "", config);
    }
}