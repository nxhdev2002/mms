import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, ElementRef, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptSeasonMonthDto, MstWptSeasonMonthServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditSeasonmonthModalComponent } from './create-or-edit-seasonmonth-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './seasonmonth.component.html',
})
export class SeasonmonthComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalSeasonmonth', { static: true })
    createOrEditModalSeasonmonth: | CreateOrEditSeasonmonthModalComponent | undefined;
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

    selectedRow: MstWptSeasonMonthDto = new MstWptSeasonMonthDto();
    saveSelectedRow: MstWptSeasonMonthDto = new MstWptSeasonMonthDto();
    datas: MstWptSeasonMonthDto = new MstWptSeasonMonthDto();
    isLoading: boolean = false;
 

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    seasonMonth: any;
    seasonType: string = '';
    isActive: string = '';
    frameworkComponents: FrameworkComponent;

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
        private _service: MstWptSeasonMonthServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer:
                    (params) => params.rowIndex + 1 + this.paginationParams.pageSize *
                        (this.paginationParams.pageNum - 1), cellClass: ['text-center'],
                width: 60,

            },
            {
                // headerName: this.l('Working Month'),
                // headerTooltip: this.l('Season Month'),
                field: 'seasonMonth',
                rowGroup: true,
                hide: true,
                cellStyle: { backgroundColor: 'cadetblue' },
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.seasonMonth, 'yyyy') ?
                            'Year : ' + this.pipe.transform(params.data?.seasonMonth, 'yyyy') : this.l('Year : '),
                width: 200,
            },
            {
                headerName: this.l('Working Month'),
                headerTooltip: this.l('Season Month'),
                field: 'seasonMonth',
                valueGetter: (params) => this.pipe.transform(params.data?.seasonMonth, 'MM/yyyy'),
                flex: 1,
                editable: true,
            },
            {
                headerName: this.l('Season Type (High/Low/Normal)'),
                headerTooltip: this.l('Season Type (High/Low/Normal)'),
                field: 'seasonType',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data?.seasonType === 'H') {
                        return {
                            'background-color': 'Red',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            // 'border-left-width': '0',
                            // 'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data?.seasonType === 'L') {
                        return {
                            'background-color': 'Yellow',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            // 'border-left-width': '0',
                            // 'padding': '3px 6px 4px'
                        };
                    }
                    return '';
                },
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
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
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }


    searchDatas(): void {
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.seasonMonth),
            this.seasonType,
            this.isActive,
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
            this.seasonMonth = '';
            this.seasonType = '';
            this.isActive = '';
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
            this._dateTimeService.convertToDatetime(this.seasonMonth),
            this.seasonType,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptSeasonMonthDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptSeasonMonthDto();
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

    deleteRow(system: MstWptSeasonMonthDto): void {
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

    //     this._service.getSeasonMonthToExcel(
    //         this.seasonMonth,
    //         this.seasonType, 
    //         this.isActive,
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
         
        this._service.getSeasonMonthToExcel(
            this.seasonMonth,
            this.seasonType,
            this.isActive,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }


}
