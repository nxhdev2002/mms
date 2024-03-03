import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvSupplierListDto, MstInvSupplierListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ViewSupplierListModalComponent } from './view-modal-supplierlist.component';

@Component({
    templateUrl: './supplierlist.component.html',

})
export class SupplierListComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewSupplierListModalComponent', { static: true }) viewSupplierListModalComponent:| ViewSupplierListModalComponent| undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstInvSupplierListDto = new MstInvSupplierListDto();
    saveSelectedRow: MstInvSupplierListDto = new MstInvSupplierListDto();
    datas: MstInvSupplierListDto = new MstInvSupplierListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    	supplierNo : string = '' ;
	supplierName : string = '' ;
remarks : string = '' ;
	supplierType : string = '' ;
	supplierNameVn : string = '' ;
	exporter : string = '' ;

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
        private _service: MstInvSupplierListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo',flex: 1},
			{headerName: this.l('Supplier Name'),headerTooltip: this.l('Supplier Name'),field: 'supplierName',flex: 1},
			{headerName: this.l('Remarks'),headerTooltip: this.l('Remarks'),field: 'remarks',flex: 1},
			{headerName: this.l('Supplier Type'),headerTooltip: this.l('Supplier Type'),field: 'supplierType',flex: 1},
			{headerName: this.l('Supplier Name Vn'),headerTooltip: this.l('Supplier Name Vn'),field: 'supplierNameVn',flex: 1},
			{headerName: this.l('Exporter'),headerTooltip: this.l('Exporter'),field: 'exporter',flex: 1},
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
			this.supplierNo,
			this.supplierName,
			this.remarks,
			this.supplierType,
			this.supplierNameVn,
			this.exporter,
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
        		this.supplierNo = '',
		this.supplierName = '',
this.remarks = '',
		this.supplierType = '',
		this.supplierNameVn = '',
		this.exporter = '',
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
 			this.supplierNo,
			this.supplierName,
			this.remarks,
			this.supplierType,
			this.supplierNameVn,
			this.exporter,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
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
    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvSupplierListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvSupplierListDto();
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

    // deleteRow(system: MstInvSupplierListDto): void {
    //     this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
    //         if (isConfirmed) {
    //             this._service.delete(system.id).subscribe(() => {
    //                 this.callBackDataGrid(this.dataParams!);
    //                 this.notify.success(this.l('SuccessfullyDeleted'));
    //                 this.notify.info(this.l('SuccessfullyDeleted'));
    //             });
    //         }
    //     });
    // }
    exportToExcel(): void {
        this._service.getGpsSupplierListToExcel(
			this.supplierNo,
			this.supplierName,
			this.remarks,
			this.supplierType,
			this.supplierNameVn,
			this.exporter,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }
}
