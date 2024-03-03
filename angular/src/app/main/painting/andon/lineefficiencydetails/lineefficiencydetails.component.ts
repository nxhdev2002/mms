import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PtsAdoLineEfficiencyDetailsDto, PtsAdoLineEfficiencyDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditLineEfficiencyDetailsModalComponent } from './create-or-edit-lineefficiencydetails-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './lineefficiencydetails.component.html',
    styleUrls: ['./lineefficiencydetails.component.less'],
})
export class LineEfficiencyDetailsComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalLineEfficiencyDetails', { static: true }) createOrEditModalLineEfficiencyDetails: | CreateOrEditLineEfficiencyDetailsModalComponent | undefined;
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

    selectedRow: PtsAdoLineEfficiencyDetailsDto = new PtsAdoLineEfficiencyDetailsDto();
    saveSelectedRow: PtsAdoLineEfficiencyDetailsDto = new PtsAdoLineEfficiencyDetailsDto();
    datas: PtsAdoLineEfficiencyDetailsDto = new PtsAdoLineEfficiencyDetailsDto();
    isLoading = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    workingDateTo: any;
    workingDateFrom: any;

    line = '';
    volActual = 0;
    lineStopTime = 0;
    lineEfficiency;
    workingDate: any;
    shift = '';
    status = '';

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
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
        private _service: PtsAdoLineEfficiencyDetailsServiceProxy,
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
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Line'),
                headerTooltip: this.l('Line'),
                field: 'line',
                flex: 1
            },
            {
                headerName: this.l('Vol Actual'),
                headerTooltip: this.l('Vol Actual'),
                field: 'volActual',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Line Stop Time'),
                headerTooltip: this.l('Line Stop Time'),
                field: 'lineStopTime',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Line Efficiency'),
                headerTooltip: this.l('Line Efficiency'),
                field: 'lineEfficiency',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Shift')
                , headerTooltip: this.l('Shift'),
                field: 'shift',
                flex: 1
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    // eslint-disable-next-line eqeqeq
                    text: (params: { data: { status: string } }) => (params.data?.status == 'Y') ? 'Active' : 'Inactive',
                    // eslint-disable-next-line eqeqeq
                    iconName: 'fa fa-circle', className: (params: { data: { status: string } }) => (params.data?.status == 'Y') ? 'btnActive' : 'btnInActive',
                },
            }
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
            this.line,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
        this.line = '',
            this.workingDateFrom = '',
            this.workingDateTo = '',
            this.shift = '',
            this.searchDatas();
    }

    onSelectionMulti() {
        let selectedRows = this.gridApi.getSelectedRows();
        let selectedRowsString = '';
        let maxToShow = 5;
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
            let othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.line,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PtsAdoLineEfficiencyDetailsDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PtsAdoLineEfficiencyDetailsDto();
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    deleteRow(system: PtsAdoLineEfficiencyDetailsDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
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
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getLineEfficiencyDetailsToExcel(
            this.line,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
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
    getDateNow() {
        var today = new Date();
        var date = today.getDate()+'/'+(today.getMonth()+1)+'/'+today.getFullYear();
        return date;
    }
}
