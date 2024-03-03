import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent, } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgaBp2PxPUpPlanDto, LgaBp2PxPUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './pxpupplan.component.html',
    styleUrls: ['./pxpupplan.component.less'],
})
export class PxPUpPlanComponent extends AppComponentBase implements OnInit {
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

    selectedRow: LgaBp2PxPUpPlanDto = new LgaBp2PxPUpPlanDto();
    saveSelectedRow: LgaBp2PxPUpPlanDto = new LgaBp2PxPUpPlanDto();
    datas: LgaBp2PxPUpPlanDto = new LgaBp2PxPUpPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    prodLine: string = '';
    noOfALineIn: number;
    unpackingTime: string = '';
    unpackingDate: any;
    caseNo: string;
    supplierNo: string;
    model: string;
    totalNoInShift: number;
    unpackingDatetime: any;
    workingDateFrom;
    workingDateTo
    workingDate: any;
    shift: string;
    upTable: string;
    finishDatetime: any;
    upLt: number;
    unpackingStartDatetime: any;
    unpackingFinishDatetime: any;
    unpackingSecond: number;
    unpackingBy: string;
    delaySecond: number;
    timeOffSecond: number;
    startPauseTime: any;
    endPauseTime: any;
    delayConfirmFlag: any;
    finishConfirmFlag: any;
    delayConfirmSecond: number;
    timeOffConfirmSecond: number;
    whLocation: string;
    invoiceDate: any;
    remarks: string;
    isNewPart: string = '';
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
        private _service: LgaBp2PxPUpPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],

            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',

            },
            {
                headerName: this.l('No Of A Line In'),
                headerTooltip: this.l('No Of A Line In'),
                field: 'noOfALineIn',

                type: 'rightAligned',
            },
            {
                headerName: this.l('Unpacking Time'),
                headerTooltip: this.l('UnpackingTime'),
                field: 'unpackingTime',

            },
            {
                headerName: this.l('Unpacking Date'),
                headerTooltip: this.l('Unpacking Date'),
                field: 'unpackingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy'),

            },
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',

            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',

            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',

            },
            {
                headerName: this.l('Total No In Shift'),
                headerTooltip: this.l('Total No In Shift'),
                field: 'totalNoInShift',

            },
            {
                headerName: this.l('Unpacking Datetime'),
                headerTooltip: this.l('Unpacking Datetime'),
                field: 'unpackingDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingDatetime, 'dd/MM/yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),

            },
            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',

            },
            {
                headerName: this.l('UpTable'),
                headerTooltip: this.l('UpTable'),
                field: 'upTable',

            },
            {
                headerName: this.l('Finish Datetime'),
                headerTooltip: this.l('Finish Datetime'),
                field: 'finishDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.finishDatetime, 'dd/MM/yyyy hh:MM:ss'),

            },
            {
                headerName: this.l('Up Lt'),
                headerTooltip: this.l('Up Lt'),
                field: 'upLt',

                type: 'rightAligned',
            },
            {
                headerName: this.l('Unpacking Start Datetime'),
                headerTooltip: this.l('Unpacking Start Datetime'),
                field: 'unpackingStartDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingStartDatetime, 'dd/MM/yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Unpacking Finish Datetime'),
                headerTooltip: this.l('Unpacking Finish Datetime'),
                field: 'unpackingFinishDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingFinishDatetime, 'dd/MM/yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Unpacking Second'),
                headerTooltip: this.l('Unpacking Second'),
                field: 'unpackingSecond',

                type: 'rightAligned',
            },
            {
                headerName: this.l('Unpacking By'),
                headerTooltip: this.l('Unpacking By'),
                field: 'unpackingBy',

            },
            {
                headerName: this.l('Delay Second'),
                headerTooltip: this.l('Delay Second'),
                field: 'delaySecond',

                type: 'rightAligned',
            },
            {
                headerName: this.l('Time Off Second'),
                headerTooltip: this.l('Time Off Second'),
                field: 'timeOffSecond',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Start Pause Time'),
                headerTooltip: this.l('Start Pause Time'),
                field: 'startPauseTime',
                valueGetter: (params) => this.pipe.transform(params.data?.startPauseTime, 'dd/MM/yyyy hh:MM:ss'),

            },
            {
                headerName: this.l('End Pause Time'),
                headerTooltip: this.l('End Pause Time'),
                field: 'endPauseTime',
                valueGetter: (params) => this.pipe.transform(params.data?.endPauseTime, 'dd/MM/yyyy hh:MM:ss'),

            },
            {
                headerName: this.l('Delay Confirm Flag'),
                headerTooltip: this.l('Delay Confirm Flag'),
                field: 'delayConfirmFlag',
                valueGetter: (params) => this.pipe.transform(params.data?.unpackingFinishDatetime, 'dd/MM/yyyy HH:mm:ss'),

            },
            {
                headerName: this.l('Finish Confirm Flag'),
                headerTooltip: this.l('Finish Confirm Flag'),
                field: 'finishConfirmFlag',
                valueGetter: (params) => this.pipe.transform(params.data?.finishConfirmFlag, 'dd/MM/yyyy hh:MM:ss'),

            },
            {
                headerName: this.l('Delay Confirm Second'),
                headerTooltip: this.l('Delay Confirm Second'),
                field: 'delayConfirmSecond',

            },
            {
                headerName: this.l('Time Off Confirm Second'),
                headerTooltip: this.l('Time Off Confirm Second'),
                field: 'timeOffConfirmSecond',

                type: 'rightAligned',
            },
            {
                headerName: this.l('Wh Location'),
                headerTooltip: this.l('Wh Location'),
                field: 'whLocation',

            },
            {
                headerName: this.l('Invoice Date'),
                headerTooltip: this.l('Invoice Date'),
                field: 'invoiceDate',
                valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),

            },
            {
                headerName: this.l('Remarks'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',

            },
            {
                headerName: this.l('Is New Part'),
                headerTooltip: this.l('Is New Part'),
                field: 'isNewPart',
                cellClass: ['text-center'],

                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isNewPart == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isNewPart == 'Y' ? 'btnActive' : 'btnInActive'),
                },
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],

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
        this._service
            .getAll(

                this.prodLine,
                this.caseNo,
                this.supplierNo,
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
        this.workingDate = '',
        this.caseNo = '',
        this.supplierNo = '',
        this.workingDateFrom = '',
        this.workingDateTo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.prodLine,
            this.caseNo,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDate),

            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaBp2PxPUpPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaBp2PxPUpPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
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
        this._service.getPxPUpPlanToExcel(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.prodLine,
            this.caseNo,
            this.supplierNo

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
