import { CommonFunction } from "@app/main/commonfuncton.component";
import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild, HostListener } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptCalendarDto, MstWptCalendarServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditCalendarModalComponent } from './create-or-edit-calendar-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { ViewDetailCalendarModalComponent } from './details-calendar-modal.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './calendar.component.html',
})
export class CalendarComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalCalendar', { static: true }) createOrEditModalCalendar:
        | CreateOrEditCalendarModalComponent
        | undefined;

    @ViewChild('viewDetailMonthModalCalendar', { static: true }) viewDetailMonthModalCalendar:
        | ViewDetailCalendarModalComponent
        | undefined;

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
    selectedRow: MstWptCalendarDto = new MstWptCalendarDto();
    saveSelectedRow: MstWptCalendarDto = new MstWptCalendarDto();
    datas: MstWptCalendarDto = new MstWptCalendarDto();
    isLoading = false;


    dataParams: GridParams | undefined;
    rowData: any[] = [];
    filter = '';
    workingType = '';
    workingStatus = '';
    seasonType = '';
    isActive = '';
    dayOfWeek = '';
    today: Date = new Date();
    pipe = new DatePipe('en-US');
    gridApi!: GridApi;
    rowSelection = 'multiple';
    workingDate: any;
    workingDateFrom: any;
    workingDateTo: any;
    frameworkComponents: FrameworkComponent;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) {
                return null;
            }
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service: MstWptCalendarServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 60,
            },
            {
                headerName: this.l('Working Month'),
                headerTooltip: this.l('Working Month'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Working Type'),
                headerTooltip: this.l('Working Type'),
                field: 'workingType',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.workingType === 'HOLIDAY') {
                        return {
                            'background-color': 'Red',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                        // eslint-disable-next-line eqeqeq
                    } else if (params.data.workingType != 'ALL DAY' && params.data.workingType != 'HOLIDAY') {
                        return {
                            'background-color': 'Yellow',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    return '';
                },
            },
            {
                headerName: this.l('Working Pattern'),
                headerTooltip: this.l('Working Pattern'),
                children: [

                    {
                        headerName: this.l('Season Type'),
                        headerTooltip: this.l('Season Type'),
                        field: 'seasonType',
                        flex: 1,
                    },
                    {
                        headerName: this.l('Week Number'),
                        headerTooltip: this.l('Week Number'),
                        field: 'weekNumber',
                        type: 'rightAligned',
                        flex: 1,
                    },
                    {
                        headerName: this.l('Working Days (in Week)'),
                        headerTooltip: this.l('Working Days (in Week)'),
                        field: 'weekWorkingDays',
                        type: 'rightAligned',
                        width: 180,
                    },
                    {
                        headerName: this.l('Day Of Week'),
                        headerTooltip: this.l('Day Of Week'),
                        field: 'dayOfWeek',
                        flex: 1,
                    },
                ]
            },
            {
                headerName: this.l('Working Status'),
                headerTooltip: this.l('Working Status'),
                field: 'workingStatus',
                flex: 1,
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    // eslint-disable-next-line eqeqeq
                    text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    // eslint-disable-next-line eqeqeq
                    className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    //     autoSizeAll() {
    //         const allColumnIds: string[] = [];
    //         this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //           if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
    //             allColumnIds.push(column.getId());
    //           }
    //         });
    //         this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    //     resetGridView(){
    //         setTimeout(()=>{
    //             this.autoSizeAll();
    //         }, 1000)
    //     }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    // eslint-disable-next-line @angular-eslint/use-lifecycle-interface
    ngAfterViewInit() {

    }

    // eslint-disable-next-line @typescript-eslint/member-ordering
    @HostListener('window:resize', ['$event'])
    onWindowResize() {

    }


    searchDatas(): void {
        this._service
            .getAll(
                // this.workingDate, --> type Date
                this._dateTimeService.convertToDatetime(this.workingDate),
                this._dateTimeService.convertToDatetime(this.workingDateFrom),
                this._dateTimeService.convertToDatetime(this.workingDateTo),
                '',
                '',
                undefined,
                '',
                '',
                '',
                this.paginationParams.skipCount,
                this.paginationParams.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                //    this.resetGridView();
            });
    }

    clearTextSearch() {
        this.workingDateFrom = '';
        this.workingDateTo = '';
        this.searchDatas();
    }

    getDatas(_paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            '',
            undefined,
            '',
            '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptCalendarDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptCalendarDto();
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
            //  this.resetGridView();
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                //     this.resetGridView();
            });
    }

    deleteRow(system: MstWptCalendarDto): void {
        this.message.confirm(this.l('AreYouSureToDelete', system.workingType), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getCalendarToExcel(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.workingType,
            this.workingStatus,
            this.seasonType,
            this.isActive,
            this.dayOfWeek,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }


    loaderVisible() {
        document.querySelectorAll<HTMLElement>('.lds-hourglass')[0].style.visibility = 'visible';
        (<HTMLInputElement>document.getElementById('exportToExcel')).disabled = true;
    }
    loaderHidden() {
        document.querySelectorAll<HTMLElement>('.lds-hourglass')[0].style.visibility = 'hidden';
        (<HTMLInputElement>document.getElementById('exportToExcel')).disabled = false;
    }
}
