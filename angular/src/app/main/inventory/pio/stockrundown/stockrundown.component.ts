import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvPIOStockRundownDto, InvPIOStockRundownServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './stockrundown.component.html',
})
export class StockRundownComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvPIOStockRundownDto = new InvPIOStockRundownDto();
    saveSelectedRow: InvPIOStockRundownDto = new InvPIOStockRundownDto();
    datas: InvPIOStockRundownDto = new InvPIOStockRundownDto();
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
    partType: string  = '';
    mktCode: string  = '';
    qty
	workingDate
	transactionId;
    myDate = new Date();
    dateCurent = new Date();
    A1 : string = '';
    A2 : string = '';
    A3 : string = '';
    A4 : string = '';
    A5 : string = '';
    A6 : string = '';
    A7 : string = '';
    A8 : string = '';
    A9 : string = '';
    A10 : string = '';
    A11 : string = '';
    A12 : string = '';
    A13 : string = '';
    A14 : string = '';
    A15 : string = '';
    A16 : string = '';
    A17 : string = '';
    A18 : string = '';
    A19 : string = '';
    A20 : string = '';
    A21 : string = '';
    A22 : string = '';
    A23 : string = '';
    A24 : string = '';
    A25 : string = '';
    A26 : string = '';
    A27 : string = '';
    A28 : string = '';
    A29 : string = '';
    A30 : string = '';
    A31 : string = '';
    A32 : string = '';
    A33 : string = '';
    A34 : string = '';
    A35 : string = '';
    A36 : string = '';
    A37 : string = '';
    A38 : string = '';
    A39 : string = '';
    A40 : string = '';
    A41 : string = '';
    A42 : string = '';
    A43 : string = '';
    A44 : string = '';
    A45 : string = '';
    A46 : string = '';
    A47 : string = '';
    A48 : string = '';
    A49 : string = '';
    A50 : string = '';
    A51 : string = '';
    A52 : string = '';
    A53 : string = '';
    A54 : string = '';
    A55 : string = '';
    A56 : string = '';
    A57 : string = '';
    A58 : string = '';
    A59 : string = '';
    A60 : string = '';
    A61 : string = '';
    A62 : string = '';
    A63 : string = '';
    A64 : string = '';
    A65 : string = '';
    A66 : string = '';
    A67 : string = '';
    A68 : string = '';
    A69 : string = '';
    A70 : string = '';
    A71 : string = '';
    A72 : string = '';
    A73 : string = '';
    A74 : string = '';
    A75 : string = '';
    // A76 : string = '';
    // A77 : string = '';
    // A78 : string = '';
    // A79 : string = '';
    // A80 : string = '';
    // A81 : string = '';
    // A82 : string = '';
    // A83 : string = '';
    // A84 : string = '';
    // A85 : string = '';
    // A86 : string = '';
    // A87 : string = '';
    // A88 : string = '';
    // A89 : string = '';
    // A90 : string = '';
    // A91 : string = '';
    // A92 : string = '';

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
        private _service: InvPIOStockRundownServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,pinned: 'left' },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 ,pinned: 'left'},
            { headerName: this.l('Mkt Code'), headerTooltip: this.l('Mkt Code'), field: 'mktCode', flex: 1,pinned: 'left' },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType', flex: 1,pinned: 'left' },
            { headerValueGetter: (params) => this.A1, field: 'a1', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a1, 0)},
            { headerValueGetter: (params) => this.A2, field: 'a2', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a2, 0)},
            { headerValueGetter: (params) => this.A3, field: 'a3', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a3, 0)},
            { headerValueGetter: (params) => this.A4, field: 'a4', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a4, 0)},
            { headerValueGetter: (params) => this.A5, field: 'a5', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a5, 0)},
            { headerValueGetter: (params) => this.A6, field: 'a6', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a6, 0)},
            { headerValueGetter: (params) => this.A7, field: 'a7', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a7, 0)},
            { headerValueGetter: (params) => this.A8, field: 'a8', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a8, 0)},
            { headerValueGetter: (params) => this.A9, field: 'a9', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a9, 0)},
            { headerValueGetter: (params) => this.A10, field: 'a10', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a10, 0)},
            { headerValueGetter: (params) => this.A11, field: 'a11', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a11, 0)},
            { headerValueGetter: (params) => this.A12, field: 'a12', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a12, 0)},
            { headerValueGetter: (params) => this.A13, field: 'a13', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a13, 0)},
            { headerValueGetter: (params) => this.A14, field: 'a14', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a14, 0)},
            { headerValueGetter: (params) => this.A15, field: 'a15', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a15, 0)},
            { headerValueGetter: (params) => this.A16, field: 'a16', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a16, 0)},
            { headerValueGetter: (params) => this.A17, field: 'a17', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a17, 0)},
            { headerValueGetter: (params) => this.A18, field: 'a18', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a18, 0)},
            { headerValueGetter: (params) => this.A19, field: 'a19', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a19, 0)},
            { headerValueGetter: (params) => this.A20, field: 'a20', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a20, 0)},
            { headerValueGetter: (params) => this.A21, field: 'a21', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a21, 0)},
            { headerValueGetter: (params) => this.A22, field: 'a22', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a22, 0)},
            { headerValueGetter: (params) => this.A23, field: 'a23', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a23, 0)},
            { headerValueGetter: (params) => this.A24, field: 'a24', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a24, 0)},
            { headerValueGetter: (params) => this.A25, field: 'a25', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a25, 0)},
            { headerValueGetter: (params) => this.A26, field: 'a26', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a26, 0)},
            { headerValueGetter: (params) => this.A27, field: 'a27', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a27, 0)},
            { headerValueGetter: (params) => this.A28, field: 'a28', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a28, 0)},
            { headerValueGetter: (params) => this.A29, field: 'a29', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a29, 0)},
            { headerValueGetter: (params) => this.A30, field: 'a30', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a30, 0)},
            { headerValueGetter: (params) => this.A31, field: 'a31', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a31, 0)},
            { headerValueGetter: (params) => this.A32, field: 'a32', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a32, 0)},
            { headerValueGetter: (params) => this.A33, field: 'a33', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a33, 0)},
            { headerValueGetter: (params) => this.A34, field: 'a34', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a34, 0)},
            { headerValueGetter: (params) => this.A35, field: 'a35', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a35, 0)},
            { headerValueGetter: (params) => this.A36, field: 'a36', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a36, 0)},
            { headerValueGetter: (params) => this.A37, field: 'a37', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a37, 0)},
            { headerValueGetter: (params) => this.A38, field: 'a38', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a38, 0)},
            { headerValueGetter: (params) => this.A39, field: 'a39', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a39, 0)},
            { headerValueGetter: (params) => this.A40, field: 'a40', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a40, 0)},
            { headerValueGetter: (params) => this.A41, field: 'a41', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a41, 0)},
            { headerValueGetter: (params) => this.A42, field: 'a42', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a42, 0)},
            { headerValueGetter: (params) => this.A43, field: 'a43', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a43, 0)},
            { headerValueGetter: (params) => this.A44, field: 'a44', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a44, 0)},
            { headerValueGetter: (params) => this.A45, field: 'a45', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a45, 0)},
            { headerValueGetter: (params) => this.A46, field: 'a46', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a46, 0)},
            { headerValueGetter: (params) => this.A47, field: 'a47', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a47, 0)},
            { headerValueGetter: (params) => this.A48, field: 'a48', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a48, 0)},
            { headerValueGetter: (params) => this.A49, field: 'a49', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a49, 0)},
            { headerValueGetter: (params) => this.A50, field: 'a50', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a50, 0)},
            { headerValueGetter: (params) => this.A51, field: 'a51', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a51, 0)},
            { headerValueGetter: (params) => this.A52, field: 'a52', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a52, 0)},
            { headerValueGetter: (params) => this.A53, field: 'a53', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a53, 0)},
            { headerValueGetter: (params) => this.A54, field: 'a54', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a54, 0)},
            { headerValueGetter: (params) => this.A55, field: 'a55', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a55, 0)},
            { headerValueGetter: (params) => this.A56, field: 'a56', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a56, 0)},
            { headerValueGetter: (params) => this.A57, field: 'a57', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a57, 0)},
            { headerValueGetter: (params) => this.A58, field: 'a58', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a58, 0)},
            { headerValueGetter: (params) => this.A59, field: 'a59', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a59, 0)},
            { headerValueGetter: (params) => this.A60, field: 'a60', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a60, 0)},
            { headerValueGetter: (params) => this.A61, field: 'a61', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a61, 0)},
            { headerValueGetter: (params) => this.A62, field: 'a62', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a62, 0)},
            { headerValueGetter: (params) => this.A63, field: 'a63', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a63, 0)},
            { headerValueGetter: (params) => this.A64, field: 'a64', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a64, 0)},
            { headerValueGetter: (params) => this.A65, field: 'a65', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a65, 0)},
            { headerValueGetter: (params) => this.A66, field: 'a66', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a66, 0)},
            { headerValueGetter: (params) => this.A67, field: 'a67', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a67, 0)},
            { headerValueGetter: (params) => this.A68, field: 'a68', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a68, 0)},
            { headerValueGetter: (params) => this.A69, field: 'a69', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a69, 0)},
            { headerValueGetter: (params) => this.A70, field: 'a70', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a70, 0)},
            { headerValueGetter: (params) => this.A71, field: 'a71', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a71, 0)},
            { headerValueGetter: (params) => this.A72, field: 'a72', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a72, 0)},
            { headerValueGetter: (params) => this.A73, field: 'a73', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a73, 0)},
            { headerValueGetter: (params) => this.A74, field: 'a74', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a74, 0)},
            { headerValueGetter: (params) => this.A75, field: 'a75', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a75, 0)},
            // { headerValueGetter: (params) => this.A76, field: 'a76', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a76, 0)},
            // { headerValueGetter: (params) => this.A77, field: 'a77', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a77, 0)},
            // { headerValueGetter: (params) => this.A78, field: 'a78', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a78, 0)},
            // { headerValueGetter: (params) => this.A79, field: 'a79', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a79, 0)},
            // { headerValueGetter: (params) => this.A80, field: 'a80', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a80, 0)},
            // { headerValueGetter: (params) => this.A81, field: 'a81', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a81, 0)},
            // { headerValueGetter: (params) => this.A82, field: 'a82', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a82, 0)},
            // { headerValueGetter: (params) => this.A83, field: 'a83', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a83, 0)},
            // { headerValueGetter: (params) => this.A84, field: 'a84', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a84, 0)},
            // { headerValueGetter: (params) => this.A85, field: 'a85', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a85, 0)},
            // { headerValueGetter: (params) => this.A86, field: 'a86', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a86, 0)},
            // { headerValueGetter: (params) => this.A87, field: 'a87', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a87, 0)},
            // { headerValueGetter: (params) => this.A88, field: 'a88', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a88, 0)},
            // { headerValueGetter: (params) => this.A89, field: 'a89', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a89, 0)},
            // { headerValueGetter: (params) => this.A90, field: 'a90', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a90, 0)},
            // { headerValueGetter: (params) => this.A91, field: 'a91', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a91, 0)},
            // { headerValueGetter: (params) => this.A92, field: 'a92', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a92, 0)},


        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.myDate.setDate(this.myDate.getDate() + 1);
        this.A1 = this.pipe.transform(this.myDate, 'dd/MM/yyyy');
        this.A2 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1)), 'dd/MM/yyyy');
        this.A3 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A4 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A5 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A6 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A7 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A8 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A9 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A10 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A11 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A12 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A13 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A14 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A15 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A16 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A17 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A18 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A19 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A20 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A21 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A22 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A23 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A24 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A25 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A26 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A27 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A28 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A29 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A30 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A31 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A32 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A33 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A34 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A35 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A36 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A37 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A38 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A39 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A40 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A41 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A42 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A43 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A44 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A45 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A46 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A47 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A48 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A49 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A50 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A51 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A52 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A53 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A54 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A55 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A56 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A57 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A58 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A59 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A60 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A61 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A62 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A63 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A64 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A65 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A66 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A67 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A68 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A69 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A70 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A71 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A72 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A73 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A74 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A75 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A76 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A77 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A78 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A79 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A80 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A81 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A82 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A83 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A84 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A85 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A86 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A87 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A88 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A89 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A90 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A91 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        // this.A92 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');

    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
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
            this.partNo,
            this.mktCode,
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
        this.partNo = '';
        this.mktCode = '';
        this.searchDatas();
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.mktCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
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

    calculatorRundown(e) {
        this.dateCurent.setDate(this.dateCurent.getDate() + 1);
        this.message.confirm(this.l('Calculator có thể diễn ra trong vài phút và sẽ xóa dữ liệu cũ, bạn có xác nhận Calculator?'), 'Confirm Calculator', (isConfirmed) => {
            if (isConfirmed) {
                this.fn.exportLoading(e, true);
                this._service.calculatorPioRundown(this._dateTimeService.convertToDatetime(this.dateCurent)).subscribe(() => {
                    this.fn.exportLoading(e);
                    this.notify.info(this.l('Calculator Successfully'));
                    this.searchDatas();
                })
            }
        });
    }
}
