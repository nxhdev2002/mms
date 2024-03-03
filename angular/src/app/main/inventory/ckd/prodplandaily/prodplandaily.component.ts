import { GridApi, GridReadyEvent, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { InvCkdProdPlanDailyDto, InvCkdProdPlanDailyServiceProxy } from '@shared/service-proxies/service-proxies';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
	templateUrl: './ProdPlanDaily.component.html',
    styleUrls: ['./ProdPlanDaily.component.less'],
})
export class ProdPlanDailyComponent extends AppComponentBase implements OnInit {
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

	selectedRow: InvCkdProdPlanDailyDto = new InvCkdProdPlanDailyDto();
	saveSelectedRow: InvCkdProdPlanDailyDto = new InvCkdProdPlanDailyDto();
	datas: InvCkdProdPlanDailyDto = new InvCkdProdPlanDailyDto();
	isLoading: boolean = false;
    pendingReportByDay: string = '';
    pendingReportByMonth: string = '';
    disable: boolean = false;

	dataParams: GridParams | undefined;
	rowData: any[] = [];
	gridApi!: GridApi;
	rowSelection = 'multiple';
	filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

	// no
	bodyNo: string = '';
	lotNo: string = '';
	noInLot: any;
	selectId;
	changedRecordsProdPlanDaily: number[] =[]
    _selectDate = -1;
    listDate = [
        { value: -1, label: "" },
        { value: 1, label: "Filter by W In" },
        { value: 2, label: "Filter by W Out" },
        { value: 3, label: "Filter by T In" },
        { value: 4, label: "Filter by T Out" },
        { value: 5, label: "Filter by A In" },
        { value: 6, label: "Filter by A Out" },
        { value: 7, label: "Filter by Lineoff" },
        { value: 8, label: "Filter by Pdi Date" }
    ];

