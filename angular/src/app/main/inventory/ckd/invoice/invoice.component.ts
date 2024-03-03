import {
    ColDef,
    GridApi,
    MasterDetailModule,
    ModuleRegistry,
} from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    GridParams,
    PaginationParamsModel,
    FrameworkComponent,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    InvCkdInvoiceDto,
    InvCkdInvoiceDetailsDto,
    InvCkdInvoiceServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
ModuleRegistry.registerModules([MasterDetailModule]);
@Component({
    templateUrl: './invoice.component.html',
})
export class InvoiceComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    columnDefs: CustomColDef[] = [];

    detailCellRendererParams: any;
    paginationParamInvoice: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamInvoiceDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    //
    invoiceColDefs: any;
    invoiceDetailsColDefs: any;

    selectedRowInvoice: InvCkdInvoiceDto = new InvCkdInvoiceDto();
    // saveSelectedRow: InvCkdInvoiceDto = new InvCkdInvoiceDto();
    // invoiceDto: InvCkdInvoiceDto = new InvCkdInvoiceDto();
    // invoicedetailsDto: InvCkdInvoiceDetailsDto = new InvCkdInvoiceDetailsDto();

    selectedRowInvoiceDetail: InvCkdInvoiceDetailsDto = new InvCkdInvoiceDetailsDto();

    datas: InvCkdInvoiceDto = new InvCkdInvoiceDto();
    pending: string = '';
    disable: boolean = false;
    pendingdetail: string = '';
    disabledetail: boolean = false;
    isLoading: boolean = false;
    dataParamsInvoice: GridParams | undefined;
    dataParamsInvoiceDetail: GridParams | undefined;
    rowDataInvoice: any[] = [];
    rowDataInvoiceDetails: InvCkdInvoiceDetailsDto[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;


    changedRecordsInvoice: number[] = [];
    changedRecordsInvoiceDetails: number[] = [];
    invoiceNo: string = '';
    invocieDateFrom: any;
    invoiceDate: any;
    invocieDateTo: any;
    supplierNo: string = '';
    billNo: string = '';
    shipmentNo: string = '';
    oderTypeCode: string = '';
    goodsTypeCode: string = '';
    isActive: string = '';
    ordertypeCode: string = '';
    partNo: string = '';
    qty: string = '';
    fixLot: string = '';
    caseNo: string = '';
    moduleNo: string = '';
    containerNo: string = '';
    renBan: string = '';
    invoiceId: number;
    invoiceDetailId: number;
    fn: CommonFunction = new CommonFunction();
    billDateFrom: any;
    billDateTo: any;
    ckdPio = 'C';
    isRow: boolean = true;

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

    _pagesizedetails = 1000000000;

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
        private _service: InvCkdInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
        private _formStoringService: FormStoringService,
    ) {
        super(injector);

        this.invoiceColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamInvoice.pageSize * (this.paginationParamInvoice.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
                pinned: true,
            },
            {
                headerName: this.l('Invoice No'),
                headerTooltip: this.l('Invoice No'),


                field: 'invoiceNo',
                flex: 1,
                pinned: true,
            },
            {
                headerName: this.l('Invoice Date'),
                headerTooltip: this.l('Invoice Date'),
                field: 'invoiceDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1,

            },
            {
                headerName: this.l('Bill No'),
                headerTooltip: this.l('Bill No'),
                field: 'billNo',
                flex: 1,
            },
            {
                headerName: this.l('Bill Date'),
                headerTooltip: this.l('Bill Date'),
                field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Shipment No'),
                headerTooltip: this.l('Shipment No'),
                field: 'shipmentNo',
                flex: 1,
            },
            {
                headerName: this.l('Order Type Code'),
                headerTooltip: this.l('Order Type Code'),
                field: 'ordertypeCode',
                flex: 1,
            },
            {
                headerName: this.l('Goods Type Code'),
                headerTooltip: this.l('Goods Type Code'),
                field: 'goodstypeCode',
                flex: 1,
            },
            {
                headerName: this.l('Invoice ParentId'),
                headerTooltip: this.l('Invoice ParentId'),
                field: 'invoiceParentNo',
                flex: 1,
                type: 'rightAligned'

            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                type: 'rightAligned',
                aggFunc: this.SumA,
            },
            {
                headerName: this.l('Freight'),
                headerTooltip: this.l('Freight'),
                field: 'freightTotal',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.freightTotal, 4),
                type: 'rightAligned',
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Insurance'),
                headerTooltip: this.l('Insurance'),
                field: 'insuranceTotal',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insuranceTotal, 4),
                type: 'rightAligned'
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                type: 'rightAligned'
            },
            {
                headerName: this.l('Thc'),
                headerTooltip: this.l('Thc'),
                field: 'thcTotal',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thcTotal, 4),
                type: 'rightAligned',
            },

            {
                headerName: this.l('Net Weight'),
                headerTooltip: this.l('Net Weight'),
                field: 'netWeight',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.netWeight, 4),
                type: 'rightAligned'
            },
            {
                headerName: this.l('Gross Weight'),
                headerTooltip: this.l('Gross Weight'),
                field: 'grossWeight',
                flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.grossWeight, 4),
                type: 'rightAligned'
            },
            {
                headerName: this.l('Cept Type'),
                headerTooltip: this.l('Cept Type'),
                field: 'ceptType',
                flex: 1,
            },
            {
                headerName: this.l('Currency'),
                headerTooltip: this.l('Currency'),
                field: 'currency',
                flex: 1,
            },
            {
                headerName: this.l('Remarks'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',
                flex: 1,
            },
            {
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'quantity',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Flag'),
                headerTooltip: this.l('Flag'),
                field: 'flag',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Freezed'),
                headerTooltip: this.l('Freezed'),
                field: 'freezed',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Source Type'),
                headerTooltip: this.l('Source Type'),
                field: 'sourceType',
                flex: 1,
            },
            {
                headerName: this.l('Status Err'),
                headerTooltip: this.l('Status Err'),
                field: 'statusErr',
                flex: 1,
            },
            {
                headerName: this.l('Last Ordertype'),
                headerTooltip: this.l('Last Ordertype'),
                field: 'lastOrdertype',
                flex: 1,
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1,
            },
            // {
            //     headerName: this.l('Fob Vn'),
            //     headerTooltip: this.l('Fob Vn'),
            //     field: 'fobVn',
            //     flex: 1,
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fobVn, 4),
            //     type: 'rightAligned'
            // },
            // {
            //     headerName: this.l('Freight Vn'),
            //     headerTooltip: this.l('Freight Vn'),
            //     field: 'freightTotalVn',
            //     flex: 1,
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.freightTotalVn, 4),
            //     type: 'rightAligned'
            // },
            // {
            //     headerName: this.l('Insurance Vn'),
            //     headerTooltip: this.l('Insurance Vn'),
            //     field: 'insuranceTotalVn',
            //     flex: 1,
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insuranceTotalVn, 4),
            //     type: 'rightAligned'
            // },
            // {
            //     headerName: this.l('Thc Vn'),
            //     headerTooltip: this.l('Thc Vn'),
            //     field: 'thcTotalVn',
            //     flex: 1,
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thcTotalVn, 4),
            //     type: 'rightAligned'
            // },
            // {
            //     headerName: this.l('Cif Vn'),
            //     headerTooltip: this.l('Cif Vn'),
            //     field: 'cifVn',
            //     flex: 1,
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.cifVn, 4),
            //     type: 'rightAligned'
            // },
            // {
            //     headerName: this.l('Spotax'),
            //     headerTooltip: this.l('Spotax'),
            //     field: 'spotax',
            //     flex: 1,
            // },
            {
                headerName: this.l('PeriodId'),
                headerTooltip: this.l('PeriodId'),
                field: 'periodId',
                flex: 1,
                type: 'rightAligned'
            },
        ];

        this.invoiceDetailsColDefs = [
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                pinned: true,
            },
            {
                headerName: this.l('Qty'),
                headerTooltip: this.l('Qty'),
                field: 'usageQty',
                type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.usageQty),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Fix Lot'),
                headerTooltip: this.l('Fix Lot'),
                field: 'fixlot',
            },
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
            },
            {
                headerName: this.l('Module No'),
                headerTooltip: this.l('Module No'),
                field: 'moduleNo',
            },
            {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
            },
            {
                headerName: this.l('RenBan'),
                headerTooltip: this.l('RenBan'),
                field: 'renban',
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
            },

            {
                headerName: this.l('Fob'), headerTooltip: this.l('Fob'), field: 'fob', type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Freight'),
                headerTooltip: this.l('Freight'),
                field: 'freight',
                type: 'rightAligned',
                valueGetter: (params) => (params.data?.freight != null ? params.data?.freight : 0),
                aggFunc: this.SumA
            },

            {
                headerName: this.l('Insurance'),
                headerTooltip: this.l('Insurance'),
                field: 'insurance',
                type: 'rightAligned',
                valueGetter: (params) => (params.data?.insurance != null ? params.data?.insurance : 0),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Fi'), headerTooltip: this.l('Fi'), field: 'fi', type: 'rightAligned',
                valueGetter: (params) => (params.data?.fi != null ? params.data?.fi : 0),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Cif'), headerTooltip: this.l('Cif'), field: 'cif', type: 'rightAligned',
                cellRenderer: (params) => (params.data?.cif != null ? params.data?.cif : 0),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Thc'),
                headerTooltip: this.l('Thc'),
                field: 'thc',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Tax'), headerTooltip: this.l('Tax'), field: 'tax', type: 'rightAligned',
                valueGetter: (params) => (params.data?.tax != null ? params.data?.tax : 0),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Vat'), headerTooltip: this.l('Vat'), field: 'vat', type: 'rightAligned',
                valueGetter: (params) => (params.data?.vat != null ? params.data?.vat : 0),
                aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax Rate'), headerTooltip: this.l('Tax Rate'), field: 'taxRate', type: 'rightAligned',
                //valueGetter: (params) => (params.data?.taxRate != null ? params.data?.taxRate : 0),
            },
            {
                headerName: this.l('Vat Rate'), headerTooltip: this.l('Vat Rate'), field: 'vatRate', type: 'rightAligned',
                //valueGetter: (params) => (params.data?.vatRate != null ? params.data?.vatRate : 0),
            },
            {
                headerName: this.l('Inland'), headerTooltip: this.l('Inland'), field: 'inland', type: 'rightAligned',
                //valueGetter: (params) => (params.data?.inland != null ? params.data?.inland : 0),
            },

            {
                headerName: this.l('Cept Type'), headerTooltip: this.l('Cept Type'), field: 'ceptType',
            },
            {
                headerName: this.l('Carfamily Code'), headerTooltip: this.l('Carfamily Code'), field: 'carfamilyCode',
            },
            {
                headerName: this.l('Part Net Weight'), headerTooltip: this.l('Part Net Weight'), field: 'partNetWeight', type: 'rightAligned'
            },
            {
                headerName: this.l('Order No'), headerTooltip: this.l('Order No'), field: 'orderNo',
            },
            {
                headerName: this.l('Firmpackingmonth'),
                headerTooltip: this.l('Firmpackingmonth'),
                field: 'firmpackingmonth',
                valueGetter: (params) => this.pipe.transform(params.data?.firmpackingmonth, 'dd/MM/yyyy'),
                flex: 1,
                //cellClass: ['text-center'],
            },
            {
                headerName: this.l('Reexport Code'),
                headerTooltip: this.l('Reexport Code'),
                field: 'reexportCode',
                type: 'rightAligned'
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                type: 'rightAligned'
            },
            // {
            //     headerName: this.l('FobVn'), headerTooltip: this.l('FobVn'), field: 'fobVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fobVn != null ? params.data?.fobVn :0, 4),
            //     aggFunc: this.SumA
            // },
            // {
            //     headerName: this.l('FreightVn'), headerTooltip: this.l('FreightVn'), field: 'freightVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.freightVn != null ? params.data?.freightVn : 0, 4),
            //     aggFunc: this.SumA
            // },
            // {
            //     headerName: this.l('InsuranceVn'), headerTooltip: this.l('InsuranceVn'), field: 'insuranceVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insuranceVn, 0),
            // },
            // {
            //     headerName: this.l('FiVn'), headerTooltip: this.l('FiVn'), field: 'fiVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fiVn, 0),
            // },
            // {
            //     headerName: this.l('ThcVn'), headerTooltip: this.l('ThcVn'), field: 'thcVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thcVn, 0),
            // },
            // {
            //     headerName: this.l('CifVn'), headerTooltip: this.l('CifVn'), field: 'cifVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.cifVn, 0),
            // },
            // {
            //     headerName: this.l('TaxVn'), headerTooltip: this.l('TaxVn'), field: 'taxVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.taxVn, 0),
            //     aggFunc: this.SumA
            // },
            // {
            //     headerName: this.l('VatVn'), headerTooltip: this.l('VatVn'), field: 'vatVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.vatVn, 0),
            //     aggFunc: this.SumA
            // },
            // {
            //     headerName: this.l('InlandVn'), headerTooltip: this.l('InlandVn'), field: 'inlandVn', type: 'rightAligned',
            //     valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.inlandVn, 0),
            // },
            {
                headerName: this.l('InvoiceId'),
                headerTooltip: this.l('InvoiceId'),
                field: 'invoiceId',
                type: 'rightAligned',
            },
            {
                headerName: this.l('PeriodId'),
                headerTooltip: this.l('PeriodId'),
                field: 'periodId',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Invoice ParentId'),
                headerTooltip: this.l('Invoice ParentId'),
                field: 'invoiceParentId',
                type: 'rightAligned',
            },

            {
                headerName: this.l('Hs Code'),
                headerTooltip: this.l('Hs Code'),
                field: 'hsCode',
                type: 'rightAligned'
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
            },
            {
                headerName: this.l('Part Name VN'),
                headerTooltip: this.l('Partname Vn'),
                field: 'partnameVn',
            },
            {
                headerName: this.l('Car Name'),
                headerTooltip: this.l('Car Name'),
                field: 'carName',
            },
            {
                headerName: this.l('Pmhist Id'),
                headerTooltip: this.l('Pmhist Id'),
                field: 'pmhistId',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Ecus5TaxRate'),
                headerTooltip: this.l('Ecus5TaxRate'),
                field: 'ecus5TaxRate',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Ecus5VatRate'),
                headerTooltip: this.l('Ecus5VatRate'),
                field: 'ecus5VatRate',
                type: 'rightAligned',
            },
            {
                headerName: this.l('Ecus5HsCode '),
                headerTooltip: this.l('Ecus5HsCode '),
                field: 'ecus5HsCode ',
                type: 'rightAligned',
            },

            {
                headerName: this.l('Declare Type'),
                headerTooltip: this.l('Declare Type'),
                field: 'declareType',
            },
        ],
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
        this.paginationParamInvoice = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamInvoiceDetails = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        this.fetchChangedRecords();
        this.resetGridView();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsInvoice = result.invoice;
            this.changedRecordsInvoiceDetails = result.invoiceDetail;
        })
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

        var _SumFob = 0;
        var _SumCif = 0;
        var _SumQty = 0;
        var _SumFreight = 0;
        var _SumInsurance = 0;
        var _SumFi = 0;
        var _SumTax = 0;
        var _SumVat = 0;
        if (this.rowDataInvoiceDetails.length > 0) {
            for (let i = 0; this.rowDataInvoiceDetails[i]; i++) {
                _SumFob += Number(this.rowDataInvoiceDetails[i].fob * this.rowDataInvoiceDetails[i].usageQty);
                _SumCif += Number(this.rowDataInvoiceDetails[i].cif * this.rowDataInvoiceDetails[i].usageQty);
                _SumQty += Number(this.rowDataInvoiceDetails[i].usageQty);
                _SumFreight += Number(this.rowDataInvoiceDetails[i].freight * this.rowDataInvoiceDetails[i].usageQty);
                _SumInsurance += Number(this.rowDataInvoiceDetails[i].insurance * this.rowDataInvoiceDetails[i].usageQty);
                _SumFi += Number(this.rowDataInvoiceDetails[i].fi * this.rowDataInvoiceDetails[i].usageQty);
                _SumTax += Number(this.rowDataInvoiceDetails[i].tax * this.rowDataInvoiceDetails[i].usageQty);
                _SumVat += Number(this.rowDataInvoiceDetails[i].vat * this.rowDataInvoiceDetails[i].usageQty);
            }
        }

        _SumFob = this._fm.decimal_math(_SumFob, 4, 'round');
        _SumCif = this._fm.formatMoney_decimal_bk(_SumCif, true,4);
        _SumFreight = this._fm.formatMoney_decimal_bk(_SumFreight, true,4);
        _SumInsurance = this._fm.formatMoney_decimal_bk(_SumInsurance, true,4);
        _SumFi = this._fm.formatMoney_decimal_bk(_SumFi, true, 4);
        _SumTax = this._fm.formatMoney_decimal_bk(_SumTax, true, 4);
        _SumVat = this._fm.formatMoney_decimal_bk(_SumVat, true, 4);

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                fob: _SumFob,
                usageQty: _SumQty,
                freight: _SumFreight,
                insurance: _SumInsurance,
                fi: _SumFi,
                tax: _SumTax,
                cif: _SumCif,
                vat: _SumVat,
            });
        }

        return result;
    }

    searchDatasInvoice(): void {
        this.isLoading = true;
        this._service.getInvoiceSearch(
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invocieDateFrom),
            this._dateTimeService.convertToDatetime(this.invocieDateTo),
            this.billNo,
            this.shipmentNo,
            this.containerNo,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.supplierNo,
            this.ordertypeCode,
            this.ckdPio,
            '',
            this.paginationParamInvoice.skipCount,
            this.paginationParamInvoice.pageSize,
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsInvoice!.api)))
            .subscribe((result) => {
                // if (result.totalCount == 0){
                //     this.rowDataInvoice = [];
                //     this.rowDataInvoiceDetails = [];
                //     this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(null);
                //     this.paginationParamInvoiceDetails.totalCount = result.totalCount;
                //     this.paginationParamInvoiceDetails.totalPage = ceil(result.totalCount / (this.paginationParamInvoiceDetails.pageSize ?? 0));
                //     this.isLoading = false;
                //     this.isRow = false;
                // }
                // else {
                //     this.rowDataInvoice = result.items;
                //     this.paginationParamInvoice.totalCount = result.totalCount;
                //     this.paginationParamInvoice.totalPage = ceil(result.totalCount / (this.paginationParamInvoice.pageSize ?? 0));
                //     this.resetGridView();
                //     this.searchDataInvoiceDetails(result.items[0].id);
                //     this.isRow = false;
                // }
                if (result.totalCount == 0){
                    this.rowDataInvoiceDetails = [];
                    this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(null);
                }
                this.rowDataInvoice = result.items;
                this.paginationParamInvoice.totalCount = result.totalCount;
                this.paginationParamInvoice.totalPage = ceil(result.totalCount / (this.paginationParamInvoice.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    autoSizeAll() {
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
            this.dataParamsInvoice.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 100)
    }


    autoSizeAllInvoiceDetail() {
        const allColumnIds: string[] = [];
        this.dataParamsInvoiceDetail.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsInvoiceDetail.columnApi!.autoSizeColumns(allColumnIds);
    }
    resetGridViewInvoiceDetail() {
        setTimeout(() => {
            this.dataParamsInvoiceDetail.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAllInvoiceDetail();
        }, 100)
    }

    clearTextSearch() {
        this.invoiceNo = '';
        this.invocieDateFrom = '';
        this.invocieDateTo = '';
        this.billNo = '';
        this.shipmentNo = '';
        this.containerNo = '';
        this.billDateFrom = '';
        this.billDateTo = '';
        this.supplierNo = '';
        this.ordertypeCode = '';
        this.ckdPio = 'C';
        this.isRow = false;
        this.paginationParamInvoiceDetails = { pageNum: 1, pageSize: this._pagesizedetails, totalCount: 0 };
        this.searchDatasInvoice();
    }

    getDataModels(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoiceSearch(
            this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invocieDateFrom),
            this._dateTimeService.convertToDatetime(this.invocieDateTo),
            this.billNo,
            this.shipmentNo,
            this.containerNo,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.supplierNo,
            this.oderTypeCode,
            this.ckdPio,
            '',
            this.paginationParamInvoice.skipCount,
            this.paginationParamInvoice.pageSize,
        );
    }

    onChangeRowSelectionInvoice(params: { api: { getSelectedRows: () => InvCkdInvoiceDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.invoiceId = selected.id;
            this.paginationParamInvoiceDetails.pageNum = 1;
            this.paginationParamInvoiceDetails.skipCount = 0;
            this.searchDataInvoiceDetails(selected.id);
            this.isRow = false;
          //  this.oderTypeCode = selected.ordertypeCode;
        }
        this.selectedRowInvoice = Object.assign({}, selected);
    }

    onChangeRowSelectionInvoiceDetails(params: { api: { getSelectedRows: () => InvCkdInvoiceDetailsDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectedRowInvoiceDetail = Object.assign({}, selected);
        this.invoiceDetailId = this.selectedRowInvoiceDetail.id;
    }

    changePageInvoice(paginationParams) {
        this.isLoading = true;
        this.paginationParamInvoice = paginationParams;
        this.paginationParamInvoice.skipCount = ((paginationParams.pageNum - 1) * paginationParams.pageSize) < 0 ? 0 : ((paginationParams.pageNum - 1) * paginationParams.pageSize);
        this.getDataModels(this.paginationParamInvoice)
        .pipe(finalize(() => {
            this.gridTableService.selectFirstRow(this.dataParamsInvoice!.api);
        }))
        .subscribe((result) => {
            this.paginationParamInvoice.totalCount = result.totalCount;
            this.rowDataInvoice = result.items ?? [];
            this.paginationParamInvoice.totalPage = ceil(result.totalCount / (this.paginationParamInvoice.pageSize ?? 0));
            this.isLoading = false;
            this.isRow = false;
            this.resetGridView();
        });
    }

    callBackDataGridInvoice(params: GridParams) {
        this.isLoading = true;
        this.dataParamsInvoice = params;   // apply cho cả auto resize
        // params.api.paginationSetPageSize(this.paginationParamInvoice.pageSize);
        this.paginationParamInvoice.skipCount =
            ((this.paginationParamInvoice.pageNum ?? 1) - 1) * (this.paginationParamInvoice.pageSize ?? 0);
        // this.paginationParamInvoice.pageSize = this.paginationParamInvoice.pageSize;
        this.getDataModels(this.paginationParamInvoice)

            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParamsInvoice!.api);
            }))
            .subscribe((result) => {
                if (result.totalCount == 0){
                    this.rowDataInvoiceDetails = [];
                    this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(null);
                }
                this.paginationParamInvoice.totalCount = result.totalCount;
                this.rowDataInvoice = result.items ?? [];
                this.paginationParamInvoice.totalPage = ceil(result.totalCount / (this.paginationParamInvoice.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    // InvoiceDetails
    searchDataInvoiceDetails(invoiceId): void {
        this.isLoading = true;
        this._service.getInvoiceDetailsGetbyinvoiceid(
            invoiceId,
            '',
            this.paginationParamInvoiceDetails.skipCount,
            this.paginationParamInvoiceDetails.pageSize
        )
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParamsInvoiceDetail!.api);
                var rows = this.createRow(1);
                // create row total
                this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                //console.log("search data");
            }
            )).subscribe((result) => {
                this.paginationParamInvoiceDetails.totalCount = result.totalCount;
                this.rowDataInvoiceDetails = result.items;
                this.paginationParamInvoiceDetails.totalPage = ceil(result.totalCount / (this.paginationParamInvoiceDetails.pageSize ?? 0));
                this.resetGridViewInvoiceDetail();
                this.isLoading = false;
                this.isRow = false;
            });
    }

    getDataInvoiceDetails(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoiceDetailsGetbyinvoiceid(
            this.invoiceId,
            '',
            this.paginationParamInvoiceDetails.skipCount,
            this.paginationParamInvoiceDetails.pageSize
        );
    }

    changePageInvoiceDetails(paginationParams) {
        // this.isLoading = true;
        this.paginationParamInvoiceDetails = paginationParams;
        this.paginationParamInvoiceDetails.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataInvoiceDetails(this.paginationParamInvoiceDetails)
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParamsInvoiceDetail!.api);
                // var rows = this.createRow(1);
                // this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows);
            }))
            .subscribe((result) => {
                this.rowDataInvoiceDetails = result.items;
                this.paginationParamInvoiceDetails.totalCount = result.totalCount;
                this.paginationParamInvoiceDetails.totalPage = ceil(result.totalCount / (this.paginationParamInvoiceDetails.pageSize ?? 0));
                // this.isLoading = false;
                this.resetGridViewInvoiceDetail();
            });
        this._pagesizedetails = this.paginationParamInvoiceDetails.pageSize;
    }

    callBackDataGridInvoiceDetails(params: GridParams) {
        // this.isLoading = true;
        this.dataParamsInvoiceDetail = params; // dành cho autoresize
        params.api.paginationSetPageSize(this.paginationParamInvoiceDetails.pageSize);
        this.paginationParamInvoiceDetails.skipCount =
            ((this.paginationParamInvoiceDetails.pageNum ?? 1) - 1) * (this.paginationParamInvoiceDetails.pageSize ?? 0);
        this.paginationParamInvoiceDetails.pageSize = this.paginationParamInvoiceDetails.pageSize;
        this.getDataInvoiceDetails(this.paginationParamInvoiceDetails)
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParamsInvoiceDetail!.api);
                var rows = this.createRow(1);
                this.dataParamsInvoiceDetail!.api.setPinnedBottomRowData(rows);
            }))
            .subscribe((result) => {
                this.paginationParamInvoiceDetails.totalCount = result.totalCount;
                this.rowDataInvoiceDetails = result.items ?? [];
                this.paginationParamInvoiceDetails.totalPage = ceil(result.totalCount / (this.paginationParamInvoiceDetails.pageSize ?? 0));
                // this.isLoading = false;
                this.resetGridViewInvoiceDetail();
            });


    }

    exportToExcelInvoice(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvoiceExportExcel
            (
                this.invoiceNo,
                this._dateTimeService.convertToDatetime(this.invocieDateFrom),
                this._dateTimeService.convertToDatetime(this.invocieDateTo),
                this.billNo,
                this.shipmentNo,
                this.containerNo,
                this._dateTimeService.convertToDatetime(this.billDateFrom),
                this._dateTimeService.convertToDatetime(this.billDateTo),
                this.supplierNo,
                this.ordertypeCode,
                this.ckdPio,
                '',
                this.paginationParamInvoice.skipCount,
                this.paginationParamInvoice.pageSize,

            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    //
    exportToExcelInvoiceDetails(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvoiceDetailExportExcel
            (
                this.invoiceId,
                '',
                this.paginationParamInvoiceDetails.skipCount,
                this.paginationParamInvoiceDetails.pageSize
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    //
    exportToExcelInvoiceCustoms(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvoiceCustomsExportExcel
            (
                this.invoiceNo,
                this._dateTimeService.convertToDatetime(this.invocieDateFrom),
                this._dateTimeService.convertToDatetime(this.invocieDateTo),
                this.billNo,
                this.shipmentNo,
                this.containerNo,
                this._dateTimeService.convertToDatetime(this.billDateFrom),
                this._dateTimeService.convertToDatetime(this.billDateTo),
                this.supplierNo,
                this.ordertypeCode,
                this.ckdPio,
                '',
                this.paginationParamInvoice.skipCount,
                this.paginationParamInvoice.pageSize,

            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}


