export class OperatingHourModel{
    //day interger https://msdn.microsoft.com/en-us/library/system.dayofweek.aspx
    day: number;
    startTime: string;
    endTime: string;
    isClossed: boolean;

    constructor();
    constructor(day?:number, startTime?:string, endTime?:string, isClossed?:boolean){
        this.day = day || 0;
        this.startTime = startTime || null;
        this.endTime = endTime || null;
        this.isClossed = isClossed || false;
    }
}