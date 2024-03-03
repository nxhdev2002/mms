import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PtsAdoPaintingDataDto, PtsAdoPaintingDataServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './paintingdata.component.html',
    styleUrls: ['./paintingdata.component.less'],
})
export class PaintingDataComponent extends AppComponentBase implements OnInit {
    //@ViewChild('createOrEditModalPaintingData', { static: true }) createOrEditModalPaintingData:| CreateOrEditPaintingDataModalComponent| undefined;
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

    selectedRow: PtsAdoPaintingDataDto = new PtsAdoPaintingDataDto();
    saveSelectedRow: PtsAdoPaintingDataDto = new PtsAdoPaintingDataDto();
    datas: PtsAdoPaintingDataDto = new PtsAdoPaintingDataDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    lifetimeNo
	model : string = '' ;
	cfc : string = '' ;
	grade : string = '' ;
	lotNo : string = '' ;
	noInLot : number = 0 ;
	bodyNo : string = '' ;
	color : string = '' ;
	prodLine : string = '' ;
	processGroup : number = 0 ;
	subGroup : number = 0 ;
	processSeq : number = 0 ;
	filler : number = 0 ;
    workingDateFrom;
    workingDateTo
	workingDate : any ;
	shift : string = '' ;
	noInDate : number = 0 ;
	processCode : string = '' ;
	infoProcess : string = '' ;
	infoProcessNo : number = 0 ;
	isProject : string = '' ;
	isActive : string = '' ;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
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
        private _service: PtsAdoPaintingDataServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Lifetime No'),
                headerTooltip: this.l('Lifetime No'),
                field: 'lifetimeNo',
                width: 70
            },
			{
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 70
            },
			{
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                width: 70
            },
			{
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 70},
			{
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 70},
			{
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 70},
			{
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 70},
			{
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                width: 70},
			{
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 70
            },
			{
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'),
                field: 'processGroup',
                width: 70
            },
			{
                headerName: this.l('Sub Group'),
                headerTooltip: this.l('Sub Group'),
                field: 'subGroup',
                width: 70
            },
			{
                headerName: this.l('Process Seq'),
                headerTooltip: this.l('Process Seq'),
                field: 'processSeq',
                width: 70
            },
			{
                headerName: this.l('Filler'),
                headerTooltip: this.l('Filler'),
                field: 'filler',
                width: 70
            },
			{
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                width:100
            },
			{
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                width: 70
            },
			{
                headerName: this.l('No In Date'),
                headerTooltip: this.l('No In Date'),
                field: 'noInDate',
                width: 70
            },
			{
                headerName: this.l('Process Code'),
                headerTooltip: this.l('Process Code'),
                field: 'processCode',
                width: 70
            },
			{
                headerName: this.l('Info Process'),
                headerTooltip: this.l('Info Process'),
                field: 'infoProcess',
                width: 70
            },
			{
                headerName: this.l('Info Process No'),
                headerTooltip: this.l('Info Process No'),
                field: 'infoProcessNo',
                width: 70
            },
			{
                headerName: this.l('Is Project'),
                headerTooltip: this.l('Is Project'),
                field:  'isProject',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo:
                {
                    text: params => (params.data?.isProject == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isProject == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
			{
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field:  'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo:
                {
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
			this.model,
			this.cfc,
			this.grade,
			this.prodLine,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
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
        this.lifetimeNo = '',
		this.model = '',
		this.cfc = '',
		this.grade = '',
		this.lotNo = '',
		this.noInLot = 0,
		this.bodyNo = '',
		this.color = '',
		this.prodLine = '',
        this.workingDateFrom = '',
        this.workingDateTo = '',
		this.processGroup = 0,
		this.subGroup = 0,
		this.processSeq = 0,
		this.filler = 0,
		this.workingDate = '',
		this.shift = '',
		this.noInDate = 0,
		this.processCode = '',
		this.infoProcess = '',
		this.infoProcessNo = 0,
		this.isProject = '',
		this.isActive = '',
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
s
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.model,
			this.cfc,
			this.grade,
			this.prodLine,
			this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PtsAdoPaintingDataDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PtsAdoPaintingDataDto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPaintingDataToExcel(
			this.model,
			this.cfc,
			this.grade,
			this.prodLine,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            )
            .subscribe((result) => {
                 setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }


    getDateNow() {
        var today = new Date();
        var date = today.getDate()+'/'+(today.getMonth()+1)+'/'+today.getFullYear();
        return date;
    }
}
