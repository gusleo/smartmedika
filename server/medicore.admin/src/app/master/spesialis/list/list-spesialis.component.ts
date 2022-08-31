import { Component, OnInit, ViewChild, EventEmitter, ElementRef, Renderer }                     from '@angular/core';
import { Router }                                           from '@angular/router';
import { ModalDirective }                                   from 'ng2-bootstrap/modal/modal.component';
import { ToasterModule, ToasterService, ToasterConfig }     from 'angular2-toaster/angular2-toaster';

//grap api, service, model
import { AppConstant }                                      		from '../../../';
import { PaginationEntity, Pagination, MedicalStaffSpesialisModel}  from '../../../model';
import { SpesialisService }                                         from '../../../services';

@Component({
    templateUrl: 'list-spesialis.component.html'
})
export class ListSpesialisComponent{

	//notification for success or error message
    private toasterService: ToasterService;

    public toasterconfig : ToasterConfig =
        new ToasterConfig({
            tapToDismiss: true,
            timeout: 5000
        });

	//array for each model
    result: Array<MedicalStaffSpesialisModel>;
	fuck: Array<MedicalStaffSpesialisModel>;
	//variable for modal
	private messageModal: string = "";
  	private titleModal: string = "";
  	private buttonTitleModal: string = "";
	private ids: number = 0;    
    @ViewChild('childModal') public childModal:ModalDirective;	
	@ViewChild('importFile') protected _importFile:ElementRef;

    constructor(private router: Router, public spesialisService: SpesialisService, toasterService: ToasterService, private renderer:Renderer) {
		this.toasterService = toasterService;
     }
     
     ngOnInit() {
         this.loadSpecialist();
     }

	 loadSpecialist()
	 {		
		this.spesialisService.getAll().subscribe(
			res => {
				this.result = res;		
				//this.fuck = res;		
			}, error => {
                this.toasterService.pop('error', 'Konfirmasi', 'Data tidak dapat ditampilkan'); 
				console.error('Error: ' + error);				
			});
	 }

    tambah()
    {
        this.router.navigate(['master/spesialis/register-spesialis/0']);
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
		this.spesialisService.delete(id).subscribe(
			res => 
			{
				this.loadSpecialist();				
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
			this.spesialisService.import(file);
        }           
    }    
}