import { IEntityBase }          from './IEntityBase';

export class UserModel implements IEntityBase{
    id: number;   
    username: string;
    email: string;
    phoneNumber: string;

    constructor(){
        this.id = 0;
        this.username = null;
        this.email = null;
        this.phoneNumber = null;
    }
}

