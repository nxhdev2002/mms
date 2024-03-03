
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { MstInvDemDetFeesServiceProxy } from '@shared/service-proxies/service-proxies';
import { ListErrorImportMstInvDemDetFeesComponent } from './list-error-import-demdetfees-modal.component';

@Component({
    selector: 'import-demdetfees',
    templateUrl: './import-demdetfees.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportDemDetFeesComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModalDemDetFees', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportMstInvDemDetFeesComponent;
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
        private _service: MstInvDemDetFeesServiceProxy,
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportMstInvDemDetFeesFromExcel';
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
                            this.mergeDemDetFees(response.result.gpsStockReceiving[0].guid);
                        }
                    } else {
                        this.exeptionDataImport(response)
                    }
                },
                (error) => {
                    this.exeptionDataImport();
                });
        }else{
            this.notify.warn('Vui lòng chọn file');
        }

    }

    mergeDataMstInvDemDetFees(guid: string) {
        this.isLoading = true;
        this._service.mergeDataMstInvDemDetFees(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
                this.progressBarHidden();
                this.disabled = false;
            });
    }

    mergeDemDetFees(guid) {
        this.mergeDataMstInvDemDetFees(guid);
    }

    showMessImport(guid){
        this._service.getMessageErrorImport(guid)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorListModal.show(guid)
            }
            else{
                this.notify.info('Lưu thành công');
                this.modal.hide();
                this.modalSave.emit(null);
                this.close();

            }
        });
    }

    exeptionDataImport(err?) {
        this.notify.warn(err ? err.error.message : "Dữ liệu không hợp lệ");
        this.progressBarHidden();
        this.close();
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
