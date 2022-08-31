export class Pagination {
	public _currentPage: number = 1;
    public _itemsPerPage: number = 15;
	public _totalItems: number = 0;
    public _totalPage:number = 0;
    public _count: number = 0;

    public _start: number;
    public _end: number;
    public _reduction: number = 0;
	public _statusUrl: boolean = false;    
    public maxSize: number = 5;
    public _onPaging: boolean = false;
    public _onSearchtext: string = "";

    protected _showLoader: boolean;

    constructor(currentPage: number = 0, totalPage: number = 0, itemsPerPage: number = 0, 
        totalItems: number = 0, count:number = 0, start: number = 0, end: number = 0, 
        reduction: number = 0, statusUrl: boolean = false, onPaging: boolean = false) {
    }

    /*public showLoader():void{
        this._showLoader = true;
    }
    public hideLoader(){
        this._showLoader = false;
    }*/

    
}
