import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgaEkbProgressDto, LgaEkbProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditProgressModalComponent } from './create-or-edit-progress-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './progress.component.html',
    styleUrls: ['./progress.component.less'],
})
export class ProgressComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalProgress', { static: true }) createOrEditModalProgress:| CreateOrEditProgressModalComponent| undefined;
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

    selectedRow: LgaEkbProgressDto = new LgaEkbProgressDto();
    saveSelectedRow: LgaEkbProgressDto = new LgaEkbProgressDto();
    datas: LgaEkbProgressDto = new LgaEkbProgressDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    prodLine : string = '' ;
    workingDateFrom;
    workingDateTo
	workingDate : any ;
	shift : string = '' ;
	noInShift : number = 0 ;
	noInDate : number = 0 ;
	processId
	processCode : string = '' ;
	newtaktDatetime : any ;
	startDatetime : any ;
	finishDatetime : any ;
	status : string = '' ;
	isActive : string = '' ;

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
        private _service: LgaEkbProgressServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Prod Line'),headerTooltip: this.l('Prod Line'),field: 'prodLine',flex: 1},
			{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),flex: 1},
			{headerName: this.l('Shift'),headerTooltip: this.l('Shift'),field: 'shift',flex: 1},
			{headerName: this.l('No In Shift'),headerTooltip: this.l('No In Shift'),field: 'noInShift',flex: 1,type: 'rightAligned',},
			{headerName: this.l('No In Date'),headerTooltip: this.l('No In Date'),field: 'noInDate',flex: 1},
			// {headerName: this.l('Process Id'),headerTooltip: this.l('Process Id'),field: 'processId',flex: 1},
			{headerName: this.l('Process Code'),headerTooltip: this.l('Process Code'),field: 'processCode',flex: 1},
			{headerName: this.l('Newtakt Datetime'),headerTooltip: this.l('Newtakt Datetime'),field: 'newtaktDatetime',valueGetter: (params) => this.pipe.transform(params.data?.newtaktDatetime, 'dd/MM/yyyy HH:mm:ss'),flex: 1},
			{headerName: this.l('Start Datetime'),headerTooltip: this.l('Start Datetime'),field: 'startDatetime',valueGetter: (params) => this.pipe.transform(params.data?.startDatetime, 'dd/MM/yyyy HH:mm:ss'),flex: 1},
			{headerName: this.l('Finish Datetime'),headerTooltip: this.l('Finish Datetime'),field: 'finishDatetime',valueGetter: (params) => this.pipe.transform(params.data?.finishDatetime, 'dd/MM/yyyy HH:mm:ss'),flex: 1},
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status',flex: 1},
			{
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field:  'isActive',
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
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			this.processCode,
			this._dateTimeService.convertToDatetime(this.startDatetime),
			this._dateTimeService.convertToDatetime(this.finishDatetime),
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
        this.prodLine = '',
        this.workingDateFrom = '',
        this.workingDateTo = '',
		this.shift = '',
		this.processCode = '',
		this.startDatetime = '',
		this.finishDatetime = '',
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
            }, 1000)
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
			this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			this.processCode,
			this.startDatetime,
			this.finishDatetime,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize,

        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaEkbProgressDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaEkbProgressDto();
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

    deleteRow(system: LgaEkbProgressDto): void {
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
        this._service.getProgressToExcel(
            this.prodLine,
			this.workingDate,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			this.processCode,
			this.startDatetime,
			this.finishDatetime
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
