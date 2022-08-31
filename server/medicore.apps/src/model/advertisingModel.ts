import { IEntityBase } from './IEntityBase';
import { WriteHistoryBaseModel } from './writeHistoryBaseModel';
import { AdvertisingType, Status } from './enumModel';
import { ImageModel } from './imageModel'
import { Type } from 'class-transformer';

export class AdvertisingModel extends WriteHistoryBaseModel implements IEntityBase {
    id: number;
    imageId?: number;
    content: string;
    type: AdvertisingType;
    status: Status;
    buttonName: string;
    buttonActionUrl: string;
    buttonSecondaryName: string;
    buttonSecondaryAction: string;

    @Type(() => ImageModel)
    image: ImageModel;
}