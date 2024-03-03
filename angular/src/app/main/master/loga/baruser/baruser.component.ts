import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaBarUserDto, MstLgaBarUserServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBarUserModalComponent } from './create-or-edit-baruser-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './baruser.component.html',
    styleUrls: ['./baruser.component.less'],
})
export class BarUserComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBarUser', { static: true }) createOrEditModalBarUser: | CreateOrEditBarUserModalComponent | undefined;
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

    selectedRow: MstLgaBarUserDto = new MstLgaBarUserDto();
    saveSelectedRow: MstLgaBarUserDto = new MstLgaBarUserDto();
    datas: MstLgaBarUserDto = new MstLgaBarUserDto();
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
    userDescription: string = '';
    isNeedPass: string = '';
    pwd: string = '';
    processId
    processCode: string = '';
    processGroup: string = '';
    processSubgroup: string = '';
    isActive: string = '';

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
        private _service: MstLgaBarUserServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('User Id'),
                headerTooltip: this.l('User Id'),
                field: 'userId',
                flex: 1
            },
            {
                headerName: this.l('User Name'),
                headerTooltip: this.l('User Name'),
                field: 'userName',
                flex: 1
            },
            {
                headerName: this.l('User Description'),
                headerTooltip: this.l('User Description'),
                field: 'userDescription',
                flex: 1
            },
            {
                headerName: this.l('Is Need Pass'),
                headerTooltip: this.l('Is Need Pass'),
                field: 'isNeedPass',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isNeedPass == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isNeedPass == 'Y' ? 'btnActive' : 'btnInActive'),
                },
            },
            {
                headerName: this.l('Pwd'),
                headerTooltip: this.l('Pwd'),
                field: 'pwd',
                flex: 1
            },
            {
                headerName: this.l('Process Id'),
                headerTooltip: this.l('Process Id'),
                field: 'processId',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Process Code'),
                headerTooltip: this.l('Process Code'),
                field: 'processCode',
                flex: 1
            },
            {
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'),
                field: 'processGroup',
                flex: 1
            },
            {
                headerName: this.l('Process Subgroup'),
                headerTooltip: this.l('Process Subgroup'),
                field: 'processSubgroup',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isActive == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isActive == 'Y' ? 'btnActive' : 'btnInActive'),
                },
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.userName,
            this.processCode,
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
            this.userDescription = '',
            this.isNeedPass = '',
            this.pwd = '',
            this.processId = '',
            this.processCode = '',
            this.processGroup = '',
            this.processSubgroup = '',
            this.isActive = '',
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
            this.processCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgaBarUserDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgaBarUserDto();
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

    deleteRow(system: MstLgaBarUserDto): void {
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
        this._service.getBarUserToExcel(
            this.userName,
            this.processCode,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
 

}
