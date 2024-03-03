import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditCallinglightstatusModalComponent } from './create-or-edit-callinglightstatus-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { LgwAdoCallingLightStatusDto, LgwAdoCallingLightStatusServiceProxy } from '@shared/service-proxies/service-proxies';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './callinglightstatus.component.html',
    styleUrls: ['./callinglightstatus.component.less'],
})
export class CallinglightstatusComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalCallingLightStatus', { static: true }) createOrEditModalCallingLightStatus:| CreateOrEditCallinglightstatusModalComponent| undefined;
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

    selectedRow: LgwAdoCallingLightStatusDto = new LgwAdoCallingLightStatusDto();
    saveSelectedRow: LgwAdoCallingLightStatusDto = new LgwAdoCallingLightStatusDto();
    datas: LgwAdoCallingLightStatusDto = new LgwAdoCallingLightStatusDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    code : string = '' ;
	lightName : string = '' ;
	prodLine : string = '' ;
	process : string = '' ;
	blockCode : string = '' ;
	blockDescription : string = '' ;
    sorting : string = '' ;
    signalId : number = 0 ;
    signalCode : string = '' ;
    workingDate : any ;
	shift : string = '' ;
    startDate : any ;
    finishDate : any ;
    status : string = '' ;
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
        private _service: LgwAdoCallingLightStatusServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Code'),headerTooltip: this.l('Code'),field: 'code',width:150},
			{headerName: this.l('Light Name'),headerTooltip: this.l('Light Name'),field: 'lightName',width:150},
			{headerName: this.l('Prod Line'),headerTooltip: this.l('Prod Line'),field: 'prodLine',width: 100},
			{headerName: this.l('Process'),headerTooltip: this.l('Process'),field: 'process',width: 100},
			{headerName: this.l('Block Code'),headerTooltip: this.l('Block Code'),field: 'blockCode',width: 100},
			{headerName: this.l('Block Description'),headerTooltip: this.l('Block Description'),field: 'blockDescription',width: 100},
			{headerName: this.l('Sorting'),headerTooltip: this.l('Sorting'),field: 'sorting',width:150},
			{headerName: this.l('Signal Id'),headerTooltip: this.l('Signal Id'),field: 'signalId',width: 100},
			{headerName: this.l('Signal Code'),headerTooltip: this.l('Signal Code'),field: 'signalCode',width: 100},
			{headerName: this.l('Start Date'),headerTooltip: this.l('Start Date'),field: 'startDate',valueGetter: (params) => this.pipe.transform(params.data?.startDate, 'dd/MM/yyyy'),width: 100},
			{headerName: this.l('Finsh Date'),headerTooltip: this.l('Finsh Date'),field: 'finshDate',valueGetter: (params) => this.pipe.transform(params.data?.finshDate, 'dd/MM/yyyy'),width: 100},
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status',width: 100},
            {headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),width: 100},
            {headerName: this.l('Shift'),headerTooltip: this.l('Shift'),field: 'shift',width: 100},
            {headerName: this.l('NoInDate'),headerTooltip: this.l('NoInDate'),field: 'noInDate',width: 100},
            {headerName: this.l('NoInShift'),headerTooltip: this.l('NoInShift'),field: 'noInShift',width: 100},
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
			this.code,
            this.prodLine,
            this.shift,
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
        },100)
    }

    clearTextSearch() {
        this.code = '',
		this.prodLine = '',
		this.shift = '',
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
            this.code,
            this.prodLine,
            this.shift,
			this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwAdoCallingLightStatusDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwAdoCallingLightStatusDto();
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

    deleteRow(system: LgwAdoCallingLightStatusDto): void {
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
        this._service.getCallingLightStatusToExcel(
            this.code,
            this.prodLine,
            this.shift,
			this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }

    getDateNow() {
        var today = new Date();
        var date = today.getDate()+'/'+(today.getMonth()+1)+'/'+today.getFullYear();
        return date;
    }
}
