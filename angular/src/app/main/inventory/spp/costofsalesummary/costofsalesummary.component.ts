import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummaryServiceProxy, InvTopsseInvoiceDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    templateUrl: './costofsalesummary.component.html',
})
export class CostOfSaleSummaryComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvSppCostOfSaleSummaryDto = new InvSppCostOfSaleSummaryDto();
    datas: InvSppCostOfSaleSummaryDto = new InvSppCostOfSaleSummaryDto();
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

    customerNo: string = '';


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
        private _serviceSppCostOfSale: InvSppCostOfSaleSummaryServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
        this.colDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Customer No'), headerTooltip: this.l('Customer No'), field: 'customerNo', flex: 1 },
            {
                headerName: this.l('Parts'),
                headerTooltip: this.l('Parts'),
                flex: 1,
                children: [
                    {
                        headerName: this.l('Quantity'),
                        field: 'partsQty',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Cost'),
                        field: 'partsCost',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Sale Amount'),
                        field: 'partsSaleAmount',
                        flex: 1,
                        type: 'rightAligned'
                    },
                ]
            },
            {
                headerName: this.l('Export'),
                headerTooltip: this.l('Export'),
                flex: 1,
                children: [
                    {
                        headerName: this.l('Quantity'),
                        field: 'exportQty',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Cost'),
                        field: 'exportCost',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Sale Amount'),
                        field: 'exportSaleAmount',
                        flex: 1,
                        type: 'rightAligned'
                    },
                ]
            },
            {
                headerName: this.l('Onhand Adjustment'),
                headerTooltip: this.l('Onhand Adjustment'),
                flex: 1,
                children: [
                    {
                        headerName: this.l('Quantity'),
                        field: 'onhandAdjustmentQty',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Cost'),
                        field: 'onhandAdjustmentCost',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Sale Amount'),
                        field: 'onhandAdjustmentSaleAmount',
                        flex: 1,
                        type: 'rightAligned'
                    },
                ]
            },
            {
                headerName: this.l('Internal'),
                headerTooltip: this.l('Internal'),
                flex: 1,
                children: [
                    {
                        headerName: this.l('Quantity'),
                        field: 'internalQty',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Cost'),
                        field: 'internalCost',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Sale Amount'),
                        field: 'internalSaleAmount',
                        flex: 1,
                        type: 'rightAligned'
                    },
                ]
            },
            {
                headerName: this.l('Others'),
                headerTooltip: this.l('Others'),
                flex: 1,
                children: [
                    {
                        headerName: this.l('Quantity'),
                        field: 'othersQty',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Cost'),
                        field: 'othersCost',
                        flex: 1,
                        type: 'rightAligned'
                    },
                    {
                        headerName: this.l('Sale Amount'),
                        field: 'othersSaleAmount',
                        flex: 1,
                        type: 'rightAligned'
                    },
                ]
            },
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        this._serviceSppCostOfSale.getAll(
            this.customerNo,
            null,
            null,
            '',
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
        this.customerNo = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._serviceSppCostOfSale.getAll(
            this.customerNo,
            null,
            null,
            '',
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
        this._serviceSppCostOfSale.getCostOfSummaryToExcel(
            this.customerNo,
            null,
            null,
            '',
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
