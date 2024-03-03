import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwLupLotUpPlanDto, LgwLupLotUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditLotUpPlanModalComponent } from './create-or-edit-lotupplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportLotupplanModalComponent } from './import-lotupplan-modal.component';
import { DateTimeProvider } from 'angular-oauth2-oidc';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './lotupplan.component.html',
    styleUrls: ['./lotupplan.component.less'],
})
export class LotUpPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalLotUpPlan', { static: true }) createOrEditModalLotUpPlan: | CreateOrEditLotUpPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportLotupplanModalComponent | undefined;


    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: LgwLupLotUpPlanDto = new LgwLupLotUpPlanDto();
    saveSelectedRow: LgwLupLotUpPlanDto = new LgwLupLotUpPlanDto();
    datas: LgwLupLotUpPlanDto = new LgwLupLotUpPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    prodLine: string = '';
    workingDate: any;
    workingDateFrom;
    workingDateTo;
    shift: string = '';
    noInShift: number = 0;
    noInDay: number = 0;
    lotNo: string = '';
    lotPartialNo: number = 0;
    unpackingStartDatetime: any;
    unpackingFinishDatetime: any;
    tpm: string = '';
    remarks: string = '';
    upCalltime: any;
    unpackingActualFinishDatetime: any;
    unpackingActualStartDatetime: any;
    upStatus: string = '';
    makingFinishDatetime: any;
    mkStatus: string = '';
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
        private _service: LgwLupLotUpPlanServiceProxy,
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
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 80,

            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd-MM-yyyy'),
                width: 120,
            },
            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                width: 60,

            },
            {
                headerName: this.l('No In Shift'),
                headerTooltip: this.l('No In Shift'),
                field: 'noInShift',
                width: 110,
                type: 'rightAligned',
            },
            {
                headerName: this.l('No In Day'),
                headerTooltip: this.l('No In Day'),
                field: 'noInDay',
                width: 110,
                type: 'rightAligned',

            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 100,

            },
            {
                headerName: this.l('Lot Partial No'),
                headerTooltip: this.l('Lot Partial No'),
                field: 'lotPartialNo',
                width: 120,
                type: 'rightAligned',

            },
            {
                headerName: this.l('Unpacking Start Datetime'),
                headerTooltip: this.l('Unpacking Start Datetime'),
                field: 'unpackingStartDatetime',
                width: 180,
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingStartDatetime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Unpacking Finish Datetime'),
                headerTooltip: this.l('Unpacking Finish Datetime'),
                field: 'unpackingFinishDatetime',
                width: 180,
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingFinishDatetime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Tpm'),
                headerTooltip: this.l('Tpm'),
                field: 'tpm',
                width: 80,

            },
            {
                headerName: this.l('Remarks'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',
                width: 100,

            },
            {
                headerName: this.l('Up Calltime'),
                headerTooltip: this.l('UpCalltime'),
                field: 'upCalltime',
                width: 90,
                valueGetter: (params) => this.pipe.transform(params.data?.upCalltime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Unpacking Actual Finish Datetime'),
                headerTooltip: this.l('Unpacking Actual Finish Datetime'),
                field: 'unpackingActualFinishDatetime',
                width: 220,
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingActualFinishDatetime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Making Finish Datetime'),
                headerTooltip: this.l('Making Finish Datetime'),
                field: 'makingFinishDatetime',
                width: 180,
                valueGetter: (params) => this.pipe.transform(params.data?.makingFinishDatetime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Up Status'),
                headerTooltip: this.l('Up Status'),
                field: 'upStatus',
                width: 100,

            },
            {
                headerName: this.l('Unpacking Actual Start Datetime'),
                headerTooltip: this.l('Unpacking Actual Start Datetime'),
                field: 'unpackingActualStartDatetime',
                width: 220,
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingActualStartDatetime, 'dd-MM-yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Mk Status'),
                headerTooltip: this.l('Mk Status'),
                field: 'mkStatus',
                width: 100,

            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
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
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });
    }

    clearTextSearch() {
            this.lotNo = '',
            this.workingDateFrom = '',
            this.workingDateTo = '',
            this.searchDatas();
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){
       
        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true, 
            });
            this.autoSizeAll();
        }, 1000)}

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
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }
    getDateNow() {

        var today = new Date();
        var date = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        return date;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwLupLotUpPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwLupLotUpPlanDto();
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
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    deleteRow(system: LgwLupLotUpPlanDto): void {
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
        this._service.getLotUpPlanToExcel(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.lotNo,
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }



}
