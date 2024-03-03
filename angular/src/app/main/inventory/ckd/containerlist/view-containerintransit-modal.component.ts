import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerListServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import ceil from 'lodash-es/ceil';

@Component({
    templateUrl: './view-containerintransit-modal.component.html',
    selector: 'view-containerintransit'
})
export class ViewContainerIntransitComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewContainerIntransit', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
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

    _containerNo: string = '';
    _renban: string = '';
    _invoiceNo: string = '';

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
        private _service: InvCkdContainerListServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Container No'),headerTooltip: this.l('Container No'),field: 'containerNo'},
            {headerName: this.l('Renban'),headerTooltip: this.l('Renban'),field: 'renban'},
            {headerName: this.l('Invoice No'),headerTooltip: this.l('Invoice No'),field: 'invoiceNo'},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo'},
            {headerName: this.l('Seal No'),headerTooltip: this.l('Seal No'),field: 'sealNo'},
            {headerName: this.l('Bill of Lading No'),headerTooltip: this.l('Bill of Lading No'),field: 'billOfLadingNo'},
            {headerName: this.l('Bill Date'),headerTooltip: this.l('Bill Date'),field: 'billDate',
        valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy')},
			{headerName: this.l('Container Size'),headerTooltip: this.l('Container Size'),field: 'containerSize',type: 'rightAligned'},
            {headerName: this.l('Plandedvanning Date'),headerTooltip: this.l('Plandedvanning Date'),field: 'plandedvanningDate',
            valueGetter: (params) => this.pipe.transform(params.data?.plandedvanningDate, 'dd/MM/yyyy')},
            {headerName: this.l('Actualvanning Date'),headerTooltip: this.l('Actualvanning Date'),field: 'actualvanningDate',
           valueGetter: (params) => this.pipe.transform(params.data?.actualvanningDate, 'dd/MM/yyyy')},
            {headerName: this.l('Cd Date'),headerTooltip: this.l('Cd Date'),field: 'cdDate',valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy')},
            {headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status'},
            {headerName: this.l('Date Status'),headerTooltip: this.l('Date Status'),field: 'dateStatus'},
            {headerName: this.l('Fob'),headerTooltip: this.l('Fob'),field: 'fob',type: 'rightAligned',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4)},
            {headerName: this.l('Freight'),headerTooltip: this.l('Freight'),field: 'freight',type: 'rightAligned',
           valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.freight, 4)},
            {headerName: this.l('Insurance'),headerTooltip: this.l('Insurance'),field: 'insurance',type: 'rightAligned',
           valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4)},
            {headerName: this.l('Tax'),headerTooltip: this.l('Tax'),field: 'tax',type: 'rightAligned',
           valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4)},
            {headerName: this.l('Amount'),headerTooltip: this.l('Amount'),field: 'amount',type: 'rightAligned',
           valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4)},
            {headerName: this.l('Tax VN'),headerTooltip: this.l('Tax VN'),field: 'taxVnd', type: 'rightAligned',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.taxVnd, 0)
           },
            {headerName: this.l('Vat VN'),headerTooltip: this.l('Vat VN'),field: 'vatVnd', type: 'rightAligned',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.vatVnd, 0)
           },
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

        setTimeout(()=>{
            this.dataParamsView.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },150)
    }

    show(contNo: string, renban: string, invoiceNo: string): void {
        this._containerNo = contNo;
        this._renban = renban;
        this._invoiceNo = invoiceNo;
        this.isLoading =  true;
        this._service.getDataContainerInvoiceByCont(
            this._containerNo,
            this._renban,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.paginationParams.totalCount = result.totalCount;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));

            this.rowData = result.items;
            this.resetGridView();
            // this.isLoading = false;
        });
        this.modal.show();
    }
    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    callBackDataGrid(params: GridParams){
        this.isLoading = true;
        this.dataParamsView = params;
        this._service.getDataContainerInvoiceByCont(
            this._containerNo,
            this._renban,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.rowData = result.items;
            this.resetGridView();
            // this.isLoading = false;
        });
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        this._service.getDataContainerInvoiceByContToExcel(
            this._containerNo,
            this._renban,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
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
