import { Type } from 'class-transformer';
import { TagModel } from './tagModel';
import { ArticleModel } from './articleModel';
import { IEntityBase } from './IEntityBase';

export class ArticleTagMapModel implements IEntityBase {
    id: number;
    articleId: number;
    tagId: number;

    //region model
    @Type(() => TagModel)
    tag: TagModel;
    @Type(() => ArticleModel)
    article: ArticleModel;

    constructor() {
        this.id = 0;
        this.articleId = 0;
        this.tagId = 0;
        this.tag = null;
        this.article = null;
    }
}

