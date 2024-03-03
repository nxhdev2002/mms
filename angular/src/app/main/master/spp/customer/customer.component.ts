import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstSppCustomerDto, MstSppCustomerServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
// import { CreateOrEditCustomerModalComponent } from './create-or-edit-customer-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './customer.component.html',
    // styleUrls: ['./customer.component.less'],
})
export class CustomerComponent extends AppComponentBase implements OnInit {
    // @ViewChild('createOrEditModalCustomer', { static: true }) createOrEditModalCustomer:| CreateOrEditCustomerModalComponent| undefined;
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

    selectedRow: MstSppCustomerDto = new MstSppCustomerDto();
    saveSelectedRow: MstSppCustomerDto = new MstSppCustomerDto();
    datas: MstSppCustomerDto = new MstSppCustomerDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    	code : string = '' ;
	name : string = '' ;
	rep : string = '' ;
	fromMonth : number = 0 ;
	fromYear : number = 0 ;
	toMonth : number = 0 ;
	toYear : number = 0 ;
	fromPeriodId
	toPeriodId
	isNew : string = '' ;
	oraCustomerId
    fromMonthYear: any;
    toMonthYear: any;
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
        private _service: MstSppCustomerServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Code'),headerTooltip: this.l('Code'),field: 'code',flex: 1},
			{headerName: this.l('Name'),headerTooltip: this.l('Name'),field: 'name',flex: 1},
			{headerName: this.l('Rep'),headerTooltip: this.l('Rep'),field: 'rep',flex: 1},
			{headerName: this.l('From Month'),headerTooltip: this.l('From Month'),field: 'fromMonth', type: 'rightAligned',flex: 1},
			{headerName: this.l('From Year'),headerTooltip: this.l('From Year'),field: 'fromYear', type: 'rightAligned',flex: 1},
			{headerName: this.l('To Month'),headerTooltip: this.l('To Month'),field: 'toMonth', type: 'rightAligned',flex: 1},
			{headerName: this.l('To Year'),headerTooltip: this.l('To Year'),field: 'toYear', type: 'rightAligned',flex: 1},
			{headerName: this.l('From Period Id'),headerTooltip: this.l('From Period Id'),field: 'fromPeriodId', type: 'rightAligned',flex: 1},
			{headerName: this.l('To Period Id'),headerTooltip: this.l('To Period Id'),field: 'toPeriodId', type: 'rightAligned',flex: 1},
			{headerName: this.l('Is New'),headerTooltip: this.l('Is New'),field:  'isNew', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
buttonDefTwo: { text: params => (params.data?.isNew == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isNew == 'Y') ? 'btnActive' : 'btnInActive',},},
			{headerName: this.l('Ora Customer Id'),headerTooltip: this.l('Ora Customer Id'),field: 'oraCustomerId', type: 'rightAligned',flex: 1},
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
			this.code,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
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
        this.code = '',
		this.fromMonthYear = null;
		this.toMonthYear = null;
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
 			this.code,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstSppCustomerDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstSppCustomerDto();
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

    // deleteRow(system: MstSppCustomerDto): void {
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCustomerToExcel(
            this.code,
            this._dateTimeService.convertToDatetime(this.fromMonthYear),
            this._dateTimeService.convertToDatetime(this.toMonthYear),
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
