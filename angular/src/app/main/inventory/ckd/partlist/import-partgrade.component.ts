
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { CkdPartListComponent } from './partlist.component';
import { ListErrorImportInvCkdPartGradeComponent } from './list-error-import-part-grade-modal.component';


@Component({

    selector: 'import-partgrade',
    templateUrl: './import-partgrade.component.html',
    styleUrls: ['../../../import-modal.less'],

})
export class ImportPartGradeComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorImportPartGrade', { static: true }) errorImportPartGrade: ListErrorImportInvCkdPartGradeComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    disabled = false;
    vissbleFileName;
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
        private _service: InvCkdPartListServiceProxy,
        private _ckdprodMap: CkdPartListComponent
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportCkdPartGradeExcel';
        { }
    }
    ngOnInit() {

    }

    show(): void {
        this.progressBarHidden();
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        // this.disabled = true;
        this.modal.show();
    }

    close(): void {
        this.InputVar.nativeElement.value = '';
        this.fileName = '';
        this.formData = new FormData();
        let viewName = document.getElementById('viewNameFileImportg');
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


            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImportg");
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
                        if (response.result.grade.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeDataInvCkdPartGrade(response.result.grade[0].guid);
                        }
                    }
                },
                    (error) => {
                        this.exeptionDataImport();
                    });
        } else {
            this.notify.warn('Vui lòng chọn file');
        }

    }
    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close();
    }

    mergeDataInvCkdPartGrade(guid: string) {
        this.isLoading = true;
        this._service.mergeDataInvCkdPartGradeNormal(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid);
                this.progressBarHidden();
                this.InputVar.nativeElement.value = '';
                this.fileName = '';
                this.formData = new FormData();
                this.disabled = false;
            });
    }

    showMessImport(guid) {
        this._service.getMessageErrorImportGrade(guid)
            .subscribe((result) => {
                if (result.items.length > 0) {
                    this.errorImportPartGrade.show(guid)
                }
                else {
                    this.notify.info('Lưu thành công');
                    this.modal.hide();
                    this.modalClose.emit(null);
                    this._ckdprodMap.searchDatas();
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
