import { Component, ViewChild, OnInit }                     from '@angular/core';
import { ModalDirective }                                   from 'ng2-bootstrap/modal/modal.component';
import { AgmCoreModule, MapsAPILoader }                     from 'angular2-google-maps/core';
import { HospitalModel, HospitalMetadataModel, 
         PhoneModel, MetaType, EmailModel, OperatingHourModel, 
         PolyClinicToHospitalMapModel, PolyClinicModel,
         RegencyModel }                                     from '../../model';
import { ActivatedRoute, Router }                           from '@angular/router';
import { ClinicService, PolyClinicService, 
         UploadImageService, RegionService }                from '../../services';
import { AppConstant }                                      from '../../';
import { plainToClass, classToPlain }                       from 'class-transformer';

@Component({
    templateUrl: 'register-clinic.component.html',
    styles: [`
        .sebm-google-map-container {
        height: 300px;
        }
    `]    
})
export class RegisterClinicComponent{
    
    public model = new HospitalModel();
    // start value for gallery picture
    public defaultGaleri = '../assets/img/avatars/placeholder.png';
    public galeriPhoto = "";
    public uploaderURL:any = {
        url: 'http://website.com/upload'
    };	
    // end value for gallery picture

    // start value for profile picture
    public picture:any =  '../assets/img/avatars/headshot.png';
    public uploaderOptions:any = {

         url: 'http://website.com/upload'
    };	
    // end value for profile picture
    profilPicture: any;
    galeriPicture1 : any = {};
    galeriPicture2 : any = {};
    galeriPicture3 : any = {};
    
    //modal
    @ViewChild('childModal') public childModal:ModalDirective;

    //set variabel for google maps   
    public longitude:number = AppConstant.Longitude;
    public latitude:number = AppConstant.Latitude;
    public zoom: number = 4;
    public markers: any;
    public label: string;
    public draggable: boolean;

    //timepicker
    ismeridian:boolean = false;
    mytime:Date = new Date("HH:mm");
    showTimepicker: boolean = false;
    
    public phones:Array<PhoneModel>;
    public emails:Array<EmailModel>;
    public operatingHours: Array<OperatingHourModel>;
    public selectedPolyClinic:Array<PolyClinicToHospitalMapModel>;
    public polyClinicList: Array<PolyClinicModel>;
    public newPolyClinicList: any = [];
    public tempKabupatenKota: any;
    public regencylist: Array<RegencyModel>;
    public regensName: Array<string> = ['Kota Denpasar', 'Badung', 'Karangasem', 'Jembrana', 'Gianyar', 'Tabanan', 'Singaraja'];

    constructor(private route: ActivatedRoute, public clinicService: ClinicService, public polyClinicService:PolyClinicService, public uploadImageservice: UploadImageService, public regionService: RegionService) {
        
        this.emails = this.initClinicEmail();
        this.phones = this.initClinicPhone();        
        this.operatingHours = this.initOperatingHours();
        this.selectedPolyClinic = new Array<PolyClinicToHospitalMapModel>();
     }

    ngOnInit(){
        let hospitalId:any = 0;

        this.route.params.subscribe(
            params => {
              hospitalId = params['id'];              
            });        
       

        if(hospitalId != 0){  
            this.clinicService.getDetail(hospitalId)
                .subscribe(res => {
                    this.model = res;  
                    
                    let phone = this.model.clinicMetas.find(x => x.metaType == MetaType.Phones);      
                    if(phone){
                        this.phones = plainToClass(PhoneModel, JSON.parse(phone.jsonValue));
                    } 

                    let email = this.model.clinicMetas.find(x => x.metaType == MetaType.Email);      
                    if(email){
                        this.emails = plainToClass(EmailModel, JSON.parse(email.jsonValue));
                    }  

                    let operatingHour = this.model.clinicMetas.find(x => x.metaType == MetaType.OperatingHours);      
                    if(operatingHour){
                        this.operatingHours = plainToClass(OperatingHourModel, JSON.parse(operatingHour.jsonValue));
                    }

                    if(this.model.polyClinicMaps){
                        this.selectedPolyClinic = this.model.polyClinicMaps;
                    }
                                 
                                           
                    
                    this.setCurrentPosition(this.model.longitude, this.model.latitude);                  
                }, error =>{
                    console.log(error);
                }) 
        };
        
         //this.searchRegency();

         this.polyClinicService.getAll().subscribe(
             res =>{
                this.polyClinicList = res;
                this.buildPoliKlinik();
             },error =>{
                    console.log(error);
             }

         );
         this.setCurrentPosition();
    }

