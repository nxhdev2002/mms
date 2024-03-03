import { InvCkdInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ceil } from 'lodash-es';

@Component({
    templateUrl: './view-lot-details-modal.component.html',
    selector: 'view-lotdetails'
})
export class ViewLotDetailsComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewLotDetails', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];

    _lotNo: string = '';

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

    constructor( injector: Injector,
        private _service: InvCkdInvoiceServiceProxy,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo'},
			{headerName: this.l('Carfamily Code'),headerTooltip: this.l('Carfamily Code'),field: 'carfamilyCode'},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo'},
			{headerName: this.l('Lot No'),headerTooltip: this.l('Lot No'),field: 'lotNo'},
			{headerName: this.l('Module No'),headerTooltip: this.l('Module No'),field: 'moduleNo'},
			{headerName: this.l('Container No'),headerTooltip: this.l('Container No'),field: 'containerNo'},
			{headerName: this.l('Invoice No'),headerTooltip: this.l('Invoice No'),field: 'invoiceNo'},
			{headerName: this.l('Usage Qty'),headerTooltip: this.l('Usage Qty'),field: 'usageQty',type: 'rightAligned'},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName'}
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void{}

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParamsView.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParamsView.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){
            this.dataParamsView.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
                this.autoSizeAll();
        
    }


    show(contNo: string): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._lotNo = contNo;
        this._service.getDataInvoiceDetailsByLot(
            this._lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
        });
        this.modal.show();
    }
    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    // callBackDataGrid(params: GridParams){
    //     this.dataParamsView = params;
    //     this._service.getDataInvoiceDetailsByLot(
    //         this._lotNo,
    //         '',
    //         this.paginationParams.skipCount,
    //         this.paginationParams.pageSize
    //     ).subscribe(result => {
    //         this.paginationParams.totalCount = result.totalCount;
    //         this.rowData = result.items;
    //         this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
    //         this.resetGridView();
    //     });
    // }

    changePage(paginationParams) {
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
        });
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getDataInvoiceDetailsByLot(
            this._lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        this._service.getInvoiceDetailsByLotNoToExcel(
            this._lotNo
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
