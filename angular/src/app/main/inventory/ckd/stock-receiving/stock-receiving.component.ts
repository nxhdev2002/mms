import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockReceivingDto, InvCkdStockReceivingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe, formatDate } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewStockReceivingDetailModalComponent } from './view-stockreceiving-detail-modal.component';
import { ViewStockReceivingMaterialModalComponent } from './view-stockreceiving-material-modal.component';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './stock-receiving.component.html',
})
export class StockReceivingComponent extends AppComponentBase implements OnInit {
    @Input() params: any;
    @Input() paramstype: any;
    @ViewChild('viewStockReceivingDetailModal', { static: true }) viewStockReceivingDetailModal: | ViewStockReceivingDetailModalComponent | undefined;
    @ViewChild('viewStockReceivingMaterialModal', { static: true }) viewStockReceivingMaterialModal: | ViewStockReceivingMaterialModalComponent | undefined;
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

    selectedRow: InvCkdStockReceivingDto = new InvCkdStockReceivingDto();
    saveSelectedRow: InvCkdStockReceivingDto = new InvCkdStockReceivingDto();
    datas: InvCkdStockReceivingDto = new InvCkdStockReceivingDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    partNo: string = '';
    partNoNormalized: string = '';
    partName: string = '';
    partNoNormalizedS4: string = '';
    colorSfx: string = '';
    partListId
    partListGradeId
    materialId
    qty
    invoiceNo
    supplierNo
    cfc
    containerNo
    transactionDatetime
    referenceId
    workingDate
    periodId
    isActive;
    workingDateFrom: any;
    workingDateTo: any;
    selectedIdMaterial;
    date = new Date();

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
        private _service: InvCkdStockReceivingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private eventBus: EventBusService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 50 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNoNormalizedS4', flex: 1 },
            { headerName: this.l('Color Sfx'), headerTooltip: this.l('Color Sfx'), field: 'colorSfx', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.qty),
                aggFunc: this.SumA,
            },
            // { headerName: this.l('Part No Normalized'), headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            // { headerName: this.l('Part No Normalized S4'), headerTooltip: this.l('Part No Normalized S4'), field: 'partNoNormalizedS4', flex: 1 },

            { headerName: this.l('Transaction Datetime'), headerTooltip: this.l('Transaction Datetime'), field: 'transactionDatetime', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.transactionDatetime, 'dd/MM/yyyy HH:mm:ss'), },
            { headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'), flex: 1 },
            { headerName: this.l('Material Id'), headerTooltip: this.l('MaterialId'), field: 'materialId', flex: 1, type: 'rightAligned' },
            // { headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            // }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };



    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        //this.date.setDate(this.date.getDate() - 3);

        let urlParams = new URLSearchParams(window.location.search);
        this.partNo = urlParams.get('p');


        if(this.partNo == null || this.partNo == undefined ) this.partNo = "";
        if(this.partNo != "" ){

                this.workingDateFrom = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, 1);
                this.workingDateTo = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, Number(urlParams.get('d')));

                this.cfc = urlParams.get('c');
                if(this.cfc == null || this.cfc == undefined ) this.cfc = "";
                this.supplierNo = urlParams.get('s')
                if(this.supplierNo == null || this.supplierNo == undefined ) this.supplierNo = "";

                this.searchDatas();
        }

        this.workingDateFrom = this.date;
        this.workingDateTo = this.date;
        this.searchDatas();




        // // Receive PartNo and Data
        // this.date.setDate(this.date.getDate() - 3);
        // var lastDay = new Date();
        // this.eventBus.getData().subscribe((data) => {

        //     if (data && data != null) {
        //         if (data.params && data.params != null) {

        //             this.partNo = data.params?.partNo;
        //             lastDay = new Date(formatDate(data.params?.workingDate, 'yyyy-MM-dd', 'en-US'));
        //             this.workingDateFrom = new Date(lastDay.getFullYear(), lastDay.getMonth(), 1);
        //             this.workingDateTo = lastDay;

        //             if (data.params?._event == "Search") {
        //                 this.searchDatas();
        //             }

        //         }
        //     }

        // })

        // this.partNo = String(this.eventBus.getData().subscribe((data) => {data.partNo}))
        // this.workingDateFrom = new Date(lastDay.getFullYear(), lastDay.getMonth(), 1);
        // this.workingDateTo = lastDay;


    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.cfc,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                // console.log(result.items)
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView()
                var grandTotal = 0;
                if (result.totalCount > 0) {
                    grandTotal = result.items[0].grandTotal;
                    var rows = this.createRow(1, grandTotal);
                    this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                    this.gridTableService.selectFirstRow(this.dataParams!.api)
                }
                else{
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.isLoading = false;


            });
    }

    clearTextSearch() {
            this.partNo = '',
            this.cfc = '',
            this.supplierNo = '',
            this.colorSfx = '',
            this.containerNo = '',
            this.invoiceNo = '',
            this.workingDateFrom = this.date;
            this.workingDateTo = this.date;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.cfc,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    // autoSizeAll() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //         if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
    //             allColumnIds.push(column.getId());
    //         }
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {

    //     setTimeout(() => {
    //         this.dataParams.columnApi!.sizeColumnsToFit({
    //             suppressColumnVirtualisation: true,
    //         });
    //         this.autoSizeAll();
    //     }, 1000)
    // }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdStockReceivingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdStockReceivingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectedIdMaterial = this.saveSelectedRow.materialId;

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).pipe(finalize(() => {


        })).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            // var grandTotal = 0;
            // if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
            // var rows = this.createRow(1, grandTotal);
            // this.dataParams!.api.setPinnedBottomRowData(rows);
            // this.isLoading = false;
            // this.resetGridView()
        });
    }

    callBackDataGrid(params: GridParams) {

        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
    }

    getViewDataDetail() {
        this.viewStockReceivingDetailModal.show(this.selectedRow);
    }

    getViewDataMaterial() {
        if (this.selectedIdMaterial != null) {
            this.viewStockReceivingMaterialModal.show(this.selectedIdMaterial);
        }

    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockReceivingToExcel(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.cfc,
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

    exportbyMaterialToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockReceivingByMaterialToExcel(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.containerNo,
            this.invoiceNo,
            this.cfc,
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

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number, grandTotal: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNoNormalizedS4: 'Grand Total',
                qty: grandTotal,
            });
        }
        return result;
    }


}
