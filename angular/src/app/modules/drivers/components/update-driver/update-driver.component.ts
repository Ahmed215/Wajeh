import {
  AdminDriverDto, AdminDriverServiceProxy,
  AdminUpdateDriverDto, AdminSubategoryServiceProxy, AdminSubcategoryDtoPagedResultDto,
  CompanyServiceProxy, CompanyDtoPagedResultDto
} from './../../../../../shared/service-proxies/service-proxies';
import { Component, OnInit, ChangeDetectionStrategy, ViewChild, Injector } from '@angular/core';
import { MenuItem, SelectItem } from 'primeng/api';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { AppComponentBase } from '@shared/app-component-base';

import { finalize } from 'rxjs/operators';
import { SaveEditableRow } from 'primeng/table/table';
import { ActivatedRoute } from '@angular/router';
import { isNullOrUndefined } from 'util';
import { message } from '@shared/Message/message';
import { getRemoteServiceBaseUrl } from 'root.module';
import { getCurrentLanguage } from 'root.module';

@Component({
  selector: 'app-update-driver',
  templateUrl: './update-driver.component.html',
  styleUrls: ['./update-driver.component.scss']
})
export class UpdateDriverComponent extends AppComponentBase implements OnInit {
  @ViewChild('Supportingfiles', { static: false }) Supportingfiles;

  firstFormSaving = false;
  forthFormSaving = false;
  driver: AdminDriverDto = new AdminDriverDto();
  _id: any;
  centers;
  isLinear = false;
  items: MenuItem[];
  firstFormGroup: FormGroup;
  PdfFile: File;

  categories: SelectItem[] = [];
  companies: SelectItem[] = [];


  Governorate: SelectItem[] = [];



  basicFormGroupSupportingfiles: FormGroup;
  // userImageSrc: any = "";
  userImageSrc1: any = '';
  userImageSrc2: any = '';
  userImageSrc3: any = '';
  userImageSrc4: any = '';
  userImageSrc5: any = '';
  userImageSrc6: any = '';
  uploadedFiles: any[] = [];
  currentFiles: any[] = [];


  forthFormGroup: FormGroup;
  DataAdditionalfile: any = {};
  UploadImage: any;
  objectAdditionalAttachments: any = {};
  researchAdditionalAttachments: any[] = [];
  FinalResearchAdditionalAttachments: any[] = [];
  FinalResearchAdditionalDescriptions: any[] = [];
  constructor(injector: Injector,
    private _formBuilder: FormBuilder,
    private _personnelService: AdminDriverServiceProxy,
    private _companyService: CompanyServiceProxy,
    private _categoryService: AdminSubategoryServiceProxy,
    private route: ActivatedRoute) {
    super(injector);
  }

  ngOnInit() {
    this.items = [
      { label: this.l('Drivers') },
      { label: this.l('EditeDriver') }
    ];


    this.initFirstFormGroup();
    this.initCatss();
    this.initCompanies();


    this.initForthFormGroup();

    // this.initGovernorates();



    this.basicFormGroupSupportingfiles = this._formBuilder.group({
      controlNameAdditionalFile: ['', Validators.required],
      FileDescription: [''],
    });


    this.route.params.subscribe(params => {
      if (params['id']) {
        this._id = params['id'];
      }
    });

    if (this._id) {
      this._personnelService.get(this._id).subscribe(result => {
        this.driver = result;
        this.patchFirstFormGroup();

        this.userImageSrc1 = getRemoteServiceBaseUrl() + '/' + this.driver.profilePicture;
        this.userImageSrc2 = getRemoteServiceBaseUrl() + '/' + this.driver.identityPicture;
        this.userImageSrc3 = getRemoteServiceBaseUrl() + '/' + this.driver.frontVehiclePicture;
        this.userImageSrc4 = getRemoteServiceBaseUrl() + '/' + this.driver.backVehiclePicture;
        this.userImageSrc5 = getRemoteServiceBaseUrl() + '/' + this.driver.lisencePicture;
        this.userImageSrc6 = getRemoteServiceBaseUrl() + '/' + this.driver.vehicleLisencePicture;
      });
    }
  }

  initCatss() {
    this._categoryService
      .getAll('', undefined, '', 0, 1000)
      .pipe(
        finalize(() => {
        })
      )
      .subscribe((result: AdminSubcategoryDtoPagedResultDto) => {
        const ReqResult = result.items;

        this.categories = [];
        this.categories.push({ label: this.l('vechileType'), value: null });
        ReqResult.forEach(element => {
          this.categories.push({ label: getCurrentLanguage() === 'en' ? element.name : element.nameAr, value: element.id });
        });
      });
  }


