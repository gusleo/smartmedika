import { IEntityBase } from './IEntityBase';

export class UserRoleModel implements IEntityBase {
    id: number;
    name: string;   
    normalizedName: string;

    constructor() {
        this.id = 0;
        this.name = null;
        this.normalizedName = null;
    }    
}

export class UserRoleViewModel extends UserRoleModel{
    isChecked: boolean;
}