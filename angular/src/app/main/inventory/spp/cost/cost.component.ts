import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostDto, InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummaryServiceProxy, InvSppCostServiceProxy, InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTime } from 'luxon';

@Component({
    templateUrl: './cost.component.html',
})
export class CostComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    colDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvSppCostDto = new InvSppCostDto();
    datas: InvSppCostDto = new InvSppCostDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();


    partNo: string = '';
    invoiceNo: string = '';
    stock: string = '';
    fromMonthYear: any;
    toMonthYear: any;

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
        private _serviceSppCost: InvSppCostServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            { headerName: this.l('Receive Qty'), headerTooltip: this.l('Receive Qty'), field: 'reciveQty', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Price'), headerTooltip: this.l('Price'), field: 'price', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PriceVn'), headerTooltip: this.l('Price Vn'), field: 'priceVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', type: 'rightAligned', flex: 1 },
            { headerName: this.l('AmountVn'), headerTooltip: this.l('Amount Vn'), field: 'amountVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Stock'), headerTooltip: this.l('Stock'), field: 'stock', flex: 1 },
            { headerName: this.l('Month'), headerTooltip: this.l('Month'), field: 'month', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Year'), headerTooltip: this.l('Year'), field: 'year', type: 'rightAligned', flex: 1 },
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        const currentDate = new Date();
        currentDate.setMonth(currentDate.getMonth() - 1);
        currentDate.setDate(1);
        this.fromMonthYear = currentDate;
    }

    searchDatas(): void {
        this.isLoading = true;
        this._serviceSppCost.getAll(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {

                this.paginationParams.totalCount = result.totalCount;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.rowData = result.items;
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        const currentDate = new Date();
        currentDate.setMonth(currentDate.getMonth() - 1);
        currentDate.setDate(1);

        this.partNo = '';
        this.invoiceNo = '';
        this.stock = '';
        this.fromMonthYear = currentDate;
        this.toMonthYear = null;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._serviceSppCost.getAll(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {

            this.rowData = result.items;
            this.paginationParams.totalCount = result.totalCount;
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
        this._serviceSppCost.getCostToExcel(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
