import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { HttpClient} from '@angular/common/http';
import { StringHelper, Base64Helper } from '../libs';

//config API & Grab everything with import model;
import { ServiceConfiguration } from '../app.config';
import { MedicalStaffModel, PaginationEntity, 
    MedicalStaffStatus, MedicalStaffImageModel, MedicalHospitalStaffViewModel, 
    HospitalStaffOperatingHoursModel, HospitalMedicalStaffModel } from '../model';


@Injectable()
export class StaffService{
    private _baseUri: string;
    private slugOpertingHour: string = "operatinghours";    
    private slugDoctor: string = "doctor";
    private slugRegistered: string = "registered";    
    private slugNonRegistered: string = "nonregistered";    
    
    constructor(protected http: HttpClient){
        this._baseUri = new ServiceConfiguration().getApiUri() + "staff";
    }

    getDoctorAdminBypaging(page: number, itemPerpage: number, clue: string): Observable<PaginationEntity<MedicalStaffModel>> {
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, page, itemPerpage, clue);
        return this.http.get<PaginationEntity<MedicalStaffModel>>(uri);
    }

    getAllDoctorByPaging(hospitalId: number, page: number, itemPerpage: number): Observable<PaginationEntity<MedicalStaffModel>>
    {
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, this.slugRegistered, hospitalId, page, itemPerpage); 
        return this.http.get<PaginationEntity<MedicalStaffModel>>(uri);
    }

    getAllDoctorBySearchPaging(hospitalId: number, page: number, itemPerpage: number, clue: string): Observable<PaginationEntity<MedicalStaffModel>>
    {
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, this.slugRegistered, hospitalId, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<MedicalStaffModel>>(uri);
    }    

    getDoctorAndOpertingHour(hospitalId: number, page: number, itemPerpage: number, clue: string): Observable<PaginationEntity<MedicalStaffModel>>
    { 
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, this.slugOpertingHour, hospitalId, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<MedicalStaffModel>>(uri);
    }

    getDoctorNonRegiteredByPaging(hospitalId: number, page: number, itemPerpage: number, clue: string): Observable<PaginationEntity<MedicalStaffModel>>
    {
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, this.slugNonRegistered, hospitalId, page, itemPerpage, clue); 
        return this.http.get<PaginationEntity<MedicalStaffModel>>(uri);
    }    
 
    changeStatus(id:number, status:MedicalStaffStatus):Observable<MedicalStaffModel>{
        let uri = StringHelper.concat("/", this._baseUri, this.slugDoctor, "status", id);
        return this.http.put<MedicalStaffModel>(uri, status);
    }    

    CoverImage(staffId:number, filename:string, base64:string):Observable<MedicalStaffImageModel>{
        var blob = Base64Helper.toBlob(base64);
        var formData = new FormData();       
        formData.append('file', blob, filename);

        
        let uri = StringHelper.concat("/", this._baseUri, "CoverImage", staffId);
            return this.http.put<MedicalStaffImageModel>(uri, formData);
    }

    getDetail(id:number):Observable<MedicalStaffModel>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.get<MedicalStaffModel>(uri);
    }

    save(param:MedicalStaffModel): Observable<MedicalStaffModel>{
        let uri = this._baseUri + "/" + this.slugDoctor;
        if(param.id == 0){
            return this.http.post<MedicalStaffModel>(uri, param);
        }else{
            uri = uri + '/' + param.id;
            return this.http.put<MedicalStaffModel>(uri, param);
        }
    }

    delete(id: number): Observable<string>{
        let uri = StringHelper.concat("/", this._baseUri, id);
        return this.http.delete<string>(uri)   
    }  

    assignToHospital(model: MedicalHospitalStaffViewModel): Observable<MedicalHospitalStaffViewModel>{
        let uri = StringHelper.concat("/", this._baseUri, "assigntohospital");
        return this.http.post<MedicalHospitalStaffViewModel>(uri, model);
    }

    getOperatingHours(hospitalIds: number, staffIds: number): Observable<HospitalMedicalStaffModel> {
        let uri = StringHelper.concat("/", this._baseUri, "operatinghours", hospitalIds, staffIds);
        return this.http.get<HospitalMedicalStaffModel>(uri);
    }

    updateOpertingHours(model: HospitalMedicalStaffModel, hospitalIds: number, staffIds: number): Observable<Array<HospitalStaffOperatingHoursModel>> {
        let uri = StringHelper.concat("/", this._baseUri, "operatinghours", hospitalIds, staffIds);
        return this.http.put<Array<HospitalStaffOperatingHoursModel>>(uri, model);
    }
}