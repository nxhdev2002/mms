import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsDailyOrderDto, InvGpsDailyOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditInvGpsDailyOrderModalComponent } from './create-or-edit-invgpsdailyorder-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './invgpsdailyorder.component.html',
    styleUrls: ['./invgpsdailyorder.component.less'],
})
export class InvGpsDailyOrderComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalInvGpsDailyOrder', { static: true }) createOrEditModalInvGpsDailyOrder: | CreateOrEditInvGpsDailyOrderModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvGpsDailyOrderDto = new InvGpsDailyOrderDto();
    saveSelectedRow: InvGpsDailyOrderDto = new InvGpsDailyOrderDto();
    datas: InvGpsDailyOrderDto = new InvGpsDailyOrderDto();
    isLoading = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    shift = '';
    workingDate: any;
    supplierName = '';
    supplierCode = '';
    orderNo = '';
    truckNo = '';
    orderDatetime: any; ;
    estArrivalDatetime: any; ;
    tripNo = 0;
    truckUnloadingId;
    isActive;
    workingDateFrom = new Date();
    workingDateTo = new Date();

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) {
        return null;
        }
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service: InvGpsDailyOrderServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'), flex: 1 },
            { headerName: this.l('Shift'), headerTooltip: this.l('Shift'), field: 'shift', flex: 1 },
            { headerName: this.l('Supplier Name'), headerTooltip: this.l('Supplier Name'), field: 'supplierName', flex: 1 },
            { headerName: this.l('Supplier Code'), headerTooltip: this.l('Supplier Code'), field: 'supplierCode', flex: 1 },
            { headerName: this.l('Order No'), headerTooltip: this.l('Order No'), field: 'orderNo', flex: 1 },
            { headerName: this.l('Order Datetime'), headerTooltip: this.l('Order Datetime'), field: 'orderDatetime', valueGetter: (params) => this.pipe.transform(params.data?.orderDatetime, 'dd/MM/yyyy HH:mm:ss'), flex: 1 },
            { headerName: this.l('Trip No'), headerTooltip: this.l('Trip No'), field: 'tripNo', flex: 1,type: 'rightAligned' },
            { headerName: this.l('Truck No'), headerTooltip: this.l('Truck No'), field: 'truckNo', flex: 1 },
            { headerName: this.l('Est Arrival Datetime'), headerTooltip: this.l('Est Arrival Datetime'), field: 'estArrivalDatetime', valueGetter: (params) => this.pipe.transform(params.data?.estArrivalDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 150 },
           // { headerName: this.l('Truck Unloading Id'), headerTooltip: this.l('Truck Unloading Id'), field: 'truckUnloadingId', flex: 1 },
            // { headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: {
            //         // eslint-disable-next-line eqeqeq
            //         text: (params: { data: { isActive: string } }) => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle',
            //         // eslint-disable-next-line eqeqeq
            //         className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
            //     },
            // }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
       // this.resetGridView();
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            this.supplierCode,
            this.orderNo,
            this.truckNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                //this.resetGridView();

            });
    }
    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi?.getAllColumns()?.forEach((column) => {
            // eslint-disable-next-line eqeqeq
            if (column.getId().toString() != 'checked' && column.getId().toString() != 'stt') {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi?.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000);
    }

    clearTextSearch() {
        this.workingDateFrom = new Date();
        this.workingDateTo = new Date();
        this.shift = '';
        this.supplierCode = '';
        this.orderNo = '';
        this.truckNo = '';
        this.searchDatas();
    }

    onSelectionMulti() {
        let selectedRows = this.gridApi.getSelectedRows();
        let selectedRowsString = '';
        let maxToShow = 5;
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
            let othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            this.supplierCode,
            this.orderNo,
            this.truckNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsDailyOrderDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsDailyOrderDto();
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
            //this.resetGridView();

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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                //this.resetGridView();

            });
    }

    deleteRow(system: InvGpsDailyOrderDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getInvGpsDailyOrderToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            this.supplierCode,
            this.orderNo,
            this.truckNo
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
