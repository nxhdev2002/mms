import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdCustomsDeclareDto, InvCkdPaymentRequestDto, InvCkdPaymentRequestServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';

@Component({
    selector: 'app-paymentrequest',
    templateUrl: './paymentrequest.component.html',
})
export class PaymentRequestComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    columnDefs: CustomColDef[] = [];
    paginationParamRequest: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamCustoms: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    requestColDefs: any;
    customsColDefs: any;

    selectedRow: InvCkdPaymentRequestDto = new InvCkdPaymentRequestDto();
    saveSelectedRow: InvCkdPaymentRequestDto = new InvCkdPaymentRequestDto();
    requestDto: InvCkdPaymentRequestDto = new InvCkdPaymentRequestDto();
    customsDto: InvCkdCustomsDeclareDto = new InvCkdCustomsDeclareDto();
    datas: InvCkdPaymentRequestDto = new InvCkdPaymentRequestDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    dataParamsCustoms: GridParams | undefined;
    rowDataRequest: any[] = [];
    rowDataCustoms: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;


    id: string = '';
    reqDate: any;
    reqPerson: string = '';
    isFromEcus5: string = '';
    reqDepartment: string = '';
    status: string = '';
    customsPort: string = '';
    time: any;

    requestNo: string = '';
    customsNo: string = '';

    invoiceNo: string = '';
    customsDeclareNo: string = '';
    declareDate: any;
    ispaid: string = '';
    ordertypeCode: string = '';
    impTax: number;
    impVat: number;
    actualTax: number;
    actualVat: number;
    exchangeRate: number;

    p_id: any;

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
        private _service: InvCkdPaymentRequestServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _formStoringService: FormStoringService,
    ) {
        super(injector);

        this.requestColDefs = [
            {
                headerName: this.l('STT'), headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamRequest.pageSize * (this.paginationParamRequest.pageNum - 1),
                cellClass: ['text-center'], flex: 1,
            },
            { headerName: this.l('Request No'), headerTooltip: this.l('Request No'), field: 'id', flex: 1, cellRenderer: 'agGroupCellRenderer', },
            {
                headerName: this.l('Request Date'),
                headerTooltip: this.l('Request Date'),
                field: 'reqDate',
                valueGetter: (params) => this.pipe.transform(params.data?.reqDate, 'dd/MM/yyyy'),
                flex: 1,
                comparator: this._formStoringService.dateComparator,
            },
            { headerName: this.l('Request Person'), headerTooltip: this.l('Request Person'), field: 'reqPerson', flex: 1, },
            { headerName: this.l('From Txt'), headerTooltip: this.l('From Txt'), field: 'isFromEcus5', flex: 1, },
            { headerName: this.l('Request Department'), headerTooltip: this.l('Request Department'), field: 'reqDepartment', flex: 1, },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1, },
            { headerName: this.l('Customs Port'), headerTooltip: this.l('Customs Port'), field: 'customsPort', flex: 1, },
            {
                headerName: this.l('Time'),
                headerTooltip: this.l('Time'),
                field: 'time',
                flex: 1,
                comparator: this._formStoringService.timeComparator,
            },
        ];



        this.customsColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'),cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamCustoms.pageSize * (this.paginationParamCustoms.pageNum - 1),cellClass: ['text-center'], width: 60,},
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo',width: 170 },
            { headerName: this.l('Customs Declare No'), headerTooltip: this.l('Customs Declare No'), field: 'customsDeclareNo',width: 170 },
            { headerName: this.l('Customs Date'), headerTooltip: this.l('Customs Date'), field: 'declareDate', valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),width: 170 },
            { headerName: this.l('Customs Port'), headerTooltip: this.l('Customs Port'), field: 'customsPort', width: 170},
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'ispaid', width: 170},
            { headerName: this.l('Order Type'), headerTooltip: this.l('Order Type'), field: 'ordertypeCode',width: 170 },
            { headerName: this.l('Imp Tax (VND)'), headerTooltip: this.l('Imp Tax (VND)'),field: 'impTax',valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.impTax),type: 'rightAligned',width: 170},
            { headerName: this.l('Imp Vat (VND)'), headerTooltip: this.l('Imp Vat (VND)'),field: 'impVat',valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.impVat),type: 'rightAligned',width: 170},
            { headerName: this.l('Actual Tax (VND)'), headerTooltip: this.l('Actual Tax (VND)'), comparator: this._formStoringService.decimalComparator, field: 'actualTax', valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.actualTax), type: 'rightAligned',width: 170 },
            { headerName: this.l('Actual Vat (VND)'), headerTooltip: this.l('Actual Vat (VND)'), comparator: this._formStoringService.decimalComparator, field: 'actualVat', valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.actualVat), type: 'rightAligned',width: 170 },
            { headerName: this.l('Exchange Rate (VND)'), headerTooltip: this.l('Exchange Rate (VND)'), comparator: this._formStoringService.decimalComparator, field: 'exchangeRate', valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.exchangeRate), type: 'rightAligned',width: 150 },
        ],
            this.frameworkComponents = {
                agCellButtonComponent: AgCellButtonRendererComponent,
            };
    }

    ngOnInit(): void {
        this.paginationParamRequest = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamCustoms = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
    }

    searchDatas(): void {
        this._service.getPaymentRequestSearch(
            this.requestNo,
            this.customsNo,
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParamRequest.skipCount,
            this.paginationParamRequest.pageSize,
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataCustoms = [];
                    this.paginationParamCustoms.totalCount = result.totalCount;
                    this.paginationParamCustoms.totalPage = ceil(result.totalCount / (this.paginationParamCustoms.pageSize ?? 0));

                } else {
                    this.searchDataCustoms(result.items[0].id);
                }
                this.rowDataRequest = result.items;
                this.paginationParamRequest.totalCount = result.totalCount;
                this.paginationParamRequest.totalPage = ceil(result.totalCount / (this.paginationParamRequest.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParamsCustoms.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsCustoms.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsCustoms.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000)
    }

    clearTextSearch() {
        this.ordertypeCode = '';
        this.requestNo = '',
            this.customsNo = '',
            this.ckdPio = 'C',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getPaymentRequestSearch(
            this.requestNo,
            this.customsNo,
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParamRequest.skipCount,
            this.paginationParamRequest.pageSize,
        );
    }

    onChangeRowSelectionRequest(params: { api: { getSelectedRows: () => InvCkdPaymentRequestDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id = selected.id;
            this.searchDataCustoms(selected.id);
        }
        this.selectedRow = Object.assign({}, selected);
    }

    changePageRequest(paginationParams) {
        this.isLoading = true;
        this.paginationParamRequest = paginationParams;
        this.paginationParamRequest.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParamRequest).subscribe((result) => {
            this.paginationParamRequest.totalCount = result.totalCount;
            this.rowDataRequest = result.items;
            this.paginationParamRequest.totalPage = ceil(result.totalCount / (this.paginationParamRequest.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGridRequest(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamRequest.pageSize);
        this.paginationParamRequest.skipCount =
            ((this.paginationParamRequest.pageNum ?? 1) - 1) * (this.paginationParamRequest.pageSize ?? 0);
        this.paginationParamRequest.pageSize = this.paginationParamRequest.pageSize;
        this.getDatas(this.paginationParamRequest)

            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }))

            .subscribe((result) => {
                this.paginationParamRequest.totalCount = result.totalCount;
                this.rowDataRequest = result.items ?? [];
                this.paginationParamRequest.totalPage = ceil(result.totalCount / (this.paginationParamRequest.pageSize ?? 0));
                this.isLoading = false;

            });
    }


    searchDataCustoms(p_id): void {
        this.isLoading = true;

        this._service.getCustomsDeclareByPayment(
            p_id,
            '',
            this.paginationParamCustoms.skipCount,
            this.paginationParamCustoms.pageSize
        )
            .pipe(finalize(() => { }
            )).subscribe((result) => {
                this.paginationParamCustoms.totalCount = result.totalCount;
                this.rowDataCustoms = result.items;
                this.paginationParamCustoms.totalPage = ceil(result.totalCount / (this.paginationParamCustoms.pageSize ?? 0));
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    getDataCustoms(paginationParams?: PaginationParamsModel) {
        return this._service.getCustomsDeclareByPayment(
            this.p_id,
            '',
            this.paginationParamCustoms.skipCount,
            this.paginationParamCustoms.pageSize
        );
    }

    changePageCustoms(paginationParams) {
        this.isLoading = true;
        this.paginationParamCustoms = paginationParams;
        this.paginationParamCustoms.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataCustoms(this.paginationParamCustoms).subscribe((result) => {
            this.paginationParamCustoms.totalCount = result.totalCount;
            this.rowDataCustoms = result.items;
            this.paginationParamCustoms.totalPage = ceil(result.totalCount / (this.paginationParamCustoms.pageSize ?? 0));
            this.isLoading = false;
            // this.resetGridView();

        });
    }

    callBackDataGridCustoms(params: GridParams) {
        this.isLoading = true;
        this.dataParamsCustoms = params;
        params.api.paginationSetPageSize(this.paginationParamCustoms.pageSize);
        this.paginationParamCustoms.skipCount =
            ((this.paginationParamCustoms.pageNum ?? 1) - 1) * (this.paginationParamCustoms.pageSize ?? 0);
        this.paginationParamCustoms.pageSize = this.paginationParamCustoms.pageSize;
        this.getDataCustoms(this.paginationParamCustoms)

            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsCustoms!.api)))

            .subscribe((result) => {
                this.paginationParamCustoms.totalCount = result.totalCount;
                this.rowDataCustoms = result.items ?? [];
                this.paginationParamCustoms.totalPage = ceil(result.totalCount / (this.paginationParamCustoms.pageSize ?? 0));
                this.isLoading = false;
                // this.resetGridView();

            });
    }

    exportToExcelRequest(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPaymentRequestToExcel(
            this.requestNo,
            this.customsNo,
            this.ckdPio,
            this.ordertypeCode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));

            });
    }

    exportToExcelCustoms(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCustomsDeclareToExcel(
            this.p_id,
            '',
            this.paginationParamRequest.skipCount,
            this.paginationParamRequest.pageSize,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));

            });
    }
}
