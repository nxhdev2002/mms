import { GridApi, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostDto, InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummaryServiceProxy, InvSppCostServiceProxy, InvSppInvoiceDetailsDto, InvSppInvoiceDetailsServiceProxy, InvSppShippingDto, InvSppShippingServiceProxy, InvSppStockDto, InvSppStockServiceProxy, InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './stock.component.html',
})
export class StockComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvSppStockDto = new InvSppStockDto();
    datas: InvSppStockDto = new InvSppStockDto();
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
    warehouse: string = '';
    fromMonthYear: any;
    toMonthYear: any;
    stock: string = 'A';
    repoty;
    stockList = [
        {value: 'A' , label: "A"},
        {value: 'N' , label: "N"},
        {value: 'C' , label: "C"},
        {value: 'S' , label: "S"},
    ];

    moneyType = [
        {value: 0 , label: "USD"},
        {value: 1 , label: "VND"},
    ]

    money = 1;

    reportList;

    reportTypeList = [
        {value: 'Sale Parts' , label: "Sale Parts"},
        {value: 'Sale C&A' , label: "Sale C&A"},
        {value: 'Sale Chemical (CHE)' , label: "Sale Chemical (CHE)"},
        {value: 'Sale Optional warranty (OPT)' , label: "Sale Optional warranty (OPT)"},
        {value: 'Sale Export' , label: "Sale Export"},
        {value: 'Onhand Adjustment' , label: "Onhand Adjustment"},
        {value: 'Internal' , label: "Internal"},
        {value: 'Others' , label: "Others"},
    ];

    defaultColDef = {
        resizable: true,
        sortable: true,
        filter: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
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
        private _serviceSppStock: InvSppStockServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('PartNo'), headerTooltip: this.l('PartNo'), field: 'partNo', flex: 1 },
            { headerName: this.l('Month'), headerTooltip: this.l('Month'), field: 'month',  type: 'rightAligned',flex: 1 },
            { headerName: this.l('Year'), headerTooltip: this.l('Year'), field: 'year', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PreQty'), headerTooltip: this.l('PreQty'), field: 'preQty', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PrePrice'), headerTooltip: this.l('PrePrice'), field: 'prePrice', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PreAmount'), headerTooltip: this.l('PreAmount'), field: 'preAmount', type: 'rightAligned', flex: 1 },
            { headerName: this.l('InQty'), headerTooltip: this.l('InQty'), field: 'inQty', type: 'rightAligned', flex: 1 },
            { headerName: this.l('InPrice'), headerTooltip: this.l('InPrice'), field: 'inPrice', type: 'rightAligned', flex: 1 },
            { headerName: this.l('InAmount'), headerTooltip: this.l('InAmount'), field: 'inAmount', type: 'rightAligned', flex: 1 },
            { headerName: this.l('OutQty'), headerTooltip: this.l('OutQty'), field: 'outQty', type: 'rightAligned', flex: 1 },
            { headerName: this.l('OutPrice'), headerTooltip: this.l('OutPrice'), field: 'outPrice',  type: 'rightAligned',flex: 1 },
            { headerName: this.l('OutAmount'), headerTooltip: this.l('OutAmount'), field: 'outAmount', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Price'), headerTooltip: this.l('Price'), field: 'price', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', type: 'rightAligned', flex: 1 },
            { headerName: this.l('Warehouse'), headerTooltip: this.l('Warehouse'), field: 'warehouse', flex: 1 },
            { headerName: this.l('PrePriceVn'), headerTooltip: this.l('PrePriceVn'), field: 'prePriceVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PreAmountVn'), headerTooltip: this.l('PreAmountVn'), field: 'preAmountVn',  type: 'rightAligned',flex: 1 },
            { headerName: this.l('InPriceVn'), headerTooltip: this.l('InPriceVn'), field: 'inPriceVn',  type: 'rightAligned',flex: 1 },
            { headerName: this.l('InAmountVn'), headerTooltip: this.l('InAmountVn'), field: 'inAmountVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('OutPriceVn'), headerTooltip: this.l('OutPriceVn'), field: 'outPriceVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('OutAmountVn'), headerTooltip: this.l('OutAmountVn'), field: 'outAmountVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('PriceVn'), headerTooltip: this.l('PriceVn'), field: 'priceVn', type: 'rightAligned', flex: 1 },
            { headerName: this.l('AmountVn'), headerTooltip: this.l('AmountVn'), field: 'amountVn', type: 'rightAligned', flex: 1 },
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

    onChangeDlrSelect(e) {
        this.reportList = '';
        this.reportList = e;
    }

    searchDatas(): void {
        this.isLoading = true;
        this._serviceSppStock.getAll(
            this.partNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
            this.reportList,

            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {

                this.paginationParams.totalCount = result.totalCount;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.rowData = result.items;
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        const currentDate = new Date();
        currentDate.setMonth(currentDate.getMonth() - 1);
        currentDate.setDate(1);

        this.partNo = '';
        this.stock = '';
        this.fromMonthYear = currentDate;
        this.toMonthYear = null;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._serviceSppStock.getAll(
            this.partNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
            this.reportList,
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
            this.resetGridView();
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
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
        }, 1000)
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._serviceSppStock.getStockToExcel(
            this.partNo,
            this.stock,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
            this.reportList,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportBalanceReport(e) {
        this.fn.exportLoading(e, true)
        this._serviceSppStock.getShippingBalanceReportToExcelNew(
            this.stock,
            this.reportList,
            this.money,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        ) .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }

}
