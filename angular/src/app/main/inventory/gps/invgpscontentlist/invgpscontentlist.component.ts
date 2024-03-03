import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsContentListDto, InvGpsContentListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditInvGpsContentListModalComponent } from './create-or-edit-invgpscontentlist-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';

import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './invgpscontentlist.component.html',
    styleUrls: ['./invgpscontentlist.component.less'],
})
export class InvGpsContentListComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalInvGpsContentList', { static: true }) createOrEditModalInvGpsContentList:| CreateOrEditInvGpsContentListModalComponent| undefined;
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

    selectedRow: InvGpsContentListDto = new InvGpsContentListDto();
    saveSelectedRow: InvGpsContentListDto = new InvGpsContentListDto();
    datas: InvGpsContentListDto = new InvGpsContentListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
	workingDateFrom = new Date();
	workingDateTo = new Date();
    shift : string = '' ;
	supplierName : string = '' ;
	supplierCode : string = '' ;
	pcAddress : string = '' ;
	dockNo : string = '' ;
	orderNo : string = '' ;
	contentNo : string = '' ;
	packagingType : string = '' ;
	generatedBy : string = '' ;
	moduleCd : string = '' ;
	status : string = '' ;


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
        private _service: InvGpsContentListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'), valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),field: 'workingDate'},
			{headerName: this.l('Shift'),headerTooltip: this.l('Shift'),field: 'shift'},
			{headerName: this.l('Supplier Name'),headerTooltip: this.l('Supplier Name'),field: 'supplierName'},
			{headerName: this.l('Supplier Code'),headerTooltip: this.l('Supplier Code'),field: 'supplierCode'},
			{headerName: this.l('Renban No'),headerTooltip: this.l('Renban No'),field: 'renbanNo'},
			{headerName: this.l('Pc Address'),headerTooltip: this.l('Pc Address'),field: 'pcAddress'},
			{headerName: this.l('Dock No'),headerTooltip: this.l('Dock No'),field: 'dockNo'},
			{headerName: this.l('Order No'),headerTooltip: this.l('Order No'),field: 'orderNo'},
			{headerName: this.l('Order Datetime'),headerTooltip: this.l('Order Datetime'),valueGetter: (params) => this.pipe.transform(params.data?.orderDatetime, 'dd/MM/yyyy HH:mm:ss'),field: 'orderDatetime'},
			{headerName: this.l('Trip No'),headerTooltip: this.l('Trip No'),field: 'tripNo'},
			{headerName: this.l('Pallet Box Qty'),headerTooltip: this.l('Pallet Box Qty'),field: 'palletBoxQty'},
			{headerName: this.l('Est Packing Datetime'),headerTooltip: this.l('Est Packing Datetime'),valueGetter: (params) => this.pipe.transform(params.data?.estPackingDatetime, 'dd/MM/yyyy HH:mm:ss'),field: 'estPackingDatetime'},
			{headerName: this.l('Est Arrival Datetime'),headerTooltip: this.l('Est Arrival Datetime'),valueGetter: (params) => this.pipe.transform(params.data?.estArrivalDatetime, 'dd/MM/yyyy HH:mm:ss'),field: 'estArrivalDatetime'},
			{headerName: this.l('Content No'),headerTooltip: this.l('Content No'),field: 'contentNo', },
			{headerName: this.l('Order Id'),headerTooltip: this.l('Order Id'),field: 'orderId'},
			{headerName: this.l('Pallet Size'),headerTooltip: this.l('Pallet Size'),field: 'palletSize'},
			{headerName: this.l('Is Pallet Only'),headerTooltip: this.l('Is Pallet Only'),field:  'isPalletOnly', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.isPalletOnly == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isPalletOnly == 'Y') ? 'btnActive' : 'btnInActive',},},
			{headerName: this.l('Packaging Type'),headerTooltip: this.l('Packaging Type'),field: 'packagingType', },
			{headerName: this.l('Is Adhoc Receiving'),headerTooltip: this.l('Is Adhoc Receiving'),field:  'isAdhocReceiving', cellClass: ['text-center'], width: 150, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.isAdhocReceiving == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isAdhocReceiving == 'Y') ? 'btnActive' : 'btnInActive',},},
			{headerName: this.l('Generated By'),headerTooltip: this.l('Generated By'),field: 'generatedBy', width: 120, },
			{headerName: this.l('Unpack Status'),headerTooltip: this.l('Unpack Status'),field:  'unpackStatus', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: { text: params => (params.data?.unpackStatus == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.unpackStatus == 'Y') ? 'btnActive' : 'btnInActive',},},
			{headerName: this.l('Module Cd'),headerTooltip: this.l('Module Cd'),field: 'moduleCd', },
			{headerName: this.l('Module Run No'),headerTooltip: this.l('Module Run No'),field: 'moduleRunNo',},
			{headerName: this.l('Up Start Act'),headerTooltip: this.l('Up Start Act'),field: 'upStartAct'},
			{headerName: this.l('Up Finish Act'),headerTooltip: this.l('Up Finish Act'),field: 'upFinishAct'},
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status'},
			// {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            // buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},
            // }
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.resetGridView();
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
            this.supplierCode,
            this.dockNo,
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
        this.workingDateFrom = null,
		this.workingDateTo = null,
        this.shift = '' ;
        this.supplierCode = '' ;
        this.dockNo ='' ;


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
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
            this.supplierCode,
            this.dockNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );

    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsContentListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsContentListDto();
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

    deleteRow(system: InvGpsContentListDto): void {
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

        this._service.getInvGpsContentListToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
			this._dateTimeService.convertToDatetime(this.workingDateTo),
			this.shift,
            this.supplierCode,
            this.dockNo,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
