import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPhysicalStockPartS4Dto, InvCkdPhysicalStockPartS4ServiceProxy, InvCkdPhysicalStockPartServiceProxy, InvCkdPhysicalStockPeriodServiceProxy, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ImportInvCkdPhysicalStockPartS4Component } from './import-physical-stock-part-s4.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './physical-stock-part-s4.component.html',
})
export class PhysicalStockPartS4Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportInvCkdPhysicalStockPartS4Component | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPhysicalStockPartS4Dto = new InvCkdPhysicalStockPartS4Dto();
    saveSelectedRow: InvCkdPhysicalStockPartS4Dto = new InvCkdPhysicalStockPartS4Dto();
    datas: InvCkdPhysicalStockPartS4Dto = new InvCkdPhysicalStockPartS4Dto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partNoNormalized: string = '';
    partName: string = '';
    partNoNormalizedS4: string = '';
    colorSfx: string = '';
    lotNo: string = '';
    partListId
    materialId
    receiveQty
    actualQty
    beginQty
    issueQty
    transtype
    calculatorQty
    lastCalDatetime
    workingDate
    remark
    periodId: number = 0;
    isActive: string = '';
    negativeStock: boolean = false;
    supplierNo;
    p_mode = 0;
    Period: any;
    cfc;
    cbbPeriod: any[] = [];
    id: any;
    loadColumdef: CustomColDef[] = [];
    periodIdList: any[] = [];
    materialCode;

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
        private _service: InvCkdPhysicalStockPartS4ServiceProxy,
        private _serviceStock: InvCkdPhysicalStockPeriodServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width:100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'fromDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),
                flex:1
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'toDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),
                flex:1
            },
        ];
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 60, },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode',flex:1 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty',flex:1,type: 'rightAligned'  },
            { headerName: this.l('From Date'), headerTooltip: this.l('From Date'), field: 'fromDate',valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),flex:1  },
            { headerName: this.l('To Date'), headerTooltip: this.l('To Date'), field: 'toDate',valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),flex:1  },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description',flex:1  },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceStock.getIdInvPhysicalStockPeriod()
            .subscribe((result) => {
                this.periodIdList = result.items;
                this.Period = (result.items.filter(s => s.status = 1))[0].infoPeriod;
            });

    }
    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.materialCode,
            this.id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => {
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            // var rows = this.createRow(1);
            // this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
        }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
              //  this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.materialCode = "",
        this.id = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.materialCode,
            this.id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPhysicalStockPartS4Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPhysicalStockPartS4Dto();
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
            this.gridTableService.selectFirstRow(this.dataParams!.api)
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
        .pipe(finalize(() => {
            this.gridTableService.selectFirstRow(this.dataParams!.api);
        }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    compareStockPart(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPhysicalStockCPartS4CompareToExcel(this.id)
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
