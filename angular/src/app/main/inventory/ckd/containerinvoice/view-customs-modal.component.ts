import { Component, ElementRef, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerInvoiceServiceProxy, InvCpsSapAssetInput, InvCkdContainerInvoiceViewCustomsDto, InvDrmPartListDto, InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ceil } from 'lodash-es';
import { finalize } from 'rxjs';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-customs-modal.component.html',
    selector: 'view-customs-modal'
})
export class ViewCustomsModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewCustoms', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    validateColDefs: CustomColDef[] = [];
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
    dataParams: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    selectedRow: InvCkdContainerInvoiceViewCustomsDto = new InvCkdContainerInvoiceViewCustomsDto();
    saveSelectedRow: InvCkdContainerInvoiceViewCustomsDto = new InvCkdContainerInvoiceViewCustomsDto();
    containerInvoiceId;
    fn: CommonFunction = new CommonFunction();
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
        private _service: InvCkdContainerInvoiceServiceProxy,
        private gridTableService: GridTableService,
        private _fm: DataFormatService,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);

        this.validateColDefs = [
            // { headerName: this.l('ContInvId'), headerTooltip: this.l('ContInvId'), field: 'contInvId', flex: 1 },
            // { headerName: this.l('InvoiceId'), headerTooltip: this.l('InvoiceId'), field: 'invoiceId', flex: 1 },
            // { headerName: this.l('BillId'), headerTooltip: this.l('BillId'), field: 'billId', flex: 1 },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', flex: 1 },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo', flex: 1 },
            {
                headerName: this.l('Declare Date'),
                headerTooltip: this.l('Declare Date'),
                field: 'declareDate', type: 'rightAligned',
                valueFormatter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                flex: 1
            },
            { headerName: this.l('Customs Declare No'), headerTooltip: this.l('Customs Declare No'), field: 'customsDeclareNo', flex: 1 },
            { headerName: this.l('Taxvnd'), headerTooltip: this.l('Taxvnd'), field: 'taxvnd',type: 'rightAligned', flex: 1 },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency', flex: 1 },
            { headerName: this.l('Exchange Rate'), headerTooltip: this.l('Exchange Rate'), field: 'exchangeRate',type: 'rightAligned', flex: 1 },
            { headerName: this.l('BillOflading No'), headerTooltip: this.l('BillOflading No'), field: 'billOfladingNo', flex: 1 },
            { headerName: this.l('Status'), headerTooltip: this.l('Status'), field: 'description', flex: 1 }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                ``
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        // setTimeout(() => {
        //     this.dataParams.columnApi!.sizeColumnsToFit({
        //         suppressColumnVirtualisation: true,
        //     });

        //     this.autoSizeAll();
        // })
    }

    searchDatas() {
        this.getDatas(this.paginationParams).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
            this.isLoading = false;
        });
    }

    show(containerInvoiceId): void {
        this.containerInvoiceId = containerInvoiceId;
        this.searchDatas();
        this.modal.show();
    }


    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvCkdContainerInvoiceViewCustoms(
            this.containerInvoiceId,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerInvoiceViewCustomsDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerInvoiceViewCustomsDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    callBackGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.resetGridView();
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
            this.isLoading = false;
        });
    }

    exportExcel(e) {
        this.fn.exportLoading(e, true);

        this._service.getInvCkdContainerInvoiceViewCustomsToExcel(
            this.containerInvoiceId
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
