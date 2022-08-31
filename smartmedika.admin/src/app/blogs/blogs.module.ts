import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { 
  MaterialModule,
  MdCardModule,
  MdIconModule,
  MdInputModule,
  MdRadioModule,
  MdButtonModule,
  MdProgressBarModule,
  MdToolbarModule  
 } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { BlogsRoutingModule } from './blogs-routing.module';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { QuillModule } from 'ngx-quill';

import { ListCategoryComponent } from './category/list/list-category.component';
import { RegisterCategoryDialog } from './category/register/register-category.dialog';
import { ListArticleComponent } from './article/list/list-article.component';
import { RegisterArticleComponent } from './article/register/register-article.component';
import { BlogService } from '../services';
import { Message} from '../../libs';


@NgModule({
  imports: [
    CommonModule,    
    BlogsRoutingModule,
    QuillModule,
    MaterialModule,
    MdCardModule,
    MdIconModule,
    MdInputModule,
    MdRadioModule,
    MdButtonModule,
    MdProgressBarModule,
    MdToolbarModule,    
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    NgxDatatableModule
  ],
  declarations: [ListCategoryComponent, RegisterCategoryDialog, ListArticleComponent, RegisterArticleComponent],
  providers: [
    BlogService, Message, DatePipe
  ]
})
export class BlogsModule { }
