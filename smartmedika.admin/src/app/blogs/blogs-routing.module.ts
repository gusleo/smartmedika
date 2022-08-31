import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListCategoryComponent } from './category/list/list-category.component';
import { RegisterCategoryDialog } from './category/register/register-category.dialog';
import { ListArticleComponent } from './article/list/list-article.component';
import { RegisterArticleComponent } from './article/register/register-article.component';

const routes: Routes = [
  {
    path: '',
    children: [{
      path: 'category',
      component: ListCategoryComponent,
    },{
      path: 'category/register/:id',
      component: RegisterCategoryDialog      
    },{
      path: 'article',
      component: ListArticleComponent      
    },{
      path: 'article/register/:id',
      component: RegisterArticleComponent      
    }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BlogsRoutingModule { }
