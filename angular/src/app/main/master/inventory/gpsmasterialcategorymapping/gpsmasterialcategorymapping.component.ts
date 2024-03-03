import { CreateOrEditMstGpsMaterialCategoryMappingDto, MstGpsMaterialCategoryMappingServiceProxy, MstInvGpsMaterialCategoryServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmColorDto, MstCmmColorServiceProxy, MstGpsMaterialCategoryMappingDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditGpsMasterialCategoryMappingModalComponent } from './create-or-edit-gpsmasterialcategorymapping-modal.component';

@Component({
    templateUrl: './gpsmasterialcategorymapping.component.html',
})
export class GpsMasterialCategoryMappingComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalGpsMtCategoryMapping', { static: true }) createOrEditModal: | CreateOrEditGpsMasterialCategoryMappingModalComponent | undefined;
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

    selectedRow: MstGpsMaterialCategoryMappingDto = new MstGpsMaterialCategoryMappingDto();
    saveSelectedRow: MstGpsMaterialCategoryMappingDto = new MstGpsMaterialCategoryMappingDto();
    datas: MstGpsMaterialCategoryMappingDto = new MstGpsMaterialCategoryMappingDto();
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

    yVCategory: string = '';
    gLExpenseAccount: string ='';
    gLLevel5InWBS;
    gLAccountDescription;
    definition;
    fixedVariableCost;
    example;
    isActive: string = '';
    changedRecordsMaterialCategoryMapping: number[] = [];
    selectId;

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
        private _service: MstGpsMaterialCategoryMappingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            {
                headerName: this.l('YV Category'),
                headerTooltip: this.l('YV Category'),
                field: 'yvCategory', flex: 1
            },
            {
                headerName: this.l('GL Expense Account'),
                headerTooltip: this.l('GL Expense Account'),
                field: 'glExpenseAccount', flex: 1
            },
            {
                headerName: this.l('GL Level 5 In WBS'),
                headerTooltip: this.l('GL Level 5 In WBS'),
                field: 'glLevel5InWBS', flex: 1
            },
            {
                headerName: this.l('GL Account Description'),
                headerTooltip: this.l('GL Account Description'),
                field: 'glAccountDescription', flex: 1
            },
            {
                headerName: this.l('Definition'),
                headerTooltip: this.l('Definition'),
                field: 'definition', flex: 1
            },
            {
                headerName: this.l('Fixed / Variable Cost'),
                headerTooltip: this.l('Fixed / Variable Cost'),
                field: 'fixedVariableCost', flex: 1
            },
            {
                headerName: this.l('Example'),
                headerTooltip: this.l('Example'),
                field: 'example', flex: 1
            },
            {
                headerName: this.l('Account Type'),
                headerTooltip: this.l('Account Type'),
                field: 'accountType',
                flex: 1
            },
            {
                headerName: this.l('Posting Key'),
                headerTooltip: this.l('Posting Key'),
                field: 'postingKey',
                flex: 1
            },
            {
                headerName: this.l('Part Type'),
                headerTooltip: this.l('Part Type'),
                field: 'partType',
                flex: 1
            },
            {
                headerName: this.l('Document Type'),
                headerTooltip: this.l('Document Type'),
                field: 'documentType',
                flex: 1
            },
            {
                headerName: this.l('Is Asset'),
                headerTooltip: this.l('Is Asset'),
                field: 'isAsset',
                flex: 1
            },
            {
                headerName: this.l('Revert Cancel'),
                headerTooltip: this.l('Revert Cancel'),
                field: 'revertCancel',
                flex: 1
            },
            {
                headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsMaterialCategoryMapping= result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        this._service.getAll(
            this.yVCategory,
            this.gLExpenseAccount,
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
            this.yVCategory = '',
            this.gLExpenseAccount = '',
            this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.yVCategory,
            this.gLExpenseAccount,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstGpsMaterialCategoryMappingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstGpsMaterialCategoryMappingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
        console.log("ID = "+ this.selectId);
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

    deleteRow(system: MstGpsMaterialCategoryMappingDto): void {
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
        this._service.getMaterialCategoryMappingToExcel(
            this.yVCategory,
            this.gLExpenseAccount
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }


}
