import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgaBarScanInfoDto, LgaBarScanInfoServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBarScanInfoModalComponent } from './create-or-edit-barscaninfo-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './barscaninfo.component.html',
    styleUrls: ['./barscaninfo.component.less'],
})
export class BarScanInfoComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBarScanInfo', { static: true }) createOrEditModalBarScanInfo: | CreateOrEditBarScanInfoModalComponent | undefined;
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

    selectedRow: LgaBarScanInfoDto = new LgaBarScanInfoDto();
    saveSelectedRow: LgaBarScanInfoDto = new LgaBarScanInfoDto();
    datas: LgaBarScanInfoDto = new LgaBarScanInfoDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    userId: string = '';
    userName: string = '';
    scanValue: string = '';
    scanPartNo: string = '';
    scanBackNo: string = '';
    scanType: string;
    scanDatetime: any = '';
    prodLine
    refId;
    status: string = '';
    isActive

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
        private _service: LgaBarScanInfoServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            // { headerName: this.l('User Id'), headerTooltip: this.l('User Id'), field: 'userId', flex: 1 },
            { headerName: this.l('User Name'), headerTooltip: this.l('User Name'), field: 'userName', flex: 1 },
            { headerName: this.l('Scan Value'), headerTooltip: this.l('Scan Value'), field: 'scanValue', flex: 1 },
            { headerName: this.l('Scan Part No'), headerTooltip: this.l('Scan Part No'), field: 'scanPartNo', flex: 1 },
            { headerName: this.l('Scan Back No'), headerTooltip: this.l('Scan Back No'), field: 'scanBackNo', flex: 1 },
            { headerName: this.l('Scan Type'), headerTooltip: this.l('Scan Type'), field: 'scanType', flex: 1 },
            { headerName: this.l('Scan Datetime'), headerTooltip: this.l('Scan Datetime'), field: 'scanDatetime', valueGetter: (params) => this.pipe.transform(params.data?.scanDatetime, 'dd/MM/yyyy hh:MM:ss'), flex: 1 },
            { headerName: this.l('Prod Line'), headerTooltip: this.l('Prod Line'), field: 'prodLine', flex: 1 },
            // { headerName: this.l('Ref Id'), headerTooltip: this.l('Ref Id'), field: 'refId', flex: 1 },

            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.status == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.status == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
        ]
            this.frameworkComponents = {

                agCellButtonComponent: AgCellButtonRendererComponent,
            };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.userName,
            this.scanPartNo,
            this.prodLine,
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
        this.userId = '',
            this.userName = '',
            this.scanPartNo = '',
            this.prodLine = '',
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
            this.userName,
            this.scanPartNo,
            this.prodLine,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaBarScanInfoDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaBarScanInfoDto();
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

    deleteRow(system: LgaBarScanInfoDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getBarScanInfoToExcel(
            this.userName,
            this.scanPartNo,
            this.prodLine
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }


}
