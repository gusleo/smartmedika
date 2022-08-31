import {Component} from '@angular/core';
import { ArticleService } from '../../providers';
import { ArticleModel } from '../../model';
import { Commons } from '../../util/commons';
import * as Constant from '../../util/constants';
import { NavController } from 'ionic-angular';
import { ArticleDetailPage } from '../article-detail/article-detail';

@Component({
  templateUrl: 'tab-page-2.html',
  providers: [ArticleService]
})
export class TabPage2 {
  articleList: ArticleModel[];
  constructor(private common: Commons, private service: ArticleService, public navCtrl: NavController) {
    this.getAllArticle();
  }

  getAllArticle() {
    let pageIndex = 1;
    let pageSize = Constant.PAGE_SIZE;
    this.common.showLoading('Mengunduh artikel...', true);
    this.service.getArticleList(pageIndex, pageSize).subscribe(
      res => {
        this.common.hideLoading();
        console.log(res.items);
        this.articleList = res.items;
      }, error => {
        console.error(error);
      }
    )
  }

  onItemClicked(id: any) {
    console.log('articleId:', id)
    this.navCtrl.push(ArticleDetailPage, {
      articleId: id
    })
  }
}
