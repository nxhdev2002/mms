import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgaEkbProgressDetailsDto, LgaEkbProgressDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditProgressDetailsModalComponent } from './create-or-edit-progressdetails-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './progressdetails.component.html',
    styleUrls: ['./progressdetails.component.less'],
})
export class ProgressDetailsComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalProgressDetails', { static: true }) createOrEditModalProgressDetails: | CreateOrEditProgressDetailsModalComponent | undefined;
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

    selectedRow: LgaEkbProgressDetailsDto = new LgaEkbProgressDetailsDto();
    saveSelectedRow: LgaEkbProgressDetailsDto = new LgaEkbProgressDetailsDto();
    datas: LgaEkbProgressDetailsDto = new LgaEkbProgressDetailsDto();
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
    workingDateTo
    shift: string = '';
    noInShift: number = 0;
    noInDate: number = 0;
    progressId
    processId
    partListId
    partNo: string = '';
    partNoNormalized: string = '';
    backNo: string = '';
    pcAddress: string = '';
    spsAddress: string = '';
    sorting: number = 0;
    usageQty: number = 0;
    bodyNo: string = '';
    lotNo: string = '';
    noInLot;
    grade: string = '';
    model: string = '';
    bodyColor: string = '';
    scanQty: number = 0;
    inputQty: number = 0;
    isZeroKb: string = '';
    startDatetime : any ;
    pikStartDatetime: any;
    pikFinishDatetime: any;
    delStartDatetime: any;
    delFinishDatetime: any;
    status: string = '';
    isActive: string = '';
    kanbanSeq: string = '';
    sequenceNo: any;
    body: any;

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
        private _service: LgaEkbProgressDetailsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                width: 90
            },
            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('No In Shift'),
                headerTooltip: this.l('No In Shift'),
                field: 'noInShift',
                width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('No In Date'),
                headerTooltip: this.l('No In Date'),
                field: 'noInDate',
                width: 90
            },
            // { headerName: this.l('Progress Id'), headerTooltip: this.l('Progress Id'), field: 'progressId', width:90 },
            // { headerName: this.l('Process Id'), headerTooltip: this.l('Process Id'), field: 'processId', width:90 },
            // { headerName: this.l('Part List Id'), headerTooltip: this.l('Part List Id'), field: 'partListId', width:90 },
            {
                headerName: this.l('Kanban Seq'),
                headerTooltip: this.l('Kanban Seq'), field: 'kanbanSeq', width: 90
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'), field: 'partNo', width: 100
            },
            {
                headerName: this.l('Part No Normalized'),
                headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized', width: 130
            },
            {
                headerName: this.l('Back No'),
                headerTooltip: this.l('Back No'), field: 'backNo', width: 90
            },
            {
                headerName: this.l('Pc Address'),
                headerTooltip: this.l('Pc Address'), field: 'pcAddress', width: 90
            },
            {
                headerName: this.l('Sps Address'),
                headerTooltip: this.l('Sps Address'), field: 'spsAddress', width: 90
            },
            {
                headerName: this.l('Sorting'),
                headerTooltip: this.l('Sorting'), field: 'sorting', width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('Usage Qty'),
                headerTooltip: this.l('Usage Qty'), field: 'usageQty', width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'), field: 'sequenceNo', width: 90
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'), field: 'bodyNo', width: 90
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'), field: 'lotNo', width: 90
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'), field: 'noInLot', width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'), field: 'grade', width: 90
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'), field: 'model', width: 90
            },
            {
                headerName: this.l('BodyColor'),
                headerTooltip: this.l('BodyColor'), field: 'bodyColor', width: 90
            },
            {
                headerName: this.l('Ekb Qty'),
                headerTooltip: this.l('Ekb Qty'), field: 'ekbQty', width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('Remain Qty'),
                headerTooltip: this.l('Remain Qty'), field: 'remainQty', width: 90, type: 'rightAligned'
            },
            {
                headerName: this.l('Is Zero Kb'),
                headerTooltip: this.l('Is Zero Kb'),
                field: 'isZeroKb',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isZeroKb == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isZeroKb == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
            {
                headerName: this.l('Newtakt Datetime'),
                headerTooltip: this.l('Newtakt Datetime'), field: 'newtaktDatetime', valueGetter: (params) => this.pipe.transform(params.data?.newtaktDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 90
            },
            {
                headerName: this.l('Pik Start Datetime'),
                headerTooltip: this.l('Pik Start Datetime'), field: 'pikStartDatetime', valueGetter: (params) => this.pipe.transform(params.data?.pikStartDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 90
            },
            {
                headerName: this.l('Pik Finish Datetime'),
                headerTooltip: this.l('Pik Finish Datetime'), field: 'pikFinishDatetime', valueGetter: (params) => this.pipe.transform(params.data?.pikFinishDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 90
            },
            {
                headerName: this.l('Del Start Datetime'),
                headerTooltip: this.l('Del Start Datetime'), field: 'delStartDatetime', valueGetter: (params) => this.pipe.transform(params.data?.delStartDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 90
            },
            {
                headerName: this.l('Del Finish Datetime'),
                headerTooltip: this.l('Del Finish Datetime'), field: 'delFinishDatetime', valueGetter: (params) => this.pipe.transform(params.data?.delFinishDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 90
            },

            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'), field: 'status', width: 90
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
            }
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
            this.prodLine,
            this.shift,
            this.partNo,
            this.grade,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.kanbanSeq,
            this.sequenceNo,
            this.body,
            this.lotNo,
            this.noInLot,
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
            this.workingDateFrom = '',
            this.workingDateTo = '',
            this.shift = '',
            this.partNo = '',
            this.grade = '',
            this.kanbanSeq = '',
            this.sequenceNo = '',
            this.body = '',
            this.lotNo = '',
            this.noInLot = '',
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
            this.prodLine,
            this.shift,
            this.partNo,
            this.grade,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.kanbanSeq,
            this.sequenceNo,
            this.body,
            this.lotNo,
            this.noInLot,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaEkbProgressDetailsDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaEkbProgressDetailsDto();
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

    deleteRow(system: LgaEkbProgressDetailsDto): void {
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
        this._service.getProgressDetailsToExcel(
            this.prodLine,
            this.shift,
            this.partNo,
            this.grade,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.kanbanSeq,
            this.sequenceNo,
            this.body,
            this.lotNo,
            this.noInLot,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
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
