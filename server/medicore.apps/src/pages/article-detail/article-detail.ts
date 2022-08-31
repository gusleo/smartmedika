import { Component } from '@angular/core';
import { NavController, NavParams } from 'ionic-angular';
import { ArticleModel } from '../../model';
import { Commons } from '../../util/commons';
import { ArticleService } from '../../providers';

/**
 * Generated class for the ArticleDetailPage page.
 *
 * See http://ionicframework.com/docs/components/#navigation for more info
 * on Ionic pages and navigation.
 */

@Component({
  selector: 'page-article-detail',
  templateUrl: 'article-detail.html',
})
export class ArticleDetailPage {

  articleId: any;
  articleDetail: ArticleModel;
  constructor(public navCtrl: NavController, public navParams: NavParams, private common: Commons, private service: ArticleService) {
    this.articleId = navParams.get('articleId');
    console.log('articleId', this.articleId);
  }

  ionViewDidLoad() {
    console.log('ionViewDidLoad ArticleDetailPage');
  }

  getArticleDetail(articleId: number) {
    this.common.showLoading('Mengunduh Artikel...', true);
    this.service.getArticleDetail(articleId).subscribe(
      res => {
        this.common.hideLoading();
        console.log('article detail', res);
        this.articleDetail = res;
      }, error => {
        console.error(error);
      }
    )
  }

}
