import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmMMValidationResultDto, MstCommonMMValidationResultServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './mmvalidationresult.component.html',
})
export class CmmMMValidationResultComponent extends AppComponentBase implements OnInit {
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

    changedRecords: number[] = [];
    selectId;
    selectedRow: MstCmmMMValidationResultDto = new MstCmmMMValidationResultDto();
    saveSelectedRow: MstCmmMMValidationResultDto = new MstCmmMMValidationResultDto();
    datas: MstCmmMMValidationResultDto = new MstCmmMMValidationResultDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();
    ruleCode: string = '';
    ruleDescription: string = '';
    ruleItem: string = '';
    materialCode: string = '';
    resultfield: string = '';
    materialGroup: string = '';

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
        private _service: MstCommonMMValidationResultServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', width:120},
            { headerName: this.l('Material Name'), headerTooltip: this.l('Material Name'), field: 'materialName',width:120 },
            { headerName: this.l('Material Group'), headerTooltip: this.l('Material Group'), field: 'materialGroup', width:120},
            { headerName: this.l('Valuation Class'), headerTooltip: this.l('Valuation Class'), field: 'valuationClass',width:120 },
            { headerName: this.l('Valuation Type'), headerTooltip: this.l('Valuation Type'), field: 'valuationType', width:120},
            { headerName: this.l('Rule Code'), headerTooltip: this.l('Rule Code'), field: 'ruleCode',width:120 },
            { headerName: this.l('Rule Description'), headerTooltip: this.l('Rule Description'), field: 'ruleDescription',width:120 },
            { headerName: this.l('Rule Item'), headerTooltip: this.l('Rule Item'), field: 'ruleItem',width:120 },
            { headerName: this.l('Option'), headerTooltip: this.l('Option'), field: 'option',width:120 },
            { headerName: this.l('Result Field'), headerTooltip: this.l('Result Field'), field: 'resultField', width:120},
            {
                headerName: this.l('Expected Result'),
                headerTooltip: this.l('Expected Result'),
                field: 'expectedResult',
                width:120,
                valueGetter: (params) => params.data?.expectedResult != '' ? params.data?.expectedResult : '(null)'
            },
            {
                headerName: this.l('Actual Result'),
                headerTooltip: this.l('Actual Result'),
                field: 'actualResult',
                width:120,
                valueGetter: (params) => params.data?.actualResult != '' ? params.data?.actualResult : '(null)'
            },
            {
                headerName: this.l('Last Validation Datetime'),
                headerTooltip: this.l('Last Validation Datetime'),
                field: 'lastValidationDatetime',
                width:120,
                valueGetter: (params) => this.pipe.transform(params.data?.lastValidationDatetime, 'dd/MM/yyyy HH:mm')
            },
            {
                headerName: this.l('Last Validation By'),
                headerTooltip: this.l('Lastvalidationby'),
                field: 'lastvalidationby',
                width:120
            },
            { headerName: this.l('Last Validation Id'), headerTooltip: this.l('Last Validation Id'), field: 'lastValidationId', width:120},
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status',width:120 },
            { headerName: this.l('Error Message'), headerTooltip: this.l('Error Message'), field: 'errorMessage',width:120 },

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
            this.changedRecords = result;
            console.log("result =", result);
        })
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
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
        })
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.materialCode,
            this.materialGroup,
            this.ruleCode,
            this.ruleItem,
            this.resultfield,
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

    clearTextSearch() {
        this.ruleCode = '';
        this.ruleItem = '';
        this.materialCode = '';
        this.resultfield = '',
        this.materialGroup = '';
        this.searchDatas();
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.materialCode,
            this.materialGroup,
            this.ruleCode,
            this.ruleItem,
            this.resultfield,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmMMValidationResultDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmMMValidationResultDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCmmMMValidationResultToExcel(
            this.materialCode,
            this.materialGroup,
            this.ruleCode,
            this.ruleItem,
            this.resultfield
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
}