  initCompanies() {
    this._companyService
      .getAll('', '', 0, 1000)
      .pipe(
        finalize(() => {
        })
      )
      .subscribe((result: CompanyDtoPagedResultDto) => {
        const ReqResult = result.items;

        this.companies = [];
        this.companies.push({ label: this.l('Company'), value: null });
        ReqResult.forEach(element => {
          this.companies.push({ label: getCurrentLanguage() === 'en' ? element.name : element.nameAr, value: element.id });
        });
      });

  }


  initFirstFormGroup() {
    this.firstFormGroup = this._formBuilder.group({
      isDriverAvilable: ['', []],
      phone: ['', [Validators.required, Validators.maxLength(512)]],
      email: ['', [Validators.required, Validators.maxLength(512)]],
      fullName: ['', [Validators.required, Validators.maxLength(512)]],
      vehicleType: ['', [Validators.required, Validators.maxLength(512)]],
      type: ['', []],
      company: ['', [Validators.required, Validators.maxLength(512)]],
      vehicleSequenceNumber: ['', [Validators.required, Validators.maxLength(512)]],
      driverIdentityNumber: ['', [Validators.required, Validators.maxLength(512)]],
      offDuty: ['']
    });
  }

  initForthFormGroup() {
    this.forthFormGroup = this._formBuilder.group({
    });
  }

  clearInput(name) {
    this.firstFormGroup.get(name).setValue('');
  }
  onFilePDFSelect(files: any) {
    this.PdfFile = files.files[0];
  }
  clearFile(fileUpload) {
    this.PdfFile = null;
    fileUpload.clear();
  }

