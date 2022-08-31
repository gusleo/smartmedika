import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpModule, Http } from '@angular/http';

import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { DialogsModule } from './shared/popup/dialog.module';

import {
  MdInputModule,
  MdAutocompleteModule,
  MdSidenavModule,
  MdCardModule,
  MdMenuModule,
  MdCheckboxModule,
  MdIconModule,
  MdButtonModule,
  MdToolbarModule,
  MdTabsModule,
  MdListModule,
  MdSlideToggleModule,
  MdSelectModule, DateAdapter, 
  MD_DATE_FORMATS } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutes } from './app.routing';
import { AppComponent } from './app.component';
import { AdminLayoutComponent } from './layouts/admin/admin-layout.component';
import { AuthLayoutComponent } from './layouts/auth/auth-layout.component';
import { SharedModule } from './shared/shared.module';
import { ImageEditorDialog } from './media';
import { ImageCropperComponent } from 'ng2-img-cropper';
import { FileUploadModule } from 'ng2-file-upload/ng2-file-upload';

import { AuthGuardService, AuthService } from './services'
import { Config } from './';
import { AgmCoreModule } from '@agm/core';
import { AuthInterceptor, ClinicService, ApplicationService } from './services';
import { IndonesiaDateAdapter, IDN_DATE_FORMATS} from '../libs';

export function createTranslateLoader(http: HttpClient) {
 return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    AuthLayoutComponent, 
    ImageEditorDialog, 
    ImageCropperComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    SharedModule,
    RouterModule.forRoot(AppRoutes),
    MdAutocompleteModule,
    FormsModule,
    HttpClientModule,
    HttpModule,
    FileUploadModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    MdInputModule,
    MdSidenavModule,
    MdCardModule,
    MdMenuModule,
    MdCheckboxModule,
    MdIconModule,
    MdButtonModule,
    MdToolbarModule,
    MdTabsModule,
    MdListModule,
    MdSlideToggleModule,
    MdSelectModule,
    FlexLayoutModule,
    AgmCoreModule.forRoot({
      apiKey: Config.GoogleApiKey,
      libraries: ["places"]
    }),
    ReactiveFormsModule,
    DialogsModule
  ],
  providers: [ 
    AuthGuardService,
    AuthService,
    ClinicService,
    ApplicationService,
    {provide: DateAdapter, useClass: IndonesiaDateAdapter},
    {provide: MD_DATE_FORMATS, useValue: IDN_DATE_FORMATS},
    // register TokenInterceptor as HttpInterceptor
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
   }
    
    ],
  entryComponents: [ImageEditorDialog],
  bootstrap: [AppComponent]
})
export class AppModule { }
