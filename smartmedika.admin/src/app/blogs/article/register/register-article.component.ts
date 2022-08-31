import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { ClinicService } from '../../../services';
import { BlogCategoryModel, BlogModel } from '../../../model';
import { EnumValues } from 'enum-values';
import { BlogService } from '../../../services';
import { Message} from '../../../../libs';
import * as Quill from 'quill';

import { Observable } from 'rxjs/Observable';

import { QuillEditorComponent } from 'ngx-quill/src/quill-editor.component';

import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

// override p with div tag
const Parchment = Quill.import('parchment');
const Block = Parchment.query('block');

Block.tagName = 'DIV';
// or class NewBlock extends Block {}; NewBlock.tagName = 'DIV';
Quill.register(Block /* or NewBlock */, true);

import { Counter } from '../../../../libs';
Quill.register('modules/counter', Counter);

@Component({
  selector: 'app-register-blog',
  templateUrl: './register-article.component.html',
  styleUrls: ['./register-article.component.scss']
})
export class RegisterArticleComponent implements OnInit {

    public titleForm: string;
    public form: FormGroup;
    autoCategory: string;
    categoryCtrl: FormControl;
    reactCategory: any;
    resultCategory: BlogCategoryModel[];
    article: BlogModel =  new BlogModel();
    id:number;
    onRequest:boolean;
    currentCategory:BlogCategoryModel;
   
    @ViewChild('editor') editor: QuillEditorComponent;

    constructor(private formBuilder: FormBuilder, private router: Router, private route: ActivatedRoute, 
        private message:Message, private services:BlogService) {
       
        this.route.params.subscribe(params => {
            if(params['id']){
               this.id =  Number.parseInt(params['id']);
            }
             
          });

        this.resultCategory = new Array<BlogCategoryModel>();
        this.categoryCtrl = new FormControl({code: '', name: ''});
        this.reactCategory = this.categoryCtrl.valueChanges
            .startWith(this.categoryCtrl.value)              
            .map(val => this.displayFn(val))
            .map(name => this.filterCategoryList(name));          
    }

    ngOnInit() {
        this.getAllCategory();
        this.getCurrentArticle(this.id);

        this.form = this.formBuilder.group({
            title: [null, Validators.compose([Validators.required])],
            category: [''],
            shortDescription: [''],
            description:['']
            
        });        

        
    }

    async getAllCategory() {
        let result = await this.services.getBlogCategory(1, 10000).toPromise();
        this.resultCategory = result.items;
    }

    async getCurrentArticle(id:number){
        if(id == 0){
            this.article = new BlogModel();            
        }else{
            this.article = await this.services.getBlogDetail(id).toPromise(); 
            if(this.article.category != null){
                this.currentCategory = this.article.category;
            }           
        }
    }

    displayFn(value: any): string {
        return value && typeof value === 'object' ? value.name : value;
    }

    filterCategoryList(val: string) {
        return val ? this.resultCategory.filter((s) => s.name.match(new RegExp(val, 'gi'))) : this.resultCategory;
    }     
    
    async save() {
        
        this.article.categoryId = this.currentCategory.id;
     
        let message = this.id == 0 ? "Sukses menambah artikel" 
            : "Sukses merubah artikel"; 
    
        this.onRequest = true;
        await this.services.blogSave(this.article).toPromise();
    
        
        this.onRequest = false;
        this.message.open(message);
              
        
      }
      cancel(){
        this.router.navigate(['/blogs/article']);
      }
}