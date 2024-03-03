import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvInDetailsDto, InvInDetailsServiceProxy, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';

import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { RequestModalComponent } from './request-modal.component';

@Component({
    templateUrl: './indetails.component.html',
    styleUrls: ['./indetails.component.less'],
})
export class InDetailsComponent extends AppComponentBase implements OnInit {

    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('reportRequest2Modal', { static: true }) reportRequest2Modal: | RequestModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    periodColumdef: CustomColDef[] = [];

    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvInDetailsDto = new InvInDetailsDto();
    saveSelectedRow: InvInDetailsDto = new InvInDetailsDto();
    datas: InvInDetailsDto = new InvInDetailsDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    id: number = 0;
    description: string = '';
    periodId: number = 0;
    invoiceno: number = 0;
    partno: string = '';
    usageqty: number = 0;
    cbbPeriod: any[] = [];
    invoicedate: any;
    receivedate: any;
    supplierno: string = '';
    fixlot: string = '';
    carfamilycode: string = '';
    customsdeclareno: number = 0;
    declaredate: any;

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
        private _serviceStock: InvStockServiceProxy,
        private _service: InvInDetailsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.periodColumdef = [
            {
                headerName: this.l('Id'),
                headerTooltip: this.l('Id'),
                field: 'id',
                width: 50,
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
        ]
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                 width: 55,
            },
            {
                headerName: this.l('Period Id'),
                headerTooltip: this.l('Period Id'),
                field: 'periodId',
                 flex: 1,
                 type: 'rightAligned',
            },
            {
                headerName: this.l('Invoice No'),
                headerTooltip: this.l('Invoice No'),
                field: 'invoiceNo',
                flex: 1
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Usage Qty'),
                headerTooltip: this.l('Usage Qty'),
                field: 'usageQty',
                type: 'rightAligned',
                flex: 1,

            },
            {
                headerName: this.l('Invoice Date'),
                headerTooltip: this.l('Invoice Date'),
                field: 'invoiceDate',
                valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Receive Date'),
                headerTooltip: this.l('Receive Date'),
                field: 'receiveDate',
                valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Fix Lot'),
                headerTooltip: this.l('Fix Lot'),
                field: 'fixLot',
                flex: 1
            },
            {
                headerName: this.l('Carfamily Code'),
                headerTooltip: this.l('Carfamily Code'),
                field: 'carfamilyCode',
                flex: 1
            },
            {
                headerName: this.l('Customsdeclareno'),
                headerTooltip: this.l('Customsdeclareno'),
                field: 'customsDeclareNo',
                width: 130,
            },
            {
                headerName: this.l('Declare Date'),
                headerTooltip: this.l('Declare Date'),
                field: 'declareDate',
                valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                flex: 1
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceStock.getIdInvPeriod()
        .subscribe((result) => {
            this.cbbPeriod = result.items;
        });

    }

    searchDatas(): void {
        this._service.getAll(
            this.id,
            this.partno,
            this.supplierno,
            this.fixlot,
            this.carfamilycode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        this.id = 0,
        this.partno = '',
        this.supplierno = '',
        this.fixlot = '',
        this.carfamilycode = '',
        this.searchDatas();
    }

    onSelectionMulti(params) {
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRow, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRow.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.id,
            this.partno,
            this.supplierno,
            this.fixlot,
            this.carfamilycode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvInDetailsDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvInDetailsDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
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


    exportToExcel(): void {
        this.isLoading = true;
        this._service.getInDetailsToExcel(
            this.id,
            this.partno,
            this.supplierno,
            this.fixlot,
            this.carfamilycode
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }

    reportRequestDetails(){
        window.open("/app/main/cmm/reportrequest")
      }

}
