import {
  AdminDriverDto, AdminDriverServiceProxy, AdminUpdateDriverDto,
  AdminCategoryServiceProxy, AdminCategoryDtoPagedResultDto,
  AdminSubategoryServiceProxy, AdminSubcategoryDtoPagedResultDto,
  CompanyServiceProxy, CompanyDtoPagedResultDto, AdminCreateDriverDto
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
import { getCurrentLanguage } from 'root.module';

@Component({
  selector: 'app-create-driver',
  templateUrl: './create-driver.component.html',
  styleUrls: ['./create-driver.component.scss']
})
export class CreateDriverComponent extends AppComponentBase implements OnInit {
  @ViewChild('Supportingfiles', { static: false }) Supportingfiles;

  firstFormSaving = false;
  forthFormSaving = false;
  driver: AdminCreateDriverDto = new AdminCreateDriverDto();
  _id: any;
  centers;
  isLinear = false;
  items: MenuItem[];
  firstFormGroup: FormGroup;
  PdfFile: File;




  Governorate: SelectItem[] = [];
  categories: SelectItem[] = [];
  companies: SelectItem[] = [];


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
    private _formBuilder: FormBuilder, private _personnelService: AdminDriverServiceProxy,
    private route: ActivatedRoute, private _categoryService: AdminSubategoryServiceProxy,
    private _companyService: CompanyServiceProxy) {
    super(injector);
  }

  ngOnInit() {
    this.items = [
      { label: this.l('Drivers') },
      { label: this.l('AddDriver') }
    ];


    this.initFirstFormGroup();


    this.initForthFormGroup();
    this.initCatss();
    this.initCompanies();
    // this.initGovernorates();




    this.basicFormGroupSupportingfiles = this._formBuilder.group({
      controlNameAdditionalFile: ['', Validators.required],
      FileDescription: [''],
    });
  }


  initFirstFormGroup() {
    this.firstFormGroup = this._formBuilder.group({
      isDriverAvilable: ['', []],
      offDuty: [''],
      phone: ['', [Validators.required, Validators.maxLength(512)]],
      email: ['', [Validators.required, Validators.maxLength(512)]],
      fullName: ['', [Validators.required, Validators.maxLength(512)]],
      vehicleSequenceNumber: ['', [Validators.required, Validators.maxLength(512)]],
      driverIdentityNumber: ['', [Validators.required, Validators.maxLength(512)]],
      vehicleType: ['', [Validators.required, Validators.maxLength(512)]],
      type: ['', []],
      company: ['', [Validators.required, Validators.maxLength(512)]],
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





  onFirstFromSubmit() {
    this.firstFormSaving = true;
    if (this.firstFormGroup.valid) {
      if (this.firstFormGroup.value.isDriverAvilable === '') {
        this.driver.isDriverAvilable = false;
      } else {
        this.driver.isDriverAvilable = this.firstFormGroup.value.isDriverAvilable;
      }
      this.driver.offDuty = !this.firstFormGroup.value.offDuty;
      this.driver.phone = this.firstFormGroup.value.phone;
      this.driver.fullName = this.firstFormGroup.value.fullName;
      this.driver.email = this.firstFormGroup.value.email;
      this.driver.vehicleType = this.firstFormGroup.value.vehicleType.value;
      this.driver.companyId = this.firstFormGroup.value.company.value;
      this.driver.vehicleSequenceNumber = this.firstFormGroup.value.vehicleSequenceNumber;
      this.driver.driverIdentityNumber = this.firstFormGroup.value.driverIdentityNumber;
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
          const itemname = getCurrentLanguage() === 'en' ? element.name : element.nameAr;
          this.categories.push({ label: itemname, value: element.id });
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
        debugger;
        this.companies = [];
        this.companies.push({ label: this.l('Company'), value: null });
        ReqResult.forEach(element => {
          this.companies.push({ label: getCurrentLanguage() === 'en' ? element.name : element.nameAr, value: element.id });
        });
      });

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
    this.driver.phone = this.ValidateMobileNumber(this.driver.phone);
    this._personnelService
      .createProfile(this.driver)
      .pipe(
        finalize(() => {
        })
      )
      .subscribe(() => {
        message.Toast.fire(this.l('SavedSuccessfully'));
      });
  }

  ValidateMobileNumber(mobileNumber: string): string {
    if (mobileNumber.startsWith('05')) {
      mobileNumber = mobileNumber.substr(1, mobileNumber.length - 1);
    } else if (mobileNumber.startsWith('00966')) {
      mobileNumber = mobileNumber.substr(5, mobileNumber.length - 5);
    }
    return mobileNumber;
  }
}
