import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    FrameworkComponent,
    GridParams,
    PaginationParamsModel,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptWorkingTimeDto, MstWptWorkingTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditWorkingTimeModalComponent } from './create-or-edit-workingtime-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './workingtime.component.html', 
    styleUrls: ['./workingtime.component.less'],
})
export class WorkingTimeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalWorkingTime', { static: true })
    createOrEditModalWorkingTime: CreateOrEditWorkingTimeModalComponent | undefined;
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

    shiftColdef: CustomColDef[] = [];
    shopColdef: CustomColDef[] = [];
    selectedRow: MstWptWorkingTimeDto = new MstWptWorkingTimeDto();
    saveSelectedRow: MstWptWorkingTimeDto = new MstWptWorkingTimeDto();
    datas: MstWptWorkingTimeDto = new MstWptWorkingTimeDto();
    isLoading: boolean = false;
    // pending: string = '';
    // disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    shiftNo: number = 0;
    shopId: number = 0;
    shopName: string = '';
    workingType: number = 0;
    startTime: any;
    endTime: any;
    description: string = '';
    patternHId: number = 0;
    seasonType: string = '';
    dayOfWeek: string = '';
    weekWorkingDays: number = 0;
    isActive = '';
    frameworkComponents: FrameworkComponent;
    pDList: any[] = [];
    sList: any[] = [];
    groupDefaultExpanded = -1

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
        private _service: MstWptWorkingTimeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.shiftColdef = [
            {
                headerName: 'Shift No',
                headerTooltip: 'Shift No',
                field: 'shiftNo',
                cellClass: ['text-center'],
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


        this.defaultColDefs = [
            {
                field: 'patternDescription',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.patternDescription ? 'Pattern : ' + params.data.patternDescription : this.l('Pattern : '),
                flex: 2
            },
            {
                field: 'seasonDayOfWeek',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.SeasonDayOfWeek ? 'Season - Week Working Days - Day Of Week : ' + params.data.SeasonDayOfWeek
                    : this.l('Season - Week Working Days - Day Of Week : '),
                flex: 2
            },

            {
                field: 'shiftNo',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.shiftNo ? 'Shift No : ' + params.data.shiftNo : this.l('Shift No : '),
                flex: 2
            },
            {
                field: 'shopName',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.shopName ? 'Shop Name : ' + params.data.shopName : this.l('Shop Name : '),
                flex: 2
            },
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                //field: 'rowNo',
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 60,
            },
            {
                headerName: this.l('Season Month (L/H/N)'),
                headerTooltip: this.l('Season Month (L/H/N)'),
                field: 'seasonType',
                width: 150,
            },
            {
                headerName: this.l('Day Of Week'),
                headerTooltip: this.l('Day Of Week'),
                field: 'dayOfWeek',
                flex: 1,
            },
            {
                headerName: this.l('Week Working Days'),
                headerTooltip: this.l('Week Working Days'),
                field: 'weekWorkingDays',
                type: 'rightAligned',
                width: 140,
            },

            {
                headerName: this.l('Shift No'),
                headerTooltip: this.l('Shift No'),
                field: 'shiftNo',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Shop Id'),
                headerTooltip: this.l('Shop Id'),
                field: 'shopId',
                type: 'rightAligned',
                flex: 1,
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
                flex: 1,
            },
            {
                headerName: this.l('Working Type'),
                headerTooltip: this.l('Working Type'),
                field: 'workingType',
                type: 'rightAligned',
                width: 100,
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

            // {
            //     headerName: this.l('Description'),
            //     headerTooltip: this.l('Description'),
            //     field: 'description',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Pattern H Id'),
            //     headerTooltip: this.l('Pattern H Id'),
            //     field: 'patternHId',
            //     flex: 1,
            // },



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
    }

    searchDatas(): void {
        this._service
            .getAll(
                this.shiftNo,
                this.shopName,
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
        this.groupDefaultExpanded == 1 ;
        this.searchDatas();

    }


    clearTextSearch() {
        this.shiftNo = 0,
            this.shopName = '',
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
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptWorkingTimeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptWorkingTimeDto();
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

    deleteRow(system: MstWptWorkingTimeDto): void {
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

    //     this._service
    //         .getWorkingTimeToExcel(
    //             this.shiftNo,
    //             this.shopName,
    //         )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void { 

        this.fn.exportLoading(e, true);

        this._service.getWorkingTimeToExcel(
            this.shiftNo,
            this.shopName,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    } 
    
}
