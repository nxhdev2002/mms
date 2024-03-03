import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ReceiveTopsseInvoiceComponent } from './receive-topsseinvoice-modal.component';

@Component({
    templateUrl: './topsseinvoice.component.html',
})
export class TopsseInvoiceComponent extends AppComponentBase implements OnInit {
    @ViewChild('receiveTopsseInvoice', { static: true }) receiveTopsseInvoice: | ReceiveTopsseInvoiceComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    colDefs: any;
    detailsColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    paginationParamsDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvTopsseInvoiceDto = new InvTopsseInvoiceDto();
    datas: InvTopsseInvoiceDto = new InvTopsseInvoiceDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    rowData: any[] = [];
    rowDataDetails: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    invoiceNo: string = '';
    orderNo: string = '';
    partNo: string = '';
    status: string = '';
    invoiceDate: any;
    p_id: any;
    _invoiceNoSelect: any;
    isReceive: boolean = false;

    _pagesizedetails = 1000000000;

    statusList = [
        {value: '' , label: "All"},
        {value: 'New' , label: "New"},
        {value: 'INL' , label: "INL"},
        {value: 'Received' , label: "Received"}
    ];

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

    constructor(
        injector: Injector,
        private _service: InvTopsseInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Dist Fd'), headerTooltip: this.l('Dist Fd'), field: 'distFd', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Invoice Date'), headerTooltip: this.l('Invoice Date'), field: 'invoiceDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Bill Of Lading'), headerTooltip: this.l('Bill Of Lading'), field: 'billOfLading', flex: 1 },
            {
                headerName: this.l('Process Date'), headerTooltip: this.l('Process Date'), field: 'processDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.processDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Etd'), headerTooltip: this.l('Etd'), field: 'etd', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.etd, 'dd/MM/yyyy')
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1 }
        ];
        this.detailsColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDetails.pageSize * (this.paginationParamsDetails.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Case No'), headerTooltip: this.l('Case No'), field: 'caseNo', flex: 1 },
            { headerName: this.l('Order No'), headerTooltip: this.l('Order No'), field: 'orderNo', flex: 1 },
            { headerName: this.l('Item No'), headerTooltip: this.l('Item No'), field: 'itemNo', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Part Qty'), headerTooltip: this.l('Part Qty'), field: 'partQty', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('Unit Fob'), headerTooltip: this.l('Unit Fob'), field: 'unitFob', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.unitFob, 0)
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 0)
            },
            { headerName: this.l('Tariff Cd'), headerTooltip: this.l('Tariff Cd'), field: 'tariffCd', flex: 1 },
            { headerName: this.l('Hs Cd'), headerTooltip: this.l('Hs Cd'), field: 'hsCd', flex: 1 },
            { headerName: this.l('Receive Qty'), headerTooltip: this.l('Receive Qty'), field: 'receiveQty', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('Receive Date'), headerTooltip: this.l('Receive Date'), field: 'receiveDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy')
            }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsDetails = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
    }

    toggleStatus() {
        this.status = this.status === undefined ? '1' : undefined;
        if (this.paginationParams.pageNum > 1) {
            this.paginationParams.pageNum = 1;
            this.paginationParams.skipCount = 0;
        }
        this.searchDatas();
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getTopsseInvoiceSearch(
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDate),
            this.orderNo,
            this.partNo,
            this.status,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataDetails = [];
                } else {
                    this.searchDatasDetails(result.items[0].id);
                }
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.invoiceNo = '';
        this.invoiceDate = '';
        this.orderNo = '';
        this.partNo = '';
        this.status = '';
        this.paginationParamsDetails = { pageNum: 1, pageSize: this._pagesizedetails, totalCount: 0 };
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getTopsseInvoiceSearch(
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDate),
            this.orderNo,
            this.partNo,
            this.status,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvTopsseInvoiceDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id = selected.id;
            this.paginationParamsDetails.pageNum = 1;
            this.paginationParamsDetails.skipCount = 0;
            this.searchDatasDetails(selected.id);

            this._invoiceNoSelect = selected.invoiceNo;
            if(selected.status == 'INL' || selected.status == 'Received') this.isReceive = false;
            else {
                this.isReceive = true;
            }
        }
        this.selectedRow = Object.assign({}, selected);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getTopsseInvoiceToExcel(
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDate),
            this.orderNo,
            this.partNo,
            this.status
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }


    searchDatasDetails(p_id): void {
        this._service.getTopsseInvoiceDetailsById(
            p_id,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        )
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDetails = result.items;
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
            });
    }

    getDatasDetails(paginationParamsDetails?: PaginationParamsModel) {
        return this._service.getTopsseInvoiceDetailsById(
            this.p_id,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        );
    }

    changePageDetails(paginationParamsDetails) {
        this.paginationParamsDetails = paginationParamsDetails;
        this.paginationParamsDetails.skipCount = (paginationParamsDetails.pageNum - 1) * paginationParamsDetails.pageSize;
        this.getDatasDetails(this.paginationParamsDetails).subscribe((result) => {
            this.paginationParamsDetails.totalCount = result.totalCount;
            this.rowDataDetails = result.items;
            this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
        });

        this._pagesizedetails = this.paginationParamsDetails.pageSize;
    }

    exportToExcelDetails(e): void {
        this.fn.exportLoading(e, true);
        this._service.getTopsseInvoiceDetailsToExcel(this.p_id)
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    receiveTopsse() {
        this.receiveTopsseInvoice.show(this.p_id, this._invoiceNoSelect);
    }
}
