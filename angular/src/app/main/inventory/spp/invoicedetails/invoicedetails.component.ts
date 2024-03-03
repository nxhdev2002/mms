import { GridApi, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostDto, InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummaryServiceProxy, InvSppCostServiceProxy, InvSppInvoiceDetailsDto, InvSppInvoiceDetailsServiceProxy, InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
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
    templateUrl: './invoicedetails.component.html',
})
export class InvoiceDetailsComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvSppInvoiceDetailsDto = new InvSppInvoiceDetailsDto();
    datas: InvSppInvoiceDetailsDto = new InvSppInvoiceDetailsDto();
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
    stock: string = '';
    supplier: string = '';
    fromMonthYear: any;
    toMonthYear: any;

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
        private _serviceSppInvoiceDetails: InvSppInvoiceDetailsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('InvoiceId'), headerTooltip: this.l('InvoiceId '), field: 'invoiceId', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('InvoiceNo'), headerTooltip: this.l('InvoiceNo '), field: 'invoiceNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('PartNo'), headerTooltip: this.l('PartNo '), field: 'partNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Supplier'), headerTooltip: this.l('Supplier '), field: 'supplier', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Fob'), headerTooltip: this.l('Fob '), field: 'fob', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Freight'), headerTooltip: this.l('Freight '), field: 'freight', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Insurance'), headerTooltip: this.l('Insurance '), field: 'insurance', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Lc'), headerTooltip: this.l('Lc '), field: 'lc', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Cif'), headerTooltip: this.l('Cif '), field: 'cif', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Vat'), headerTooltip: this.l('Vat '), field: 'vat', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('InLand'), headerTooltip: this.l('InLand '), field: 'inLand', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Tax'), headerTooltip: this.l('Tax '), field: 'tax', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('InvoiceQty'), headerTooltip: this.l('InvoiceQty '), field: 'invoiceQty', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('ReceicedQty'), headerTooltip: this.l('ReceicedQty '), field: 'receicedQty', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('RejectQty'), headerTooltip: this.l('RejectQty '), field: 'rejectQty', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('PartId'), headerTooltip: this.l('PartId '), field: 'partId', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Type'), headerTooltip: this.l('Type '), field: 'type', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('CaseNo'), headerTooltip: this.l('CaseNo '), field: 'caseNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Month'), headerTooltip: this.l('Month '), field: 'month',  type: 'rightAligned',flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Year'), headerTooltip: this.l('Year '), field: 'year', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('Stock'), headerTooltip: this.l('Stock '), field: 'stock', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('FobVn'), headerTooltip: this.l('FobVn '), field: 'fobVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('PONo'), headerTooltip: this.l('PONo'), field: 'poNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('LcVn'), headerTooltip: this.l('LcVn '), field: 'lcVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('CifVn'), headerTooltip: this.l('CifVn '), field: 'cifVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('VatVn'), headerTooltip: this.l('VatVn '), field: 'vatVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('InlandVn'), headerTooltip: this.l('InlandVn '), field: 'inlandVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('TaxVn'), headerTooltip: this.l('TaxVn '), field: 'taxVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('ItemNo'), headerTooltip: this.l('ItemNo '), field: 'itemNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('FreightVn'), headerTooltip: this.l('FreightVn '), field: 'freightVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true },
            { headerName: this.l('InsuranceVn'), headerTooltip: this.l('InsuranceVn '), field: 'insuranceVn', type: 'rightAligned', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, },
            { headerName: this.l('ReceiveDate'), headerTooltip: this.l('ReceiveDate '), field: 'receiveDate', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy') },
            { headerName: this.l('ProcessDate'), headerTooltip: this.l('ProcessDate '), field: 'processDate', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.processDate, 'dd/MM/yyyy') },
            { headerName: this.l('CustomerNo'), headerTooltip: this.l('CustomerNo '), field: 'customerNo', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, },
            { headerName: this.l('IsInternal'), headerTooltip: this.l('IsInternal '), field: 'isInternal', flex: 1, enableRowGroup: true, enablePivot: true, enableValue: true, },

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
        this._serviceSppInvoiceDetails.getAll(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this.supplier,
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
        this.supplier = '';
        this.fromMonthYear = currentDate;
        this.toMonthYear = null;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._serviceSppInvoiceDetails.getAll(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this.supplier,
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
        this._serviceSppInvoiceDetails.getInvoiceDetailsToExcel(
            this.partNo,
            this.invoiceNo,
            this.stock,
            this.supplier,
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
