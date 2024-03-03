import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM07Dto, IF_FQF3MM07ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewFundCommmitmentItemDMComponent } from './view-fund-commitment-item-modal.component';
import { ViewLoggingResponseBudgetCheckComponent } from './view-logging-response-budget-check-modal.component';
import { ViewFqf3mm07ValidateModalComponent } from './view-fqf3mm07-validate-modal.component';
import { ChangeDetectorRef } from '@angular/core';
import { ViewFqf3mm07ValidateResultModalComponent } from './view-fqf3mm07-validate-result-modal.component';

@Component({
    templateUrl: './fqf3mm07.component.html',
})
export class FQF3MM07Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewFundCommitmentItem', { static: true }) viewFundCommitmentItem: ViewFundCommmitmentItemDMComponent | undefined;
    @ViewChild('viewLoggingResponseBudgetCheck', { static: true }) viewLoggingResponseBudgetCheck: ViewLoggingResponseBudgetCheckComponent | undefined;
    @ViewChild('ViewFqf3mm07Validate', { static: true }) ViewFqf3mm07Validate: ViewFqf3mm07ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm07ValidateResult', { static: true }) ViewFqf3mm07ValidateResult: ViewFqf3mm07ValidateResultModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM07Dto = new IF_FQF3MM07Dto();
    saveSelectedRow: IF_FQF3MM07Dto = new IF_FQF3MM07Dto();
    datas: IF_FQF3MM07Dto = new IF_FQF3MM07Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    documentNo: string = '';
    documentDateFrom: any;
    documentDateTo: any;
    postingDateFrom: any;
    postingDateTo: any;
    idFund: number;
    documentNoN;
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
    checkFun: boolean = true;
    constructor(
        injector: Injector,
        private _service: IF_FQF3MM07ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Record Id (M)'), headerTooltip: this.l('Record Id (M)'), field: 'recordId', flex: 1 },
            { headerName: this.l('Country Code (O)'), headerTooltip: this.l('Country Code (O)'), field: 'countryCode', flex: 1 },
            { headerName: this.l('Company Code (M)'), headerTooltip: this.l('Company Code (M)'), field: 'companyCode', flex: 1 },
            { headerName: this.l('Company Branch (O)'), headerTooltip: this.l('Company Branch (O)'), field: 'companyBranch', flex: 1 },
            { headerName: this.l('Posting Key (M)'), headerTooltip: this.l('Posting Key (M)'), field: 'postingKey', flex: 1 },
            { headerName: this.l('Cost Center (O)'), headerTooltip: this.l('Cost Center (O)'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Document No (M)'), headerTooltip: this.l('Document No (M)'), field: 'documentNo', flex: 1 },
            { headerName: this.l('Document Type (M)'), headerTooltip: this.l('Document Type (M)'), field: 'documentType', flex: 1 },
            { headerName: this.l('Document Date (M)'), headerTooltip: this.l('Document Date (M)'), field: 'documentDate', flex: 1 },
            { headerName: this.l('Posting Date (M)'), headerTooltip: this.l('Posting Date (M)'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Plant (O)'), headerTooltip: this.l('Plant (O)'), field: 'plant', flex: 1 },
            { headerName: this.l('Reference Document No (O)'), headerTooltip: this.l('Reference Document No (O)'), field: 'referenceDocumentNo', flex: 1 },
            { headerName: this.l('Amount (M)'), headerTooltip: this.l('Amount (M)'), field: 'amount', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Currency (M)'), headerTooltip: this.l('Currency (M)'), field: 'currency', flex: 1 },
            { headerName: this.l('Order (O)'), headerTooltip: this.l('Order (O)'), field: 'order', flex: 1 },
            { headerName: this.l('Gl Account (O)'), headerTooltip: this.l('Gl Account (O)'), field: 'glAccount', flex: 1 },
            { headerName: this.l('Normal Cancel Flag (M)'), headerTooltip: this.l('Normal Cancel Flag (M)'), field: 'normalCancelFlag', flex: 1 },
            { headerName: this.l('Text (O)'), headerTooltip: this.l('Text (O)'), field: 'text', flex: 1 },
            { headerName: this.l('Profit Center (O)'), headerTooltip: this.l('Profit Center (O)'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Wbs (O)'), headerTooltip: this.l('Wbs (O)'), field: 'wbs', flex: 1 },
            { headerName: this.l('Quantity (O)'), headerTooltip: this.l('Quantity (O)'), field: 'quantity', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Base Unit Of Measure (O)'), headerTooltip: this.l('Base Unit Of Measure (O)'), field: 'baseUnitOfMeasure', flex: 1 },
            { headerName: this.l('Amount In Local Currency (O)'), headerTooltip: this.l('Amount In Local Currency (O)'), field: 'amountInLocalCurrency', flex: 1 },
            { headerName: this.l('Exchange Rate (O)'), headerTooltip: this.l('Exchange Rate (O)'), field: 'exchangeRate', flex: 1 },
            { headerName: this.l('Ref Key 1 (O)'), headerTooltip: this.l('Ref Key 1 (O)'), field: 'refKey1', flex: 1 },
            { headerName: this.l('Ref Key 2 (O)'), headerTooltip: this.l('Ref Key 2 (O)'), field: 'refKey2', flex: 1 },
            { headerName: this.l('Ref Key 3 (O)'), headerTooltip: this.l('Ref Key 3 (O)'), field: 'refKey3', flex: 1 },
            { headerName: this.l('Earmark Fund (O)'), headerTooltip: this.l('Earmark Fund (O)'), field: 'earmarkFund', flex: 1 },
            { headerName: this.l('Earmark Fund Item (O)'), headerTooltip: this.l('Earmark Fund Item (O)'), field: 'earmarkFundItem', flex: 1 },
            { headerName: this.l('Material No (O)'), headerTooltip: this.l('Material No (O)'), field: 'materialNo', flex: 1 },
            { headerName: this.l('Main Asset Number (O)'), headerTooltip: this.l('Main Asset Number (O)'), field: 'mainAssetNumber', flex: 1 },
            { headerName: this.l('Asset Sub Number (O)'), headerTooltip: this.l('Asset Sub Number (O)'), field: 'assetSubNumber', flex: 1 },
            { headerName: this.l('Trans Type (O)'), headerTooltip: this.l('Trans Type(O)'), field: 'transType', flex: 1 },
            { headerName: this.l('Ending Of Record (M)'), headerTooltip: this.l('Ending Of Record (M)'), field: 'endingOfRecord', flex: 1 },
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
            this.documentNo,
            this._dateTimeService.convertToDatetime(this.documentDateFrom),
            this._dateTimeService.convertToDatetime(this.documentDateTo),
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() =>{}))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }
    clearTextSearch() {
        this.documentNo = '',
            this.documentDateFrom = '',
            this.documentDateTo = '',
            this.postingDateFrom = '',
            this.postingDateTo = '',
            this.searchDatas();
    }
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.documentNo,
            this._dateTimeService.convertToDatetime(this.documentDateFrom),
            this._dateTimeService.convertToDatetime(this.documentDateTo),
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM07Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM07Dto();
        this.idFund = this.saveSelectedRow.id;
        this.documentNoN = this.saveSelectedRow.documentNo;
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.checkFun = false;

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
            .pipe(finalize(() => {}))
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

        this._service.getFQF3MM07ToExcel(
            this.documentNo,
            this._dateTimeService.convertToDatetime(this.documentDateFrom),
            this._dateTimeService.convertToDatetime(this.documentDateTo),
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo)
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });

    }

    exportFundToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFundCommitmentItemDMToExcel().subscribe((result) => {
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
            this._service.reCreateDataFQF3MM07(
                this._dateTimeService.convertToDatetime(this.postingDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewFundCommitItemDM(){
        this.viewFundCommitmentItem.show(this.idFund);
    }

    viewLoggingResponse(){
        this.viewLoggingResponseBudgetCheck.show(this.documentNoN);
    }

    viewValidate(){
        this.ViewFqf3mm07Validate.show(this.postingDateFrom,this.postingDateTo);
    }

    viewValidateData() {
        this.ViewFqf3mm07ValidateResult.show(this.postingDateFrom,this.postingDateTo);
    }
}



