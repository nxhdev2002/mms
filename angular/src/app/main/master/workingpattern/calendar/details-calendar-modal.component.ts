import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptCalendarDto, MstWptCalendarDetailDto, MstWptCalendarServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DatePipe } from '@angular/common';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { ceil } from 'lodash-es';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Paginator } from 'primeng/paginator/paginator';


@Component({
    selector: 'details-calendar-modal',
    templateUrl: './details-calendar-modal.component.html',
    styleUrls: ['./details-calendar-modal.component.less']
})
export class ViewDetailCalendarModalComponent extends AppComponentBase {
    @ViewChild('viewDetailModalCalendar', { static: true }) modal: ModalDirective | undefined;
    // @ViewChild('paginator', { static: true }) paginator: Paginator;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    rowdata: MstWptCalendarDetailDto = new MstWptCalendarDetailDto();
    selectedRow: MstWptCalendarDetailDto = new MstWptCalendarDetailDto();

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    filter: string = '';
    workingType: string = '';
    workingStatus: string = '';
    today: Date = new Date();
    pipe = new DatePipe('en-US');
    gridApi!: GridApi;
    workingDate: any;
    active: boolean = false;
    rowSelection = 'multiple';
    monthSearch: any;
    yearSearch: any;
    yearCurent: Date = new Date();

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
        private _service: MstWptCalendarServiceProxy,
        private gridTableService: GridTableService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 50,
            },
            {
                headerName: this.l('W/M'),
                headerTooltip: this.l('W/M'),
                field: 'workingMonth',
                valueGetter: (params) => this.pipe.transform(params.data?.workingMonth, 'MM/yyyy'),
                width: 70,

            },
            {
                headerName: this.l('D1'),
                headerTooltip: this.l('D1'),
                field: 'daY_1',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_1 === 'Off') {
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
                    }
                    else if (params.data.daY_1 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_1 != 'Off' && params.data.daY_1 != '' && params.data.daY_1 != 'S1,2,3') {
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
                headerName: this.l('D2'),
                headerTooltip: this.l('D2'),
                field: 'daY_2',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_2 === 'Off') {
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
                    }
                    else if (params.data.daY_2 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_2 != 'Off' && params.data.daY_2 != '' && params.data.daY_2 != 'S1,2,3') {
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
                headerName: this.l('D3'),
                headerTooltip: this.l('D3'),
                field: 'daY_3',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_3 === 'Off') {
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
                    }
                    else if (params.data.daY_3 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_3 != 'Off' && params.data.daY_3 != '' && params.data.daY_3 != 'S1,2,3') {
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
                headerName: this.l('D4'),
                headerTooltip: this.l('D4'),
                field: 'daY_4',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_4 === 'Off') {
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
                    }
                    else if (params.data.daY_4 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_4 != 'Off' && params.data.daY_4 != '' && params.data.daY_4 != 'S1,2,3') {
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
                headerName: this.l('D5'),
                headerTooltip: this.l('D5'),
                field: 'daY_5',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_5 === 'Off') {
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
                    }
                    else if (params.data.daY_5 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_5 != 'Off' && params.data.daY_5 != '' && params.data.daY_5 != 'S1,2,3') {
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
                headerName: this.l('D6'),
                headerTooltip: this.l('D6'),
                field: 'daY_6',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_6 === 'Off') {
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
                    }
                    else if (params.data.daY_6 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_6 != 'Off' && params.data.daY_6 != '' && params.data.daY_6 != 'S1,2,3') {
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
                headerName: this.l('D7'),
                headerTooltip: this.l('D7'),
                field: 'daY_7',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_7 === 'Off') {
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
                    }
                    else if (params.data.daY_7 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_7 != 'Off' && params.data.daY_7 != '' && params.data.daY_7 != 'S1,2,3') {
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
                headerName: this.l('D8'),
                headerTooltip: this.l('D8'),
                field: 'daY_8',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_8 === 'Off') {
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
                    }
                    else if (params.data.daY_8 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_8 != 'Off' && params.data.daY_8 != '' && params.data.daY_8 != 'S1,2,3') {
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
                headerName: this.l('D9'),
                headerTooltip: this.l('D9'),
                field: 'daY_9',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_9 === 'Off') {
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
                    }
                    else if (params.data.daY_9 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_9 != 'Off' && params.data.daY_9 != '' && params.data.daY_9 != 'S1,2,3') {
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
                headerName: this.l('D10'),
                headerTooltip: this.l('D10'),
                field: 'daY_10',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_10 === 'Off') {
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
                    }
                    else if (params.data.daY_10 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_10 != 'Off' && params.data.daY_10 != '' && params.data.daY_10 != 'S1,2,3') {
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
                headerName: this.l('D11'),
                headerTooltip: this.l('D11'),
                field: 'daY_11',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_11 === 'Off') {
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
                    }
                    else if (params.data.daY_11 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_11 != 'Off' && params.data.daY_11 != '' && params.data.daY_11 != 'S1,2,3') {
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
                headerName: this.l('D12'),
                headerTooltip: this.l('D12'),
                field: 'daY_12',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_12 === 'Off') {
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
                    }
                    else if (params.data.daY_12 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_12 != 'Off' && params.data.daY_12 != '' && params.data.daY_12 != 'S1,2,3') {
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
                headerName: this.l('D13'),
                headerTooltip: this.l('D13'),
                field: 'daY_13',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_13 === 'Off') {
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
                    }
                    else if (params.data.daY_13 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_13 != 'Off' && params.data.daY_13 != '' && params.data.daY_13 != 'S1,2,3') {
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
                headerName: this.l('D14'),
                headerTooltip: this.l('D14'),
                field: 'daY_14',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_14 === 'Off') {
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
                    }
                    else if (params.data.daY_14 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_14 != 'Off' && params.data.daY_14 != '' && params.data.daY_14 != 'S1,2,3') {
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
                headerName: this.l('D15'),
                headerTooltip: this.l('D15'),
                field: 'daY_15',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_15 === 'Off') {
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
                    }
                    else if (params.data.daY_15 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_15 != 'Off' && params.data.daY_15 != '' && params.data.daY_15 != 'S1,2,3') {
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
                headerName: this.l('D16'),
                headerTooltip: this.l('D16'),
                field: 'daY_16',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_16 === 'Off') {
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
                    }
                    else if (params.data.daY_16 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_16 != 'Off' && params.data.daY_16 != '' && params.data.daY_16 != 'S1,2,3') {
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
                headerName: this.l('D17'),
                headerTooltip: this.l('D17'),
                field: 'daY_17',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_17 === 'Off') {
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
                    }
                    else if (params.data.daY_17 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_17 != 'Off' && params.data.daY_17 != '' && params.data.daY_17 != 'S1,2,3') {
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
                headerName: this.l('D18'),
                headerTooltip: this.l('D18'),
                field: 'daY_18',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_18 === 'Off') {
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
                    }
                    else if (params.data.daY_18 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_18 != 'Off' && params.data.daY_18 != '' && params.data.daY_18 != 'S1,2,3') {
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
                headerName: this.l('D19'),
                headerTooltip: this.l('D19'),
                field: 'daY_19',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_19 === 'Off') {
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
                    }
                    else if (params.data.daY_19 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_19 != 'Off' && params.data.daY_19 != '' && params.data.daY_19 != 'S1,2,3') {
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
                headerName: this.l('D20'),
                headerTooltip: this.l('D20'),
                field: 'daY_20',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_20 === 'Off') {
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
                    }
                    else if (params.data.daY_20 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_20 != 'Off' && params.data.daY_20 != '' && params.data.daY_20 != 'S1,2,3') {
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
                headerName: this.l('D21'),
                headerTooltip: this.l('D21'),
                field: 'daY_21',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_21 === 'Off') {
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
                    }
                    else if (params.data.daY_21 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_21 != 'Off' && params.data.daY_21 != '' && params.data.daY_21 != 'S1,2,3') {
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
                headerName: this.l('D22'),
                headerTooltip: this.l('D22'),
                field: 'daY_22',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_22 === 'Off') {
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
                    }
                    else if (params.data.daY_22 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_22 != 'Off' && params.data.daY_22 != '' && params.data.daY_22 != 'S1,2,3') {
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
                headerName: this.l('D23'),
                headerTooltip: this.l('D23'),
                field: 'daY_23',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_23 === 'Off') {
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
                    }
                    else if (params.data.daY_23 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_23 != 'Off' && params.data.daY_23 != '' && params.data.daY_23 != 'S1,2,3') {
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
                headerName: this.l('D24'),
                headerTooltip: this.l('D24'),
                field: 'daY_24',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_24 === 'Off') {
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
                    }
                    else if (params.data.daY_24 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_24 != 'Off' && params.data.daY_24 != '' && params.data.daY_24 != 'S1,2,3') {
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
                headerName: this.l('D25'),
                headerTooltip: this.l('D25'),
                field: 'daY_25',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_25 === 'Off') {
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
                    }
                    else if (params.data.daY_25 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_25 != 'Off' && params.data.daY_25 != '' && params.data.daY_25 != 'S1,2,3') {
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
                headerName: this.l('D26'),
                headerTooltip: this.l('D26'),
                field: 'daY_26',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_26 === 'Off') {
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
                    }
                    else if (params.data.daY_26 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_26 != 'Off' && params.data.daY_26 != '' && params.data.daY_26 != 'S1,2,3') {
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
                headerName: this.l('D27'),
                headerTooltip: this.l('D27'),
                field: 'daY_27',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_27 === 'Off') {
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
                    }
                    else if (params.data.daY_27 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_27 != 'Off' && params.data.daY_27 != '' && params.data.daY_27 != 'S1,2,3') {
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
                headerName: this.l('D28'),
                headerTooltip: this.l('D28'),
                field: 'daY_28',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_28 === 'Off') {
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
                    }
                    else if (params.data.daY_28 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_28 != 'Off' && params.data.daY_28 != '' && params.data.daY_28 != 'S1,2,3') {
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
                headerName: this.l('D29'),
                headerTooltip: this.l('D29'),
                field: 'daY_29',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_29 === 'Off') {
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
                    }
                    else if (params.data.daY_29 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_29 != 'Off' && params.data.daY_29 != '' && params.data.daY_29 != 'S1,2,3') {
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
                headerName: this.l('D30'),
                headerTooltip: this.l('D30'),
                field: 'daY_30',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_30 === 'Off') {
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
                    }
                    else if (params.data.daY_30 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_30 != 'Off' && params.data.daY_30 != '' && params.data.daY_30 != 'S1,2,3') {
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
                headerName: this.l('D31'),
                headerTooltip: this.l('D31'),
                field: 'daY_31',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.daY_31 === 'Off') {
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
                    }
                    else if (params.data.daY_31 === null) {
                        return {
                            'background-color': 'Gray',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            'border-left-width': '0',
                            'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.daY_31 != 'Off' && params.data.daY_31 != '' && params.data.daY_31 != 'S1,2,3') {
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

        ];
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    ngAfterViewInit() {
        this.setHeight();
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.setHeight();
    }

    setHeight() {

        let w = window.innerWidth;
        let objSearch = document.querySelectorAll<HTMLElement>("#container .form-group");
        let h_search = 0;
        if (objSearch.length > 0) { h_search = objSearch[0].offsetHeight }

        let h = (window.innerHeight - (55 + 10 + 32 + h_search + 28 + 12 + 39 + 30 + (10))) + 'px';
        document.querySelector<HTMLElement>('simple-ag-grid ag-grid-angular').style.height = h;
    }

    show(): void {
        this._service
            .get_Pivot_Month(
                this.monthSearch,
                this.yearSearch,
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
        this.active = true;
        this.modal.show();
        this.yearSearch = this.yearCurent;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptCalendarDetailDto[] } }) {
        this.selectedRow = Object.assign({});
    }

    searchDataDetails(): void {
        this._service
            .get_Pivot_Month(
                this.monthSearch,
                this.yearSearch,
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
        this.active = true;
        this.modal.show();
    }

    clearTextSearchDetail() {
        this.monthSearch = '';
        this.yearSearch = '';
        this.searchDataDetails();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.get_Pivot_Month(
            this.monthSearch,
            this.yearSearch,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
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

    closeDetail(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }


    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.closeDetail();
        }
    }

}
