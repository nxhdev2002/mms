import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCpsInvoiceHeadersGrid, InvCpsInvoiceHeadersServiceProxy, InvCpsInvoiceLinesDtoGrid } from '@shared/service-proxies/service-proxies';
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
    templateUrl: './invoiceheaders.component.html',
})
export class InvoiceHeadersComponent extends AppComponentBase implements OnInit {
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

    selectedRowHeaders: InvCpsInvoiceHeadersGrid = new InvCpsInvoiceHeadersGrid();
    saveSelectedRowHeaders: InvCpsInvoiceHeadersGrid = new InvCpsInvoiceHeadersGrid();
    selectedRowLines: InvCpsInvoiceLinesDtoGrid = new InvCpsInvoiceLinesDtoGrid();
    saveSelectedRowLines: InvCpsInvoiceLinesDtoGrid = new InvCpsInvoiceLinesDtoGrid();
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
    invoiceId: number;
    vendorId: number = -1;
    invoiceNum: string = '';
    invoiceSymbol: string = '';
    inventoryGroupId: number = 35;
    creationTimeFrom: any;
    creationTimeTo: any;

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
        private _serviceHeaders: InvCpsInvoiceHeadersServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm:DataFormatService,
    ) {
        super(injector);
        this.headersColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsHeaders.pageSize * (this.paginationParamsHeaders.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Invoice Symbol'), headerTooltip: this.l('Invoice Symbol'), field: 'invoiceSymbol', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNum', flex: 1 },
            { headerName: this.l('Invoice Date'), headerTooltip: this.l('Invoice Date'), field: 'creationTime', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.creationTime, 'dd/MM/yyyy'), },
            { headerName: this.l('Inventory Group'), headerTooltip: this.l('Inventory Group'), field: 'productgroupname', flex: 1 },
            { headerName: this.l('Vendor Name'), headerTooltip: this.l('Vendor Name'), field: 'supplierName', flex: 1 },
            { headerName: this.l('VAT Invoice No'), headerTooltip: this.l('VAT Invoice No'), field: 'vatregistrationInvoice', flex: 1 },
            { headerName: this.l('VAT Registration Number'), headerTooltip: this.l('VAT Registration Number'), field: 'vatregistrationNum', with: 150},
            { headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', flex: 1, type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 0)},
            { headerName: this.l('Currency Code'), headerTooltip: this.l('Currency'), field: 'currencyCode', flex: 1 },
        ];
        this.linesColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsLines.pageSize * (this.paginationParamsLines.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Po Number'), headerTooltip: this.l('Po Number'), field: 'poNumber', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'itemDescription', flex: 1},
            { headerName: this.l('Quantity'), headerTooltip: this.l('Quantity'), field: 'quantity', flex: 1, type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.quantity, 0) },
            { headerName: this.l('Quantity Order'), headerTooltip: this.l('Quantity Order'), field: 'quantityOrder', flex: 1, type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.quantityOrder, 0) },
            { headerName: this.l('Unit Price'), headerTooltip: this.l('Unit Price'), field: 'unitPrice', flex: 1, type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.unitPrice, 0)},
            { headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', flex: 1, type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 0)},
            { headerName: this.l('Amount Vat'), headerTooltip: this.l('Amount Vat'), field: 'amountVat', flex: 1, type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amountVat, 0)},
            { headerName: this.l('Tax Rate'), headerTooltip: this.l('Tax Rate'), field: 'taxRate', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Note'), headerTooltip: this.l('Note'), field: 'note', flex: 1 },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParamsHeaders = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsLines = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };

        this.getCbxInventoryGroup();
        this.getCbxSupplier();
        this.searchTime();
    }

    getCbxInventoryGroup() {
        this.inventoryGroupList.push({value:-1,label:''});
        this._serviceHeaders.getCbxInventoryGroup().subscribe((result) => {
            result.forEach(e => this.inventoryGroupList.push({ value: e.id, label: e.productgroupname }));
        });
    }

    getCbxSupplier() {
        this.supplierList.push({value:-1,label:''});
        this._serviceHeaders.getCbxSupplier().subscribe((result) => {
            result.forEach(e => this.supplierList.push({ value: e.id, label: e.supplierName }));
        })
    }

    searchTime(){
        var date=new Date();
        date.setDate(date.getDate() - 30);
        this.creationTimeFrom = date;
        this.creationTimeTo = new Date;
    }



    searchDatasHeaders(): void {
        this.isLoading=true;
        this._serviceHeaders.getInvoiceHeadersSearch(
            this.poNumber,
            this.inventoryGroupId,
            this.vendorId,
            this._dateTimeService.convertToDatetime(this.creationTimeFrom),
            this._dateTimeService.convertToDatetime(this.creationTimeTo),
            this.invoiceNum,
            this.invoiceSymbol,
            this.partNo,
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
                else{
                    this.searchDatasLines(result.items[0].id);
                }
                this.paginationParamsHeaders.totalCount = result.totalCount;
                this.rowDataHeaders = result.items;
                this.paginationParamsHeaders.totalPage = ceil(result.totalCount / (this.paginationParamsHeaders.pageSize ?? 0));
              //  this.resetGridView();
                this.isLoading=false;
            });
    }

    clearTextSearch() {
        this.searchTime();
        this.poNumber = '';
        this.inventoryGroupId = 35;
        this.vendorId = -1;
        this.invoiceNum = '';
        this.invoiceSymbol = '';
        this.partNo = '';
        this.searchDatasHeaders();
    }



    getDatasHeaders(paginationParams?: PaginationParamsModel) {
        return this._serviceHeaders.getInvoiceHeadersSearch(
            this.poNumber,
            this.inventoryGroupId,
            this.vendorId,
            this._dateTimeService.convertToDatetime(this.creationTimeFrom),
            this._dateTimeService.convertToDatetime(this.creationTimeTo),
            this.invoiceNum,
            this.invoiceSymbol,
            this.partNo,
            '',
            this.paginationParamsHeaders.skipCount,
            this.paginationParamsHeaders.pageSize
        );
    }

    onChangeRowSelectionHeaders(params: { api: { getSelectedRows: () => InvCpsInvoiceHeadersGrid[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.invoiceId = selected.id;
            this.searchDatasLines(selected.id);
        }
        this.selectedRowHeaders = Object.assign({}, selected);
    }


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
           // this.resetGridView();
        });
    }

    callBackDataGridLines(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsLines.pageSize);
        this.paginationParamsLines.skipCount =
            ((this.paginationParamsLines.pageNum ?? 1) - 1) * (this.paginationParamsLines.pageSize ?? 0);
        this.paginationParamsLines.pageSize = this.paginationParamsLines.pageSize;
        this.getDatasLines(this.paginationParamsLines)
            .pipe(finalize(() => (this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsLines.totalCount = result.totalCount;
                this.rowDataLines = result.items ?? [];
                this.paginationParamsLines.totalPage = ceil(result.totalCount / (this.paginationParamsLines.pageSize ?? 0));
                this.isLoading = false;
              //  this.resetGridView();
            });
    }
    searchDatasLines(invoiceId): void {
        // this.isLoading = true;
        this._serviceHeaders.getInvoiceLinesGetByInvoiceId(
            invoiceId,
            '',
            this.paginationParamsLines.skipCount,
            this.paginationParamsLines.pageSize
        )
            .pipe(finalize(() => (this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsLines.totalCount = result.totalCount;
                this.rowDataLines = result.items;
                this.paginationParamsLines.totalPage = ceil(result.totalCount / (this.paginationParamsLines.pageSize ?? 0));
               // this.resetGridView();

            });
    }

    getDatasLines(paginationParams?: PaginationParamsModel) {
        return this._serviceHeaders.getInvoiceLinesGetByInvoiceId(
            this.invoiceId,
            '',
            this.paginationParamsLines.skipCount,
            this.paginationParamsLines.pageSize
        );
    }

    onChangeRowSelectionLines(params: { api: { getSelectedRows: () => InvCpsInvoiceLinesDtoGrid[] } }) {
        this.saveSelectedRowLines = params.api.getSelectedRows()[0] ?? new InvCpsInvoiceLinesDtoGrid();
        this.selectedRowLines = Object.assign({}, this.saveSelectedRowLines);
    }

    changePageLines(paginationParams) {
        // this.isLoading = true;
        this.paginationParamsLines = paginationParams;
        this.paginationParamsLines.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasLines(this.paginationParamsLines).subscribe((result) => {
            this.paginationParamsLines.totalCount = result.totalCount;
            this.rowDataLines = result.items;
            this.paginationParamsLines.totalPage = ceil(result.totalCount / (this.paginationParamsLines.pageSize ?? 0));
            this.isLoading = false;
          //  this.resetGridView();
        });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._serviceHeaders.getInvoiceHeadersToExcel(
            this.poNumber,
            this.inventoryGroupId,
            this.vendorId,
            this._dateTimeService.convertToDatetime(this.creationTimeFrom),
            this._dateTimeService.convertToDatetime(this.creationTimeTo),
            this.invoiceNum,
            this.invoiceSymbol,
            this.partNo
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    exportToExcelLines(e): void {

        this.fn.exportLoading(e, true);

        this._serviceHeaders.getInvoiceLinesToExcel(
            this.invoiceId
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }

}
