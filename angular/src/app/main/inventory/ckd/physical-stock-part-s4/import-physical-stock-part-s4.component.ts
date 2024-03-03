
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartListServiceProxy, InvCkdPhysicalStockPartS4ServiceProxy, InvCkdPhysicalStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { ListErrorImportInvCkdPhysicalStockPartS4Component } from './list-error-import-physical-stock-part-s4-modal.component';
import { PhysicalStockPartS4Component } from './physical-stock-part-s4.component';

@Component({

    selector: 'physicalstockparts4',
    templateUrl: './import-physical-stock-part-s4.component.html',
    styleUrls: ['../../../import-modal.less'],
})
export class ImportInvCkdPhysicalStockPartS4Component extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportInvCkdPhysicalStockPartS4Component;
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

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrlPart: string;
    uploadUrlLot: string;
    checkStock;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: InvCkdPhysicalStockPartS4ServiceProxy,
        private _phyStockPart: PhysicalStockPartS4Component
    ) {
        super(injector);
        this.uploadUrlPart = AppConsts.remoteServiceBaseUrl + '/Prod/ImportPhysicalStockPartS4FromExcel';
        { }
    }
    ngOnInit() {
    }

    show(cs:string ): void {
        this.checkStock = cs;
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this.disabled = true;
        this.modal.show();
        this.progressBarHidden();
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

    upload() {

        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrlPart, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.physSPs4.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeStockPartS4(response.result.physSPs4.result[0].guid);
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


    mergeStockPartS4(guid) {
        this.isLoading = true;
        this._service.mergeDataInvCkdPhysicalStockPartS4(guid)
        .pipe(
            finalize(() => {
                this.isLoading = false;
            })
        )
        .subscribe(() => {
            this.showMessImport(guid,'P');
            this.progressBarHidden();
        });
    }

    showMessImport(guid,screen){
        this._service.getMessageErrorImport(guid)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorListModal.show(guid)
            }
            else{
                this.notify.info('Lưu thành công');
                this.modal.hide();
                this.modalClose.emit(null);
                this._phyStockPart.searchDatas();
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
