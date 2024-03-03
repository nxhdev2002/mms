import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, BusinessDataDto, IF_FQF3MM03ServiceProxy, GetIF_FQF3MM01_VALIDATE, IF_FQF3MM01ServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-fqf3mm03-validate-result-modal.component.html',
    selector: 'view-fqf3mm03-validate-result-modal'
})
export class ViewFqf3mm03ValidateResultModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm03ValidateResult', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: GetIF_FQF3MM01_VALIDATE = new GetIF_FQF3MM01_VALIDATE();
    saveSelectedRow: GetIF_FQF3MM01_VALIDATE = new GetIF_FQF3MM01_VALIDATE();
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

    postingDate;
    constructor(injector: Injector,
        private _service: IF_FQF3MM03ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            { headerName: this.l('Record Id(M)'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Company Code(M)'), headerTooltip: this.l('Company Code'), field: 'companyCode', flex: 1 },
         
            { headerName: this.l('Document Type(M)'), headerTooltip: this.l('Document Type'), field: 'documentType', flex: 1 },
            { headerName: this.l('Document Date(M)'), headerTooltip: this.l('Document Date'), field: 'documentDate', flex: 1 },
          
            { headerName: this.l('Customer Plant Code(M)'), headerTooltip: this.l('Customer Plant Code'), field: 'customerPlantCode', flex: 1 },
            { headerName: this.l('Customer Dock Code(M)'), headerTooltip: this.l('Customer Dock Code'), field: 'customerDockCode', flex: 1 },
            {headerName: this.l('Part Category(M)'), headerTooltip: this.l('Part Category'), field: 'partCategory', flex: 1},
		
            {headerName: this.l('Order Type(M)'), headerTooltip: this.l('Order Type'), field: 'orderType', flex: 1 },
			{ headerName: this.l('Pds No(M)'), headerTooltip: this.l('Pds No'), field: 'pdsNo', flex: 1 },
            { headerName: this.l('Part Received Date(M)'), headerTooltip: this.l('Part Received Date'), field: 'partReceivedDate', flex: 1 },
            { headerName: this.l('Sequence Date(M)'), headerTooltip: this.l('Sequence Date'), field: 'sequenceDate', flex: 1 },
            { headerName: this.l('Sequence No(M)'), headerTooltip: this.l('Sequence No'), field: 'sequenceNo', flex: 1 },
            { headerName: this.l('Part No(M)'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
          
            { headerName: this.l('Posting Date(M)'), headerTooltip: this.l('Posting Date'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Supplier Code(M)'), headerTooltip: this.l('Supplier Code'), field: 'supplierCode', flex: 1 },
          
			{ headerName: this.l('Part Quantity(M)'), headerTooltip: this.l('Part Quantity'), field: 'partQuantity', flex: 1 },
            { headerName: this.l('Unit Buying Price(M)'), headerTooltip: this.l('Unit Buying Price'), field: 'unitBuyingPrice', flex: 1 },        
            {headerName: this.l('Price Status(M)'), headerTooltip: this.l('Price Status'), field: 'priceStatus', flex: 1},		
            { headerName: this.l('Vat Code(M)'), headerTooltip: this.l('Vat Code'), field: 'vatCode', flex: 1 },
			
			{ headerName: this.l('Valuation Type(M)'), headerTooltip: this.l('Valuation Type'), field: 'valuationType', flex: 1 },
         
            { headerName: this.l('Spt Code(M)'), headerTooltip: this.l('Spt Code'), field: 'sptCode', flex: 1 },
        
			{headerName: this.l('Ending Of Record(M)'), headerTooltip: this.l('Ending Of Record'), field: 'endingOfRecord', flex: 1},
            { headerName: this.l('Header FwgId'), headerTooltip: this.l('HeaderFwgId'), field: 'headerFwgId', type: 'rightAligned'},
            { headerName: this.l('Header Id'), headerTooltip: this.l('HeaderId'), field: 'headerId', type: 'rightAligned' },
            { headerName: this.l('Trailer Id'), headerTooltip: this.l('TrailerId'), field: 'trailerId' },
            
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

    show(postingDateFrom?: any, postingDate?: any): void {
        this.rowData = [];
        this.postingDateFrom = postingDateFrom;
        this.postingDate = postingDate;
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM03ValidateResult(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvInterfaceFQF3MM03ValidateResult(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
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

        this._service.getValidateResult_FQF3MM03ToExcel(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDate),
    
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
        
    }
}
