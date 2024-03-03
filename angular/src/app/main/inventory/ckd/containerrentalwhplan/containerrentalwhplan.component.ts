import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerPartRepackInput, InvCkdContainerRentalWHPlanDetails, InvCkdContainerRentalWHPlanDto, InvCkdContainerRentalWHPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditContainerRentalWHPlanModalComponent } from './create-or-edit-containerrentalwhplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { ImportContainerRentalWHPlanComponent } from './import-containerrentalwhplan-modal.component';
import { truncate } from 'fs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AbpSessionService } from 'abp-ng2-module';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { NumericEditor } from '@app/shared/common/numeric-cell-editor/NumericEditor';
import { ImportRepackComponent } from './import-repack-modal.component';

@Component({
    templateUrl: './containerrentalwhplan.component.html',
    styleUrls: ['./containerrentalwhplan.component.less'],
})
export class ContainerRentalWHPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalContainerRentalWHPlan', { static: true }) createOrEditModalContainerRentalWHPlan: | CreateOrEditContainerRentalWHPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportContainerRentalWHPlanComponent | undefined;
    @ViewChild('importRepackExcelModal', { static: true }) importRepackExcelModal: | ImportRepackComponent | undefined;

    selecttoConfirm: InvCkdContainerRentalWHPlanDto[] = [];
    defaultColDefs: CustomColDef[] = [];
    columnDefs: CustomColDef[] = [];
    columnDetailsDefs: CustomColDef[] = [];
    columnRepackDefs: CustomColDef[] = [];
    listChangeds: InvCkdContainerPartRepackInput[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationDetailsParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationRepackParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    listIdStatus = '';
    arrIdStatus: InvCkdContainerRentalWHPlanDto[] = [];

    selectedRow: InvCkdContainerRentalWHPlanDto = new InvCkdContainerRentalWHPlanDto();
    saveSelectedRow: InvCkdContainerRentalWHPlanDto = new InvCkdContainerRentalWHPlanDto();

    selectedRowDetails: InvCkdContainerRentalWHPlanDetails = new InvCkdContainerRentalWHPlanDetails();
    saveSelectedRowDetails: InvCkdContainerRentalWHPlanDetails = new InvCkdContainerRentalWHPlanDetails();

    selectedRowRepack: InvCkdContainerRentalWHPlanDetails = new InvCkdContainerRentalWHPlanDetails();
    saveSelectedRowRepack: InvCkdContainerRentalWHPlanDetails = new InvCkdContainerRentalWHPlanDetails();


    datas: InvCkdContainerRentalWHPlanDto = new InvCkdContainerRentalWHPlanDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    dataParamsRepack: GridParams | undefined;
    rowData: any[] = [];
    rowDetailsData: any[] = [];
    rowRepackData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;


    containerNo: string = '';
    renban: string = '';
    requestDate
    requestTime
    invoiceNo: string = '';
    billofladingNo: string = '';
    supplierNo: string = '';
    sealNo: string = '';
    requestDateFrom;
    requestDateTo
    listcaseNo: string = '';
    listLotNo: string = '';
    cdDate
    lotNo
    moduleCaseNo
    partNo
    devanningDate
    devanningTime
    actualDevanningDate
    gateInPlanTime
    gateInActualDateTime
    transport: string = '';
    plateId: string = '';
    status: string = '';
    isActive: string = '';
    isShowConfirm: boolean = true;
    activeTab = 1;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        suppressRowClickSelection: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };
    RestrictList = [
        { key: 'R', value: "REQUESTED" },
        { key: 'P', value: "PENDING" },
        { key: 'F', value: "CONFIRM" },
        { key: 'C', value: "CANCEL" },
    ];

    constructor(
        injector: Injector,
        private _service: InvCkdContainerRentalWHPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
        private _sessionService: AbpSessionService
    ) {
        super(injector);
        this.columnDefs = [
            {
                headerName: "",
                headerTooltip: "",
                field: "checked",
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true,
                pinned: true,
                width: 37
            },
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 50, pinned: true },
            //{ headerName: this.l('ID'), headerTooltip: this.l('ID'),  field: 'id', flex: 1},

            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',

                cellRenderer: 'agSelectRendererComponent',
                list: this.RestrictList,
                cellClass: ['RendererCombobox', 'text-center'],
                pinned: true
            },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', pinned: true },
            { headerName: this.l('WH Code'), headerTooltip: this.l('WH Code'), field: 'wHCode', pinned: true },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', pinned: true },
            { headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', pinned: true, valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy') },
            { headerName: this.l('Request Time'), headerTooltip: this.l('Request Time'), field: 'requestTime', pinned: true, },
            { headerName: this.l('Devanning Date'), headerTooltip: this.l('Devanning Date'), field: 'devanningDate', valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy') },
            { headerName: this.l('Devanning Time'), headerTooltip: this.l('Devanning Time'), field: 'devanningTime', },
           // { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', },
            //{ headerName: this.l('Module Case No'), headerTooltip: this.l('Module Case No'), field: 'moduleCaseNo', },
           // { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', },
            { headerName: this.l('Billoflading No'), headerTooltip: this.l('Billoflading No'), field: 'billofladingNo', },
            // { headerName: this.l('Custums1'), headerTooltip: this.l('Custums1'), field: 'custums1', },
            // { headerName: this.l('Custums2'), headerTooltip: this.l('Custums2'), field: 'custums2', },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', },
            { headerName: this.l('Seal No'), headerTooltip: this.l('Seal No'), field: 'sealNo', },
            { headerName: this.l('Listcase No'), headerTooltip: this.l('Listcase No'), field: 'listcaseNo', width: 250, },
            { headerName: this.l('List Lot No'), headerTooltip: this.l('List Lot No'), field: 'listLotNo', },
            { headerName: this.l('Cd Date'), headerTooltip: this.l('Cd Date'), field: 'cdDate', valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy') },

            { headerName: this.l('Actual Devanning Date'), headerTooltip: this.l('Actual Devanning Date'), field: 'actualDevanningDate', valueGetter: (params) => this.pipe.transform(params.data?.actualDevanningDate, 'dd/MM/yyyy') },
            { headerName: this.l('Gate In Plan Time'), headerTooltip: this.l('Gate In Plan Time'), field: 'gateInPlanTime', valueGetter: (params) => this.pipe.transform(params.data?.gateInPlanTime, 'dd/MM/yyyy HH:mm:ss') },
            { headerName: this.l('Gate In Actual Date Time'), headerTooltip: this.l('Gate In Actual Date Time'), field: 'gateInActualDateTime', valueGetter: (params) => this.pipe.transform(params.data?.gateInActualDateTime, 'dd/MM/yyyy HH:mm:ss') },
            { headerName: this.l('Transport'), headerTooltip: this.l('Transport'), field: 'transport', },
            { headerName: this.l('Plate Id'), headerTooltip: this.l('Plate Id'), field: 'plateId', },

            // {
            //     headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            // }
        ];

        this.columnDetailsDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, pinned: 'left'},
			{ headerName: this.l('Part No'), headerTooltip: this.l('PartNo'), field: 'partNo',pinned: 'left',
                },
			{ headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'carfamilyCode',pinned: 'left',
                },
			{ headerName: this.l('Supplier No'), headerTooltip: this.l('SupplierNo'), field: 'supplierNo', pinned: 'left',
                },
            { headerName: this.l('Part Name'), headerTooltip: this.l('PartName'), field: 'partName',
                },
            { headerName: this.l('WH Location'), headerTooltip: this.l('WH Location'), field: 'whLocation',
                },
            { headerName: this.l('Lot No'), headerTooltip: this.l('LotNo'), field: 'lotNo',
                },
			{ headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty' , type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.usageQty),},
            { headerName: this.l('Remain Qty'), headerTooltip: this.l('Remain Qty'), field: 'remainQty' , type: 'rightAligned',
                valueGetter: (params) => params.data?.remainQty !== 0 ? this._fm.formatMoney_decimal(params.data?.remainQty ?? params.data?.usageQty) : 0
                },
            {
                    headerName: this.l('Repack Qty'),
                    headerTooltip: this.l('Repack Qty'),
                    field: 'repackQty',
                    valueFormatter: (params) => {
                        let exist = this.listChangeds.find(x => x.id == params.data.id);
                        if (exist) {
                            return exist.qty;
                        } else {
                            return !params.data?.repackQty ? 0 : params.data?.repackQty;
                        }
                    },
                    flex: 1,
                    type: 'rightAligned',
                    editable: (params) => params.data.remainQty === null || params.data.remainQty > 0,
                    cellStyle: params => {
                        if (params.data.remainQty === null || params.data.remainQty > 0) {
                            return { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' };
                        }
                    },
                    cellEditor: NumericEditor,
            },
			{ headerName: this.l('Fix Lot'), headerTooltip: this.l('FixLot'), field: 'fixlot',
                },
			{ headerName: this.l('Module Case No'), headerTooltip: this.l('ModuleCaseNo'), field: 'moduleCaseNo', pinned: 'left'
                },
			{ headerName: this.l('Firm Packing Month'), headerTooltip: this.l('Firmpackingmonth'), field: 'firmpackingmonth',
                valueGetter: (params) => this.pipe.transform(params.data?.firmpackingmonth, 'dd/MM/yyyy'),
                },
            { headerName: this.l('Order No'), headerTooltip: this.l('OrderNo'), field: 'orderNo',
                },
			{ headerName: this.l('Car Name'), headerTooltip: this.l('CarName'), field: 'carName',
                },
			{ headerName: this.l('Invoice No'), headerTooltip: this.l('InvoiceNo'), field: 'invoiceNo',
                },
			{ headerName: this.l('Invoice Date'), headerTooltip: this.l('InvoiceDate'), field: 'invoiceDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Container No'), headerTooltip: this.l('ContainerNo'), field: 'containerNo',
                },
			{ headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban',
                },
			{ headerName: this.l('Bill No'), headerTooltip: this.l('BillNo'), field: 'billofladingNo',
                },
			{ headerName: this.l('Bill Date'),headerTooltip: this.l('Bill Date'),field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),flex: 1,},
			{ headerName: this.l('Shipment No'), headerTooltip: this.l('ShipmentNo'), field: 'shipmentNo',
                },
			{ headerName: this.l('Seal No'), headerTooltip: this.l('SealNo'), field: 'sealNo',
                },
			{ headerName: this.l('Cd Date'), headerTooltip: this.l('CdDate'), field: 'cdDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Cd Status'), headerTooltip: this.l('CdStatus'), field: 'cdStatus' , type: 'rightAligned',
                },
			{ headerName: this.l('Container Size'), headerTooltip: this.l('ContainerSize'), field: 'containerSize' , type: 'rightAligned',
                },
			{ headerName: this.l('Shipping Date'), headerTooltip: this.l('ShippingDate'), field: 'shippingDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Port Date'), headerTooltip: this.l('PortDate'), field: 'portDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Receive Date'), headerTooltip: this.l('ReceiveDate'), field: 'receiveDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Port Date Actual'), headerTooltip: this.l('PortDateActual'), field: 'portDateActual',
			    valueGetter: (params) => this.pipe.transform(params.data?.portDateActual, 'dd/MM/yyyy'),
                },
            { headerName: this.l('Port Transit Date'), headerTooltip: this.l('PortTransitDate'), field: 'portTransitDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.portTransitDate, 'dd/MM/yyyy'),
                 },
            { headerName: this.l('Unpacking Date'), headerTooltip: this.l('UnpackingDate'), field: 'unpackingDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy'),
                },
			{ headerName: this.l('Storage Location Code'), headerTooltip: this.l('StorageLocationCode'), field: 'storageLocationCode',
                },
            { headerName: this.l('Order Type Code'), headerTooltip: this.l('ordertypeCode'), field: 'ordertypeCode',
                },
            { headerName: this.l('Fob'),  headerTooltip: this.l('Fob'), field: 'fob',  type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                },
            { headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true},
            { headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance',  type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true},
            { headerName: this.l('Cif'),  headerTooltip: this.l('Cif'), field: 'cif', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true },
            { headerName: this.l('Tax'),  headerTooltip: this.l('Tax'), field: 'tax', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true},
            { headerName: this.l('Amount'),  headerTooltip: this.l('Amount'), field: 'amount', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true},

        ];

        this.columnRepackDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, pinned: 'left'},
			{ headerName: this.l('Exp'), headerTooltip: this.l('Exp'), field: 'exp', flex: 1 },
            { headerName: this.l('LotNo'), headerTooltip: this.l('LotNo'), field: 'lotNo', flex: 1 },
            { headerName: this.l('Module'), headerTooltip: this.l('Module'), field: 'module', flex: 1 },
            { headerName: this.l('RepackModuleNo'), headerTooltip: this.l('RepackModuleNo'), field: 'repackModuleNo', flex: 1 },
            { headerName: this.l('PartNo'), headerTooltip: this.l('PartNo'), field: 'partNo', flex: 1 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1 },
            { headerName: this.l('Remain Qty'), headerTooltip: this.l('Remain Qty'), field: 'remainQty' , type: 'rightAligned',
                valueGetter: (params) => params.data?.remainQty !== 0 ? this._fm.formatMoney_decimal(params.data?.remainQty ?? params.data?.qty) : 0
                },
            {
                    headerName: this.l('Repack Qty'),
                    headerTooltip: this.l('Repack Qty'),
                    field: 'repackQty',
                    valueFormatter: (params) => {
                        let exist = this.listChangeds.find(x => x.id == params.data.id);
                        if (exist) {
                            return exist.qty;
                        } else {
                            return !params.data?.repackQty ? 0 : params.data?.repackQty;
                        }
                    },
                    flex: 1,
                    type: 'rightAligned',
                    editable: (params) => params.data.remainQty === null || params.data.remainQty > 0,
                    cellStyle: params => {
                        if (params.data.remainQty === null || params.data.remainQty > 0) {
                            return { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' };
                        }
                    },
                    cellEditor: NumericEditor,
            },
            { headerName: this.l('PackingDate'), headerTooltip: this.l('PackingDate'), field: 'packingDate', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.packingDate, 'dd/MM/yyyy') },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'container', flex: 1 },
            { headerName: this.l('WHCurrent'), headerTooltip: this.l('WHCurrent'), field: 'whCurrent', flex: 1 },
            { headerName: this.l('WHNew'), headerTooltip: this.l('WHNew'), field: 'whNew', flex: 1 },
            { headerName: this.l('Shift'), headerTooltip: this.l('Shift'), field: 'shift', flex: 1 },
            { headerName: this.l('ReceiveDateTime'), headerTooltip: this.l('ReceiveDateTime'), field: 'receiveDateTime', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.receiveDateTime, 'dd/MM/yyyy hh:mm:ss'),
            },
        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationDetailsParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationRepackParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.billofladingNo,
            this.supplierNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView()
                this.isLoading = false;
            });
    }

    searchDatasDetails(): void {
        this.resetGridView();
        if (this.rowClickData) {
            this.isLoading = true;
            this._service.getAllDetails(
                this.rowClickData.containerNo,
                '',
                this.paginationParams.skipCount,
                this.paginationParams.pageSize
            ).subscribe((result) => {
                this.paginationDetailsParams.totalCount = result.totalCount;
                this.rowDetailsData = result.items;
                this.paginationDetailsParams.totalPage = ceil(result.totalCount / (this.paginationDetailsParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
        }
    }

    searchDatasRepack(): void {
        this.resetGridView();
        this.isLoading = true;
        this._service.getAllRepack(
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe((result) => {
            this.paginationRepackParams.totalCount = result.totalCount;
            this.rowRepackData = result.items;
            this.paginationRepackParams.totalPage = ceil(result.totalCount / (this.paginationRepackParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi?.getAllColumns()?.forEach((column) => {
            // eslint-disable-next-line eqeqeq
            if (column.getId().toString() != 'Listcase No' && column.getId().toString() != 'stt') {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi?.autoSizeColumns(allColumnIds);
    }

    autoSizeDetailsAll() {
        const allColumnIds: string[] = [];
        this.dataParamsDetails.columnApi?.getAllColumns()?.forEach((column) => {
            // eslint-disable-next-line eqeqeq
            if (column.getId().toString() != 'Listcase No' && column.getId().toString() != 'stt') {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsDetails.columnApi?.autoSizeColumns(allColumnIds);
    }

    autoSizeRepackAll() {
        const allColumnIds: string[] = [];
        this.dataParamsRepack.columnApi?.getAllColumns()?.forEach((column) => {
            // eslint-disable-next-line eqeqeq
            if (column.getId().toString() != 'stt') {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsRepack.columnApi?.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.dataParamsDetails.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            // this.dataParamsRepack.columnApi!.sizeColumnsToFit({
                // suppressColumnVirtualisation: true,
            // });
            this.autoSizeAll();
            this.autoSizeDetailsAll();
            this.autoSizeRepackAll();
        }, 5);
    }

    clearTextSearch() {
        this.containerNo = '',
            this.renban = '',
            this.requestDate = '',
            this.requestTime = '',
            this.invoiceNo = '',
            this.billofladingNo = '',
            this.supplierNo = '',
            this.sealNo = '',
            this.requestDateFrom = '',
            this.requestDateTo = '',
            this.lotNo= '',
            this.moduleCaseNo= '',
            this.partNo = '',
            this.lotNo = '',
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.billofladingNo,
            this.supplierNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    getDataDetails(paginationParams?: PaginationParamsModel) {
        return this._service.getAllDetails(
            this.rowClickData.containerNo,
            '',
            this.paginationDetailsParams.skipCount,
            this.paginationDetailsParams.pageSize
        )
    }


    getDataRepack(paginationParams?: PaginationParamsModel) {
        return this._service.getAllRepack(
            '',
            this.paginationDetailsParams.skipCount,
            this.paginationDetailsParams.pageSize
        )
    }


    checkedBoxRows: number = 0;
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerRentalWHPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerRentalWHPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.arrIdStatus = params.api.getSelectedRows();
        this.checkedBoxRows = this.arrIdStatus.length;
        //show/hide button confirm
        var myDate = new Date();
        if (this.selectedRow.requestDate) {
            var date1 = this.selectedRow.requestDate.toString().substring(0, 10);
            var date2 = this._dateTimeService.convertToDatetime(myDate).toString().substring(0, 10);
            this.isShowConfirm = !(this.selectedRow.id && (date1 == date2))
        }
    }

    onChangeDetailsRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerRentalWHPlanDetails[] } }) {
        this.saveSelectedRowDetails = params.api.getSelectedRows()[0] ?? new InvCkdContainerRentalWHPlanDetails();
        this.selectedRowDetails = Object.assign({}, this.saveSelectedRowDetails);

    }

    onChangeRepackRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerRentalWHPlanDetails[] } }) {
        this.saveSelectedRowRepack = params.api.getSelectedRows()[0] ?? new InvCkdContainerRentalWHPlanDetails();
        this.selectedRowRepack = Object.assign({}, this.saveSelectedRowRepack);

    }

    rowClickData: InvCkdContainerRentalWHPlanDto;
    onRowClick(params) {

        let _rows = document.querySelectorAll<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container .ag-row.ag-row-level-0.ag-row-position-absolute");
        for (let i = 0; _rows[i]; i++) { _rows[i].classList.remove("setcolor_background_rowclick"); }

        if (this.rowClickData && this.rowClickData.id == params.data.id) this.rowClickData = undefined;
        else {
            this.rowClickData = params.data;
            let _row = document.querySelector<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container div[row-id='" + params.node.rowIndex + "'].ag-row.ag-row-level-0.ag-row-position-absolute");
            if (_row) _row.classList.add("setcolor_background_rowclick");
        }
        this.searchDatasDetails();
    }

    changePage(paginationParams) {
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.isLoading = true;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView()
        });
    }


    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.isLoading = true;
        this.getDatas(this.paginationParams)
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView()
            });
    }


    changeDetailsPage(paginationParams) {
        this.paginationDetailsParams = paginationParams;
        this.paginationDetailsParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.isLoading = true;
        this.getDataDetails(this.paginationDetailsParams).subscribe((result) => {
            this.paginationDetailsParams.totalCount = result.totalCount;
            this.rowDetailsData = result.items;
            this.paginationDetailsParams.totalPage = ceil(result.totalCount / (this.paginationDetailsParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView()
        });
    }

    changeRepackPage(paginationParams) {
        this.paginationRepackParams = paginationParams;
        this.paginationRepackParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.isLoading = true;
        this.getDataRepack(this.paginationRepackParams).subscribe((result) => {
            this.paginationRepackParams.totalCount = result.totalCount;
            this.rowRepackData = result.items;
            this.paginationRepackParams.totalPage = ceil(result.totalCount / (this.paginationRepackParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView()
        });
    }

    callBackDataGridDetails(params: GridParams) {
        this.isLoading = true;
        this.dataParamsDetails = params;
        params.api.paginationSetPageSize(this.paginationDetailsParams.pageSize);
        this.paginationDetailsParams.skipCount =
            ((this.paginationDetailsParams.pageNum ?? 1) - 1) * (this.paginationDetailsParams.pageSize ?? 0);
        this.paginationDetailsParams.pageSize = this.paginationDetailsParams.pageSize;
        this.getDataDetails(this.paginationDetailsParams)
            .subscribe((result) => {
                this.paginationDetailsParams.totalCount = result.totalCount;
                this.rowDetailsData = result.items ?? [];
                this.paginationDetailsParams.totalPage = ceil(result.totalCount / (this.paginationDetailsParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView()
            });
    }

    callBackDataGridRepack(params: GridParams) {
        this.isLoading = true;
        this.dataParamsRepack = params;
        params.api.paginationSetPageSize(this.paginationRepackParams.pageSize);
        this.paginationRepackParams.skipCount =
            ((this.paginationRepackParams.pageNum ?? 1) - 1) * (this.paginationRepackParams.pageSize ?? 0);
        this.paginationRepackParams.pageSize = this.paginationRepackParams.pageSize;
        this.getDataDetails(this.paginationRepackParams)
            .subscribe((result) => {
                this.paginationRepackParams.totalCount = result.totalCount;
                this.rowDetailsData = result.items ?? [];
                this.paginationRepackParams.totalPage = ceil(result.totalCount / (this.paginationRepackParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView()
            });
    }

    conFirmStartusMultiChk(system: InvCkdContainerRentalWHPlanDto): void {

        this.arrIdStatus.forEach(e => {
            this.listIdStatus += e.id + ',';
        })

        this._service.conFirmStatusMultiCkb(this.listIdStatus, 'F').subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }
    deleteRow(system: InvCkdContainerRentalWHPlanDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerRentalWHPlanToExcel(
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.billofladingNo,
            this.supplierNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
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

    onCellValueChanged(ev) {
        this._service.conFirmStatus(ev.data.id.toString(), ev.newValue).subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    onDetailsCellValueChanged(ev) {
        this._service.conFirmStatus(ev.data.id.toString(), ev.newValue).subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    searchLateDate(i: number) {

        let _btn = document.querySelector('.actionButton_w' + i);
        if (_btn.classList.contains('active')) {
            _btn.classList.remove('active');
            this.searchDatas();
        } else {
            _btn.classList.add('active');
            this._service.getDataLateDate()
                .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
                .subscribe((result) => {
                    this.paginationParams.totalCount = result.totalCount;
                    this.rowData = result.items;
                    this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                    this.resetGridView();
                });
        }
    }

    onCellValueChangedDetails(e) {
        // init one array to push changed id
        if (e.column.colId == 'repackQty') {
            this.addPartToRepack(e, 1);
        }
    }

    onCellValueChangedRepack(e) {
        // init one array to push changed id
        if (e.column.colId == 'repackQty') {
            this.addPartToRepack(e, 2);
        }
    }
    ///
    addPartToRepack(rowdata, source) {
        /// source = 1: from details, 2: from repack
        const originalQty = source == 1 ? rowdata.data.usageQty : rowdata.data.qty;
        rowdata.data.repackQty = rowdata.newValue;
        rowdata.data.remainQty = rowdata.data.remainQty == null ? originalQty : rowdata.data.remainQty;
        if (rowdata.newValue <= rowdata.data.remainQty && rowdata.newValue >= 0) {
            let exist = this.listChangeds.find(x => x.id == rowdata.data.id);
            if (!exist) {
                this.listChangeds.push(new InvCkdContainerPartRepackInput({
                    id: rowdata.data.id,
                    exp: rowdata.data.supplierNo,
                    lotNo: rowdata.data.lotNo,
                    module: rowdata.data.moduleCaseNo,
                    partNo: rowdata.data.partNo,
                    qty: rowdata.newValue,
                    source: source
                }));
            } else {
                exist.qty = rowdata.newValue;
            }
        } else {
            this.notify.error('RepackQty must be less than RemainQty');
            rowdata.data.repackQty = 0;
            this.gridApi.refreshCells({columns: ['repackQty']});
        }
    }
    ///
    repackPart() {
        let moduleNo = prompt('Enter Module No: ')
        if (moduleNo.length > 6) {
            this.notify.error('Module No must be less than 6 characters');
            return;
        }
        this.message.confirm(
            this.l('AreYouSure'),
            this.l('AreYouSureRepackParts'),
            (isConfirmed) => {
                if (isConfirmed) {
                    this._service.repackPart(
                        moduleNo,
                        this.listChangeds.filter((e) => e.qty > 0)
                    ).subscribe(() => {
                        this.notify.success(this.l('Successfully'));
                        this.notify.info(this.l('Successfully'));
                        this.listChangeds = [];
                        this.searchDatasDetails();
                    });
                }
            }
        );
    }

    changeTabToRepack() {
        this.activeTab = 2;
        this.searchDatasRepack();
    }

    changeTabToContDetails() {
        this.activeTab = 1;
        this.searchDatasDetails();
    }
}

