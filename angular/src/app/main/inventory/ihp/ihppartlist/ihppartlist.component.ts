import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvIhpPartGradeDto, InvIhpPartListDto, InvIhpPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './ihppartlist.component.html',
})
export class IhpPartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    changedRecordsPartList = [];
    changedRecordsPartGrade = [];
    partListColDefs: CustomColDef[] = [];
    partGradeColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsPartGrade: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvIhpPartListDto = new InvIhpPartListDto();
    saveSelectedRow: InvIhpPartListDto = new InvIhpPartListDto();
    selectedRowGrade: InvIhpPartGradeDto = new InvIhpPartGradeDto();
    datas: InvIhpPartListDto = new InvIhpPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsGrade: GridParams | undefined;
    rowDataPartList: any[] = [];
    rowDataPartGrade: any[] = [];
    dataParamsCustoms: GridParams | undefined;

    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    supplierType: string = '';
    supplierCd: string = '';
    model: string = '';
    partCode: string = '';
    partName: string = '';
    cfc: string = '';
    materialCode: string = '';
    materialSpec: string = '';
    partSpec: string = '';
    partSize: string = '';
    sourcing: string = '';
    cutting: string = '';
    packing
    sheetWeight
    yiledRation;
    partListId;
    grade;
    fn: CommonFunction = new CommonFunction();
    _selectCfc;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
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
        private _service: InvIhpPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.partListColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Supplier Type'), headerTooltip: this.l('Supplier Type'), field: 'supplierType', flex: 1 },
            { headerName: this.l('Supplier Cd'), headerTooltip: this.l('Supplier Cd'), field: 'supplierCd', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Material Spec'), headerTooltip: this.l('Material Spec'), field: 'materialSpec', flex: 1 },
            { headerName: this.l('Sourcing'), headerTooltip: this.l('Sourcing'), field: 'sourcing', flex: 1 },
            { headerName: this.l('Cutting'), headerTooltip: this.l('Cutting'), field: 'cutting', flex: 1 },
            {
                headerName: this.l('Packing'), headerTooltip: this.l('Packing'), field: 'packing', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.packing, 0)
            },
            {
                headerName: this.l('Sheet Weight'), headerTooltip: this.l('Sheet Weight'), field: 'sheetWeight', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.sheetWeight, 2)
            },
            {
                headerName: this.l('Yiled Ration'), headerTooltip: this.l('Yiled Ration'), field: 'yiledRation', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.yiledRation, 2)
            },
        ];
        this.partGradeColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsPartGrade.pageSize * (this.paginationParamsPartGrade.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade', flex: 1 },
            //  { headerName: this.l('Ihp Part Id'), headerTooltip: this.l('Ihp Part Id'), field: 'ihpPartId', flex: 1 },
            { headerName: this.l('Usage Qty'), headerTooltip: this.l('Usage Qty'), field: 'usageQty', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('First Day Product'), headerTooltip: this.l('First Day Product'), field: 'firstDayProduct', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.firstDayProduct, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Last Day Product'), headerTooltip: this.l('Last Day Product'), field: 'lastDayProduct', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.lastDayProduct, 'dd/MM/yyyy'),
            },
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
        this.fetchChangedRecords();
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsPartList = result.partList;
            this.changedRecordsPartGrade = result.partGrade;
        })
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getDataPartList(
            this.supplierCd,
            this.cfc,
            this.partCode,
            this.grade,
            this.partName,
            this.materialCode,
            this.materialSpec,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowDataPartList = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if (result.totalCount == 0) this.rowDataPartGrade = [];
                else {
                    this.searchDataPartGrade(result.items[0].id);
                }
            });
    }

    clearTextSearch() {
        this.supplierCd = '',
            this.partCode = '',
            this.grade = '',
            this.partName = '',
            this.materialCode = '',
            this.materialSpec = '',
            this.cfc = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getDataPartList(
            this.supplierCd,
            this.cfc,
            this.partCode,
            this.grade,
            this.partName,
            this.materialCode,
            this.materialSpec,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvIhpPartListDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.partListId = selected.id;
            this.searchDataPartGrade(selected.id);
        }
        this.selectedRow = Object.assign({}, selected);

        this._selectCfc = selected.cfc;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowDataPartList = result.items;
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
                this.rowDataPartList = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    //PartGrade
    searchDataPartGrade(partListId): void {
        this._service.getDataPartGradebyId(
            partListId,
            '',
            this.paginationParamsPartGrade.skipCount,
            this.paginationParamsPartGrade.pageSize
        ).pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsGrade!.api)))
            .subscribe((result) => {
                this.paginationParamsPartGrade.totalCount = result.totalCount;
                this.rowDataPartGrade = result.items;
                this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
            });
    }

    getDataPartGrade(paginationParamsPartGrade?: PaginationParamsModel) {
        return this._service.getDataPartGradebyId(
            this.partListId,
            '',
            this.paginationParamsPartGrade.skipCount,
            this.paginationParamsPartGrade.pageSize
        );
    }


    changePagePartGrade(paginationParamsPartGrade) {
        this.isLoading = true;
        this.paginationParamsPartGrade = paginationParamsPartGrade;
        this.paginationParamsPartGrade.skipCount = (paginationParamsPartGrade.pageNum - 1) * paginationParamsPartGrade.pageSize;
        this.getDataPartGrade(this.paginationParamsPartGrade).subscribe((result) => {
            this.paginationParamsPartGrade.totalCount = result.totalCount;
            this.rowDataPartGrade = result.items;
            this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGridGrade(params: GridParams) {
        this.dataParamsGrade = params;
    }

    onChangeRowSelectionGrade(params: { api: { getSelectedRows: () => InvIhpPartGradeDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectedRowGrade = Object.assign({}, selected);
    }

    // callBackDataGridCustoms(params: GridParams) {
    //     this.isLoading = true;
    //     this.dataParamsCustoms = params;
    //     params.api.paginationSetPageSize(this.paginationParamsPartGrade.pageSize);
    //     this.paginationParamsPartGrade.skipCount =
    //         ((this.paginationParamsPartGrade.pageNum ?? 1) - 1) * (this.paginationParamsPartGrade.pageSize ?? 0);
    //     this.paginationParamsPartGrade.pageSize = this.paginationParamsPartGrade.pageSize;
    //     this.getDataPartGrade(this.paginationParamsPartGrade)

    //         .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsCustoms!.api)))

    //         .subscribe((result) => {
    //             this.paginationParamsPartGrade.totalCount = result.totalCount;
    //             this.rowDataPartGrade = result.items ?? [];
    //             this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
    //             this.isLoading = false;
    //             // this.resetGridView();

    //         });
    // }
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getIhpPartListToExcel(
            this.supplierCd,
            this.cfc,
            this.partCode,
            this.grade,
            this.partName,
            this.materialCode,
            this.materialSpec,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    exportPartGradeToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getInvIhpPartGradeToExcel(
            this.partListId
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    deleteIhpPartGrade() {
        this.message.confirm(this.l('Delete this Grade'), this.l('Are You Sure?'), (isConfirmed) => {
            if (isConfirmed) {
                this.isLoading = true;
                this._service.deleteInhousePartGrade(this.selectedRowGrade.id)
                    .pipe(finalize(() => this.isLoading = false))
                    .subscribe((res) => {
                        this.notify.success(this.l('Delete Part Grade Successfully'));
                        this.searchDataPartGrade(this.partListId);
                        this.isLoading = false;
                    })
            }
        });
    }
}
