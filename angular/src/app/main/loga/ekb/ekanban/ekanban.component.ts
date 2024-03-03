import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';
import { AfterViewInit, Component, OnInit, ViewChild, ElementRef, Injector, HostListener } from '@angular/core';
import { BigPartTabletAndonOutput, BigPartTabletEcarOutput, LgaBp2BigPartDDLServiceProxy, LgaEkbEkanbanDto, LgaEkbEkanbanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { Paginator } from 'primeng/paginator';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ceil } from 'lodash-es';

@Component({
    selector: 'app-ekanban',
    templateUrl: './ekanban.component.html',
    styleUrls: ['./ekanban.component.less']
})
export class EkanbanComponent extends AppComponentBase implements OnInit {

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

    selectedRow: LgaEkbEkanbanDto = new LgaEkbEkanbanDto();
    saveSelectedRow: LgaEkbEkanbanDto = new LgaEkbEkanbanDto();
    datas: LgaEkbEkanbanDto = new LgaEkbEkanbanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    kanbanSeq : string = '';
    kanbanNoInDate : number;
    prodLine : string = '';
	workingDate : any ;
	workingDateFrom : any ;
	workingDateTo : any ;
	shift : string = '' ;
    progressId : number;
    processId : number;
    psrtListId : number;
    partNo : string = '';
    partNoNormalized : string = '';
    backNo : string = '';
    pcAddress : string = '';
    spsAddress : string = '';
    sorting : number;
    requestQty : number;
    scanQty : number;
    inputQty : number;
    isZeroKb : string = '';
    requestDatetime : any;
    pikStartDatetime : any;
    pikFinishDatetime : any;
    delStartDatetime : any;
    delFinishDatetime : any;
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
        private _service: LgaEkbEkanbanServiceProxy,
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
                headerName: this.l('Kanban Seq'),
                headerTooltip: this.l('Kanban Seq'),
                field: 'kanbanSeq',
                width: 120,
            },
            {
                headerName: this.l('Kanban No In Date'),
                headerTooltip: this.l('Kanban No In Date'),
                field: 'kanbanNoInDate',
                width: 90,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90,
            },
			{
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                width: 90,
            },
			{
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                width: 90,
            },
            {
                headerName: this.l('Progress Id'),
                headerTooltip: this.l('Progress Id'),
                field: 'progressId',
                width: 60,
            },
            {
                headerName: this.l('Process Id'),
                headerTooltip: this.l('Process Id'),
                field: 'processId',
                width: 90,
            },
            {
                headerName: this.l('Part List Id'),
                headerTooltip: this.l('Part List Id'),
                field: 'partListId',
                width: 90,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                width: 90,
            },
            {
                headerName: this.l('Part No Normalized'),
                headerTooltip: this.l('Part No Normalized'),
                field: 'partNoNormalized',
                width: 90,
            },
            {
                headerName: this.l('Back No'),
                headerTooltip: this.l('Back No'),
                field: 'backNo',
                width: 90,
            },
            {
                headerName: this.l('Pc Address'),
                headerTooltip: this.l('Pc Address'),
                field: 'pcAddress',
                width: 90,
            },
            {
                headerName: this.l('Sps Address'),
                headerTooltip: this.l('Sps Address'),
                field: 'spsAddress',
                width: 90,
            },
            {
                headerName: this.l('Sorting'),
                headerTooltip: this.l('Sorting'),
                field: 'sorting',
                width: 90,
            },
            {
                headerName: this.l('Request Qty'),
                headerTooltip: this.l('Request Qty'),
                field: 'requestQty',
                width: 90,
            },
            {
                headerName: this.l('Scan Qty'),
                headerTooltip: this.l('Scan Qty'),
                field: 'scanQty',
                width: 90,
            },
            {
                headerName: this.l('Input Qty'),
                headerTooltip: this.l('Input Qty'),
                field: 'inputQty',
                width: 90,
            },
            {
                headerName: this.l('Is Zero Kb'),
                headerTooltip: this.l('Is Zero Kb'),
                field: 'isZeroKb',
                width: 90,
            },
            {
                headerName: this.l('Request Datetime'),
                headerTooltip: this.l('Request Datetime'),
                field: 'requestDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.requestDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 90,
            },
            {
                headerName: this.l('Pik Start Datetime'),
                headerTooltip: this.l('Pik Start Datetime'),
                field: 'pikStartDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.pikStartDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Pik Finish Datetime'),
                headerTooltip: this.l('Pik Finish Datetime'),
                field: 'pikFinishDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.pikFinishDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Del Start Datetime'),
                headerTooltip: this.l('Del Start Datetime'),
                field: 'delStartDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.delStartDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Del Finish Datetime'),
                headerTooltip: this.l('Del Finish Datetime'),
                field: 'delFinishDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.delFinishDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                width: 90,
            },
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
			this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			this.partNo,
            this.backNo,
            this.pcAddress,
            this.spsAddress,
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
		this.workingDateFrom = '',
		this.workingDateTo = '',
		this.shift = '',
		this.partNo = '',
		this.backNo = '',
		this.pcAddress = '',
        this.spsAddress = '',
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
			this.shift,
			this.partNo,
			this.backNo,
			this.pcAddress,
            this.spsAddress,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaEkbEkanbanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaEkbEkanbanDto();
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
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getEkanbanToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
			this.partNo,
            this.backNo,
			this.pcAddress,
			this.spsAddress
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }

    
    
}
