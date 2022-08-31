export class PhotoModel{
    order: number;
    path: string;
    imageName: string;
    isCover: boolean;

    constructor(){
        this.order = 0;
        this.path = null;
        this.imageName = null;
        this.isCover = false;
    }
}