import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdBillDto, InvCkdBillServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { rangeRight } from 'lodash-es';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './bill.component.html',
})
export class BillComponent extends AppComponentBase implements OnInit {
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

    changedRecordsBill: number[] = [];
    selectGradeId;
    isRowGrade: boolean = false;
    _billoflandingno: string = '';
    selectedRow: InvCkdBillDto = new InvCkdBillDto();
    saveSelectedRow: InvCkdBillDto = new InvCkdBillDto();
    datas: InvCkdBillDto = new InvCkdBillDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    billofladingNo: string = '';
    billDate: any;
    billDateFrom: any;
    billDateTo: any;
    status
    isActive: string = '';
    statusCode
    body: any;
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
        private _service: InvCkdBillServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Billoflading No'), field: 'billofladingNo', flex: 1 },
            { headerName: this.l('Shipment Id'), headerTooltip: this.l('Shipment Id'), field: 'shipmentId', flex: 1 },
            {
                headerName: this.l('Bill Date'), headerTooltip: this.l('Bill Date'), field: 'billDate',
                valueGetter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'), flex: 1,
                comparator: this._formStoringService.dateComparator,
            },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', flex: 1}
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
            this.changedRecordsBill = result;
            console.log("result =", result);
        })
    }

    searchDatas(): void {
        this._service.getAll(
            this.billofladingNo,
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
            });
    }

    clearTextSearch() {
        this.billofladingNo = '',
            this.billDateFrom = '',
            this.billDateTo = '',
            this.ckdPio = 'C',
            this.ordertypeCode = '';
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.billofladingNo,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdBillDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdBillDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectGradeId = this.selectedRow.id;
        this.isRowGrade = (this.selectedRow.id != null ? true : false);
        this._billoflandingno = this.selectedRow.billofladingNo;
        console.log("id =", this.selectGradeId);
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
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getBillToExcel(
            this.billofladingNo,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });

    }
}
