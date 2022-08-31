export enum MetaType {
   Phones = 0,
   OperatingHours = 1,
   PhotoGallery = 2,
   Email = 3
}

export enum HospitalStatus {
    InActive = 0,
    Active = 1,
    Suspended = 2
}

export enum HospitalStaffStatus {
    InActive = 0,
    Active = 1,
    Suspended = 2
}

export enum MedicalStaffType {
    Nurse = 0,    
    Doctor = 1,
    MidWife = 2,
    Therapist = 3
}

export enum MedicalStaffStatus {
    InActive = 0,
    Active = 1,
    Death = 2,
    Suspended = 3,
    Pending = 4    
}

export enum ArticleStatus {   
    UnConfirmed = 0,
    Confirmed = 1,
    Archive = 2,
    Draft = 3    
}

export enum Status {
    InActive = 0,
    Active = 1
}

export enum PatientStatus {
    InActive = 0,
    Active = 1,
    Death = 2    
}

export enum RelationshipStatus {
    Spouse = 0,
    Parent = 1,
    Child = 2,
    Other = 3
}

export enum AppointmentCanceled {
    NotCanceled = 0,
    CanceledByStaff = 1,
    CanceledByUser = 2
}

export enum Gender {
    Male = 0,
    Female = 1
}

export enum AppointmentStatus {
    Cancel = 0,
    Finish = 1,
    Pending  = 2,
    Process = 3
}

export enum MenuType {
    Admin = 1,
    Client = 0
}

export enum AdvertisingType {
    Popup = 0,
    Slider = 1
}
