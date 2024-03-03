import { GridApi, GridReadyEvent, MasterDetailModule, ModuleRegistry } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvGpsSupplierInfoDto, MstInvGpsSupplierInfoServiceProxy, MstInvGpsSupplierOrderTimeDto, MstInvGpsSupplierOrderTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditGpsSupplierInfoModalComponent } from './create-or-edit-gpssupplierinfo-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CreateOrEditGpsSupplierOrderTimeModalComponent } from './create-or-edit-gpssupplierordertime-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
ModuleRegistry.registerModules([MasterDetailModule]);

@Component({
    templateUrl: './gpssupplierinfo.component.html',
})
export class GpsSupplierInfoComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalGpsSupplierInfo', { static: true }) createOrEditModalGpsSupplierInfo:| CreateOrEditGpsSupplierInfoModalComponent| undefined;
    @ViewChild('createOrEditModalGpsSupplierOrderTime', { static: true }) createOrEditModalGpsSupplierOrderTime:| CreateOrEditGpsSupplierOrderTimeModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParamsInfo: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsOrderTime: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    infoColDefs:any;
    orderTimeColDefs:any;

    selectedRow: MstInvGpsSupplierInfoDto = new MstInvGpsSupplierInfoDto();
    saveSelectedRow: MstInvGpsSupplierInfoDto = new MstInvGpsSupplierInfoDto();
    selectedRowOrderTime: MstInvGpsSupplierOrderTimeDto = new MstInvGpsSupplierOrderTimeDto();
    saveSelectedRowOrderTime: MstInvGpsSupplierOrderTimeDto = new MstInvGpsSupplierOrderTimeDto();
    datas: MstInvGpsSupplierInfoDto = new MstInvGpsSupplierInfoDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    pending1: string = '';
    disable1: boolean = false;

    dataParams: GridParams | undefined;
    rowDataInfo: any[] = [];
    rowDataOrderTime: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    supplierCode : string = '' ;
	supplierName : string = '' ;
	address : string = '' ;
	dockX : string = '' ;
	dockXAddress : string = '' ;
	deliveryMethod : string = '' ;
	deliveryFrequency : string = '' ;
	cd : string = '' ;
	orderDateType : string = '' ;
	keihenType : any;
	productionShift : any ;
	supplierNameEn : string = '' ;
    supplierPlantCode
    deliveryLt
    orderTrip
    stkConceptTmvMin
    stkConceptSupPMax
    stkConceptSupMMin
    stkConceptTmvMax
    stkConceptSupPMin
    stkConceptSupMMax
    tmvProductPercentage
    picMainId
    _tcFrom
    _tcTo

    supplierId:number;
    orderSeq:number;
    orderType:string='';
    orderTime:any;
    receivingDay:number;
    receiveTime:any;
    keihenDay:number;
    keihenTime:any;
    isActive:string='';

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
        private _service: MstInvGpsSupplierInfoServiceProxy,
        private _serviceOrderTime: MstInvGpsSupplierOrderTimeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,

    ) {
        super(injector);
        this.infoColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParamsInfo.pageSize * (this.paginationParamsInfo.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Supplier Code'),headerTooltip: this.l('Supplier Code'),field: 'supplierCode',flex: 1},
			{headerName: this.l('Supplier Plant Code'),headerTooltip: this.l('Supplier Plant Code'),field: 'supplierPlantCode', flex: 1},
			{headerName: this.l('Supplier Name'),headerTooltip: this.l('Supplier Name'),field: 'supplierName',flex: 1},
			{headerName: this.l('Address'),headerTooltip: this.l('Address'),field: 'address',flex: 1},
			{headerName: this.l('Dock X'),headerTooltip: this.l('Dock X'),field: 'dockX',flex: 1},
			{headerName: this.l('Dock X Address'),headerTooltip: this.l('Dock X Address'),field: 'dockXAddress',flex: 1},
			{headerName: this.l('Delivery Method'),headerTooltip: this.l('Delivery Method'),field: 'deliveryMethod',flex: 1},
			{headerName: this.l('Delivery Frequency'),headerTooltip: this.l('Delivery Frequency'),field: 'deliveryFrequency',flex: 1},
			{headerName: this.l('Cd'),headerTooltip: this.l('Cd'),field: 'cd',flex: 1},
			{headerName: this.l('Order Date Type'),headerTooltip: this.l('Order Date Type'),field: 'orderDateType',flex: 1},
			{headerName: this.l('Keihen Type'),headerTooltip: this.l('Keihen Type'),field: 'keihenType',flex: 1},
			{headerName: this.l('Stk Concept Tmv Min'),headerTooltip: this.l('Stk Concept Tmv Min'),field: 'stkConceptTmvMin',flex: 1},
			{headerName: this.l('Stk Concept Tmv Max'),headerTooltip: this.l('Stk Concept Tmv Max'),field: 'stkConceptTmvMax',flex: 1},
			{headerName: this.l('Stk Concept Sup M Min'),headerTooltip: this.l('Stk Concept Sup M Min'),field: 'stkConceptSupMMin',flex: 1},
			{headerName: this.l('Stk Concept Sup M Max'),headerTooltip: this.l('Stk Concept Sup M Max'),field: 'stkConceptSupMMax',flex: 1},
			{headerName: this.l('Stk Concept Sup P Min'),headerTooltip: this.l('Stk Concept Sup P Min'),field: 'stkConceptSupPMin',flex: 1},
			{headerName: this.l('Stk Concept Sup P Max'),headerTooltip: this.l('Stk Concept Sup P Max'),field: 'stkConceptSupPMax',flex: 1},
			{headerName: this.l('Tmv Product Percentage'),headerTooltip: this.l('Tmv Product Percentage'),field: 'tmvProductPercentage',flex: 1},
			{headerName: this.l('Pic Main Id'),headerTooltip: this.l('Pic Main Id'),field: 'picMainId',flex: 1},
			{headerName: this.l('Delivery Lt'),headerTooltip: this.l('Delivery Lt'),field: 'deliveryLt',flex: 1},
			{headerName: this.l('Production Shift'),headerTooltip: this.l('Production Shift'),field: 'productionShift',flex: 1},
			{headerName: this.l('Tc From'),headerTooltip: this.l('Tc From'),field: 'tcFrom',flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.tcFrom, 'dd/MM/yyyy')},
			{headerName: this.l('Tc To'),headerTooltip: this.l('Tc To'),field: 'tcTo',flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.tcTo, 'dd/MM/yyyy')},
			{headerName: this.l('Order Trip'),headerTooltip: this.l('Order Trip'),field: 'orderTrip',flex: 1},
			{headerName: this.l('Supplier Name En'),headerTooltip: this.l('Supplier Name En'),field: 'supplierNameEn',flex: 1},
			{headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},}
        ];
        this.orderTimeColDefs = [
            {headerName: this.l('Supplier Id'),headerTooltip: this.l('Supplier Id'),field: 'supplierId',flex: 1},
			{headerName: this.l('Order Seq'),headerTooltip: this.l('Order Seq'),field: 'orderSeq',flex: 1},
			{headerName: this.l('Order Type'),headerTooltip: this.l('Order Type'),field:  'orderType', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.orderType == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.orderType == 'Y') ? 'btnActive' : 'btnInActive',},},
			{headerName: this.l('Order Time'),headerTooltip: this.l('Order Time'),field: 'orderTime',flex: 1},
			{headerName: this.l('Receiving Day'),headerTooltip: this.l('Receiving Day'),field: 'receivingDay',flex: 1},
			{headerName: this.l('Receive Time'),headerTooltip: this.l('Receive Time'),field: 'receiveTime',flex: 1},
			{headerName: this.l('Keihen Time'),headerTooltip: this.l('Keihen Time'),field: 'keihenTime',flex: 1},
			{headerName: this.l('Keihen Day'),headerTooltip: this.l('Keihen Day'),field: 'keihenDay',flex: 1},
			{headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},}
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParamsInfo = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsOrderTime = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAllSupplierInfo(
            this.supplierCode,
			this.deliveryMethod,
			this.deliveryFrequency,
			this.orderDateType,
            this.keihenType,
			'',
            this.paginationParamsInfo.skipCount,
            this.paginationParamsInfo.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParamsInfo.totalCount = result.totalCount;
            this.rowDataInfo = result.items;
            this.paginationParamsInfo.totalPage = ceil(result.totalCount / (this.paginationParamsInfo.pageSize ?? 0));
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
    this.supplierCode= '' ;
	this.supplierName= '' ;
	this.address= '' ;
	this.dockX= '' ;
	this.dockXAddress= '' ;
	this.deliveryMethod= '' ;
	this.deliveryFrequency= '' ;
	this.cd= '' ;
	this.orderDateType= '' ;
	this.keihenType= '' ;
	this.productionShift= '' ;
	this.supplierNameEn= '' ;

        this.searchDatas();
    }

    onSelectionMulti(params) {
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRowOrderTime, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRowOrderTime.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAllSupplierInfo(
            this.supplierCode,
			this.deliveryMethod,
			this.deliveryFrequency,
			this.orderDateType,
            this.keihenType,
			'',
            this.paginationParamsInfo.skipCount,
            this.paginationParamsInfo.pageSize
        );
    }

    onChangeRowSelectionInfo(params: { api: { getSelectedRows: () => MstInvGpsSupplierInfoDto[] } }) {
        const selected=params.api.getSelectedRows()[0];
        if(selected){
            this.supplierId=selected.id;
            this.searchDatasOrderTime(selected.id);
        }
        this.selectedRow = Object.assign({}, selected);
    }

    changePageInfo(paginationParams) {
        this.isLoading = true;
        this.paginationParamsInfo = paginationParams;
        this.paginationParamsInfo.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParamsInfo).subscribe((result) => {
            this.paginationParamsInfo.totalCount = result.totalCount;
            this.rowDataInfo = result.items;
            this.paginationParamsInfo.totalPage = ceil(result.totalCount / (this.paginationParamsInfo.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    callBackDataGridInfo(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsInfo.pageSize);
        this.paginationParamsInfo.skipCount =
            ((this.paginationParamsInfo.pageNum ?? 1) - 1) * (this.paginationParamsInfo.pageSize ?? 0);
        this.paginationParamsInfo.pageSize = this.paginationParamsInfo.pageSize;
        this.getDatas(this.paginationParamsInfo)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsInfo.totalCount = result.totalCount;
                this.rowDataInfo = result.items ?? [];
                this.paginationParamsInfo.totalPage = ceil(result.totalCount / (this.paginationParamsInfo.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    deleteRowInfo(system: MstInvGpsSupplierInfoDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGridInfo(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void { 

        this.fn.exportLoading(e, true);
		
           this._service.getGpsSupplierInfoToExcel(
			this.supplierCode,
			this.supplierPlantCode,
			this.supplierName,
			this.address,
			this.dockX,
			this.dockXAddress,
			this.deliveryMethod,
			this.deliveryFrequency,
			this.cd,
			this.orderDateType,
			this.deliveryLt,
			this.orderTrip,
            this.keihenType,
            this.productionShift,
            this.stkConceptTmvMin,
            this.stkConceptSupPMax,
            this.stkConceptSupMMin,
            this.stkConceptTmvMax,
            this.stkConceptSupPMin,
            this.stkConceptSupMMax,
            this.tmvProductPercentage,
            this.picMainId,
            this.deliveryLt,
            this.productionShift,
            this._tcFrom,
            this._tcTo,
            )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }
	
    searchDatasOrderTime(supplierId): void {
        this.isLoading = true;

        this._service.getSupplierOrderTimeBySupplierId(
                supplierId,
                '',
                this.paginationParamsOrderTime.skipCount,
                this.paginationParamsOrderTime.pageSize
            )
            .pipe(finalize(() => {}
              )).subscribe((result) => {
                this.paginationParamsOrderTime.totalCount = result.totalCount;
                this.rowDataOrderTime = result.items;
                this.paginationParamsOrderTime.totalPage = ceil(result.totalCount / (this.paginationParamsOrderTime.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }
    getDatasOrderTime(paginationParams?: PaginationParamsModel) {
        return this._service.getSupplierOrderTimeBySupplierId(
            this.supplierId,
			'',
            this.paginationParamsOrderTime.skipCount,
            this.paginationParamsOrderTime.pageSize
        );
    }
    onChangeRowSelectionOrderTime(params: { api: { getSelectedRows: () => MstInvGpsSupplierOrderTimeDto[] } }) {
        this.saveSelectedRowOrderTime=params.api.getSelectedRows()[0] ?? new MstInvGpsSupplierOrderTimeDto();
        this.selectedRowOrderTime = Object.assign({}, this.saveSelectedRowOrderTime);
    }

    changePageOrderTime(paginationParams) {
        this.isLoading = true;
        this.paginationParamsOrderTime = paginationParams;
        this.paginationParamsOrderTime.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasOrderTime(this.paginationParamsOrderTime).subscribe((result) => {
            this.paginationParamsOrderTime.totalCount = result.totalCount;
            this.rowDataOrderTime = result.items;
            this.paginationParamsOrderTime.totalPage = ceil(result.totalCount / (this.paginationParamsOrderTime.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    callBackDataGridOrderTime(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsOrderTime.pageSize);
        this.paginationParamsOrderTime.skipCount =
            ((this.paginationParamsOrderTime.pageNum ?? 1) - 1) * (this.paginationParamsOrderTime.pageSize ?? 0);
        this.paginationParamsOrderTime.pageSize = this.paginationParamsOrderTime.pageSize;
        this.getDatasOrderTime(this.paginationParamsOrderTime)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsOrderTime.totalCount = result.totalCount;
                this.rowDataOrderTime = result.items ?? [];
                this.paginationParamsOrderTime.totalPage = ceil(result.totalCount / (this.paginationParamsOrderTime.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    deleteRowOrderTime(system: MstInvGpsSupplierOrderTimeDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._serviceOrderTime.delete(system.id).subscribe(() => {
                    this.callBackDataGridOrderTime(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }


    exportToExcel1(e): void { 

        this.fn.exportLoading(e, true);
		
            this._serviceOrderTime.getMstInvGpsSupplierOrderTimeToExcel(
            this.supplierId,
            this.orderSeq,
            this.orderType,
            this.orderTime,
            this.receivingDay,
            this.receiveTime,
            this.keihenTime,
            this.keihenDay,
            this.isActive
            )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }
}
