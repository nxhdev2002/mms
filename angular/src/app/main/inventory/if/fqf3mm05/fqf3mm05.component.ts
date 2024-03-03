import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM05Dto, IF_FQF3MM05ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewFqf3mm05ValidateModalComponent } from './view-fqf3mm05-validate-modal.component';
import { ViewFqf3mm05ValidateResultModalComponent } from './view-fqf3mm05-validate-result-modal.component';
import { ChangeDetectorRef } from '@angular/core';

@Component({
    templateUrl: './fqf3mm05.component.html',
})
export class FQF3MM05Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ViewFqf3mm05Validate', { static: true }) ViewFqf3mm05Validate: ViewFqf3mm05ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm05ValidateResult', { static: true }) ViewFqf3mm05ValidateResult: ViewFqf3mm05ValidateResultModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM05Dto = new IF_FQF3MM05Dto();
    saveSelectedRow: IF_FQF3MM05Dto = new IF_FQF3MM05Dto();
    datas: IF_FQF3MM05Dto = new IF_FQF3MM05Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    documentDate: any;
    postingDate: any;
    materialCodeFrom: string = '';
    valuationTypeFrom: string = '';
    postingDateTo: any;
    postingDateFrom: any;

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
        private _service: IF_FQF3MM05ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Running No (M)'), headerTooltip: this.l('Running No (M)'), field: 'runningNo', flex: 1 },
            { headerName: this.l('Document Date (M)'), headerTooltip: this.l('Document Date (M)'), field: 'documentDate', flex: 1 },
            { headerName: this.l('Posting Date (M)'), headerTooltip: this.l('Posting Date (M)'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Document Header Text (M)'), headerTooltip: this.l('Document Header Text (M)'), field: 'documentHeaderText', flex: 1 },
            { headerName: this.l('Movement Type (M)'), headerTooltip: this.l('Movement Type (M)'), field: 'movementType', flex: 1 },
            { headerName: this.l('Material Code From (M)'), headerTooltip: this.l('Material Code From (M)'), field: 'materialCodeFrom', flex: 1 },
            { headerName: this.l('Plant From (M)'), headerTooltip: this.l('Plant From (M)'), field: 'plantFrom', flex: 1 },
            { headerName: this.l('Valuation Type From (O)'), headerTooltip: this.l('Valuation Type From (O)'), field: 'valuationTypeFrom', flex: 1 },
            { headerName: this.l('Storage Location From (M)'), headerTooltip: this.l('Storage Location From (M)'), field: 'storageLocationFrom', flex: 1 },
            { headerName: this.l('Production Version (O)'), headerTooltip: this.l('Production Version (O)'), field: 'productionVersion', flex: 1 },
            { headerName: this.l('Quantity (M)'), headerTooltip: this.l('Quantity (M)'), field: 'quantity', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Unit Of Entry (M)'), headerTooltip: this.l('Unit Of Entry (M)'), field: 'unitOfEntry', flex: 1 },
            { headerName: this.l('Item Text (O)'), headerTooltip: this.l('Item Text (O)'), field: 'itemText', flex: 1 },
            { headerName: this.l('Gl Account (O)'), headerTooltip: this.l('Gl Account (O)'), field: 'glAccount', flex: 1 },
            { headerName: this.l('Cost Center (C)'), headerTooltip: this.l('Cost Center (C)'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Wbs (C)'), headerTooltip: this.l('Wbs (C)'), field: 'wbs', flex: 1 },
            { headerName: this.l('Material Code To (C)'), headerTooltip: this.l('Material Code To (C)'), field: 'materialCodeTo', flex: 1 },
            { headerName: this.l('Plant To (C)'), headerTooltip: this.l('Plant To (C)'), field: 'plantTo', flex: 1 },
            { headerName: this.l('Valuation Type To (C)'), headerTooltip: this.l('Valuation Type To (C)'), field: 'valuationTypeTo', flex: 1 },
            { headerName: this.l('Storage Location To (C)'), headerTooltip: this.l('Storage Location To (C)'), field: 'storageLocationTo', flex: 1 },
            { headerName: this.l('Bf Pc (O)'), headerTooltip: this.l('Bf Pc (O)'), field: 'bfPc', flex: 1 },
            { headerName: this.l('Cancel Flag (O)'), headerTooltip: this.l('Cancel Flag (O)'), field: 'cancelFlag', flex: 1 },
            { headerName: this.l('Reff Mat Doc No (C)'), headerTooltip: this.l('Reff Mat Doc No (C)'), field: 'reffMatDocNo', flex: 1 },
            { headerName: this.l('Vendor No (C)'), headerTooltip: this.l('Vendor No (C)'), field: 'vendorNo', flex: 1 },
            { headerName: this.l('Profit Center (C)'), headerTooltip: this.l('Profit Center (C)'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Shipemnt Cat (C)'), headerTooltip: this.l('Shipemnt Cat (C)'), field: 'shipemntCat', flex: 1 },
            { headerName: this.l('Reference (O)'), headerTooltip: this.l('Reference (O)'), field: 'reference', flex: 1 },
            { headerName: this.l('Asset No (C)'), headerTooltip: this.l('Asset No (C)'), field: 'assetNo', flex: 1 },
            { headerName: this.l('Sub Asset No (C)'), headerTooltip: this.l('Sub Asset No (C)'), field: 'subAssetNo', flex: 1 },
            { headerName: this.l('End Of Record (M)'), headerTooltip: this.l('End Of Record (M)'), field: 'endOfRecord', flex: 1 }
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
        this.cdr.detectChanges();
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        this.autoSizeAll();
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.materialCodeFrom,
            this.valuationTypeFrom,
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
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.postingDateFrom = '',
            this.postingDateTo = '',
            this.materialCodeFrom = '',
            this.valuationTypeFrom = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.materialCodeFrom,
            this.valuationTypeFrom,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM05Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM05Dto();
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
            this.resetGridView();
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM05ToExcel(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.materialCodeFrom,
            this.valuationTypeFrom,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    reCreateData(e): void {
        if (this.postingDateFrom == null || this.postingDateFrom == '') {
            this.message.warn(this.l('Hãy nhập Posting Date From trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM05(
                this._dateTimeService.convertToDatetime(this.postingDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewValidate(){
        this.ViewFqf3mm05Validate.show(this.postingDateFrom, this.postingDateTo);
    }

    viewValidateData() {
        this.ViewFqf3mm05ValidateResult.show(this.postingDateFrom, this.postingDateTo);
    }
}
