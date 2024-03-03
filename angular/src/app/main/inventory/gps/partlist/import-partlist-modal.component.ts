
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { PartListComponent } from './partlist.component';
import { ListErrorImportGpsPartListComponent } from './list-error-import-part-list-modal.component';
// import { ListErrorImportInvGpsPartListComponent } from './import-partlistnocolor-modal.component';

@Component({
  selector: 'import-partlist-modal',
  templateUrl: './import-partlist-modal.component.html',
  styleUrls: ['../../../import-modal.less'],
})
export class ImportPartListComponent   extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportGpsPartListComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];
    disabled = false;
    vissibleProgess;
    vissbleInputName;
    checkPartListColor;

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;
    uploadLotUrl: string;

    constructor(
      injector: Injector,
      private _httpClient: HttpClient,
      private _service: InvGpsPartListServiceProxy,
      private _gpspartList: PartListComponent
    ) {
      super(injector);
      this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportPartListNoColorFromExcel';
      this.uploadLotUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportPartListFromExcel';
      {}
    }
    ngOnInit() {

    }

    show(cs:string): void {
        this.checkPartListColor = cs;
        this.progressBarHidden();
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this.modal.show();
    }

    close(): void {
        this.InputVar.nativeElement.value = '';
        this.fileName = '';
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImport');
        if (viewName != null) {
            viewName.innerHTML = '';
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.isLoading = false;
    }

    onUpload(data: { target: { files: Array<any> } }): void {
        if (data?.target?.files.length > 0) {
            this.formData = new FormData();
            const formData: FormData = new FormData();
            const file = data?.target?.files[0];
            this.fileName = file?.name;
            formData.append('file', file, file.name);
            this.formData = formData;
            this.disabled = false;
            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }
    }

    uploadColor() {
        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadLotUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.gentani.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeGpspartListColor(response.result.gentani.result[0].guid);
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
    uploadNoColor() {

        console.log("upload No Color");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.partListNoColor.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeGpspartListNoColor(response.result.partListNoColor.result[0].guid);
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
        this.close()
    }

    mergeDataInvGpsPartList(guid: string, type: string) {
        this.isLoading = true;
        this._service.mergeDataInvGpsPartList(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid,type);
                this.progressBarHidden();
                this.disabled = false;
            });
    }
    mergeDataInvGpsPartListNoColor(guid: string, type: string) {
        this.isLoading = true;
        this._service.mergeDataInvGpsPartListNoColor(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid,type);
                this.progressBarHidden();
                this.disabled = false;
            });
    }

    mergeGpspartListColor(guid) {
        this.mergeDataInvGpsPartList(guid, 'Color');
    }

    mergeGpspartListNoColor(guid) {
        this.mergeDataInvGpsPartListNoColor(guid, 'NoColor');
    }
    showMessImport(guid,screen){
        this._service.getMessageErrorImport(guid,screen)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorListModal.show(guid,screen)
            }
            else{
                this.notify.info('Lưu thành công');
                this.modal.hide();
                this.modalClose.emit(null);
                this._gpspartList.searchDatas();
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
