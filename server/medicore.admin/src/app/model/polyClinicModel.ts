import { IEntityBase }      from './IEntityBase';

export class PolyClinicModel implements IEntityBase{
    id: number;
    name: string;

    constructor(){
        this.id = 0;
        this.name = null;
    }
}