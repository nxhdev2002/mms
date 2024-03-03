import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvCpsSuppliersDto, MstLspSupplierInforDto, MstLspSupplierInforServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
// import { CreateOrEditCpsSuppliersModalComponent } from './create-or-edit-cpssuppliers-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './lspsupplierinfo.component.html',
    styleUrls: ['./lspsupplierinfo.component.less'],
})
export class LspSupplierInfoComponent extends AppComponentBase implements OnInit {
   // @ViewChild('createOrEditModalCpsSuppliers', { static: true }) createOrEditModalCpsSuppliers:| CreateOrEditCpsSuppliersModalComponent| undefined;
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

    selectedRow: MstLspSupplierInforDto = new MstLspSupplierInforDto();
    saveSelectedRow: MstLspSupplierInforDto = new MstLspSupplierInforDto();
    datas: MstLspSupplierInforDto = new MstLspSupplierInforDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    supplierCode : string = '' ; 
	supplierPlantCode : string = '' ; 
	supplierName : string = '' ; 
	address : string = '' ; 
	dockX : string = '' ; 
	dockXAddress : string = '' ; 
    deliveryMethod : string = '' ; 
    deliveryFrequency : string = '' ; 
    cd ; 
    orderDateType ; 
    keihenType ; 
    stkConceptTmvMin; 
    stkConceptTmvMax ; 
    stkConceptSupMMin; 
    stkConceptSupMMax ; 
    stkConceptSupPMin ; 
    stkConceptSupPMax; 
    tmvProductPercentage;
    picMainId;
    deliveryLt;
    productionShift;
    tcFrom;
    tcTo;
    orderTrip;
    supplierNameEn;
    isActive;
    createdBy;
    updatedDate;
    updatedBy;

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
        private _service: MstLspSupplierInforServiceProxy,
        private gridTableService: GridTableService,
       private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Id'),headerTooltip: this.l('Id'),field: 'id',flex: 1, hide:true},
            {headerName: this.l('Supplier Code'),headerTooltip: this.l('Supplier Code'),field: 'supplierCode',flex: 1},
			{headerName: this.l('Supplier Plant Code'),headerTooltip: this.l('Supplier Plant Code'),field: 'supplierPlantCode',flex: 1},
			{headerName: this.l('Supplier Name'),headerTooltip: this.l('Supplier Name'),field: 'supplierName',flex: 1},
			{headerName: this.l('Address'),headerTooltip: this.l('Address'),field: 'address',flex: 1},
			{headerName: this.l('Dock X'),headerTooltip: this.l('Dock X'),field: 'dockX',flex: 1},
			{headerName: this.l('Dock X Address'),headerTooltip: this.l('Dock X Address'),field: 'dockXAddress',flex: 1},
			{headerName: this.l('Delivery Method'),headerTooltip: this.l('Delivery Method'),field: 'deliveryMethod',flex: 1},
			{headerName: this.l('Delivery Frequency'),headerTooltip: this.l('Delivery Frequency'),field: 'deliveryFrequency',flex: 1},
			{headerName: this.l('Cd'),headerTooltip: this.l('Cd'),field: 'cd',flex: 1},
            {headerName: this.l('Order Date Type'),headerTooltip: this.l('Order Date Type'),field: 'orderDateType',flex: 1},
            {headerName: this.l('Keihen Type'),headerTooltip: this.l('Keihen Type'),field: 'keihenType',flex: 1},
            {headerName: this.l('Stk Concept Tmv Min'),headerTooltip: this.l('Stk Concept Tmv Min'),field: 'stkConceptTmvMin',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Stk Concept Tmv Max'),headerTooltip: this.l('Stk Concept Tmv Max'),field: 'stkConceptTmvMax',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Stk Concept Sup M Min'),headerTooltip: this.l('Stk Concept Sup M Min'),field: 'stkConceptSupMMin',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Stk Concept Sup M Max'),headerTooltip: this.l('Stk Concept Sup M Max'),field: 'stkConceptSupMMax',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Stk Concept Sup P Min'),headerTooltip: this.l('Stk Concept Sup P Min'),field: 'stkConceptSupPMin',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Stk Concept Sup P Max'),headerTooltip: this.l('Stk Concept Sup P Max'),field: 'stkConceptSupPMax',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Tmv Product Percentage'),headerTooltip: this.l('Tmv Product Percentage'),field: 'tmvProductPercentage',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Pic Main Id'),headerTooltip: this.l('Pic Main Id'),field: 'picMainId',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Delivery Lt'),headerTooltip: this.l('Delivery Lt'),field: 'deliveryLt',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Production Shift'),headerTooltip: this.l('Production Shift'),field: 'productionShift',flex: 1},
            {headerName: this.l('Tc From'),headerTooltip: this.l('Tc From'),field: 'tcFrom',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.tcFrom, 'dd/MM/yyyy')},
            {headerName: this.l('Tc To'),headerTooltip: this.l('Tc To'),field: 'tcTo',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.tcTo, 'dd/MM/yyyy')},
            {headerName: this.l('Order Trip'),headerTooltip: this.l('Order Trip'),field: 'orderTrip',flex: 1, type: 'rightAligned'},
            {headerName: this.l('Supplier Name En'),headerTooltip: this.l('Supplier Name En'),field: 'supplierNameEn',flex: 1},
            // {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',buttonDefTwo: { text: params => (params.data?.'isActive' == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.'isActive' == 'Y') ? 'btnActive' : 'btnInActive',},}      	  
            {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field: 'isActive',flex: 1},
            {headerName: this.l('Created By'),headerTooltip: this.l('Created By'),field: 'createdBy',flex: 1},
            {headerName: this.l('Updated Date'),headerTooltip: this.l('Updated Date'),field: 'updatedDate',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.updatedDate, 'dd/MM/yyyy')},
            {headerName: this.l('Updated By'),headerTooltip: this.l('Updated By'),field: 'updatedBy',flex: 1},
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 50, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.supplierCode,
			this.deliveryMethod,
			this.deliveryFrequency,
			this.orderDateType,
			this.keihenType,

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
        this.supplierCode = '' ; 
        this.deliveryMethod  = '' ; 
        this.deliveryFrequency  = '' ; 
        this.orderDateType  = '' ; 
        this.keihenType = '' ; 

        this.searchDatas();
    }

    // onSelectionMulti(params) {
    //     var selectedRows = this.gridApi.getSelectedRows();
    //     var selectedRowsString = '';
    //     var maxToShow = 5;
    //     selectedRows.forEach(function (selectedRow, index) {
    //         if (index >= maxToShow) {
    //             return;
    //         }
    //         if (index > 0) {
    //             selectedRowsString += ', ';
    //         }
    //         selectedRowsString += selectedRow.athlete;
    //     });
    //     if (selectedRows.length > maxToShow) {
    //         var othersCount = selectedRows.length - maxToShow;
    //         selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
    //     }
    //     (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    // }
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
			this.supplierCode,
			this.supplierPlantCode,
			this.supplierName,
			this.address,
			this.dockX,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLspSupplierInforDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLspSupplierInforDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        console.log(this.selectedRow)
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
            this.resetGridView()
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

    deleteRow(system: MstLspSupplierInforDto): void {
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
        this._service.getMstLspSupplierInfoToExcel(
			this.supplierCode,
            this.deliveryMethod,
            this.deliveryFrequency,
            this.orderDateType,
            this.keihenType
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }
}
