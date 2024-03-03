import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPhysicalStockPartPeriodDto, InvCkdPhysicalStockPartPeriodServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPhysicalStockPartPeriodModalComponent } from './create-or-edit-physicalstockpartperiod-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './physicalstockpartperiod.component.html',
    styleUrls: ['./physicalstockpartperiod.component.less'],
})
export class PhysicalStockPartPeriodComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPhysicalStockPartPeriod', { static: true }) createOrEditModalPhysicalStockPartPeriod:| CreateOrEditPhysicalStockPartPeriodModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPhysicalStockPartPeriodDto = new InvCkdPhysicalStockPartPeriodDto();
    saveSelectedRow: InvCkdPhysicalStockPartPeriodDto = new InvCkdPhysicalStockPartPeriodDto();
    datas: InvCkdPhysicalStockPartPeriodDto = new InvCkdPhysicalStockPartPeriodDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    	partNo : string = '' ; 
	partNoNormalized : string = '' ; 
	partName : string = '' ; 
	partNoNormalizedS4 : string = '' ; 
	colorSfx : string = '' ; 
	lotNo : string = '' ; 
	partListId
	materialId
    beginQty
    receiveQty
    issueQty
    calculatorQty
    actualQty
	qty
	workingDate
	periodId
	lastCalDatetime
    transtype
    remark
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
        private _service: InvCkdPhysicalStockPartPeriodServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo',flex: 1},
			{headerName: this.l('Part No Normalized'),headerTooltip: this.l('Part No Normalized'),field: 'partNoNormalized',flex: 1},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName',flex: 1},
			{headerName: this.l('Part No Normalized S4'),headerTooltip: this.l('Part No Normalized S4'),field: 'partNoNormalizedS4',flex: 1},
			{headerName: this.l('Color Sfx'),headerTooltip: this.l('Color Sfx'),field: 'colorSfx',flex: 1},
			{headerName: this.l('Lot No'),headerTooltip: this.l('Lot No'),field: 'lotNo',flex: 1},
			{headerName: this.l('Part List Id'),headerTooltip: this.l('Part List Id'),field: 'partListId',flex: 1},
			{headerName: this.l('Material Id'),headerTooltip: this.l('Material Id'),field: 'materialId',flex: 1},
			{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1},
			{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',flex: 1},
			{headerName: this.l('Period Id'),headerTooltip: this.l('Period Id'),field: 'periodId',flex: 1},
			{headerName: this.l('Last Cal Datetime'),headerTooltip: this.l('Last Cal Datetime'),field: 'lastCalDatetime',flex: 1},
			{headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},}      	  
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 20, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.partNo,
			this.partNoNormalized,
			this.partName,
			this.partNoNormalizedS4,
			this.colorSfx,
			this.lotNo,
            '',
			// this.partListId,
			// this.materialId,
			// this.qty,
			// this.workingDate,
			// this.periodId,
			// this.lastCalDatetime,
			this.isActive,
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
		this.partNoNormalized = '',
		this.partName = '',
		this.partNoNormalizedS4 = '',
		this.colorSfx = '',
		this.lotNo = '',
		this.partListId = '',
		this.materialId = '',
		this.qty = '',
		this.workingDate = '',
		this.periodId = '',
		this.lastCalDatetime = '',
		this.isActive = '',
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
			this.partNoNormalized,
			this.partName,
			this.partNoNormalizedS4,
			this.colorSfx,
			this.lotNo,
            '',
			// this.partListId,
			// this.materialId,
			// this.qty,
			// this.workingDate,
			// this.periodId,
			// this.lastCalDatetime,
			this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPhysicalStockPartPeriodDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPhysicalStockPartPeriodDto();
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

    deleteRow(system: InvCkdPhysicalStockPartPeriodDto): void {
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
        this._service.getPhysicalStockPartPeriodToExcel(
			this.partNo,
			this.partNoNormalized,
			this.partName,
			this.partNoNormalizedS4,
			this.colorSfx,
			this.lotNo,
			this.partListId,
			this.materialId,
            this.beginQty,
            this.receiveQty,
            this.issueQty,
            this.calculatorQty,
            this.actualQty,
			// this.qty,
			// this.workingDate,
			this.periodId,
			this.lastCalDatetime,
            this.transtype,
            this.remark,
			this.isActive,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }
}