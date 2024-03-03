import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwMwhContListDto, LgwMwhContListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './contlist.component.html',
    styleUrls: ['./contlist.component.less'],
})
export class ContListComponent extends AppComponentBase implements OnInit {
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

    selectedRow: LgwMwhContListDto = new LgwMwhContListDto();
    saveSelectedRow: LgwMwhContListDto = new LgwMwhContListDto();
    datas: LgwMwhContListDto = new LgwMwhContListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    containerNo: string = '';
    renban: string = '';
    supplierNo: string = '';
    devanningDate: any;
    devanningDateFrom: any;
    devanningDateTo: any;

    startDevanningDate: any;
    finishDevanningDate: any;
    status: string = '';
    contScheduleId: number = 0;
    shop: string = '';
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
        private _service: LgwMwhContListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService

    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                flex: 1
            },
            {
                headerName: this.l('Renban'),
                headerTooltip: this.l('Renban'),
                field: 'renban',
                flex: 1 },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Devanning Date'),
                headerTooltip: this.l('Devanning Date'),
                field: 'devanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                width: 115,
            },
            {
                headerName: this.l('Start Devanning Date'),
                headerTooltip: this.l('Start Devanning Date'),
                field: 'startDevanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.startDevanningDate, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Finish Devanning Date'),
                headerTooltip: this.l('Finish Devanning Date'),
                field: 'finishDevanningDate',
                valueGetter: (params) => this.pipe.transform(params.data?.finishDevanningDate, 'dd/MM/yyyy HH:mm:ss'),
                width: 150,
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1
            },
            // {
            //     headerName: this.l('Cont Schedule Id'),
            //     headerTooltip: this.l('Cont Schedule Id'),
            //     field: 'contScheduleId',
            //     width: 120,
            // },
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
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
        this._service.getAll(
            this.containerNo,
            this.renban,
            this._dateTimeService.convertToDatetime(this.devanningDateFrom),
            this._dateTimeService.convertToDatetime(this.devanningDateTo),
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
        this.containerNo = '',
        this.renban = '',
        this.devanningDateFrom = null;
        this.devanningDateTo = null;

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
            this.containerNo,
            this.renban,
            this._dateTimeService.convertToDatetime(this.devanningDateFrom),
            this._dateTimeService.convertToDatetime(this.devanningDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwMwhContListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwMwhContListDto();
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

    exportToExcel(): void {
        this._service.getContListToExcel(
            this.containerNo,
            this.renban,
            this._dateTimeService.convertToDatetime(this.devanningDateFrom),
            this._dateTimeService.convertToDatetime(this.devanningDateTo),
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
