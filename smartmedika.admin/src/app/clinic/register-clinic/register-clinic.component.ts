import { Component, OnInit, ViewChild, ElementRef, NgZone, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { MapsAPILoader } from '@agm/core';
import { MdDialog, MdDialogRef, MdDialogConfig } from '@angular/material';
import { Marker, DateHelper, Message} from '../../../libs';
import { PolyClinicService, ClinicService, GeoLocationService, AuthService } from '../../services';
import { PolyClinicModel, PolyClinicViewModel, HospitalOpratingHoursModel, HospitalImage,
          HospitalModel, PolyClinicToHospitalMapModel, PhoneModel, EmailModel, RegencyModel, PaginationEntity } from '../../model';
import { FormBuilder, FormGroup, Validators, FormControl }  from '@angular/forms';
import { CustomValidators }  from 'ng2-validation';
import { OperatingHoursDialog } from '../popup';
import { DummyData } from '../dummyData';
import { DialogConfig, FileUploadAPI } from '../../';
import { ImageEditorDialog } from '../../media';
import { ImageNotAvailable } from '../../app.config'

declare var google: any;

@Component({
  selector: 'app-register-clinic',
  templateUrl: './register-clinic.component.html',
  styleUrls: ['./register-clinic.component.scss']
})
export class RegisterClinicComponent implements OnInit, AfterViewInit {

  @ViewChild("search")
  public searchElementRef:ElementRef;

  clinicImage:string = ImageNotAvailable;
  filename:string;
  isNewImage:boolean = false;

  form:FormGroup;
  hospital = new HospitalModel();
  polyclinics: Array<PolyClinicViewModel>;
  operatingHours: Array<HospitalOpratingHoursModel>;
  dummy:DummyData = new DummyData();
  id:number = 0;
  onRequest:boolean = false;
  
  //google map
  lat = -8.6559667;
  lng = 115.2169412;
  zoom = 15;
  draggable: false;
  marker: Marker = new Marker(this.lat, this.lng, true);

  //autoComplete
  currentRegency:any;
  regencys: Observable<RegencyModel[]>;
  regencyContent:RegencyModel[] = [];

  //dialog
  operatingHoursDialog: MdDialogRef<OperatingHoursDialog>;
  imageEditorDialog: MdDialogRef<ImageEditorDialog>;
  
  
  constructor(private polyClinicServices: PolyClinicService, private geoServices: GeoLocationService, 
    private clinicService:ClinicService, private route: ActivatedRoute, private formBuilder: FormBuilder, private message:Message, private authService: AuthService, 
    public dialog: MdDialog, public _location: Location, private router: Router, private mapsAPILoader: MapsAPILoader, private ngZone:NgZone){

      this.route.params.subscribe(params => {
        if(params['id']){
           this.id =  Number.parseInt(params['id']);
        }
         
      });
      this.createForm();
     
  }

  async ngOnInit() {
    this.operatingHours = this.dummy.getDefaultOperatingHour();
    await this.getListOfPolyClinic();  
    this.getHospitalDetail(this.id);  
    
  }
  
  ngAfterViewInit(){
    this.googleAutoComplete();
  }

  googleAutoComplete(){
    
    this.mapsAPILoader.load().then(() =>{
      let autocomplete = new google.maps.places.Autocomplete(this.searchElementRef.nativeElement,{
        types: ["address"]
      });
      autocomplete.addListener("place_changed", () => {
        this.ngZone.run(() => {
          //get the place result
          let place:any = autocomplete.getPlace();
  
          //verify result
          if (place.geometry === undefined || place.geometry === null) {
            return;
          }
          
          //set latitude, longitude and zoom
          this.marker.lat = this.lat = this.hospital.latitude = place.geometry.location.lat();
          this.marker.lng = this.lng = this.hospital.longitude = place.geometry.location.lng();         
          this.zoom = 15;
        });
      });
    });
  }
  

  createForm(){
    // setting for validation
        this.form = this.formBuilder.group({
            name: [null, Validators.compose([Validators.required, Validators.minLength(5)])],
            primaryPhone: [null, Validators.compose([Validators.required, CustomValidators.number])],
            secondaryPhone: [null, Validators.compose([CustomValidators.number])],
            email: [null, Validators.compose([CustomValidators.email])],
            website: [null, Validators.compose([CustomValidators.url])],
            address: [null, Validators.compose([Validators.required, Validators.minLength(10)])],
            description: [null],
            regency: [null, Validators.compose([Validators.required])],
            searchLocation: ['']
        });

     this.initRegencyAutoComplete();
  }

  initRegencyAutoComplete(){
    this.regencys = this.form.get('regency').valueChanges
      .debounceTime(400)
      .do (value =>{
        let exist = this.regencyContent.findIndex(t => t.name === value);
        if (exist > -1) return;

        // get data from the server. my response is an array [{id:1, text:'hello world'}]
        this.geoServices.getRegencyByClue(1, 1000, false, value).subscribe((res: PaginationEntity<RegencyModel>) => { this.regencyContent = res.items; }); 
      }).delay(500).map(() => this.regencyContent);
  }

  displayFn(value: RegencyModel): string {
    return value && typeof value === 'object' ? value.name + ' (' + value.region.name + ')' : '';
  }

  
  openCloseClinic(day:number, isClossed:boolean){
    let index:number = this.operatingHours.findIndex(x => x.day == day);
    this.operatingHours[index].isClossed = isClossed;
  }

  openOperatingHours(day:number, startTime: string, endTime: string){
    let config:any = DialogConfig;
    config.data = {
      title: "Jadwal Praktek Dokter Hari " + this.getDayName(day),
      day: day,
      startTime: startTime,
      endTime: endTime
    }
    this.operatingHoursDialog = this.dialog.open(OperatingHoursDialog, config);
    this.operatingHoursDialog.afterClosed().subscribe(result => {
      if(result != undefined){
        let index:number = this.operatingHours.findIndex(x => x.day == result.day);
        if(index > -1){
          this.operatingHours[index].startTime = result.startTime;
          this.operatingHours[index].endTime = result.endTime;
          this.operatingHours[index].isClossed = false;
        }
        
      }
      this.operatingHoursDialog = null;
    });
  }
  
  async getListOfPolyClinic(){
    let res = await this.polyClinicServices.getAll().toPromise();
    this.polyclinics = new Array<PolyClinicViewModel>();
    res.forEach(element => {
      this.polyclinics.push({
        id: element.id,
        name: element.name,
        isChecked: false
      });
    });
  }

  async getHospitalDetail(id:number){    
    if(id > 0){
      this.hospital = await this.clinicService.getDetail(id).toPromise();
      if(this.hospital){
          let img = this.hospital.images.filter(image => image.image.isPrimary == true);
          if(img.length > 0){
            this.clinicImage = img[0].image.imageUrl;
          }
          this.currentRegency = this.hospital.regency;
          this.setCoordinate(this.hospital);
          this.getHospitalPolyClinic();

          if(this.hospital.operatingHours != null && this.hospital.operatingHours.length > 0){
            this.operatingHours = this.hospital.operatingHours;
          }

      }      
    }
  }

  setCoordinate(hospital:HospitalModel){
    if(hospital.longitude != null && hospital.latitude != null){
      this.marker.lat = this.lat = hospital.latitude;
      this.marker.lng = this.lng = hospital.longitude;
    }else{
      this.setCurrentPosition();
    }
  }
private setCurrentPosition() {
    if ("geolocation" in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.marker.lat = this.lat = this.hospital.latitude = position.coords.latitude;
        this.marker.lat = this.lat = this.hospital.latitude = position.coords.longitude;
        this.zoom = 12;
      });
    }
  }
  getHospitalPolyClinic(){
    if(this.hospital.polyClinicMaps != null){
      this.hospital.polyClinicMaps.forEach(item =>{
        this.polyclinics.forEach(element => {
          if(element.id == item.polyClinicId){
            element.isChecked = true;
            return false;
          }
        });
      })
    }
  }

  getDayName(day:number):string{    
    let res:string = new DateHelper().getDayName(day); 
    return res;

  }
  async saveContinue() {
    
    await this.saveHelper();
    this.hospital = new HospitalModel();
    this.id = 0;
    await this.getListOfPolyClinic();      
    
  }

  async submitHospital(){
    await this.saveHelper(); 
  }

  async saveHelper(){
    if(this.hospital.longitude == null || this.hospital.latitude == null){
      this.message.open("Koordinat lokasi harus diisi");
      return;
    }

    let message = this.id == 0 ? "Sukses menambah rumah sakit" 
        : "Sukses merubah profil rumah sakit"; 

    this.onRequest = true;
    await this.updateHospital();

    if(this.isNewImage){
      await this.updateCoverImage();
    }
    
    this.onRequest = false;
    this.message.open(message);
  }

  async updateCoverImage(){
    let result = await this.clinicService.CoverImage(this.id, this.filename, this.clinicImage).toPromise();
  }
  
  async updateHospital(){   
    let selectedPolyClinic = this.getSelectedPolyClinic();
    this.hospital.polyClinicMaps = selectedPolyClinic;
    this.hospital.operatingHours = this.operatingHours;
    
    if(this.currentRegency != null){
      this.hospital.regencyId = this.currentRegency.id;
    }
    
    let result = await this.clinicService.save(this.hospital).toPromise();   
    this.id = result.id;
  }
  
