import { Type }                 from 'class-transformer';
import { UserModel }         from './userModel';
export class WriteHistoryBaseModel{
        createdById:number;
        updatedById:number;
        @Type(() => UserModel)
        createdByUser:UserModel;
        @Type(() => UserModel)
        updatedByUser:UserModel;        
        createdDate:Date;
        updatedDate:Date;

        constructor(){
            this.createdById = 0;
            this.updatedById = 0;
            this.createdDate = null;
            this.updatedDate =  null;
            this.createdByUser = null;
            this.updatedByUser = null;
        }
       
}