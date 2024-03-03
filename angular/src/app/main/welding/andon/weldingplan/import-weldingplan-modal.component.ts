import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from "@angular/core";
import { AppConsts } from "@shared/AppConsts";
import { AppComponentBase } from "@shared/common/app-component-base";
import { WldAdoWeldingPlanServiceProxy } from "@shared/service-proxies/service-proxies";
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { WeldingPlanComponent } from "./weldingplan.component";
import { ListErrorImportWeldingPlanComponent } from "./list-error-import-weldingplan-modal.component";

@Component({
    selector: 'import-weldingplan-modal',
    templateUrl: './import-weldingplan-modal.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportWeldingPlanComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild("importModal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('errorListModal') errorListModal:| ListErrorImportWeldingPlanComponent| undefined;


    result;
    fileName: string = '';
    inrParams;
    vissbleInputName;
    vissibleProgess;
    isLoading: boolean = false;
    selectedInfImport;
    uploadData = [];

    formData: FormData = new FormData();
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrl: string;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: WldAdoWeldingPlanServiceProxy,
        private _weldingPlan: WeldingPlanComponent
    ) {
        super(injector);
        this.uploadUrl = AppConsts.remoteServiceBaseUrl + '/Prod/ImportWldAdoWeldingPlanFromExcel';
        { }
    }

    ngOnInit() { }

    show(): void {
        this.progressBarHidden();
        this.fileName = '';
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


            this.vissbleInputName = 'vissible';
            let viewName = document.getElementById("viewNameFileImport");
            if (viewName != null) {
                viewName.innerHTML = this.fileName.toString();
            }
        }
    }

    upload() {
        if (this.fileName != '') {
            this.isLoading = true;
            this._httpClient
                .post<any>(this.uploadUrl, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    console.log(response);
                    if (response.success) {
                        if (response.result.weldingPlan.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this._service.updateUserId(response.result.weldingPlan.result[0].guid)
                            .subscribe(result => {
                                this.mergeWeldingPlan(response.result.weldingPlan.result[0].guid);
                            })
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
        }else{
            this.notify.warn('Vui lòng chọn file');
        }

    }

    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close();
    }


    mergeWeldingPlan(guid) {
        this.isLoading = true;
        this._service.mergeDataWeldingPlan(guid)
            .pipe(
                finalize(() => {
                    this.isLoading = false;
                })
            )
            .subscribe(() => {
                this.showMessImport(guid)
            });
    }


    showMessImport(guid){
        this._service.getMessageErrorWeldingPlanImport(guid)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorListModal.show(guid)
            }
            else{
                this.notify.info('Lưu thành công');
                this.modal.hide();
                this.modalClose.emit(null);
                this._weldingPlan.searchDatas();
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
