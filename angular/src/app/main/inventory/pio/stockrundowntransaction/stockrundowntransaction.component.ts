import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvPIOStockRundownTransactionDto, InvPIOStockRundownTransactionServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './stockrundowntransaction.component.html',
})
export class StockRundownTransactionComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvPIOStockRundownTransactionDto = new InvPIOStockRundownTransactionDto();
    saveSelectedRow: InvPIOStockRundownTransactionDto = new InvPIOStockRundownTransactionDto();
    datas: InvPIOStockRundownTransactionDto = new InvPIOStockRundownTransactionDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    partNo: string = '';
    partName: string = '';
    mktCode: string = '';
    workingDateFrom:any;
    workingDateTo:any;
    route: string = '';
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
        private _service: InvPIOStockRundownTransactionServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo',  },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName',  },
          //  { headerName: this.l('Part Id'), headerTooltip: this.l('Part Id'), field: 'partId',  },
            { headerName: this.l('Mkt Code'), headerTooltip: this.l('Mkt Code'), field: 'mktCode',  },
            { headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),},
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty',  type: 'rightAligned' },
           // { headerName: this.l('Trans Id'), headerTooltip: this.l('Trans Id'), field: 'transId',  },
            { headerName: this.l('Trans Datetime'), headerTooltip: this.l('Trans Datetime'), field: 'transDatetime', valueGetter: (params) => this.pipe.transform(params.data?.transDatetime, 'dd/MM/yyyy HH:mm:ss'),},
           // { headerName: this.l('Vehicle Id'), headerTooltip: this.l('Vehicle Id'), field: 'vehicleId',  },
            { headerName: this.l('Vin No'), headerTooltip: this.l('Vin No'), field: 'vinNo',  },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType',  },

            { headerName: this.l('Car Type'), headerTooltip: this.l('Car Type'), field: 'carType',  },
            { headerName: this.l('Interior Color'), headerTooltip: this.l('Interior Color'), field: 'interiorColor' },
            { headerName: this.l('Ext Color'), headerTooltip: this.l('ExtColor'), field: 'extColor',  },
            { headerName: this.l('Route'), headerTooltip: this.l('Route'), field: 'route',  },
        ];
		this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
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
        })
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.mktCode,
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
            // this.resetGridView();
            this.isLoading = false;
        });
    }


    clearTextSearch() {
        this.partNo = '';
       // this.partName = '';
        this.mktCode = '';
        this.workingDateFrom = '',
        this.workingDateTo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.mktCode,
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
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            // this.resetGridView();
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
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockRundownTransactionToExcel(
            this.partNo,
            this.mktCode,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo)
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
