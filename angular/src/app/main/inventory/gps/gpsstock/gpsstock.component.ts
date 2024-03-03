import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockDto, InvGpsStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ViewGPSStockPartDetailModalComponent } from './view-gpsstockpart-detail-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './gpsstock.component.html',
})
export class GpsStockComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewGpsStockPartDetailModal', { static: true }) viewGpsStockPartDetailModal:| ViewGPSStockPartDetailModalComponent| undefined;
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

    selectedRow: InvGpsStockDto = new InvGpsStockDto();
    saveSelectedRow: InvGpsStockDto = new InvGpsStockDto();
    datas: InvGpsStockDto = new InvGpsStockDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    pending: string = '';
    disable: boolean = false;
    partNo : string = '' ;
	partName : string = '' ;
	supplierNo : string = '' ;
	vinNo : string = '' ;
	cfc : string = '' ;
	lotNo : string = '' ;
	noInLot;
	partId
	qty
	workingDate
    workingDateFrom
    workingDateTo
	transactionId

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
        private _service: InvGpsStockServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo',flex: 1},
            {headerName: this.l('Cfc'),headerTooltip: this.l('Cfc'),field: 'cfc',flex: 1},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo',flex: 1},
            {headerName: this.l('Uom'),headerTooltip: this.l('Uom'),field: 'uom',flex: 1},
            {headerName: this.l('Lot No'),headerTooltip: this.l('Lot No'),field: 'lotNo',flex: 1},
			{headerName: this.l('No In Lot'),headerTooltip: this.l('No In Lot'),field: 'noInLot',flex: 1},
			{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1},
			{headerName: this.l('Vin No'),headerTooltip: this.l('Vin No'),field: 'vinNo',flex: 1},
			{headerName: this.l('Body No'),headerTooltip: this.l('Body No'),field: 'bodyNo',flex: 1},
			{headerName: this.l('Color'),headerTooltip: this.l('Color'),field: 'color',flex: 1},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName',flex: 1},
			{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',flex: 1},
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
			this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom ),
            this._dateTimeService.convertToDatetime(this.workingDateTo ),
            this.supplierNo ,
            this.vinNo,
            this.cfc,
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

    clearTextSearch() {
        this.partNo = '',
        this.workingDateFrom = null;
        this.workingDateTo = null;
        this.supplierNo = '',
        this.vinNo = '',
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
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom ),
            this._dateTimeService.convertToDatetime(this.workingDateTo ),
            this.supplierNo ,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsStockDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsStockDto();
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getGpsStockToExcel(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom ),
            this._dateTimeService.convertToDatetime(this.workingDateTo ),
            this.supplierNo,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    getViewDataDetail(){
        this.viewGpsStockPartDetailModal.show(this.selectedRow);
    }
}
