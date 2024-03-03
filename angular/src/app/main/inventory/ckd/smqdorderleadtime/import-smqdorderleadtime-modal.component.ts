
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdOrderLeadTimeServiceProxy, InvCkdSmqdOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
//import { ListErrorImportSmqdOrderComponent } from './list-error-import-smqdorder-modal.component';
import { SmqdOrderLeadTimeComponent } from './smqdorderleadtime.component';
import { ListErrorImportSmqdOrderLeadTimeComponent } from './list-error-import-smqdorderleadtime-modal.component';
// import { ListErrorImportSmqdOrderComponent } from './list-error-import-smqdorder-modal.component';

@Component({
    selector: 'import-modal',
    templateUrl: './import-smqdorderleadtime-modal.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportSmqdOrderLeadTimeComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModalSmqdOrderLeadTime', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('errorListModal') errorListModal:| ListErrorImportSmqdOrderLeadTimeComponent| undefined;

    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];
    disabled = false;
    vissibleProgess;
    vissbleInputName;

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;
    orderType: number  ;



    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: InvCkdSmqdOrderLeadTimeServiceProxy,
        private _smqdOrder: SmqdOrderLeadTimeComponent
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportSmqdOrderLeadTimeFromExcel';
        { }
    }
    ngOnInit() {

    }

    show(): void {
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

    upload() {
        if (this.fileName != '') {
            this.progressBarVisible();
            this.disabled = true;
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.smqdOrderLeadTime.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeSMQDOrder(response.result.smqdOrderLeadTime.result[0].guid);
                        }
                    }
                    else if (response.error) {
                        this.exeptionDataImport();
                    }
                },
                error => {
                    console.log('Error:', error);
                    this.exeptionDataImport();
                });
        }
        else{
            this.notify.warn('Vui lòng chọn file');
        }
    }

    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close()
    }

    mergeSMQDOrder(guid) {
        this.isLoading = true;
        this._service.mergeDataInvCkdSmqdOrderLeadTime(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
                // this.notify.info('Lưu thành công');
                    this.modal.hide();
                    this.modalClose.emit(null);
                    this._smqdOrder.searchDatas();
                    this.disabled = false;
                    this.close();
            });
    }

    showMessImport(guid) {
        this._service.getMessageErrorImportOrderLeadTime(guid)
            .subscribe((result) => {
                console.log(result);
                if (result.items.length > 0) {
                    this.errorListModal.show(guid)
                }
                else {
                    this.notify.info('Lưu thành công');
                    this.modal.hide();
                    this.modalClose.emit(null);
                    this._smqdOrder.searchDatas();
                    this.disabled = false;
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
