import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvDrmStockPartDto, InvDrmStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportDrmStockPartComponent } from './import-drmstockpart.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './drmstockpart.component.html',
})
export class DrmStockPartComponent extends AppComponentBase implements OnInit {
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportDrmStockPartComponent| undefined;
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

    selectedRow: InvDrmStockPartDto = new InvDrmStockPartDto();
    saveSelectedRow: InvDrmStockPartDto = new InvDrmStockPartDto();
    datas: InvDrmStockPartDto = new InvDrmStockPartDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    materialCode: string = '';
    materialSpec: string = '';
    drmMaterialId
    partNo: string = '';
    partName: string = '';
    partId
    materialId
    qty
    workingDate

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
        private _service: InvDrmStockPartServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width:100 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width:100 },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', width:150 },
            { headerName: this.l('Material Spec'), headerTooltip: this.l('Material Spec'), field: 'materialSpec', width:300 },
            { headerName: this.l('Part Code'), headerTooltip: this.l('Part Code'), field: 'partCode', width:150 },
            { headerName: this.l('Drm Material Id'), headerTooltip: this.l('Drm Material Id'), field: 'drmMaterialId', width:150 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width:150 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width:150 },
            { headerName: this.l('Part Id'), headerTooltip: this.l('Part Id'), field: 'partId', width:150 },
            { headerName: this.l('Material Id'), headerTooltip: this.l('Material Id'), field: 'materialId', width:150 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', width:150, type: 'rightAligned' },
            {
                headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', width:150,
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy')
            },
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

    searchDatas(): void {
        this._service.getAll(
            this.materialCode,
            this.materialSpec,
            this.partNo,
            this.partName,
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
        this.materialCode = '',
            this.materialSpec = '',
            this.partNo = '',
            this.partName = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.materialCode,
            this.materialSpec,
            this.partNo,
            this.partName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmStockPartDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmStockPartDto();
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
            // this.resetGridView();
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
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getDrmStockPartToExcel(
            this.materialCode,
            this.materialSpec,
            this.partNo,
            this.partName
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
