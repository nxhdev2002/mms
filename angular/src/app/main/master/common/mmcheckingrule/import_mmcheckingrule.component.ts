import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import {  MstCmmMMCheckingRuleServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { MMCheckingRuleComponent } from './mmcheckingrule.component';
import { ListErrorImportMMCheckingRuleComponent } from './list-error-import-mmcheckingrule-modal.component';

@Component({
    selector: 'import_mmcheckingrule',
    templateUrl: './import_mmcheckingrule.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportMMCheckingRuleComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importMMModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportMMCheckingRuleComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    disabled = false;
    vissbleFileName;
    vissibleProgess;
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
        private _service: MstCmmMMCheckingRuleServiceProxy,
        private _mmcheckingrule: MMCheckingRuleComponent,

    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportMstCmmMMCheckingRuleFromExcel';
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
            this.vissbleFileName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }

    }


    upload() {
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {

                        if (response.result.rule.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeDataMstCmmMMCheckingRule(response.result.rule.result[0].guid);
                        }
                    }
                    else 
                    {
                        this.exeptionDataImport();
                    }
                },
                    error => {
                        console.log('Error:', error);
                        this.exeptionDataImport();
                    });
        }
        else {
            this.notify.warn('Vui lòng chọn file');
        }
    }
    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close()
    }


    mergeDataMstCmmMMCheckingRule(guid) {
        this._service.mergeDataMstCmmMMCheckingRuleImport(guid)
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
                    this._mmcheckingrule.searchDatas();
                    this.close();
                }
            });
    }

    progressBarVisible() {
        this.disabled = true;
        this.vissbleFileName = 'vissible'
        this.vissibleProgess = 'vissible active'

    }
    progressBarHidden() {
        this.disabled = false;
        this.vissbleFileName = ''
        this.vissibleProgess = ''
    }
}
