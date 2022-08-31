import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { MdDialog, MdDialogRef, MdDialogConfig, MD_DIALOG_DATA } from '@angular/material';
import { BlogCategoryModel } from '../../../model';

//config API & Grab everything with import model;
import { BlogService } from '../../../services';

@Component({
  selector: 'master-register-category-blog',
  templateUrl: './register-category.dialog.html',
  styleUrls: ['./register-category.dialog.scss']
})
export class RegisterCategoryDialog {

    category:BlogCategoryModel
    onRequest: boolean;
    form: FormGroup;       
    constructor(public dialogRef: MdDialogRef<RegisterCategoryDialog>, private service:BlogService, @Inject(MD_DIALOG_DATA) public data: any, private formBuilder: FormBuilder){
        this.category = data.category;   
        if(this.category == undefined)
            this.category = new BlogCategoryModel();
        
       this.form = this.formBuilder.group({
          nameMessage: [null, Validators.compose([Validators.required])]          
      });        
    }


    close(){
        this.dialogRef.close();
    }
    async save(){
        let response:any;
        this.onRequest = true;        
        response = await this.service.categorySave(this.category).toPromise();
        this.onRequest = false;
        this.dialogRef.close({
            category: response
        });

    }   
    

}