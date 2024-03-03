import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgaPcsStockDto, LgaPcsStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditStockModalComponent } from './create-or-edit-stock-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.less'],
})
export class StockComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalStock', { static: true }) createOrEditModalStock: | CreateOrEditStockModalComponent | undefined;
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

    selectedRow: LgaPcsStockDto = new LgaPcsStockDto();
    saveSelectedRow: LgaPcsStockDto = new LgaPcsStockDto();
    datas: LgaPcsStockDto = new LgaPcsStockDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partName: string = '';
    supplierNo: string = '';
    backNo: string = '';
    pcRackAddress: string = '';
    usagePerHour: number = 0;
    rackCapBox: number = 0;
    outType: string = '';
    stockQty: number = 0;
    boxQty: number = 0;
    isActive;

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
        private _service: LgaPcsStockServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
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
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                width:120
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                width:300,
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Back No'),
                headerTooltip: this.l('Back No'),
                field: 'backNo',
                flex: 1
            },
            {
                headerName: this.l('Pc Rack Address'),
                headerTooltip: this.l('Pc Rack Address'),
                field: 'pcRackAddress',
                flex: 1
            },
            {
                headerName: this.l('Usage Per Hour'),
                headerTooltip: this.l('Usage Per Hour'),
                field: 'usagePerHour',
                flex: 1
            },
            {
                headerName: this.l('Rack Cap Box'),
                headerTooltip: this.l('Rack Cap Box'),
                field: 'rackCapBox',
                flex: 1
            },
            {
                headerName: this.l('Out Type'),
                headerTooltip: this.l('Out Type'),
                field: 'outType',
                flex: 1
            },
            {
                headerName: this.l('Stock Qty'),
                headerTooltip: this.l('Stock Qty'),
                field: 'stockQty',
                flex: 1
            },
            {
                headerName: this.l('Box Qty'),
                headerTooltip: this.l('Box Qty'),
                field: 'boxQty',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
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
            this.partNo,
            this.partName,
            this.supplierNo,
            this.backNo,
            this.pcRackAddress,
            this.usagePerHour,
            this.rackCapBox,
            this.outType,
            this.stockQty,
            this.boxQty,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });
    }
    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){
      
        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true, 
            });
            this.autoSizeAll();
        },100)
    }
    clearTextSearch() {
        this.partNo = '',
            this.partName = '',
            this.supplierNo = '',
            this.backNo = '',
            this.pcRackAddress = '',
            this.usagePerHour = 0,
            this.rackCapBox = 0,
            this.outType = '',
            this.stockQty = 0,
            this.boxQty = 0,
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
            this.partNo,
            this.partName,
            this.supplierNo,
            this.backNo,
            this.pcRackAddress,
            this.usagePerHour,
            this.rackCapBox,
            this.outType,
            this.stockQty,
            this.boxQty,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgaPcsStockDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgaPcsStockDto();
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
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    deleteRow(system: LgaPcsStockDto): void {
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
        this._service.getStockToExcel(
            this.partNo,
            this.partName,
            this.supplierNo,
            this.backNo,
            this.pcRackAddress,
            this.usagePerHour,
            this.rackCapBox,
            this.outType,
            this.stockQty,
            this.boxQty,
            this.isActive
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
