import { IEntityBase } from './IEntityBase';
import { BlogModel } from './blogModel';
import { TagModel } from './tagModel';
export class TagMapModel implements IEntityBase{
    id:number;
    articleId:number;
    tagId:number;

    tag:TagModel;
    article:BlogModel;
}