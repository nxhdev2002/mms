import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdRundownWarehouseServiceProxy, InvGpsStockRundownDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './stock-rundown-warehouse.component.html',
})
export class StockRundownWearehouseComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvGpsStockRundownDto = new InvGpsStockRundownDto();
    saveSelectedRow: InvGpsStockRundownDto = new InvGpsStockRundownDto();
    datas: InvGpsStockRundownDto = new InvGpsStockRundownDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    partNo: string = '';
    partName: string = '';
    p_CfcCode;
    p_SupplierCode;
    myDate = new Date();
    A1: string = '';
    A2: string = '';
    A3: string = '';
    A4: string = '';
    A5: string = '';
    A6: string = '';
    A7: string = '';
    A8: string = '';
    A9: string = '';
    A10: string = '';
    A11: string = '';
    A12: string = '';
    A13: string = '';
    A14: string = '';
    A15: string = '';
    A16: string = '';
    A17: string = '';
    A18: string = '';
    A19: string = '';
    A20: string = '';
    A21: string = '';
    A22: string = '';
    A23: string = '';
    A24: string = '';
    A25: string = '';
    A26: string = '';
    A27: string = '';
    A28: string = '';
    A29: string = '';
    A30: string = '';
    A31: string = '';
    A32: string = '';
    A33: string = '';
    A34: string = '';
    A35: string = '';
    A36: string = '';
    A37: string = '';
    A38: string = '';
    A39: string = '';
    A40: string = '';
    A41: string = '';
    A42: string = '';
    A43: string = '';
    A44: string = '';
    A45: string = '';
    A46: string = '';
    A47: string = '';
    A48: string = '';
    A49: string = '';
    A50: string = '';
    A51: string = '';
    A52: string = '';
    A53: string = '';
    A54: string = '';
    A55: string = '';
    A56: string = '';
    A57: string = '';
    A58: string = '';
    A59: string = '';
    A60: string = '';
    // +7 ngày trước ngày hiện tại
    A61: string = '';
    A62: string = '';
    A63: string = '';
    A64: string = '';
    A65: string = '';
    A66: string = '';
    A67: string = '';

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
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
        private _service: InvCkdRundownWarehouseServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, pinned: 'left' },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 150, pinned: 'left' },
            // { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 200, pinned: 'left' },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width: 100, pinned: 'left' },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', pinned: 'left', width: 100 },
            { headerValueGetter: (params) => this.A1, field: 'a1', type: 'rightAligned', valueGetter: (params) => (params.data?.a1 == null || params.data?.a1 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a1) },
            { headerValueGetter: (params) => this.A2, field: 'a2', type: 'rightAligned', valueGetter: (params) => (params.data?.a2 == null || params.data?.a2 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a2) },
            { headerValueGetter: (params) => this.A3, field: 'a3', type: 'rightAligned', valueGetter: (params) => (params.data?.a3 == null || params.data?.a3 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a3) },
            { headerValueGetter: (params) => this.A4, field: 'a4', type: 'rightAligned', valueGetter: (params) => (params.data?.a4 == null || params.data?.a4 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a4) },
            { headerValueGetter: (params) => this.A5, field: 'a5', type: 'rightAligned', valueGetter: (params) => (params.data?.a5 == null || params.data?.a5 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a5) },
            { headerValueGetter: (params) => this.A6, field: 'a6', type: 'rightAligned', valueGetter: (params) => (params.data?.a6 == null || params.data?.a6 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a6) },
            { headerValueGetter: (params) => this.A7, field: 'a7', type: 'rightAligned', valueGetter: (params) => (params.data?.a7 == null || params.data?.a7 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a7) },
            { headerValueGetter: (params) => this.A8, field: 'a8', type: 'rightAligned', valueGetter: (params) => (params.data?.a8 == null || params.data?.a8 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a8) },
            { headerValueGetter: (params) => this.A9, field: 'a9', type: 'rightAligned', valueGetter: (params) => (params.data?.a9 == null || params.data?.a9 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a9) },
            { headerValueGetter: (params) => this.A10, field: 'a10', type: 'rightAligned', valueGetter: (params) => (params.data?.a10 == null || params.data?.a10 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a10) },
            { headerValueGetter: (params) => this.A11, field: 'a11', type: 'rightAligned', valueGetter: (params) => (params.data?.a11 == null || params.data?.a11 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a11) },
            { headerValueGetter: (params) => this.A12, field: 'a12', type: 'rightAligned', valueGetter: (params) => (params.data?.a12 == null || params.data?.a12 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a12) },
            { headerValueGetter: (params) => this.A13, field: 'a13', type: 'rightAligned', valueGetter: (params) => (params.data?.a13 == null || params.data?.a13 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a13) },
            { headerValueGetter: (params) => this.A14, field: 'a14', type: 'rightAligned', valueGetter: (params) => (params.data?.a14 == null || params.data?.a14 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a14) },
            { headerValueGetter: (params) => this.A15, field: 'a15', type: 'rightAligned', valueGetter: (params) => (params.data?.a15 == null || params.data?.a15 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a15) },
            { headerValueGetter: (params) => this.A16, field: 'a16', type: 'rightAligned', valueGetter: (params) => (params.data?.a16 == null || params.data?.a16 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a16) },
            { headerValueGetter: (params) => this.A17, field: 'a17', type: 'rightAligned', valueGetter: (params) => (params.data?.a17 == null || params.data?.a17 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a17) },
            { headerValueGetter: (params) => this.A18, field: 'a18', type: 'rightAligned', valueGetter: (params) => (params.data?.a18 == null || params.data?.a18 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a18) },
            { headerValueGetter: (params) => this.A19, field: 'a19', type: 'rightAligned', valueGetter: (params) => (params.data?.a19 == null || params.data?.a19 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a19) },
            { headerValueGetter: (params) => this.A20, field: 'a20', type: 'rightAligned', valueGetter: (params) => (params.data?.a20 == null || params.data?.a20 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a20) },
            { headerValueGetter: (params) => this.A21, field: 'a21', type: 'rightAligned', valueGetter: (params) => (params.data?.a21 == null || params.data?.a21 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a21) },
            { headerValueGetter: (params) => this.A22, field: 'a22', type: 'rightAligned', valueGetter: (params) => (params.data?.a22 == null || params.data?.a22 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a22) },
            { headerValueGetter: (params) => this.A23, field: 'a23', type: 'rightAligned', valueGetter: (params) => (params.data?.a23 == null || params.data?.a23 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a23) },
            { headerValueGetter: (params) => this.A24, field: 'a24', type: 'rightAligned', valueGetter: (params) => (params.data?.a24 == null || params.data?.a24 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a24) },
            { headerValueGetter: (params) => this.A25, field: 'a25', type: 'rightAligned', valueGetter: (params) => (params.data?.a25 == null || params.data?.a25 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a25) },
            { headerValueGetter: (params) => this.A26, field: 'a26', type: 'rightAligned', valueGetter: (params) => (params.data?.a26 == null || params.data?.a26 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a26) },
            { headerValueGetter: (params) => this.A27, field: 'a27', type: 'rightAligned', valueGetter: (params) => (params.data?.a27 == null || params.data?.a27 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a27) },
            { headerValueGetter: (params) => this.A28, field: 'a28', type: 'rightAligned', valueGetter: (params) => (params.data?.a28 == null || params.data?.a28 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a28) },
            { headerValueGetter: (params) => this.A29, field: 'a29', type: 'rightAligned', valueGetter: (params) => (params.data?.a29 == null || params.data?.a29 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a29) },
            { headerValueGetter: (params) => this.A30, field: 'a30', type: 'rightAligned', valueGetter: (params) => (params.data?.a30 == null || params.data?.a30 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a30) },
            { headerValueGetter: (params) => this.A31, field: 'a31', type: 'rightAligned', valueGetter: (params) => (params.data?.a31 == null || params.data?.a31 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a31) },
            { headerValueGetter: (params) => this.A32, field: 'a32', type: 'rightAligned', valueGetter: (params) => (params.data?.a32 == null || params.data?.a32 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a32) },
            { headerValueGetter: (params) => this.A33, field: 'a33', type: 'rightAligned', valueGetter: (params) => (params.data?.a33 == null || params.data?.a33 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a33) },
            { headerValueGetter: (params) => this.A34, field: 'a34', type: 'rightAligned', valueGetter: (params) => (params.data?.a34 == null || params.data?.a34 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a34) },
            { headerValueGetter: (params) => this.A35, field: 'a35', type: 'rightAligned', valueGetter: (params) => (params.data?.a35 == null || params.data?.a35 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a35) },
            { headerValueGetter: (params) => this.A36, field: 'a36', type: 'rightAligned', valueGetter: (params) => (params.data?.a36 == null || params.data?.a36 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a36) },
            { headerValueGetter: (params) => this.A37, field: 'a37', type: 'rightAligned', valueGetter: (params) => (params.data?.a37 == null || params.data?.a37 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a37) },
            { headerValueGetter: (params) => this.A38, field: 'a38', type: 'rightAligned', valueGetter: (params) => (params.data?.a38 == null || params.data?.a38 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a38) },
            { headerValueGetter: (params) => this.A39, field: 'a39', type: 'rightAligned', valueGetter: (params) => (params.data?.a39 == null || params.data?.a39 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a39) },
            { headerValueGetter: (params) => this.A40, field: 'a40', type: 'rightAligned', valueGetter: (params) => (params.data?.a40 == null || params.data?.a40 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a40) },
            { headerValueGetter: (params) => this.A41, field: 'a41', type: 'rightAligned', valueGetter: (params) => (params.data?.a41 == null || params.data?.a41 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a41) },
            { headerValueGetter: (params) => this.A42, field: 'a42', type: 'rightAligned', valueGetter: (params) => (params.data?.a42 == null || params.data?.a42 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a42) },
            { headerValueGetter: (params) => this.A43, field: 'a43', type: 'rightAligned', valueGetter: (params) => (params.data?.a43 == null || params.data?.a43 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a43) },
            { headerValueGetter: (params) => this.A44, field: 'a44', type: 'rightAligned', valueGetter: (params) => (params.data?.a44 == null || params.data?.a44 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a44) },
            { headerValueGetter: (params) => this.A45, field: 'a45', type: 'rightAligned', valueGetter: (params) => (params.data?.a45 == null || params.data?.a45 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a45) },
            { headerValueGetter: (params) => this.A46, field: 'a46', type: 'rightAligned', valueGetter: (params) => (params.data?.a46 == null || params.data?.a46 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a46) },
            { headerValueGetter: (params) => this.A47, field: 'a47', type: 'rightAligned', valueGetter: (params) => (params.data?.a47 == null || params.data?.a47 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a47) },
            { headerValueGetter: (params) => this.A48, field: 'a48', type: 'rightAligned', valueGetter: (params) => (params.data?.a48 == null || params.data?.a48 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a48) },
            { headerValueGetter: (params) => this.A49, field: 'a49', type: 'rightAligned', valueGetter: (params) => (params.data?.a49 == null || params.data?.a49 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a49) },
            { headerValueGetter: (params) => this.A50, field: 'a50', type: 'rightAligned', valueGetter: (params) => (params.data?.a50 == null || params.data?.a50 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a50) },
            { headerValueGetter: (params) => this.A51, field: 'a51', type: 'rightAligned', valueGetter: (params) => (params.data?.a51 == null || params.data?.a51 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a51) },
            { headerValueGetter: (params) => this.A52, field: 'a52', type: 'rightAligned', valueGetter: (params) => (params.data?.a52 == null || params.data?.a52 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a52) },
            { headerValueGetter: (params) => this.A53, field: 'a53', type: 'rightAligned', valueGetter: (params) => (params.data?.a53 == null || params.data?.a53 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a53) },
            { headerValueGetter: (params) => this.A54, field: 'a54', type: 'rightAligned', valueGetter: (params) => (params.data?.a54 == null || params.data?.a54 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a54) },
            { headerValueGetter: (params) => this.A55, field: 'a55', type: 'rightAligned', valueGetter: (params) => (params.data?.a55 == null || params.data?.a55 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a55) },
            { headerValueGetter: (params) => this.A56, field: 'a56', type: 'rightAligned', valueGetter: (params) => (params.data?.a56 == null || params.data?.a56 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a56) },
            { headerValueGetter: (params) => this.A57, field: 'a57', type: 'rightAligned', valueGetter: (params) => (params.data?.a57 == null || params.data?.a57 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a57) },
            { headerValueGetter: (params) => this.A58, field: 'a58', type: 'rightAligned', valueGetter: (params) => (params.data?.a58 == null || params.data?.a58 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a58) },
            { headerValueGetter: (params) => this.A59, field: 'a59', type: 'rightAligned', valueGetter: (params) => (params.data?.a59 == null || params.data?.a59 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a59) },
            { headerValueGetter: (params) => this.A60, field: 'a60', type: 'rightAligned', valueGetter: (params) => (params.data?.a60 == null || params.data?.a60 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a60) },
            { headerValueGetter: (params) => this.A61, field: 'a61', type: 'rightAligned', valueGetter: (params) => (params.data?.a61 == null || params.data?.a61 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a61) },
            { headerValueGetter: (params) => this.A60, field: 'a62', type: 'rightAligned', valueGetter: (params) => (params.data?.a62 == null || params.data?.a62 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a62) },
            { headerValueGetter: (params) => this.A60, field: 'a63', type: 'rightAligned', valueGetter: (params) => (params.data?.a63 == null || params.data?.a63 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a63) },
            { headerValueGetter: (params) => this.A60, field: 'a64', type: 'rightAligned', valueGetter: (params) => (params.data?.a64 == null || params.data?.a64 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a64) },
            { headerValueGetter: (params) => this.A65, field: 'a65', type: 'rightAligned', valueGetter: (params) => (params.data?.a65 == null || params.data?.a65 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a65) },
            { headerValueGetter: (params) => this.A66, field: 'a66', type: 'rightAligned', valueGetter: (params) => (params.data?.a66 == null || params.data?.a66 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a66) },
            { headerValueGetter: (params) => this.A67, field: 'a67', type: 'rightAligned', valueGetter: (params) => (params.data?.a67 == null || params.data?.a67 == 0) ? 0 : this._fm.formatMoney_decimal(params.data?.a67) },
            //{ headerValueGetter: (params) => this.A61, field: 'a61', type: 'rightAligned', valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a61 != null ? params.data?.a61 : 0, 2), },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

        let options = [{ day: 'numeric' }, { month: 'short' }];

        this.A1 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() - 7), options, ' - ');
        this.A2 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A3 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A4 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A5 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A6 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A7 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A8 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A9 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A10 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A11 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A12 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A13 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A14 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A15 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A16 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A17 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A18 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A19 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A20 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A21 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A22 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A23 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A24 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A25 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A26 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A27 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A28 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A29 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A30 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A31 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A32 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A33 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A34 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A35 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A36 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A37 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A38 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A39 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A40 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A41 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A42 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A43 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A44 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A45 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A46 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A47 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A48 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A49 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A50 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A51 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A52 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A53 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A54 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A55 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A56 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A57 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A58 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A59 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A60 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A61 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A62 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A63 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A64 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A65 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A66 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        this.A67 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
        // this.A61 = this.formatdategrid(this.myDate.setDate(this.myDate.getDate() + 1), options, ' - ');
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt"
                && column.getId().toString() != "Part No"
                && column.getId().toString() != "Part Name"
                && column.getId().toString() != "Cfc"
                && column.getId().toString() != "SupplierNo") {
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
        }, 200);
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.p_CfcCode,
            this.p_SupplierCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.p_CfcCode = '';
        this.p_SupplierCode = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.p_CfcCode,
            this.p_SupplierCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsStockRundownDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsStockRundownDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getExportWarehouseRundownToExcel(
            this.partNo,
            this.p_CfcCode,
            this.p_SupplierCode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    calculatorRundown(e) {
        this.message.confirm(this.l('Calculator có thể diễn ra trong vài phút và sẽ xóa dữ liệu cũ, bạn có xác nhận Calculator?'), 'Confirm Calculator Rundown',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.fn.exportLoading(e, true);
                    this._service.calculatorRundown().subscribe(() => {
                        setTimeout(() => {
                            this.notify.info(this.l('Calculator Successfully'));
                            this.searchDatas();
                        }, this.fn.exportLoading(e));
                    })
                }
            });
    }

    //format date theo kiểu Quý's tộc :v
    formatdategrid(date, options, separator) {
        //let options = [{day: 'numeric'}, {month: 'short'}, {year: 'numeric'}];
        /*
            day: "numeric", "2-digit".
            weekday: "narrow", "short", "long".
            year: "numeric", "2-digit".
            month: "numeric", "2-digit", "narrow", "short", "long".
            hour: "numeric", "2-digit".
            minute: "numeric", "2-digit".
            second: "numeric", "2-digit".
            hour12: true, false.
        */
        function format(option) {
            let formatter = new Intl.DateTimeFormat('en', option);
            return formatter.format(date);
        }
        return options.map(format).join(separator);
    }
}