    // ngFor untuk jenis layanan
    private buildPoliKlinik()
    {
        var stringList = this.polyClinicList;
        var colCount = 2;
        for (var i = 0; i < stringList.length; i = i + 2) {
            var element = stringList[i];
            var row = [];
            while (row.length < colCount) {
                var value = stringList[i + row.length];
                if (!value) {
                    break;
                }
                row.push(value);
            }
            this.newPolyClinicList.push(row);
        }       
    }

    private initClinicPhone():Array<PhoneModel>{
        let phones:PhoneModel[] = new Array<PhoneModel>();
        //primaryPhone
        phones.push({order: 0, phone:''});
        //secondaryPhone
        phones.push({order:1, phone:''});
        return phones;
    }
    private initClinicEmail():Array<EmailModel>{
        let emails:EmailModel[] = new Array<EmailModel>();
        //primaryEmail
        emails.push({order: 0, email:''});       
        return emails;
    }
    private setCurrentPosition(longitude?:number, latitude?:number)
    {
        longitude = longitude || this.longitude;
        latitude = latitude || this.latitude;

        if("geolocation" in navigator){
            navigator.geolocation.getCurrentPosition((position) => {
                latitude = position.coords.latitude;
                longitude = position.coords.longitude;
                this.zoom = 12                   
            });
        }
    }
    private markerDragEnd($event: MouseEvent)
    {         
        let e:any = ($event as Object)
        this.model.longitude = e.coords.lng;
        this.model.latitude = e.coords.lat;        
        
    }

    //get value from tinymce
    public keyupHandlerFunction($event):void{
        this.model.description = $event;
    }

    //show modal on click
    public showChildModal():void {
        this.childModal.show();
    }