  ImagePreview(files, imgnum) {
    if (files.length === 0) { return; }

    const mimeType = files[0].type;
    if (mimeType.match(/image\/*/) == null) {
      alert(this.l('OnlyImagesAreSupported'));
      return;
    }

    const reader = new FileReader();
    this.UploadImage = files[0];
    reader.readAsDataURL(files[0]);
    reader.onload = _event => {

      switch (imgnum) {
        case 1:
          this.userImageSrc1 = reader.result;
          break;

        case 2:
          this.userImageSrc2 = reader.result;
          break;
        case 3:
          this.userImageSrc3 = reader.result;
          break;
        case 4:
          this.userImageSrc4 = reader.result;
          break;
        case 5:
          this.userImageSrc5 = reader.result;
          break;
        case 6:
          this.userImageSrc6 = reader.result;
          break;
        default:
          break;
      }
    };
  }
  uploadFileVehicle(imgnum) {
    switch (imgnum) {
      case 1:
        $('.upload-image-register-dealer1').click();
        break;

      case 2:
        $('.upload-image-register-dealer2').click();
        break;
      case 3:
        $('.upload-image-register-dealer3').click();
        break;
      case 4:
        $('.upload-image-register-dealer4').click();
        break;
      case 5:
        $('.upload-image-register-dealer5').click();
        break;
      case 6:
        $('.upload-image-register-dealer6').click();
        break;
      default:
        break;
    }
  }
  RemoveuploadFileRegisterDealer(imgnum) {
    switch (imgnum) {
      case 1:
        this.userImageSrc1 = '';
        break;

      case 2:
        this.userImageSrc2 = '';
        break;
      case 3:
        this.userImageSrc3 = '';
        break;
      case 4:
        this.userImageSrc4 = '';
        break;
      case 5:
        this.userImageSrc5 = '';
        break;
      case 6:
        this.userImageSrc6 = '';
        break;
      default:
        break;
    }
  }

  UploadAnotherFile() {
    $('.form-field-multi-fileUpload p-fileupload .ui-fileupload .ui-fileupload-buttonbar .ui-fileupload-choose input').click();
  }
  removeFile(file: any) {
    const index: number = this.researchAdditionalAttachments.indexOf(file);
    if (index !== -1) {
      this.researchAdditionalAttachments.splice(index, 1);
      this.FinalResearchAdditionalAttachments.splice(index, 1);
      this.FinalResearchAdditionalDescriptions.splice(index, 1);
    }
  }
  onAdditionalFileSelect(files: any) {
    this.DataAdditionalfile = files.files[0];
    this.basicFormGroupSupportingfiles.get('controlNameAdditionalFile').setValue(files.files[0].name);
  }
  removeFileAdditionalFile(file: any, AdditionalFile) {
    AdditionalFile.value = '';
    this.basicFormGroupSupportingfiles.get('controlNameAdditionalFile').setValue('');
  }
  fieldTextarea(name) {
    this.basicFormGroupSupportingfiles.get('FileDescription').setValue('');
  }
  AddSupportingfiles() {
    const objectTypeURL = new Object(this.DataAdditionalfile) as any;
    let objectURL = '';
    if (objectTypeURL.type === 'application/pdf') {
      objectURL = 'pdf';
    } else if (objectTypeURL.type === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
      objectURL = 'wordprocessingml';
    } else if (objectTypeURL.type === 'image/png' || objectTypeURL.type === 'image/jpeg') {
      objectURL = 'image';
    } else {
      objectURL = 'wordprocessingml';
    }
    if (this.basicFormGroupSupportingfiles.valid) {
      this.objectAdditionalAttachments = {
        name: this.DataAdditionalfile.name,
        lastModified: this.DataAdditionalfile.lastModified,
        lastModifiedDate: this.DataAdditionalfile.lastModifiedDate,
        webkitRelativePath: this.DataAdditionalfile.webkitRelativePath,
        size: this.DataAdditionalfile.size,
        type: this.DataAdditionalfile.type,
        objectURL: {
          changingThisBreaksApplicationSecurity: objectURL === 'image' ?
            this.DataAdditionalfile.objectURL.changingThisBreaksApplicationSecurity : objectURL
        },
        FileDescription: this.basicFormGroupSupportingfiles.get('FileDescription').value
      };
      this.researchAdditionalAttachments.push(this.objectAdditionalAttachments);
      this.FinalResearchAdditionalAttachments.push(this.DataAdditionalfile);
      this.FinalResearchAdditionalDescriptions.push(this.objectAdditionalAttachments.FileDescription);

      this.Supportingfiles.hide();
      this.basicFormGroupSupportingfiles.reset();
    } else {
      this.Supportingfiles.hide();
      this.basicFormGroupSupportingfiles.reset();
    }
  }

  patchFirstFormGroup() {
    // console.log(this.categories.find(c => c.value === this.driver.vehicleType));
    // console.log(this.companies.find(c => c.value === this.driver.companyId));
    // console.log(this.driver.vehicleType);
    // console.log(this.driver.companyId);
    this.firstFormGroup.patchValue({
      isDriverAvilable: this.driver.isDriverAvilable,
      offDuty: !this.driver.offDuty,
      phone: this.driver.phone,
      fullName: this.driver.fullName,
      email: this.driver.email,
      vehicleType: this.categories.find(c => c.value === this.driver.vehicleType),
      company: this.companies.find(c => c.value === this.driver.companyId),
      vehicleSequenceNumber: this.driver.vehicleSequenceNumber,
      driverIdentityNumber: this.driver.driverIdentityNumber,
    });

  }

  onFirstFromSubmit() {
    this.firstFormSaving = true;
    if (this.firstFormGroup.valid) {
      if (this.firstFormGroup.value.isDriverAvilable === '') {
        this.driver.isDriverAvilable = false;
      } else {
        this.driver.isDriverAvilable = this.firstFormGroup.value.isDriverAvilable;
      }

      this.driver.id = this._id;
      this.driver.phone = this.firstFormGroup.value.phone;
      this.driver.fullName = this.firstFormGroup.value.fullName;
      this.driver.email = this.firstFormGroup.value.email;
      this.driver.vehicleSequenceNumber = this.firstFormGroup.value.vehicleSequenceNumber;
      this.driver.driverIdentityNumber = this.firstFormGroup.value.driverIdentityNumber;
      this.driver.vehicleType = this.firstFormGroup.value.vehicleType.value;
      this.driver.companyId = this.firstFormGroup.value.company.value;
      this.driver.offDuty = !this.firstFormGroup.get('offDuty').value;
    }
  }


  onForthFromSubmit() {
    if (this.firstFormSaving === false) {
      this.onFirstFromSubmit();
    }

    this.forthFormSaving = true;

    if (this.firstFormGroup.valid &&
      this.forthFormGroup.valid) {

      this.driver.profilePicture = (<string>this.userImageSrc1).slice((<string>this.userImageSrc1).indexOf(',') + 1);
      this.driver.identityPicture = (<string>this.userImageSrc2).slice((<string>this.userImageSrc2).indexOf(',') + 1);
      this.driver.frontVehiclePicture = (<string>this.userImageSrc3).slice((<string>this.userImageSrc3).indexOf(',') + 1);
      this.driver.backVehiclePicture = (<string>this.userImageSrc4).slice((<string>this.userImageSrc4).indexOf(',') + 1);
      this.driver.lisencePicture = (<string>this.userImageSrc5).slice((<string>this.userImageSrc5).indexOf(',') + 1);
      this.driver.vehicleLisencePicture = (<string>this.userImageSrc6).slice((<string>this.userImageSrc6).indexOf(',') + 1);
      this.save();
    }
  }

  save() {
    this._personnelService
      .updateProfile(<AdminUpdateDriverDto>this.driver)
      .pipe(
        finalize(() => {
        })
      )
      .subscribe(() => {
        message.Toast.fire(this.l('SavedSuccessfully'));
      });
  }
}
