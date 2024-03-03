import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { InvPIOStockReceivingDto, InvPIOStockReceivingServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './stockreceving.component.html',
})
export class StockReceivingComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvPIOStockReceivingDto = new InvPIOStockReceivingDto();
    saveSelectedRow: InvPIOStockReceivingDto = new InvPIOStockReceivingDto();
    datas: InvPIOStockReceivingDto = new InvPIOStockReceivingDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();
    mktCode: string = '';
    partNo: string = '';
    partName: string = '';
    vinNo: string = '';
    route: string = '';
    workingDateFrom: any;
    workingDateTo: any;
    date = new Date();

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
        private _service: InvPIOStockReceivingServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo',width:150 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName',width:150 },
            { headerName: this.l('Mkt Code'), headerTooltip: this.l('Mkt Code'), field: 'mktCode' ,width:150},
            {
                headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),width:150
            },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', type: 'rightAligned',aggFunc: this.SumA,width:150 },
            {
                headerName: this.l('Trans Datetime'), headerTooltip: this.l('Trans Datetime'), field: 'transDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.transDatetime, 'dd/MM/yyyy HH:mm:ss'),width:150
            },
            { headerName: this.l('Vin No'), headerTooltip: this.l('Vin No'), field: 'vinNo',width:150 },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType',width:150 },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop',width:150 },
            { headerName: this.l('Car Type'), headerTooltip: this.l('Car Type'), field: 'carType',width:150 },
            { headerName: this.l('Interior Color'), headerTooltip: this.l('Interior Color'), field: 'interiorColor',width:150 },
            { headerName: this.l('Ext Color'), headerTooltip: this.l('Ext Color'), field: 'extColor',width:150 },
            { headerName: this.l('Route'), headerTooltip: this.l('Route'), field: 'route',width:150 },
            { headerName: this.l('InvoiceNo'), headerTooltip: this.l('InvoiceNo'), field: 'invoiceNo',width:150 },
            { headerName: this.l('Source'), headerTooltip: this.l('Source'), field: 'source',width:150 },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.date.setDate(this.date.getDate() - 1);
        this.workingDateFrom = this.date;
    }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number,
        grandQty: number,

    ): any[] {
        let result: any[] = [];
        console.log(grandQty);
        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                qty: grandQty,
            });
        }
        return result;
    }

    // autoSizeAll() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //         if (column.getId().toString() != "stt") {
    //             allColumnIds.push(column.getId());
    //         }
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }
    // resetGridView() {
    //     setTimeout(() => {
    //         this.dataParams.columnApi!.sizeColumnsToFit({
    //             suppressColumnVirtualisation: true,
    //         });
    //         this.autoSizeAll();
    //     })
    // }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.mktCode,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));

                var grandQty = 0;
                if (result.totalCount > 0) {
                    grandQty = result.items[0].grandQty;
                    var rows = this.createRow(1, grandQty);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                }
                else this.dataParams!.api.setPinnedBottomRowData(null);
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.mktCode = '';
        this.vinNo = '';
        this.date.setDate(this.date.getDate() - 2);
        this.workingDateFrom = this.date;
        this.workingDateTo = '';
        this.searchDatas();
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.mktCode,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
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

            var grandQty = 0;
            if (result.totalCount > 0) {
                grandQty = result.items[0].grandQty;
                var rows = this.createRow(1, grandQty);
                this.dataParams!.api.setPinnedBottomRowData(rows);
            }
            else this.dataParams!.api.setPinnedBottomRowData(null);
            this.isLoading = false;
            //this.resetGridView()
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

                var grandQty = 0;
                if (result.totalCount > 0) {
                    grandQty = result.items[0].grandQty;
                    var rows = this.createRow(1, grandQty);
                    this.dataParams!.api.setPinnedBottomRowData(rows);
                }
                else this.dataParams!.api.setPinnedBottomRowData(null);
                this.isLoading = false;
                //this.resetGridView()
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockReceivingToExcel(
            this.partNo,
            this.mktCode,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
        )
            .subscribe((result) => {
                console.log(result);

                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
