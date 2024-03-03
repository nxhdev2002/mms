import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwRenbanModuleDto, MstLgwRenbanModuleServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditRenbanModuleModalComponent } from './create-or-edit-renbanmodule-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
//import { ImportRenbanModuleComponent } from './import-renbanmodule-modal.component';


@Component({
    templateUrl: './renbanmodule.component.html',
    styleUrls: ['./renbanmodule.component.less'],
})
export class RenbanModuleComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalRenbanModule', { static: true }) createOrEditModalRenbanModule: | CreateOrEditRenbanModuleModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    //@ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportRenbanModuleComponent| undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstLgwRenbanModuleDto = new MstLgwRenbanModuleDto();
    saveSelectedRow: MstLgwRenbanModuleDto = new MstLgwRenbanModuleDto();
    datas: MstLgwRenbanModuleDto = new MstLgwRenbanModuleDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    renban: string = '';
    caseNo: string = '';
    supplierNo: string = '';
    minModule: number = 0;
    maxModule: number = 0;
    moduleCapacity: number = 0;
    moduleType: string = '';
    moduleSize: string = '';
    sortingType: string = '';
    minMod: number = 0;
    maxMod: number = 0;
    monitorVisualize: number = 0;
    caseOrder: number = 0;
    caseType: string = '';
    prodLine: string = '';
    model: string = '';
    cfc: string = '';
    whLoc: string = '';
    isUsePxpData: string = '';
    upLeadtime: number = 0;
    remark: string = '';
    isActive: string = '';


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
        private _service: MstLgwRenbanModuleServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Renban'),
                headerTooltip: this.l('Renban'),
                field: 'renban',
                width: 90
            },
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
                width: 90
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width: 90
            },
            {
                headerName: this.l('Min Module'),
                headerTooltip: this.l('Min Module'),
                field: 'minModule',
                type: 'rightAligned',
                width: 100
            },
            {
                headerName: this.l('Max Module'),
                headerTooltip: this.l('Max Module'),
                field: 'maxModule',
                type: 'rightAligned',
                width: 100
            },
            {
                headerName: this.l('Module Capacity'),
                headerTooltip: this.l('Module Capacity'),
                field: 'moduleCapacity',
                type: 'rightAligned',
                width: 120
            },
            {
                headerName: this.l('Module Type'),
                headerTooltip: this.l('Module Type'),
                field: 'moduleType',
                width: 100
            },
            {
                headerName: this.l('Module Size'),
                headerTooltip: this.l('Module Size'),
                field: 'moduleSize',
                width: 100
            },
            {
                headerName: this.l('Sorting Type'),
                headerTooltip: this.l('Sorting Type'),
                field: 'sortingType',
                width: 100
            },
            {
                headerName: this.l('Min Mod'),
                headerTooltip: this.l('Min Mod'),
                field: 'minMod',
                width: 90
            },
            {
                headerName: this.l('Max Mod'),
                headerTooltip: this.l('Max Mod'),
                field: 'maxMod',
                width: 90
            },
            {
                headerName: this.l('Monitor Visualize'),
                headerTooltip: this.l('Monitor Visualize'),
                field: 'monitorVisualize',
                type: 'rightAligned',
                width: 130
            },
            {
                headerName: this.l('Case Order'),
                headerTooltip: this.l('Case Order'),
                field: 'caseOrder',
                type: 'rightAligned',
                width: 90
            },
            {
                headerName: this.l('Case Type'),
                headerTooltip: this.l('Case Type'),
                field: 'caseType',
                width: 90
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 90
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                width: 90
            },
            {
                headerName: this.l('Wh Loc'),
                headerTooltip: this.l('Wh Loc'),
                field: 'whLoc',
                width: 90
            },
            {
                headerName: this.l('Is Use Pxp Data'),
                headerTooltip: this.l('Is Use Pxp Data'),
                field: 'isUsePxpData',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                width: 130,
                buttonDefTwo: {
                    text: params => (params.data?.isUsePxpData == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isUsePxpData == "Y") ? 'btnActive' : 'btnInActive',
                },

            },
            {
                headerName: this.l('Up Leadtime'),
                headerTooltip: this.l('Up Leadtime'),
                field: 'upLeadtime',
                type: 'rightAligned',
                width: 100
            },
            {
                headerName: this.l('Remark'),
                headerTooltip: this.l('Remark'),
                field: 'remark',
                width: 100
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                width: 90,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },

            },
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
            this.renban,
            this.caseNo,
            this.supplierNo,
            this.minModule,
            this.maxModule,
            this.moduleCapacity,
            this.moduleType,
            this.moduleSize,
            this.sortingType,
            this.minMod,
            this.maxMod,
            this.monitorVisualize,
            this.caseOrder,
            this.caseType,
            this.prodLine,
            this.model,
            this.cfc,
            this.whLoc,
            this.isUsePxpData,
            this.upLeadtime,
            this.remark,
            this.isActive,
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
            this.renban = '',
            this.caseNo = '',
            this.supplierNo = '',
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
            this.renban,
            this.caseNo,
            this.supplierNo,
            this.minModule,
            this.maxModule,
            this.moduleCapacity,
            this.moduleType,
            this.moduleSize,
            this.sortingType,
            this.minMod,
            this.maxMod,
            this.monitorVisualize,
            this.caseOrder,
            this.caseType,
            this.prodLine,
            this.model,
            this.cfc,
            this.whLoc,
            this.isUsePxpData,
            this.upLeadtime,
            this.remark,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgwRenbanModuleDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgwRenbanModuleDto();
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

    deleteRow(system: MstLgwRenbanModuleDto): void {
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
        this.isLoading = true;
        this._service.getRenbanModuleToExcel(
            this.renban,
            this.caseNo,
            this.supplierNo,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }

    
    
}
