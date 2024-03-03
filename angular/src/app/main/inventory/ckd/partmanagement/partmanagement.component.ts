import { GridApi, GridReadyEvent, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { InvCkdPartManagementDto, InvCkdPartManagementServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
	templateUrl: './partmanagement.component.html',
    styleUrls: ['./partmanagement.component.less'],
})
export class PartManagementComponent extends AppComponentBase implements OnInit {
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

	selectedRow: InvCkdPartManagementDto = new InvCkdPartManagementDto();
	saveSelectedRow: InvCkdPartManagementDto = new InvCkdPartManagementDto();
	datas: InvCkdPartManagementDto = new InvCkdPartManagementDto();
	isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
	dataParams: GridParams | undefined;
	rowData: any[] = [];
	gridApi!: GridApi;
	rowSelection = 'multiple';
	filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    invoiceDate: any;
    cdDate: any;
    shippingDate: any;
    portDate: any;
    receiveDate: any;
    portDateActual: any;
    portTransitDate: any;
    unpackingDate: any;
    billDateFrom: any;
    billDateTo: any;
	partNo: string = '';
    cfc: string = '';
    supplierNo: string = '';
    invoiceNo: string = '';
    ordertypeCode: string = '';
    goodstypeCode: string = '';
    invoiceDateFrom: any;
    invoiceDateTo: any;
    billofladingNo: string = '';
    shipmentNo: string = '';
    containerNo: string = '';
    renban: string = '';
    portDateFrom: any;
    portDateTo: any;
    lotNo : string = '';
    firmPackingDateFrom: any;
    firmPackingDateTo: any;
    receiveDateFrom: any;
    receiveDateTo: any;
    moduleCaseNo: string = '';
    unpackingDateFrom: any;
    unpackingDateTo: any;
    storageLocationCode: string = '';
    changedRecordsPartManagement: number[] = [];
    selectId;
    p_radio: string = '';
	orderNo: string = '';
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters

    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];

    goodstypeCodeList = [
        { value: '', label: "Mix" },
        { value: 'L', label: "Lot" },
        { value: 'P', label: "PxP" },
    ];

    ckdPio = 'C';

    ckdPioList = [
        { value: 'C', label: "CKD" },
        { value: 'P', label: "PIO" },
    ];

	defaultColDef = {
        enableValue: true,
        enableRowGroup: true,
        enablePivot: true,

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
		private _service: InvCkdPartManagementServiceProxy,
		private gridTableService: GridTableService,
		private _fileDownloadService: FileDownloadService,
		private _dateTimeService: DateTimeService,
		private _fm: DataFormatService,
	) {
		super(injector);
		this.defaultColDefs = [
			{ headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, pinned: 'left'},
			{ headerName: this.l('Part No'), headerTooltip: this.l('PartNo'), field: 'partNo',pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'carfamilyCode',pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Supplier No'), headerTooltip: this.l('SupplierNo'), field: 'supplierNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Part Name'), headerTooltip: this.l('PartName'), field: 'partName',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Lot No'), headerTooltip: this.l('LotNo'), field: 'lotNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'usageQty' , type: 'rightAligned',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.usageQty),
                enablePivot: true, enableRowGroup: true, enableValue: true,aggFunc: this.SumA},
			{ headerName: this.l('Fix Lot'), headerTooltip: this.l('FixLot'), field: 'fixlot',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Module Case No'), headerTooltip: this.l('ModuleCaseNo'), field: 'moduleCaseNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Firm Packing Month'), headerTooltip: this.l('Firmpackingmonth'), field: 'firmpackingmonth',
                valueGetter: (params) => this.pipe.transform(params.data?.firmpackingmonth, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Order No'), headerTooltip: this.l('OrderNo'), field: 'orderNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Car Name'), headerTooltip: this.l('CarName'), field: 'carName',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Invoice No'), headerTooltip: this.l('InvoiceNo'), field: 'invoiceNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Invoice Date'), headerTooltip: this.l('InvoiceDate'), field: 'invoiceDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Container No'), headerTooltip: this.l('ContainerNo'), field: 'containerNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Bill No'), headerTooltip: this.l('BillNo'), field: 'billofladingNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Bill Date'),headerTooltip: this.l('Bill Date'),field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),flex: 1,},
			{ headerName: this.l('Shipment No'), headerTooltip: this.l('ShipmentNo'), field: 'shipmentNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Seal No'), headerTooltip: this.l('SealNo'), field: 'sealNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Cd Date'), headerTooltip: this.l('CdDate'), field: 'cdDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Cd Status'), headerTooltip: this.l('CdStatus'), field: 'cdStatus' , type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Container Size'), headerTooltip: this.l('ContainerSize'), field: 'containerSize' , type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('ETD'), headerTooltip: this.l('ETD'), field: 'shippingDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('ETA'), headerTooltip: this.l('ETA'), field: 'portDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Receive Date'), headerTooltip: this.l('ReceiveDate'), field: 'receiveDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('ATA'), headerTooltip: this.l('ATA'), field: 'portDateActual',
			    valueGetter: (params) => this.pipe.transform(params.data?.portDateActual, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Port Transit Date'), headerTooltip: this.l('PortTransitDate'), field: 'portTransitDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.portTransitDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true, },
            { headerName: this.l('Unpacking Date'), headerTooltip: this.l('UnpackingDate'), field: 'unpackingDate',
			    valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,},
			{ headerName: this.l('Storage Location Code'), headerTooltip: this.l('StorageLocationCode'), field: 'storageLocationCode',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Order Type Code'), headerTooltip: this.l('ordertypeCode'), field: 'ordertypeCode',
                enablePivot: true, enableRowGroup: true, enableValue: true,},
            { headerName: this.l('Fob'),  headerTooltip: this.l('Fob'), field: 'fob',  type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4),
                enablePivot: true, enableRowGroup: true,  enableValue: true,aggFunc: this.SumA  },
            { headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true, aggFunc: this.SumA},
            { headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance',  type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true, aggFunc: this.SumA},
            { headerName: this.l('Cif'),  headerTooltip: this.l('Cif'), field: 'cif', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true, aggFunc: this.SumA },
            { headerName: this.l('Tax'),  headerTooltip: this.l('Tax'), field: 'tax', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true, aggFunc: this.SumA},
            { headerName: this.l('Amount'),  headerTooltip: this.l('Amount'), field: 'amount', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4),
                enablePivot: true, enableRowGroup: true, enableValue: true, aggFunc: this.SumA},

		],
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
            this.changedRecordsPartManagement = result;
            console.log("result =", result);
        })
    }
	searchDatas(): void {
        this.isLoading = true;
		// this.paginator.changePage(this.paginator.getPage());
		this._service.getAll(
			this.partNo,
			this.cfc,
			this.supplierNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
			this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.billofladingNo,
            this.shipmentNo,
            this.containerNo,
            this.renban,
			this._dateTimeService.convertToDatetime(this.portDateFrom),
			this._dateTimeService.convertToDatetime(this.portDateTo),
			this._dateTimeService.convertToDatetime(this.receiveDateFrom),
			this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.p_radio,
            this.moduleCaseNo,
			this._dateTimeService.convertToDatetime(this.unpackingDateFrom),
			this._dateTimeService.convertToDatetime(this.unpackingDateTo),
            this.storageLocationCode,
			this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
			this.orderNo,
            this.ordertypeCode,
            this.goodstypeCode,
            this.ckdPio,
            this._dateTimeService.convertToDatetime(this.firmPackingDateFrom),
            this._dateTimeService.convertToDatetime(this.firmPackingDateTo),
            this.lotNo,
			'',
			this.paginationParams.skipCount,
			this.paginationParams.pageSize
		)
			.pipe(finalize(() => {

                this.gridTableService.selectFirstRow(this.dataParams!.api)

            }))
			.subscribe((result) => {
				this.paginationParams.totalCount = result.totalCount;
				this.rowData = result.items;
				this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if(this.rowData.length > 0){
                    var rows = this.createRow(1);
                    this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                }else{
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }

				this.resetGridView();
                this.isLoading = false;
			});
	}

	clearTextSearch() {
        this.partNo = '';
        this.cfc = '';
        this.supplierNo = '';
        this.invoiceNo = '';
        this.invoiceDateFrom = '';
        this.invoiceDateTo = '';
        this.billofladingNo = '';
        this.shipmentNo = '';
        this.containerNo = '';
        this.renban = '';
        this.portDateFrom = '';
        this.portDateTo = '';
        this.receiveDateFrom = '';
        this.receiveDateTo = '';
        // this.p_radio = '',
        this.moduleCaseNo = '';
        this.unpackingDateFrom = '';
        this.unpackingDateTo = '';
        this.storageLocationCode = '';
        this.billDateFrom = '',
        this.billDateTo = '',
        this.orderNo = '',
        this.ordertypeCode = "",
        this.goodstypeCode = "",
        this.ckdPio = 'C',
        this.firmPackingDateFrom = "",
        this.firmPackingDateTo = "",
        this.lotNo = "",
		this.searchDatas();
	}

	getDatas(paginationParams?: PaginationParamsModel) {
		return this._service.getAll(
			this.partNo,
			this.cfc,
			this.supplierNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
			this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.billofladingNo,
            this.shipmentNo,
            this.containerNo,
            this.renban,
			this._dateTimeService.convertToDatetime(this.portDateFrom),
			this._dateTimeService.convertToDatetime(this.portDateTo),
			this._dateTimeService.convertToDatetime(this.receiveDateFrom),
			this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.p_radio,
            this.moduleCaseNo,
			this._dateTimeService.convertToDatetime(this.unpackingDateFrom),
			this._dateTimeService.convertToDatetime(this.unpackingDateTo),
            this.storageLocationCode,
			this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
			this.orderNo,
            this.ordertypeCode,
            this.goodstypeCode,
            this.ckdPio,
            this._dateTimeService.convertToDatetime(this.firmPackingDateFrom),
            this._dateTimeService.convertToDatetime(this.firmPackingDateTo),
            this.lotNo,
			'',
			this.paginationParams.skipCount,
			this.paginationParams.pageSize
		);
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

        var _SumQty = 0;
        var _SumFob = 0;
        var _SumCif = 0;
        var _SumFreight = 0;
        var _SumInsurance = 0;
        var _SumTax = 0;
        var _SumAmount = 0;
        if (this.rowData && this.rowData.length > 0) {

            _SumQty = this.rowData[0].totalQty || 0;
            _SumFob = this.rowData[0].totalFob || 0;
            _SumCif = this.rowData[0].totalCif || 0;
            _SumFreight = this.rowData[0].totalFreight || 0;
            _SumInsurance = this.rowData[0].totalInsurance || 0;
            _SumTax = this.rowData[0].totalTax || 0;
            _SumAmount = this.rowData[0].totalAmount || 0;
        }

        _SumQty = this._fm.decimal_math(_SumQty, 4, 'round');
        _SumFob = this._fm.decimal_math(_SumFob, 4, 'round');
        _SumCif = this._fm.decimal_math(_SumCif, 4, 'round');
        _SumFreight = this._fm.decimal_math(_SumFreight, 4, 'round');
        _SumInsurance = this._fm.decimal_math(_SumInsurance, 4, 'round');
        _SumTax = this._fm.decimal_math(_SumTax, 4, 'round');
        _SumAmount = this._fm.decimal_math(_SumAmount, 4, 'round');

        for (var i = 0; i < count; i++) {
            result.push({
                partNo: 'Grand Total',
                usageQty: _SumQty,
                fob: _SumFob,
                cif: _SumCif,
                freight: _SumFreight,
                insurance: _SumInsurance,
                tax: _SumTax,
                amount: _SumAmount,
            });
        }
        return result;
    }

	onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPartManagementDto[] } }) {
		this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPartManagementDto();
		this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
        console.log("ID = "+this.selectedRow.id);
        // var rows = this.createRow(1);
        // this.dataParams!.api.setPinnedBottomRowData(rows);
	}

	changePage(paginationParams) {
		this.isLoading = true;
		this.paginationParams = paginationParams;
		this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
		this.getDatas(this.paginationParams)
        .pipe(finalize(() => {
            if(this.rowData.length > 0)  this.gridTableService.selectFirstRow(this.dataParams!.api)

            // var rows = this.createRow(1);
            // this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
        }))
        .subscribe((result) => {
			this.paginationParams.totalCount = result.totalCount;
			this.rowData = result.items;
			this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
			// var rows = this.createRow(1);
            // this.dataParams!.api.setPinnedBottomRowData(rows);
            this.isLoading = false;
			this.resetGridView();
		});
	}
	autoSize() {
		const allColumnIds: string[] = [];
		this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
			allColumnIds.push(column.getId());
		});
		this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
	}

	resetGridView() {
		setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
			this.autoSize();
		}, 500)
	}

	callBackDataGrid(params: GridParams) {
		this.isLoading = true;
		this.dataParams = params;
		params.api.paginationSetPageSize(this.paginationParams.pageSize);
		this.paginationParams.skipCount =
			((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
		this.paginationParams.pageSize = this.paginationParams.pageSize;
		this.getDatas(this.paginationParams)
			.pipe(finalize(() => {


                this.gridTableService.selectFirstRow(this.dataParams!.api);


            }))
			.subscribe((result) => {
				this.paginationParams.totalCount = result.totalCount;
				this.rowData = result.items ?? [];
				this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
				this.isLoading = false;

                // var rows = this.createRow(1);
                // this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid

				this.resetGridView();
			});


	}

	exportToExcel(e): void {
        this.fn.exportLoading(e, true);
		this._service.getPartManagementToExcel(
			this.partNo,
			this.cfc,
			this.supplierNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
			this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.billofladingNo,
            this.shipmentNo,
            this.containerNo,
            this.renban,
			this._dateTimeService.convertToDatetime(this.portDateFrom),
			this._dateTimeService.convertToDatetime(this.portDateTo),
			this._dateTimeService.convertToDatetime(this.receiveDateFrom),
			this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.p_radio,
            this.moduleCaseNo,
			this._dateTimeService.convertToDatetime(this.unpackingDateFrom),
			this._dateTimeService.convertToDatetime(this.unpackingDateTo),
            this.storageLocationCode,
			this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
			this.orderNo,
            this.ordertypeCode,
            this.goodstypeCode,
            this.ckdPio,
            this._dateTimeService.convertToDatetime(this.firmPackingDateFrom),
            this._dateTimeService.convertToDatetime(this.firmPackingDateTo),
            this.lotNo,
		)
			.subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
			});
	}


    exportReportToExcel(e): void {
        this.fn.exportLoading(e, true);
		this._service.getPartManagementReportToExcel(
			this.partNo,
			this.cfc,
			this.supplierNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
			this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.billofladingNo,
            this.shipmentNo,
            this.containerNo,
            this.renban,
			this._dateTimeService.convertToDatetime(this.portDateFrom),
			this._dateTimeService.convertToDatetime(this.portDateTo),
			this._dateTimeService.convertToDatetime(this.receiveDateFrom),
			this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.p_radio,
            this.moduleCaseNo,
			this._dateTimeService.convertToDatetime(this.unpackingDateFrom),
			this._dateTimeService.convertToDatetime(this.unpackingDateTo),
            this.storageLocationCode,
			this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
			this.orderNo,
            this.ordertypeCode,
            this.goodstypeCode,
            this.ckdPio,
            this._dateTimeService.convertToDatetime(this.firmPackingDateFrom),
            this._dateTimeService.convertToDatetime(this.firmPackingDateTo),
            this.lotNo,
		)
			.subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
			});
	}

    setvalradio(e) {

        let _btnUncheck = document.querySelector('.actionButton_w'+e+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.p_radio = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+e);
            if(_btn) _btn.classList.add('active');
            this.p_radio = e;
        }

        this.searchDatas();


    }

}
