
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { InvCkdSmqdServiceProxy } from '@shared/service-proxies/service-proxies';
import { SmqdComponent } from './smqdmanagement.component';
import { ListErrorImportSmqdModalComponent } from './list-error-import-smqd-modal.component';

@Component({

    selector: 'import-smqd-pxpinout',
    templateUrl: './import-smqd-pxpinout.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportInvCkdSmqdPxpInOutComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importPxPInOutModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportSmqdModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    importType;
    disabled = false;
    vissibleProgess;
    vissbleInputName;
    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];
    uploadUrlIn;
    uploadUrlReturn;
    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;
    uploadUrlOut: string;
    radiovalue;
    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: InvCkdSmqdServiceProxy,
        private _confirmLot: SmqdComponent,

    ) {
        super(injector);

        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportInvCkdSmqdFromExcel';
        this.uploadUrlOut = AppConsts.remoteServiceBaseUrl + '/Prod/ImportSmqdPxPOtherOutFromExcel';
        this.uploadUrlIn = AppConsts.remoteServiceBaseUrl + '/Prod/ImportSmqdPxPInFromExcel';
        this.uploadUrlReturn = AppConsts.remoteServiceBaseUrl + '/Prod/ImportSmqdPxPReturnFromExcel';
        { }
    }
    ngOnInit() {

    }

    show(importType): void {
        this.radiovalue = importType;
        this.progressBarHidden();
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this.modal.show();
        console.log(importType);
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
            formData.append('importType', this.importType);
            this.formData = formData;
            this.disabled = false;
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
                        console.log(response.result.smqd.length);
                        if (response.result.smqd.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeCkdSmqd(response.result.smqd[0].guid, this.radiovalue);
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

    uploadOut() {
        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrlOut, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.smqd.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeCkdSmqdPxpOut(response.result.smqd[0].guid, this.radiovalue);
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

    uploadReturn() {
        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrlReturn, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.smqd.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeCkdSmqdPxpReturn(response.result.smqd[0].guid, this.radiovalue);
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

    
    uploadIn() {
        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrlIn, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.smqd.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeCkdSmqdPxpIn(response.result.smqd[0].guid, this.radiovalue);
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


    mergeCkdSmqd(guid, type) {
        this._service.mergeDataInvCkdSmqd(guid,type)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
            });
    }

    mergeCkdSmqdPxpOut(guid: string, type: string) {
        this._service.mergeDataSmqdPxpOtherOut(guid, type)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
            });
    }


    mergeCkdSmqdPxpIn(guid: string, type: string) {
        this._service.mergeDataSmqdPxpIn(guid, type)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
            });
    }

    mergeCkdSmqdPxpReturn(guid: string, type: string) {
        this._service.mergeDataSmqdPxpReturn(guid, type)
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
                    this._confirmLot.searchDatas();
                    this.disabled = false;
                    this.close();
                }
            });
    }


    progressBarVisible() {
        this.disabled = true;
        this.vissbleInputName = 'vissible';
        this.vissibleProgess = 'vissible active';

    }
    progressBarHidden() {
        this.disabled = false;
        this.vissibleProgess = '';
        this.vissbleInputName = '';
    }
}
