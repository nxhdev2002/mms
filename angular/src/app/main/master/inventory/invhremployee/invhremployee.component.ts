import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvGpsTruckSupplierDto, MstInvGpsTruckSupplierServiceProxy, MstInvHrEmployeeDto, MstInvHrEmployeeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './invhremployee.component.html',
})
export class InvHrEmployeeComponent extends AppComponentBase implements OnInit {
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

    selectedRow: MstInvHrEmployeeDto = new MstInvHrEmployeeDto();
    saveSelectedRow: MstInvHrEmployeeDto = new MstInvHrEmployeeDto();
    datas: MstInvHrEmployeeDto = new MstInvHrEmployeeDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    supplierId : any;
    truckName : string = '' ;
    isActive : string = '';
    p_employee_code;
    p_name;
    p_email_address;
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
        private _service: MstInvHrEmployeeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Employee Code'),headerTooltip: this.l('Employee Code'),field: 'employeeCode',flex: 1},
			{headerName: this.l('User Name'),headerTooltip: this.l('User Name'),field: 'userName',flex: 1},
            {headerName: this.l('Email Address'),headerTooltip: this.l('Email Address'),field: 'emailAddress',flex: 1},
			{headerName: this.l('Title Name'),headerTooltip: this.l('Title Name'),field: 'titleName',flex: 1},
            {headerName: this.l('Position Name'),headerTooltip: this.l('Position Name'),field: 'positionName',flex: 1},
			{headerName: this.l('Org Structure Name'),headerTooltip: this.l('Org Structure Name'),field: 'orgStructureName',flex: 1},
            // {
            //     headerName: this.l('Is Active'),
            //     headerTooltip: this.l('Is Active'),
            //     field: 'isActive',
            //     cellClass: ['text-center'],
            //     width: 150,
            //     cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: {
            //         text: (params) => (params.data?.isActive == 'true' ? 'Active' : 'Inactive'),
            //         iconName: 'fa fa-circle',
            //         className: (params) => (params.data?.isActive == 'true' ? 'btnActive' : 'btnInActive'),
            //     },
            // },
        ],
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
            this.p_employee_code,
            this.p_name,
            this.p_email_address,
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
        console.log(this.rowData);

    }

    clearTextSearch() {
        this.p_email_address = '' ;
        this.p_employee_code = '';
        this.p_name = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.p_employee_code,
            this.p_name,
            this.p_email_address,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvHrEmployeeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvHrEmployeeDto();
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

        this._service.getEmployeeToExcel(
            this.p_employee_code,
            this.p_name,
            this.p_email_address,
            )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
