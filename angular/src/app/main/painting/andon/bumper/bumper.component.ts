import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PtsAdoBumperDto, PtsAdoBumperServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './bumper.component.html',
    styleUrls: ['./bumper.component.less'],
})
export class BumperComponent extends AppComponentBase implements OnInit {
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

    selectedRow: PtsAdoBumperDto = new PtsAdoBumperDto();
    saveSelectedRow: PtsAdoBumperDto = new PtsAdoBumperDto();
    datas: PtsAdoBumperDto = new PtsAdoBumperDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    wipId
    model: string = '';
    grade: string = '';
    bodyNo: string = '';
    lotNo: string = '';
    noInLot: number = 0;
    color: string = '';
    bumperStatus: number = 0;
    initialDate: any;
    i1Date: any;
    i2Date: any;
    inlInDate: any;
    inlOutDate: any;
    preparationDate: any;
    unpackingDate: any;
    jigSettingDate: any;
    boothDate: any;
    finishedDate: any;
    extSeq: number = 0;
    unpSeq: number = 0;
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
        private _service: PtsAdoBumperServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('Wip Id'),
                headerTooltip: this.l('Wip Id'),
                field: 'wipId',
                width: 120,
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 120,
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 120,
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 120,
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 120,
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                width: 120,
            },
            {
                headerName: this.l('Bumper Status'),
                headerTooltip: this.l('Bumper Status'),
                field: 'bumperStatus',
                width: 120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Initial Date'),
                headerTooltip: this.l('Initial Date'),
                field: 'initialDate', valueGetter: (params) => this.pipe.transform(params.data?.initialDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('I1 Date'),
                headerTooltip: this.l('I1 Date'),
                field: 'i1Date', valueGetter: (params) => this.pipe.transform(params.data?.i1Date, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('I2 Date'),
                headerTooltip: this.l('I2 Date'),
                field: 'i2Date', valueGetter: (params) => this.pipe.transform(params.data?.i2Date, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Inl In Date'),
                headerTooltip: this.l('Inl In Date'),
                field: 'inlInDate', valueGetter: (params) => this.pipe.transform(params.data?.inlInDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Inl Out Date'),
                headerTooltip: this.l('Inl Out Date'),
                field: 'inlOutDate', valueGetter: (params) => this.pipe.transform(params.data?.inlOutDate, 'dd/MM/yyyy hh:MM'), width: 120,
            },
            {
                headerName: this.l('Preparation Date'), headerTooltip: this.l('Preparation Date'), field: 'preparationDate', valueGetter: (params) => this.pipe.transform(params.data?.preparationDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Unpacking Date'),
                headerTooltip: this.l('Unpacking Date'),
                field: 'unpackingDate', valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Jig Setting Date'),
                headerTooltip: this.l('Jig Setting Date'),
                field: 'jigSettingDate', valueGetter: (params) => this.pipe.transform(params.data?.jigSettingDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Booth Date'),
                headerTooltip: this.l('Booth Date'),
                field: 'boothDate', valueGetter: (params) => this.pipe.transform(params.data?.boothDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Finished Date'),
                headerTooltip: this.l('Finished Date'),
                field: 'finishedDate', valueGetter: (params) => this.pipe.transform(params.data?.finishedDate, 'dd/MM/yyyy hh:MM'),
                width: 120,
            },
            {
                headerName: this.l('Ext Seq'),
                headerTooltip: this.l('Ext Seq'),
                field: 'extSeq',
                width: 120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Unp Seq'),
                headerTooltip: this.l('Unp Seq'),
                field: 'unpSeq',
                width: 120,
                type: 'rightAligned',
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.model,
            this.grade,
            this.bodyNo,
            this.lotNo,

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
        this.wipId = '',
            this.model = '',
            this.grade = '',
            this.bodyNo = '',
            this.lotNo = '',

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
            this.model,
            this.grade,
            this.bodyNo,
            this.lotNo,

            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PtsAdoBumperDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PtsAdoBumperDto();
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
        this.isLoading = true;
        this._service.getBumperToExcel(
            this.model,
            this.grade,
            this.bodyNo,
            this.lotNo,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
    
    
}
