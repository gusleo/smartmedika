import { MenuType } from './enumModel';
import { IEntityBase } from './IEntityBase';

export class TreeMenuModel implements IEntityBase {
    id: number;
    parentId: number;
    displayName: string;
    link: string;
    displayIcon: string;
    roles: string;
    type: MenuType;
    order: number;
}

