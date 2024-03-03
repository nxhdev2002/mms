import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockIssuingServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ceil } from 'lodash-es';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';

@Component({
    templateUrl: './view-transfer-sloc-modal.component.html',
    selector: 'view-transfer-sloc'
})
export class ViewTransferLocComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewTransferLoc', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];
    paginationParams1: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    frameworkComponents: FrameworkComponent;
    isLoading1: boolean = false;
    pending: string = '';
    disable: boolean = false;
    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];

    workingDateFrom: any;
    workingDateTo: any;
    partType:any;
    _totalcount: number;

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

    constructor(injector: Injector,
        private _service: InvCkdStockIssuingServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('Running No'), headerTooltip: this.l('Running No'), field: 'runningNo' },
            { headerName: this.l('Vin No'), headerTooltip: this.l('Vin No'), field: 'vinNo' },
            { headerName: this.l('Document Date'), headerTooltip: this.l('Document Date'), field: 'documentDate' },
            { headerName: this.l('Posting Date'), headerTooltip: this.l('Posting Date'), field: 'postingDate' },
            { headerName: this.l('Document Header Text'), headerTooltip: this.l('Document Header Text'), field: 'documentHeaderText' },
            { headerName: this.l('Movement Type'), headerTooltip: this.l('Movement Type'), field: 'movementType' },
            { headerName: this.l('Material Code From'), headerTooltip: this.l('Material Code From'), field: 'materialCodeFrom' },
            { headerName: this.l('Plant From'), headerTooltip: this.l('Plant From'), field: 'plantFrom' },
            { headerName: this.l('Valuation Type From'), headerTooltip: this.l('Valuation Type From'), field: 'valuationTypeFrom' },
            { headerName: this.l('Storage Location From'), headerTooltip: this.l('Storage Location From'), field: 'storageLocationFrom' },
            { headerName: this.l('Production Version'), headerTooltip: this.l('Production Version'), field: 'productionVersion' },
            {
                headerName: this.l('Quantity'), headerTooltip: this.l('Quantity'), field: 'quantity', type: 'rightAligned',
                comparator: this._formStoringService.decimalComparator, cellRenderer: (params) => this._fm.formatMoney_decimal(params.data?.quantity),
                aggFunc: this.SumA,
            },
            { headerName: this.l('Unit Of Entry'), headerTooltip: this.l('Unit Of Entry'), field: 'unitOfEntry' },
            { headerName: this.l('Item Text'), headerTooltip: this.l('Item Text'), field: 'itemText' },
            { headerName: this.l('Gl Account'), headerTooltip: this.l('Gl Account'), field: 'glAccount' },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter' },
            { headerName: this.l('Wbs'), headerTooltip: this.l('Wbs'), field: 'wbs' },
            { headerName: this.l('Material Code To'), headerTooltip: this.l('Material Code To'), field: 'materialCodeTo' },
            { headerName: this.l('Plant To'), headerTooltip: this.l('Plant To'), field: 'plantTo' },
            { headerName: this.l('Valuation Type To'), headerTooltip: this.l('Valuation Type To'), field: 'valuationTypeTo' },
            { headerName: this.l('Storage Location To'), headerTooltip: this.l('Storage Location To'), field: 'storageLocationTo' },
            { headerName: this.l('Bf Pc'), headerTooltip: this.l('Bf Pc'), field: 'bfPc' },
            { headerName: this.l('Cancel Flag'), headerTooltip: this.l('Cancel Flag'), field: 'cancelFlag' },
            { headerName: this.l('Reff Mat Doc No'), headerTooltip: this.l('Reff Mat Doc No'), field: 'reffMatDocNo' },
            { headerName: this.l('Vendor No'), headerTooltip: this.l('Vendor No'), field: 'vendorNo' },
            { headerName: this.l('Profit Center'), headerTooltip: this.l('Profit Center'), field: 'profitCenter' },
            { headerName: this.l('Shipemnt Cat'), headerTooltip: this.l('Shipemnt Cat'), field: 'shipemntCat' },
            { headerName: this.l('Reference'), headerTooltip: this.l('Reference'), field: 'reference' },
            { headerName: this.l('Asset No'), headerTooltip: this.l('Asset No'), field: 'assetNo' },
            { headerName: this.l('Sub Asset No'), headerTooltip: this.l('Sub Asset No'), field: 'subAssetNo' },
            { headerName: this.l('End Of Record'), headerTooltip: this.l('End Of Record'), field: 'endOfRecord' }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void { }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number, grandTotal: number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                runningNo: 'Grand Total',
                quantity: grandTotal,
            });
        }
        return result;
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParamsView.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsView.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsView.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 100)
    }

    show(workingDateFrom,workingDateTo,partType,): void {
        this.workingDateFrom = workingDateFrom;
        this.workingDateTo = workingDateTo;
        this.workingDateTo = workingDateTo;
        this.partType = partType;

        this.paginationParams1 = { pageNum: 1, pageSize: 500, totalCount: 0 };

        this.isLoading1 = true;
        this._service.getDataStockIssuingView(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.partType,
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        ).subscribe(result => {
            this.paginationParams1.totalCount = result.totalCount;
            this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
            this.rowData = result.items;
            this.isLoading1 = false;
            var grandTotal = 0;
            if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
            var rows = this.createRow(1, grandTotal);
            this.dataParamsView!.api.setPinnedBottomRowData(rows);
            this._totalcount = result.totalCount;
            this.resetGridView();
        });
        this.modal.show();
    }
    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    // callBackDataGrid(params: GridParams) {
    //     this.isLoading1 = true;
    //     this.dataParamsView = params;
    //     this._service.getDataStockIssuingView(
    //         this._dateTimeService.convertToDatetime(this._workingDateFrom),
    //         '',
    //         this.paginationParams1.skipCount,
    //         this.paginationParams1.pageSize
    //     ).subscribe(result => {
    //         this.rowData = result.items;
    //         var grandTotal = 0;
    //         if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
    //         var rows = this.createRow(1, grandTotal);
    //         this.dataParamsView!.api.setPinnedBottomRowData(rows);
    //         // this.resetGridView();
    //         this.isLoading1 = false;
    //     });
    // }
    // getDatas(paginationParams?: PaginationParamsModel) {
    //     return  this._service.getDataStockIssuingView(
    //         this._dateTimeService.convertToDatetime(this._workingDateFrom),
    //         '',
    //         this.paginationParams1.skipCount,
    //         this.paginationParams1.pageSize
    //         );
    // }
    changePage(paginationParams) {
        // this.isLoading1 = true;
        // this.paginationParams1 = paginationParams;
        // this.paginationParams1.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.getDatas(this.paginationParams1).subscribe((result) => {
        //     this.paginationParams1.totalCount = result.totalCount;
        //     this.rowData = result.items;
        //     this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
        //     this.isLoading1 = false;
        // });
        this.isLoading1 = true;
        this._service.getDataStockIssuingView(
             this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.partType,
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        ).subscribe(result => {
            this.paginationParams1.totalCount = result.totalCount;
            this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
            this.rowData = result.items;
            this.isLoading1 = false;
            var grandTotal = 0;
            if (result.totalCount > 0) grandTotal = result.items[0].grandTotal;
            var rows = this.createRow(1, grandTotal);
            this.dataParamsView!.api.setPinnedBottomRowData(rows);
            this._totalcount = result.totalCount;
            this.resetGridView();
        });
    }
    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        this._service.getDataStockIssuingViewToExcel(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.partType,
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
