import { ViewLotDetailsComponent } from './view-lot-details-modal.component';
import { InvCkdPhysicalStockIssuingServiceProxy, InvCkdPhysicalStockPeriodServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPhysicalStockIssuingDto } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';
import { ViewPhysicalStockIssuingDetailModalComponent } from './view-modal.component';
import { ViewMaterialModalComponent } from './view-material-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    selector: 'app-physicalstockissuing',
    templateUrl: './physical-stock-issuing.component.html'
})
export class PhysicalStockIssuingComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewLotDetails', { static: true }) viewLotDetails: ViewLotDetailsComponent;
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal: | ViewMaterialModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewPhysicalStockIssuingDetailModal', { static: true }) viewPhysicalStockIssuingDetailModal: | ViewPhysicalStockIssuingDetailModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPhysicalStockIssuingDto = new InvCkdPhysicalStockIssuingDto();
    saveSelectedRow: InvCkdPhysicalStockIssuingDto = new InvCkdPhysicalStockIssuingDto();
    datas: InvCkdPhysicalStockIssuingDto = new InvCkdPhysicalStockIssuingDto();
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
    vinNo: string = '';
    lotNo: string = '';
    useLot: string = '';
    noInLot
    partListId
    partListGradeId
    materialId
    qty
    transactionDatetime
    referenceId
    workingDate
    periodId: number = 0;
    isActive: string = '';
    workingDateFrom: any;
    workingDateTo: any;
    selectedIdMaterial;
    selectedCarId;
    date = new Date();
    cfc: string = '';
    supplierNo;
    p_mode = 0;
    periodIdList: any[] = [];
    id: any;
    Period: any;

    _lotno: string = '';

    modelist = [
        { value: '0', label: "Mix" },
        { value: '1', label: "Lot" },
        { value: '2', label: "Pxp" },
    ];
    loadColumdef: CustomColDef[] = [];
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
    defaultColDef1 = {
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
        private _service: InvCkdPhysicalStockIssuingServiceProxy,
        private _serviceStock: InvCkdPhysicalStockPeriodServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width: 100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'fromDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'toDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),
                flex: 1
            },

        ];
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNoNormalizedS4',flex:1 },
            { headerName: this.l('Color Sfx'), headerTooltip: this.l('Color Sfx'), field: 'colorSfx',flex:1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc',flex:1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo',flex:1 },
            { headerName: this.l('Use Lot No'), headerTooltip: this.l('Use Lot No'), field: 'useLot',flex:1 },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo',flex:1 },
            { headerName: this.l('No In Lot'), headerTooltip: this.l('No In Lot'), field: 'noInLot',flex:1 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.qty), type: 'rightAligned', aggFunc: this.SumA,flex:1 },
            { headerName: this.l('Vin No'), headerTooltip: this.l('Vin No'), field: 'vinNo',flex:1 },
            { headerName: this.l('Body No'), headerTooltip: this.l('Body No'), field: 'bodyNo',flex:1 },
            { headerName: this.l('Color'), headerTooltip: this.l('Color'), field: 'color',flex:1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName',flex:1 },
            { headerName: this.l('Transaction Datetime'), headerTooltip: this.l('Transaction Datetime'), field: 'transactionDatetime', valueGetter: (params) => this.pipe.transform(params.data?.transactionDatetime, 'dd/MM/yyyy HH:mm:ss'),flex:1 },
            { headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'), field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),flex:1 },
            { headerName: this.l('MaterialId'), headerTooltip: this.l('MaterialId'), field: 'materialId', type: 'rightAligned',flex:1 },


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        // this.date.setDate(this.date.getDate() - 3);
        // this.workingDateFrom = this.date;
        // this.workingDateTo = this.date;
        this._serviceStock.getIdInvPhysicalStockPeriod()
            .subscribe((result) => {
                this.periodIdList = result.items;
                this.Period = (result.items.filter(s => s.status = 1))[0].infoPeriod;
            });

    }

    // autoSize() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //         allColumnIds.push(column.getId());
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {
    //     setTimeout(() => {
    //         this.dataParams.columnApi!.sizeColumnsToFit({
    //             suppressColumnVirtualisation: true,
    //         });
    //             this.autoSize();
    //     }, 1000);

    // }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getDataPhysicalStockIssuing(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.vinNo,
            this.lotNo,
            this.useLot,
            this.noInLot,
            this.p_mode,
            this.id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                var grandTotal = 0;
                if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
                var rows = this.createRow(1, grandTotal);
                this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                this.gridTableService.selectFirstRow(this.dataParams!.api)
                this.isLoading = false;
            });
    }

    clearTextSearch() {
            this.partNo = '',
            this.workingDateFrom = null;
            this.workingDateTo = null;
            this.cfc = '',
            this.supplierNo = '',
            this.colorSfx = '',
            this.vinNo = '',
            this.lotNo = '',
            this.useLot = '',
            this.noInLot = '',
            this.p_mode = 0,
            this.id = ''
            this.rowData = [];
           // this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getDataPhysicalStockIssuing(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.vinNo,
            this.lotNo,
            this.useLot,
            this.noInLot,
            this.p_mode,
            this.id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPhysicalStockIssuingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPhysicalStockIssuingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectedIdMaterial = this.saveSelectedRow.materialId;
        this.selectedCarId = this.saveSelectedRow.carid;

        this._lotno = this.saveSelectedRow.useLot;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;

        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            var grandTotal = 0;
            if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
            var rows = this.createRow(1, grandTotal);
            this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
            this.gridTableService.selectFirstRow(this.dataParams!.api)
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
       // this.isLoading = true;
        this.dataParams = params;
         params.api.paginationSetPageSize(this.paginationParams.pageSize);
         this.paginationParams.skipCount = ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
         this.paginationParams.pageSize = this.paginationParams.pageSize;
        // this.getDatas(this.paginationParams)
        //     .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        //     .subscribe((result) => {
        //         this.paginationParams.totalCount = result.totalCount;
        //         this.rowData = result.items ?? [];
        //         this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        //         var grandTotal = 0;
        //         if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
        //         this.gridTableService.selectFirstRow(this.dataParams!.api);
        //         var rows = this.createRow(1, grandTotal);
        //         this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
        //          this.isLoading = false;
        //         this.resetGridView();
        //     });
    }

    getViewMaterial() {
        if (this.selectedIdMaterial != null) {
            this.viewMaterialModal.show(this.selectedIdMaterial, 'Material');
        } else {
            this.viewMaterialModal.show(this.selectedCarId, 'Vehicle');
        }

    }
    
    exportToExcelSummary(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdPhysicalStockIssuingReportSummaryToExcel(           
            this.id,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCkdPhysicalStockIssuingToExcel(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.vinNo,
            this.lotNo,
            this.useLot,
            this.noInLot,
            this.p_mode,
            this.id,
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

    viewLotDetail() {
        if (this._lotno != null) {
            if (this._lotno.length == 6) this.viewLotDetails.show(this._lotno.replace('-', '0'));
            if (this._lotno.length == 5) this.viewLotDetails.show(this._lotno.replace('-', '00'));
            if (this._lotno.length == 4) this.viewLotDetails.show(this._lotno.replace('-', '000'));
        }
    }

    exportToExcelDetails(e) {
        this.fn.exportLoading(e, true);
        this._service.getCkdPhysicalStockIssuingDetailsToExcel(
            this.partNo,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.vinNo,
            this.lotNo,
            this.useLot,
            this.noInLot,
            this.p_mode,
            this.id,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}