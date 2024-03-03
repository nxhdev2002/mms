import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsReceivingDto, InvGpsReceivingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditReceivingModalComponent } from './create-or-edit-receiving-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ImportReceivingComponent } from './import-receiving.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { AgCheckboxRendererComponent } from '@app/shared/common/grid/ag-checkbox-renderer/ag-checkbox-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    templateUrl: './receiving.component.html',
    styleUrls: ['./receiving.component.less'],
})
export class ReceivingComponent extends AppComponentBase implements OnInit {
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportReceivingComponent | undefined;
    @ViewChild('createOrEditModalStockReceiving', { static: true }) createOrEditModalStockReceiving: | CreateOrEditReceivingModalComponent | undefined;
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

    selectedRow: InvGpsReceivingDto = new InvGpsReceivingDto();
    saveSelectedRow: InvGpsReceivingDto = new InvGpsReceivingDto();
    datas: InvGpsReceivingDto = new InvGpsReceivingDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();
    rowDataUpdate: InvGpsReceivingDto = new InvGpsReceivingDto();
    poNo: string = '';
    partNo: string = '';
    supplier: string = '';
    receivedDateFrom: any;
    receivedDateTo: any;
    dock;
    isDockList = [
        {value: '' , label: "All"},
        {value: 'T' , label: "T"},
        {value: 'GPS' , label: "GPS"},
    ];
    date = new Date();

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
        private _service: InvGpsReceivingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('PO No'), headerTooltip: this.l('PO No'), field: 'poNo', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('UOM'), headerTooltip: this.l('UOM'), field: 'uom', flex: 1 },
            { headerName: this.l('Box Qty'), headerTooltip: this.l('Boxqty'), field: 'boxqty', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Box'), headerTooltip: this.l('Box'), field: 'box', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'),editable: true, field: 'qty', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('PoPrice'), headerTooltip: this.l('PoPrice'), field: 'poPrice', flex: 1, type: 'rightAligned',
                cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.poPrice)
            },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', flex: 1 },
            { headerName: this.l('Prod Date'), headerTooltip: this.l('Prod Date'), field: 'prodDate', flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.prodDate, 'dd/MM/yyyy') },
            { headerName: this.l('Expiry Date'), headerTooltip: this.l('Exp Date'), field: 'expDate', flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.expDate, 'dd/MM/yyyy') },
            { headerName: this.l('Supplier'), headerTooltip: this.l('Supplier'), field: 'supplier', flex: 1 },
            { headerName: this.l('Received Date'), headerTooltip: this.l('Received Date'), field: 'receivedDate', flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.receivedDate, 'dd/MM/yyyy') },
            { headerName: this.l('Received User'), headerTooltip: this.l('Received User'), field: 'userReceives', flex: 1},
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1},
            { headerName: this.l('Dock'), headerTooltip: this.l('Dock'), field: 'dock', flex: 1}
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
            agCheckboxRendererComponent: AgCheckboxRendererComponent
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        //this.receivedDateFrom = this.date;
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.poNo,
            this.partNo,
            this.supplier,
            this._dateTimeService.convertToDatetime(this.receivedDateFrom),
            this._dateTimeService.convertToDatetime(this.receivedDateTo),
            this.dock,
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
            this.poNo = '',
            this.partNo = '',
            this.supplier = '',
            this.receivedDateFrom = '',
            this.receivedDateTo = '',
            this.dock = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.poNo,
            this.partNo,
            this.supplier,
            this.receivedDateFrom,
            this.receivedDateTo,
            this.dock,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsReceivingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsReceivingDto();
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

    onCellValueChanged(cellParams){
        this.saveSelectedRow.qty =   this.rowData[cellParams.rowIndex].qty
        this._service.createOrEdit(this.saveSelectedRow)
        .pipe( finalize(() => this.isLoading = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
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

    deleteRow(system: InvGpsReceivingDto): void {
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockReceivingToExcel(
            this.poNo,
            this.partNo,
            this.supplier,
            this.receivedDateFrom,
            this.receivedDateTo,
            this.dock,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
        });
    }
}