    //close modal on click
    public hideChildModal():void {
        this.childModal.hide();
    }  
    private initOperatingHours():Array<OperatingHourModel>{
        //this is just dummy data
        let operatingHours:Array<OperatingHourModel> = new Array<OperatingHourModel>();
        operatingHours.push({day:0, startTime: '18.00', endTime: '00.00', isClossed: false});
        operatingHours.push({day:1, startTime: '', endTime: '', isClossed: true});
        operatingHours.push({day:2, startTime: '18.00', endTime: '00.00', isClossed: false});
        operatingHours.push({day:3, startTime: '', endTime: '', isClossed: true});
        operatingHours.push({day:4, startTime: '18.00', endTime: '00.00', isClossed: false});
        operatingHours.push({day:5, startTime: '', endTime: '', isClossed: true});
        operatingHours.push({day:6, startTime: '18.00', endTime: '00.00', isClossed: false});
        return operatingHours;
    }
    private setHospitalMeta(){
        
       
        if(!this.model.clinicMetas){
            this.model.clinicMetas = new Array<HospitalMetadataModel>();
        }
        
        let metaPhone = this.model.clinicMetas.find(x => x.metaType == MetaType.Phones);            
        if(!metaPhone){                
            metaPhone = new HospitalMetadataModel()
            metaPhone.id = 0;
            metaPhone.hospitalId = this.model.id;
            metaPhone.metaType = MetaType.Phones;
            metaPhone.jsonValue = JSON.stringify(this.phones);
            this.model.clinicMetas.push(metaPhone);
        }else{
            metaPhone.jsonValue = JSON.stringify(this.phones);
            let index:number = this.model.clinicMetas.findIndex(x => x.metaType == MetaType.Phones);
            this.model.clinicMetas[index] = metaPhone;
        }
        let metaEmail = this.model.clinicMetas.find(x => x.metaType == MetaType.Email);
        if(!metaEmail){           
            metaEmail = new HospitalMetadataModel();
            metaEmail.id = 0;
            metaEmail.hospitalId = this.model.id;
            metaEmail.metaType = MetaType.Email;
            metaEmail.jsonValue = JSON.stringify(this.emails);
            this.model.clinicMetas.push(metaEmail);
        }else{
            metaEmail.jsonValue = JSON.stringify(this.emails);
            let index: number = this.model.clinicMetas.findIndex(x => x.metaType == MetaType.Email);
            this.model.clinicMetas[index] = metaEmail;
        }

        let metaOperatingHour = this.model.clinicMetas.find(x => x.metaType == MetaType.OperatingHours);
        if(!metaOperatingHour){
            metaOperatingHour = new HospitalMetadataModel();
            metaOperatingHour.id = 0;
            metaOperatingHour.hospitalId = this.model.id;
            metaOperatingHour.metaType = MetaType.OperatingHours;
            metaOperatingHour.jsonValue = JSON.stringify(this.operatingHours);
            this.model.clinicMetas.push(metaOperatingHour);
        }else{
            metaOperatingHour.jsonValue = JSON.stringify(this.operatingHours);
            let index: number = this.model.clinicMetas.findIndex(x => x.metaType == MetaType.OperatingHours);
            this.model.clinicMetas[index] = metaOperatingHour;
        }
        
        
    }
    public saveClinicProfile():void{
        this.setHospitalMeta();  

        //upload image 
        this.toUploadImage();
        //harcoded, please fix
        this.model.regencyId = 1;

        if(this.model.id == 0){
            this.clinicService.create(this.model).subscribe(
                res =>{
                    console.log(res);
                },
                error => {
                    console.log(error);
                }
            )
        }
        
    }

    toUploadImage()
    {
        this.uploadImageservice.upload(this.profilPicture, this.galeriPicture1, this.galeriPicture2, this.galeriPicture3).subscribe(
            res =>{
                console.log(res);
            },
            error =>{
                console.log(error);
            }      
        );
    }

    //untuk mendapat file untuk profile picture
    public handleOTImage(myFile)
    {
        this.profilPicture = myFile;
        console.log(this.profilPicture);
    }
    
    //untuk mendapat file untuk galeri 1
    public handleGaleri1(myFile)
    {
        this.galeriPicture1 = myFile;
        console.log(this.galeriPicture1);        
    }

    //untuk mendapat file untuk galeri 2
    public handleGaleri2(myFile)
    {
        this.galeriPicture2 = myFile;
        console.log(this.galeriPicture2);        
        
    }

    //untuk mendapat file untuk galeri 3
    public handleGaleri3(myFile)
    {
        this.galeriPicture3 = myFile;
        console.log(this.galeriPicture3);                
    }        
    
    //====== untuk komponen ng2select =========
    
    //menampilkan data regency 
    public searchRegency()
    {
        this.regionService.getRegencyByClue().subscribe( 
            res => {
                this.regencylist = res;
                for(let i=0; i<res.length; i++)
                {
                    this.regensName.push(res[i].name);
                
                }
                console.log(this.regensName);
			}, error => {
				console.error('Error: ' + error);                
           });        
    }

    //untuk refresh value    
    public refreshValue(value:any):void {
        this.tempKabupatenKota = value;
    }    

    //untuk memilih value    
    public selected(value:any):void {
        console.log('Selected value is: ', value);
    }

    //untuk menghapus value    
    public removed(value:any):void {
        console.log('Removed value is: ', value);
    }        
}
