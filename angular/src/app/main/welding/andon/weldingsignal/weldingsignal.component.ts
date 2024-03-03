import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WldAdoWeldingSignalDto, WldAdoWeldingSignalServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditWeldingSignalModalComponent } from './create-or-edit-weldingsignal-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './weldingsignal.component.html',
    styleUrls: ['./weldingsignal.component.less'],
})
export class WeldingSignalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalWeldingSignal', { static: true }) createOrEditModalWeldingSignal: | CreateOrEditWeldingSignalModalComponent | undefined;
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

    selectedRow: WldAdoWeldingSignalDto = new WldAdoWeldingSignalDto();
    saveSelectedRow: WldAdoWeldingSignalDto = new WldAdoWeldingSignalDto();
    datas: WldAdoWeldingSignalDto = new WldAdoWeldingSignalDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;


    prodLine: string = '';
    process: string = '';
    signalType: string = '';
    signalBy: string = '';
    signalDate: any;
    signalDateFrom: any;
    signalDateTo: any;
    isActive: string = '';

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
        private _service: WldAdoWeldingSignalServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1
            },
            {
                headerName: this.l('Process')
                , headerTooltip: this.l('Process'),
                field: 'process',
                flex: 1
            },
            {
                headerName: this.l('Signal Type'),
                headerTooltip: this.l('Signal Type'),
                field: 'signalType',
                flex: 1
            },

            {
                headerName: this.l('SignalBy'),
                headerTooltip: this.l('SignalBy'),
                field: 'signalBy',
                flex: 1
            },
            {
                headerName: this.l('Signal Date'),
                headerTooltip: this.l('Signal Date'),
                field: 'signalDate', valueGetter: (params) => this.pipe.transform(params.data?.signalDate, 'dd/MM/yyyy HH:mm:ss'),
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                width: 90,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.prodLine,
            this.process,
            this.signalType,
            this._dateTimeService.convertToDatetime(this.signalDateFrom),
            this._dateTimeService.convertToDatetime(this.signalDateTo),
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
            this.prodLine = '',
            this.process = '',
            this.signalType = '',
            this.signalDateFrom = '',
            this.signalDateTo = '',
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
            this.prodLine,
            this.process,
            this.signalType,
            this._dateTimeService.convertToDatetime(this.signalDateFrom),
            this._dateTimeService.convertToDatetime(this.signalDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );

    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => WldAdoWeldingSignalDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new WldAdoWeldingSignalDto();
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

    deleteRow(system: WldAdoWeldingSignalDto): void {
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
        this._service.getWeldingSignalToExcel(
            this.prodLine,
            this.process,
            this.signalType,
            this._dateTimeService.convertToDatetime(this.signalDateFrom),
            this._dateTimeService.convertToDatetime(this.signalDateTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
}
