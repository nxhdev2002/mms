import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCpsInventoryItemPriceServiceProxy, InvCpsInventoryItemPriceDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './inventoryitemprice.component.html',
})
export class InventoryItemPriceComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParamsHeaders: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsLines: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    headersColDefs: any;
    linesColDefs: any;

    // selectedRowHeaders: InvCpsInventoryItemPrice = new InvCpsInventoryItemPrice();
    // saveSelectedRowHeaders: InvCpsInventoryItemPrice = new InvCpsInventoryItemPrice();
    selectedRowLines: InvCpsInventoryItemPriceDto = new InvCpsInventoryItemPriceDto();
    saveSelectedRowLines: InvCpsInventoryItemPriceDto = new InvCpsInventoryItemPriceDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowDataHeaders: any[] = [];
    rowDataLines: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    inventoryGroupList: { value: number, label: string }[] = [];
    supplierList: { value: number, label: string }[] = [];
    poNumber: string = '';
    partNo: string = '';
    partName: string = '';
    color: string = '';
    partNameSupplier: string = '';
    supplierName: string = '';
    currencyCode: string = '';
    unitPrice: any;
    taxPrice: any;
    partNoCPS: any;
    productGroupName: any;
    unitMeasLookupCode: any;
    approveDate: any;

    effectiveFrom: any;
    effectiveTo: any;

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
        private _serviceHeaders: InvCpsInventoryItemPriceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm:DataFormatService,
    ) {
        super(injector);
        this.headersColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsHeaders.pageSize * (this.paginationParamsHeaders.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Color'), headerTooltip: this.l('Color'), field: 'color', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Part Name Supplier'), headerTooltip: this.l('Part Name Supplier'), field: 'partNameSupplier', flex: 1 },
            { headerName: this.l('Supplier Name'), headerTooltip: this.l('Supplier Name'), field: 'supplierName', flex: 1 },
            { headerName: this.l('Currency Code'), headerTooltip: this.l('Currency Code'), field: 'currencyCode', flex: 1 },
            { headerName: this.l('Unit Price'), headerTooltip: this.l('Unit Price'), field: 'unitPrice', flex: 1 },
            { headerName: this.l('Tax Price'), headerTooltip: this.l('Tax Price'), field: 'taxPrice', flex: 1 },
            { headerName: this.l('Effective From'), headerTooltip: this.l('Effective From'), field: 'effectiveFrom', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.effectiveFrom, 'dd/MM/yyyy'), },
            { headerName: this.l('Effective To'), headerTooltip: this.l('Effective To'), field: 'effectiveTo', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.effectiveTo, 'dd/MM/yyyy'), },
            { headerName: this.l('Part No CPS'), headerTooltip: this.l('Part No CPS'), field: 'partNoCPS', flex: 1 },
            { headerName: this.l('Product Group Name'), headerTooltip: this.l('Product Group Name'), field: 'productGroupName', flex: 1 },
            { headerName: this.l('Unit Meas Lookup Code'), headerTooltip: this.l('Unit Meas Lookup Code'), field: 'unitMeasLookupCode', flex: 1 },
            { headerName: this.l('Approve Date'), headerTooltip: this.l('Approve Date'), field: 'approveDate', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.approveDate, 'dd/MM/yyyy hh:mm:ss'), },
        ];
        
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParamsHeaders = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsLines = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
    }


    searchDatasHeaders(): void {
        this.isLoading=true;
        this._serviceHeaders.getCpsInventoryItemPriceSearch(
            this.partNo,
            this.partName,
            this._dateTimeService.convertToDatetime(this.effectiveFrom),
            this._dateTimeService.convertToDatetime(this.effectiveTo),
            '',
            this.paginationParamsHeaders.skipCount,
            this.paginationParamsHeaders.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if(result.totalCount==0) {
                    this.rowDataLines = [];
                    this.paginationParamsLines.totalCount = result.totalCount;
                    this.paginationParamsLines.totalPage = ceil(result.totalCount / (this.paginationParamsHeaders.pageSize ?? 0));
                }
                
                this.paginationParamsHeaders.totalCount = result.totalCount;
                this.rowDataHeaders = result.items;
                this.paginationParamsHeaders.totalPage = ceil(result.totalCount / (this.paginationParamsHeaders.pageSize ?? 0));
               this.resetGridView();
                this.isLoading=false;
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.partName = '';
        this.effectiveFrom = "";
        this.effectiveTo = "";

        this.searchDatasHeaders();
    }


    
    getDatasHeaders(paginationParams?: PaginationParamsModel) {
        return this._serviceHeaders.getCpsInventoryItemPriceSearch(
            this.partNo,
            this.partName,
            this._dateTimeService.convertToDatetime(this.effectiveFrom),
            this._dateTimeService.convertToDatetime(this.effectiveTo),
            '',
            this.paginationParamsHeaders.skipCount,
            this.paginationParamsHeaders.pageSize
        );
    }

    // onChangeRowSelectionHeaders(params: { api: { getSelectedRows: () => InvCpsInventoryItemPrice[] } }) {
    //     const selected = params.api.getSelectedRows()[0];
    //     if (selected) {
    //         this.invoiceId = selected.id;
    //         this.searchDatasLines(selected.id);
    //     }
    //     this.selectedRowHeaders = Object.assign({}, selected);
    // }


    callBackDataGridHeaders(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsHeaders.pageSize);
        this.paginationParamsHeaders.skipCount =
            ((this.paginationParamsHeaders.pageNum ?? 1) - 1) * (this.paginationParamsHeaders.pageSize ?? 0);
        this.paginationParamsHeaders.pageSize = this.paginationParamsHeaders.pageSize;
        this.getDatasHeaders(this.paginationParamsHeaders)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsHeaders.totalCount = result.totalCount;
                this.rowDataHeaders = result.items ?? [];
                this.paginationParamsHeaders.totalPage = ceil(result.totalCount / (this.paginationParamsHeaders.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }


    changePageHeaders(paginationParams) {
        this.isLoading = true;
        this.paginationParamsHeaders = paginationParams;
        this.paginationParamsHeaders.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasHeaders(this.paginationParamsHeaders).subscribe((result) => {
            this.paginationParamsHeaders.totalCount = result.totalCount;
            this.rowDataHeaders = result.items;
            this.paginationParamsHeaders.totalPage = ceil(result.totalCount / (this.paginationParamsHeaders.pageSize ?? 0));
            this.isLoading = false;
           this.resetGridView();
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._serviceHeaders.getCpsInventoryItemPriceToExcel(
            this.partNo,
            this.color,
            this.partName,
            this.partNameSupplier,
            this.supplierName,
            this.currencyCode,
            this.unitPrice,
            this.taxPrice,
            this._dateTimeService.convertToDatetime(this.effectiveFrom),
            this._dateTimeService.convertToDatetime(this.effectiveTo),
            this.partNoCPS,
            this.productGroupName,
            this.unitMeasLookupCode,
            this._dateTimeService.convertToDatetime(this.approveDate)


        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
