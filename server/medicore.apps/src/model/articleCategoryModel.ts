import { IEntityBase } from './IEntityBase';

export class ArticleCategoryModel implements IEntityBase {
    id: number;
    parentId: number;
    categoryName: string;
    slug: string;
    isVisible: boolean;
    articleCount: number;

    constructor() {
        this.id = 0;
        this.parentId = 0;
        this.categoryName = null;
        this.slug = null;
        this.isVisible = false;
        this.articleCount = 0;
    }
}

