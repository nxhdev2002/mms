import { ColDef, GridApi, GridReadyEvent, RowNode, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdRequestDto, InvCkdRequestServiceProxy, MstWptDailyWorkingTimeDto,  } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './request.component.html',
})
export class RequestComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('bylot', { static: true }) bylot: Paginator;

    defaultColDefs: CustomColDef[] = [];
    byLotColDefs: CustomColDef[] = [];
    byPxPColDefs: CustomColDef[] = [];
    byMakeColDefs: CustomColDef[] = [];
    byCallColDefs: CustomColDef[] = [];
    wtColdef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    paginationParamByLot: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamByPxp: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamByMake: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamByCall: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectId: any;
    selectedRow: InvCkdRequestDto = new InvCkdRequestDto();
    saveSelectedRow: InvCkdRequestDto = new InvCkdRequestDto();
    datas: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsbyMake: GridParams | undefined;
    dataParamsbycall: GridParams | undefined;
    dataParamsbyPxp: GridParams | undefined;
    dataParamsbyLot: GridParams | undefined;
    rowData: any[] = [];
    rowDataByLot: any[] = [];
    rowDataByCall: any[] = [];
    rowDataScheduleByMake: any[] = [];
    rowDataScheduleByCall: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    shiftNo: number = 0;
    shopName: string = '';
    shiftName: string = '';
    shopId: number;
    workingDate: any;
    startTime: any;
    endTime: any;
    workingType: number = 0;
    description: string = '';
    fromTime: any;
    toTime: any;
    isManual: string = '';
    isActive: string = '';
    p_request_no: any;
    p_issue_date: any;
    p_req_date: any;
    pDList: any[] = [];
    sList: any[] = [];
    wTList: any[] = [];
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    groupDefaultExpanded = 1;
    p_ckd_req_id;
    requestDateFrom: any;
    requestDateTo: any;
    statusFliter;


    listStatus = [
        { value: '-1', label: ''},
		{ value: 0, label: 'NEW' },
		{ value: 1, label: 'FIRMED' },
		{ value: 2, label: 'CANCEL' },
    ];
    p_containerno;
    p_renban;
    p_invoiceno;
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
        cellStyle: function (params: any) {
            if (params.value == 'Shift No : 2') return {
                'background-color': 'yellow',
                'border-bottom': '1px Solid #c0c0c0',
                'border-right': '1px Solid #c0c0c0',
                'overflow': 'hidden',
                'border-top-width': '0',
                'border-left-width': '0',
                'padding': '3px 6px 4px',
                'position': 'absolute',
                'z-index': '-1',
            };
            return { 'color': '' };
        },
    };


    constructor(
        injector: Injector,
        private _service: InvCkdRequestServiceProxy ,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
    ) {
        super(injector);


        this.byLotColDefs = [
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'dock',
                rowGroup: true,
                hide: true,
                valueGetter :(params: ValueGetterParams) => params.data.dock ?  'Shop : ' + (params.data?.dock) : this.l('Shop: '),
                cellClass: ['cell-border', 'cell-readonly',],
                flex: 1
            },
            {
                headerName: this.l('Time Request'),
                headerTooltip: this.l('Time Request'),
                field: 'timeRequest',
                flex: 1
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1
            },
              {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex: 1
            },     {
                headerName: this.l('Robbing'),
                headerTooltip: this.l('Robbing'),
                field: 'robbing',
                flex: 1
            },
            {
                headerName: this.l('Remarks'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',
                flex: 1
            },
        ];

        this.byMakeColDefs = [
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                rowGroup: true,
                hide: true,
                valueGetter :(params: ValueGetterParams) => params.data.shop ?  'Shop : ' + (params.data?.shop) : this.l('Shop: '),
                cellClass: ['cell-border', 'cell-readonly',],
                flex: 1

            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1
            },
            {
                headerName: this.l('Time Req'),
                headerTooltip: this.l('Time Request'),
                field: 'timeRequest',
                flex: 1
            },
                 {
                headerName: this.l('PCD Time Req'),
                headerTooltip: this.l('PCD Time Req'),
                field: 'pcdTimeRequest',
                flex: 1
            },     {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                flex: 1
            },
            {
                headerName: this.l('RenBan'),
                headerTooltip: this.l('RenBan'),
                field: 'renban',
                flex: 1
            },
            {
                headerName: this.l('Req Lot No'),
                headerTooltip: this.l('Req Lot No'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('Follow Lot No'),
                headerTooltip: this.l('Follow Lot No'),
                field: 'lotFollow',
                flex: 1
            },
            {
                headerName: this.l('Cont. Size '),
                headerTooltip: this.l('Cont. Size '),
                field: 'containerSize',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Shipping Comp.'),
                headerTooltip: this.l('Shipping Comp.'),
                field: 'shippingcompanyCode',
                flex: 1
            },

            {
                headerName: this.l('Return Type'),
                headerTooltip: this.l('Return Type'),
                field: 'returnType',
                flex: 1
            },

             {
                headerName: this.l('Bill Of Lading'),
                headerTooltip: this.l('Bill Of Lading'),
                field: 'billofladingNo',
                flex: 1
            },
            {
                headerName: this.l('Invoice No '),
                headerTooltip: this.l('Invoice No '),
                field: 'invoiceNo',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('List Lot No'),
                headerTooltip: this.l('List Lot No'),
                field: 'listLotNo',
                flex: 1
            },

            {
                headerName: this.l('List Case No'),
                headerTooltip: this.l('List Case No'),
                field: 'listcaseNo',
                flex: 1
            },
            {
                headerName: this.l('Source '),
                headerTooltip: this.l('Source'),
                field: 'source',
                flex: 1
            },
            {
                headerName: this.l('Transporter'),
                headerTooltip: this.l('Transporter'),
                field: 'transport',
                flex: 1
            },
            {
                headerName: this.l('C/D Date'),
                headerTooltip: this.l('C/D Date'),
                field: 'cdDate',
                valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                flex: 1
            },

             {
                headerName: this.l('Devaning Date'),
                headerTooltip: this.l('Devaning Date'),
                field: 'devanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Devaning Time  '),
                headerTooltip: this.l('Devaning Time  '),
                field: 'devanningTime',
                flex: 1
            },
            {
                headerName: this.l('Act. Devaning Date'),
                headerTooltip: this.l('Act. Devaning Date'),
                field: 'actualDevanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.actualDevanningDate, 'dd/MM/yyyy'),
                flex: 1
            },

            {
                headerName: this.l('Act. Devaning Time'),
                headerTooltip: this.l('Act. Devaning Time'),
                field: 'actualDevanningTime',
                flex: 1
            },
            {
                headerName: this.l('Cont. Status '),
                headerTooltip: this.l('Cont. Status'),
                field: 'contStatus',
                flex: 1
            },
        ];


        this.byCallColDefs = [
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter :(params: ValueGetterParams) => params.data.shop ?  'Shop : ' + (params.data?.shop) : this.l('Shop: '),
                flex: 1
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1
            },
            {
                headerName: this.l('Time Req'),
                headerTooltip: this.l('Time Req'),
                field: 'timeRequest',
                flex: 1
            },
             {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                flex: 1
            },
            {
                headerName: this.l('RenBan'),
                headerTooltip: this.l('RenBan'),
                field: 'renban',
                flex: 1
            },
            {
                headerName: this.l('Req Lot No'),
                headerTooltip: this.l('Req Lot No'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('Follow Lot No'),
                headerTooltip: this.l('Follow Lot No'),
                field: 'lotFollow',
                flex: 1
            },
             {
                headerName: this.l('Bill Of Lading'),
                headerTooltip: this.l('Bill Of Lading'),
                field: 'billofladingNo',
                flex: 1
            },
            {
                headerName: this.l('Invoice No '),
                headerTooltip: this.l('Invoice No '),
                field: 'invoiceNo',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('List Lot No'),
                headerTooltip: this.l('List Lot No'),
                field: 'listLotNo',
                flex: 1
            },

            {
                headerName: this.l('List Case No'),
                headerTooltip: this.l('List Case No'),
                field: 'listcaseNo',
                flex: 1
            },
            {
                headerName: this.l('Source '),
                headerTooltip: this.l('Source'),
                field: 'source',
                flex: 1
            },
            {
                headerName: this.l('Transporter'),
                headerTooltip: this.l('Transporter'),
                field: 'transport',
                flex: 1
            },

            {
                headerName: this.l('C/D Date'),
                headerTooltip: this.l('C/D Date'),
                field: 'cdDate',
                valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Return Type'),
                headerTooltip: this.l('Return Type'),
                field: 'returnType',
                flex: 1
            },

             {
                headerName: this.l('Devaning Date'),
                headerTooltip: this.l('Devaning Date'),
                field: 'devanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Devaning Time  '),
                headerTooltip: this.l('Devaning Time  '),
                field: 'devanningTime',
                flex: 1
            },
            {
                headerName: this.l('Act. Devaning Date'),
                headerTooltip: this.l('Act. Devaning Date'),
                field: 'actualDevanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.actualDevanningDate, 'dd/MM/yyyy'),
                flex: 1
            },

            {
                headerName: this.l('Act. Devaning Time'),
                headerTooltip: this.l('Act. Devaning Time'),
                field: 'actualDevanningTime',
                flex: 1
            },
            {
                headerName: this.l('Cont. Status '),
                headerTooltip: this.l('Cont. Status'),
                field: 'contStatus',
                flex: 1
            },
        ];
        this.byPxPColDefs = [
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                flex: 1
            },
                 {
                headerName: this.l('Source'),
                headerTooltip: this.l('Source'),
                field: 'source',
                flex: 1
            },     {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1
            },
            {
                headerName: this.l('Stock End of Request Date'),
                headerTooltip: this.l('Stock End of Request Date'),
                field: 'stockendofReqDate',
                flex: 1
            },
            {
                headerName: this.l('Min Stock At TMV'),
                headerTooltip: this.l('Min Stock At TMV'),
                field: 'minstockAtTmv',
                flex: 1
            },
            {
                headerName: this.l('Balance'),
                headerTooltip: this.l('Balance'),
                field: 'balance',
                flex: 1
            },
                 {
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'reqQuantity',
                flex: 1
            },
            {
                headerName: this.l('Time Request '),
                headerTooltip: this.l('Time Request '),
                field: 'timeRequest',
                flex: 1
            },
            {
                headerName: this.l('Remarks'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',
                flex: 1
            },

        ];

        this.defaultColDefs = [
            {
                headerName: this.l('Request Date'),
                headerTooltip: this.l('Request Date'),
                field: 'reqDate',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                flex: 1,
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.reqDate, 'dd/MM/yyyy') ? 'Request Date : ' + this.pipe.transform(params.data?.reqDate, 'dd/MM/yyyy') : this.l('Request Date : '),

            },
            {
                headerName: this.l('Versison'),
                headerTooltip: this.l('Versison'),
                field: 'version',
                flex: 1
            },
            {
                headerName: this.l('CKD Request No'),
                headerTooltip: this.l('CKD Request No'),
                field: 'ckdReqNo',
                flex: 1
            },
            {
                headerName: this.l('Issue Date'),
                headerTooltip: this.l('Issue Date'),
                field: 'issueDate',
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.issueDate, 'dd/MM/yyyy') ,
                flex: 1,
                sortable:false,
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamByLot = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamByPxp = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamByMake = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamByCall = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.searchDatas();
        this.resetGridView();

    }
    autoSizeAll() {
        const allColumnIds1: string[] = [];
        this.dataParamsbyMake.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" ) {
                allColumnIds1.push(column.getId());
            }
        });
        this.dataParamsbyMake.columnApi!.autoSizeColumns(allColumnIds1);
    }

    autoSizeAllCall() {

        const allColumnIds: string[] = [];
        this.dataParamsbycall.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" ) {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsbycall.columnApi!.autoSizeColumns(allColumnIds);
        this.resetGridView();
    }


    resetGridView() {

        setTimeout(() => {
            this.dataParamsbycall.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
            this.autoSizeAllCall();
        }, 500)
    }




    onChangeRowSelectionModel(params: { api: { getSelectedRows: () => InvCkdRequestDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.selectId = selected.id;
            this.getListInvCkdRequest();
        }
       this.selectedRow = Object.assign({}, selected);
    }

    getListInvCkdRequest(){
        this.getAllByLot(this.selectId);
        this.getAllByPxP(this.selectId);
        this.getAllDeliveryScheduleByMake(this.selectId);
        this.getAllDeliveryScheduleByCall(this.selectId);
       this.resetGridView();
    }

    getAllByLot(selectedId?: any){
        this._service.getAllInvCkdByLot(
            selectedId,

            '',
            this.paginationParamByLot.skipCount,
            this.paginationParamByLot.pageSize
        ).subscribe( (result) => {
            this.paginationParamByLot.totalCount = result.totalCount;
            this.rowDataByLot = result.items;
            this.paginationParamByLot.totalPage = ceil(result.totalCount / (this.paginationParamByLot.pageSize ?? 0));
        })
    }

    callBackByLotGrid(params) {
        this.dataParamsbyLot= params;
        this.dataParamsbyLot.api.paginationSetPageSize(
            this.paginationParamByLot.pageSize
        );
    }
    changePageByLot(paginationParams) {
        this.paginationParamByLot = paginationParams;
        this.paginationParamByLot.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getAllByLot(this.selectId)
    }

    getAllByPxP(selectedID?: any){
        this._service.getAllInvCkdByPxP(
            selectedID,
            '',
            this.paginationParamByPxp.skipCount,
            this.paginationParamByPxp.pageSize
        ).subscribe( (result) => {
            this.paginationParamByPxp.totalCount = result.totalCount;
            this.rowDataByCall = result.items;
            this.paginationParamByPxp.totalPage = ceil(result.totalCount / (this.paginationParamByPxp.pageSize ?? 0));
        })
    }
    callBackByPxpGrid(params) {
        this.dataParamsbyPxp = params;
        this.dataParamsbyPxp.api.paginationSetPageSize(
            this.paginationParamByPxp.pageSize
        );
    }
    changePageByPxp(paginationParams) {
        this.paginationParamByPxp = paginationParams;
        this.paginationParamByPxp.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getAllByPxP(this.selectId)
    }

    getAllDeliveryScheduleByMake(selectedId?: any){
        this._service.getAllDeliveryScheduleByMake(
            selectedId,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno,
            '',
            this.paginationParamByMake.skipCount,
            this.paginationParamByMake.pageSize
        ).subscribe( (result) => {
            this.paginationParamByMake.totalCount = result.totalCount;
            this.rowDataScheduleByMake = result.items;
            this.paginationParamByMake.totalPage = ceil(result.totalCount / (this.paginationParamByMake.pageSize ?? 0));
           this.resetGridView();

        })
    }
    callBackByMakeGrid(params) {
        this.dataParamsbyMake = params;
        this.dataParamsbyMake.api.paginationSetPageSize(
            this.paginationParamByMake.pageSize
        );
    }
    changePageByMake(paginationParams) {
        this.paginationParamByMake = paginationParams;
        this.paginationParamByMake.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getAllDeliveryScheduleByMake(this.selectId);
       this.resetGridView();
    }

    getAllDeliveryScheduleByCall(selectedId?: any){
        this._service.getAllDeliveryScheduleByCall(
            selectedId,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno,
            '',
            this.paginationParamByCall.skipCount,
            this.paginationParamByCall.pageSize
        ).subscribe( (result) => {
            console.log(result.items);
            


            this.paginationParamByCall.totalCount = result.totalCount;
            this.rowDataScheduleByCall = result.items;
            this.paginationParamByCall.totalPage = ceil(result.totalCount / (this.paginationParamByCall.pageSize ?? 0));
           this.resetGridView();
        })
    }
    callBackByCallGrid(params) {
        this.dataParamsbycall= params;
        this.dataParamsbycall.api.paginationSetPageSize(
            this.paginationParamByCall.pageSize
        );
    }
    changePageByCall(paginationParams) {
        this.paginationParamByCall = paginationParams;
        this.paginationParamByCall.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getAllDeliveryScheduleByCall(this.selectId);
        this.resetGridView();
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAllInvCkdRequestSearch(
            this.p_request_no,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.statusFliter ,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));

                if(result.totalCount==0)
                {
                    this.rowDataByLot= [];
                    this.rowDataByCall = [];
                    this.rowDataScheduleByMake = [];
                    this.rowDataScheduleByCall = [];
                    this.isLoading = false;
                }
                else
                {
                    this.selectId = result.items[0].id;
                    this.getAllByLot(result.items[0].id);
                    this.getAllByPxP(result.items[0].id);
                    this.getAllDeliveryScheduleByCall(result.items[0].id );
                    this.getAllDeliveryScheduleByMake(result.items[0].id)
                    this.isLoading = false;
                }
            });
    }


    clearTextSearch() {
        this.p_request_no = '',
        this.requestDateFrom = '';
        this.requestDateTo = '';
        this.statusFliter = '';
        this.p_containerno = '';
        this.p_invoiceno = '';
        this.p_renban ='';
        this.p_containerno ='';
        this.p_invoiceno ='';
        this.searchDatas();
    }


    unRowGroup() {
        this.groupDefaultExpanded == 1 ? this.groupDefaultExpanded = -1 : this.groupDefaultExpanded = 1
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAllInvCkdRequestSearch(
            this.p_request_no,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.statusFliter,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno,
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
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getRequestToExcel(
			this.p_request_no,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.statusFliter,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno,
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }

    exportToExcelByLot(e): void {
        this.fn.exportLoading(e, true);
        this._service.getRequestByLotToExcel(
			this.selectId
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }
    exportToExcelByPxp(e): void {
        this.fn.exportLoading(e, true);
        this._service.getRequestByPxpToExcel(
			this.selectId
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }
    exportToExcelByMake(e): void {
        this.fn.exportLoading(e, true);
        this._service.getRequestByMakeToExcel(
			this.selectId
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }
    exportToExcelByCall(e): void {
        this.fn.exportLoading(e, true);
        this._service.getRequestByCallToExcel(
			this.selectId,
            this.p_containerno,
            this.p_renban,
            this.p_invoiceno
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }

    public autoGroupColumnDef: ColDef = {
        hide:true,
        rowGroup:true,
        comparator: (valueA, valueB, nodeA, nodeB, isDescending) => {
            valueA = valueA.toString().substring(15,valueA.length);
            valueB = valueB.toString().substring(15,valueB.length);
            return this._formStoringService.dateComparator(valueA, valueB);
        },
        field: 'reqDate',
        valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.reqDate, '') ,

      };
}
