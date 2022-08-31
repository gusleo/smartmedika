import { Type } from 'class-transformer';
import { UserModel } from './userModel';
import { ArticleCategoryModel } from './articleCategoryModel';
import { ArticleTagMapModel } from './articleTagMapModel';
import { ArticleImageModel } from './articleImageModel';
import { ArticleStatus } from './enumModel';
import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class ArticleModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    slug: string;
    title: string;
    description: string;
    shortDescription: string;
    metadata: string;
    categoryId: number;
    isFeatured?: boolean;
    acceptedById?: number;
    acceptedDate?: Date;
    status: ArticleStatus;
    //region model
    @Type(() => UserModel)
    acceptedByUser: UserModel;
    @Type(() => ArticleCategoryModel)
    category: ArticleCategoryModel;
    @Type(() => ArticleTagMapModel)
    tagMaps: ArticleTagMapModel[];
    @Type(() => ArticleImageModel)
    imageMaps: ArticleImageModel[];

    constructor() {
        super();
        this.id = 0;
        this.slug = null;
        this.title = null;
        this.description = null;
        this.shortDescription = null;
        this.metadata = null;
        this.categoryId = 0;
        this.isFeatured = false;
        this.acceptedById = 0;
        this.acceptedDate = null;
        this.status = null;
        this.acceptedByUser = null;
        this.category = null;
        this.tagMaps = null;
        this.imageMaps = null;
    }
}

