import { Component, OnInit, ViewChild, EventEmitter, ElementRef, Renderer }                     from '@angular/core';
import { Router }                                           from '@angular/router';
import { ModalDirective }                                   from 'ng2-bootstrap/modal/modal.component';
import { ToasterModule, ToasterService, ToasterConfig }     from 'angular2-toaster/angular2-toaster';
//grap api, service, model
import { PaginationEntity, Pagination, PolyClinicModel}	    from '../../../model';
import { PolyClinicService }                                from '../../../services';
import { AppConstant }                                      from '../../../';

@Component({
    templateUrl: 'list-polyclinic-component.html'
})
export class ListPolyClinicComponent{

	//notification for success or error message
    private toasterService: ToasterService;

    public toasterconfig : ToasterConfig =
        new ToasterConfig({
            tapToDismiss: true,
            timeout: 5000
        });

	//array for each model
    result: Array<PolyClinicModel>;
    //variable for search
	public keywordsInt: number = 0;
	public textSearch: string = "";
	//variable for modal
	private messageModal: string = "";
  	private titleModal: string = "";
  	private buttonTitleModal: string = "";
	private ids: number = 0;
    @ViewChild('childModal') public childModal:ModalDirective;	
	
	@ViewChild('importFile') protected _importFile:ElementRef;
    
	constructor(private router: Router, public polyClinicService: PolyClinicService, toasterService: ToasterService, private renderer:Renderer) {       
		this.toasterService = toasterService;		
     }
    
	ngOnInit() {
		this.loadPolyClinic();
	}
    
    loadPolyClinic()
    {
        this.polyClinicService.getAll().subscribe( 
            res => {
                this.result = res;
			}, error => {
				this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat ditampilkan'); 
				console.error('Error: ' + error);                
           });
    }

	   

	//route to register polyclinic
    tambah()
    {
        this.router.navigate(['master/polyclinic/register-polyclinic/0']);
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
		this.polyClinicService.delete(id).subscribe(
			res => {				
				this.loadPolyClinic();				
				this.hideChildModal();				
				this.toasterService.pop('success', 'Konfirmasi', 'Data berhasil dihapus');				

			}, error => {
				this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat dihapus'); 
				console.error('Error: ' + error);				
		   });
	}	


	//import file start function
    public bringFileSelector():boolean {
        
        this.renderer.invokeElementMethod(this._importFile.nativeElement, 'click');
        return false;
        
    }	
    public onFiles(data):void {
        let files = this._importFile.nativeElement.files;

        if (files.length) {
        	const file = files[0];			
            console.log(files[0]);
			this.polyClinicService.import(file);
        }           
    }        
}