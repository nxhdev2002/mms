import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AsyAdoAInPlanDto, AsyAdoAInPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ViewAInPlanDetailsModalComponent } from './view-ainplan-detail-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './ainplan.component.html',
})
export class AinplanComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewAInPlanDetails', { static: true }) viewAInPlanDetails:| ViewAInPlanDetailsModalComponent| undefined;
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

    selectedRow: AsyAdoAInPlanDto = new AsyAdoAInPlanDto();
    saveSelectedRow: AsyAdoAInPlanDto = new AsyAdoAInPlanDto();
    datas: AsyAdoAInPlanDto = new AsyAdoAInPlanDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    model : string = '' ;
	lotNo : string = '' ;
	noInLot : number = 0 ;
	grade : string = '' ;
	bodyNo : string = '' ;
	color : string = '' ;
	aInDateActual : any ;
	aOutDateActual : any ;
	aInPlanDatetime : any ;
	aOutPlanDatetime : any ;
	sequenceNo : string = '' ;
	isStart : number = 0 ;
    cfc: string = '';
    vinNo: string = '';
    engineId: string = '';
    transId: string = '';
    salesSfx: string = '';
    colorType: string = '';
    indentLine: string = '';
    ssNo: string = '';
    edNumber: string = '';
    goshiCar: string = '';
    lineoffDatetime: any;
    lineoffDate: any;
    lineoffTime: any;
    pdiDatetime: any;
    pdiDate: any;
    pdiTime: any;
    assemblyId: any;
    changedRecordsAInPlan: number [] = [];
    selectId;
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
        private _service: AsyAdoAInPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService:DateTimeService
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
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',

            },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'),   field: 'cfc', },
			{
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',

            },
			{
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                type: 'rightAligned',
            },
			{
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
            },
			{
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
            },
			{
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
            },
            {
                headerName: this.l('Vin No'),
                headerTooltip: this.l('Vin No'),
                field: 'vinNo',
            },
			{
                headerName: this.l('A In Date Actual'),
                headerTooltip: this.l('A In Date Actual'),
                field: 'aInDateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.aInDateActual, 'dd/MM/yyyy'),
            },
			{
                headerName: this.l('A Out Date Actual'),
                headerTooltip: this.l('A Out Date Actual'),
                field: 'aOutDateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.aOutDateActual, 'dd/MM/yyyy'),
            },
			{
                headerName: this.l('A In Plan Datetime'),
                headerTooltip: this.l('A In Plan Datetime'),
                field: 'aInPlanDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.aInPlanDatetime, 'dd/MM/yyyy HH:mm:ss'),
            },
			{
                headerName: this.l('A Out Plan Datetime'),
                headerTooltip: this.l('A Out Plan Datetime'),
                field: 'aOutPlanDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.aOutPlanDatetime, 'dd/MM/yyyy HH:mm:ss'),
            },
			{
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'),
                field: 'sequenceNo',
            },
			{
                headerName: this.l('Is Start'),
                headerTooltip: this.l('Is Start'),
                field: 'isStart',
                type: 'rightAligned',
            },
			{
                headerName: this.l('Vehicle Id'),
                headerTooltip: this.l('Vehicle Id'),
                field: 'vehicleId',
                type: 'rightAligned',
            },
			{
                headerName: this.l('Lineoff Datetime'),
                headerTooltip: this.l('Lineoff Datetime'),
                field: 'lineoffDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.lineoffDatetime, 'dd/MM/yyyy HH:mm:ss'),
            },
			{
                headerName: this.l('Lineoff Date'),
                headerTooltip: this.l('Lineoff Date'),
                field: 'lineoffDate',
                valueGetter: (params) => this.pipe.transform(params.data?.lineoffDate, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Lineoff Time'),
                headerTooltip: this.l('Lineoff Time'),
                field: 'lineoffTime',
                // valueGetter: (params) => this._dateTimeService.getTime(params.data?.lineoffTime),
            },
			{
                headerName: this.l('Pdi Datetime'),
                headerTooltip: this.l('Pdi Datetime'),
                field: 'pdiDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.pdiDatetime, 'dd/MM/yyyy HH:mm:ss'),
            },
			{
                headerName: this.l('Pdi Date'),
                headerTooltip: this.l('Pdi Date'),
                field: 'pdiDate',
                valueGetter: (params) => this.pipe.transform(params.data?.pdiDate, 'dd/MM/yyyy'),
            },
			{
                headerName: this.l('Pdi Time'),
                headerTooltip: this.l('Pdi Time'),
                field: 'pdiTime',
            },
            {
                headerName: this.l('PIO Actual Datetime'),
                headerTooltip: this.l('PIOActualDatetime'),
                valueGetter: (params) => this.pipe.transform(params.data?.pioActualDatetime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'pioActualDatetime',
                width: 110
            },
            {
                headerName: this.l('PIO Actual Date'),
                headerTooltip: this.l('PIOActualDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.pioActualDate, 'dd/MM/yyyy'),
                field: 'pioActualDate',
                width: 110
            },
            {
                headerName: this.l('PIO Actual Time'),
                headerTooltip: this.l('pIOActualTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.pdiTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'formatPIOActualTime',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Datetime'),
                headerTooltip: this.l('SalesActualDatetime'),
                valueGetter: (params) => this.pipe.transform(params.data?.salesActualDatetime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'salesActualDatetime',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Date'),
                headerTooltip: this.l('SalesActualDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.salesActualDate, 'dd/MM/yyyy'),
                field: 'salesActualDate',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Time'),
                headerTooltip: this.l('SalesActualTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.pdiTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'formatSalesActualTime',
                width: 110
            },
            {
                headerName: this.l('Assembly Id'),
                headerTooltip: this.l('Assembly Id'),
                field: 'assemblyId',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Engine Id'),
                headerTooltip: this.l('Engine Id'),
                field: 'engineId',
            },
			{
                headerName: this.l('Trans Id'),
                headerTooltip: this.l('Trans Id'),
                field: 'transId',
            },
			{
                headerName: this.l('Sales Sfx'),
                headerTooltip: this.l('Sales Sfx'),
                field: 'salesSfx',
            },
			{
                headerName: this.l('Color Type'),
                headerTooltip: this.l('Color Type'),
                field: 'colorType',
            },
			{
                headerName: this.l('Indent Line'),
                headerTooltip: this.l('Indent Line'),
                field: 'indentLine',
            },
			{
                headerName: this.l('Ss No'),
                headerTooltip: this.l('Ss No'),
                field: 'ssNo',
            },
			{
                headerName: this.l('Ed Number'),
                headerTooltip: this.l('Ed Number'),
                field: 'edNumber',
            },
			{
                headerName: this.l('Goshi Car'),
                headerTooltip: this.l('Goshi Car'),
                field: 'goshiCar',
            },

        ];
		this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsAInPlan = result;
            console.log("result =", result);
        })
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

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
			this.model,
			this.lotNo,
			this.grade,
			this.bodyNo,
			this.sequenceNo,
			this.vinNo,
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
            this.isLoading = false;
        });
    }

    clearTextSearch() {
        this.model = '',
		this.lotNo = '',
		this.grade = '',
		this.bodyNo = '',
        this.sequenceNo = '',
        this.vinNo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.model,
			this.lotNo,
			this.grade,
			this.bodyNo,
			this.sequenceNo,
			this.vinNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => AsyAdoAInPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new AsyAdoAInPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId =this.selectedRow.id;
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getAInPlanToExcel(
			this.model,
			this.lotNo,
			this.grade,
			this.bodyNo,
			this.sequenceNo,
			this.vinNo,
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }
}
