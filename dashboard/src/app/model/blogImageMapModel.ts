import { IEntityBase} from './IEntityBase';
import { BlogModel } from './blogModel';
import { ImageModel } from './imageModel';

export class BlogImageMapModel implements IEntityBase{
    id:number;
    articleId: number;
    imageid:number;

    article:BlogModel;
    image:ImageModel;

} 