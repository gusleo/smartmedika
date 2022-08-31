import { Component, OnInit, ViewChild } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { MdDialog, MdDialogRef, MdDialogConfig } from "@angular/material";
import { Observable } from "rxjs/Observable";
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl
} from "@angular/forms";
import { CustomValidators } from "ng2-validation";
import { EnumValues } from "enum-values";
import { DialogConfig, FileUploadAPI } from "../../";
import { ImageEditorDialog } from "../../media";
import { ImageNotAvailable } from "../../app.config";
import { Message } from "../../../libs";

// config API & Grab everything with import model;
import {
  MedicalStaffModel,
  HospitalStatus,
  RegionModel,
  PaginationEntity,
  RegencyModel,
  MedicalStaffSpesialisModel,
  MedicalStaffSpecialistMapModel,
  MedicalStaffSpesialisViewModel,
  MedicalSpecialistViewModel,
  MedicalStaffStatus,
  MedicalStaffType
} from "../../model";
import {
  StaffService,
  GeoLocationService,
  SpecialistService
} from "../../services";

@Component({
  selector: "app-register-doctor",
  templateUrl: "./register-doctor.component.html",
  styleUrls: ["./register-doctor.component.scss"]
})
export class RegisterDoctorComponent implements OnInit {
  doctorImage: string = ImageNotAvailable;
  filename: string;
  isNewImage: boolean = false;

  //autoComplete
  currentRegency: any;
  regencys: Observable<RegencyModel[]>;
  regencyContent: RegencyModel[] = [];

  public titleForm: string;
  model = new MedicalStaffModel();
  resultRegion: Array<RegionModel>;
  regionCtrl: FormControl;

  resultSpecialist: Array<MedicalStaffSpesialisModel>;
  specialistView: Array<MedicalStaffSpesialisViewModel>;

  public form: FormGroup;
  ////for degree
  degreeList: any[] = [];
  hospitalId: number = 2;
  id: number = 0;
  onRequest: boolean = false;

  imageEditorDialog: MdDialogRef<ImageEditorDialog>;

  constructor(
    private route: ActivatedRoute,
    private staffService: StaffService,
    private geoServices: GeoLocationService,
    private specialistService: SpecialistService,
    private formBuilder: FormBuilder,
    public dialog: MdDialog,
    private message: Message,
    private router: Router
  ) {
    // set value default
    this.titleForm = "Form Dokter";
    this.route.params.subscribe(params => {
      let id = Number.parseInt(params["id"]);
      if (Number.isNaN(id)) {
        this.id = 0;
      } else {
        this.id = id;
      }
    });

    this.createform();
  }

  async ngOnInit() {
    this.getinfoDegree();
    await this.getInfoSpecialist();
    this.getStaffDetail(this.id);
  }

  createform() {
    // setting for validation
    this.form = this.formBuilder.group({
      degreeMessage: [null, Validators.compose([Validators.required])],
      firstName: [null, Validators.compose([Validators.required])],
      lastName: [null, Validators.compose([Validators.required])],
      medicalStaffRegisteredNumber: [
        null,
        Validators.compose([Validators.required])
      ],
      primaryPhone: [
        null,
        Validators.compose([Validators.required, CustomValidators.number])
      ],
      secondaryPhone: [null, Validators.compose([CustomValidators.number])],
      address: [""],
      email: [""],
      regency: [null, Validators.compose([Validators.required])]
    });
    this.initRegencyAutoComplete();
  }

  initRegencyAutoComplete() {
    this.regencys = this.form
      .get("regency")
      .valueChanges.debounceTime(400)
      .do(value => {
        let exist = this.regencyContent.findIndex(t => t.name === value);
        if (exist > -1) return;

        // get data from the server. my response is an array [{id:1, text:'hello world'}]
        this.geoServices
          .getRegencyByClue(1, 1000, false, value)
          .subscribe((res: PaginationEntity<RegencyModel>) => {
            this.regencyContent = res.items;
          });
      })
      .delay(500)
      .map(() => this.regencyContent);
  }

  displayFn(value: RegencyModel): string {
    return value && typeof value === "object"
      ? value.name + " (" + value.region.name + ")"
      : "";
  }

  getinfoDegree() {
    this.degreeList.push({ code: "dr.", name: "dr." });
    this.degreeList.push({ code: "Dr.", name: "Dr." });
    this.degreeList.push({ code: "Prof.", name: "Prof." });
  }

