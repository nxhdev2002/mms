import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, Input, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditMstWptDailyWorkingTimeDto, CreateOrEditMstWptShopDto, CreateOrEditMstWptWorkingTypeDto, MstWptDailyWorkingTimeDto, MstWptDailyWorkingTimeServiceProxy, MstWptShopDto, MstWptWorkingTypeDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditDailyWorkingTimeModalComponent } from './create-or-edit-dailyworkingtime-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './dailyworkingtime.component.html',
})
export class DailyWorkingTimeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalDailyWorkingTime', { static: true }) createOrEditModalDailyWorkingTime: | CreateOrEditDailyWorkingTimeModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    shiftColdef: CustomColDef[] = [];
    shopColdef: CustomColDef[] = [];
    wtColdef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    saveSelectedRow: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    datas: MstWptDailyWorkingTimeDto = new MstWptDailyWorkingTimeDto();
    isLoading: boolean = false;


    dataParams: GridParams | undefined;
    rowData: any[] = [];
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
    workingDateFrom: any;
    workingDateTo: any;
    pDList: any[] = [];
    sList: any[] = [];
    wTList: any[] = [];
    frameworkComponents: FrameworkComponent;
    groupDefaultExpanded = -1;

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
            // if (params.data?.shiftName == 'SHIFT 2') return {
            //     'background-color': 'yellow',
            //     'border-bottom': '1px Solid #c0c0c0',
            //     'border-right': '1px Solid #c0c0c0',
            //     'overflow': 'hidden',
            //     'border-top-width': '0',
            //     'border-left-width': '0',
            //     'padding': '3px 6px 4px',
            //     'position': 'absolute',
            //     'z-index': '-1',
            // };
            return { 'color': '' };
        },
    };

    constructor(
        injector: Injector,
        private _service: MstWptDailyWorkingTimeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.shiftColdef = [
            {
                headerName: 'Shift No',
                headerTooltip: 'Shift No',
                field: 'shiftNo',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: 'Shift Name',
                headerTooltip: 'Shift Name',
                field: 'shiftName',
                cellClass: ['text-center'],
                flex: 1
            },
        ];

        this.shopColdef = [
            {
                headerName: 'Shop Name',
                headerTooltip: 'Shop Name',
                field: 'shopName',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                flex: 1
            },
        ];

        this.wtColdef = [
            {
                headerName: 'Working Type',
                headerTooltip: 'Working Type',
                field: 'workingType',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'Is Active',
                headerTooltip: 'Is Active',
                field: 'isActive',
                cellClass: ['text-center'],
                flex: 1
            },
        ];


        this.defaultColDefs = [
            // {
            //     headerName: this.l('STT'),
            //     headerTooltip: this.l('STT'),
            //     cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
            //     cellClass: ['text-center'],
            //     width: 55,
            // },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                // valueGetter: (params) => this.pipe.transform(params.data?.workingDate,'dd/MM/yyyy') ,
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy') ? 'Working Date : ' + this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy') : this.l('Working Date : '),
                flex: 2
            },
            {
                headerName: this.l('Shift No'),
                headerTooltip: this.l('Shift No'),
                field: 'shiftNo',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.shiftNo ? 'Shift No : ' + params.data.shiftNo : this.l('Shift No : '),
                flex: 2
            },
            {
                headerName: this.l('Shop Name'),
                headerTooltip: this.l('Shop Name'),
                field: 'shopName',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.shopName ? 'Shop Name : ' + params.data.shopName : this.l('Shop Name : '),
                flex: 2
            },
            {
                headerName: this.l('Shift Name'),
                headerTooltip: this.l('Shift Name'),
                field: 'shiftName',
                flex: 1
            },
            {
                headerName: this.l('Start Time'),
                headerTooltip: this.l('Start Time'),
                field: 'startTime',
                flex: 1,
            },
            {
                headerName: this.l('End Time'),
                headerTooltip: this.l('End Time'),
                field: 'endTime',
                flex: 1
            },
            {
                headerName: this.l('Working Type'),
                headerTooltip: this.l('Working Type'),
                field: 'workingTypeDesc',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
            {
                headerName: this.l('From Time'),
                headerTooltip: this.l('From Time'),
                field: 'fromTime',
                valueGetter: (params) => this.pipe.transform(params.data?.fromTime, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('To Time'),
                headerTooltip: this.l('To Time'),
                field: 'toTime',
                valueGetter: (params) => this.pipe.transform(params.data?.toTime, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Is Manual'),
                headerTooltip: this.l('Is Manual'),
                field: 'isManual',
                flex: 1,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isManual == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isManual == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isManual == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isManual == "N") {
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
                flex: 1,
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._service.mstWptPatternD_GetsActive()
            .subscribe((result) => {
                this.pDList = result.items;
            });
        this._service.mstWptShop_GetsActive()
            .subscribe((result) => {
                this.sList = result.items;
            });

        this._service.mstWptWorkingType_GetsForDaily()
            .subscribe((result) => {
                this.wTList = result.items;
            });
    }


    searchDatas(): void {
        this._service.getAll(
            this.shiftNo,
            this.shopName,
            this.workingType,
            //this.workingDateFrom,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
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

    SearchS1(i: number, s:any) {

        let _btnUncheck = document.querySelector('.actionButton_w'+i+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.shiftNo = 0;

        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+i);
            if(_btn) _btn.classList.add('active');

            this.shiftNo = s;
        }

        this.searchDatas();

    }


    clearTextSearch() {
        this.shiftNo = 0,
            this.shopName = '',
            this.workingType = 0,
            this.workingDateFrom = '',
            this.workingDateTo = '',
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
            this.shiftNo,
            this.shopName,
            this.workingType,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptDailyWorkingTimeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptDailyWorkingTimeDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
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

    deleteRow(system: MstWptDailyWorkingTimeDto): void {
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
    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;

    //     this._service.getDailyWorkingTimeToExcel(
    //         this.shiftNo,
    //         this.shopName,
    //         this.workingType,
    //         this._dateTimeService.convertToDatetime(this.workingDateFrom),
    //         this._dateTimeService.convertToDatetime(this.workingDateTo),
    //     )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }



    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getDailyWorkingTimeToExcel(
            this.shiftNo,
            this.shopName,
            this.workingType,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
