import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsPartListDto, InvGpsPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';
import { ImportPartListComponent } from './import-partlist-modal.component';

@Component({
    selector: 'app-partlist',
    templateUrl: './partlist.component.html'
})
export class PartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportPartListComponent| undefined;
    defaultColDefs: CustomColDef[] = [];
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

    listColDefs: any;
    gradeColDefs: any;

    selectedRow: InvGpsPartListDto = new InvGpsPartListDto();
    saveSelectedRow: InvGpsPartListDto = new InvGpsPartListDto();
    listDto: InvGpsPartListDto = new InvGpsPartListDto();
    datas: InvGpsPartListDto = new InvGpsPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParams_PartGrade: GridParams | undefined;
    rowData: any[] = [];
    rowDataGrade: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partNoNormalized: string = '';
    partName: string = '';
    supplierNo: string = '';
    uom: string = '';
    type: string = '';
    color: string = '';
    summerRadio: any = '';
    winterRatio: any = '';
    diffRatio: any = '';
    seasonType: string = '';
    remark: string = '';
    boxqty: string = '';
    remark1: string = '';

    grade: string = '';
    bodyColor: string = '';
    processUse: string = '';
    shop: string = '';
    usageQty: any;
    partListId: any;
    vehicleId: any;
    isPartColor;
    isPartColorList = [
        {value: '' , label: "All"},
        {value: 'Y' , label: "Color"},
        {value: 'N' , label: "No Color"},
    ];
    p_id: any;

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
        private _service: InvGpsPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.listColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part No Normalized'), headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Uom'), headerTooltip: this.l('Uom'), field: 'uom', flex: 1 },
            { headerName: this.l('BoxQty'), headerTooltip: this.l('BoxQty'), field: 'boxQty', flex: 1 },
            { headerName: this.l('Type'), headerTooltip: this.l('Type'), field: 'type', flex: 1 },
            { headerName: this.l('Color'), headerTooltip: this.l('Color'), field: 'color', flex: 1 },
            { headerName: this.l('Summer Radio'), headerTooltip: this.l('Summer Radio'), field: 'summerRadio', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Winter Ratio'), headerTooltip: this.l('Winter Ratio'), field: 'winterRatio', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Diff Ratio'), headerTooltip: this.l('Diff Ratio'), field: 'diffRatio', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Season Type'), headerTooltip: this.l('Season Type'), field: 'seasonType', flex: 1 },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 },
            { headerName: this.l('Remark1'), headerTooltip: this.l('Remark1'), field: 'remark1', flex: 1 },
            { headerName: this.l('MinLot'), headerTooltip: this.l('MinLot'), field: 'minLot', flex: 1 },
            { headerName: this.l('Category'), headerTooltip: this.l('Category'), field: 'category', flex: 1 },
            { headerName: this.l('StartDate'), headerTooltip: this.l('StartDate'), field: 'startDate', flex: 1 },
            { headerName: this.l('EndDate'), headerTooltip: this.l('EndDate'), field: 'endDate', flex: 1 },
            { headerName: this.l('IsPartColor'), headerTooltip: this.l('IsPartColor'), field: 'isPartColor', flex: 1 },

        ];
        this.gradeColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsPartGrade.pageSize * (this.paginationParamsPartGrade.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part List Id'), headerTooltip: this.l('Part List Id'), field: 'partListId', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade', flex: 1 },
            { headerName: this.l('Body Color'), headerTooltip: this.l('Body Color'), field: 'bodyColor', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Usage Qty'), headerTooltip: this.l('Usage Qty'), field: 'usageQty', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Process Use'), headerTooltip: this.l('Process Use'), field: 'processUse', flex: 1 },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1 },
            { headerName: this.l('Vehicle Id'), headerTooltip: this.l('Vehicle Id'), field: 'vehicleId', flex: 1 },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsPartGrade = { pageNum: 1, pageSize: 500, totalCount: 0 };
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

    autoSizeAll_PartGrade() {
        const allColumnIds: string[] = [];
        this.dataParams_PartGrade.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams_PartGrade.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView_PartGrade() {

        setTimeout(() => {
            this.dataParams_PartGrade.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll_PartGrade();
        }, 100)
    }

    clearTextSearch() {
    this.partNo = '',
    this.supplierNo = '',
    this.isPartColor = '',
    this.searchDatas();
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        console.log(this.isPartColor)
        this._service.getGpsPartList(
            this.partNo,
            this.supplierNo,
            this.isPartColor,
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


                if(result.totalCount = 0) {
                    this.rowDataGrade = [];
                } else {
                    this.searchDataGrade(result.items[0].id);
                }
            });
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
        return this._service.getGpsPartList(
            this.partNo,
            this.supplierNo,
            this.isPartColor,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsPartListDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id = selected.id;
            this.searchDataGrade(selected.id);
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

    callBackDataGrid_PartGrade(params: GridParams) {
        this.dataParams_PartGrade = params;
    }

    searchDataGrade(p_id): void {
        this.isLoading = true;

        this._service.getGpsPartGradeByPartList(
            p_id,
            this.grade,
            '',
            this.paginationParamsPartGrade.skipCount,
            this.paginationParamsPartGrade.pageSize
        )
            .pipe(finalize(() => { }
            )).subscribe((result) => {
                this.paginationParamsPartGrade.totalCount = result.totalCount;
                this.rowDataGrade = result.items;
                this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
                // this.resetGridView_PartGrade();
                this.resetGridView();
                this.isLoading = false;
            });
    }

    getDataGrade(paginationParams?: PaginationParamsModel) {
        return this._service.getGpsPartGradeByPartList(
            this.p_id,
            this.grade,
            '',
            this.paginationParamsPartGrade.skipCount,
            this.paginationParamsPartGrade.pageSize
        );
    }

    changePageGrade(paginationParams) {
        this.isLoading = true;
        this.paginationParamsPartGrade = paginationParams;
        this.paginationParamsPartGrade.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataGrade(this.paginationParamsPartGrade).subscribe((result) => {
            this.paginationParamsPartGrade.totalCount = result.totalCount;
            this.rowDataGrade = result.items;
            this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();

        });
    }

    callBackDataGridGrade(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsPartGrade.pageSize);
        this.paginationParamsPartGrade.skipCount =
            ((this.paginationParamsPartGrade.pageNum ?? 1) - 1) * (this.paginationParamsPartGrade.pageSize ?? 0);
        this.paginationParamsPartGrade.pageSize = this.paginationParamsPartGrade.pageSize;
        this.getDataGrade(this.paginationParamsPartGrade)

            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))

            .subscribe((result) => {
                this.paginationParamsPartGrade.totalCount = result.totalCount;
                this.rowDataGrade = result.items ?? [];
                this.paginationParamsPartGrade.totalPage = ceil(result.totalCount / (this.paginationParamsPartGrade.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();

            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getExportInvGpsPartList(
            this.partNo,
            this.grade
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }


}

