import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvHrOrgStructureDto, MstInvHrOrgStructureServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './hrorgstructure.component.html',
})
export class HrOrgStructureComponent extends AppComponentBase implements OnInit {
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

    selectedRow: MstInvHrOrgStructureDto = new MstInvHrOrgStructureDto();
    saveSelectedRow: MstInvHrOrgStructureDto = new MstInvHrOrgStructureDto();
    datas: MstInvHrOrgStructureDto = new MstInvHrOrgStructureDto();
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

    id: string = '';
    code: string = '';
    name: string = '';
    description: string = '';
    published: number = 0;
    orgstructuretypename: string = '';
    orgstructuretypecode: string = '';
    parentid: string = '';
    isActive: string = '';

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
        private _service: MstInvHrOrgStructureServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Code'), headerTooltip: this.l('Code'), field: 'code', flex: 1 },
            { headerName: this.l('Name'), headerTooltip: this.l('Name'), field: 'name', flex: 1 },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description', flex: 1 },
            { headerName: this.l('Published'), headerTooltip: this.l('Published'), field: 'published', flex: 1 },
            { headerName: this.l('Org Structure Type Name'), headerTooltip: this.l('Org Structure Type Name'), field: 'orgstructuretypename', width :180},
            { headerName: this.l('Org Structure Type Code'), headerTooltip: this.l('  Org Structure Type Code'), field: 'orgstructuretypecode', width :180},
            { headerName: this.l('Parent Id'), headerTooltip: this.l('Parent Id'), field: 'parentid', flex: 1 },
            // {
            //     headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            // }
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
            this.name,
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
        this.id = '',
            this.code = '',
            this.name = '',
            this.description = '',
            this.published = 0,
            this.orgstructuretypename = '',
            this.orgstructuretypecode = '',
            this.parentid = '',
            this.isActive = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.code,
            this.name,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvHrOrgStructureDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvHrOrgStructureDto();
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

		  this._service.getHrOrgStructureToExcel(
			this.id,
			this.code,
			this.name,
			this.description,
			this.published,
			this.orgstructuretypename,
			this.orgstructuretypecode,
			this.parentid,
			this.isActive,
            )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}