export class MenuItemModel{

    id:number;
    parentId:number;
    name:string;
    state:string;
    icon:string;
    roles:Array<string>;
    order:number;
    type:string;
    children:Array<MenuItemModel>;
}