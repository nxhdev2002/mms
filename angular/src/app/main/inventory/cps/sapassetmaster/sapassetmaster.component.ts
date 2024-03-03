import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCpsSapAssetMasterDto, InvCpsSapAssetMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { getDate } from 'ngx-bootstrap/chronos/utils/date-getters';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './sapassetmaster.component.html',
})
export class SapAssetMasterComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    poLineColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsPoLines: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    datas: InvCpsSapAssetMasterDto = new InvCpsSapAssetMasterDto();
    isLoading = false;

    dataParams: GridParams | undefined;
    dataParamsPoLines: GridParams | undefined;
    rowData: any[] = [];
    rowDataPoLines: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    companyCode = ''
    fixedAssetNumber = ''
    serialNumber = ''
    wbs = ''
    costCenter = ''

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
        private _service: InvCpsSapAssetMasterServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
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
                headerName: this.l('Status Remark'),
                headerTooltip: this.l('Status Remark'),
                field: 'statusRemark',
                flex: 1
            },
            {
                headerName: this.l('Company Code'),
                headerTooltip: this.l('Company Code'),
                field: 'companyCode',
                flex: 1
            },
            {
                headerName: this.l('Fixed Asset Number'),
                headerTooltip: this.l('Fixed Asset Number'),
                field: 'fixedAssetNumber',
                flex: 1
            },
            {
                headerName: this.l('Sub Asset Number'),
                headerTooltip: this.l('Sub Asset Number'),
                field: 'subAssetNumber',
                flex: 1
            },
            {
                headerName: this.l('Asset Description'),
                headerTooltip: this.l('Asset Description'),
                field: 'assetDescription',
                flex: 1
            },
            {
                headerName: this.l('Additional Asset Description'),
                headerTooltip: this.l('Additional Asset Description'),
                field: 'additionalAssetDescription',
                flex: 1
            },
            {
                headerName: this.l('Asset Class'),
                headerTooltip: this.l('Asset Class'),
                field: 'assetClass',
                flex: 1
            },
            {
                headerName: this.l('Asset Class Description'),
                headerTooltip: this.l('Asset Class Description'),
                field: 'assetClassDescription',
                flex: 1
            },
            {
                headerName: this.l('Serial Number'),
                headerTooltip: this.l('Serial Number'),
                field: 'serialNumber',
                flex: 1
            },
            {
                headerName: this.l('WBS'),
                headerTooltip: this.l('WBS'),
                field: 'wbs',
                flex: 1
            },
            {
                headerName: this.l('Cost Center'),
                headerTooltip: this.l('Cost Center'),
                field: 'costCenter',
                flex: 1
            },
            {
                headerName: this.l('Responsible Cost Center'),
                headerTooltip: this.l('Responsible Cost Center'),
                field: 'responsibleCostCenter',
                flex: 1
            },
            {
                headerName: this.l('Deactivation Date'),
                headerTooltip: this.l('Deactivation Date'),
                field: 'deactivationDate',
                flex: 1
            },
            {
                headerName: this.l('Acquisition Lock'),
                headerTooltip: this.l('Acquisition Lock'),
                field: 'acquisitionLock',
                flex: 1
            },
            {
                headerName: this.l('Cost Of Asset'),
                headerTooltip: this.l('Cost Of Asset'),
                field: 'costOfAsset',
                flex: 1
            },
            {
                headerName: this.l('Line Item'),
                headerTooltip: this.l('Line Item'),
                field: 'lineItem',
                width: 600
            },
            {
                headerName: this.l('Ordering'),
                headerTooltip: this.l('Ordering'),
                field: 'ordering',
                type: 'rightAligned',
                flex: 1
            },
        ];

        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
        this.resetGridView();
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.companyCode,
            this.fixedAssetNumber,
            this.serialNumber,
            this.wbs,
            this.costCenter,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                // eslint-disable-next-line eqeqeq
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.resetGridView();
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }



    clearTextSearch() {
        this.companyCode = '',
        this.fixedAssetNumber = '',
        this.serialNumber = '',
        this.wbs = '',
        this.costCenter = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.companyCode,
            this.fixedAssetNumber,
            this.serialNumber,
            this.wbs,
            this.costCenter,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
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
            this.resetGridView();
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
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams?.api)
            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi?.getAllColumns()?.forEach((column) => {
            // eslint-disable-next-line eqeqeq
            if (column.getId().toString() != 'checked' && column.getId().toString() != 'stt') {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi?.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000);
    } f

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true)
        this._service.getSapAssetMasterToExcel(
            this.companyCode,
            this.fixedAssetNumber,
            this.serialNumber,
            this.wbs,
            this.costCenter,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }
}
