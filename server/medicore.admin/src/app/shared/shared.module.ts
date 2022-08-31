import { NgModule }                             from '@angular/core';
import { CommonModule }                         from '@angular/common';

import { HttpModule }                           from '@angular/http';
import { FormsModule, ReactiveFormsModule }                          from '@angular/forms';

//Upload Galeri Component
import { GaleriUploadComponent }                from '../shared/upload/galeri-upload.component';
//Upload Profile picture Component
import { ProfileUploadComponent }               from '../shared/upload/profile-upload.component';
//Tinymce Component
import { MyTinyComponent }                      from '../shared/tiny-mce.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        GaleriUploadComponent,
        ProfileUploadComponent,
        MyTinyComponent
    ],
    exports: [
        GaleriUploadComponent,
        ProfileUploadComponent,
        MyTinyComponent,
        HttpModule,
        CommonModule,        
        FormsModule,
        ReactiveFormsModule
    ]
})
export class SharedModule {}