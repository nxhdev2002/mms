import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwPikPickingProgressDto, LgwPikPickingProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './pickingprogress.component.html',
    styleUrls: ['./pickingprogress.component.less'],
})
export class PickingProgressComponent extends AppComponentBase implements OnInit {
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

    selectedRow: LgwPikPickingProgressDto = new LgwPikPickingProgressDto();
    saveSelectedRow: LgwPikPickingProgressDto = new LgwPikPickingProgressDto();
    datas: LgwPikPickingProgressDto = new LgwPikPickingProgressDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    pickingTabletId: string = '';
    prodLine: string = '';
    bodyNo: string = '';
    lotNo: string = '';
    processCode: string = '';
    processGroup: string = '';
    seqNo: number = 0;
    workingDate: any;
    workingDateFrom;
    workingDateTo;
    shift: string = '';
    taktStartTime: any;
    startTime: any;
    finishTime: any;
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
        private _service: LgwPikPickingProgressServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
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
                headerName: this.l('Picking Tablet Id'),
                headerTooltip: this.l('Picking Tablet Id'),
                field: 'pickingTabletId',
                width: 130,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo'
                , flex: 1
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo'
                , flex: 1
            },
            {
                headerName: this.l('Process Code'),
                headerTooltip: this.l('Process Code'),
                field: 'processCode',
                 width: 100
            },
            {
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'),
                field: 'processGroup',
                width: 100
            },
            {
                headerName: this.l('Seq No'),
                headerTooltip: this.l('Seq No'),
                field: 'seqNo',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                width: 100
            },
            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                flex: 1
            },
            {
                headerName: this.l('Takt Start Time'),
                headerTooltip: this.l('Takt Start Time'),
                field: 'taktStartTime',
                valueGetter: (params) => this.pipe.transform(params.data?.taktStartTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 120,
            },

            {
                headerName: this.l('Start Time'),
                headerTooltip: this.l('Start Time'),
                field: 'startTime',
                valueGetter: (params) => this.pipe.transform(params.data?.startTime, 'dd/MM/yyyy HH:mm:ss'),
                flex: 1
            },
            {
                headerName: this.l('Finish Time'),
                headerTooltip: this.l('Finish Time'),
                field: 'finishTime',
                valueGetter: (params) => this.pipe.transform(params.data?.finishTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 100
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                width: 90,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
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
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.prodLine,
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
                this.resetGridView();
            });
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
        },1000)
    }
    clearTextSearch() {
        this.prodLine = '',
        this.processCode = '',
        this.workingDateFrom = '',
        this.workingDateTo = '',
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
            this.prodLine,
            this.processCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwPikPickingProgressDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwPikPickingProgressDto();
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


    exportToExcel(): void {
        this.isLoading = true;
        this._service.getPickingProgressToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.prodLine,
            this.processCode,
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }



    getDateNow() {
        var today = new Date();
        var date = today.getDate()+'/'+(today.getMonth()+1)+'/'+today.getFullYear();
        return date;
    }
}
