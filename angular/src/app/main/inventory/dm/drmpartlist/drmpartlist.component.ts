import { log } from 'console';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { InvDrmPartListDto, InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ImportDrmPartListComponent } from './import-drmpartlist.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AddAssetDrmPartListModalComponent } from './add-asset-drmpartlist-modal.component';
import { ViewAsAssetModalComponent } from './view-asset-drmpartlist-modal.component';

@Component({
    selector: 'app-drmpartlist',
    templateUrl: './drmpartlist.component.html',
})
export class DrmPartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportDrmPartListComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('addAssetDrmPartList', { static: true }) addAssetDrmPartList: | AddAssetDrmPartListModalComponent | undefined;
    @ViewChild('viewModalAsAsset', { static: true }) viewModalAsAsset: | ViewAsAssetModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvDrmPartListDto = new InvDrmPartListDto();
    saveSelectedRow: InvDrmPartListDto = new InvDrmPartListDto();
    datas: InvDrmPartListDto = new InvDrmPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    supplierType: string = '';
    supplierCd: string = '';
    cfc: string = '';
    materialCode: string = '';
    materialSpec: string = '';
    partCode: string = '';
    partSpec: string = '';
    partSize: string = '';
    sourcing: string = '';
    cutting: string = '';
    packing
    sheetWeight
    yiledRation
    selectedIdAsset;
    selectId;
    changedRecordsDrmPartList: number[] = [];

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
        private _service: InvDrmPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Supplier Type'), headerTooltip: this.l('Supplier Type'), field: 'supplierType',width:150 },
            { headerName: this.l('Supplier Cd'), headerTooltip: this.l('Supplier Cd'), field: 'supplierCd',width:150 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc',width:150 },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', width:150 },
            { headerName: this.l('Material Spec'), headerTooltip: this.l('Material Spec'), field: 'materialSpec',width:500 },
            { headerName: this.l('Part Spec'), headerTooltip: this.l('Part Spec'), field: 'partSpec',width:150 },
            { headerName: this.l('Part Size'), headerTooltip: this.l('Part Size'), field: 'partSize', width:200},
            { headerName: this.l('Part Code'), headerTooltip: this.l('Part Code'), field: 'partCode',width:150 },
            { headerName: this.l('Box Qty'), headerTooltip: this.l('Box Qty'), field: 'boxQty',width:150, type: 'rightAligned' },

            {
                headerName: this.l('First Day Product'), headerTooltip: this.l('First Day Product'), field: 'firstDayProduct',width:150,
                valueGetter: (params) => this.pipe.transform(params.data?.firstDayProduct, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Last Day Product'), headerTooltip: this.l('Last Day Product'), field: 'lastDayProduct',width:150,
                valueGetter: (params) => this.pipe.transform(params.data?.lastDayProduct, 'dd/MM/yyyy'),
            },
            // { headerName: this.l('Asset Id'), headerTooltip: this.l('AssetId'), field: 'assetId',width:90 },
            // { headerName: this.l('Main Asset Number'), headerTooltip: this.l('MainAssetNumber'), field: 'mainAssetNumber',width:600 },
            // { headerName: this.l('Asset Sub Number'), headerTooltip: this.l('AssetSubNumber'), field: 'assetSubNumber',width:150 },
            // { headerName: this.l('WBS'), headerTooltip: this.l('WBS'), field: 'wbs',width:150 },
            // { headerName: this.l('Cost Center'), headerTooltip: this.l('CostCenter'), field: 'costCenter',width:150 },
            // { headerName: this.l('Responsible Cost Center'), headerTooltip: this.l('Responsible Cost Center'), field: 'responsibleCostCenter',width:150 },
            // { headerName: this.l('Cost Of Asset'), headerTooltip: this.l('CostOfAsset'), field: 'costOfAsset',width:150 },


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
            this.changedRecordsDrmPartList = result;
            console.log("result =", result);
        })
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                ``
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
    getViewAsAsset(){
        if(this.selectedIdAsset != null){
            this.viewModalAsAsset.show(this.selectedIdAsset);
        }

    }
    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.supplierType,
            this.supplierCd,
            this.cfc,
            this.materialCode,
            this.materialSpec,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount != 0) {
                    // this.resetGridView();
                }
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.supplierType = '';
        this.supplierCd = '';
        this.materialCode = '';
        this.cfc = '';
        this.materialSpec = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.supplierType,
            this.supplierCd,
            this.cfc,
            this.materialCode,
            this.materialSpec,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmPartListDto[] } }) {

        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmPartListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectedIdAsset = this.saveSelectedRow.assetId != null ? this.saveSelectedRow.assetId : null;
        this.selectId = this.selectedRow.id;
        console.log("haha",this.selectId)
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
            if (result.totalCount != 0) {
                // this.resetGridView();
            }
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
                if (result.totalCount != 0) {
                    // this.resetGridView();
                }
                this.isLoading = false;
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getDrmPartListToExcel(
            this.supplierType,
            this.supplierCd,
            this.cfc,
            this.materialCode,
            this.materialSpec,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
