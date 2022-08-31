import { Component, OnInit, ViewChild }        						from '@angular/core';
import { Router }                   								from '@angular/router';
import { ModalDirective }           								from 'ng2-bootstrap/modal/modal.component';

//grap api, service, model
import { AppConstant }                                      		from '../../';
import { PaginationEntity, Pagination, HospitalModel, RegionModel }	from '../../model';
import {RegionService, ClinicService } 								from '../../services';



@Component({
    templateUrl: 'list-klinik.component.html'
})
export class ListKlinikComponent extends Pagination {

	//array for each model
    result: Array<HospitalModel>;
	resultRegion: Array<RegionModel>;
    //variable for search
	public keywordsInt: number = 0;
	public textSearch: string = "";
	//variable for modal
	private messageModal: string = "";
  	private titleModal: string = "";
  	private buttonTitleModal: string = "";
	private ids: number = 0;
    @ViewChild('childModal') public childModal:ModalDirective;	

    constructor(private router: Router, public clinicService: ClinicService, public regionService: RegionService) {
        super();
     }
    
	ngOnInit() {
		this.loadClinic(this._currentPage, this._itemsPerPage);
		this.loadRegion();
	}
	
	loadRegion()
	{
		let datas = [];
		this.regionService.getRegion().subscribe(
			res => {
				this.resultRegion = res;				
			}, error => {
				if (error.status == 401) {
					//this.utilityService.navigateToSignIn();
				}
				console.error('Error: ' + error);				
			});
	}
    
    loadClinic(page: number, itemPerPage: number)
    {
        this.clinicService.getAllHospitalByPaging(page, itemPerPage).subscribe( 
            res => {
                this.assignResultToPage(res);
			}, error => {
				if (error.status == 401) {
					//this.utilityService.navigateToSignIn();
				}
				console.error('Error: ' + error);                
           });
    }

	protected assignResultToPage(res: PaginationEntity<HospitalModel>){
		this._totalItems = res.totalCount;
		this._totalPage = res.totalPages;
		if(res.page == 1){ this._start = this._currentPage; this._end = this._totalItems; }  
		if(res.totalPages <= this._end){ this._end = this._totalPage; }  
		this.result = res.items;
	}	
        
	private pageChanged(event: any): void {
		this._currentPage = event.page;
		if(this._statusUrl == false){
			this.loadClinic(this._currentPage, this._itemsPerPage);
		}else{
			this.loadSearch(this._currentPage);
		}
		this._start = this._itemsPerPage * (this._currentPage - 1) + 1;
		this._reduction = this._itemsPerPage * (this._currentPage - 1) + this._itemsPerPage;
		this._totalPage <= this._reduction ? this._end = this._totalPage : this._end = this._reduction;
	}

	private OnSearching($event: any): void{
        this._currentPage = 1;
		this._statusUrl = true;
		this.loadSearch(this._currentPage);
	}

	loadSearch(page: number)
	{
		this.clinicService.getHospitalBySearching(page, this._itemsPerPage, this.keywordsInt, this.textSearch).subscribe( 
            res => {
                this.assignResultToPage(res);
			}, error => {
				if (error.status == 401) {
					//this.utilityService.navigateToSignIn();
				}
				console.error('Error: ' + error);                
           });
	}

	//show modal on click
	private showChildModal(id: number, name: string): void
	{
		this.titleModal = "Konfirmasi Info";
		this.messageModal = "Apakah anda yakin menghapus " + name + "?";
		this.buttonTitleModal = "Ya";
		this.ids = id;		
		this.childModal.show();
	}

	//close modal on click
	private hideChildModal():void {
		this.childModal.hide();
	}

	private onModalClick(id: number)
	{
		this.clinicService.hospitalDelete(id).subscribe(
			res => {
				this._currentPage = 1;
				this._statusUrl = false;
				this.loadClinic(this._currentPage, this._itemsPerPage);
			}, error => {
				if (error.status == 401) {
					//this.utilityService.navigateToSignIn();
				}
				console.error('Error: ' + error);				
		   });
	}

    tambah()
    {
        this.router.navigate(['clinic/register-clinic/0']);
    }
}
