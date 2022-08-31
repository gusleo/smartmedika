import { Type } from 'class-transformer';
import { ArticleTagMapModel } from './articleTagMapModel';
import { IEntityBase } from './IEntityBase';

export class TagModel implements IEntityBase {
    id: number;
    tagName: string;

    //region model
    @Type(() => ArticleTagMapModel)
    tagMaps: ArticleTagMapModel[];

    constructor() {
        this.id = 0;
        this.tagName = null;
        this.tagMaps = null;
    }
}

