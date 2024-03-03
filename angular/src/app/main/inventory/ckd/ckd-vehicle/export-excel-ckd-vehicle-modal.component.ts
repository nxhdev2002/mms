import { DatePipe } from '@angular/common';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { CustomColDef } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPhysicalStockPeriodServiceProxy, InvCkdVehhicleServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileUpload } from 'primeng/fileupload';

@Component({
    selector: 'exportExcelModal',
    templateUrl: './export-excel-ckd-vehicle-modal.component.html',
    styleUrls: ['./export-excel-ckd-vehicle-modal.component.less'],
})
export class ExportExcelCkdVehicleComponent extends AppComponentBase {
    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild('ExcelFileUpload', { static: false }) excelFileUpload: FileUpload;
    @ViewChild('exportExcelModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    pipe = new DatePipe('en-US');
    periodIdList: any[] = [];
    period: any;
    id: any;
    loadColumdef: CustomColDef[] = [];
    pending: string = '';
    disable: boolean = false;

    constructor(
        injector: Injector,
        private _fileDownloadService: FileDownloadService,
        private _serviceStock: InvCkdPhysicalStockPeriodServiceProxy,
        private _service: InvCkdVehhicleServiceProxy
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width: 100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'fromDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'toDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),
                flex: 1
            },

        ];
    }

    show(): void {
        this.id = null;
        this._serviceStock.getIdInvPhysicalStockPeriod()
            .subscribe((result) => {
                this.periodIdList = result.items;
                this.period = (result.items.filter(s => s.status = 1))[0].infoPeriod;
            });
        this.modal.show();
    }

    exportToExcel() {
        if (this.id == null) {
            this.notify.warn('Hãy chọn kì trước khi Export Excel!');
            return;
        }
        this.pending = 'pending';
        this.disable = true;
        this._service.getCkdVehicleByPeriodToExcel(this.id)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.pending = '';
                this.disable = false;
            });
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
    }
}
