import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockConceptDto, InvGpsStockConceptServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditInvGpsStockConceptModalComponent } from './create-or-edit-invgpsstockconcept-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTime } from 'luxon';
import { ImportInvGpsStockConceptComponent } from './import-invgpsstockconcept-modal';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './invgpsstockconcept.component.html',
})

export class InvGpsStockConceptComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalInvGpsStockConcept', { static: true }) createOrEditModalInvGpsStockConcept:| CreateOrEditInvGpsStockConceptModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportInvGpsStockConceptComponent| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvGpsStockConceptDto = new InvGpsStockConceptDto();
    saveSelectedRow: InvGpsStockConceptDto = new InvGpsStockConceptDto();
    datas: InvGpsStockConceptDto = new InvGpsStockConceptDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    supplierCode : string = '' ;
	stkConcept : string = '' ;
    isActive : string = '' ;
    monthStk : DateTime;
    minStk1 : number;
    minStk2 : number;
    minStk3 : number;
    minStk4 : number;
    minStk5 : number;
    minStk6 : number;
    minStk7 : number;
    minStk8 : number;
    minStk9 : number;
    minStk10 : number;
    minStk11 : number;
    minStk12 : number;
    minStk13 : number;
    minStk14 : number;
    minStk15 : number;
    maxStk1 : number;
    maxStk2 : number;
    maxStk3 : number;
    maxStk4 : number;
    maxStk5 : number;
    minStkConcept : number;
    maxStkConcept : number;
    stkConceptFrq   : number;

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
        private _service: InvGpsStockConceptServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Supplier Code'),headerTooltip: this.l('Supplier Code'),field: 'supplierCode'},
			{headerName: this.l('Month Stk'),headerTooltip: this.l('Month Stk'),field: 'monthStk',valueGetter: (params) => this.pipe.transform(params.data?.monthStk, 'dd/MM/yyyy')},
			{headerName: this.l('Min Stk 1'),headerTooltip: this.l('Min Stk 1'),field: 'minStk1', type: 'rightAligned'},
			{headerName: this.l('Min Stk 2'),headerTooltip: this.l('Min Stk 2'),field: 'minStk2', type: 'rightAligned'},
			{headerName: this.l('Min Stk 3'),headerTooltip: this.l('Min Stk 3'),field: 'minStk3', type: 'rightAligned'},
			{headerName: this.l('Min Stk 4'),headerTooltip: this.l('Min Stk 4'),field: 'minStk4', type: 'rightAligned'},
			{headerName: this.l('Min Stk 5'),headerTooltip: this.l('Min Stk 5'),field: 'minStk5', type: 'rightAligned'},
			{headerName: this.l('Min Stk 6'),headerTooltip: this.l('Min Stk 6'),field: 'minStk6', type: 'rightAligned'},
			{headerName: this.l('Min Stk 7'),headerTooltip: this.l('Min Stk 7'),field: 'minStk7', type: 'rightAligned'},
			{headerName: this.l('Min Stk 8'),headerTooltip: this.l('Min Stk 8'),field: 'minStk8', type: 'rightAligned'},
			{headerName: this.l('Min Stk 9'),headerTooltip: this.l('Min Stk 9'),field: 'minStk9', type: 'rightAligned'},
			{headerName: this.l('Min Stk 10'),headerTooltip: this.l('Min Stk 10'),field: 'minStk10', type: 'rightAligned'},
			{headerName: this.l('Min Stk 11'),headerTooltip: this.l('Min Stk 11'),field: 'minStk11', type: 'rightAligned'},
			{headerName: this.l('Min Stk 12'),headerTooltip: this.l('Min Stk 12'),field: 'minStk12', type: 'rightAligned'},
			{headerName: this.l('Min Stk 13'),headerTooltip: this.l('Min Stk 13'),field: 'minStk13', type: 'rightAligned'},
			{headerName: this.l('Min Stk 14'),headerTooltip: this.l('Min Stk 14'),field: 'minStk14', type: 'rightAligned'},
			{headerName: this.l('Min Stk 15'),headerTooltip: this.l('Min Stk 15'),field: 'minStk15', type: 'rightAligned'},
			{headerName: this.l('Max Stk 1'),headerTooltip: this.l('Max Stk 1'),field: 'maxStk1', type: 'rightAligned'},
			{headerName: this.l('Max Stk 2'),headerTooltip: this.l('Max Stk 2'),field: 'maxStk2', type: 'rightAligned'},
			{headerName: this.l('Max Stk 3'),headerTooltip: this.l('Max Stk 3'),field: 'maxStk3', type: 'rightAligned'},
			{headerName: this.l('Max Stk 4'),headerTooltip: this.l('Max Stk 4'),field: 'maxStk4', type: 'rightAligned'},
			{headerName: this.l('Max Stk 5'),headerTooltip: this.l('Max Stk 5'),field: 'maxStk5', type: 'rightAligned'},
			{headerName: this.l('Min Stk Concept'),headerTooltip: this.l('Min Stk Concept'),field: 'minStkConcept', type: 'rightAligned'},
			{headerName: this.l('Max Stk Concept'),headerTooltip: this.l('Max Stk Concept'),field: 'maxStkConcept', type: 'rightAligned'},
			{headerName: this.l('Stk Concept'),headerTooltip: this.l('Stk Concept'),field: 'stkConcept', type: 'rightAligned'},
			{headerName: this.l('Stk Concept Frq'),headerTooltip: this.l('Stk Concept Frq'),field: 'stkConceptFrq', type: 'rightAligned'},
			// {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //         buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},
            // }
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.supplierCode,
            this.stkConcept,
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
         this.supplierCode = '';
	    this.stkConcept = '';
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
			this.supplierCode,
            this.stkConcept,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsStockConceptDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsStockConceptDto();
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

    deleteRow(system: InvGpsStockConceptDto): void {
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getInvGpsStockConceptToExcel(
            this.supplierCode,
            this.stkConcept
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }


}
