import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvTmssDispatchPlanDto, InvTmssDispatchPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './tmssdispatchplan.component.html',
    styleUrls: ['./tmssdispatchplan.component.less'],
})
export class TmssDispatchPlanComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvTmssDispatchPlanDto = new InvTmssDispatchPlanDto();
    saveSelectedRow: InvTmssDispatchPlanDto = new InvTmssDispatchPlanDto();
    datas: InvTmssDispatchPlanDto = new InvTmssDispatchPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    fn: CommonFunction = new CommonFunction();
    vehicleType : string = '' ;
	model : string = '' ;
    marketingCode : string = '' ;
	productionCode : string = '' ;
	dealer : string = '' ;
	vin : string = '' ;
	extColor : string = '' ;
    intColor : string = '' ;
    route: string = '' ;
    changedRecordsInvTmssDispatchPlan: number[] = [];
    selectId;
    dlrDispatchPlanDateFrom
    dlrDispatchPlanDateTo
    dlrDispatchDateFrom
    dlrDispatchDateTo
    dateCurent = new Date();

    pendingReportByDay: string = '';
    pendingReportByMonth: string = ''
    disable: boolean = false;

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
        private _service: InvTmssDispatchPlanServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Vehicle Type'),headerTooltip: this.l('Vehicle Type'),field: 'vehicleType'},
			{headerName: this.l('Model'),headerTooltip: this.l('Model'),field: 'model'},
			{headerName: this.l('Marketing Code'),headerTooltip: this.l('Marketing Code'),field: 'marketingCode'},
			{headerName: this.l('Production Code'),headerTooltip: this.l('Production Code'),field: 'productionCode'},
			{headerName: this.l('Vin'),headerTooltip: this.l('Vin'),field: 'vin'},
            {headerName: this.l('Dealer'),headerTooltip: this.l('Dealer'),field: 'dealer'},
			{headerName: this.l('Ext Color'),headerTooltip: this.l('Ext Color'),field: 'extColor'},
			{headerName: this.l('Int Color'),headerTooltip: this.l('Int Color'),field: 'intColor'},
            {headerName: this.l('Katashiki'),headerTooltip: this.l('Katashiki'),field: 'katashiki'},
			{headerName: this.l('Dlr Dispatch Plan'),headerTooltip: this.l('Dlr Dispatch Plan'),field: 'dlrDispatchPlan',valueFormatter: (params) => this.pipe.transform(params.data?.dlrDispatchPlan, 'dd/MM/yyyy')},
			{headerName: this.l('Dlr Dispatch Date'),headerTooltip: this.l('Dlr Dispatch Date'),field: 'dlrDispatchDate',valueFormatter: (params) => this.pipe.transform(params.data?.dlrDispatchDate, 'dd/MM/yyyy')},
            {headerName: this.l('P Install Date'),headerTooltip: this.l('P Install Date'),field: 'pInstallDate',valueFormatter: (params) => this.pipe.transform(params.data?.pInstallDate, 'dd/MM/yyyy')},
			{headerName: this.l('Install Date'),headerTooltip: this.l('Install Date'),field: 'installDate',valueFormatter: (params) => this.pipe.transform(params.data?.installDate, 'dd/MM/yyyy')},
            {headerName: this.l('Route'),headerTooltip: this.l('Route'),field: 'route'},

        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.dateCurent.setDate(this.dateCurent.getDate());
        this.dlrDispatchPlanDateFrom = this.dateCurent;
        this.fetchChangedRecords();
    }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsInvTmssDispatchPlan = result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        this._service.getAll(
			this.vehicleType,
			this.model,
			this.marketingCode,
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateTo),
			this.vin,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            // this.resetGridView();
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        });
    }

    clearTextSearch() {
        this.vehicleType = '',
		this.model = '',
        this.marketingCode = '',
		this.vin = '',
		this.dlrDispatchDateFrom = '',
        this.dlrDispatchDateTo = '',
		this.dlrDispatchPlanDateFrom = '',
		this.dlrDispatchPlanDateTo = '',
        this.searchDatas();
    }
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.vehicleType,
			this.model,
			this.marketingCode,
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateTo),
			this.vin,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvTmssDispatchPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvTmssDispatchPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            // this.resetGridView();
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
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 100)
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getTmssDispatchPlanToExcel(
            this.vehicleType,
            this.model,
            this.marketingCode,
            this.vin,
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateFrom),
            this._dateTimeService.convertToDatetime(this.dlrDispatchDateTo),
            )
            .subscribe((result) => {
                console.log(result);

                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportReportByDay(): void {

        if(this.dlrDispatchPlanDateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if(this.dlrDispatchPlanDateTo == undefined) {
            this.dlrDispatchPlanDateTo = this.dateCurent;
           
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByDay = 'pending';
        this.disable = true;
		this._service.getExportReportDaily(
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByDay = '';
                this.disable = false;
			});
	}

    exportReportMarketingCodeByMonth(){
        if(this.dlrDispatchPlanDateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if(this.dlrDispatchPlanDateTo == undefined) {
            this.dlrDispatchPlanDateTo = this.dateCurent;
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByMonth = 'pending';
        this.disable = true;
		this._service.getExportReportMarketingCodeMonthly(
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByMonth = '';
                this.disable = false;
			});
    }

    exportReportExtColorByDay(){
        if(this.dlrDispatchPlanDateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if(this.dlrDispatchPlanDateTo == undefined) {
            this.dlrDispatchPlanDateTo = this.dateCurent;
           
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByDay = 'pending';
        this.disable = true;
		this._service.getExportReportColorDaily(
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByDay = '';
                this.disable = false;
			});
    }

    exportReportColorByMonth(){
        if(this.dlrDispatchPlanDateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if(this.dlrDispatchPlanDateTo == undefined) {
            this.dlrDispatchPlanDateTo = this.dateCurent;
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dlrDispatchPlanDateTo);
        if(_dateTo < _dateFrom){
            this.notify.warn("Date From < Date To!");
            return;
        }


        this.pendingReportByMonth = 'pending';
        this.disable = true;
		this._service.getExportReportColorMonthly(
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateFrom),
			this._dateTimeService.convertToDatetime(this.dlrDispatchPlanDateTo),
		)
			.subscribe((result) => {
				this._fileDownloadService.downloadTempFile(result);
                this.pendingReportByMonth = '';
                this.disable = false;
			});
    }
}
