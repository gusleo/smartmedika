import { Component, OnInit, ViewChild, EventEmitter, 
         ElementRef, Renderer, TemplateRef }                from '@angular/core';
import { Router }                                           from '@angular/router';
import { MdDialog, MdDialogRef, MdDialogConfig }            from '@angular/material';
import { MdSnackBar, MdSnackBarConfig }                     from '@angular/material';
import { DialogConfig } from '../../../';
import { BlogCategoryModel, PaginationEntity } from '../../../model';
import { BlogService } from '../../../services';
import { RegisterCategoryDialog } from '../register/register-category.dialog';

@Component({
  selector: 'app-list-category-blog',
  templateUrl: './list-category.component.html',
  styleUrls: ['./list-category.component.scss']
})

export class ListCategoryComponent implements OnInit {
    results: BlogCategoryModel[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    numberOfpage: number = 0; 
    autoHide = 3000;  
    registerCategoryDialog: MdDialogRef<RegisterCategoryDialog>;

    constructor(private router: Router, public blogService:BlogService, public dialog: MdDialog, public snackBar: MdSnackBar) {
        //this.getAllCategory();
    }

    ngOnInit() {
      this.getAllCategory();
    }    

    async getAllCategory() {
        let res:PaginationEntity<BlogCategoryModel> = await this.blogService.getBlogCategory(this.page + 1, this.PAGE_SIZE).toPromise();
        this.results = res.items;
        //this.page = res.page;
        this.total = res.totalCount;
    }

    onPage(event) {
      this.page = event.offset;
      this.getAllCategory();
      
    }    
    	
    openRegisterDialog(cat?:BlogCategoryModel) {
        let value:BlogCategoryModel
        if(cat == undefined){
          value = new BlogCategoryModel();
        }else{
          value = Object.assign(new BlogCategoryModel(), cat);
        }

        var messageConfig = new MdSnackBarConfig();
        messageConfig.duration = this.autoHide;

        let config:any = DialogConfig;
        config.data = {     
          category: value
        }
    
        this.registerCategoryDialog = this.dialog.open(RegisterCategoryDialog, config);
        this.registerCategoryDialog.afterClosed().subscribe(result => {
          if(result != undefined){       
            if(value.id == 0){
              this.snackBar.open("Berhasil menambahkan data", "", messageConfig);
            }else{
                this.snackBar.open("Berhasil mengedit data", "", messageConfig);      
            }
            this.getAllCategory();
          }
          this.registerCategoryDialog = null;
        });
      } 

      async archiveCategory(cat:BlogCategoryModel){
        var messageConfig = new MdSnackBarConfig();
        messageConfig.duration = this.autoHide;        
        let data = Object.assign({}, cat);
        data.isVisible = !data.isVisible;        
        await this.blogService.categorySave(data).toPromise();
        this.snackBar.open("Berhasil merubah data", "", messageConfig);
        cat.isVisible = data.isVisible;
      }

}