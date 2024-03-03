import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GridPoHeadersDto, InvCpsPoHeadersServiceProxy, InvCpsPolinesDto } from '@shared/service-proxies/service-proxies';
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
    templateUrl: './poheaders.component.html',
})
export class PoHeadersComponent extends AppComponentBase implements OnInit {
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
    valueTest = '123';
    ListTest = [
        { value: '', label: '123' },
        { value: '', label: 'All' },
    ];


    inventoryGroupList: { value: number; label: string }[] = [];
    supplierList: { value: number; label: string }[] = [];
    productGroupNameList: any[] = [];
    selectedRowPoHeader: GridPoHeadersDto = new GridPoHeadersDto();
    selectedRowPoLines: InvCpsPolinesDto = new InvCpsPolinesDto();
    saveSelectedRowPoHeader: GridPoHeadersDto = new GridPoHeadersDto();
    saveSelectedRowPoLines: GridPoHeadersDto = new GridPoHeadersDto();
    datas: GridPoHeadersDto = new GridPoHeadersDto();
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

    poNumber = '';
    inventoryGroup = 35;
    pOCreationFromDate;
    pOCreationToDate;
    partNo;
    partName;
    productgroupcode = 'GPS';
    supplierName;
    supplierNumber;
    poHeaderId;

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
        private _service: InvCpsPoHeadersServiceProxy,
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
                headerName: this.l('PO Number'),
                headerTooltip: this.l('PO Number'),
                field: 'poNumber',
                flex: 1
            },
            {
                headerName: this.l('Create Date'),
                headerTooltip: this.l('Create Date'),
                valueGetter: (params) => this.pipe.transform(params.data?.creationTime, 'dd/MM/yyyy'),
                field: 'creationTime',
                flex: 1
            },
            {
                headerName: this.l('Comments'),
                headerTooltip: this.l('Comments'),
                field: 'comments',
                flex: 1
            },
            {
                headerName: this.l('Type'),
                headerTooltip: this.l('Type'),
                field: 'typeLookupCode',
                flex: 1
            },
            {
                headerName: this.l('Inventory Group'),
                headerTooltip: this.l('Inventory Group'),
                field: 'productgroupname',
                flex: 1
            },
            {
                headerName: this.l('Vendor Name'),
                headerTooltip: this.l('Vendor Name'),
                field: 'supplierName',
                flex: 1
            },
            {
                headerName: this.l('Total Price '),
                headerTooltip: this.l('Total Price '),
                field: 'totalPrice', flex: 1,
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.totalPrice, 0),
                type: 'rightAligned'
            },


        ];
        this.poLineColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsPoLines.pageSize * (this.paginationParamsPoLines.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'itemDescription',
                flex: 1
            },
            {
                headerName: this.l('Unit'),
                headerTooltip: this.l('Unit'),
                field: 'unitMeasLookupCode',
                flex: 1
            },
            {
                headerName: this.l('List Price Per Unit'),
                headerTooltip: this.l('List Price Per Unit'),
                field: 'listPricePerUnit',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.listPricePerUnit, 0),
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Unit Price'),
                headerTooltip: this.l('Unit Price'),
                field: 'unitPrice',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.unitPrice, 0),
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity '),
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.quantity, 0),
                field: 'quantity', flex: 1,
                type: 'rightAligned',
            },


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsPoLines = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        let date = new Date();
        date.setDate(date.getDate() - 30);
        this.pOCreationFromDate = date;
        this.pOCreationToDate = new Date;

        this.getCbxInventoryGroup();
        this.getCbxSupplier();

    }
    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    // _CountWin:number = 0;
    createRow(count: number): any[] {
        let result: any[] = [];

        if (this.rowDataPoLines.length > 0) {
            result.push({
                color: 'Grand Total',
                unitPrice: this.rowDataPoLines[0].totalPrice,
            });
        }
        else {
            result.push({
                color: 'Grand Total',
                unitPrice: 0,
            });
        }
        return result;
    }

    getCbxInventoryGroup() {
        this._service.getCbxInventoryGroup().subscribe((result) => {
            result.forEach(e => this.inventoryGroupList.push({ value: e.id, label: e.productgroupname }));
        });
    }
    getCbxSupplier() {
        this._service.getCbxSupplier().subscribe((result) => {
            this.supplierList.push({ value: -1, label: '' });
            result.forEach(e => this.supplierList.push({ value: e.id, label: e.supplierName }));
        });
    }

    searchDatas(): void {
        this.isLoading = true;
        let searchSupplierNumber = this.supplierNumber;
        if (searchSupplierNumber === 1) {
            searchSupplierNumber = -1;
        } else {
            this._service.getCpsPoHeaderSearch(
                this.poNumber,
                this.inventoryGroup ?? -1,
                searchSupplierNumber ?? -1,
                this._dateTimeService.convertToDatetime(this.pOCreationFromDate),
                this._dateTimeService.convertToDatetime(this.pOCreationToDate),
                this.partNo,
                this.partName,
                '',
                this.paginationParams.skipCount,
                this.paginationParams.pageSize
            )
                .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
                .subscribe((result) => {
                    // eslint-disable-next-line eqeqeq
                    if (result.totalCount == 0) {
                        this.rowDataPoLines = [];
                        var rows = this.createRow(1);
                        this.dataParamsPoLines!.api.setPinnedBottomRowData(rows);
                        this.paginationParamsPoLines.totalCount = result.totalCount;
                        this.paginationParamsPoLines.totalPage = ceil(result.totalCount / (this.paginationParamsPoLines.pageSize ?? 0));
                    } else {
                        this.searchDatasPoLines(result.items[0].id);
                    }
                    this.paginationParams.totalCount = result.totalCount;
                    this.rowData = result.items;
                    this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                    this.isLoading = false;
                });
        }
    }

    searchDatasPoLines(supplierId): void {
        this._service.getCpsPoLine(
            supplierId,
            '',
            this.paginationParamsPoLines.skipCount,
            this.paginationParamsPoLines.pageSize
        )
            .pipe(finalize(() => {
                var rows = this.createRow(1);
                this.dataParamsPoLines!.api.setPinnedBottomRowData(rows);
            }
            )).subscribe((result) => {
                this.paginationParamsPoLines.totalCount = result.totalCount;
                this.rowDataPoLines = result.items;
                this.paginationParamsPoLines.totalPage = ceil(result.totalCount / (this.paginationParamsPoLines.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        this.poNumber = '';
        this.inventoryGroup = -1;
        this.supplierNumber = -1;
        this.pOCreationFromDate = new Date(-30);
        this.pOCreationToDate = new Date;
        this.partNo = '';
        this.partName = '';

        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getCpsPoHeaderSearch(
            this.poNumber,
            this.inventoryGroup ?? -1,
            this.supplierNumber ?? -1,
            this._dateTimeService.convertToDatetime(this.pOCreationFromDate),
            this._dateTimeService.convertToDatetime(this.pOCreationToDate),
            this.partNo,
            this.partName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    getDatasPoline(paginationParams?: PaginationParamsModel) {
        return this._service.getCpsPoLine(
            this.poHeaderId,
            '',
            this.paginationParamsPoLines.skipCount,
            this.paginationParamsPoLines.pageSize
        )
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => GridPoHeadersDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected.id) {
            this.poHeaderId = selected.id;
            this.searchDatasPoLines(selected.id);
        }
        this.selectedRowPoHeader = Object.assign({}, selected);
    }

    changePagePoLines(paginationParams) {
        this.paginationParamsPoLines = paginationParams;
        this.paginationParamsPoLines.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasPoline(this.paginationParamsPoLines).subscribe((result) => {
            this.paginationParamsPoLines.totalCount = result.totalCount;
            this.rowDataPoLines = result.items;
            this.paginationParamsPoLines.totalPage = ceil(result.totalCount / (this.paginationParamsPoLines.pageSize ?? 0));
        });
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
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams?.api)
            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    callBackDataGridPoLines(params: GridParams) {
        this.dataParamsPoLines = params;
        this.dataParamsPoLines.api.paginationSetPageSize(
            this.paginationParamsPoLines.pageSize
        );
        this.paginationParamsPoLines.skipCount =
            ((this.paginationParamsPoLines.pageNum ?? 1) - 1) * (this.paginationParamsPoLines.pageSize ?? 0);
        this.paginationParamsPoLines.pageSize = this.paginationParamsPoLines.pageSize;
        this.getDatasPoline(this.paginationParamsPoLines)
            .subscribe((result) => {
                this.paginationParamsPoLines.totalCount = result.totalCount;
                this.rowDataPoLines = result.items ?? [];
                this.paginationParamsPoLines.totalPage = ceil(result.totalCount / (this.paginationParamsPoLines.pageSize ?? 0));
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

        this.fn.exportLoading(e, true);

        let searchSupplierNumber = this.supplierNumber;
        if (searchSupplierNumber === 1) {
            searchSupplierNumber = -1;
        } else {

            this._service.getPoHeadersToExcel(
                this.poNumber,
                this.inventoryGroup ?? -1,
                searchSupplierNumber ?? -1,
                this._dateTimeService.convertToDatetime(this.pOCreationFromDate),
                this._dateTimeService.convertToDatetime(this.pOCreationToDate),
                this.partNo,
                this.partName,
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


    exportToExcelPoLine(e): void {

        this.fn.exportLoading(e, true);

        this._service.getPoLinesToExcel(
            this.poHeaderId
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
