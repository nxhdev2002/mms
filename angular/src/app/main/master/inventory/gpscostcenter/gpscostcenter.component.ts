import { MstGpsCostCenterServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstGpsCostCenterDto, MstGpsMaterialRegisterByShopDto, MstGpsMaterialRegisterByShopServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditGpsCostCenterModalComponent } from './create-or-edit-gpscostcenter-modal.component';
import { ImportGpsCostCenterComponent } from './import-gpscostcenter.component';


@Component({
    templateUrl: './gpscostcenter.component.html',
})
export class GpsCostCenterComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditCostCenter', { static: true }) createOrEditCostCenter: CreateOrEditGpsCostCenterModalComponent;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportGpsCostCenterComponent | undefined;
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

    selectedRow: MstGpsCostCenterDto = new MstGpsCostCenterDto();
    saveSelectedRow: MstGpsCostCenterDto = new MstGpsCostCenterDto();
    datas: MstGpsCostCenterDto = new MstGpsCostCenterDto();
    pending: string = '';
    disable: boolean = false;
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    group;
    subGroup;
    division;
    dept;
    shop;
    costCenter

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
        private _service1: MstGpsCostCenterServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Group'), headerTooltip: this.l('Group'), field: 'group', flex: 1 },
            { headerName: this.l('SubGroup'), headerTooltip: this.l('SubGroup'), field: 'subGroup',  flex: 1},
            { headerName: this.l('Division'), headerTooltip: this.l('Division'), field: 'division', flex: 1 },
            { headerName: this.l('Dept'), headerTooltip: this.l('Dept'), field: 'dept', flex: 1 },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1 },
            { headerName: this.l('Team'), headerTooltip: this.l('Team'), field: 'team', flex: 1 },
            { headerName: this.l('Cost Center Category'), headerTooltip: this.l('CostCenterCategory'), field: 'costCenterCategory', flex: 1 },
            { headerName: this.l('Budget Owner'), headerTooltip: this.l('BudgetOwner'), field: 'budgetOwner', flex: 1 },
            { headerName: this.l('Cost Center Group'), headerTooltip: this.l('CostCenterGroup'), field: 'costCenterGroup', flex: 1 },
            { headerName: this.l('Cost Center Current'), headerTooltip: this.l('CostCenterCurrent'), field: 'costCenterCurrent', flex: 1 },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('CostCenter'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Is Direct Cost Center'), headerTooltip: this.l('IsDirectCostCenter'), field: 'isDirectCostCenter', flex: 1 },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }


    searchDatas(): void {
        this._service1.getAll(
            this.group,
            this.subGroup,
            this.division,
            this.dept,
            this.shop,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView()
            });
    }

    clearTextSearch() {
        this.group = ''
        this.subGroup = ''
        this.division = ''
        this.dept = ''
        this.shop = ''
        this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service1.getAll(
            this.group,
            this.subGroup,
            this.division,
            this.dept,
            this.shop,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        // setTimeout(() => {
        //     this.dataParams.columnApi!.sizeColumnsToFit({
        //         suppressColumnVirtualisation: true,
        //     });
        //     this.autoSize();
        // }, 1000)
    }


    onChangeRowSelection(params: { api: { getSelectedRows: () => MstGpsCostCenterDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstGpsCostCenterDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;

        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            //this.resetGridView();
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
                //this.resetGridView();
            });
    }


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

       this.fn.exportLoading(e, true);


        this._service1.getMstGpsCostCenterToExcel(
            this.group,
            this.subGroup,
            this.division,
            this.dept,
            this.shop,
        )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    deleteRow(system: MstGpsCostCenterDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service1.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

}
