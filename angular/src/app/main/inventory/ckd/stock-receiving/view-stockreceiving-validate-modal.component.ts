import { Component, ElementRef, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockReceivingServiceProxy, InvCkdStockReceivingValidateDto } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ceil } from 'lodash-es';
import { finalize } from 'rxjs';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './view-stockreceiving-validate-modal.component.html',
    selector: 'view-stockreceiving-validate-modal'
})
export class ViewStockReceivingValidateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewStockReceivingValidateModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    validateColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    _isActive: boolean = false;
    frameworkComponents: FrameworkComponent;
    saving: boolean = false;
    isActive: boolean;
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    partNo: string = '';
    supplierNo;
    cfc;
    containerNo;
    renban;
    receiveDate;
    status;
    cdDate;
    errDesc;
    invoiceNo;
    colorSfx;
    workingDateFrom: any;
    workingDateTo: any;
    fn: CommonFunction = new CommonFunction();

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(injector: Injector,
        private _service: InvCkdStockReceivingServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService,
        private gridTableService: GridTableService
    ) {
        super(injector);

        this.validateColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55 },
            { headerName: this.l('ErrDesc'), headerTooltip: this.l('ErrDesc'), field: 'errDesc', width: 95 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 86 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width: 92 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 130 },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', width: 95 },
            { headerName: this.l(' ReceiveDate'), headerTooltip: this.l('ReceiveDate'), field: 'receiveDate', valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'), width: 115 },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', width: 85 },
            { headerName: this.l(' CdDate'), headerTooltip: this.l('CdDate'), field: 'cdDate', valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'), width: 80 },

        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }
    show(): void {
        this.getValidateStockReceiving();
        this.modal.show();
    }
    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getCheckValidateStockReceiving(
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }
    getValidateStockReceiving() {
        this.isLoading = true;
        this._service.getCheckValidateStockReceiving(
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        });
    }
    changeActive(event) {
        this._isActive = event.target.checked;
    }
    callBackGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    exportValidateToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getValidateStockReceivingToExcel()
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
