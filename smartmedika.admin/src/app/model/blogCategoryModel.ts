import { IEntityBase }          from './IEntityBase';

export class BlogCategoryModel implements IEntityBase{
    id: number;
    parentId: number;
    name: string;
    slug: string;
    isVisible: boolean;
    articleCount:number;

    constructor()
    {
        this.id = 0;
        this.parentId = 0;
        this.name = null;
        this.slug = null;
        this.isVisible = true;
        this.articleCount = 0;
    }
}