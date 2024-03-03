import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockIssuingTransactionDto, InvGpsStockIssuingTransactionServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';
import { ViewGpsStockIssuingTransactionModalComponent } from './view-stockissuingtrans-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    selector: 'app-stockissuingtransaction',
    templateUrl: './stockissuingtransaction.component.html'
})
export class StockIssuingTransactionComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewGpsStockIssuingTrans', { static: true }) viewGpsStockIssuingTrans: | ViewGpsStockIssuingTransactionModalComponent | undefined;
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

    selectedRow: InvGpsStockIssuingTransactionDto = new InvGpsStockIssuingTransactionDto();
    saveSelectedRow: InvGpsStockIssuingTransactionDto = new InvGpsStockIssuingTransactionDto();
    datas: InvGpsStockIssuingTransactionDto = new InvGpsStockIssuingTransactionDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    vinNo: string = '';
    lotNo: string = '';
    noInLot: any;
    workingDateFrom: any;
    workingDateTo: any;
    date = new Date();
    cfc: string = '';
    supplierNo: string = '';
    _selectrow;

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
        private _service: InvGpsStockIssuingTransactionServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'), headerTooltip: this.l('STT'), cellClass: ['text-center'], width: 65,
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1)
            },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Puom'), headerTooltip: this.l('Puom'), field: 'puom', flex: 1 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.qty, 4),
                comparator: this._formStoringService.decimalComparator, aggFunc: this.SumA
            },

            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'), flex: 1
            }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.workingDateFrom = this.date;
        this.workingDateTo = this.date;
    }

    autoSize() {

        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked") {
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
            this.autoSize();
        }, 1000)
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount > 0) {
                    var grandTotal = 0;
                    grandTotal = result.items[0].grandTotal;
                    var rows = this.createRow(1, grandTotal);
                    this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                    this.gridTableService.selectFirstRow(this.dataParams!.api);
              //      this.resetGridView();
                }
                else {
                    this._selectrow = null;
                    this.dataParams!.api.setPinnedBottomRowData(null); //remove row total nếu totalcount = 0
                }
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.workingDateFrom = this.date;
        this.workingDateTo = this.date;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsStockIssuingTransactionDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsStockIssuingTransactionDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._selectrow = this.selectedRow.partNo;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.gridTableService.selectFirstRow(this.dataParams!.api);
             //   this.resetGridView();
                this.isLoading = false;
            });
    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
    }

    getViewDataDetail() {
        this.viewGpsStockIssuingTrans.show(this.selectedRow);
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getGpsStockIssuingTransToExcel(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number, grandTotal: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                qty: grandTotal,
            });
        }
        return result;
    }
}

