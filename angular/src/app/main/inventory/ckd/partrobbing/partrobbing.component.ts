import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartRobbingDto, InvCkdPartRobbingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './partrobbing.component.html',
    styleUrls: ['./partrobbing.component.less'],
})
export class PartRobbingComponent extends AppComponentBase implements OnInit {
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
    paginationParamsDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    fn: CommonFunction = new CommonFunction();
    robbingColDefs: any;
    robbingDetailsColDefs: any;

    selectedRow: InvCkdPartRobbingDto = new InvCkdPartRobbingDto();
    saveSelectedRow: InvCkdPartRobbingDto = new InvCkdPartRobbingDto();
    robbingDto: InvCkdPartRobbingDto = new InvCkdPartRobbingDto();
    datas: InvCkdPartRobbingDto = new InvCkdPartRobbingDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    rowData: any[] = [];
    rowDataDetails: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partNoNormalized: string = '';
    partName: string = '';
    orderPattern: string = '';
    cfc: string = '';
    shop: string = '';
    case: string = '';
    box: string = '';
    supplierNo: string = '';
    isActive: string = '';

    grade: string = '';
    robbingQty: number;

    p_part_id: number;

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
        private _service: InvCkdPartRobbingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.robbingColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 150 },
            { headerName: this.l('Part No Normalized'), headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized', width: 150 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 300 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width: 150 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 150 },
            { headerName: this.l('Robbing Qty'), headerTooltip: this.l('RobbingQty'), field: 'robbingQty', width: 150, type: 'rightAligned' },
            { headerName: this.l('Unit Qty'), headerTooltip: this.l('UnitQty'), field: 'unitQty', width: 150, type: 'rightAligned' },
            { headerName: this.l('EffectVeh Qty'), headerTooltip: this.l('EffectVehQty'), field: 'effectVehQty', width: 150, type: 'rightAligned' },
            { headerName: this.l('DetailModel'), headerTooltip: this.l('DetailModel'), field: 'detailModel', width: 150 },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', width: 150 },
            { headerName: this.l('Case'), headerTooltip: this.l('Case'), field: 'case', width: 100 },
            { headerName: this.l('Box'), headerTooltip: this.l('Box'), field: 'box', width: 100 },
        ];

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.fn.setHeight_notFullHeight();
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsDetails = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getPartRobbingSearch(
            this.partNo,
            this.cfc,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
                this.isLoading = false;
            });
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
        }, 100)
    }

    clearTextSearch() {
        this.partNo = '',
        this.cfc = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getPartRobbingSearch(
            this.partNo,
            this.cfc,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPartRobbingDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_part_id = selected.id;
        }
        this.selectedRow = Object.assign({}, selected);
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
            // this.resetGridView();
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
                // this.resetGridView();
            });
    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPartRobbingToExcel(
            this.partNo,
            this.cfc,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
