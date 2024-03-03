import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvCpsSuppliersDto, MstInvCpsSuppliersServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './cpssuppliers.component.html',
})
export class CpsSuppliersComponent extends AppComponentBase implements OnInit {
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

    selectedRow: MstInvCpsSuppliersDto = new MstInvCpsSuppliersDto();
    saveSelectedRow: MstInvCpsSuppliersDto = new MstInvCpsSuppliersDto();
    datas: MstInvCpsSuppliersDto = new MstInvCpsSuppliersDto();
    isLoading = false;
    pending: string = '';
    disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    supplierName: any;
	supplierNumber: any;
	vatregistrationNum: any;
	vatregistrationInvoice: any;
	taxPayerId: any;
	startDateActive: any;
    isActive: any;
    registryId: any;
    endDateActive: any;



    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) {
        return null;
        }
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service: MstInvCpsSuppliersServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            {headerName: this.l('Supplier Name'), headerTooltip: this.l('Supplier Name'), field: 'supplierName', flex: 1},
			{headerName: this.l('Supplier Number'), headerTooltip: this.l('Supplier Number'), field: 'supplierNumber', flex: 1,  type: 'rightAligned'},
			{headerName: this.l('Vatregistration Num'), headerTooltip: this.l('Vatregistration Num'), field: 'vatregistrationNum', flex: 1},
			{headerName: this.l('Vatregistration Invoice'), headerTooltip: this.l('Vatregistration Invoice'), field: 'vatregistrationInvoice', flex: 1},
			{headerName: this.l('Tax Payer Id'), headerTooltip: this.l('Tax Payer Id'), field: 'taxPayerId', flex: 1},
			{headerName: this.l('Registry Id'), headerTooltip: this.l('Registry Id'), field: 'registryId', flex: 1,  type: 'rightAligned',},
			{headerName: this.l('Start Date Active'), headerTooltip: this.l('Start Date Active'), field: 'startDateActive', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.startDateActive, 'dd/MM/yyyy')},
			{headerName: this.l('End Date Active'), headerTooltip: this.l('End Date Active'), field: 'endDateActive', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.endDateActive, 'dd/MM/yyyy')},
			{headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            // eslint-disable-next-line eqeqeq
            buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', }, }
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
			this.supplierName,
			this.vatregistrationInvoice,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
        });
    }

    clearTextSearch() {
        this.supplierName = '' ;
        this.vatregistrationInvoice  = '' ;
        this.searchDatas();
    }


    getDatas(_paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.supplierName,
			this.vatregistrationInvoice,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvCpsSuppliersDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvCpsSuppliersDto();
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);
		this._service.getCpsSuppliersToExcel(
			this.supplierName,
			this.vatregistrationInvoice,
            )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
