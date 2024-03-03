import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { InVoiceListDto, InvCkdCustomsDeclareDto, InvCkdCustomsDeclareServiceProxy, PreCustomerDto } from '@shared/service-proxies/service-proxies';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './customsdeclare.component.html',
})
export class CustomsDeclareModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    customerColDefs: CustomColDef[] = [];
    preCustomerColDefs: CustomColDef[] = [];
    invoiceColDefs: CustomColDef[] = [];
    paginationParamsDeclare: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsPreCustomer: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsInvoice: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdCustomsDeclareDto = new InvCkdCustomsDeclareDto();
    saveSelectedRow: InvCkdCustomsDeclareDto = new InvCkdCustomsDeclareDto();

    saveSelectedRowPreCustomer: PreCustomerDto = new PreCustomerDto();
    selectedRowPreCustomer: PreCustomerDto = new PreCustomerDto();

    saveSelectedRowInVoiceList: InVoiceListDto = new InVoiceListDto();
    selectedRowInVoiceList: InVoiceListDto = new InVoiceListDto();

    datas: InvCkdCustomsDeclareDto = new InvCkdCustomsDeclareDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsPreCustomer: GridParams | undefined;
    dataParamsInvoice: GridParams | undefined;
    rowDataDeclare: any[] = [];
    rowDataInvoice: any[] = [];;
    rowDataInvoiceDetail: any[] = [];;
    gridApi!: GridApi;
    // rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    rowDataPreCustomer;
    billNo;
    customsDeclareNo;
    declareDate;
    invoiceId;
    invoiceNo;
    billofladingNo;
    billDate;
    tax;
    customsDeclareId;
    vat;
    totalvatCept;
    description;
    totaltaxCept
    totaltaxNoncept
    totalvatNoncept
    totalcifCept
    totalcifNoncept;
    preCustomsId;
    id;
    status: string = '';
    ordertypeCode: string = '';
    updatedDate
    groupId
    isvnaccs
    groupParentId
    declareType: string = '';
    isActive: string = '';
    test: number = 0;
    ckdPio = 'C';

    ckdPioList = [
        { value: 'C', label: "CKD" },
        { value: 'P', label: "PIO" },
    ];

    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];

    fn: CommonFunction = new CommonFunction();

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
        private _service: InvCkdCustomsDeclareServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
    ) {
        super(injector);
        this.customerColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDeclare.pageSize * (this.paginationParamsDeclare.pageNum - 1),
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: this.l('Customs Declare No '),
                headerTooltip: this.l('Customs Declare No'),
                field: 'customsDeclareNo',
                flex: 1
            },
            {
                headerName: this.l('Declare Date'),
                headerTooltip: this.l('Declare Date'),
                field: 'declareDate',
                comparator: this._formStoringService.dateComparator,
                valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Bill Of Lading No '),
                headerTooltip: this.l('Billoflading No'),
                field: 'billofladingNo',
                flex: 1
            },
            {
                headerName: this.l('Bill Date'),
                headerTooltip: this.l('Bill Date'),
                field: 'billDate',
                comparator: this._formStoringService.dateComparator,
                valueGetter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Tax (VND)'),
                headerTooltip: this.l('Tax (VND)'),
                field: 'tax',
                // comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 0,false,true).toString(),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Vat (VND)'),
                headerTooltip: this.l('Vat (VND)'),
                field: 'vat',
                // comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vat, 0,false,true).toString(),
                flex: 1,
                type: 'rightAligned',

            },
            {
                headerName: this.l('Is From Ecus'),
                headerTooltip: this.l('Is From Ecus'),

                field: 'isFromEcus',
                flex: 1
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
            {
                headerName: this.l('Customs Port'),
                headerTooltip: this.l('Customs Port'),
                field: 'customsPort',
                flex: 1
            },
            {
                headerName: this.l('Business Type'),
                headerTooltip: this.l('Business Type'),
                field: 'businessType',
                flex: 1
            },
            {
                headerName: this.l('Input Date'),
                headerTooltip: this.l('Input Date'),
                field: 'inputDate',
                // comparator: this._formStoringService.dateComparator,
                valueFormatter: (params) => this.pipe.transform(params.data?.inputDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Exchange Rate'),
                headerTooltip: this.l('Exchange Rate'),
                field: 'exchangeRate',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.exchangeRate, 0),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Forwarder'),
                headerTooltip: this.l('Forwarder'),
                field: 'forwarder',
                flex: 1
            },
            {
                headerName: this.l('Actual Tax'),
                headerTooltip: this.l('Actual Tax'),
                field: 'actualTax',
                // comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.actualTax, 0,false,true).toString(),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Actual Vat'),
                headerTooltip: this.l('Actual Vat'),
                field: 'actualVat',
                // comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.actualVat, 0,false,true).toString(),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Sum'),
                headerTooltip: this.l('Sum'),
                field: 'sum',
                // comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.sum, 0,false,true).toString(),
                flex: 1,
                type: 'rightAligned',
                sortable: true,
            },
            {
                headerName: this.l('Complete Tax'),
                headerTooltip: this.l('Complete Tax'),
                field: 'completeTax',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.completeTax, 0,false,true),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Complete Vat'),
                headerTooltip: this.l('Complete Vat'),
                field: 'completeVat',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.completeVat, 0,false,true),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Tax Note'),
                headerTooltip: this.l('Tax Note'),
                field: 'taxNote',
                flex: 1
            },

        ];
        this.preCustomerColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsPreCustomer.pageSize * (this.paginationParamsPreCustomer.pageNum - 1),
                cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Id'),
                headerTooltip: this.l('Id'),
                field: 'id',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Pre No Group'),
                headerTooltip: this.l('Pre No Group'),
                field: 'preNoGroup',
                flex: 1
            },
            {
                headerName: this.l('Invoice List'),
                headerTooltip: this.l('Invoice List'),
                field: 'invoiceList',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Billoflading No'),
                headerTooltip: this.l('Billoflading No'),
                field: 'billofladingNo',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Tax (USD)'),
                headerTooltip: this.l('Tax (USD)'),
                field: 'tax',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Vat (USD)'),
                headerTooltip: this.l('Vat (USD)'),
                field: 'vat',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.vat, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },

        ];
        this.invoiceColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsInvoice.pageSize * (this.paginationParamsInvoice.pageNum - 1),
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
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Freight'),
                headerTooltip: this.l('Freight'),
                field: 'freight',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Insurance'),
                headerTooltip: this.l('Insurance'),
                field: 'insurance',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Thc'),
                headerTooltip: this.l('Thc'),
                field: 'thc',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.thc, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Cept Type'),
                headerTooltip: this.l('Cept Type'),
                field: 'ceptType',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Tax'),
                headerTooltip: this.l('Tax'),
                field: 'tax',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Vat'),
                headerTooltip: this.l('Vat'),
                field: 'vat',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vat, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Tax Rate'),
                headerTooltip: this.l('Tax Rate'),
                field: 'taxRate',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.taxRate, 2),
                flex: 1,
                type: 'rightAligned'
            },
            {

                headerName: this.l('Vat Rate'),
                headerTooltip: this.l('Vat Rate'),
                field: 'vatRate',
                comparator: this._formStoringService.decimalComparator,
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.vatRate, 2),
                flex: 1,
                type: 'rightAligned'
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
        this.paginationParamsDeclare = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsPreCustomer = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsInvoice = { pageNum: 1, pageSize: 500, totalCount: 0 };
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
    autoSizeAllInvoice() {
        const allColumnIds: string[] = [];
        this.dataParamsInvoice.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsInvoice.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.dataParamsInvoice.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
            this.autoSizeAllInvoice();
        }, 1000)
    }

    searchDatas(): void {
        //this.isLoading = true;
        this._service.getAllCustomsDeclare(
            this.customsDeclareNo,
            this._dateTimeService.convertToDatetime(this.declareDate),
            this.billofladingNo,
            this.invoiceNo,
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0){
                    this.rowDataDeclare = [];
                    this.rowDataInvoice = [];
                    this.rowDataInvoiceDetail = [];
                    this.rowDataPreCustomer = [];
                    this.paginationParamsPreCustomer.totalCount = result.totalCount;
                    this.paginationParamsPreCustomer.totalPage = ceil(result.totalCount / (this.paginationParamsPreCustomer.pageSize ?? 0));
                    this.paginationParamsInvoice.totalCount = result.totalCount;
                    this.paginationParamsInvoice.totalPage = ceil(result.totalCount / (this.paginationParamsInvoice.pageSize ?? 0));
                }
                else {
                    this.searchDatasPreCustoms(
                        result.items[0].id,
                        result.items[0].billofladingNo
                    );
                }
                this.rowDataDeclare = result.items;

                this.paginationParamsDeclare.totalCount = result.totalCount;
                this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
                this.isLoading = false;
            });
    }
    searchDatasPreCustoms(CustomsDeclareId, BillofladingNo): void {

        //this.isLoading = true;
        this._service.getInvCkdPreCustomsList(
            CustomsDeclareId,
            BillofladingNo,
            '',
            this.paginationParamsPreCustomer.skipCount,
            this.paginationParamsPreCustomer.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsPreCustomer!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataPreCustomer = [];
                    this.isLoading = false;
                }
                else {
                    this.searchDatasInvoice(result.items[0].id);
                }
                this.paginationParamsPreCustomer.totalCount = result.totalCount;
                this.rowDataPreCustomer = result.items;
                this.paginationParamsPreCustomer.totalPage = ceil(result.totalCount / (this.paginationParamsPreCustomer.pageSize ?? 0));
                this.resetGridView();
                // this.isLoading = false;
            });
    }
    searchDatasInvoice(preCustomsId): void {
        this.isLoading = true;
        this.preCustomsId = preCustomsId;
        this._service.getInvoice(
            preCustomsId,
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        )
            .pipe(finalize(() => (this.dataParamsInvoice!.api)))
            .subscribe((result) => {
                this.paginationParamsInvoice.totalCount = result.totalCount;
                this.rowDataInvoice = result.items;
                this.paginationParamsInvoice.totalPage = ceil(result.totalCount / (this.paginationParamsInvoice.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });

    }

    clearTextSearch() {
        this.ordertypeCode = '';
        this.customsDeclareNo = '',
            this.declareDate = '',
            this.billofladingNo = '',
            this.invoiceNo = '',
            this.ckdPio = 'C',
            this.searchDatas();
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

    getAllCustomsDeclare(paginationParams?: PaginationParamsModel) {
        return this._service.getAllCustomsDeclare(
            this.customsDeclareNo,
            this.declareDate,
            this.billofladingNo,
            this.invoiceNo,
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        );
    }

    getInvCkdPreCustomsList(paginationParams?: PaginationParamsModel) {
        return this._service.getInvCkdPreCustomsList(
            this.selectedRow.id, // customesdecraid
            this.selectedRow.billofladingNo,
            '',
            this.paginationParamsPreCustomer.skipCount,
            this.paginationParamsPreCustomer.pageSize
        );
    }
    getInvoice(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoice(
            this.selectedRowPreCustomer.id, //this.preCustomsId
            '',
            this.paginationParamsInvoice.skipCount,
            this.paginationParamsInvoice.pageSize
        );
    }



    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParamsDeclare = paginationParams;
        this.paginationParamsDeclare.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getAllCustomsDeclare(this.paginationParamsDeclare).subscribe((result) => {
            this.paginationParamsDeclare.totalCount = result.totalCount;
            this.rowDataDeclare = result.items;
            this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    changePagePreCustomer(paginationParams) {
        // this.isLoading = true;
        this.paginationParamsPreCustomer = paginationParams;
        this.paginationParamsPreCustomer.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getInvCkdPreCustomsList(this.paginationParamsInvoice).subscribe((result) => {
            this.paginationParamsPreCustomer.totalCount = result.totalCount;
            this.rowDataPreCustomer = result.items;
            this.paginationParamsPreCustomer.totalPage = ceil(result.totalCount / (this.paginationParamsPreCustomer.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }
    changePageInvoice(paginationParams) {
        this.paginationParamsInvoice = paginationParams;
        this.paginationParamsInvoice.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getInvoice(this.paginationParamsInvoice).subscribe((result) => {
            this.paginationParamsInvoice.totalCount = result.totalCount;
            this.rowDataInvoice = result.items;
            this.paginationParamsInvoice.totalPage = ceil(result.totalCount / (this.paginationParamsInvoice.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }
    // truyền 1 parameter là id xuống grid 2
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdCustomsDeclareDto[] } }) {

        const selectedTable1 = params.api.getSelectedRows()[0];
        if (selectedTable1) {
            this.searchDatasPreCustoms(selectedTable1.id, selectedTable1.billofladingNo);
        }
        this.selectedRow = Object.assign({}, selectedTable1);

    }
    // truyền 1 parameter là bill no xuống grid 3
    onChangeRowSelectionPreCustomer(params: { api: { getSelectedRows: () => PreCustomerDto[] } }) {
        const selectedTable2 = params.api.getSelectedRows()[0];
        this.selectedRowPreCustomer = Object.assign({}, selectedTable2);
    }
    onChangeRowSelectionInvoice(params: { api: { getSelectedRows: () => InVoiceListDto[] } }) {
        this.saveSelectedRowInVoiceList = params.api.getSelectedRows()[0] ?? new InVoiceListDto();
        this.selectedRowInVoiceList = Object.assign({}, this.selectedRowInVoiceList);
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsDeclare.pageSize);
        this.paginationParamsDeclare.skipCount =
            ((this.paginationParamsDeclare.pageNum ?? 1) - 1) * (this.paginationParamsDeclare.pageSize ?? 0);
        this.paginationParamsDeclare.pageSize = this.paginationParamsDeclare.pageSize;
        this.getAllCustomsDeclare(this.paginationParamsDeclare)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsDeclare.totalCount = result.totalCount;
                this.rowDataDeclare = result.items ?? [];
                this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    callBackDataGridPreCustoms(params: GridParams) {
        this.dataParamsPreCustomer = params;
        this.resetGridView();
    }

    callBackDataGridInvoice(params: GridParams) {
        this.dataParamsInvoice = params;
        this.dataParamsInvoice.api.paginationSetPageSize(
            this.paginationParamsInvoice.pageSize
        );
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdCustomerDeclareToExcel(
            this.customsDeclareNo,
            this.declareDate,
            this.billofladingNo,
            this.invoiceNo,
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
    exportPreCustomsToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdPreCustomsToExcel(
            this.selectedRow.id, // = CustomsDeclareId
            this.selectedRow.billofladingNo,
            '',
            this.paginationParamsPreCustomer.skipCount,
            this.paginationParamsPreCustomer.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
    exportToInvoiceExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdInvoiceToExcel(
            this.selectedRowPreCustomer.id, // this.preCustomsId
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

