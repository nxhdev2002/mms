
import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartListServiceProxy, InvCkdPhysicalStockPartServiceProxy, InvCkdPhysicalStockPeriodServiceProxy } from '@shared/service-proxies/service-proxies';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';
import { finalize } from 'rxjs/operators';
import { PhysicalStockPartComponent } from './physical-stock-part.component';
import { ListErrorImportInvCkdPhysicalStockPartComponent } from './list-error-import-physical-stock-part-modal.component';
import { CustomColDef } from '@app/shared/common/models/base.model';
import { DatePipe } from '@angular/common';


@Component({

    selector: 'physicalstockpart',
    templateUrl: './import-physical-stock-part.component.html',
    styleUrls: ['./import-physical-stock-part.component.less'],
})
export class ImportInvCkdPhysicalStockPartComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('importModal', { static: true }) modal: ModalDirective;
    @ViewChild('errorListModal', { static: true }) errorListModal: ListErrorImportInvCkdPhysicalStockPartComponent;
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
    loadColumdef: CustomColDef[] = [];
    pipe = new DatePipe('en-US');

    formData: FormData = new FormData();
    processInfo: any[] = [];
    urlBase: string = AppConsts.remoteServiceBaseUrl;
    uploadUrlPart: string;
    uploadUrlLot: string;
    checkStock;
    periodIdList: any[] = [];
    Period: any;
    id: any;

    constructor(
        injector: Injector,
        private _httpClient: HttpClient,
        private _service: InvCkdPhysicalStockPartServiceProxy,
        private _phyStockPart: PhysicalStockPartComponent,
        private _serviceStock: InvCkdPhysicalStockPeriodServiceProxy,
    ) {
        super(injector);
        this.uploadUrlPart = AppConsts.remoteServiceBaseUrl + '/Prod/ImportCkdPhysicalStockPartExcel';
        this.uploadUrlLot = AppConsts.remoteServiceBaseUrl + '/Prod/ImportCkdPhysicalStockLotExcel';

        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width:100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'fromDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),
                flex:1
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'toDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),
                flex:1
            },

        ];
        { }
    }
    ngOnInit() {
    }

    show(cs:string ): void {
        this.checkStock = cs;
        this.fileName = '';
        this.processInfo = [];
        this.uploadData = [];
        this.modal.show();

        this._serviceStock.getIdInvPhysicalStockPeriodImport()
        .subscribe((result) => {
            this.periodIdList = result.items;
            this.Period = (result.items.filter(s => s.status = 1))[0].infoPeriod;
        });
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



    uploadStockPart() {

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
                        if (response.result.stockpart.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeStockPart(response.result.stockpart.result[0].guid,this.id);
                        }
                    }
                },
                (error) => {
                    this.exeptionDataImport();
                });
        }else{
            this.notify.warn('Vui lòng chọn file');
        }

        // this.progressBarVisible();
        // this.disabled = true;
        // if (this.fileName != '') {
        //     this.isLoading = true;
        //     this._httpClient
        //         .post<any>(this.uploadUrlPart, this.formData)
        //         .pipe(finalize(() => {
        //             this.excelFileUpload?.clear();
        //         }))
        //         .subscribe(response => {
        //             if (response.success) {
        //                 if (response.result.stockpart.result.length == 0) {
        //                     this.exeptionDataImport();
        //                 }
        //                 else {
        //                     this.mergeStockPart(response.result.stockpart.result[0].guid);
        //                 }
        //             }
        //             else if (response.error) {
        //                 this.exeptionDataImport();
        //             }
        //         },
        //         error => {
        //             console.log('Error:', error);
        //             this.exeptionDataImport();
        //         });
        // }
    }

    uploadStockLot() {

        console.log("upload");
        if (this.fileName != '') {
            this.progressBarVisible();
            this._httpClient
                .post<any>(this.uploadUrlLot, this.formData)
                .pipe(finalize(() => {
                    this.excelFileUpload?.clear();
                }))
                .subscribe(response => {
                    if (response.success) {
                        if (response.result.stocklot.result.length == 0) {
                            this.exeptionDataImport();
                        }
                        else {
                            this.mergeStockLot(response.result.stocklot.result[0].guid,this.id);
                        }
                    }
                },
                (error) => {
                    this.exeptionDataImport();
                });
        }else{
            this.notify.warn('Vui lòng chọn file');
        }

        // this.progressBarVisible();
        // this.disabled = true;
        // if (this.fileName != '') {
        //     this.isLoading = true;
        //     this._httpClient
        //         .post<any>(this.uploadUrlLot, this.formData)
        //         .pipe(finalize(() => {
        //             this.excelFileUpload?.clear();
        //         }))
        //         .subscribe(response => {
        //             if (response.success) {
        //                 if (response.result.stocklot.result.length == 0) {
        //                     this.exeptionDataImport();
        //                 }
        //                 else {
        //                     this.mergeStockLot(response.result.stocklot.result[0].guid);
        //                 }
        //             }
        //             else if (response.error) {
        //                 this.exeptionDataImport();
        //             }
        //         },
        //         error => {
        //             console.log('Error:', error);
        //             this.exeptionDataImport();
        //         });
        // }
    }

    exeptionDataImport() {
        this.notify.warn('Dữ liệu không hợp lệ');
        this.progressBarHidden();
        this.close()
    }


    mergeStockPart(guid,periodId) {
        this.isLoading = true;
        this._service.mergeDataInvCkdPhysicalStockPart(guid,periodId)
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


    mergeStockLot(guid,periodId) {
        this.isLoading = true;
        this._service.mergeDataInvCkdPhysicalStockLot(guid,periodId)
        .pipe(
            finalize(() => {
                this.isLoading = false;
            })
        )
        .subscribe(() => {
            this.showMessImport(guid,'L');
            this.progressBarHidden();
        });
    }


    showMessImport(guid,screen){
        this._service.getMessageErrorImport(guid,screen)
        .subscribe((result) => {

            if(result.items.length > 0){
                this.errorListModal.show(guid,screen)
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
