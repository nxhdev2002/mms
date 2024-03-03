import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPVServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { FramePlanBMPVComponent } from './frameplanbmpv.component';
import { ListErrorImportFramePlanBMPVComponent } from './list-error-import-frameplanbmpv-modal.component';

@Component({
    selector: 'import-frameplanbmpv-modal',
    templateUrl: './import-frameplanbmpv-modal.component.html',
    styleUrls: ['./import-frameplanbmpv-modal.component.less'],
})
export class ImportFramePlanBMPVComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('errorListModal') errorListModal:| ListErrorImportFramePlanBMPVComponent| undefined;

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

    constructor(injector: Injector,
        private _httpClient: HttpClient,
        private _service: FrmAdoFramePlanBMPVServiceProxy,
        private _framePlanBMPV: FramePlanBMPVComponent
        ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportFramePlanBMPVFromExcel';
        {
        }
    }
    ngOnInit() {}

    show(): void {
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

            //view name file import
            let viewName = document.getElementById('viewNameFileImport');
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
                viewName.style.color = 'red';
            }
        }
    }

    upload() {
        if(this.fileName != ''){
        this.isLoading = true;
        this._httpClient
            .post<any>(this.uploadUrl, this.formData)
            .pipe(
                finalize(() => {
                    this.excelFileUpload?.clear();
                    this.isLoading = true;
                })
            )
            .subscribe((response) => {
                if (response.success) {
                    this.mergeFramePlanBMPV(response.result.framPlanBMPV.result[0].guid);
                } else if (response.error != null || !response.result.framPlanBMPV) {
                    this.notify.warn('Dữ liệu không hợp lệ');
                }
            });
        }
    }

    mergeFramePlanBMPV(guid) {
        this.isLoading = true;
        this._service
            .mergeDataFramePlanBMPV(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
            });
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
                this.modalClose.emit(null);
                this._framePlanBMPV.searchDatas();
                this.close();
            }
        });
    }

    refresh() {
        this.InputVar.nativeElement.value = '';
        this.fileName = '';
        this.formData = new FormData();
        this.InputVar.nativeElement.click();
    }
    progressBarVisible(){
        var VissibleProgressBar = document.getElementById("loader-line" ).style.visibility = "visible";
        var viewName = document.getElementById('viewNameFileImport').style.visibility = 'hidden';

        var hideButtom = document.querySelectorAll<HTMLElement>('.modal-body .ui-md-12');
        (hideButtom[0] as HTMLElement).style.visibility = 'hidden';

    }
    progressBarHidden(){
        var HiddenProgressBar = document.getElementById("loader-line").style.visibility = "hidden"
        var viewName = document.getElementById('viewNameFileImport').style.visibility = 'visible'
        var hideButtom = document.querySelectorAll<HTMLElement>('.modal-body .ui-md-12');
        (hideButtom[0] as HTMLElement).style.visibility = 'visible';

    }
}
