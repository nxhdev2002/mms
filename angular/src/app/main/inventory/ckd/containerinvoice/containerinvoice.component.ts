import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerInvoiceDto, InvCkdContainerInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditContainerInvoiceModalComponent } from './create-or-edit-containerinvoice-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewCustomsModalComponent } from './view-customs-modal.component';

@Component({
    templateUrl: './containerinvoice.component.html',
})
export class ContainerInvoiceComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewCustoms', { static: true }) viewCustomsModal: | ViewCustomsModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdContainerInvoiceDto = new InvCkdContainerInvoiceDto();
    saveSelectedRow: InvCkdContainerInvoiceDto = new InvCkdContainerInvoiceDto();
    datas: InvCkdContainerInvoiceDto = new InvCkdContainerInvoiceDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    billOfLadingNo;
    containerNo: string = '';
    renban: string = '';
    invoiceId
    supplierNo: string = '';
    sealNo: string = '';
    containerSize
    plandedvanningDate
    actualvanningDate
    thc
    inland
    cdDate
    status: string = '';
    thcVn
    inlandVn
    periodDate
    periodId
    dateStatus: string = '';
    fob
    freight
    insurance
    amount
    invoiceNo
    isActive: string = '';
    statusId: string = "-1";
    statusList: { value: string, label: string }[] = [];
    billDateFrom: any;
    billDateTo: any;
    selectContainerId;
    changedRecordsContainerInvoice: number[] = [];

    ckdPio = 'C';

    ckdPioList = [
        { value: 'C', label: "CKD" },
        { value: 'P', label: "PIO" },
    ];

    ordertypeCode: string = '';
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
        private _service: InvCkdContainerInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Id'), headerTooltip: this.l('Id'), field: 'id', hide: true },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo' },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban' },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo' },
            { headerName: this.l('Seal No'), headerTooltip: this.l('Seal No'), field: 'sealNo' },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo' },
            {
                headerName: this.l('Bill Date'),
                headerTooltip: this.l('Bill Date'),
                field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Container Size'),
                headerTooltip: this.l('Container Size'),
                field: 'containerSize',
                type: 'rightAligned'
            },
            {
                headerName: this.l('Plan Devanning Date'),
                headerTooltip: this.l('Plan Devanning Date'),
                field: 'plandedvanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.plandedvanningDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Actualvanning Date'),
                headerTooltip: this.l('Actualvanning Date'),
                field: 'actualvanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.actualvanningDate, 'dd/MM/yyyy')
            },
            { headerName: this.l('Cd Date'), headerTooltip: this.l('Cd Date'), field: 'cdDate', valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy') },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status' },
            { headerName: this.l('Date Status'), headerTooltip: this.l('Date Status'), field: 'dateStatus' },
            {
                headerName: this.l('Fob'), headerTooltip: this.l('Fob'), field: 'fob', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4), aggFunc: this.SumA
            },
            {
                headerName: this.l('Freight'), headerTooltip: this.l('Freight'), field: 'freight', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4), aggFunc: this.SumA
            },
            {
                headerName: this.l('Insurance'), headerTooltip: this.l('Insurance'), field: 'insurance', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4), aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax'), headerTooltip: this.l('Tax'), field: 'tax', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4), aggFunc: this.SumA
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'amount', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4), aggFunc: this.SumA
            },
            {
                headerName: this.l('Tax VN'), headerTooltip: this.l('Tax VN'), field: 'taxVnd', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.taxVnd, 0, false, true), aggFunc: this.SumA
            },
            {
                headerName: this.l('Vat VN'), headerTooltip: this.l('Vat VN'), field: 'vatVnd', type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.vatVnd, 0, false, true), aggFunc: this.SumA
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.getCbxStatus();
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsContainerInvoice = result;
            console.log("result =", result);
        })
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked"
                && column.getId().toString() != "stt"
                && column.getId().toString() != "listLotNo"
                && column.getId().toString() != "listCaseNo") {
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
        }, 1000);

    }

    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.billOfLadingNo,
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.supplierNo,
            this.sealNo,
            this.statusId,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
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

                var grandFob = 0;
                var grandFreight = 0;
                var grandInsurance = 0;
                var grandTax = 0;
                var grandAmount = 0;
                var grandTaxVn = 0;
                var grandVatVn = 0;
                if (result.totalCount > 0) {
                    grandFob = result.items[0].grandFob;
                    grandFreight = result.items[0].grandFreight;
                    grandInsurance = result.items[0].grandInsurance;
                    grandTax = result.items[0].grandTax;
                    grandAmount = result.items[0].grandAmount;
                    grandTaxVn = result.items[0].grandTaxVn;
                    grandVatVn = result.items[0].grandVatVn;
                    var rows = this.createRow(1, grandFob, grandFreight, grandInsurance, grandTax, grandAmount, grandTaxVn, grandVatVn);
                    this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                }else{
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.isLoading = false;
            });
    }

    getCbxStatus() {
        this.statusList.push({ value: "-1", label: '' });
        this._service.getContStatusList().subscribe((result) => {
            result.forEach(e => this.statusList.push({ value: e.code, label: e.description }));
        })
    }

    clearTextSearch() {
        this.billOfLadingNo = '',
            this.containerNo = '',
            this.renban = '',
            this.invoiceId = '',
            this.supplierNo = '',
            this.sealNo = '',
            this.containerSize = '',
            this.plandedvanningDate = '',
            this.actualvanningDate = '',
            this.thc = '',
            this.inland = '',
            this.cdDate = '',
            this.status = '',
            this.thcVn = '',
            this.inlandVn = '',
            this.periodDate = '',
            this.periodId = '',
            this.dateStatus = '',
            this.fob = '',
            this.freight = '',
            this.insurance = '',
            this.amount = '',
            this.invoiceNo = '',
            this.isActive = '',
            this.invoiceNo = '',
            this.statusId = '-1',
            this.billDateFrom = '',
            this.billDateTo = '',
            this.ckdPio = 'C',
            this.ordertypeCode = '';
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
            this.billOfLadingNo,
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.supplierNo,
            this.sealNo,
            this.statusId,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerInvoiceDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerInvoiceDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectContainerId = this.selectedRow.id;
        console.log("id =", this.selectContainerId);

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            var grandFob = 0;
            var grandFreight = 0;
            var grandInsurance = 0;
            var grandTax = 0;
            var grandAmount = 0;
            var grandTaxVn = 0;
            var grandVatVn = 0;
            if (result.totalCount > 0) {
                grandFob = result.items[0].grandFob;
                grandFreight = result.items[0].grandFreight;
                grandInsurance = result.items[0].grandInsurance;
                grandTax = result.items[0].grandTax;
                grandAmount = result.items[0].grandAmount;
                grandTaxVn = result.items[0].grandTaxVn;
                grandVatVn = result.items[0].grandVatVn;
            }
            // var rows = this.createRow(1, grandFob, grandFreight, grandInsurance, grandTax, grandAmount, grandTaxVn, grandVatVn);
            // this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
            this.isLoading = false;
            this.resetGridView()
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
                var grandFob = 0;
                var grandFreight = 0;
                var grandInsurance = 0;
                var grandTax = 0;
                var grandAmount = 0;
                var grandTaxVn = 0;
                var grandVatVn = 0;
                if (result.totalCount > 0) {
                    grandFob = result.items[0].grandFob;
                    grandFreight = result.items[0].grandFreight;
                    grandInsurance = result.items[0].grandInsurance;
                    grandTax = result.items[0].grandTax;
                    grandAmount = result.items[0].grandAmount;
                    grandTaxVn = result.items[0].grandTaxVn;
                    grandVatVn = result.items[0].grandVatVn;
                }
                var rows = this.createRow(1, grandFob, grandFreight, grandInsurance, grandTax, grandAmount, grandTaxVn, grandVatVn);
                this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                this.isLoading = false;
                this.resetGridView()
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerInvoiceToExcel(
            this.billOfLadingNo,
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.supplierNo,
            this.sealNo,
            this.statusId,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
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

    exportCustomsToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvCkdContainerInvoiceCustomsToExcel(
            this.billOfLadingNo,
            this.containerNo,
            this.renban,
            this.invoiceNo,
            this.supplierNo,
            this.sealNo,
            this.statusId,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
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


    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number,
        grandFob: number,
        grandFreight: number,
        grandInsurance: number,
        grandTax: number,
        grandAmount: number,
        grandTaxVn: number,
        grandVatVn: number,
    ): any[] {
        let result: any[] = [];
        console.log(grandFob);
        for (var i = 0; i < count; i++) {
            result.push({
                containerNo: 'Grand Total',
                fob: grandFob,
                freight: grandFreight,
                insurance: grandInsurance,
                tax: grandTax,
                amount: grandAmount,
                taxVnd: grandTaxVn,
                vatVnd: grandVatVn
            });
        }
        return result;
    }
}