private errorEventHandler(item: any, response: string, status: number, headers: any): any {
      let error = JSON.parse(response);
     // this.errors = error.errors;
      //this.showErrors = true;
      console.log(error.errors.length);
  }
  getSelectedPolyClinic(): Array<PolyClinicToHospitalMapModel>{
    let selectedPolyClinic = new Array<PolyClinicToHospitalMapModel>();

    this.polyclinics.forEach(el => {
      if(el.isChecked){
        let poly:PolyClinicToHospitalMapModel = new PolyClinicToHospitalMapModel(0, this.id, el.id);
        selectedPolyClinic.push(poly);
      }
    });
    return selectedPolyClinic;
  }
  clickedMarker(m: Marker, $event: any) {    
    
  }
  
  mapClicked($event: any) {   
    this.updateMarker($event.coords.lat, $event.coords.lng);   
  }

  markerDragEnd(m: Marker, $event: any) {
     this.updateMarker($event.coords.lat, $event.coords.lng); 
  }

  updateMarker(lat:number, lng:number){
    this.marker.lat = lat;
    this.marker.lng = lng;
    
    this.hospital.longitude = this.marker.lng;
    this.hospital.latitude = this.marker.lat;
  }

  imageChange($event:any){
    let file:File = $event.target.files[0];
    this.filename = file.name;
    let fileReader = new FileReader();
    var img;
    if(file){
      fileReader.onload = (event:any) => {
        this.openImageEditorDialog(event.target.result);
      }
      fileReader.readAsDataURL(file);
    }
    
  }

  openImageEditorDialog(base64:any){
    let config:any = DialogConfig;
    config.data = {      
      base64: base64
    };
    config.width = '';
    config.height = '';

    let that = this;
    this.imageEditorDialog = this.dialog.open(ImageEditorDialog, config);
    this.imageEditorDialog.afterClosed().subscribe(result => {
      if(result != undefined){       
       that.clinicImage = result.base64;  
       that.isNewImage = true;      
      }
      this.imageEditorDialog = null;
    });
  }

  cancel(): void {     
      this.router.navigate(['/clinic/list']);      
  }
}
