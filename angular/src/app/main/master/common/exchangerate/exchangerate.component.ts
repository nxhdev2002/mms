import { ColDef, GridApi, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmExchangeRateDto, MstCmmExchangeRateServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ConsoleLogger } from '@microsoft/signalr/dist/esm/Utils';

@Component({
    templateUrl: './exchangerate.component.html',
})
export class ExchangeRateComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    changedRecordsExchangeRate: number[] = [];
    selectId;
    selectedRow: MstCmmExchangeRateDto = new MstCmmExchangeRateDto();
    saveSelectedRow: MstCmmExchangeRateDto = new MstCmmExchangeRateDto();
    datas: MstCmmExchangeRateDto = new MstCmmExchangeRateDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    exchangeDateFrom: any;
    exchangeDateTo: any;
    version: number = 0;
    majorCurrency: string = '';
    minorCurrency: string = '';
    ceilingRate
    svbRate
    floorRate
    buyingOd
    buyingTt
    sellingTtOd
    isDownloaded: string = '';
    isEmailReceived: string = '';
    isDiff: string = '';
    downloadDatetime: any;
    emailReceiveDatetime: any;
    checkedBy
    checkedDatetime: any;
    approvedBy: string = '';
    approvedDatetime: any;
    status: string = '';
    toolName: string = '';
    isActive: string = '';
    isShowCheck: boolean = true;
    isShowCheckDiff: boolean = true;
    isShowApprove: boolean = true;
    dateCurent = new Date()

    RestrictList = [
        { value: 'CREATED', label: "Created" },
        { value: 'CHECKED', label: "Checked" },
        { value: 'APPROVE', label: "Approve" },
    ];

    autoGroupColumnDef: ColDef = {
        // headerValueGetter: (params) => `${params.colDef.headerName} Group Column`,
        minWidth: 220,
        cellRendererParams: {
          suppressCount: true,
          checkbox: false,
        },
      };


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
        private _service: MstCmmExchangeRateServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('Exchange Date'),
                headerTooltip: this.l('Exchange Date'),
                field: 'exchangeDate',
                rowGroup: true,
                hide: true,
                //cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.exchangeDate, 'dd/MM/yyyy') ? 'Exchange Date : ' + this.pipe.transform(params.data?.exchangeDate, 'dd/MM/yyyy') : this.l('Exchange Date : '),
                pinned: 'left',
                //  width: 250, 
                //  minWidth: 250, // resizable: true
            },
            {
                headerName: this.l('Version'),
                headerTooltip: this.l('Version'),
                field: 'version',
                rowGroup: true,
                hide: true,
                //cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.version ? 'Version : ' + params.data.version : this.l('Version : '),
                // width:300,
                pinned: 'left',
            },
            {
                headerName: this.l('Is Diff'),
                headerTooltip: this.l('Is Diff'),
                field: 'isDiff',
                 cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isDiff == "Y") { return 'Y' }
                        else if (params.data?.isDiff == "N") { return 'N' }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isDiff == "Y") { return 'btnActive' }
                        else if (params.data?.isDiff == "N") { return 'btnInActive'}
                        return 'btnUndefined';
                    }
               },
                flex: 1,
               // pinned: 'left',
               
            },
            {
                headerName: this.l('Major Currency'),
                headerTooltip: this.l('Major Currency'),
                field: 'majorCurrency',
                flex: 1,
              //  pinned: 'left',
            },
            {
                headerName: this.l('Minor Currency'),
                headerTooltip: this.l('Minor Currency'),
                field: 'minorCurrency',
                flex: 1,
              //  pinned: 'left',
                
            },
            {
                headerName: this.l('Ceiling Rate'),
                headerTooltip: this.l('Ceiling Rate'),
                field: 'ceilingRate',
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.ceilingRate,4),type: 'rightAligned'
            },
            {
                headerName: this.l('Svb Rate'),
                headerTooltip: this.l('Svb Rate'),
                field: 'svbRate',
                flex: 1,
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.svbRate,4),type: 'rightAligned'
            },
            {
                headerName: this.l('Floor Rate'),
                headerTooltip: this.l('Floor Rate'),
                field: 'floorRate',
                flex: 1,
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.floorRate,4),type: 'rightAligned'
            },
            {
                headerName: this.l('Buying Od'),
                headerTooltip: this.l('Buying Od'),
                field: 'buyingOd',
                flex: 1,
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.buyingOd,4),type: 'rightAligned'
            },
            {
                headerName: this.l('Buying Tt'),
                headerTooltip: this.l('Buying Tt'),
                field: 'buyingTt',
                flex: 1,
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.buyingTt,4),type: 'rightAligned'
            },
            {
                headerName: this.l('Selling Tt Od'),
                headerTooltip: this.l('Selling Tt Od'),
                field: 'sellingTtOd',
                flex: 1,
                comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.sellingTtOd,4),type: 'rightAligned'
            },

            {
                headerName: this.l('Download Datetime'),
                headerTooltip: this.l('Download Datetime'),
                field: 'downloadDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.downloadDatetime, 'dd/MM/yyyy'), flex: 1
            },
            {
                headerName: this.l('Email Receive Datetime'),
                headerTooltip: this.l('Email Receive Datetime'),
                field: 'emailReceiveDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.emailReceiveDatetime, 'dd/MM/yyyy'), flex: 1
            },
            {
                headerName: this.l('Checked By'),
                headerTooltip: this.l('Checked By'),
                field: 'checkedBy',
            },
            {
                headerName: this.l('Checked Datetime'),
                headerTooltip: this.l('Checked Datetime'),
                field: 'checkedDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.checkedDatetime, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Approved By'),
                headerTooltip: this.l('Approved By'),
                field: 'approvedBy',
            },
            {
                headerName: this.l('Approved Datetime'),
                headerTooltip: this.l('Approved Datetime'),
                field: 'approvedDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.approvedDatetime, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
            },
            {
                headerName: this.l('Tool Name'),
                headerTooltip: this.l('Tool Name'),
                field: 'toolName',
            },
            {
                headerName: this.l('Is Downloaded'),
                headerTooltip: this.l('Is Downloaded'),
                field: 'isDownloaded',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isDownloaded == "Y") {
                            return 'Downloaded'
                        }
                        else if (params.data?.isDownloaded == "N") {
                            return 'InDownloaded'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isDownloaded == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isDownloaded == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },
            {
                headerName: this.l('Is Email Received'),
                headerTooltip: this.l('Is Email Received'),
                field: 'isEmailReceived',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isEmailReceived == "Y") {
                            return 'EmailReceived'
                        }
                        else if (params.data?.isEmailReceived == "N") {
                            return 'InEmailReceived'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isEmailReceived == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isEmailReceived == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },
           
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };


        // this.gridOptions.api.forEachNode(node => {
        //         node.expanded = true;
        // });
        // this.gridOptions.api.onGroupExpandedOrCollapsed();
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.dateCurent.setDate(this.dateCurent.getDate());
        this.exchangeDateTo = this.dateCurent;
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsExchangeRate= result;
            console.log("result =", result);
        })
    }

    searchDatas(): void {
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.exchangeDateFrom),
            this._dateTimeService.convertToDatetime(this.exchangeDateTo),
            this.status,
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
            });
    }

    onExpandCollapseAll() {

        this.gridApi.forEachNode(node => {
        //   if (node.expanded) {
        //     node.setExpanded(false);
        //     this.isExpandedGrid = false;
        //   } else {
            node.setExpanded(true);
        //     this.isExpandedGrid = true;
        //   }
        });

        //--> master table : grid child
        this.gridApi.forEachDetailGridInfo(grid => {
          grid.api.forEachNode(node => {
            //   if (node.expanded) {
            //   node.setExpanded(false);
            //   this.isExpandedGrid = false;
            // } else {
              node.setExpanded(true);
            //   this.isExpandedGrid = true;
            // }
          });
        });
      }


    autoSizeAll() {
        const allColumnIds: string[] = [];
        console.log(this.dataParams.columnApi!.getAllColumns());

        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" 
            && column.getId().toString() != "stt" 
            && column.getId().toString() != "listcaseNo"  
            ) { 
                allColumnIds.push(column.getId());
            }
        });
        // this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
        this.dataParams.columnApi!.autoSizeAllColumns();
    }

    resetGridView() {
      
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true, 
                // defaultMinWidth: 20,
                // columnLimits: [{ key: 'listcaseNo', }], 
            });
            this.autoSizeAll();
        }, 100)
    }

    clearTextSearch() {
        this.exchangeDateFrom = '',
        this.exchangeDateTo = '',
            this.minorCurrency = '',
            this.status = '',
            this.searchDatas();
    }

    today(i: number) {

        let _btn = document.querySelector('.actionButton_w'+i);
        if (_btn.classList.contains('active')){
            _btn.classList.remove('active');

            this.exchangeDateTo = '';
            this.exchangeDateFrom = '';
            this.searchDatas();

        }else {
            _btn.classList.add('active');
            this.exchangeDateFrom = new Date,
            this.exchangeDateTo = new Date,
            this.searchDatas();
        }


    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.exchangeDateFrom),
            this._dateTimeService.convertToDatetime(this.exchangeDateTo),
            this.status,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmExchangeRateDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmExchangeRateDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
        this.selectedRow.isDiff == 'Y' ? this.isShowCheckDiff = false : this.isShowCheckDiff = true ;

        //show/hide button confirm
        var myDate = new Date();
        if (this.selectedRow.exchangeDate) {
            var date1 = this.selectedRow.exchangeDate.toString().substring(0, 10);
            var date2 = this._dateTimeService.convertToDatetime(myDate).toString().substring(0, 10);
            this.isShowCheck = !(this.selectedRow.id && (date1 == date2))
            this.isShowApprove = !(this.selectedRow.id && (date1 == date2))
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
            this.resetGridView();
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        this.gridApi = params.api;

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

                // this.gridApi.expandAll();
            });
    }

    checkExchangeRate(system: MstCmmExchangeRateDto): void {
        this._service.conFirmExchangeRate(this.selectedRow.id, 'CHECKED').subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    approveExchangeRate(system: MstCmmExchangeRateDto): void {
        this._service.conFirmExchangeRate(this.selectedRow.id, 'APPROVED').subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    viewDiffExchangeRate(){
        
    }
}
