import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM03Dto, IF_FQF3MM03ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewBusinessDataComponent } from './view-business-data-modal.component';
import { ViewFqf3mm03ValidateModalComponent } from './view-fqf3mm03-validate-modal.component';
import { ViewFqf3mm03ValidateResultModalComponent } from './view-fqf3mm03-validate-result-modal.component';

@Component({
    templateUrl: './fqf3mm03.component.html',
})
export class FQF3MM03Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewBusinessData', { static: true }) viewBusinessData: ViewBusinessDataComponent | undefined;
    @ViewChild('ViewFqf3mm03Validate', { static: true }) ViewFqf3mm03Validate: ViewFqf3mm03ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm03ValidateResult', { static: true }) ViewFqf3mm03ValidateResult: ViewFqf3mm03ValidateResultModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM03Dto = new IF_FQF3MM03Dto();
    saveSelectedRow: IF_FQF3MM03Dto = new IF_FQF3MM03Dto();
    datas: IF_FQF3MM03Dto = new IF_FQF3MM03Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    recordId: string = '';
    companyCode: string = '';
    documentNo: string = '';
    documentType: string = '';
    customerCode: string = '';
    customerPlantCode: string = '';
    customerDockCode: string = '';
    partCategory: string = '';
    withholdingTaxFlag: string = '';
    withholdingTaxRate: string = '';
    orderType: string = '';
    pdsNo: string = '';
    partReceivedDate: any;
    sequenceDate: any;
    sequenceNo: string = '';
    referenceDocumentNo: string = '';
    supplierCode: string = '';
    supplierPlantCode: string = '';
    partQuantity: string = '';
    unitBuyingPrice: string = '';
    unitBuyingAmount: string = '';
    unitSellingPrice: string = '';
    unitSellingAmount: string = '';
    priceStatus: string = '';
    totalAmount: string = '';
    vatAmount: string = '';
    vatCode: string = '';
    paymentTerm: string = '';
    reasonCode: string = '';
    markCode: string = '';
    signCode: string = '';
    cancelFlag: string = '';
    supplierInvoiceNo: string = '';
    topasSmrNo: string = '';
    topasSmrItemNo: string = '';
    customerBranch: string = '';
    costCenter: string = '';
    wbs: string = '';
    asset: string = '';
    orderReasonCode: string = '';
    retroFlag: string = '';
    valuationType: string = '';
    conditionType: string = '';
    conditionTypeAmt: string = '';
    prepaidTaxAmt: string = '';
    withholdingTaxAmt: string = '';
    stampFeeAmt: string = '';
    glAmount: string = '';
    sptCode: string = '';
    higherLevelItem: string = '';
    withholdingTaxBaseAmt: string = '';
    typeOfSales: string = '';
    profitCenter: string = '';
    dueDate: any;
    itemText: string = '';
    paymentMethod: string = '';
    endingOfRecord: string = '';
    postingDateFrom: any;
    postingDate: any;
    fn: CommonFunction = new CommonFunction();
    sequenceDateFrom;
    sequenceDateTo;

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
        private _service: IF_FQF3MM03ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Record Id(M)'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Company Code(M)'), headerTooltip: this.l('Company Code'), field: 'companyCode', flex: 1 },
            { headerName: this.l('Document No(O)'), headerTooltip: this.l('Document No'), field: 'documentNo', flex: 1 },
            { headerName: this.l('Document Type(M)'), headerTooltip: this.l('Document Type'), field: 'documentType', flex: 1 },
            { headerName: this.l('Document Date(M)'), headerTooltip: this.l('Document Date'), field: 'documentDate', flex: 1 },
            { headerName: this.l('Customer Code(O)'), headerTooltip: this.l('Customer Code'), field: 'customerCode', flex: 1 },
            { headerName: this.l('Customer Plant Code(M)'), headerTooltip: this.l('Customer Plant Code'), field: 'customerPlantCode', flex: 1 },
            { headerName: this.l('Customer Dock Code(M)'), headerTooltip: this.l('Customer Dock Code'), field: 'customerDockCode', flex: 1 },
            {headerName: this.l('Part Category(M)'), headerTooltip: this.l('Part Category'), field: 'partCategory', flex: 1},
			{headerName: this.l('Withholding Tax Flag(O)'), headerTooltip: this.l('Withholding Tax Flag'), field: 'withholdingTaxFlag', flex: 1},
			{ headerName: this.l('Withholding Tax Rate(O)'), headerTooltip: this.l('Withholding Tax Rate'), field: 'withholdingTaxRate', flex: 1 },
            {headerName: this.l('Order Type(M)'), headerTooltip: this.l('Order Type'), field: 'orderType', flex: 1 },
			{ headerName: this.l('Pds No(M)'), headerTooltip: this.l('Pds No'), field: 'pdsNo', flex: 1 },
            { headerName: this.l('Part Received Date(M)'), headerTooltip: this.l('Part Received Date'), field: 'partReceivedDate', flex: 1 },
            { headerName: this.l('Sequence Date(M)'), headerTooltip: this.l('Sequence Date'), field: 'sequenceDate', flex: 1 },
            { headerName: this.l('Sequence No(M)'), headerTooltip: this.l('Sequence No'), field: 'sequenceNo', flex: 1 },
            { headerName: this.l('Part No(M)'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Reference Document No(O)'), headerTooltip: this.l('Reference Document No'), field: 'referenceDocumentNo', flex: 1 },
            { headerName: this.l('Posting Date(M)'), headerTooltip: this.l('Posting Date'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Supplier Code(M)'), headerTooltip: this.l('Supplier Code'), field: 'supplierCode', flex: 1 },
            {headerName: this.l('Supplier Plant Code(O)'), headerTooltip: this.l('Supplier Plant Code'), field: 'supplierPlantCode', flex: 1},
			{ headerName: this.l('Part Quantity(M)'), headerTooltip: this.l('Part Quantity'), field: 'partQuantity', flex: 1 },
            { headerName: this.l('Unit Buying Price(M)'), headerTooltip: this.l('Unit Buying Price'), field: 'unitBuyingPrice', flex: 1 },
            { headerName: this.l('Unit Buying Amount(O)'), headerTooltip: this.l('Unit Buying Amount'), field: 'unitBuyingAmount', flex: 1 },
            { headerName: this.l('Unit Selling Price(O)'), headerTooltip: this.l('Unit Selling Price'), field: 'unitSellingPrice', flex: 1 },
            { headerName: this.l('Unit Selling Amount(O)'), headerTooltip: this.l('Unit Selling Amount'), field: 'unitSellingAmount', flex: 1 },
            {headerName: this.l('Price Status(M)'), headerTooltip: this.l('Price Status'), field: 'priceStatus', flex: 1},
			{ headerName: this.l('Total Amount(O)'), headerTooltip: this.l('Total Amount'), field: 'totalAmount', flex: 1 },
            { headerName: this.l('Vat Amount(O)'), headerTooltip: this.l('Vat Amount'), field: 'vatAmount', flex: 1 },
            { headerName: this.l('Vat Code(M)'), headerTooltip: this.l('Vat Code'), field: 'vatCode', flex: 1 },
            { headerName: this.l('Payment Term(O)'), headerTooltip: this.l('Payment Term'), field: 'paymentTerm', flex: 1 },
            {headerName: this.l('Reason Code(O)'), headerTooltip: this.l('Reason Code'), field: 'reasonCode', flex: 1},
			{headerName: this.l('Mark Code(O)'), headerTooltip: this.l('Mark Code'), field: 'markCode', flex: 1},
			{headerName: this.l('Sign Code(O)'), headerTooltip: this.l('Sign Code'), field: 'signCode', flex: 1},
			{headerName: this.l('Cancel Flag'), headerTooltip: this.l('Cancel Flag'), field: 'cancelFlag', flex: 1},
			{ headerName: this.l('Supplier Invoice No(O)'), headerTooltip: this.l('Supplier Invoice No'), field: 'supplierInvoiceNo', flex: 1 },
            { headerName: this.l('Topas Smr No(O)'), headerTooltip: this.l('Topas Smr No'), field: 'topasSmrNo', flex: 1 },
            { headerName: this.l('Topas Smr Item No(O)'), headerTooltip: this.l('Topas Smr Item No'), field: 'topasSmrItemNo', flex: 1 },
            { headerName: this.l('Customer Branch(O)'), headerTooltip: this.l('Customer Branch'), field: 'customerBranch', flex: 1 },
            { headerName: this.l('Cost Center(O)'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Wbs(O)'), headerTooltip: this.l('Wbs'), field: 'wbs', flex: 1 },
            { headerName: this.l('Asset(O)'), headerTooltip: this.l('Asset'), field: 'asset', flex: 1 },
            {headerName: this.l('Order Reason Code(O)'), headerTooltip: this.l('Order Reason Code'), field: 'orderReasonCode', flex: 1},
			{headerName: this.l('Retro Flag(O)'), headerTooltip: this.l('Retro Flag'), field: 'retroFlag', flex: 1},
			{ headerName: this.l('Valuation Type(M)'), headerTooltip: this.l('Valuation Type'), field: 'valuationType', flex: 1 },
            { headerName: this.l('Condition Type(O)'), headerTooltip: this.l('Condition Type'), field: 'conditionType', flex: 1 },
            { headerName: this.l('Condition Type Amt(O)'), headerTooltip: this.l('Condition Type Amt'), field: 'conditionTypeAmt', flex: 1 },
            { headerName: this.l('Prepaid Tax Amt(O)'), headerTooltip: this.l('Prepaid Tax Amt'), field: 'prepaidTaxAmt', flex: 1 },
            { headerName: this.l('Withholding Tax Amt(O)'), headerTooltip: this.l('Withholding Tax Amt'), field: 'withholdingTaxAmt', flex: 1 },
            { headerName: this.l('Stamp Fee Amt(O)'), headerTooltip: this.l('Stamp Fee Amt'), field: 'stampFeeAmt', flex: 1 },
            { headerName: this.l('Gl Amount(O)'), headerTooltip: this.l('Gl Amount'), field: 'glAmount', flex: 1 },
            { headerName: this.l('Spt Code(M)'), headerTooltip: this.l('Spt Code'), field: 'sptCode', flex: 1 },
            { headerName: this.l('Higher Level Item(O)'), headerTooltip: this.l('Higher Level Item'), field: 'higherLevelItem', flex: 1 },
            { headerName: this.l('Withholding Tax Base Amt(O)'), headerTooltip: this.l('Withholding Tax Base Amt'), field: 'withholdingTaxBaseAmt', flex: 1 },
            { headerName: this.l('Type Of Sales(O)'), headerTooltip: this.l('Type Of Sales'), field: 'typeOfSales', flex: 1 },
            { headerName: this.l('Profit Center(O)'), headerTooltip: this.l('Profit Center'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Due Date(O)'), headerTooltip: this.l('Due Date'), field: 'dueDate', flex: 1 },
            { headerName: this.l('Item Text(O)'), headerTooltip: this.l('Item Text'), field: 'itemText', flex: 1 },
            {headerName: this.l('Payment Method(O)'), headerTooltip: this.l('Payment Method'), field: 'paymentMethod', flex: 1},
			{headerName: this.l('Ending Of Record(M)'), headerTooltip: this.l('Ending Of Record'), field: 'endingOfRecord', flex: 1}
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
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.supplierCode,
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
            this.pdsNo,
            this._dateTimeService.convertToDatetime(this.sequenceDateFrom),
            this._dateTimeService.convertToDatetime(this.sequenceDateTo),
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
            this.partNo = '',
            this.postingDate = '',
            this.postingDateFrom = '',
            this.pdsNo = '',
            this.sequenceDateFrom = '',
            this.sequenceDateTo = '',
            this.supplierCode = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.supplierCode,
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
            this.pdsNo,
            this._dateTimeService.convertToDatetime(this.sequenceDateFrom),
            this._dateTimeService.convertToDatetime(this.sequenceDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM03Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM03Dto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM03ToExcel(
            this.partNo,
            this.supplierCode,
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
            this.pdsNo,
            this._dateTimeService.convertToDatetime(this.sequenceDateFrom),
            this._dateTimeService.convertToDatetime(this.sequenceDateTo),  
             )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.fn.exportLoading(e);
            });
    }
    reCreateData(e): void {
        if (this.postingDateFrom == null || this.postingDateFrom == '') {
            this.message.warn(this.l('Hãy nhập Posting Date From trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM03(
                this._dateTimeService.convertToDatetime(this.postingDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewLoggingResponse(){
        this.viewBusinessData.show(this.postingDateFrom,this.postingDate);
    }

    viewValidate(){
        this.ViewFqf3mm03Validate.show(this.postingDateFrom,this.postingDate);
    }

    viewValidateData() {
        this.ViewFqf3mm03ValidateResult.show(this.postingDateFrom,this.postingDate);
    }
}
