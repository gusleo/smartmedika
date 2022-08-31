import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';

export class ImageModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    imageUrl: string;
    fileName: string;
    fileExtension: string;
    isPrimary: boolean;
    description: string;
}