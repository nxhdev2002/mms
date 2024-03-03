import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WldAdoPunchQueueDto, WldAdoPunchQueueServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPunchQueueModalComponent } from './create-or-edit-punchqueue-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './punchqueue.component.html',
    styleUrls: ['./punchqueue.component.less'],
})
export class PunchQueueComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPunchQueue', { static: true }) createOrEditModalPunchQueue: | CreateOrEditPunchQueueModalComponent | undefined;
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

    selectedRow: WldAdoPunchQueueDto = new WldAdoPunchQueueDto();
    saveSelectedRow: WldAdoPunchQueueDto = new WldAdoPunchQueueDto();
    datas: WldAdoPunchQueueDto = new WldAdoPunchQueueDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    bodyNo: string = '';
    model: string = '';
    line: string = '';
    lotNo: string = '';
    color: string = '';
    processGroup: number = 0;
    scanTime: any;
    scanTimeFrom: any;
    scanTimeTo: any;
    punchFlag: string = '';
    punchIndicator: string = '';
    isCall: string = '';
    isCf: string = '';

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
        private _service: WldAdoPunchQueueServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                flex: 1
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1
            },
            {
                headerName: this.l('Line'),
                headerTooltip: this.l('Line'),
                field: 'line',
                flex: 1
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                flex: 1
            },
            {
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'),
                field: 'processGroup',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Scan Time'),
                headerTooltip: this.l('Scan Time'),
                field: 'scanTime', valueGetter: (params) => this.pipe.transform(params.data?.scanTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Punch Flag'),
                headerTooltip: this.l('Punch Flag'),
                field: 'punchFlag',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.punchFlag == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.punchFlag == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.punchFlag == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.punchFlag == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
                flex: 1,
            },

            {
                headerName: this.l('Punch Indicator'),
                headerTooltip: this.l('Punch Indicator'),
                field: 'punchIndicator',
                width: 140,
            },
            {
                headerName: this.l('Is Call'),
                headerTooltip: this.l('Is Call'),
                field: 'isCall',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isCall == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isCall == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isCall == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isCall == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
                flex: 1,
            },

            {
                headerName: this.l('Is Cf'),
                headerTooltip: this.l('Is Cf'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isCf == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isCf == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isCf == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isCf == "N") {
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
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.bodyNo,
            this.model,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
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
        this.bodyNo = '',
        this.model = '',
        this.lotNo = '',
        this.scanTimeFrom = '',
        this.scanTimeTo = '',
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
            this.bodyNo,
            this.model,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => WldAdoPunchQueueDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new WldAdoPunchQueueDto();
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

    deleteRow(system: WldAdoPunchQueueDto): void {
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
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getPunchQueueToExcel(
            this.bodyNo,
            this.model,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }




}
