import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerDeliveryGateInDto, InvCkdContainerDeliveryGateInServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './containerdeliverygatein.component.html',
})
export class ContainerDeliveryGateInComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvCkdContainerDeliveryGateInDto = new InvCkdContainerDeliveryGateInDto();
    saveSelectedRow: InvCkdContainerDeliveryGateInDto = new InvCkdContainerDeliveryGateInDto();
    datas: InvCkdContainerDeliveryGateInDto = new InvCkdContainerDeliveryGateInDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    plateNo : string = '' ;
	containerNo : string = '' ;
	renban : string = '' ;
	dateIn;
    dateFrom:any;
    dateTo:any;
	driverName : string = '' ;
	forwarder : string = '' ;
	timeIn
	timeOut
	ckdReqId
	cardNo : string = '' ;
	mobile : string = '' ;
	callTime
	cancelTime
	startTime
	finishTime
	idNo : string = '' ;
	isActive : string = '' ;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
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
        private _service: InvCkdContainerDeliveryGateInServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55},
            {headerName: this.l('Plate No'),headerTooltip: this.l('Plate No'),field: 'plateNo',flex:1},
			{headerName: this.l('Container No'),headerTooltip: this.l('Container No'),field: 'containerNo',flex:1},
			{headerName: this.l('Renban'),headerTooltip: this.l('Renban'),field: 'renban',flex:1},
			{headerName: this.l('Date In'),headerTooltip: this.l('Date In'),field: 'dateIn',valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'), flex: 1},
			{headerName: this.l('Driver Name'),headerTooltip: this.l('Driver Name'),field: 'driverName',flex:1},
			{headerName: this.l('Forwarder'),headerTooltip: this.l('Forwarder'),field: 'forwarder',flex:1},
			{headerName: this.l('Time In'),headerTooltip: this.l('Time In'),field: 'timeIn',flex: 1, type: 'rightAligned'},
			{headerName: this.l('Time Out'),headerTooltip: this.l('Time Out'),field: 'timeOut',flex: 1, type: 'rightAligned'},
			{headerName: this.l('Ckd Req Id'),headerTooltip: this.l('Ckd Req Id'),field: 'ckdReqId',flex:1, type: 'rightAligned'},
			{headerName: this.l('Card No'),headerTooltip: this.l('Card No'),field: 'cardNo',flex:1},
			{headerName: this.l('Mobile'),headerTooltip: this.l('Mobile'),field: 'mobile',flex:1},
			{headerName: this.l('Call Time'),headerTooltip: this.l('Call Time'),field: 'callTime',flex: 1},
			{headerName: this.l('Cancel Time'),headerTooltip: this.l('Cancel Time'),field: 'cancelTime',flex: 1},
			{headerName: this.l('Start Time'),headerTooltip: this.l('Start Time'),field: 'startTime',valueGetter: (params) => this.pipe.transform(params.data?.startTime, 'dd/MM/yyyy HH:mm:ss'),flex:1},
			{headerName: this.l('Finish Time'),headerTooltip: this.l('Finish Time'),field: 'finishTime',valueGetter: (params) => this.pipe.transform(params.data?.finishTime, 'dd/MM/yyyy HH:mm:ss'),flex:1},
			{headerName: this.l('Id No'),headerTooltip: this.l('Id No'),field: 'idNo',flex:1, width: 55},
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
			this.plateNo,
			this.containerNo,
			this.renban,
			this.driverName,
			this.forwarder,
			this.cardNo,
			this.mobile,
			this.callTime,
			this.idNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
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
        this.plateNo = '',
		this.containerNo = '',
		this.renban = '',
        this.dateFrom = '',
        this.dateTo = '',
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
            this.plateNo,
			this.containerNo,
			this.renban,
			this.driverName,
			this.forwarder,
			this.cardNo,
			this.mobile,
			this.callTime,
			this.idNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
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
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerDeliveryGateInDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerDeliveryGateInDto();
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
        this._service.getContainerDeliveryGateInToExcel(
			this.plateNo,
			this.containerNo,
			this.renban,
            this.driverName,
			this.forwarder,
			this.cardNo,
			this.mobile,
			this.idNo,
			this.isActive,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }
}
