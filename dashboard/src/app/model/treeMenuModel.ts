import { IEntityBase } from './IEntityBase';
import {MenuType} from './enumModel';

export class TreeMenuModel implements IEntityBase{
   
   constructor(){
       this.id = 0;
       this.parentId = 0;
       this.order = 0;
       this.displayIcon = "";
   }

   id:number;
   parentId:number;
   link:string;
   displayIcon:string;
   displayName:string;
   roles:string;
   menuType: MenuType;
   order:number;
   

   static getRoles(){
       return new Array<RolesMenuModel>(new RolesMenuModel("SuperAdmin"), new RolesMenuModel("Admin"), 
       new RolesMenuModel("Operator"), new RolesMenuModel("Staff"), new RolesMenuModel("Member"));
   }
}

export class RolesMenuModel{
    constructor(roleName:string, checked:boolean = false){
        this.roleName = roleName;
        this.checked = checked;
    }
    
    checked:boolean;
    roleName:string
}