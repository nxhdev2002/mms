import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstSppGlAccountServiceProxy, MstSppGlAccountDto } from '@shared/service-proxies/service-proxies';
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
    templateUrl: './glaccount.component.html',
    // styleUrls: ['./customer.component.less'],
})
export class GLAccountComponent extends AppComponentBase implements OnInit {
    // @ViewChild('createOrEditModalGLAccount', { static: true }) createOrEditModalGLAccount:| CreateOrEditGLAccountModalComponent| undefined;
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

    selectedRow: MstSppGlAccountDto = new MstSppGlAccountDto();
    saveSelectedRow: MstSppGlAccountDto = new MstSppGlAccountDto();
    datas: MstSppGlAccountDto = new MstSppGlAccountDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    glAccountNo : string = '' ;
	glType : string = '' ;
    glAccountNoS4;
    glDescEn;
    glDesc;
    crDb;
	rep : string = '' ;
	fromMonth : number = 0 ;
	fromYear : number = 0 ;
	toMonth : number = 0 ;
	toYear : number = 0 ;
	fromPeriodId
	toPeriodId
	isNew : string = '' ;
	oraGLAccountId
    startDate: any;
    endDate: any;
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
        private _service: MstSppGlAccountServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('GlAccountNo'),headerTooltip: this.l('GlAccountNo'),field: 'glAccountNo',flex: 1},
            {headerName: this.l('GlAccountNoS4'),headerTooltip: this.l('GlAccountNoS4'),field: 'glAccountNoS4',flex: 1},
			{headerName: this.l('GlType'),headerTooltip: this.l('GlType'),field: 'glType',flex: 1},
            {headerName: this.l('Start Date'), headerTooltip: this.l('Start Date'),field: 'startDate', valueFormatter: (params) => this.pipe.transform(params.data?.startDate, 'dd/MM/yyyy'),flex: 1},
			{headerName: this.l('End Date'),headerTooltip: this.l('End Date'),field: 'endDate',flex: 1},
            {headerName: this.l('GlDescEn'),headerTooltip: this.l('GlDescEn'),field: 'glDescEn',flex: 1},
            {headerName: this.l('GlDesc'),headerTooltip: this.l('GlDesc'),field: 'glDesc',flex: 1},
            {headerName: this.l('CrDb'),headerTooltip: this.l('CrDb'),field: 'crDb',flex: 1},

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
			this.glAccountNo,
            this.glType,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
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
        this.glAccountNo = '',
		this.glType = '',
		this.startDate = null;
		this.endDate = null;
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
            this.glAccountNo,
            this.glType,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstSppGlAccountDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstSppGlAccountDto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getGLAccountToExcel(
            this.glAccountNo,
            this.glType,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
