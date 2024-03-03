import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerTransitPortPlanDto, InvCkdContainerTransitPortPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditContainerTransitPortPlanModalComponent } from './create-or-edit-containertransitportplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AbpSessionService } from 'abp-ng2-module';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { ImportContainerTransitPortPlanComponent } from './import-containertransitportplan-modal.component';
import { forEach } from 'lodash-es';
import { DateTime } from 'luxon';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './containertransitportplan.component.html',
})
export class ContainerTransitPortPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalContainerTransitPortPlan', { static: true }) createOrEditModalContainerTransitPortPlan: | CreateOrEditContainerTransitPortPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportContainerTransitPortPlanComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdContainerTransitPortPlanDto = new InvCkdContainerTransitPortPlanDto();

    saveSelectedRow: InvCkdContainerTransitPortPlanDto = new InvCkdContainerTransitPortPlanDto();
    datas: InvCkdContainerTransitPortPlanDto = new InvCkdContainerTransitPortPlanDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    listIdStatus = '';
    arrIdStatus: InvCkdContainerTransitPortPlanDto[] = [];

    containerNo: string = '';
    renban: string = '';
    requestDate: any;
    requestTime: any;
    invoiceNo: string = '';
    billOfLadingNo: string = '';
    supplierNo: string = '';
    sealNo: string = '';
    listCaseNo: string = '';
    listLotNo: string = '';
    cdDate
    transport: string = '';
    status: string = '';
    portCode: string = '';
    portName: string = '';
    remarks: any;
    isActive: any;
    lotNo: string = '';
    moduleCaseNo: string = '';
    caseNo: string = '';
    partNo: string = '';
    isShowConfirm: boolean = true;
    requestDateFrom:any;
    requestDateTo:any;
    receiveDateFrom:any;
    receiveDateTo:any;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    RestrictList = [
        { key: 'R', value: "REQUESTED" },
        { key: 'P', value: "PENDING" },
        { key: 'F', value: "CONFIRM" },
        { key: 'C', value: "CANCEL" },
    ];

    statusList = [
        { value: 'R', label: "REQUESTED" },
        { value: 'P', label: "PENDING" },
        { value: 'F', label: "CONFIRM" },
        { value: 'C', label: "CANCEL" },
    ];

    ordertypeCode: string = '';
    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];


    constructor(
        injector: Injector,
        private _service: InvCkdContainerTransitPortPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _sessionService: AbpSessionService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: "",
                headerTooltip: "",
                field: "checked",
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true,
                pinned: true,
                width: 37
            },
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },

            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                width: 120,
                cellRenderer: 'agSelectRendererComponent',
                list: this.RestrictList,
                cellClass: ['RendererCombobox', 'text-center'],
            },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', width: 120 },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', width: 120 },
            { headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy'), width: 120 },
            { headerName: this.l('Request Time'), headerTooltip: this.l('Request Time'), field: 'requestTime', width: 120 },
            { headerName: this.l('Receive Date'), headerTooltip: this.l('Receive Date'), field: 'receiveDate', valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'), width: 120 },
           // { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', },
           // { headerName: this.l('Module Case No'), headerTooltip: this.l('Module Case No'), field: 'moduleCaseNo', },
           // { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', width: 120 },
            { headerName: this.l('Bill Of Lading No'), headerTooltip: this.l('Bill Of Lading No'), field: 'billOfLadingNo', width: 120 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', width: 120 },
            { headerName: this.l('Seal No'), headerTooltip: this.l('Seal No'), field: 'sealNo', width: 120 },
            { headerName: this.l('List Case No'), headerTooltip: this.l('List Case No'), field: 'listCaseNo', width: 120 },
            { headerName: this.l('List Lot No'), headerTooltip: this.l('List Lot No'), field: 'listLotNo', width: 120 },
            { headerName: this.l('Cd Date'), headerTooltip: this.l('Cd Date'), field: 'cdDate', valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'), width: 120 },
              { headerName: this.l('Customs (VP)'), headerTooltip: this.l('Customs (VP)'), field: 'custums1', width: 120 },
            { headerName: this.l('Customs (HP&Trucking)'), headerTooltip: this.l('Customs (HP&Trucking)'), field: 'custums2', width: 120 },
            { headerName: this.l('Transport'), headerTooltip: this.l('Transport'), field: 'transport', width: 120 },
            { headerName: this.l('Port Code'), headerTooltip: this.l('Port Code'), field: 'portCode', width: 120 },
            { headerName: this.l('Port Name'), headerTooltip: this.l('Port Name'), field: 'portName', width: 120 },
            { headerName: this.l('Remarks'), headerTooltip: this.l('Remarks'), field: 'remarks', width: 120 },

        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        var _d = new Date();
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.invoiceNo,
            this.billOfLadingNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.status,
            this.ordertypeCode,
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
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
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt" && column.getId().toString() != "listcaseNo") {
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
        }, 100)
    }

    clearTextSearch() {
        this.containerNo = '',
        this.renban = '',
        this.supplierNo = '',
        this.invoiceNo = '',
        this.billOfLadingNo = '',
        this.sealNo = '',
        this.requestDateFrom = '',
        this.requestDateTo = '',
        this.receiveDateFrom = '',
        this.receiveDateTo = '',
        this.status = '',
        this.ordertypeCode = '';
        this.lotNo= '';
        this.moduleCaseNo= '';
        this.partNo= '';
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
            this.invoiceNo,
            this.billOfLadingNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.status,
            this.ordertypeCode,
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    checkedBoxRows: number = 0;
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerTransitPortPlanDto[] } }) {
        this.arrIdStatus = params.api.getSelectedRows();
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerTransitPortPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.checkedBoxRows = this.arrIdStatus.length;
        //show/hide button confirm
        var myDate = new Date();
        if (this.selectedRow.requestDate) {
            var date1 = this.selectedRow.requestDate.toString().substring(0, 10);
            var date2 = this._dateTimeService.convertToDatetime(myDate).toString().substring(0, 10);
            this.isShowConfirm = !(this.selectedRow.id && (date1 == date2))
        }

    }

    rowClickData: InvCkdContainerTransitPortPlanDto;
    onRowClick(params) {

        let _rows = document.querySelectorAll<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container .ag-row.ag-row-level-0.ag-row-position-absolute");
        for (let i = 0; _rows[i]; i++) { _rows[i].classList.remove("setcolor_background_rowclick"); }

        if (this.rowClickData && this.rowClickData.id == params.data.id) this.rowClickData = undefined;
        else {
            this.rowClickData = params.data;
            let _row = document.querySelector<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container div[row-id='" + params.node.rowIndex + "'].ag-row.ag-row-level-0.ag-row-position-absolute");
            if (_row) _row.classList.add("setcolor_background_rowclick");
        }

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
            // this.resetGridView();
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

            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                //this.resetGridView()
            });
    }


    conFirmStartusMultiCkb(system: InvCkdContainerTransitPortPlanDto): void {
        this.arrIdStatus.forEach(e => {
            this.listIdStatus += e.id + ',';
        })

        this._service.conFirmStatusMultiCkb(this.listIdStatus, 'F').subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    deleteRow(system: InvCkdContainerTransitPortPlanDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerTransitPortPlanToExcel(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.invoiceNo,
            this.billOfLadingNo,
            this.sealNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.status,
            this.ordertypeCode,
            this.lotNo,
            this.moduleCaseNo,
            this.partNo,
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

    onCellValueChanged(ev) {
        this._service.conFirmStatus(ev.data.id.toString(), ev.newValue).subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });

    }

    searchLateDate(i: number) {

        let _btn = document.querySelector('.actionButton_w' + i);
        if (_btn.classList.contains('active')) {
            _btn.classList.remove('active');
            this.searchDatas();
        } else {
            _btn.classList.add('active');
            this._service.getDataLateDate()
                .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
                .subscribe((result) => {
                    this.paginationParams.totalCount = result.totalCount;
                    this.rowData = result.items;
                    this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                    // this.resetGridView();
                });
        }
    }
}
