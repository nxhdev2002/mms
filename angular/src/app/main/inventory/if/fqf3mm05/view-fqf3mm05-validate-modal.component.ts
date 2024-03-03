import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, BusinessDataDto, IF_FQF3MM03ServiceProxy, GetIF_FQF3MM05_VALIDATE, IF_FQF3MM05ServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-fqf3mm05-validate-modal.component.html',
    selector: 'view-fqf3mm05-validate-modal'
})
export class ViewFqf3mm05ValidateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm05Validate', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: GetIF_FQF3MM05_VALIDATE = new GetIF_FQF3MM05_VALIDATE();
    saveSelectedRow: GetIF_FQF3MM05_VALIDATE = new GetIF_FQF3MM05_VALIDATE();
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    idFund: number;
    logId;
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
    postingDateFrom;
    postingDateTo;

    constructor(injector: Injector,
        private _service: IF_FQF3MM05ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
           
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            { headerName: this.l('Running No (M)'), headerTooltip: this.l('Running No (M)'), field: 'runningNo', flex: 1 },
            { headerName: this.l('Document Date (M)'), headerTooltip: this.l('Document Date (M)'), field: 'documentDate', flex: 1 },
            { headerName: this.l('Posting Date (M)'), headerTooltip: this.l('Posting Date (M)'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Document Header Text (M)'), headerTooltip: this.l('Document Header Text (M)'), field: 'documentHeaderText', flex: 1 },
            { headerName: this.l('Movement Type (M)'), headerTooltip: this.l('Movement Type (M)'), field: 'movementType', flex: 1 },
            { headerName: this.l('Material Code From (M)'), headerTooltip: this.l('Material Code From (M)'), field: 'materialCodeFrom', flex: 1 },
            { headerName: this.l('Plant From (M)'), headerTooltip: this.l('Plant From (M)'), field: 'plantFrom', flex: 1 },
            { headerName: this.l('Storage Location From (M)'), headerTooltip: this.l('Storage Location From (M)'), field: 'storageLocationFrom', flex: 1 },
            { headerName: this.l('Quantity (M)'), headerTooltip: this.l('Quantity (M)'), field: 'quantity', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Unit Of Entry (M)'), headerTooltip: this.l('Unit Of Entry (M)'), field: 'unitOfEntry', flex: 1 },
            { headerName: this.l('Cost Center (C)'), headerTooltip: this.l('Cost Center (C)'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Wbs (C)'), headerTooltip: this.l('Wbs (C)'), field: 'wbs', flex: 1 },
            { headerName: this.l('Material Code To (C)'), headerTooltip: this.l('Material Code To (C)'), field: 'materialCodeTo', flex: 1 },
            { headerName: this.l('Plant To (C)'), headerTooltip: this.l('Plant To (C)'), field: 'plantTo', flex: 1 },
            { headerName: this.l('Valuation Type To (C)'), headerTooltip: this.l('Valuation Type To (C)'), field: 'valuationTypeTo', flex: 1 },
            { headerName: this.l('Storage Location To (C)'), headerTooltip: this.l('Storage Location To (C)'), field: 'storageLocationTo', flex: 1 },
            { headerName: this.l('Reff Mat Doc No (C)'), headerTooltip: this.l('Reff Mat Doc No (C)'), field: 'reffMatDocNo', flex: 1 },
            { headerName: this.l('Vendor No (C)'), headerTooltip: this.l('Vendor No (C)'), field: 'vendorNo', flex: 1 },
            { headerName: this.l('Profit Center (C)'), headerTooltip: this.l('Profit Center (C)'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Shipemnt Cat (C)'), headerTooltip: this.l('Shipemnt Cat (C)'), field: 'shipemntCat', flex: 1 },
            { headerName: this.l('Asset No (C)'), headerTooltip: this.l('Asset No (C)'), field: 'assetNo', flex: 1 },
            { headerName: this.l('Sub Asset No (C)'), headerTooltip: this.l('Sub Asset No (C)'), field: 'subAssetNo', flex: 1 },
            { headerName: this.l('End Of Record (M)'), headerTooltip: this.l('End Of Record (M)'), field: 'endOfRecord', flex: 1 },
           
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void { }

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
        }, 150)
    }

    show(postingDateFrom?: any, postingDateTo?: any): void {
        this.rowData = [];
        this.postingDateFrom = postingDateFrom;
        this.postingDateTo = postingDateTo
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM05Validate(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.rowData = result.items;
            this.resetGridView();
            this.isLoading = false;
        });
        this.modal.show();
    }
    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    ok(){
        
    }
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvInterfaceFQF3MM05Validate(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
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
        this.dataParamsView = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM05VALIDATEToExcel(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo)
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
