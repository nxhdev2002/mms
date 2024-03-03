import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PtsAdoScanInfoDto, PtsAdoScanInfoServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './scaninfo.component.html',
    styleUrls: ['./scaninfo.component.less'],
})
export class ScanInfoComponent extends AppComponentBase implements OnInit {
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

    selectedRow: PtsAdoScanInfoDto = new PtsAdoScanInfoDto();
    saveSelectedRow: PtsAdoScanInfoDto = new PtsAdoScanInfoDto();
    datas: PtsAdoScanInfoDto = new PtsAdoScanInfoDto();
    isLoading: boolean = false;;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    scanType: string = '';
    scanValue: string = '';
    scanLocation: string = '';
    scanTime: any;
    scanBy: string = '';
    isProcessed: string = '';
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
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
        private _service: PtsAdoScanInfoServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Scan Type'),
                headerTooltip: this.l('Scan Type'),
                field: 'scanType',
                flex: 1,
            },
            {
                headerName: this.l('Scan Value'),
                headerTooltip: this.l('Scan Value'),
                field: 'scanValue',
                flex: 1,
            },
            {
                headerName: this.l('Scan Location'),
                headerTooltip: this.l('Scan Location'),
                field: 'scanLocation',
                flex: 1,
            },
            {
                headerName: this.l('Scan Time'),
                headerTooltip: this.l('Scan Time'),
                field: 'scanTime',
                valueGetter: (params) => this.pipe.transform(params.data?.scanTime, 'dd/MM/yyyy HH:mm:ss'),
                flex: 1,
            },
            {
                headerName: this.l('Scan By'),
                headerTooltip: this.l('Scan By'),
                field: 'scanBy',
                flex: 1,
            },
            {
                headerName: this.l('Is Processed'),
                headerTooltip: this.l('Is Processed'),
                field: 'isProcessed',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isProcessed == 'Y' ? 'Processed' : 'InProcessed'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isProcessed == 'Y' ? 'btnActive' : 'btnInActive'),
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
        this._service
            .getAll(
                this.scanType,
                this.scanValue,
                this.scanLocation,
                this.scanTime,
                this.scanBy,
                this.isProcessed,
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
        (this.scanType = ''),
            (this.scanValue = ''),
            (this.scanLocation = ''),
            (this.scanTime = ''),
            (this.scanBy = ''),
            (this.isProcessed = ''),
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
            this.scanType,
            this.scanValue,
            this.scanLocation,
            this.scanTime,
            this.scanBy,
            this.isProcessed,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PtsAdoScanInfoDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PtsAdoScanInfoDto();
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

    deleteRow(system: PtsAdoScanInfoDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                // this._service.delete(system.id).subscribe(() => {
                //     this.callBackDataGrid(this.dataParams!);
                //     this.notify.success(this.l('SuccessfullyDeleted'));
                //     this.notify.info(this.l('SuccessfullyDeleted'));
                // });
            }
        });
    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service
            .getScanInfoToExcel(
                this.scanType,
                this.scanLocation,
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }


}
