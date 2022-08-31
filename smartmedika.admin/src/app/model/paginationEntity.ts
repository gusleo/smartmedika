export class PaginationEntity<T> {
	public page: number = 0;
	public totalPages: number = 0;
	public totalCount: number = 0;
	public count: number = 0;
	public pageSize: number = 0;
    public items: Array<T>;

	constructor(page: number = 0, pageSize:number = 0, totalPages: number = 0, totalCount: number = 0, count:number = 0) {
		this.page = page;
		this.pageSize = pageSize;
		this.totalPages = totalPages;
		this.totalCount = totalCount;
        this.count = count;
	}	

	get offset():number{
		let offset =  (this.page - 1) * this.pageSize;
		return offset;
	}
}