
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { InvCkdSmqdServiceProxy, MstInvDemDetDaysServiceProxy } from '@shared/service-proxies/service-proxies';
import { ListErrorImportMstInvDemDetDaysComponent } from './list-error-import-demdetdays-modal.component';

@Component({
    selector: 'import-demdetdays',
    templateUrl: './import-demdetdays.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportDemDetDaysComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModalDemDetDays', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportMstInvDemDetDaysComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    disabled = false;
    vissibleProgess;
    vissbleInputName;
    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: MstInvDemDetDaysServiceProxy,
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportMstInvDemDetDaysFromExcel';
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
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImport');
        if (viewName != null) {
            viewName.innerHTML = '';
        }
        this.modal.hide();
        this.modalClose.emit(null);
        this.fileName = '';
        this.progressBarHidden()

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
    mergeDataMstInvDemDetDays(guid: string) {
        this.isLoading = true;
        this._service.mergeDataMstInvDemDetDays(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
                this.close();
                this.disabled = false;
            });
    }
    mergeDemDetDays(guid) {
        this.mergeDataMstInvDemDetDays(guid);
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
                        if (response.result.gpsStockReceiving.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeDemDetDays(response.result.gpsStockReceiving[0].guid);
                        }
                    } else {
                        this.exeptionDataImport(response)
                    }
                },
                    (error) => {
                        this.exeptionDataImport();
                    });
        } else {
            this.notify.warn('Vui lòng chọn file');
        }

    }
    exeptionDataImport(err?) {
        this.notify.warn(err ? err.error.message : "Dữ liệu không hợp lệ");
        this.progressBarHidden();
        this.close();
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
                    this.modalSave.emit(null);
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
