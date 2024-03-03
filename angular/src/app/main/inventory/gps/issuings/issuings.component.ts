import { GridApi } from '@ag-grid-enterprise/all-modules';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetGpsIssuingDetailInput, GetGpsIssuingRequetCreateInput, InvGpsIssuingsDetails, InvGpsIssuingsHeaderDto, InvGpsIssuingsServiceProxy, InvGpsPartListDto, InvGpsPartListServiceProxy, OnlineBudgetCheckRequest, SapIFServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs';
import { ImportInvGpsIssuingsComponent } from './import-issuings-modal.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { BrandComponent } from '@app/main/master/common/brand/brand.component';
import { NumericEditor } from '@app/shared/common/numeric-cell-editor/NumericEditor';
import { log } from 'console';
import { ViewLoggingResponseBudgetCheckComponent } from './view-logging-response-budget-check-modal.component';
import { ViewFundCommmitmentItemDMComponent } from './view-fund-commitment-item-modal.component';

@Component({
    selector: 'app-issuings',
    templateUrl: './issuings.component.html',
    styleUrls: ['./issuings.component.less']
})
export class IssuingsComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewFundCommitmentItem', { static: true }) viewFundCommitmentItem: ViewFundCommmitmentItemDMComponent | undefined;
    @ViewChild('importInvGpsIssuing', { static: true }) importInvGpsIssuing: | ImportInvGpsIssuingsComponent | undefined;
    @ViewChild('viewLoggingResponseBudgetCheck', { static: true }) viewLoggingResponseBudgetCheck: ViewLoggingResponseBudgetCheckComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    paginationParamsDetail: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    listColDefs: any;
    gradeColDefs: any;

    selectedRow: InvGpsIssuingsHeaderDto = new InvGpsIssuingsHeaderDto();
    saveSelectedRow: InvGpsIssuingsHeaderDto = new InvGpsIssuingsHeaderDto();
    selectedRowDetail: InvGpsIssuingsDetails = new InvGpsIssuingsDetails();
    listDto: InvGpsPartListDto = new InvGpsPartListDto();
    datas: InvGpsPartListDto = new InvGpsPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParams_PartGrade: GridParams | undefined;
    rowData: any[] = [];
    rowDataDetails: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    documentNo;
    documentDate;
    documentDateNew;
    shop;
    team;
    costCenter;
    documentId = '';
    partNo = '';
    lotNo = '';
    issueFromDate;
    issueToDate;
    requestFromDate;
    requestToDate;
    p_id_header: any;
    p_id_detail: any;
    selectHeaderId;
    selectDetailId;
    selectedRowDetails;
    qtyRequest;
    requestDate;
    detailsId;
    qtyIssue;
    issueDate;
    status;
    isDataValidation: boolean = false;
    isBudgetSuccess: boolean = false;
    AvailableBudgetAvailableAmount;
    listWbs = [];
    listPart = [];
    listData = [];
    changedRecordsHeader: number[] = [];
    changedRecordsDetail: number[] = [];
    listErrBudget = '';
    partName;
    costCenterNew;
    wbsdetail;
    listId = [];
    statusFilter: any;
    dateCurent = new Date();
    PartNo = '';
    listDataDetails: InvGpsIssuingsDetails = new InvGpsIssuingsDetails();
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
    detailId;
    todayfilter: any;
    rowDataRequestCreate: GetGpsIssuingRequetCreateInput = new GetGpsIssuingRequetCreateInput();
    rowDataDetail: GetGpsIssuingDetailInput = new GetGpsIssuingDetailInput();
    budgetCheckMessage;
    fundCommitmentMessage;
    idFund: Number;
    ListGpsIssuing = [
        { key: 'NEW', value: "NEW" },
        { key: 'REQUESTED', value: "REQUESTED" },
        { key: 'SUBMITED', value: "SUBMITED" },
        { key: 'APPROVED', value: "APPROVED" },
        { key: 'CANCELLED', value: "CANCELLED" },
        { key: 'GPS APPROVED', value: "GPS APPROVED" },
        { key: 'GPS CANCELLED', value: "GPS CANCELLED" },
    ];

    listGpsIssuingDetails = [
        // { key: 'NEW', value: "NEW" },
        // { key: 'REQUESTED', value: "REQUESTED" },
        // { key: 'SUBMITED', value: "SUBMITED" },
        { key: 'APPROVED', value: "APPROVED" },
        //{ key: 'CANCELLED', value: "CANCELLED" },
        { key: 'ISSUED', value: "ISSUED" },
        { key: 'GPS CANCELLED', value: "GPS CANCELLED" },
    ];

    _permiss_MemberShop: any;
    _permiss_LeaderShop: any;
    _permiss_LeaderGps: any;
    isCheckBudget: string = '';
    isFunCommitment: string = '';
    documentNoN;
    checkFun: boolean = true;
    constructor(
        injector: Injector,
        private _service: InvGpsIssuingsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private dataFormatService: DataFormatService,
        private _sapService: SapIFServiceProxy,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.listColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', width: 120 },
            { headerName: this.l('Document No'), headerTooltip: this.l('Document No'), field: 'documentNo', flex: 1 },
            { headerName: this.l('Document Date'), headerTooltip: this.l('Document Date'), field: 'documentDate', flex: 1 },
            { headerName: this.l('Employee Code'), headerTooltip: this.l('Employee Code'), field: 'employeeCode', flex: 1 },
            { headerName: this.l('User Request'), headerTooltip: this.l('UserRequest'), field: 'userRequest', flex: 1 },
            { headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy') },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1 },
            { headerName: this.l('Team'), headerTooltip: this.l('Team'), field: 'team', flex: 1 },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },



        ];
        this.gradeColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDetail.pageSize * (this.paginationParamsDetail.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'status', width: 120 },
            { headerName: this.l('Document No'), headerTooltip: this.l('Document No'), field: 'documentNo', flex: 1 },
            { headerName: this.l('Document Date'), headerTooltip: this.l('Document Date'), field: 'documentDate', flex: 1, },
            { headerName: this.l('Item No'), headerTooltip: this.l('Item No'), field: 'itemNo', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Uom'), headerTooltip: this.l('Uom'), field: 'uom', flex: 1, },
            {
                headerName: this.l('Qty Request'),
                headerTooltip: this.l('Qty Request'),
                field: 'qtyRequest',
                flex: 1,
                type: 'rightAligned',
                editable: (params) => {
                    if (params.data.status == 'NEW' && this.permission.isGranted('Pages.GPS.Issuings.MemberShop')) return true;
                    else return false;
                },
                cellStyle: params => {
                    if (this.permission.isGranted('Pages.GPS.Issuings.MemberShop') && (params.data.status == 'NEW')) {
                        return { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' };
                    }
                },
                cellEditor: NumericEditor,
            },
            { headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/YYYY'), },
            {
                headerName: this.l('Qty Reject'),
                headerTooltip: this.l('Qty Reject'),
                field: 'qtyReject',
                flex: 1,
                type: 'rightAligned',
                editable: (params) => {
                    if ((params.data.status == 'SUBMITED' || params.data.status == 'CANCELLED') && params.data.headerStatus == "SUBMITED" && this.permission.isGranted('Pages.GPS.Issuings.LeaderShop')) return true;
                    else return false;
                },
                cellStyle: params => {
                    console.log(params);
                    if ((params.data.status == 'SUBMITED' || params.data.status == 'CANCELLED') && params.data.headerStatus == "SUBMITED" && this.permission.isGranted('Pages.GPS.Issuings.LeaderShop')) {
                        return { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' };
                    }
                },
                cellEditor: NumericEditor,
            },
            {
                headerName: this.l('Qty Issue'),
                headerTooltip: this.l('Qty Issue'),
                field: 'qtyIssue',
                flex: 1,
                type: 'rightAligned',
                editable: (params) => {
                    if ((params.data.status == 'APPROVED' || params.data.status == 'GPS CANCELLED') && params.data.headerStatus == "APPROVED" && this.permission.isGranted('Pages.GPS.Issuings.LeaderGps')) return true;
                    else return false;
                },
                cellStyle: params => {
                    if ((params.data.status == 'APPROVED' || params.data.status == 'GPS CANCELLED') && params.data.headerStatus == "APPROVED" && this.permission.isGranted('Pages.GPS.Issuings.LeaderGps')) {
                        return { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' };
                    }
                },
                cellEditor: NumericEditor,
            },

            { headerName: this.l('Issue Date'), headerTooltip: this.l('IssueDate'), field: 'issueDate', flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.issueDate, 'dd/MM/yyyy') },

            {
                headerName: this.l('Price'),
                headerTooltip: this.l('Price'),
                field: 'price',
                flex: 1,
                type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.price, 2)
            },
            {
                headerName: this.l('Amount'),
                headerTooltip: this.l('Amount'),
                field: 'amount',
                flex: 1,
                type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 2)
            },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Wbs'), headerTooltip: this.l('Wbs'), field: 'wbs', flex: 1 },
            { headerName: this.l('Cost Center Mapping'), headerTooltip: this.l('Cost Center Mapping'), field: 'costCenterMapping', flex: 1 },
            { headerName: this.l('Wbs Mapping'), headerTooltip: this.l('Wbs Mapping'), field: 'wbsMapping', flex: 1 },
            { headerName: this.l('Gl Account'), headerTooltip: this.l('Gl Account'), field: 'glAccount', flex: 1 },
            { headerName: this.l('Earmark Fund'), headerTooltip: this.l('Earmark Fund'), field: 'earmarkFund', flex: 1, },
            { headerName: this.l('Earmark Fund Item'), headerTooltip: this.l('Earmark Fund Item'), field: 'earmarkFundItem', flex: 1, },
            { headerName: this.l('Budget Check Message'), headerTooltip: this.l('Budget Check Message'), field: 'budgetCheckMessage', flex: 1, },
            { headerName: this.l('Fund Commitment Message'), headerTooltip: this.l('Fund Commitment Message'), field: 'fundCommitmentMessage', flex: 1 },
            { headerName: this.l('Fin Status'), headerTooltip: this.l('Fin Status'), field: 'finStatus', flex: 1 },
            { headerName: this.l('Trans Type'), headerTooltip: this.l('Trans Type'), field: 'transType', flex: 1 },
            { headerName: this.l('Category'), headerTooltip: this.l('Category'), field: 'catgory', flex: 1 },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType', flex: 1 },
            { headerName: this.l('Location'), headerTooltip: this.l('Location'), field: 'location', flex: 1 },
            { headerName: this.l('Is Budget Check'), headerTooltip: this.l('Is Budget Check'), field: 'isBudgetCheck', flex: 1 },
        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
            agDatepickerRendererComponent: AgDatepickerRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsDetail = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._permiss_MemberShop = this.permission.isGranted('Pages.GPS.Issuings.MemberShop');
        this._permiss_LeaderShop = this.permission.isGranted('Pages.GPS.Issuings.LeaderShop');
        this._permiss_LeaderGps = this.permission.isGranted('Pages.GPS.Issuings.LeaderGps');
        this._service.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG_IS_ENABLE_BUDGET_CHECK').subscribe((e) => {
            this.isCheckBudget = e.itemValue;
        });

        this._service.getItemValue('INV_GPS_ISSUNG', 'GPS_ISSUNG_IS_ENABLE_BUDGET_CHECK').subscribe((e) => {
            this.isFunCommitment = e.itemValue;
        });
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsHeader = result.issuingHeader;
            this.changedRecordsDetail = result.issuingDetail;
            // this.changedRecordsGradeColorDetail = result.gradeColorDetail;
            console.log("result =", result.issuingHeader);
            console.log("result Detail =", result.issuingDetail);

        })
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
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

    autoSizeAll_PartGrade() {
        const allColumnIds: string[] = [];
        this.dataParams_PartGrade.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams_PartGrade.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView_PartGrade() {
        setTimeout(() => {
            this.dataParams_PartGrade.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll_PartGrade();
        }, 100)
    }

    clearTextSearch() {
        this.partNo = '',
            this.documentDate = '';
        this.documentNo = '';
        this.shop = '';
        this.team = '';
        this.costCenter = '';
        this.partNo = '';
        this.lotNo = '';
        this.issueFromDate = '';
        this.issueToDate = '';
        this.requestFromDate = '';
        this.requestToDate = '';
        this.statusFilter = '';
        this.todayfilter = '';
        this.searchDatas();
    }

    searchbyStatus(s) {
        if (s == 'R') {
            if (document.querySelector(".disableBtn")) {
                document.querySelector(".disableBtn").classList.remove('disableBtn');
                this.clearTextSearch()
            } else if (document.querySelector(".disableBtnIssued")) {
                document.querySelector(".disableBtnIssued").classList.remove('disableBtnIssued');
                var btnRequested = document.querySelector(".requestedBtn");
                btnRequested.classList.add("disableBtn");
                this.statusFilter = 'REQUESTED';
                this.searchDatas();
            } else if (document.querySelector(".disableBtnToday")) {
                document.querySelector(".disableBtnToday").classList.remove('disableBtnToday');
                var btnRequested = document.querySelector(".requestedBtn");
                btnRequested.classList.add("disableBtn");
                this.todayfilter = "";
                this.statusFilter = 'REQUESTED';
                this.searchDatas();
            }
            else {
                var btnRequested = document.querySelector(".requestedBtn");
                btnRequested.classList.add("disableBtn");
                this.statusFilter = 'REQUESTED';
                this.searchDatas();
            }
        } else if (s == 'I') {
            if (document.querySelector(".disableBtnIssued")) {
                document.querySelector(".disableBtnIssued").classList.remove('disableBtnIssued');
                this.clearTextSearch()
            } else if (document.querySelector(".disableBtn")) {
                document.querySelector(".disableBtn").classList.remove('disableBtn');
                var btnIssued = document.querySelector(".issuedBtn");
                btnIssued.classList.add("disableBtnIssued");
                this.statusFilter = 'GPS APPROVED';
                this.searchDatas();
            } else if (document.querySelector(".disableBtnToday")) {
                document.querySelector(".disableBtnToday").classList.remove('disableBtnToday');
                var btnIssued = document.querySelector(".issuedBtn");
                btnIssued.classList.add("disableBtnIssued");
                this.statusFilter = 'GPS APPROVED';
                this.todayfilter = "";
                this.searchDatas();
            }
            else {
                var btnIssued = document.querySelector(".issuedBtn");
                btnIssued.classList.add("disableBtnIssued");
                this.statusFilter = 'GPS APPROVED';
                this.searchDatas();
            }

        } else if (s == 'D') {
            if (document.querySelector(".disableBtnToday")) {
                document.querySelector(".disableBtnToday").classList.remove('disableBtnToday');
                this.clearTextSearch();
            } else if (document.querySelector(".disableBtn")) {
                var btnToday = document.querySelector(".todayBtn");
                btnToday.classList.add("disableBtnToday");
                this.todayfilter = this.pipe.transform(this.dateCurent, 'yyyyMMdd');
                this.statusFilter = '';
                this.searchDatas();
                document.querySelector(".disableBtn").classList.remove('disableBtn');

            } else if (document.querySelector(".disableBtnIssued")) {
                var btnToday = document.querySelector(".todayBtn");
                btnToday.classList.add("disableBtnToday");
                this.todayfilter = this.pipe.transform(this.dateCurent, 'yyyyMMdd');
                this.statusFilter = '';
                this.searchDatas();

                document.querySelector(".disableBtnIssued").classList.remove('disableBtnIssued');
            }
            else {
                var btnToday = document.querySelector(".todayBtn");
                btnToday.classList.add("disableBtnToday");
                this.todayfilter = this.pipe.transform(this.dateCurent, 'yyyyMMdd');
                this.searchDatas();
            }
        }


    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        if (this.documentDate) {
            this.documentDateNew = this.pipe.transform(this.documentDate, 'yyyyMMdd');
        }
        this._service.getAll(
            this.documentNo,
            this.documentDateNew,
            this.shop,
            this.team,
            this.costCenter,
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.issueFromDate),
            this._dateTimeService.convertToDatetime(this.issueToDate),
            this._dateTimeService.convertToDatetime(this.requestFromDate),
            this._dateTimeService.convertToDatetime(this.requestToDate),
            this.statusFilter,
            this.todayfilter,
            this._permiss_MemberShop,
            this._permiss_LeaderShop,
            this._permiss_LeaderGps,

            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
                if (result.totalCount == 0) {
                    this.rowDataDetails = [];
                } else {
                    this.searchDataDetails(result.items[0].id);
                }
            });
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        if (this.documentDate) {
            this.documentDateNew = this.pipe.transform(this.documentDate, 'yyyyMMdd');
        }
        return this._service.getAll(
            this.documentNo,
            this.documentDateNew,
            this.shop,
            this.team,
            this.costCenter,
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.issueFromDate),
            this._dateTimeService.convertToDatetime(this.issueToDate),
            this._dateTimeService.convertToDatetime(this.requestFromDate),
            this._dateTimeService.convertToDatetime(this.requestToDate),
            this.statusFilter,
            this.todayfilter,
            this._permiss_MemberShop,
            this._permiss_LeaderShop,
            this._permiss_LeaderGps,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsIssuingsHeaderDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id_header = selected.id;
            this.searchDataDetails(selected.id);
            this.status = selected.status;
            this.costCenterNew = selected.costCenter;
            this.documentNoN = selected.documentNo;
            this.checkFun = true;
        }
        this.selectedRow = Object.assign({}, selected);
        this.selectHeaderId = this.selectedRow.id;
        console.log("ID = ", this.selectHeaderId)
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
            //this.resetGridView();

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
                // this.resetGridView();

            });
    }

    callBackDataGrid_Details(params: GridParams) {
        this.dataParams_PartGrade = params;
    }

    searchDataDetails(p_id): void {
        this.isLoading = true;

        this._service.getAllDetails(
            this.p_id_header,
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.issueFromDate),
            this._dateTimeService.convertToDatetime(this.issueToDate),
            this._dateTimeService.convertToDatetime(this.requestFromDate),
            this._dateTimeService.convertToDatetime(this.requestToDate),
            this._permiss_MemberShop,
            this._permiss_LeaderShop,
            this._permiss_LeaderGps,
            '',
            this.paginationParamsDetail.skipCount,
            this.paginationParamsDetail.pageSize
        )
            .pipe(finalize(() => { }
            )).subscribe((result) => {
                this.paginationParamsDetail.totalCount = result.totalCount;
                this.rowDataDetails = result.items;
                this.paginationParamsDetail.totalPage = ceil(result.totalCount / (this.paginationParamsDetail.pageSize ?? 0));
                this.resetGridView_PartGrade();
                this.listPart = [];
                this.listWbs = [];
                this.isLoading = false;
            });
    }

    getDataDetails(paginationParams?: PaginationParamsModel) {
        return this._service.getAllDetails(
            this.p_id_header,
            this.partNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.issueFromDate),
            this._dateTimeService.convertToDatetime(this.issueToDate),
            this._dateTimeService.convertToDatetime(this.requestFromDate),
            this._dateTimeService.convertToDatetime(this.requestToDate),
            this._permiss_MemberShop,
            this._permiss_LeaderShop,
            this._permiss_LeaderGps,
            '',
            this.paginationParamsDetail.skipCount,
            this.paginationParamsDetail.pageSize
        );
    }

    changePageDetails(paginationParams) {
        this.isLoading = true;
        this.paginationParamsDetail = paginationParams;
        this.paginationParamsDetail.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataDetails(this.paginationParamsDetail).subscribe((result) => {
            this.paginationParamsDetail.totalCount = result.totalCount;
            this.rowDataDetails = result.items;
            this.paginationParamsDetail.totalPage = ceil(result.totalCount / (this.paginationParamsDetail.pageSize ?? 0));
            this.isLoading = false;
            // this.resetGridView();
            this.resetGridView_PartGrade();
        });
    }

    callBackDataGridDetails(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsDetail.pageSize);
        this.paginationParamsDetail.skipCount =
            ((this.paginationParamsDetail.pageNum ?? 1) - 1) * (this.paginationParamsDetail.pageSize ?? 0);
        this.paginationParamsDetail.pageSize = this.paginationParamsDetail.pageSize;
        this.getDataDetails(this.paginationParamsDetail)

            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))

            .subscribe((result) => {
                this.paginationParamsDetail.totalCount = result.totalCount;
                this.rowDataDetails = result.items ?? [];
                this.paginationParamsDetail.totalPage = ceil(result.totalCount / (this.paginationParamsDetail.pageSize ?? 0));
                this.isLoading = false;
                // this.resetGridView();
                this.resetGridView_PartGrade();

            });
    }


    fn: CommonFunction = new CommonFunction();

    onChangeButton(e) {
        this.listErrBudget = '';
        var totalAmount = 0;
        var listId = [];
        var listIdFunc = [];
        var listCheckBudget = [];
        var wbsMasterData;
        var costCenter;
        this.listData = [];
        listCheckBudget = this.rowDataDetails.filter(x => x.status != 'CANCELLED');

        var month = Number(new Date().getMonth() + 1);
        var year = Number(new Date().getFullYear());
        var _fiscalYear = month > 4 ? (year + 1) : year;

        //listIdFunc
        for (var i = 0; i < listCheckBudget.length; i++) {
            listIdFunc.push(listCheckBudget[i].id);
        }

        //distinct costcenter and wbs
        var listWbs = listCheckBudget.filter((value, index, selfArray) =>
            index === selfArray.findIndex((p) => (
                p.wbsMapping === value.wbsMapping && p.costCenterMapping === value.costCenterMapping
            ))
        );

        console.log(listWbs);
        // //list id and totalAmount
        for (var j = 0; j < listWbs.length; j++) {
            //clear data
            totalAmount = 0;
            listId = [];
            wbsMasterData = '';
            costCenter = '';

            //group Wbs
            for (var i = 0; i < listCheckBudget.length; i++) {
                if (listWbs[j].wbsMapping == listCheckBudget[i].wbsMapping && listWbs[j].costCenterMapping == listCheckBudget[i].costCenterMapping) {
                    totalAmount = totalAmount + Number(listCheckBudget[i].amount)
                    listId.push(listCheckBudget[i].id);
                    wbsMasterData = listCheckBudget[i].wbsMapping;
                    costCenter = listCheckBudget[i].costCenterMapping;
                }
            }
            //push to list
            this.listData.push({ wbs: wbsMasterData, costCenter: costCenter, totalAmout: totalAmount, listId: listId })
        }

        //check budget and funcommitment
        if (this.isCheckBudget === 'Y') {
            console.log('check budget and funcommitment');
            switch (e) {
                case 'R':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Request không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            for (var n = 0; n < this.listData.length; n++) {
                                console.log('x0' + n.toString());
                                let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                                    wbsMasterData: this.listData[n].wbsMapping,
                                    fiscalYear: _fiscalYear.toString(),
                                    costCenter: this.listData[n].costCenterMapping,
                                    fixedAssetNo: '',
                                    system: 'MMS',
                                    listItemId: this.listData[n].listId,
                                    index1: n,
                                    totalAmount: this.listData[n].totalAmout
                                })

                                console.log('x1' + obj.index1.toString());
                                this._sapService.sapOnlineBudgetCheck(obj)
                                    .pipe(finalize(() => { }))
                                    .subscribe((budget) => {
                                        console.log(budget);
                                        this.isDataValidation = budget.response.dataValidation.dataValidationResult == 'T' ? true : false
                                        this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false
                                        console.log(this.isDataValidation);
                                        console.log('x2' + obj.index1.toString());
                                        if (this.isDataValidation) {
                                            if (this.isBudgetSuccess) {
                                                console.log(budget.response.availableBudget.availableBudgetAvailableAmount * 100);
                                                console.log(obj.totalAmount);
                                                if (budget.response.availableBudget.availableBudgetAvailableAmount * 100 > Number(obj.totalAmount)) {
                                                    console.log('sr' + this.listErrBudget);
                                                    console.log('s2ss' + this.listErrBudget);
                                                    console.log('s2ss1' + (this.listData.length - 1).toString());
                                                    console.log('s2ss1' + (obj.index1).toString());
                                                    if (this.listErrBudget == '' && this.listData.length - 1 == obj.index1) {
                                                        console.log('sss' + this.listErrBudget);
                                                        console.log('sss1' + (this.listData.length - 1).toString());
                                                        console.log('sss1' + (obj.index1).toString());

                                                        this.rowDataRequestCreate.headerId = this.p_id_header,
                                                            this.rowDataRequestCreate.budgetCheckMessage = budget.response.availableBudget.availableBudgetMessage,
                                                            this._service.gpsIssuingRequestCreate(this.rowDataRequestCreate)
                                                                .pipe(finalize(() => { }))
                                                                .subscribe((res) => {
                                                                    this.notify.info(this.l('Create Request Successfully'));
                                                                    this.searchDatas();
                                                                });
                                                    } else {
                                                        if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                            console.log('er' + this.listErrBudget);
                                                            this.message.confirm(this.listErrBudget);
                                                            this.isLoading = false;
                                                            return;
                                                        }
                                                    }

                                                }
                                                else {
                                                    console.log('3a');

                                                    //this.notify.warn(this.l('Budget is not enough. The remaining budget is: ' + budget.response.availableBudget.availableBudgetAvailableAmount));
                                                    this.listErrBudget = this.listErrBudget
                                                        + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                        + 'CostCenter: ' + obj.costCenter + ' - '
                                                        + 'Budget is not enough. The remaining budget is: ' + budget.response.availableBudget.availableBudgetAvailableAmount * 100 + ';';
                                                    //   this.isLoading = false;
                                                    //  return;

                                                    console.log('3' + this.listErrBudget);
                                                    console.log(this.listData.length - 1);
                                                    console.log(n);

                                                    // return;
                                                    if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                        console.log(this.listErrBudget);
                                                        this.message.confirm(this.listErrBudget);
                                                        this.isLoading = false;
                                                        return;
                                                    }
                                                }
                                            }
                                            else {
                                                console.log('2a');
                                                this.listErrBudget = this.listErrBudget
                                                    + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                    + 'CostCenter: ' + obj.costCenter
                                                    + ' => ' + budget.response.availableBudget.availableBudgetMessage
                                                    + ';';
                                                console.log('2' + this.listErrBudget);
                                                console.log(this.listData.length - 1);
                                                console.log(n);

                                                // return;
                                                if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                    console.log(this.listErrBudget);
                                                    this.message.confirm(this.listErrBudget);
                                                    this.isLoading = false;
                                                    return;
                                                }
                                            }
                                        }
                                        else {
                                            console.log('x5' + obj.index1.toString());
                                            console.log('1a');
                                            // this.notify.warn(this.l('Budget invalid'));
                                            console.log(budget.response.dataValidation.dataValidationMessage);
                                            this.listErrBudget = this.listErrBudget
                                                + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                + 'CostCenter: ' + obj.costCenter
                                                + '=>' + budget.response.dataValidation.dataValidationMessage
                                                + ';';
                                            //  this.isLoading = false;
                                            console.log('1' + this.listErrBudget);
                                            console.log(this.listData.length - 1);
                                            console.log('x3' + n.toString());


                                            // return;
                                            if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                console.log(this.listErrBudget);
                                                this.message.confirm(this.listErrBudget);
                                                this.isLoading = false;
                                                return;
                                            }

                                        }
                                    })
                            }

                        }
                    });
                    break;

                case 'S':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Submit không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            console.log(this.listData.length);

                            for (var n = 0; n < this.listData.length; n++) {
                                console.log('x0' + n.toString());
                                let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                                    wbsMasterData: this.listData[n].wbsMapping,
                                    fiscalYear: _fiscalYear.toString(),
                                    costCenter: this.listData[n].costCenterMapping,
                                    fixedAssetNo: '',
                                    system: 'MMS',
                                    listItemId: this.listData[n].listId,
                                    index1: n,
                                    totalAmount: this.listData[n].totalAmout
                                })
                                console.log(obj);

                                this._sapService.sapOnlineBudgetCheck(obj)
                                    .pipe(finalize(() => { }))
                                    .subscribe((budget) => {
                                        console.log(budget);
                                        console.log('x1' + n.toString());
                                        this.isDataValidation = budget.response.dataValidation.dataValidationResult == 'T' ? true : false
                                        this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false
                                        console.log(this.isBudgetSuccess);
                                        console.log(this.isDataValidation);
                                        if (this.isDataValidation) {
                                            console.log('x2' + obj.index1.toString());
                                            console.log(budget.response.availableBudget.availableBudgetAvailableAmount);
                                            console.log(totalAmount);
                                            if (this.isBudgetSuccess) {
                                                if (budget.response.availableBudget.availableBudgetAvailableAmount * 100 > Number(obj.totalAmount)) {
                                                    console.log(this.isFunCommitment);
                                                    if (this.listErrBudget == '' && this.listData.length - 1 == obj.index1) {
                                                        if (this.isFunCommitment == 'Y') {
                                                            console.log('run funcommit');
                                                            console.log(listIdFunc);
                                                            this._sapService.submitFundCommitmentRequest('', 'INSERTED', listIdFunc)
                                                                .subscribe((sub) => {
                                                                    console.log(sub);
                                                                    this._service.gpsIssuingChangeStatus(this.p_id_header, 'SUBMITED')
                                                                        .pipe(finalize(() => { }))
                                                                        .subscribe((res) => {
                                                                            console.log('x3' + obj.index1.toString());

                                                                            console.log('ss' + this.listErrBudget);
                                                                            this.notify.info(this.l('Submit Successfully'));
                                                                            this.searchDatas();

                                                                        });
                                                                })
                                                        }
                                                    }
                                                }

                                                else {
                                                    console.log('x4' + obj.index1.toString());
                                                    console.log('Budget is not enough. The remaining budget is');

                                                    this.listErrBudget = this.listErrBudget
                                                        + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                        + 'CostCenter: ' + obj.costCenter + ' - '
                                                        + 'Budget is not enough. The remaining budget is: ' + budget.response.availableBudget.availableBudgetAvailableAmount * 100 + ';';

                                                    console.log(this.listData.length - 1);
                                                    console.log(obj.index1);

                                                    // return;
                                                    if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                        console.log(this.listErrBudget);
                                                        this.message.confirm(this.listErrBudget);
                                                        this.isLoading = false;
                                                        return;
                                                    }

                                                }
                                            } else {
                                                console.log('x5' + obj.index1.toString());
                                                this.listErrBudget = this.listErrBudget
                                                    + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                    + 'CostCenter: ' + obj.costCenter
                                                    + ' => ' + budget.response.availableBudget.availableBudgetMessage
                                                    + ';';
                                                console.log(this.listData.length - 1);
                                                console.log(n);

                                                // return;
                                                if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                    console.log(this.listErrBudget);
                                                    this.message.confirm(this.listErrBudget);
                                                    this.isLoading = false;
                                                    return;
                                                }
                                            }
                                        }
                                        else {
                                            console.log('x6' + obj.index1.toString());
                                            console.log('Budget invalid');
                                            this.listErrBudget = this.listErrBudget
                                                + 'Wbs: ' + obj.wbsMasterData + ' - '
                                                + 'CostCenter: ' + obj.costCenter
                                                + '=>' + budget.response.dataValidation.dataValidationMessage
                                                + ';';
                                            console.log(this.listData.length - 1);
                                            console.log(n);

                                            // return;
                                            if (this.listErrBudget != '' && this.listData.length - 1 == obj.index1) {
                                                console.log(this.listErrBudget);
                                                this.message.confirm(this.listErrBudget);
                                                this.isLoading = false;
                                                return;
                                            }

                                        }

                                    })
                            }
                        }
                    });
                    break;

                case 'A':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Approved không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            if (this.status == 'SUBMITED') {
                                this._service.gpsIssuingChangeStatus(this.p_id_header, 'APPROVED')
                                    .pipe(finalize(() => { }))
                                    .subscribe((res) => {
                                        this.notify.info(this.l('Approved Successfully'));
                                        this.searchDatas();
                                    });
                            }
                            else {
                                this.notify.warn(this.l('Document has not been sent'));
                                this.searchDatas();
                            }
                        }
                    });
                    break;

                case 'C':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Cancel không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            if (this.status == 'SUBMITED') {
                                if (this.isFunCommitment == 'Y') {
                                    console.log(listIdFunc);
                                    this._sapService.submitFundCommitmentRequest('', 'CANCELED', listIdFunc)
                                        .subscribe((sub) => {
                                            this._service.gpsIssuingChangeStatus(this.p_id_header, 'CANCELLED')
                                                .pipe(finalize(() => { }))
                                                .subscribe((res) => {
                                                    this.notify.info(this.l('Cancelled Successfully'));
                                                    this.searchDatas();
                                                });
                                        });
                                }
                            }
                            else {
                                this.notify.warn(this.l('Document has not been sent'));
                                this.searchDatas();
                            }
                        }
                    });
                    break;

                case 'GPSA':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Approved không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            this._service.gpsIssuingChangeStatus(this.p_id_header, 'GPS APPROVED')
                                .pipe(finalize(() => { }))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Gps Approve Successfully'));
                                    this.searchDatas();
                                });
                        }
                    });
                    break;

                case 'GPSC':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Cancel không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            if (this.isFunCommitment == 'Y') {
                                console.log(listIdFunc);
                                this._sapService.submitFundCommitmentRequest('', 'CANCELED', listIdFunc)
                                    .subscribe((sub) => {
                                        this._service.gpsIssuingChangeStatus(this.p_id_header, 'GPS CANCELLED')
                                            .pipe(finalize(() => { }))
                                            .subscribe((res) => {
                                                this.notify.info(this.l('Gps Cancelled Successfully'));
                                                this.searchDatas();
                                            });
                                    });

                            }
                        }
                    });
                    break;
            }
        }
        else {
            // not check budget and not funcommitment
            console.log('not check budget and not funcommitment');
            switch (e) {
                case 'R':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Request không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            this.rowDataRequestCreate.headerId = this.p_id_header,
                                this._service.gpsIssuingRequestCreate(this.rowDataRequestCreate)
                                    .pipe(finalize(() => { }))
                                    .subscribe((res) => {
                                        this.notify.info(this.l('Create Request Successfully'));
                                        this.searchDatas();
                                    });
                        }
                    });
                    break;

                case 'S':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Submit không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            this._service.gpsIssuingChangeStatus(this.p_id_header, 'SUBMITED')
                                .pipe(finalize(() => { }))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Submit Successfully'));
                                    this.searchDatas();
                                });
                        }
                    });
                    break;

                case 'A':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Approved không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            if (this.status == 'SUBMITED') {
                                this._service.gpsIssuingChangeStatus(this.p_id_header, 'APPROVED')
                                    .pipe(finalize(() => { }))
                                    .subscribe((res) => {
                                        this.notify.info(this.l('Approved Successfully'));
                                        this.searchDatas();
                                    });
                            }
                            else {
                                this.notify.warn(this.l('Document has not been sent'));
                                this.searchDatas();
                            }
                        }
                    });
                    break;

                case 'C':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Cancel không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            if (this.status == 'SUBMITED') {
                                this._service.gpsIssuingChangeStatus(this.p_id_header, 'CANCELLED')
                                    .pipe(finalize(() => { }))
                                    .subscribe((res) => {
                                        this.notify.info(this.l('Cancelled Successfully'));
                                        this.searchDatas();
                                    });
                            }
                            else {
                                this.notify.warn(this.l('Document has not been sent'));
                                this.searchDatas();
                            }
                        }
                    });
                    break;

                case 'GPSA':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Approved không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            this._service.gpsIssuingChangeStatus(this.p_id_header, 'GPS APPROVED')
                                .pipe(finalize(() => { }))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Gps Approve Successfully'));
                                    this.searchDatas();
                                });
                        }
                    });
                    break;

                case 'GPSC':
                    this.message.confirm(this.l(' Bạn có chắc chắn muốn Cancel không?'), this.l('AreYouSure'), (isConfirmed) => {
                        if (isConfirmed) {
                            this.isLoading = true;
                            this._service.gpsIssuingChangeStatus(this.p_id_header, 'GPS CANCELLED')
                                .pipe(finalize(() => { }))
                                .subscribe((res) => {
                                    this.notify.info(this.l('Gps Cancelled Successfully'));
                                    this.searchDatas();
                                });
                        }
                    });
                    break;
            }
        }
    }

    onChangeRowSelectionDetails(params: { api: { getSelectedRows: () => InvGpsIssuingsDetails[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id_detail = selected.id;
            this.qtyIssue = selected.qtyRequest;
            this.issueDate = selected.requestDate;
            this.detailId = selected.id;
            this.wbsdetail = selected.wbs;
            this.qtyRequest = selected.qtyRequest;
            this.PartNo = selected.partNo;
            this.idFund = selected.id;
            this.documentNoN = selected.documentNo;
            this.checkFun = false;
        }
        this.selectedRowDetail = Object.assign({}, selected);
        this.selectDetailId = this.selectedRowDetail.id
        console.log("ID Detail = ", this.selectDetailId)

    }
    onCellValueChangedDetails(event) {
        console.log(event);
        var listId = [];

        //this.isLoading = true;
        var month = Number(new Date().getMonth() + 1);
        var year = Number(new Date().getFullYear());
        var _fiscalYear = month > 4 ? (year + 1) : year;
        this.detailsId = event.data.id;
        this.qtyRequest = event.data.qtyRequest;
        this.requestDate = event.data.requestDate;

        //validate
        const requestQtyPattern = /^[1-9][0-9]*$/
        const rejectQtyPattern = /^[0-9][0-9]*$/
        const issueQtyPattern = /^[0-9][0-9]*$/


        if (event.column.colId == 'qtyRequest') {
            if (!requestQtyPattern.test(event.data.qtyRequest)) {
                this.notify.warn(this.l('Trường QtyRequest phải là số'));
                event.data.qtyRequest = event.oldValue;
                event.api.refreshCells({ columns: ["qtyRequest"] });
                return;
            }
        }

        if (event.column.colId == 'qtyReject') {
            if (!rejectQtyPattern.test(event.data.qtyReject)) {
                this.notify.warn(this.l('Trường qtyReject phải là số'));
                event.data.qtyReject = event.oldValue;
                event.api.refreshCells({ columns: ["qtyReject"] });
                return;
            } else if (Number(event.data.qtyReject) > Number(event.data.qtyRequest)) {
                this.notify.warn(this.l('QtyReject phải nhỏ hơn QtyRequest'));
                event.data.qtyReject = event.oldValue;
                event.api.refreshCells({ columns: ["qtyReject"] });
                return;
            }
        }
        if (event.column.colId == 'qtyIssue') {
            if (!issueQtyPattern.test(event.data.qtyIssue)) {
                this.notify.warn(this.l('Trường qtyIssue phải là số'));
                event.data.qtyIssue = event.oldValue;
                event.api.refreshCells({ columns: ["qtyIssue"] });
                return;
            } else if (Number(event.data.qtyIssue) > Number(event.data.qtyRemain)) {
                this.notify.warn(this.l('Trường qtyIssue tối đa là ' + event.data.qtyRemain));
                event.data.qtyIssue = event.oldValue;
                event.api.refreshCells({ columns: ["qtyIssue"] });
                return;
            }
        }

        // check budget and funcommitment
        if (this.isCheckBudget === 'Y') {
            listId.push(event.data.id);
            let obj = Object.assign(new OnlineBudgetCheckRequest(), {
                wbsMasterData: event.data.wbsMapping,
                fiscalYear: _fiscalYear.toString(),
                costCenter: event.data.costCenterMapping,
                fixedAssetNo: '',
                system: 'MMS',
                listItemId: listId
            })
            console.log(obj);
            if (event.column.colId == 'qtyRequest' && this._permiss_MemberShop == true) {
                if (event.data.status == 'REQUESTED' || event.data.status == 'NEW') {
                    this.isLoading = true;
                    this.rowDataDetail.idDetails = event.data.id;
                    this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                    this.rowDataDetail.qtyReject = event.data.qtyReject;
                    this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                    this.rowDataDetail.statusItem = event.data.status;
                    event.data.amount = Number(event.data.qtyRequest) * Number(event.data.price)
                    event.api.refreshCells({ columns: ["amount"] });
                    this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                        .pipe(finalize(() => { }))
                        .subscribe((res) => {
                            this.notify.info(this.l('Update QtyRequest Successfully'));
                            this.isLoading = false;
                        });
                }
            } else if (event.column.colId == 'qtyReject' && this._permiss_LeaderShop == true) {
                if (Number(event.data.qtyReject) >= 0 && Number(event.data.qtyReject) <= Number(event.data.qtyRequest) && (event.data.status == 'SUBMITED' || event.data.status == 'CANCELLED')) {
                    this.isLoading = true;
                    this._sapService.sapOnlineBudgetCheck(obj)
                        .subscribe((budget) => {
                            console.log(budget);
                            this.isDataValidation = budget.response.dataValidation.dataValidationResult == 'T' ? true : false
                            this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false
                            if (this.isDataValidation) { // BudgetSuccess
                                if (budget.response.availableBudget.availableBudgetAvailableAmount * 100 >= Number(event.data.price * (event.data.qtyRequest - event.data.qtyReject))) {
                                    var v_action = (Number(event.data.qtyRequest) == Number(event.data.qtyReject)) ? 'CANCELED' : 'UPDATED';
                                    this._sapService.submitFundCommitmentRequest('', v_action, obj.listItemId)
                                        .subscribe((sub) => {
                                            console.log(sub);
                                            this.rowDataDetail.idDetails = event.data.id;
                                            this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                                            this.rowDataDetail.qtyReject = event.data.qtyReject;
                                            this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                                            this.rowDataDetail.statusItem = (Number(event.data.qtyRequest) == Number(event.data.qtyReject)) ? 'CANCELLED' : 'SUBMITED';
                                            event.data.status = (Number(event.data.qtyRequest) == Number(event.data.qtyReject)) ? 'CANCELLED' : 'SUBMITED';
                                            event.api.refreshCells({ columns: ["status"] });
                                            event.data.amount = (Number(event.data.qtyRequest) - Number(event.data.qtyReject)) * Number(event.data.price)
                                            event.api.refreshCells({ columns: ["amount"] });
                                            //    this.rowDataDetail.budgetCheckMessage = budget.response.availableBudget.availableBudgetMessage;
                                            //   this.rowDataDetail.fundCommitmentMessage = sub.sapResponse[0].response.response.documents[0].message;
                                            this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                                                .pipe(finalize(() => { }))
                                                .subscribe((res) => {
                                                    this.notify.info(this.l('Update QtyReject Successfully'));
                                                    this.isLoading = false;
                                                });
                                        });
                                }
                            }
                        })
                }

            } else if (event.column.colId == 'qtyIssue' && this._permiss_LeaderGps == true) {
                if (Number(event.data.qtyIssue) > Number(this.qtyRequest) && (event.data.status == 'APPROVED' || event.data.status == 'GPS CANCELLED')) {
                    this.notify.warn(this.l('Issue Quantity must be smaller or equal Qty Request!'));
                    this.searchDatas();
                } else if (event.data.qtyIssue <= this.qtyRequest && (event.data.status == 'APPROVED' || event.data.status == 'GPS CANCELLED')) {
                    this.isLoading = true;
                    this._sapService.sapOnlineBudgetCheck(obj)
                        .subscribe((budget) => {
                            console.log(budget);
                            this.isDataValidation = budget.response.dataValidation.dataValidationResult == 'T' ? true : false
                            this.isBudgetSuccess = budget.response.availableBudget.availableBudgetMessageType == 'S' ? true : false
                            if (this.isDataValidation) { // BudgetSuccess
                                if (budget.response.availableBudget.availableBudgetAvailableAmount * 100 >= Number(event.data.price * event.data.qtyIssue)) {
                                    var v_action = (Number(event.data.qtyIssue) == 0) ? 'CANCELED' : 'UPDATED';
                                    this._sapService.submitFundCommitmentRequest('', v_action, obj.listItemId)
                                        .subscribe((sub) => {
                                            this.rowDataDetail.idDetails = event.data.id;
                                            this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                                            this.rowDataDetail.qtyReject = Number(event.data.qtyRequest) - Number(event.data.qtyIssue);
                                            this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                                            event.data.qtyReject = Number(event.data.qtyRequest) - Number(event.data.qtyIssue);
                                            event.api.refreshCells({ columns: ["qtyReject"] });
                                            this.rowDataDetail.statusItem = (Number(event.data.qtyIssue) == 0) ? 'GPS CANCELLED' : 'APPROVED';
                                            event.data.status = (Number(event.data.qtyIssue) == 0) ? 'CANCELLED' : 'APPROVED';
                                            event.api.refreshCells({ columns: ["status"] });
                                            event.data.amount = Number(event.data.qtyIssue) * Number(event.data.price)
                                            event.api.refreshCells({ columns: ["amount"] });
                                            //  this.rowDataDetail.budgetCheckMessage = budget.response.availableBudget.availableBudgetMessage;
                                            //  this.rowDataDetail.fundCommitmentMessage = sub.sapResponse[0].response.response.documents[0].message;
                                            this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                                                .pipe(finalize(() => { }))
                                                .subscribe((res) => {
                                                    this.notify.info(this.l('Update Qty Issue Successfully'));
                                                    this.isLoading = false;
                                                });
                                        })
                                }
                            }
                        })
                }
            }
        }
        else {
            // not check budget and not funcommitment
            if (event.column.colId == 'qtyRequest' && this._permiss_MemberShop == true) {
                if (event.data.status == 'REQUESTED' || event.data.status == 'NEW') {
                    this.rowDataDetail.idDetails = event.data.id;
                    this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                    this.rowDataDetail.qtyReject = event.data.qtyReject;
                    this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                    this.rowDataDetail.statusItem = event.data.status;
                    event.data.amount = Number(event.data.qtyRequest) * Number(event.data.price)
                    event.api.refreshCells({ columns: ["amount"] });
                    this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                        .pipe(finalize(() => { }))
                        .subscribe((res) => {
                            this.notify.info(this.l('Update QtyRequest Successfully'));
                            event.data.amount = event.data.qtyRequest * event.data.price;
                            event.api.refreshCells({ columns: ["amount"] });
                        });
                }
            } else if (event.column.colId == 'qtyReject' && this._permiss_LeaderShop == true) {
                if (Number(event.data.qtyReject) >= 0 && Number(event.data.qtyReject) <= Number(event.data.qtyRequest) && (event.data.status == 'SUBMITED' || event.data.status == 'CANCELLED')) {
                    this.rowDataDetail.idDetails = event.data.id;
                    this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                    this.rowDataDetail.qtyReject = event.data.qtyReject;
                    this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                    this.rowDataDetail.statusItem = (Number(event.data.qtyRequest) == Number(event.data.qtyReject)) ? 'CANCELLED' : 'SUBMITED';
                    event.data.status = (Number(event.data.qtyRequest) == Number(event.data.qtyReject)) ? 'CANCELLED' : 'SUBMITED';
                    event.api.refreshCells({ columns: ["status"] });
                    event.data.amount = (Number(event.data.qtyRequest) - Number(event.data.qtyReject)) * Number(event.data.price)
                    event.api.refreshCells({ columns: ["amount"] });
                    this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                        .pipe(finalize(() => { }))
                        .subscribe((res) => {
                            this.notify.info(this.l('Update QtyReject Successfully'));
                        });
                }

            } else if (event.column.colId == 'qtyIssue' && this._permiss_LeaderGps == true) {
                if (Number(event.data.qtyIssue) > Number(this.qtyRequest) && (event.data.status == 'APPROVED' || event.data.status == 'GPS CANCELLED')) {
                    this.notify.warn(this.l('Issue Quantity must be smaller or equal Qty Request!'));
                    return;
                } else if (Number(event.data.qtyIssue) <= Number(this.qtyRequest) && (event.data.status == 'APPROVED' || event.data.status == 'GPS CANCELLED')) {
                    this.rowDataDetail.idDetails = event.data.id;
                    this.rowDataDetail.qtyRequest = event.data.qtyRequest;
                    this.rowDataDetail.qtyReject = Number(event.data.qtyRequest) - Number(event.data.qtyIssue);
                    this.rowDataDetail.qtyIssue = event.data.qtyIssue;
                    event.data.qtyReject = Number(event.data.qtyRequest) - Number(event.data.qtyIssue);
                    event.api.refreshCells({ columns: ["qtyReject"] });
                    this.rowDataDetail.statusItem = (Number(event.data.qtyIssue) == 0) ? 'GPS CANCELLED' : 'APPROVED';
                    event.data.status = (Number(event.data.qtyIssue) == 0) ? 'GPS CANCELLED' : 'APPROVED';
                    event.api.refreshCells({ columns: ["status"] });
                    event.data.amount = Number(event.data.qtyIssue) * Number(event.data.price)
                    event.api.refreshCells({ columns: ["amount"] });
                    this._service.updateGpsIssuingDetailQty(this.rowDataDetail)
                        .pipe(finalize(() => { }))
                        .subscribe((res) => {
                            this.notify.info(this.l('Update Qty Issue Successfully'));

                        });
                }
                else {
                    this.notify.warn(this.l('Issue Quantity must be greater than 0!'));
                    this.isLoading = false;
                }
            }
        }
    }


    viewLoggingResponse() {
        this.viewLoggingResponseBudgetCheck.show(this.documentNoN);
    }

    viewFundCommitItemDM() {
        this.viewFundCommitmentItem.show(this.idFund);
    }


}


