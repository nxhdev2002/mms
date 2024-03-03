import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsMappingDto, InvGpsMappingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './mapping.component.html'
})
export class InvGpsMappingComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvGpsMappingDto = new InvGpsMappingDto();
    saveSelectedRow: InvGpsMappingDto = new InvGpsMappingDto();
    datas: InvGpsMappingDto = new InvGpsMappingDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partCatetory: string = '';
    shopRegister: string = '';
    costCenter: string = '';
    wbs: string = '';
    glAccount: string = '';

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
        private _service: InvGpsMappingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo' },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType' },
            { headerName: this.l('Part Catetory'), headerTooltip: this.l('Part Catetory'), field: 'partCatetory' },
            { headerName: this.l('Shop Register'), headerTooltip: this.l('Shop Register'), field: 'shopRegister' },
            { headerName: this.l('Location'), headerTooltip: this.l('Location'), field: 'location' },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter' },
            { headerName: this.l('Wbs'), headerTooltip: this.l('Wbs'), field: 'wbs' },
            { headerName: this.l('Gl Account'), headerTooltip: this.l('Gl Account'), field: 'glAccount' },
            { headerName: this.l('Expense Account'), headerTooltip: this.l('Expense Account'), field: 'expenseAccount' },
            {
                headerName: this.l('Effective Date From'), headerTooltip: this.l('Effective Date From'), field: 'effectiveDateFrom',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/YYYY')
            },
            {
                headerName: this.l('Effective Date To'), headerTooltip: this.l('Effective Date To'), field: 'effectiveDateTo',
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/YYYY')
            },
            { headerName: this.l('Last Renew'), headerTooltip: this.l('Last Renew'), field: 'lastRenew' },
            { headerName: this.l('Renew By'), headerTooltip: this.l('Renew By'), field: 'renewBy' },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status' },
            // {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //             buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},
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
        }, 1000)
    }

    searchDatas(): void {
        this._service.getAll(
            this.partNo,
            this.partCatetory,
            this.shopRegister,
            this.costCenter,
            this.wbs,
            this.glAccount,
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
        this.partNo = '';
        this.partCatetory = '';
        this.shopRegister = '';
        this.costCenter = '';
        this.wbs = '';
        this.glAccount = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.partCatetory,
            this.shopRegister,
            this.costCenter,
            this.wbs,
            this.glAccount,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsMappingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsMappingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
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


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvGpsMappingToExcel(
            this.partNo,
            this.partCatetory,
            this.shopRegister,
            this.costCenter,
            this.wbs,
            this.glAccount
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }


    reMapData(e): void {
        this.fn.exportLoading(e, true);
        this._service.reMapInvGpsMapping().subscribe(() => {
            this.fn.exportLoading(e);
            this.notify.success(this.l('Validate Successfully!'));
            this.searchDatas();
        });
    }

    validateData(e): void {
        this.fn.exportLoading(e, true);
        this._service.validateInvGpsMapping().subscribe(() => {
            this.fn.exportLoading(e);
            this.notify.success(this.l('Re - map Successfully!'));
            this.searchDatas();
        });
    }
}
