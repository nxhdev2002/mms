import { Params } from '@angular/router';
import { ColDef, GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockBalanceDto, InvCkdStockBalanceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe, formatDate } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewStockBalanceModalComponent } from './view-stockbalance-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { log } from 'console';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './stock-balance.component.html',
})
export class StockBalanceComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewModalStockBalance', { static: true }) viewModalStockBalance: | ViewStockBalanceModalComponent | undefined;
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

    selectedRow: InvCkdStockBalanceDto = new InvCkdStockBalanceDto();
    saveSelectedRow: InvCkdStockBalanceDto = new InvCkdStockBalanceDto();
    datas: InvCkdStockBalanceDto = new InvCkdStockBalanceDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    partNo: string = '';
    cfc: string = '';
    workingDate
    periodId
    isActive;
    workingDateFrom: any;
    workingDateTo: any;
    periodIdList: any[] = [];
    id;
    loadColumdef: CustomColDef[] = [];
    date = new Date();
    colorSfx;
    supplierNo;
    diff: boolean = false;


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
        private _service: InvCkdStockBalanceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
        private _formStoringService: FormStoringService,
        private eventBus: EventBusService
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width: 100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'from_Date',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.from_Date, 'dd/MM/yyyy'),
                width: 75
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'to_Date',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.to_Date, 'dd/MM/yyyy'),
                width: 75
            },
            {
                headerName: 'Status',
                headerTooltip: 'Status',
                field: 'status',
                cellClass: ['text-center'],
                width: 65
            },
        ];
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNoNormalizedS4',
                flex: 1
            },
            {
                headerName: this.l('Color Sfx'),
                headerTooltip: this.l('Color Sfx'),
                field: 'colorSfx',
                flex: 1
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                flex: 1
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'source',
                flex: 1
            },
            {
                headerName: this.l('Begining'),
                headerTooltip: this.l('Begining'),
                field: 'begining',
                flex: 1,
                comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.begining != null ? params.data?.begining : 0),
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Receiving'),
                headerTooltip: this.l('Receiving'),
                field: 'receiving', flex: 1,
                comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.receiving != null ? params.data?.receiving : 0),
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Issuing'),
                headerTooltip: this.l('Issuing'),
                field: 'issuing',
                flex: 1,
                comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.issuing != null ? params.data?.issuing : 0),
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Closing'),
                headerTooltip: this.l('Closing'),
                field: 'closing',
                flex: 1,
                comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.closing != null ? params.data?.closing : 0),
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Concept'),
                headerTooltip: this.l('Concept'),
                field: 'concept',
                flex: 1,
                comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.concept != null ? params.data?.concept : 0),
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Diff'),
                headerTooltip: this.l('Diff'),
                field: 'diff',
                flex: 1,
                type: 'rightAligned',
                aggFunc: this.SumA //sum, min, max, count, avg, first, last
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,


        };


    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

        // Receive PartNo and Data
      //  this.date.setDate(this.date.getDate() - 3);

        let urlParams = new URLSearchParams(window.location.search);
        this.partNo = urlParams.get('p');
        if(this.partNo == null || this.partNo == undefined ) this.partNo = "";
        if(this.partNo != "" ){

                this.workingDateFrom = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, 1);
                this.workingDateTo = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, Number(urlParams.get('d')));

                this.cfc = urlParams.get('c');
                if(this.cfc == null || this.cfc == undefined ) this.cfc = "";

                this.supplierNo = urlParams.get('s')
                if(this.supplierNo == null || this.supplierNo == undefined ) this.supplierNo = "";
                this.searchDatas();
        }

        let y = this.date.getFullYear(), m = this.date.getMonth();
        var firstDay = new Date(y, m, 1);
        this.workingDateFrom = firstDay;
        this.workingDateTo = this.date;

    }

    SumA(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    createRow(count: number, _sumBegining: number, _sumReceiving: number, _sumIssuing: number,
        _sumClosing: number, _sumConcept: number, _sumDiff: number): any[] {
        let result: any[] = [];
        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                cfc: '',
                source: '',
                begining: _sumBegining,
                receiving: _sumReceiving,
                issuing: _sumIssuing,
                closing: _sumClosing,
                concept: _sumConcept,
                diff: _sumDiff
            });
        }
        return result;
    }

    searchDatas(): void {
        this.isLoading = true;
        if (this.workingDateFrom > this.workingDateTo) {
            this.rowData = [];
            this.isLoading = false;
        }
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getDataBalance(
            this.partNo,
            this.colorSfx,
            this.cfc,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.diff,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }
            ))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;

                var _sumBegining = 0;
                var _sumReceiving = 0;
                var _sumIssuing = 0;
                var _sumClosing = 0;
                var _sumDiff = 0;
                var _sumConcept = 0;
                if (result.totalCount > 0) {
                    _sumBegining = result.items[0].grandBegining;
                    _sumReceiving = result.items[0].grandReceiving;
                    _sumIssuing = result.items[0].grandIssuing;
                    _sumClosing = result.items[0].grandClosing;
                    _sumDiff = result.items[0].grandDiff;
                    _sumConcept = result.items[0].grandConcept;
                    var rows = this.createRow(1, _sumBegining, _sumReceiving, _sumIssuing, _sumClosing, _sumConcept, _sumDiff);
                }
                this.dataParams!.api.setPinnedBottomRowData(rows);
            });
    }

    clearTextSearch() {
        //set date
        let y = this.date.getFullYear(), m = this.date.getMonth();
        var firstDay = new Date(y, m, 1);
        this.workingDateFrom = firstDay;
        this.workingDateTo = this.date;
        this.partNo = '',
            this.diff = false;
        this.cfc = '';
        this.supplierNo = '';
        this.colorSfx = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getDataBalance(
            this.partNo,
            this.colorSfx,
            this.cfc,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.diff,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdStockBalanceDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdStockBalanceDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;

                // var _sumBegining = 0;
                // var _sumReceiving = 0;
                // var _sumIssuing = 0;
                // var _sumClosing = 0;
                // var _sumDiff = 0;
                // var _sumConcept = 0;
                // if (result.totalCount > 0) {
                //     _sumBegining = result.items[0].grandBegining;
                //     _sumReceiving = result.items[0].grandReceiving;
                //     _sumIssuing = result.items[0].grandIssuing;
                //     _sumClosing = result.items[0].grandClosing;
                //     _sumDiff = result.items[0].grandDiff;
                //     _sumConcept = result.items[0].grandConcept;
                //     var rows = this.createRow(1, _sumBegining, _sumReceiving, _sumIssuing, _sumClosing, _sumConcept, _sumDiff);
                // }
                // this.dataParams!.api.setPinnedBottomRowData(rows);
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
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;

                var _sumBegining = 0;
                var _sumReceiving = 0;
                var _sumIssuing = 0;
                var _sumClosing = 0;
                var _sumDiff = 0;
                var _sumConcept = 0;
                if (result.totalCount > 0) {
                    _sumBegining = result.items[0].grandBegining;
                    _sumReceiving = result.items[0].grandReceiving;
                    _sumIssuing = result.items[0].grandIssuing;
                    _sumClosing = result.items[0].grandClosing;
                    _sumDiff = result.items[0].grandDiff;
                    _sumConcept = result.items[0].grandConcept;
                    var rows = this.createRow(1, _sumBegining, _sumReceiving, _sumIssuing, _sumClosing, _sumConcept, _sumDiff);
                }
                this.dataParams!.api.setPinnedBottomRowData(rows);
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockBalanceToExcel(
            this.partNo,
            this.colorSfx,
            this.cfc,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.diff
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
