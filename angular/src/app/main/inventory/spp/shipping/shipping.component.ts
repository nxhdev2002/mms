import { GridApi, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostDto, InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummaryServiceProxy, InvSppCostServiceProxy, InvSppInvoiceDetailsDto, InvSppInvoiceDetailsServiceProxy, InvSppShippingDto, InvSppShippingServiceProxy, InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ExcelSummaryModalComponent } from './excel-summary-modal.component';

@Component({
    templateUrl: './shipping.component.html',
    styleUrls: ['./shipping.component.less'],
})
export class ShippingComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('excelSummaryModal', { static: true }) excelSummaryModal: | ExcelSummaryModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvSppShippingDto = new InvSppShippingDto();
    datas: InvSppShippingDto = new InvSppShippingDto();
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
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters

    partNo: string = '';
    invoiceNo: string = '';
    stock: string = 'A';
    customerNo: string = '';
    customerOrderNo: string = '';
    fromMonthYear: any;
    toMonthYear: any;
    repoty;
    reportList;
    FromMonthYear;
    ToMonthYear;
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
    money = 1;

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
        private _shippingService: InvSppShippingServiceProxy,
        private _serviceSppShipping: InvSppShippingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('PartNo'), field: 'partNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Part Name'), headerTooltip: this.l('PartName'), field: 'partName', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('InvoiceNo'), field: 'invoiceNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Customer Order No'), headerTooltip: this.l('CustomerOrderNo'), field: 'customerOrderNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Customer No'), headerTooltip: this.l('CustomerNo'), field: 'customerNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Customer Type'), headerTooltip: this.l('CustomerType'), field: 'customerType', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Exporter'), headerTooltip: this.l('Exporter'), field: 'exporter', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('FrCd'), headerTooltip: this.l('FrCd'), field: 'frCd', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Order Qty'), headerTooltip: this.l('OrderQty'), field: 'orderQty', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Allcocated Qty'), headerTooltip: this.l('AllcocatedQty'), field: 'allcocatedQty', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Sale Price'), headerTooltip: this.l('SalePrice'), field: 'salePrice',  type: 'rightAligned',flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Sale Price Usd'), headerTooltip: this.l('SalePriceUsd'), field: 'salePriceUsd',  type: 'rightAligned',flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Price'), headerTooltip: this.l('Price'), field: 'price', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Price Vn'), headerTooltip: this.l('PriceVn'), field: 'priceVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Sale Amount Usd'), headerTooltip: this.l('SaleAmountUsd'), field: 'saleAmountUsd', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Sale Amount'), headerTooltip: this.l('SaleAmount'), field: 'saleAmount', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Month'), headerTooltip: this.l('Month'), field: 'month', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Year'), headerTooltip: this.l('Year'), field: 'year', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Stock'), headerTooltip: this.l('Stock'), field: 'stock', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Correction Cd'), headerTooltip: this.l('CorrectionCd'), field: 'correctionCd', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Route No'), headerTooltip: this.l('RouteNo'), field: 'routeNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Item No'), headerTooltip: this.l('ItemNo'), field: 'itemNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Process Date'), headerTooltip: this.l('ProcessDate'), field: 'processDate', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.processDate, 'dd/MM/yyyy') },
            { headerName: this.l('Sales Price Cd'), headerTooltip: this.l('SalesPriceCd'), field: 'salesPriceCd', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Correction Reason'), headerTooltip: this.l('CorrectionReason'), field: 'correctionReason', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
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
        this._serviceSppShipping.getAll(
            this.partNo,
            this.customerNo,
            this.customerOrderNo,
            this.invoiceNo,
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
        this.invoiceNo = '';
        this.stock = '';
        this.customerNo = '';
        this.customerOrderNo = '';
        this.fromMonthYear = currentDate;
        this.toMonthYear = null;
        this.reportList = [],
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._serviceSppShipping.getAll(
            this.partNo,
            this.customerNo,
            this.customerOrderNo,
            this.invoiceNo,
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
        this._serviceSppShipping.getShippingToExcel(
            this.partNo,
            this.customerNo,
            this.customerOrderNo,
            this.invoiceNo,
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


    showExportPopup(e, exportType): void {
        switch(exportType) {
            case 0: // cost of sale (usd)
                this.exportCostOfSaleUsd(e)
                break
            case 1: // cost of sale (usd)
                this.exportCostOfSaleSummary(e)
                break
            case 2: // gl transaction
                this.exportGLTransaction(e)
                break

        }
       // this.excelSummaryModal.show(exportType)
    }

    exportGLTransaction(e) {
        this.isLoading = true;
        this._shippingService.getShippingGLTransactionToExcel(
            this.stock,
            this.money,
            this.reportList,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        ) .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.isLoading = false;
            }, );
        });
    }

    exportCostOfSaleUsd(e) {
        this.isLoading = true;
        this._shippingService.getShippingCostOfSaleToExcel(
            this.stock,
            this.money,
            this.reportList,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        ) .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.isLoading = false;
            }, );
        });
    }
    exportCostOfSaleSummary(e){
        this.isLoading = true;
        this._shippingService.getShippingCostOfSaleSummaryToExcel(
            this.stock,
            this.money,
            this.reportList,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        ) .subscribe((result) => {
            setTimeout(() => {

                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.isLoading = false;
            }, )
        });
    }

    exportCostOfSaleVnd(e) {
        this.fn.exportLoading(e, true);
        this._shippingService.getShippingCostOfSaleSummaryToExcel(
            this.stock,
            1,
            this.reportList,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        ) .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }


    prodSumaryOption() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcus() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
    }


}
