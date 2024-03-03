import { ColDef, GridApi, GridReadyEvent, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, EventEmitter, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerListDto, InvCkdContainerListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewContainerListModalComponent } from './view-detail-containerlist-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ViewContainerIntransitComponent } from './view-containerintransit-modal.component';
import { ViewImportDevanComponent } from './view-importdevan.component';
import { CommonFunction } from '@app/main/commonfuncton.component';



@Component({
    templateUrl: './containerlist.component.html',
    styleUrls: ['./containerlist.component.less'],

})
export class ContainerListComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewModalContainerList', { static: true }) viewModalContainerList: ViewContainerListModalComponent;
    @ViewChild('viewContainerIntransit', { static: true }) viewContainerIntransit: ViewContainerIntransitComponent;
    @ViewChild('viewImportDevanComponent', { static: true }) viewModalDevanList: ViewImportDevanComponent;
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

    selectedRow: InvCkdContainerListDto = new InvCkdContainerListDto();
    saveSelectedRow: InvCkdContainerListDto = new InvCkdContainerListDto();
    datas: InvCkdContainerListDto = new InvCkdContainerListDto();
    isLoading: boolean = true;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowDataContainerList: InvCkdContainerListDto[] = [];
    rowData: any;
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    billDateFrom: any;
    billDateTo: any;
    start_time
    end_time


    changedRecordsContainerList: number[] = [];
    selectGradeId;
    isRowGrade: boolean = false;
    _containerno: string = '';
    _renban: string = '';
    _invoiceNo: string = '';
    containerNo: string;
    renban: string;
    supplierNo: string = '';
    haisenNo: string = '';
    billOfLadingNo: string = '';
    invoiceNo: string;
    ordertypeCode: string = '';
    goodstypeCode: string = '';
    portDateFrom;
    receiveDate;
    portDateTo;
    lotNo;
    receiveDateFrom;
    receiveDateTo;
    radio: string = '';
    listId: string;
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters
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

    goodstypeCodeList = [
        { value: '', label: "Mix" },
        { value: 'L', label: "Lot" },
        { value: 'P', label: "PxP" },
    ];



    defaultColDef = {
        enableValue: true,
        enableRowGroup: true,
        enablePivot: true,

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
        private _service: InvCkdContainerListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService

    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, pinned: 'left' },
            {
                headerName: this.l('Request Status'), headerTooltip: this.l('Request Status'), field: 'requestStatus', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },

            {
                headerName: this.l('Carrier'), headerTooltip: this.l('Carrier'), field: 'carrier',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'description',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Haisen No'), headerTooltip: this.l('Haisen No'), field: 'haisenNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Seal No'), headerTooltip: this.l('Seal No'), field: 'sealNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Cd Date'), headerTooltip: this.l('Cd Date'), field: 'cdDate', valueFormatter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Cd Status'), headerTooltip: this.l('Cd Status'), field: 'cdStatusDes',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Container Size'), headerTooltip: this.l('Container Size'), field: 'containerSize',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('ETD'), headerTooltip: this.l('ETD'), field: 'shippingDate', valueFormatter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('ETA'), headerTooltip: this.l('ETA'), field: 'portDate', valueFormatter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Receive Date'), headerTooltip: this.l('Receive Date'), field: 'receiveDate', valueFormatter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('ATA'), headerTooltip: this.l('ATA'), field: 'portDateActual', valueFormatter: (params) => this.pipe.transform(params.data?.portDateActual, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Port Transit Date'), headerTooltip: this.l('Port Transit Date'), field: 'portTransitDate', valueFormatter: (params) => this.pipe.transform(params.data?.portTransitDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },


            // {
            // headerName: 'Request',
            // children: [
            {
                field: 'shop', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'dock', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'requestLotNo', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', },
            {
                field: 'invoiceNo', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'listLotNo', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
                width: 800
            },
            {
                field: 'listCaseNo', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
                width: 900
            },
            {
                field: 'transport', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'devanningDate', headerClass: "cell-group-parent-child cell-parent-request",
                valueFormatter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'devanningTime', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'devaningDateTime', headerClass: "cell-group-parent-child cell-parent-request",
                enablePivot: true, enableRowGroup: true, enableValue: true,valueFormatter: (params) => this.pipe.transform(params.data?.devaningDateTime, 'dd/MM/yyyy HH:mm')
            },
            { field: 'remark', headerClass: "cell-group-parent-child cell-parent-request", },
            { field: 'whLocation', headerClass: "cell-group-parent-child cell-parent-request", },
            // ]
            // },

            // {
            //     headerName: 'Gate In',
            //     children: [
            {
                field: 'gateinDate', headerClass: "cell-group-parent-child cell-parent-gate-in",
                valueFormatter: (params) => this.pipe.transform(params.data?.gateinDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                field: 'gateinTime', headerClass: "cell-group-parent-child cell-parent-gate-in",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            //     ]
            // },

            // {
            //     headerName: 'Transit Port',
            //     children: [
            {
                field: 'transitPortReqDate', headerClass: "cell-group-parent-child cell-parent-transit-port",
                valueFormatter: (params) => this.pipe.transform(params.data?.transitPortReqDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Transit Port Req Time'), field: 'formatTransitPortReqTime', headerClass: "cell-group-parent-child cell-parent-transit-port",
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            { field: 'transitPortRemark' },
            //     ]
            // },
            // {
            //     headerName: 'Invoice',
            //     children: [
            {
                field: 'fob', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'freight', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'insurance', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'cif', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'tax', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'amount', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            //     ]
            // },

            {
                headerName: this.l('Location Code'), headerTooltip: this.l('Location Code'), field: 'locationCode',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                headerName: this.l('Location Date'), headerTooltip: this.l('Location Date'), field: 'locationDate', valueFormatter: (params) => this.pipe.transform(params.data?.locationDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                headerName: this.l('Ordertype Code'), headerTooltip: this.l('Ordertype Code'), field: 'ordertypeCode',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                headerName: this.l('Goodstype Code'), headerTooltip: this.l('Goodstype Code'), field: 'goodstypeCode',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overDEM', headerClass: "cell-group-parent-child cell-parent-invoice",
				aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overDET', headerClass: "cell-group-parent-child cell-parent-invoice",
				aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overDEMDET', headerClass: "cell-group-parent-child cell-parent-invoice",
				aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overFeeDEM', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.overFeeDEM, 0), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overFeeDET', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.overFeeDET, 0), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                field: 'overFeeDEMDET', headerClass: "cell-group-parent-child cell-parent-invoice",
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.overFeeDEMDET, 0), aggFunc: this.SumA, type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true
            },
            {
                headerName: this.l('Est Over DEM'), headerTooltip: this.l('Est Over DEM'), field: 'estOverDEM', type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.estOverDEM, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Est Over DET'), headerTooltip: this.l('Est Over DET'), field: 'estOverDET', type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.estOverDET, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Est Over Combine'), headerTooltip: this.l('Est Over Combine'), field: 'estOverCombine', type: 'rightAligned',
                enablePivot: true, enableRowGroup: true, enableValue: true, valueFormatter: (params) => this.pipe.transform(params.data?.estOverCombine, 'dd/MM/yyyy')
            },

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
            this.changedRecordsContainerList = result;
            console.log("result =", result);
        })
    }
    SumA(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }

    createRow(count: number,
        _SumFob: number,
        _SumCif: number,
        _SumFreight: number,
        _SumInsurance: number,
        _SumTax: number,
        _SumAmount: number,
        _SumOverDEM: number,
        _SumOverDET: number,
        _SumOverDEMDET: number,
        _SumOverFeeDEM: number,
        _SumOverFeeDET: number,
        _SumOverFeeDEMDET: number
    ): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                requestStatus: 'Grand Total',
                fob: _SumFob,
                freight: _SumFreight,
                insurance: _SumInsurance,
                cif: _SumCif,
                tax: _SumTax,
                amount: _SumAmount,
                overDEM:_SumOverDEM,
                overDET:_SumOverDET,
                overDEMDET:_SumOverDEMDET,
                overFeeDEM:_SumOverFeeDEM,
                overFeeDET:_SumOverFeeDET,
                overFeeDEMDET:_SumOverFeeDEMDET
            });
        }
        return result;
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked"
                && column.getId().toString() != "stt"
                && column.getId().toString() != "Request") {
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
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowDataContainerList = result.items;
                this.rowData = {
                    containerNo: this.containerNo,
                    renban: this.renban,
                    supplierNo: this.supplierNo,
                    haisenNo: this.haisenNo,
                    billOfLadingNo: this.billOfLadingNo,
                    invoiceNo: this.invoiceNo,
                    ordertypeCode: this.ordertypeCode,
                    portDateFrom: this._dateTimeService.convertToDatetime(this.portDateFrom),
                    portDateTo: this._dateTimeService.convertToDatetime(this.portDateTo),
                    receiveDateFrom: this._dateTimeService.convertToDatetime(this.receiveDateFrom),
                    receiveDateTo: this._dateTimeService.convertToDatetime(this.receiveDateTo),
                    goodstypeCode: this.goodstypeCode,
                    radio: this.radio,
                };

                this.listId = this.rowDataContainerList.map(e => e.id).toString();
                var _SumFob = 0;
                var _SumCif = 0;
                var _SumFreight = 0;
                var _SumInsurance = 0;
                var _SumTax = 0;
                var _SumAmount = 0;
                var _SumOverDEM = 0;
                var _SumOverDET = 0;
                var _SumOverDEMDET = 0;
                var _SumOverFeeDEM = 0;
                var _SumOverFeeDET = 0;
                var _SumOverFeeDEMDET = 0;
                if (result.totalCount > 0) {
                    _SumFob = result.items[0].grandFob;
                    _SumCif = result.items[0].grandCif;
                    _SumFreight = result.items[0].grandFreight;
                    _SumInsurance = result.items[0].grandInsurance;
                    _SumTax = result.items[0].grandTax;
                    _SumAmount = result.items[0].grandAmount;
                    _SumOverDEM = result.items[0].grandOverDEM;
                    _SumOverDET = result.items[0].grandOverDET;
                    _SumOverDEMDET = result.items[0].grandOverDEMDET;
                    _SumOverFeeDEM = result.items[0].grandOverFeeDEM;
                    _SumOverFeeDET = result.items[0].grandOverFeeDET;
                    _SumOverFeeDEMDET = result.items[0].grandOverFeeDEMDET;
                    var rows = this.createRow(1, _SumFob, _SumCif, _SumFreight, _SumInsurance, _SumTax, _SumAmount, _SumOverDEM, _SumOverDET, _SumOverDEMDET, _SumOverFeeDEM, _SumOverFeeDET, _SumOverFeeDEMDET);
                }
                this.dataParams!.api.setPinnedBottomRowData(rows);

                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));

                this.resetGridView();
                this.isLoading = false;
            });
    }
    clearTextSearch() {
        this.containerNo = '',
            this.renban = '',
            this.supplierNo = '',
            this.haisenNo = '',
            this.billOfLadingNo = '',
            this.invoiceNo = '',
            this.receiveDate = '',
            this.ordertypeCode = "",
            this.goodstypeCode = "",
            this.portDateFrom = '',
            this.portDateTo = '',
            this.receiveDateFrom = '',
            this.receiveDateTo = '',
            this.billDateFrom = '',
            this.billDateTo = '',
            this.radio = '',
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
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectGradeId = this.selectedRow.id;
        this.isRowGrade = (this.selectedRow.id != null ? true : false);
        this._containerno = this.selectedRow.containerNo;
        this._renban = this.selectedRow.renban;
        console.log("id =", this.selectGradeId);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
        
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowDataContainerList = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;

            // var _SumFob = 0;
            // var _SumCif = 0;
            // var _SumFreight = 0;
            // var _SumInsurance = 0;
            // var _SumTax = 0;
            // var _SumAmount = 0;
            // if (result.totalCount > 0) {
            //     _SumFob = result.items[0].grandFob;
            //     _SumCif = result.items[0].grandCif;
            //     _SumFreight = result.items[0].grandFreight;
            //     _SumInsurance = result.items[0].grandInsurance;
            //     _SumTax = result.items[0].grandTax;
            //     _SumAmount = result.items[0].grandAmount;
            //     var rows = this.createRow(1, _SumFob, _SumCif, _SumFreight, _SumInsurance, _SumTax, _SumAmount);
            // }
            // this.dataParams!.api.setPinnedBottomRowData(rows);

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
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api)
            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowDataContainerList = result.items ?? [];

                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                var _SumFob = 0;
                var _SumCif = 0;
                var _SumFreight = 0;
                var _SumInsurance = 0;
                var _SumTax = 0;
                var _SumAmount = 0;
                var _SumOverDEM = 0;
                var _SumOverDET = 0;
                var _SumOverDEMDET = 0;
                var _SumOverFeeDEM = 0;
                var _SumOverFeeDET = 0;
                var _SumOverFeeDEMDET = 0;
                if (result.totalCount > 0) {
                    _SumFob = result.items[0].grandFob;
                    _SumCif = result.items[0].grandCif;
                    _SumFreight = result.items[0].grandFreight;
                    _SumInsurance = result.items[0].grandInsurance;
                    _SumTax = result.items[0].grandTax;
                    _SumAmount = result.items[0].grandAmount;
                    _SumOverDEM = result.items[0].grandOverDEM;
                    _SumOverDET = result.items[0].grandOverDET;
                    _SumOverDEMDET = result.items[0].grandOverDEMDET;
                    _SumOverFeeDEM = result.items[0].grandOverFeeDEM;
                    _SumOverFeeDET = result.items[0].grandOverFeeDET;
                    _SumOverFeeDEMDET = result.items[0].grandOverFeeDEMDET;
                    var rows = this.createRow(1, _SumFob, _SumCif, _SumFreight, _SumInsurance, _SumTax, _SumAmount, _SumOverDEM, _SumOverDET, _SumOverDEMDET, _SumOverFeeDEM, _SumOverFeeDET, _SumOverFeeDEMDET);
                }

                this.dataParams!.api.setPinnedBottomRowData(rows);

                this.resetGridView();

            });
    }


    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerListToExcel(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportContInvoiceToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerInvoiceAllToExcel(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportShipmentInfoDetailExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getShipmentInfoDetailsToExcel(this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportInvoiceDetailsExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvoiceDetailsListToExcel(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportNoCustomsDeclareExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getNoCustomsDeclareToExcel()
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    viewContainerTransit() {
        this.viewContainerIntransit.show(this._containerno, this._renban, this._invoiceNo);
    }

    viewModalDevan() {
        this.viewModalDevanList.show(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this.portDateFrom,
            this.portDateTo,
            this.receiveDateFrom,
            this.receiveDateTo,
            this.goodstypeCode,
            this.radio,
            this.billDateFrom,
            this.billDateTo,
            this.ckdPio);
    }

    setvalradio(i: string) {

        let _btnUncheck = document.querySelector('.actionButton_w' + i + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.radio = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + i);
            if (_btn) _btn.classList.add('active');
            this.radio = i;
        }
        this.searchDatas();

    }

    outPartByWIP() {
        var outPart = document.querySelector('.btn.out-part-wip') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
        else outPart.classList.add("active");
    }

    lostForcusWIP() {
        var outPart = document.querySelector('.btn.out-part-wip') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
    }

    exportToExcelVehicleOutDetailsWIP(e, partType): void {
        this.exportLoading('exportExcelWIP', e, true)
        this.lostForcusWIP()
        if (partType == 'P') {
            this._service.getShipmentInfoDetailsPxpToExcel(
                this.containerNo,
                this.renban,
                this.supplierNo,
                this.haisenNo,
                this.billOfLadingNo,
                this.invoiceNo,
                this.ordertypeCode,
                this._dateTimeService.convertToDatetime(this.portDateFrom),
                this._dateTimeService.convertToDatetime(this.portDateTo),
                this._dateTimeService.convertToDatetime(this.receiveDateFrom),
                this._dateTimeService.convertToDatetime(this.receiveDateTo),
                this.goodstypeCode,
                this.radio,
                this._dateTimeService.convertToDatetime(this.billDateFrom),
                this._dateTimeService.convertToDatetime(this.billDateTo),
                this.ckdPio,
                this.lotNo
            )
                .subscribe((result) => {
                    setTimeout(() => {
                        this._fileDownloadService.downloadTempFile(result);
                        this.notify.success(this.l('Download Excel Successfully'));
                    },
                        this.exportLoading('exportExcelWIP', e, false)
                    );
                });
        } else if (partType == 'M') {
            this._service.getShipmentInfoDetailsToExcel(this.containerNo,
                this.renban,
                this.supplierNo,
                this.haisenNo,
                this.billOfLadingNo,
                this.invoiceNo,
                this.ordertypeCode,
                this._dateTimeService.convertToDatetime(this.portDateFrom),
                this._dateTimeService.convertToDatetime(this.portDateTo),
                this._dateTimeService.convertToDatetime(this.receiveDateFrom),
                this._dateTimeService.convertToDatetime(this.receiveDateTo),
                this.goodstypeCode,
                this.radio,
                this._dateTimeService.convertToDatetime(this.billDateFrom),
                this._dateTimeService.convertToDatetime(this.billDateTo),
                this.ckdPio,
                this.lotNo,
            )
                .subscribe((result) => {
                    setTimeout(() => {
                        this._fileDownloadService.downloadTempFile(result);
                        this.notify.success(this.l('Download Excel Successfully'));
                    },
                        this.exportLoading('exportExcelWIP', e, false)
                    );
                });
        }
        else if (partType == 'L') {
            this._service.getShipmentInfoDetailsToExcel(this.containerNo,
                this.renban,
                this.supplierNo,
                this.haisenNo,
                this.billOfLadingNo,
                this.invoiceNo,
                this.ordertypeCode,
                this._dateTimeService.convertToDatetime(this.portDateFrom),
                this._dateTimeService.convertToDatetime(this.portDateTo),
                this._dateTimeService.convertToDatetime(this.receiveDateFrom),
                this._dateTimeService.convertToDatetime(this.receiveDateTo),
                'L',
                this.radio,
                this._dateTimeService.convertToDatetime(this.billDateFrom),
                this._dateTimeService.convertToDatetime(this.billDateTo),
                this.ckdPio,
                this.lotNo,
            )
                .subscribe((result) => {
                    setTimeout(() => {
                        this._fileDownloadService.downloadTempFile(result);
                        this.notify.success(this.l('Download Excel Successfully'));
                    },
                        this.exportLoading('exportExcelWIP', e, false)
                    );
                });
        }

    }

    exportLoading(id, e, _isLoad?: boolean) {
        console.log(e);
        if (_isLoad) {
            this.start_time = new Date();
            document.getElementById(id).classList.add('exportExcel');
        }
        else {
            this.end_time = new Date();
            let s = this.end_time.getSeconds() - this.start_time.getSeconds();
            let ms = this.end_time.getMilliseconds() - this.start_time.getMilliseconds();
            let coundown = 2000 - ms; coundown = (coundown > 0) ? coundown : 0;
            setTimeout(() => {
                document.getElementById(id).classList.remove('exportExcel');
            }, coundown);
            return coundown;
        }
    }
}
