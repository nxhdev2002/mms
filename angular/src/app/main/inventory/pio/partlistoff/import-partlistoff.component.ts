
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartListServiceProxy, InvPartListOffServiceProxy, InvPioPartListInlServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { log } from 'console';

import { PioPartListOffComponent } from './partlistoff.component';
import { ListErrorImportInvPioPartListComponent } from './list-error-import-part-list-modal.component';


@Component({

    selector: 'import-partlistoff',
    templateUrl: './import-partlistoff.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportInvPioPartListOffComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportInvPioPartListComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    disabled = false;
    vissbleFileName ;
    vissibleProgess;
    vissbleInputName;
    result;
    fileName: string = '';
    inrParams;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];
    checkOrderPattern;

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;
    uploadLotUrl: string;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: InvPartListOffServiceProxy,
        private _pioPartList: PioPartListOffComponent
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportPioPartListOffExcel';
        { }
    }
    ngOnInit() {

    }

    show(cs:string): void {
        this.checkOrderPattern = cs;
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


            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }

    }


    uploadPxp() {

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
                        if (response.result.gentani.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergePioPartList(response.result.gentani[0].guid);
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


    mergeDataInvPioPartList(guid: string) {
        this.isLoading = true;
        this._service.mergeDataInvPioPartListOff(guid)
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

    mergePioPartList(guid) {
        this.mergeDataInvPioPartList(guid);
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
                 this._pioPartList.searchDatas();
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
