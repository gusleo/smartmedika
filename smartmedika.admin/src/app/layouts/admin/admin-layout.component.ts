import { Component, OnInit, OnDestroy, ViewChild, HostListener, AfterViewInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { FormControl, FormGroup, FormBuilder } from '@angular/forms';
import { MenuItems } from '../../shared/menu-items/menu-items';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/startWith';
import 'rxjs/add/operator/map';

import { TranslateService } from '@ngx-translate/core';
import * as Ps from 'perfect-scrollbar';
import { MenuType, MenuItemModel, HospitalModel, HospitalViewModel } from '../../model';
import { ApplicationService, ClinicService } from '../../services';

@Component({
  selector: 'app-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  private _router: Subscription;

  today: number = Date.now();
  url: string;
  showSettings = false;
  dark: boolean;
  boxed: boolean;
  collapseSidebar: boolean;
  compactSidebar: boolean;
  currentLang = 'en';

  form:FormGroup;
  //autocomplete
  reHospital: HospitalViewModel[] = [];
  globalHospitalCtrl: FormControl;
  listHospital:any;
  hosModel: any;
  hospitalauto;
  menuItems:MenuItemModel[] = [];

  @ViewChild('sidemenu') sidemenu;
  @ViewChild('root') root;

  constructor(private router: Router, public translate: TranslateService, private appService:ApplicationService, private clinicService: ClinicService, private formBuilder: FormBuilder ) {
    const browserLang: string = translate.getBrowserLang();
    translate.use(browserLang.match(/en|fr/) ? browserLang : 'en');

    this.getMenuItems();
    this.ngOnloadHospitalAsscotiedUser();

    if(localStorage.getItem('globalHospitalId') === null){
      localStorage.setItem('globalHospitalId', "2");        
      localStorage.setItem('globalHospitalName', "Klinik Sehat");          
    }    

    this.onReactiveHospital();    
  }

  async getMenuItems(){
    let res = await this.appService.getMenuByType(MenuType.Admin).toPromise();
    this.menuItems = res;
  }
  async ngOnloadHospitalAsscotiedUser() {
    let res =  await this.clinicService.getHospitalAsscoiateUser().toPromise();
    this.hosModel = res;
    this.hosModel.forEach(el => {
      this.reHospital.push({id: el.id, name: el.name});
    });
  }

  ngOnInit(): void {
    const elemSidebar = <HTMLElement>document.querySelector('.app-inner > .sidebar-panel');
    const elemContent = <HTMLElement>document.querySelector('.app-inner > .mat-sidenav-content');

    if (window.matchMedia(`(min-width: 960px)`).matches && !this.isMac() && !this.compactSidebar) {
      Ps.initialize(elemSidebar, { wheelSpeed: 2, suppressScrollX: true });
      Ps.initialize(elemContent, { wheelSpeed: 2, suppressScrollX: true });
    }

    this.url = this.router.url;

    this._router = this.router.events.filter(event => event instanceof NavigationEnd).subscribe((event: NavigationEnd) => {
      document.querySelector('.app-inner .mat-sidenav-content').scrollTop = 0;
      this.url = event.url;
      this.runOnRouteChange();
    });
  }

  onReactiveHospital() {
    let valHospitalID: string = localStorage.getItem('globalHospitalId'); 
    let valHospitalName: string = localStorage.getItem('globalHospitalName'); 
    
    this.globalHospitalCtrl = new FormControl({id: localStorage.getItem('globalHospitalId'), name: localStorage.getItem('globalHospitalName')});
    this.listHospital = this.globalHospitalCtrl.valueChanges
        .startWith(this.globalHospitalCtrl.value)
        .map(val => this.displayHospital(val))
        .map(name => this.filterHospital(name));
  }

  displayHospital(value: any): string {
    return value && typeof value === 'object' ? value.name : value;
  }

  filterHospital(val: String) {
    if (val) {
      if(this.globalHospitalCtrl.value.id != undefined){
        this.setGlobalHospital();
      }      
      const filterValue = val.toLowerCase();
      return this.reHospital.filter(item => item.name.toLowerCase().indexOf(filterValue) > -1);
    }    
    return this.reHospital;
  } 

  setGlobalHospital() {   
    localStorage.setItem('globalHospitalId', this.globalHospitalCtrl.value.id);
    localStorage.setItem('globalHospitalName', this.globalHospitalCtrl.value.name);                       
    this.router.navigate(['/']);    
  }  

  ngAfterViewInit(): void  {
    this.root.dir = 'ltr';
    this.runOnRouteChange();
  }

  ngOnDestroy(): void  {
    this._router.unsubscribe();
  }

  runOnRouteChange(): void {
    if (this.isOver()) {
      this.sidemenu.close();
    }

    if (window.matchMedia(`(min-width: 960px)`).matches && !this.isMac() && !this.compactSidebar) {
      const elemContent = <HTMLElement>document.querySelector('.app-inner > .mat-sidenav-content');
      Ps.update(elemContent);
    }
  }

  isOver(): boolean {
    if (this.url === '/apps/messages' ||
      this.url === '/apps/calendar' ||
      this.url === '/apps/media' ||
      this.url === '/maps/leaflet' ||
      this.url === '/taskboard') {
      return true;
    } else {
      return window.matchMedia(`(max-width: 960px)`).matches;
    }
  }

  isMac(): boolean {
    let bool = false;
    if (navigator.platform.toUpperCase().indexOf('MAC') >= 0 || navigator.platform.toUpperCase().indexOf('IPAD') >= 0) {
      bool = true;
    }
    return bool;
  }

  menuMouseOver(): void {
    if (window.matchMedia(`(min-width: 960px)`).matches && this.collapseSidebar) {
      this.sidemenu.mode = 'over';
    }
  }

  menuMouseOut(): void {
    if (window.matchMedia(`(min-width: 960px)`).matches && this.collapseSidebar) {
      this.sidemenu.mode = 'side';
    }
  }

  updatePS(): void  {
    if (window.matchMedia(`(min-width: 960px)`).matches && !this.isMac() && !this.compactSidebar) {
      const elemSidebar = <HTMLElement>document.querySelector('.app-inner > .sidebar-panel');
      setTimeout(() => { Ps.update(elemSidebar); }, 350);
    }
  }

  
}
