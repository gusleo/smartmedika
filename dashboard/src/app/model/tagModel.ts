import { IEntityBase } from './IEntityBase';
import { TagMapModel } from './tagMapModel';
export class TagModel implements IEntityBase{
    id:number;
    tagName:string;
    tagMaps:Array<TagMapModel>;
}
