import { MdDialogRef } from '@angular/material';
import { Component } from '@angular/core';

@Component({
    selector: 'confirm-dialog',
    template: `
        <md-card-title><strong>{{ title }}</strong></md-card-title> 
        <md-card-content>
            <div class="mb-1">
            <p> {{ message }} ?</p>    
            </div>        
        </md-card-content>
        <md-card-actions>
            <div fxLayout="row" fxLayoutAlign="center center">
                <button type="button" md-raised-button md-button-sm class="mr-1" color="red" (click)="dialogRef.close()">Batal</button>
                <button type="button" md-raised-button md-button-sm class="mr-1" color="primary" (click)="dialogRef.close(true)">OK</button> 
            </div>           
        </md-card-actions> 
    `,
})
export class ConfirmDialog {

    public title: string;
    public message: string;

    constructor(public dialogRef: MdDialogRef<ConfirmDialog>) {

    }
}