  async getInfoSpecialist() {
    let res = await this.specialistService.getAll().toPromise();
    this.specialistView = new Array<MedicalStaffSpesialisViewModel>();
    res.forEach(element => {
      this.specialistView.push({
        id: element.id,
        name: element.name,
        alias: element.alias,
        isChecked: false
      });
    });
  }

  async getStaffDetail(id: number) {
    if (id > 0) {
      this.model = await this.staffService.getDetail(id).toPromise();
      if (this.model) {
        let img = this.model.images.filter(
          image => image.image.isPrimary == true
        );
        if (img.length > 0) {
          this.doctorImage = img[0].image.imageUrl;
        }
        this.currentRegency = this.model.regency;
        this.getSpecialistDoctor();
        //if api operating hour for doctor in down make to check if avaible of data
      }
    } else {
      this.model = new MedicalStaffModel();
    }
  }
  getSpecialistDoctor() {
    if (this.model.medicalStaffSpecialists != null) {
      this.model.medicalStaffSpecialists.forEach(item => {
        this.specialistView.forEach(element => {
          if (element.id == item.medicalStaffSpecialistId) {
            element.isChecked = true;
            return false;
          }
        });
      });
    }
  }

  imageChange($event: any) {
    let file: File = $event.target.files[0];
    this.filename = file.name;
    let fileReader = new FileReader();
    var img;
    if (file) {
      fileReader.onload = (event: any) => {
        this.openImageEditorDialog(event.target.result);
      };
      fileReader.readAsDataURL(file);
    }
  }

  openImageEditorDialog(base64: any) {
    let config: any = DialogConfig;
    config.data = {
      base64: base64
    };
    config.width = "";
    config.height = "";

    let that = this;
    this.imageEditorDialog = this.dialog.open(ImageEditorDialog, config);
    this.imageEditorDialog.afterClosed().subscribe(result => {
      if (result != undefined) {
        that.doctorImage = result.base64;
        that.isNewImage = true;
      }
      this.imageEditorDialog = null;
    });
  }

  onDegreeChange(event) {
    this.model.title = event;
  }

  async submitDoctor() {
    let message =
      this.id == 0 ? "Sukses menambah dokter" : "Sukses merubah profil dokter";

    this.onRequest = true;
    await this.updateDoctor();

    if (this.isNewImage) {
      await this.updateCoverImage();
    }
    this.onRequest = false;
    this.message.open(message);
  }

  async updateCoverImage() {
    let result = await this.staffService
      .CoverImage(this.id, this.filename, this.doctorImage)
      .toPromise();
  }

  async updateDoctor() {
    let message =
      this.id == 0 ? "Sukses menambah dokter" : "Sukses merubah profil dokter";

    this.onRequest = true;
    let selectSpecialist = this.getSelectedSpecialist();
    this.model.medicalStaffSpecialists = selectSpecialist;

    //awaiting for api operating hour for doctor

    if (this.currentRegency != null) {
      this.model.regencyId = this.currentRegency.id;
    }
    this.model.staffType = MedicalStaffType.Doctor;
    this.model.status = MedicalStaffStatus.Pending;
    let respon = await this.staffService.save(this.model).toPromise();
    this.id = respon.id;
    this.onRequest = false;
    this.message.open(message);
  }

  getSelectedSpecialist(): Array<MedicalStaffSpecialistMapModel> {
    let selectSpecialist = new Array<MedicalStaffSpecialistMapModel>();

    let that = this;
    this.specialistView.forEach(el => {
      if (el.isChecked) {
        let specially: MedicalStaffSpecialistMapModel = new MedicalStaffSpecialistMapModel(
          0,
          that.id,
          el.id
        );
        selectSpecialist.push(specially);
      }
    });
    return selectSpecialist;
  }

  backClicked(): void {
    //this._location.back();
    this.router.navigate(["/doctor/list"], { skipLocationChange: true });
  }

  async saveContinue() {
    let message =
      this.id == 0 ? "Sukses menambah dokter" : "Sukses merubah profil dokter";

    this.onRequest = true;
    await this.updateDoctor();

    if (this.isNewImage) {
      await this.updateCoverImage();
    }

    this.model = new MedicalStaffModel();
    this.id = 0;
    this.getInfoSpecialist();

    this.onRequest = false;
    this.message.open(message);
  }
}
