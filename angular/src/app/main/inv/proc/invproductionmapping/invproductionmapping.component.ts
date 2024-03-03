import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent, } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvPeriodDto, InvProductionMappingDto, InvProductionMappingServiceProxy, LgaBp2ProgressDto, LgaBp2ProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';

import { DatePipe, DecimalPipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './invproductionmapping.component.html',
    styleUrls: ['./invproductionmapping.component.less'],
})
export class InvProductionMappingComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvProductionMappingDto = new InvProductionMappingDto();
    saveSelectedRow: InvProductionMappingDto = new InvProductionMappingDto();
    datas: InvProductionMappingDto = new InvProductionMappingDto();
    perioddatas: InvPeriodDto = new InvPeriodDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    periodIdList: any[] = [];
    id;
    from_Date: any;
    to_Date: any;
    status: string = '';
    loadColumdef: CustomColDef[] = [];
    v_shop;
    planSequence;
    shop: string = '';
    model: string = '';;
    lotNo: string = '';;
    noInLot: string = '';
    shift: string = '';
    grade: string = '';
    bodyNo: string = '';
    dateIn: any;
    dateInFrom: any;
    dateInTo: any;
    timeIn: string = '';
    useLotNo: string = '';
    supplierNo: string = '';
    changedRecordsProductionMapping: number [] = [];
    partId;
    quantity;
    cost;
    cif;
    fob;
    freight;
    insurance;
    thc;
    tax;
    inLand;
    amount;
    periodId;
    costVn;
    cifVn;
    fobVn;
    freightVn;
    insuranceVn;
    thcVn;
    taxVn;
    inlandVn;
    amountVn;
    wipId;
    inStockId;
    mappingId;
    selectId;
    

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
        private _service: InvProductionMappingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
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
                field: 'from_Date',
                cellClass: ['text-center'],
                width:75
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'to_Date',
                cellClass: ['text-center'],
                width:75
            },
            {
                headerName: 'Status',
                headerTooltip: 'Status',
                field: 'status',
                cellClass: ['text-center'],
                width:65
            },
        ];

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
                headerName: this.l('Plan Sequence'),
                headerTooltip: this.l('Plan Sequence'),
                field: 'planSequence',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                width: 100,
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 100,
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 100,
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 100,
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 100,
            },
            {
                headerName: this.l('Date In'),
                headerTooltip: this.l('Date In'),
                field: 'dateIn',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
                width: 100,
            },
            {
                headerName: this.l('Time In'),
                headerTooltip: this.l('Time In'),
                field: 'timeIn',
                width: 150,
            },
            {
                headerName: this.l('Use Lot No'),
                headerTooltip: this.l('Use Lot No'),
                field: 'useLotNo',
                width: 100,
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width: 100,
            },

            {
                headerName: this.l('Part Id'),
                headerTooltip: this.l('Part Id'),
                field: 'partId',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'quantity',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cost'),
                headerTooltip: this.l('Cost'),
                field: 'cost',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Freight'),
                headerTooltip: this.l('Freight'),
                field: 'freight',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Insurance'),
                headerTooltip: this.l('Insurance'),
                field: 'insurance',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Thc'),
                headerTooltip: this.l('Thc'),
                field: 'thc',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Tax'),
                headerTooltip: this.l('Tax'),
                field: 'tax',
                width: 100,
                type: 'rightAligned',
            },

            {
                headerName: this.l('InLand'),
                headerTooltip: this.l('InLand'),
                field: 'inLand',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Amount'),
                headerTooltip: this.l('Amount'),
                field: 'amount',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('PeriodId'),
                headerTooltip: this.l('PeriodId'),
                field: 'periodId',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cost Vn'),
                headerTooltip: this.l('Cost Vn'),
                field: 'costVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cif Vn'),
                headerTooltip: this.l('Cif Vn'),
                field: 'cifVn',
                width: 100,
                type: 'rightAligned',
            },

            {
                headerName: this.l('Fob Vn'),
                headerTooltip: this.l('Fob Vn'),
                field: 'fobVn',
                width: 100,
                type: 'rightAligned',
            },

            {
                headerName: this.l('Freight Vn'),
                headerTooltip: this.l('Freight Vn'),
                field: 'freightVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Insurance Vn'),
                headerTooltip: this.l('Insurance Vn'),
                field: 'insuranceVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Thc Vn'),
                headerTooltip: this.l('Thc Vn'),
                field: 'thcVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Tax Vn'),
                headerTooltip: this.l('Tax Vn'),
                field: 'taxVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Inland Vn'),
                headerTooltip: this.l('Inland Vn'),
                field: 'inlandVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Amount Vn'),
                headerTooltip: this.l('Amount Vn'),
                field: 'amountVn',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Wip Id'),
                headerTooltip: this.l('Wip Id'),
                field: 'wipId',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('In Stock Id'),
                headerTooltip: this.l('In Stock Id'),
                field: 'inStockId',
                width: 100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Mapping Id'),
                headerTooltip: this.l('Mapping Id'),
                field: 'mappingId',
                width: 100,
                type: 'rightAligned',
            },

        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
            this._service.getIdInvPeriod()
            .subscribe((result) => {
                this.periodIdList = result.items;
            });
        this.fetchChangedRecords();
    }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsProductionMapping = result;
            console.log("result =", result);
        })
    }


    searchDatas(this): void {
        this.isLoading = true;;
        this._service.getAll(
                this.id,
                this.shop,
                this.bodyNo,
                this.lotNo,
                this._dateTimeService.convertToDatetime(this.dateInFrom),
                this._dateTimeService.convertToDatetime(this.dateInTo),
                '',
                this.paginationParams.skipCount,
                this.paginationParams.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }
    searchDatasShop(shop): void {
        this.isLoading = true;
        this._service.getAll(
                this.periodId,
                shop,
                this.bodyNo,
                this.lotNo,
                this._dateTimeService.convertToDatetime(this.dateInFrom),
                this._dateTimeService.convertToDatetime(this.dateInTo),
                '',
                this.paginationParams.skipCount,
                this.paginationParams.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.id = '',
        this.periodId = '',
        this.shop = '',
        this.bodyNo = '',
        this.lotNo = '',
        this.dateInFrom = '',
        this.dateInTo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.periodId,
            this.shop,
            this.bodyNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.dateInFrom),
            this._dateTimeService.convertToDatetime(this.dateInTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvProductionMappingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvProductionMappingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
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

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getExportToFile(
            this.periodId,
            this.v_shop,
            this.bodyNo,
            this.lotNo,
            this._dateTimeService.convertToDatetime(this.dateInFrom),
            this._dateTimeService.convertToDatetime(this.dateInTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
}
