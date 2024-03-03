import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockRundownDto, InvGpsStockRundownServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';


@Component({
    templateUrl: './gpsstockrundown.component.html',
})
export class GpsStockRundownComponent extends AppComponentBase implements OnInit {
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
    partNo : string = '' ;
	partName : string = '' ;
	partId
	materialId
	qty
	workingDate
	transactionId;
    myDate = new Date();
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
        private _service: InvGpsStockRundownServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,pinned: 'left'},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo',width:150,pinned: 'left'},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName',width:200,pinned: 'left'},
			{headerName: this.l('Uom'),headerTooltip: this.l('Uom'),field: 'uom',width:100,pinned: 'left'},
			//{headerName: this.l('Material Id'),headerTooltip: this.l('Material Id'),field: 'materialId',flex: 1},
			//{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1},
			//{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',flex: 1},
			//{headerName: this.l('Transaction Id'),headerTooltip: this.l('Transaction Id'),field: 'transactionId',flex: 1},
            { headerValueGetter: (params) => this.A1, field: 'a1', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a1 != null ? params.data?.a1 : 0, 2), },
            { headerValueGetter: (params) => this.A2, field: 'a2', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a2 != null ? params.data?.a2 : 0, 2), },
            { headerValueGetter: (params) => this.A3, field: 'a3', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a3 != null ? params.data?.a3 : 0, 2), },
            { headerValueGetter: (params) => this.A4, field: 'a4', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a4 != null ? params.data?.a4 : 0, 2), },
            { headerValueGetter: (params) => this.A5, field: 'a5', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a5 != null ? params.data?.a5 : 0, 2), },
            { headerValueGetter: (params) => this.A6, field: 'a6', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a6 != null ? params.data?.a6 : 0, 2), },
            { headerValueGetter: (params) => this.A7, field: 'a7', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a7 != null ? params.data?.a7 : 0, 2), },
            { headerValueGetter: (params) => this.A8, field: 'a8', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a8 != null ? params.data?.a8 : 0, 2), },
            { headerValueGetter: (params) => this.A9, field: 'a9', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a9 != null ? params.data?.a9 : 0, 2), },
            { headerValueGetter: (params) => this.A10, field: 'a10', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a10 != null ? params.data?.a10 : 0, 2), },
            { headerValueGetter: (params) => this.A11, field: 'a11', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a11 != null ? params.data?.a11 : 0, 2), },
            { headerValueGetter: (params) => this.A12, field: 'a12', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a12 != null ? params.data?.a12 : 0, 2), },
            { headerValueGetter: (params) => this.A13, field: 'a13', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a13 != null ? params.data?.a13 : 0, 2), },
            { headerValueGetter: (params) => this.A14, field: 'a14', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a14 != null ? params.data?.a14 : 0, 2), },
            { headerValueGetter: (params) => this.A15, field: 'a15', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a15 != null ? params.data?.a15 : 0, 2), },
            { headerValueGetter: (params) => this.A16, field: 'a16', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a16 != null ? params.data?.a16 : 0, 2), },
            { headerValueGetter: (params) => this.A17, field: 'a17', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a17 != null ? params.data?.a17 : 0, 2), },
            { headerValueGetter: (params) => this.A18, field: 'a18', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a18 != null ? params.data?.a18 : 0, 2), },
            { headerValueGetter: (params) => this.A19, field: 'a19', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a19 != null ? params.data?.a19 : 0, 2), },
            { headerValueGetter: (params) => this.A20, field: 'a20', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a20 != null ? params.data?.a20 : 0, 2), },
            { headerValueGetter: (params) => this.A21, field: 'a21', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a21 != null ? params.data?.a21 : 0, 2), },
            { headerValueGetter: (params) => this.A22, field: 'a22', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a22 != null ? params.data?.a22 : 0, 2), },
            { headerValueGetter: (params) => this.A23, field: 'a23', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a23 != null ? params.data?.a23 : 0, 2), },
            { headerValueGetter: (params) => this.A24, field: 'a24', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a24 != null ? params.data?.a24 : 0, 2), },
            { headerValueGetter: (params) => this.A25, field: 'a25', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a25 != null ? params.data?.a25 : 0, 2), },
            { headerValueGetter: (params) => this.A26, field: 'a26', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a26 != null ? params.data?.a26 : 0, 2), },
            { headerValueGetter: (params) => this.A27, field: 'a27', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a27 != null ? params.data?.a27 : 0, 2), },
            { headerValueGetter: (params) => this.A28, field: 'a28', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a28 != null ? params.data?.a28 : 0, 2), },
            { headerValueGetter: (params) => this.A29, field: 'a29', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a29 != null ? params.data?.a29 : 0, 2), },
            { headerValueGetter: (params) => this.A30, field: 'a30', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a30 != null ? params.data?.a30 : 0, 2), },
            { headerValueGetter: (params) => this.A31, field: 'a31', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a31 != null ? params.data?.a31 : 0, 2), },
            { headerValueGetter: (params) => this.A32, field: 'a32', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a32 != null ? params.data?.a32 : 0, 2), },
            { headerValueGetter: (params) => this.A33, field: 'a33', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a33 != null ? params.data?.a33 : 0, 2), },
            { headerValueGetter: (params) => this.A34, field: 'a34', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a34 != null ? params.data?.a34 : 0, 2), },
            { headerValueGetter: (params) => this.A35, field: 'a35', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a35 != null ? params.data?.a35 : 0, 2), },
            { headerValueGetter: (params) => this.A36, field: 'a36', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a36 != null ? params.data?.a36 : 0, 2), },
            { headerValueGetter: (params) => this.A37, field: 'a37', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a37 != null ? params.data?.a37 : 0, 2), },
            { headerValueGetter: (params) => this.A38, field: 'a38', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a38 != null ? params.data?.a38 : 0, 2), },
            { headerValueGetter: (params) => this.A39, field: 'a39', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a39 != null ? params.data?.a39 : 0, 2), },
            { headerValueGetter: (params) => this.A40, field: 'a40', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a40 != null ? params.data?.a40 : 0, 2), },
            { headerValueGetter: (params) => this.A41, field: 'a41', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a41 != null ? params.data?.a41 : 0, 2), },
            { headerValueGetter: (params) => this.A42, field: 'a42', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a42 != null ? params.data?.a42 : 0, 2), },
            { headerValueGetter: (params) => this.A43, field: 'a43', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a43 != null ? params.data?.a43 : 0, 2), },
            { headerValueGetter: (params) => this.A44, field: 'a44', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a44 != null ? params.data?.a44 : 0, 2), },
            { headerValueGetter: (params) => this.A45, field: 'a45', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a45 != null ? params.data?.a45 : 0, 2), },
            { headerValueGetter: (params) => this.A46, field: 'a46', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a46 != null ? params.data?.a46 : 0, 2), },
            { headerValueGetter: (params) => this.A47, field: 'a47', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a47 != null ? params.data?.a47 : 0, 2), },
            { headerValueGetter: (params) => this.A48, field: 'a48', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a48 != null ? params.data?.a48 : 0, 2), },
            { headerValueGetter: (params) => this.A49, field: 'a49', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a49 != null ? params.data?.a49 : 0, 2), },
            { headerValueGetter: (params) => this.A50, field: 'a50', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a50 != null ? params.data?.a50 : 0, 2), },
            { headerValueGetter: (params) => this.A51, field: 'a51', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a51 != null ? params.data?.a51 : 0, 2), },
            { headerValueGetter: (params) => this.A52, field: 'a52', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a52 != null ? params.data?.a52 : 0, 2), },
            { headerValueGetter: (params) => this.A53, field: 'a53', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a53 != null ? params.data?.a53 : 0, 2), },
            { headerValueGetter: (params) => this.A54, field: 'a54', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a54 != null ? params.data?.a54 : 0, 2), },
            { headerValueGetter: (params) => this.A55, field: 'a55', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a55 != null ? params.data?.a55 : 0, 2), },
            { headerValueGetter: (params) => this.A56, field: 'a56', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a56 != null ? params.data?.a56 : 0, 2), },
            { headerValueGetter: (params) => this.A57, field: 'a57', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a57 != null ? params.data?.a57 : 0, 2), },
            { headerValueGetter: (params) => this.A58, field: 'a58', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a58 != null ? params.data?.a58 : 0, 2), },
            { headerValueGetter: (params) => this.A59, field: 'a59', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a59 != null ? params.data?.a59 : 0, 2), },
            { headerValueGetter: (params) => this.A60, field: 'a60', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a60 != null ? params.data?.a60 : 0, 2), },
            { headerValueGetter: (params) => this.A61, field: 'a61', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.a61 != null ? params.data?.a61 : 0, 2), },



        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
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
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if ( column.getId().toString() != "stt"
                && column.getId().toString() != "Part No"
                && column.getId().toString() != "Part Name") {
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
            },200);
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            console.log(result.items);
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    clearTextSearch() {
        this.partNo = '',
		this.partName = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
 			this.partNo,
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

    calculatorRundown() {
        this.isLoading = true;
        this._service.calculatorRundown().subscribe(() => {
            this.isLoading = false;
        })
    }

    // exportToExcel(): void {
    //     this._service.getGpsStockRundownToExcel(
	// 		this.partNo
    //         )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //     });
    // }
}