    vin: string = '';
    isPdiDate: string = '';
    cfc;
    dateFrom: any;
	dateTo: any;
	date = new Date();

    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters


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
		private _service: InvCkdProdPlanDailyServiceProxy,
		private gridTableService: GridTableService,
		private _fileDownloadService: FileDownloadService,
		private _dateTimeService: DateTimeService,
		private _fm: DataFormatService,
	) {
		super(injector);
		this.defaultColDefs = [
			{ headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
			{ headerName: this.l('No'), headerTooltip: this.l('No'), field: 'no' ,type: 'rightAligned'
            , enablePivot: true, enableRowGroup: true, enableValue: true, },
			{ headerName: this.l('Prod Line'), headerTooltip: this.l('Prodline'), field: 'prodline'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Model'), headerTooltip: this.l('Model'), field: 'model'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Color'), headerTooltip: this.l('Color'), field: 'color'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Body No'), headerTooltip: this.l('Bodyno'), field: 'bodyNo'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Lot No'), headerTooltip: this.l('Lotno'), field: 'lotNo'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('No In Lot'), headerTooltip: this.l('Noinlot'), field: 'noInLot',type: 'rightAligned'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Vin No'), headerTooltip: this.l('Vinno'), field: 'vinNo'
            , enablePivot: true, enableRowGroup: true, enableValue: true,},
			{
				headerName: this.l('W In Plan DateTime'), headerTooltip: this.l('Winplandatetime'), field: 'wInPlanDateTime_DDMMYYYY_HHMMSS'
                , enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.wInPlanDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
			{
				headerName: this.l('W Out Plan DateTime'), headerTooltip: this.l('Woutplandatetime'), field: 'wOutPlanDateTime_DDMMYYYY_HHMMSS'
                , enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.wOutPlanDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
			{
				headerName: this.l('T In Plan DateTime'), headerTooltip: this.l('Tinplandatetime'), field: 'tInPlanDatetime_DDMMYYYY_HHMMSS'
                , enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.tInPlanDatetime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
			{
				headerName: this.l('T Out Plan DateTime'), headerTooltip: this.l('Toutplandatetime'), field: 'tOutPlanDatetime_DDMMYYYY_HHMMSS'
                , enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.tOutPlanDatetime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
			{
				headerName: this.l('A In Plan DateTime'), headerTooltip: this.l('Ainplandatetime'), field: 'aInPlanDateTime_DDMMYYYY_HHMMSS',
                enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.aInPlanDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
			{
				headerName: this.l('A Out Plan DateTime'), headerTooltip: this.l('Aoutplandatetime'), field: 'aOutPlanDateTime_DDMMYYYY_HHMMSS',
                enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.aOutPlanDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
            {
				headerName: this.l('Line off DateTime'), headerTooltip: this.l('LineOffDatetime'), field: 'lineoffDateTime_DDMMYYYY_HHMMSS',
                enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.lineoffDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},
            {
				headerName: this.l('Pdi DateTime'), headerTooltip: this.l('PdiDatetime'), field: 'pdiDateTime_DDMMYYYY_HHMMSS',
                enablePivot: true, enableRowGroup: true, enableValue: true,
				cellRenderer: (params) => params.data?.pdiDateTime_DDMMYYYY_HHMMSS,
				aggFunc: this.SumA,
			},

			{ headerName: this.l('Is Project'), headerTooltip: this.l('Isproject'), field: 'isproject' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Engine Id'), headerTooltip: this.l('Engineid'), field: 'engineid' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Vehicle Id'), headerTooltip: this.l('Vehicleid'), field: 'vehicleid',
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Trans Id'), headerTooltip: this.l('Transid'), field: 'transid',type: 'rightAligned' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Sales Sfx'), headerTooltip: this.l('Salessfx'), field: 'salessfx' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Color Type'), headerTooltip: this.l('Colortype'), field: 'colortype' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Indent Line'), headerTooltip: this.l('Indentline'), field: 'indentline',
            enablePivot: true, enableRowGroup: true, enableValue: true, },
			{ headerName: this.l('Ssno'), headerTooltip: this.l('Ssno'), field: 'ssno' ,type: 'rightAligned',
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Ed Number'), headerTooltip: this.l('EdNumber'), field: 'edNumber' ,
            enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Goshicar'), headerTooltip: this.l('Goshicar'), field: 'goshicar',
            enablePivot: true, enableRowGroup: true, enableValue: true,},


		];
		this.frameworkComponents = {

			agCellButtonComponent: AgCellButtonRendererComponent,
		};
	}

	ngOnInit(): void {
		this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
		this.date.setDate(this.date.getDate());
		this.dateFrom = this.date;
		this.fetchChangedRecords();
	}
	fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsProdPlanDaily = result;
            console.log("result =", result);
        })
    }
	searchDatas(): void {
		this.isLoading = true;
		this._service.getAll(
			this.vin,
			this.lotNo,
			this.noInLot,
			this.cfc,
			this.bodyNo,
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate,
            this._selectDate,
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

				var rows = this.createRow();
				this.dataParams!.api.setPinnedBottomRowData(rows);
				this.isLoading = false;
			});
	}

	clearTextSearch() {
		this.vin = '';
			this.lotNo = '';
			this.noInLot = '';
			this.cfc = '';
			this.bodyNo = '';
			this.dateTo = null;
            this._selectDate = -1;
			this.searchDatas();
	}

	getDatas(paginationParams?: PaginationParamsModel) {
		return this._service.getAll(
			this.vin,
			this.lotNo,
			this.noInLot,
			this.cfc,
			this.bodyNo,
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate,
            this._selectDate,
			'',
			this.paginationParams.skipCount,
			this.paginationParams.pageSize
		);
	}

	onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdProdPlanDailyDto[] } }) {
		this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdProdPlanDailyDto();
		this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;

		// var rows = this.createRow();
		// this.dataParams!.api.setPinnedBottomRowData(rows);
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

			// var rows = this.createRow();
			// this.dataParams!.api.setPinnedBottomRowData(rows);
			this.resetGridView();
		});
	}
	autoSize() {
		const allColumnIds: string[] = [];
		this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
			allColumnIds.push(column.getId());
		});
		this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
	}

	resetGridView() {

		setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
			this.autoSize();
		}, 500)
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

				var rows = this.createRow();
				this.dataParams!.api.setPinnedBottomRowData(rows);
				this.isLoading = false;
				this.resetGridView();
			});
	}

	exportToExcel(e): void {
        this.fn.exportLoading(e, true);
		this._service.getProdPlanDailyToExcel(
			this.vin,
			this.lotNo,
			this.noInLot,
			this.cfc,
			this.bodyNo,
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate,
            this._selectDate,
		)
			.subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
			});
	}

	SumA(values) {
		var sum = 0;
		if (values) {
			values.forEach(function (value) { sum += parseFloat(value); });
		}
		return sum;
	}

	createRow(): any[] {
        let result: any[] = [];

        var grandWin = 0;
        var grandWout = 0;
        var grandAin = 0;
        var grandAout = 0;
        var grandTin = 0;
        var grandTout = 0;
        var grandPdi = 0;
        var grandLineOff = 0;

        if (this.rowData.length>0) {

            grandWin = this.rowData[0].countWinPlan;
            grandWout = this.rowData[0].countWoutPlan;
            grandTin = this.rowData[0].countTinPlan;
            grandTout = this.rowData[0].countToutPlan;
            grandAin = this.rowData[0].countAinPlan;
            grandAout = this.rowData[0].countAoutPlan;
            grandPdi = this.rowData[0].countPdiDate;
            grandLineOff = this.rowData[0].countLineoffdate;
        }

		// console.log(grandWin);
		// console.log(grandWout);
		// for (var i = 0; i < count; i++) {
        result.push({
            partNoNormalizedS4: 'Grand Total',
            wInPlanDateTime_DDMMYYYY_HHMMSS: grandWin,
            wOutPlanDateTime_DDMMYYYY_HHMMSS: grandWout,
            tInPlanDatetime_DDMMYYYY_HHMMSS: grandTin,
            tOutPlanDatetime_DDMMYYYY_HHMMSS: grandTout,
            aInPlanDateTime_DDMMYYYY_HHMMSS: grandAin,
            aOutPlanDateTime_DDMMYYYY_HHMMSS: grandAout,
            lineoffDateTime_DDMMYYYY_HHMMSS: grandLineOff,
            pdiDateTime_DDMMYYYY_HHMMSS: grandPdi
        });
		// }
		return result;
	}

    exportReportByDay(): void {

        if(this.dateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if(this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByDay = 'pending';
        this.disable = true;
		this._service.getExportReportDaily(
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByDay = '';
                this.disable = false;
			});
	}

    exportReportByMonth(): void {

        if(this.dateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if(this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByMonth = 'pending';
        this.disable = true;
		this._service.getExportReportMonthly(
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByMonth = '';
                this.disable = false;
			});
	}



    exportReportColorByDay(): void {

        if(this.dateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if(this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }



        this.disable = true;
		this._service.getExportReportColorDaily(
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);

                this.disable = false;
			});
	}

    exportReportColorByMonth(): void {

        if(this.dateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if(this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.disable = true;
		this._service.getExportReportColorMonthly(
			this._dateTimeService.convertToDatetime(this.dateFrom),
			this._dateTimeService.convertToDatetime(this.dateTo),
            this.isPdiDate
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);

                this.disable = false;
			});
	}


    searchPdiDate(i: number) {

        let _btn = document.querySelector('.actionButton_w'+i);
        if (_btn.classList.contains('active')){
            _btn.classList.remove('active');

            this.isPdiDate = '';
            this.searchDatas();

        }else {
            _btn.classList.add('active');
            this.isPdiDate = 'Y',
            this.searchDatas();
        }

    }

    prodSumaryOption(){
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if(optionSummary.classList.contains('active'))  optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcus() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if(optionSummary.classList.contains('active'))  optionSummary.classList.remove("active");
    }

}
