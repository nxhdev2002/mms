
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockConceptServiceProxy, InvGpsIssuingServiceProxy, InvGpsIssuingsServiceProxy, GetIssuingImportView } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { log } from 'console';
import { IssuingsComponent } from './issuings.component';
import { ListErrorImportGpsIssuingsComponent } from './list-error-import-issuings-modal.component';

@Component({

  selector: 'import-issuings-modal',
  templateUrl: './import-issuings-modal.component.html',
  styleUrls: ['./import-issuings-modal.component.less'],
  //styleUrls: ['./import-issuings-modal.component.less']
})
export class ImportInvGpsIssuingsComponent   extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportGpsIssuingsComponent;

    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];
    vissibleProgess;
    vissbleInputName;

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;
    typeFile;
    documentNo;
    documentDate;
    shop;
    status;
    costCenter;
    _project;
    listCostCenter = [{ value: '', label: '' }];;
    rowData: GetIssuingImportView = new GetIssuingImportView();
    constructor(
      injector: Injector,
      private _httpClient: HttpClient,
      private _service: InvGpsIssuingsServiceProxy,
      private _serviceissuing: IssuingsComponent
    ) {
      super(injector);
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportGpsIssuingsFromExcel';
      {}
    }
    ngOnInit() {
        this.listCostCenter = [];
        this._service.getListCostCenter().subscribe((result) => {
            result.map(e => {
                this.listCostCenter.push({ value: e.costCenter, label:e.costCenter })
            })
        });
  
    }

    show(): void {
        this.progressBarHidden();
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this._service.getIssuingImportView('').subscribe((e)=> {
            this.rowData = e;
        })

        this.modal.show();
    }

    onChangeCostCenter(v_costCenter){
        this._service.getIssuingImportView(v_costCenter).subscribe((e)=> {
            this.rowData.documentNo = e.documentNo;
        })
    }


    close(): void {
        this.InputVar.nativeElement.value = '';
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImport');
        if (viewName != null) {
                viewName.innerHTML = '';
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.fileName = '';
    }

    onUpload(data: { target: { files: Array<any> } }): void {
        if (data?.target?.files.length > 0) {
            this.formData = new FormData();
            const formData: FormData = new FormData();
            const file = data?.target?.files[0];
            this.fileName = file?.name;
            formData.append('file', file, file.name);
            this.formData = formData;


            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }
    }


    upload() {
        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.material.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeStockConcept(response.result.material.result[0].guid);
                        }
                    }
                },
                (error) => {
                    this.exeptionDataImport();
                });
        }else{
            this.notify.warn('Vui lòng chọn file');
        }
    }

    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close();
    }


    mergeStockConcept(guid) {

        this.isLoading = true;
        this._service.mergeDataInvGpsIssuing(guid,this.rowData.costCenter)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
              this.showMessImport(guid);
            });
    }

    showMessImport(guid) {
        this._service.getMessageErrorImport(guid)
            .subscribe((result) => {

                if (result.items.length > 0) {
                    this.errorListModal.show(guid)
                }
                else {
                    this.notify.info('Lưu thành công');
                    this.modal.hide();
                    this.modalClose.emit(null);
                    this._serviceissuing.searchDatas();
                    this.close();
                }
            });
    }
    progressBarVisible() {

        this.vissbleInputName = 'vissible';
        this.vissibleProgess = 'vissible active';

    }
    progressBarHidden() {

        this.vissibleProgess = '';
        this.vissbleInputName = '';
    }


  }
