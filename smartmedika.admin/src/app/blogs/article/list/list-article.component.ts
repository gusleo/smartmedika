import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ClinicService } from '../../../services';
import { BlogCategoryModel, BlogModel, PaginationEntity } from '../../../model';
import { EnumValues } from 'enum-values';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { BlogService } from '../../../services';
import { DATETIME_FORMAT } from "../../../app.config";
import { DialogsService } from '../../../shared/popup/dialogs.service';


@Component({
  selector: 'app-list-blog-article',
  templateUrl: './list-article.component.html',
  styleUrls: ['./list-article.component.scss']
})
export class ListArticleComponent implements OnInit {
    results: BlogModel[];
    page:number = 0;
    PAGE_SIZE:number = 20; 
    total:number = 0;
    numberOfpage: number = 0; 
    
    constructor(private blogService:BlogService, private datePipe:DatePipe, public snackBar: MdSnackBar, private dialog:DialogsService){}

    ngOnInit() {
        this.getAll();
    }

    async getAll() {
        let res = await this.blogService.getBlog((this.page + 1), this.PAGE_SIZE).toPromise();
        this.results = res.items;
        this.total = res.totalCount;
        this.numberOfpage = Math.max(this.page * this.PAGE_SIZE, 0);
    }
    displayDate(date:Date){
        let res = this.datePipe.transform(date, DATETIME_FORMAT);
        return res;
    }
    confirm(id:number){
        var deletedId = id;
        this.dialog
            .confirm('Confirm Dialog', 'Apakah anda yakin menghapus artikel ini')
            .subscribe(res => {
                if(res){
                    this.delete(deletedId);
                }
            });
        
    }
    publish(isPublish:boolean, id:number){
        if(isPublish){
            var deletedId = id;
            this.dialog
                .confirm('Confirm Dialog', 'Apakah anda yakin mempublish artikel ini')
                .subscribe(res => {
                    if(res){
                        this.publishBlog(deletedId, true);
                    }
                });
        }else{
            var deletedId = id;
            this.dialog
                .confirm('Confirm Dialog', 'Apakah anda yakin men-unpublish artikel ini')
                .subscribe(res => {
                    if(res){
                        this.publishBlog(deletedId, false);
                    }
                });
        }
    }

    async delete(id:number){
        await this.blogService.blogDelete(id).toPromise();
        this.infoWindow("Artikel berhasil dihapus.");
        this.getAll();
    }
    async publishBlog(id:number, isPublish){
        await this.blogService.blogPublish(id, isPublish).toPromise();
        if(isPublish)
            this.infoWindow("Artikel berhasil dipublish.");
        else
            this.infoWindow("Artikel berhasil di-unpublish.");

            this.getAll();
    }

    onPage(event) {
        this.page = event.offset;
        this.getAll();
        
    }    

    infoWindow(message:string) {
        const config = new MdSnackBarConfig();
        config.duration = 3000;
        config.extraClasses = null;
        this.snackBar.open(message, "", config);
      }
    
}