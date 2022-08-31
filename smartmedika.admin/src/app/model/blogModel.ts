
import { IEntityBase } from './IEntityBase';
import { ArticleStatus } from './enumModel';
import { BlogCategoryModel, } from './blogCategoryModel';
import { TagMapModel } from './tagMapModel';
import { BlogImageMapModel } from './blogImageMapModel';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class BlogModel extends WriteHistoryBaseModel implements IEntityBase{
    id: number;
    title: string;
    categoryId: number;
    description: string;
    shortDescription: string;
    isFeatured?: boolean;
    acceptedById?: number;
    acceptedDate?: string;
    metadata: string;
    slug: string;
    category: BlogCategoryModel;   
    status: ArticleStatus;
    tagMaps: Array<TagMapModel>;
    imageMaps: Array<BlogImageMapModel>;
    
    constructor()
    constructor(id: number, title: string, categoryId: number, description: string, shortDescription: string, isFeatured?: boolean,
        acceptedById?: number, acceptedDate?: string)
    constructor(id?: number, title?: string, categoryId?: number, description?: string, shortDescription?: string, isFeatured?: boolean,
        acceptedById?: number, acceptedDate?: string) {
        super();
        this.id = 0;
        this.title = title || '';
        this.categoryId = categoryId || null;
        this.description = description || '';
        this.shortDescription = shortDescription || '';
        this.isFeatured = isFeatured || false;
        this.acceptedById = acceptedById || null;
        this.acceptedDate = acceptedDate || null;
        this.metadata = null;
        this.slug = null;
        this.category = null;
        this.status = ArticleStatus.UnConfirmed;
        this.tagMaps = null;
        this.imageMaps = null;
    }
}