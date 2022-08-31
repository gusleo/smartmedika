import { IEntityBase } from './IEntityBase';
import { Type } from 'class-transformer';
import { ArticleModel, ImageModel } from '../model';

export class ArticleImageModel implements IEntityBase {
    id: number;
    articleId: number;
    imageId: number;
    @Type(() => ArticleModel)
    article: ArticleModel;
    @Type(() => ImageModel)
    image: ImageModel;

    constructor() {
        this.id = 0;
        this.imageId = 0;
        this.article = null;
        this.image = null;
    }
}

