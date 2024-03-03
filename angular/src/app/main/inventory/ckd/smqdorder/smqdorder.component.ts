import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdOrderDto, InvCkdSmqdOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './smqdorder.component.html',
})
export class SmqdOrderComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdSmqdOrderDto = new InvCkdSmqdOrderDto();
    saveSelectedRow: InvCkdSmqdOrderDto = new InvCkdSmqdOrderDto();
    datas: InvCkdSmqdOrderDto = new InvCkdSmqdOrderDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    shop: string = '';
    smqdDate
    runNo: string = '';
    cfc: string = '';
    supplierNo: string = '';
    partNo: string = '';
    partName: string = '';
    orderNo: string = '';
    orderQty
    orderDate
    transport: string = '';
    reasonCode: string = '';
    etaRequest
    actualEtaPort: string = '';
    etaExpReply
    invoiceNo: string = '';
    receiveDate
    receiveQty
    remark: string = '';
    orderType;
    smqdDateFrom: any;
    smqdDateTo: any;
    orderDateFrom: any;
    orderDateTo: any;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
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
        private _service: InvCkdSmqdOrderServiceProxy,
        private gridTableService: GridTableService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },

            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', cellClass: ['text-center']},
            { headerName: this.l('SMQD Date'), headerTooltip: this.l('Smqd Date'), field: 'smqdDate',valueGetter: (params) => this.pipe.transform(params.data?.smqdDate, 'dd/MM/yyyy')},
            { headerName: this.l('Run No'), headerTooltip: this.l('Run No'), field: 'runNo'},
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc'},
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo'},
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo'},
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName'},
            { headerName: this.l('Order No'), headerTooltip: this.l('Order No'), field: 'orderNo'},
            { headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'orderQty',type: 'rightAligned' },
            { headerName: this.l('Order Date'), headerTooltip: this.l('Order Date'), field: 'orderDate',valueGetter: (params) => this.pipe.transform(params.data?.orderDate, 'dd/MM/yyyy')},
            { headerName: this.l('Transport'), headerTooltip: this.l('Transport'), field: 'transport'},
            { headerName: this.l('Reason Code'), headerTooltip: this.l('Reason Code'), field: 'reasonCode'},
            { headerName: this.l('Eta Request'), headerTooltip: this.l('Eta Request'), field: 'etaRequest',valueGetter: (params) => this.pipe.transform(params.data?.etaRequest, 'dd/MM/yyyy')},
            { headerName: this.l('Actual Eta Port'), headerTooltip: this.l('Actual Eta Port'), field: 'actualEtaPort'},
            { headerName: this.l('Eta Exp Reply'), headerTooltip: this.l('Eta Exp Reply'), field: 'etaExpReply',valueGetter: (params) => this.pipe.transform(params.data?.etaExpReply, 'dd/MM/yyyy')},
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo'},
            { headerName: this.l('Receive Date'), headerTooltip: this.l('Receive Date'), field: 'receiveDate',valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy')},
            { headerName: this.l('Receive Qty'), headerTooltip: this.l('Receive Qty'), field: 'receiveQty',type: 'rightAligned' },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark'},
            { headerName: this.l('Order Type'), headerTooltip: this.l('Order Type'), field: 'orderType',valueGetter: (params) => params.data?.orderType == 0 ? 'Normal Order' : 'Special Order' },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked"
                && column.getId().toString() != "stt"
                && column.getId().toString() != "listLotNo"
                && column.getId().toString() != "listCaseNo") {
                ``
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
    },500)
    }

    searchDatas(): void {
        this._service.getAll(
            this.shop,
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.orderNo,
            this._dateTimeService.convertToDatetime(this.orderDateFrom),
            this._dateTimeService.convertToDatetime(this.orderDateTo),
            this.invoiceNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.shop = '',
            this.smqdDateFrom = null,
            this.smqdDateTo = null,
            this.partNo = '',
            this.cfc = '',
            this.supplierNo = '',
            this.orderNo = '',
            this.orderDateFrom = null,
            this.orderDateTo = null,
            this.invoiceNo = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.shop,
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.orderNo,
            this._dateTimeService.convertToDatetime(this.orderDateFrom),
            this._dateTimeService.convertToDatetime(this.orderDateTo),
            this.invoiceNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdSmqdOrderDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdSmqdOrderDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        console.log("haha",this.saveSelectedRow)
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getSmqdOrderListToExcel(
            this.shop,
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.orderNo,
            this._dateTimeService.convertToDatetime(this.orderDateFrom),
            this._dateTimeService.convertToDatetime(this.orderDateTo),
            this.invoiceNo
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
