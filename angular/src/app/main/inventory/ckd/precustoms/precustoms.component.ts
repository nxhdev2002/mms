import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPreCustomsDto, InvCkdPreCustomsServiceProxy, InvoiceDetailListDto, InvoiceListDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './precustoms.component.html',
})
export class PreCustomsComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    invoiceColDefs: CustomColDef[] = [];
    invoiceDetailColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsInvoice: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsInvoiceDetail: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPreCustomsDto = new InvCkdPreCustomsDto();
    saveSelectedRow: InvCkdPreCustomsDto = new InvCkdPreCustomsDto();
    saveSelectedRowInvoice: InvoiceListDto = new InvoiceListDto();
    selectedRowInvoice: InvoiceListDto = new InvoiceListDto();
    saveSelectedRowInvoiceDetail: InvoiceDetailListDto = new InvoiceDetailListDto();
    selectedRowInvoiceDetail: InvoiceDetailListDto = new InvoiceDetailListDto();
    datas: InvCkdPreCustomsDto = new InvCkdPreCustomsDto();
    isLoading: boolean = false;
    isLoadingInvoiceDetail: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsInvoiceDetail: GridParams | undefined;
    rowData: any[] = [];
    rowDataInvoice;
    rowDataInvoiceDetail :InvoiceDetailListDto[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    billNo;
    invoiceNo;
    billDate;
    preCustomsId;
    id;
    preCustomsIds;

    _pagesizedetails = 500;

    ckdPio = 'C';

    ckdPioList = [
        { value: 'C', label: "CKD" },
        { value: 'P', label: "PIO" },
    ];

    ordertypeCode: string = '';
    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];

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
        private _service: InvCkdPreCustomsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
        private _formStoringService: FormStoringService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 80
            },
            {
                headerName: this.l('Pre Customs No'),
                headerTooltip: this.l('Pre Customs No'),
                field: 'id',
                flex: 1
            },
            {
                headerName: this.l('Billoflading No'),
                headerTooltip: this.l('Billoflading No'),
                field: 'billofladingNo',
                flex: 1
            },
            {
                headerName: this.l('Bill Date'),
                headerTooltip: this.l('Bill Date'),
                field: 'billDate',
                valueGetter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
                comparator: this._formStoringService.dateComparator,
                flex: 1
            },
            {
                headerName: this.l('TAX'),
                headerTooltip: this.l('TAX'),
                field: 'tax',
                // comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('VAT'),
                headerTooltip: this.l('VAT'),
                field: 'vat',
                // comparator: this._formStoringService.decimalComparator,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.vat, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'description',
                flex: 1
            },

        ];
        this.invoiceColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Invoice No'),
                headerTooltip: this.l('Invoice No'),
                field: 'invoiceNo',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                type: 'rightAligned',
                flex: 1,

            },
            {
                headerName: this.l('Freight Total'),
                headerTooltip: this.l('Freight Total'),
                field: 'freightTotal',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.freightTotal, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Insurance Total'),
                headerTooltip: this.l('Insurance Total'),
                field: 'insuranceTotal',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.insuranceTotal, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Currency'),
                headerTooltip: this.l('Currency'),
                field: 'currency',
                flex: 1
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'description',
                flex: 1
            },

        ];
        this.invoiceDetailColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsInvoiceDetail.pageSize * (this.paginationParamsInvoiceDetail.pageNum - 1),
                cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Invoice No'),
                headerTooltip: this.l('Invoice No'),
                field: 'invoiceNo',
                flex: 1
            },
            {
                headerName: this.l('Fixlot'),
                headerTooltip: this.l('Fixlot'),
                field: 'fixlot',
                flex: 1
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                flex: 1
            },
            {
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'quantity',
                type: 'rightAligned',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.quantity),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Freight'),
                headerTooltip: this.l('Freight'),
                field: 'freight',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Insurance'),
                headerTooltip: this.l('Insurance'),
                field: 'insurance',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Thc'),
                headerTooltip: this.l('Thc'),
                field: 'thc',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.thc, 4),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Cept Type'),
                headerTooltip: this.l('Cept Type'),
                field: 'ceptType',
                flex: 1
            },
            {
                headerName: this.l('Tax'),
                headerTooltip: this.l('Tax'),
                field: 'tAX',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tax,4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Vat'),
                headerTooltip: this.l('Vat'),
                field: 'vAT',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vat,4),
                type: 'rightAligned',
                flex: 1,
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax Rate'),
                headerTooltip: this.l('Tax Rate'),
                field: 'taxRate',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.taxRate, 2),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Vat Rate'),
                headerTooltip: this.l('Vat Rate'),
                field: 'vatRate',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vatRate, 2),
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Declare Type'),
                headerTooltip: this.l('Declare Type'),
                field: 'declareType',
                flex: 1
            },

        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsInvoiceDetail = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }
    createRow(count: number): any[] {
        let result: any[] = [];

        var _SumQuantity = 0;
        var _SumFob = 0;
        var _SumFreight = 0;
        var _SumInsurance = 0;
		var _SumCif = 0;
        var _SumTax = 0;
        var _SumVat = 0;
        if (this.rowDataInvoiceDetail.length > 0) {
            for (let i = 0; this.rowDataInvoiceDetail[i]; i++) {
                _SumQuantity += Number(this.rowDataInvoiceDetail[i].quantity);
                _SumFob += Number(this.rowDataInvoiceDetail[i].fob * this.rowDataInvoiceDetail[i].quantity);
                _SumFreight += Number(this.rowDataInvoiceDetail[i].freight * this.rowDataInvoiceDetail[i].quantity);
                _SumInsurance += Number(this.rowDataInvoiceDetail[i].insurance * this.rowDataInvoiceDetail[i].quantity);
				_SumCif += Number(this.rowDataInvoiceDetail[i].cif * this.rowDataInvoiceDetail[i].quantity);
                _SumTax += Number(this.rowDataInvoiceDetail[i].tax * this.rowDataInvoiceDetail[i].quantity);
                _SumVat += Number(this.rowDataInvoiceDetail[i].vat * this.rowDataInvoiceDetail[i].quantity);
            }
        }

        _SumFob = this._fm.decimal_math(_SumFob, 4, 'round');
        _SumFreight = this._fm.decimal_math(_SumFreight,4);
        _SumInsurance = this._fm.decimal_math(_SumInsurance, 4);
		_SumCif = this._fm.decimal_math(_SumCif,4);
        _SumTax = this._fm.decimal_math(_SumTax, 4);
        _SumVat = this._fm.decimal_math(_SumVat, 4);

        for (var i = 0; i < count; i++) {
            result.push({
                invoiceNo: 'Grand Total',
                quantity: _SumQuantity,
                fob: _SumFob,
                freight: _SumFreight,
                insurance: _SumInsurance,
                tax: _SumTax,
                cif: _SumCif,
                vat: _SumVat,
            });
        }

        return result;
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsInvoiceDetail.columnApi!.autoSizeColumns(allColumnIds);
    }
    autoSizeAllInvoiceDetail() {
        const allColumnIds: string[] = [];
        this.dataParamsInvoiceDetail.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsInvoiceDetail.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsInvoiceDetail.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });

                this.autoSizeAll();
                this.autoSizeAllInvoiceDetail();
        }, 1000);


    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAllPreCustoms(
            this.preCustomsIds,
            this.billNo,
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.billDate),
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            //.pipe(finalize(() => (this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataInvoice = [];
                    this.rowDataInvoiceDetail = [];
                    this.paginationParamsInvoiceDetail.totalCount = result.totalCount;
                    this.paginationParamsInvoiceDetail.totalPage = ceil(result.totalCount / (this.paginationParamsInvoiceDetail.pageSize ?? 0));
                    this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(null);
                }
                else {
                    this.searchDatasInvoice(result.items[0].id);
                }
                this.rowData = result.items;
                this.paginationParams.totalCount = result.totalCount;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }
    searchDatasInvoice(preCustomsId): void {
        this._service.getInvoice(
            preCustomsId,
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        )
            .pipe(finalize(() => this.onChangeRowSelectionInvoice({ api: { getSelectedRows: () => [this.rowDataInvoice[0]] } })))
            .subscribe((result) => {
                this.paginationParamsInvoice.totalCount = result.totalCount;
                this.rowDataInvoice = result.items;
                this.paginationParamsInvoice.totalPage = ceil(result.totalCount / (this.paginationParamsInvoice.pageSize ?? 0));
            });
    }
    searchDatasInvoiceDetail(id): void {
        this._service.getInvoiceDetail(
            this.preCustomsId,
            id,
            '',
            this.paginationParamsInvoiceDetail.skipCount,
            this.paginationParamsInvoiceDetail.pageSize
        )
            .pipe(finalize(() => {
                var rows = this.createRow(1);
                this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows);
            }
            )).subscribe((result) => {
                this.paginationParamsInvoiceDetail.totalCount = result.totalCount;
                this.rowDataInvoiceDetail = result.items;

                this.paginationParamsInvoiceDetail.totalPage = ceil(result.totalCount / (this.paginationParamsInvoiceDetail.pageSize ?? 0));
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.ordertypeCode = '';
        this.preCustomsIds = '',
            this.billNo = '',
            this.invoiceNo = '',
            this.billDate = '',
            this.ckdPio = 'C',
            this.paginationParamsInvoiceDetail = { pageNum: 1, pageSize: this._pagesizedetails, totalCount: 0 };
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAllPreCustoms(
            this.preCustomsIds,
            this.billNo,
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.billDate),
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    getDatasInvoice(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoice(
            this.preCustomsId,
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        );
    }
    getDatasInvoiceDetail(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoiceDetail(
            this.preCustomsId,
            this.id,
            '',
            this.paginationParamsInvoiceDetail.skipCount,
            this.paginationParamsInvoiceDetail.pageSize
        );
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
    changePageInvoice(paginationParams) {
        // this.isLoading = true;
        this.paginationParamsInvoice = paginationParams;
        this.paginationParamsInvoice.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasInvoice(this.paginationParamsInvoice).subscribe((result) => {
            this.paginationParamsInvoice.totalCount = result.totalCount;
            this.rowDataInvoice = result.items;
            this.paginationParamsInvoice.totalPage = ceil(result.totalCount / (this.paginationParamsInvoice.pageSize ?? 0));
            this.isLoading = false;
        });
    }
    changePageInvoiceDetail(paginationParams) {
        // this.isLoading = true;

        this.paginationParamsInvoiceDetail = paginationParams;
        this.paginationParamsInvoiceDetail.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // var rows = this.createRow(1);
        // this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows);
        this.getDatasInvoiceDetail(this.paginationParamsInvoiceDetail)
        .subscribe((result) => {
            this.paginationParamsInvoiceDetail.totalCount = result.totalCount;
            this.rowDataInvoiceDetail = result.items;
            this.paginationParamsInvoiceDetail.totalPage = ceil(result.totalCount / (this.paginationParamsInvoiceDetail.pageSize ?? 0));
            this.resetGridView();
            this.isLoading = false;
        });

        this._pagesizedetails = this.paginationParamsInvoiceDetail.pageSize;
    }
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPreCustomsDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.preCustomsId = selected.id;
            this.searchDatasInvoice(selected.id);
        }
        this.selectedRow = Object.assign({}, selected);
    }
    onChangeRowSelectionInvoice(params: { api: { getSelectedRows: () => InvoiceListDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.id = selected.id;
            this.paginationParamsInvoiceDetail.pageNum = 1;
            this.paginationParamsInvoiceDetail.skipCount = 0;
            this.searchDatasInvoiceDetail(selected.id);
        }
        this.selectedRowInvoice = Object.assign({}, this.selectedRowInvoice);
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
    callBackInvoiceDetailGrid(params) {
        this.dataParamsInvoiceDetail = params;
        this.dataParamsInvoiceDetail.api.paginationSetPageSize(
            this.paginationParamsInvoiceDetail.pageSize);
        this.getDatasInvoiceDetail(this.paginationParamsInvoiceDetail)
        .pipe(finalize(()=>{

            this.gridTableService.selectFirstRow(this.dataParamsInvoiceDetail!.api);
            var rows = this.createRow(1);

            this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows);
        }))
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdPreCustomsToExcel(
            this.preCustomsIds,
            this.billNo,
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.billDate),
            this.ckdPio,
            this.ordertypeCode,
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
    exportInvoiceToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdInvoiceToExcel(
            this.preCustomsId,
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
    exportInvoiceDetailToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdInvoiceDdtailToExcel(
            this.preCustomsId,
            this.id,
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
