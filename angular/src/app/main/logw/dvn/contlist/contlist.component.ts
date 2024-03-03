import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwDvnContListDto, LgwDvnContListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditContListModalComponent } from './create-or-edit-contlist-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contlist.component.html',
    styleUrls: ['./contlist.component.less'],
})
export class ContListComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalContList', { static: true }) createOrEditModalContList: | CreateOrEditContListModalComponent | undefined;
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

    selectedRow: LgwDvnContListDto = new LgwDvnContListDto();
    saveSelectedRow: LgwDvnContListDto = new LgwDvnContListDto();
    datas: LgwDvnContListDto = new LgwDvnContListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    containerNo: string = '';
    renban: string = '';
    supplierNo: string = '';
    lotNo: string = '';
    workingDate: any;
    shiftNo: string = '';
    devanningDock: string = '';
    planDevanningDate: any;
    actDevanningDate: any;
    actDevanningDateFinished: any;
    devanningType: string = '';
    status: string = '';
    devLeadtime: number = 0;
    planDevanningLineOff: number = 0;
    sortingStatus: string = '';
    isActive: string = '';
    workingDateFrom;
    workingDateTo;

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
        private _service: LgwDvnContListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                width: 120
            },
            {
                headerName: this.l('Renban'),
                headerTooltip: this.l('Renban'),
                field: 'renban',
                width: 120
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width: 120
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 120
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy') ? 'Working Date : ' + this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy') : this.l('Working Date : '),
                width: 120
            },
            {
                headerName: this.l('Shift No'),
                headerTooltip: this.l('Shift No'),
                field: 'shiftNo',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.shiftNo ? 'Shift No : ' + params.data.shiftNo : this.l('Shift No : '),

                width: 120
            },
            {
                headerName: this.l('Devanning Dock'),
                headerTooltip: this.l('Devanning Dock'),
                field: 'devanningDock',
                width: 120
            },
            {
                headerName: this.l('Plan Devanning Date'),
                headerTooltip: this.l('Plan Devanning Date'),
                field: 'planDevanningDate', valueGetter: (params) => this.pipe.transform(params.data?.planDevanningDate, 'dd/MM/yyyy HH:mm'),
                width: 150
            },
            {
                headerName: this.l('Act Devanning Date'),
                headerTooltip: this.l('Act Devanning Date'),
                field: 'actDevanningDate', valueGetter: (params) => this.pipe.transform(params.data?.actDevanningDate, 'dd/MM/yyyy HH:mm'),
                width: 150
            },
            {
                headerName: this.l('Act Devanning Date Finished'),
                headerTooltip: this.l('Act Devanning Date Finished'),
                field: 'actDevanningDateFinished', valueGetter: (params) => this.pipe.transform(params.data?.actDevanningDateFinished, 'dd/MM/yyyy HH:mm'),
                width: 180
            },
            {
                headerName: this.l('Devanning Type'),
                headerTooltip: this.l('Devanning Type'),
                field: 'devanningType',
                width: 120
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                width: 120
            },
            {
                headerName: this.l('Dev Leadtime'),
                headerTooltip: this.l('Dev Leadtime'),
                field: 'devLeadtime',
                width: 120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Plan Devanning Line Off'),
                headerTooltip: this.l('Plan Devanning Line Off'),
                field: 'planDevanningLineOff',
                width: 170,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Sorting Status'),
                headerTooltip: this.l('Sorting Status'),
                field: 'sortingStatus',
                width: 120
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },
                width: 120,
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
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.containerNo,
            this.renban,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.workingDate),
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
        this.containerNo = '',
        this.renban = '',
        this.lotNo = '',
        this.workingDate = '',
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
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.containerNo,
            this.renban,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.workingDate),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwDvnContListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwDvnContListDto();
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

    deleteRow(system: LgwDvnContListDto): void {
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
        this._service.getContListToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.containerNo,
            this.renban,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.workingDate),
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
}
