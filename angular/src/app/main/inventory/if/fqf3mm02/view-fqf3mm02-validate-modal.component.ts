import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, BusinessDataDto, IF_FQF3MM03ServiceProxy, GetIF_FQF3MM02_VALIDATE, IF_FQF3MM02ServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-fqf3mm02-validate-modal.component.html',
    selector: 'view-fqf3mm02-validate-modal'
})
export class ViewFqf3mm02ValidateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm02Validate', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: GetIF_FQF3MM02_VALIDATE = new GetIF_FQF3MM02_VALIDATE();
    saveSelectedRow: GetIF_FQF3MM02_VALIDATE = new GetIF_FQF3MM02_VALIDATE();
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    postingDateTo: any;
    postingDateFrom: any;

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
    lineOffDatetime;
    constructor(injector: Injector,
        private _service: IF_FQF3MM02ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            { headerName: this.l('Record Id'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Company Code'), headerTooltip: this.l('Company Code'), field: 'companyCode', flex: 1 },
            { headerName: this.l('Plant Code'), headerTooltip: this.l('Plant Code'), field: 'plantCode', flex: 1 },
            { headerName: this.l('Maru Code'), headerTooltip: this.l('Maru Code'), field: 'maruCode', flex: 1 },
            { headerName: this.l('Posting Date'), headerTooltip: this.l('Posting Date'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Part Code'), headerTooltip: this.l('Part Code'), field: 'partCode', flex: 1 },
            { headerName: this.l('Quantity'), headerTooltip: this.l('Quantity'), field: 'quantity', flex: 1, type: 'rightAligned'},
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Normal Cancel Flag'), headerTooltip: this.l('Normal Cancel Flag'), field: 'normalCancelFlag', flex: 1 },
            { headerName: this.l('Grgi No'), headerTooltip: this.l('Grgi No'), field: 'grgiNo', flex: 1 },
            { headerName: this.l('Material Doc Type'), headerTooltip: this.l('Material Doc Type'), field: 'materialDocType', flex: 1 },
            { headerName: this.l('Related Part Receive No'), headerTooltip: this.l('Related Part Receive No'), field: 'relatedPartReceiveNo', flex: 1 },
            { headerName: this.l('Related Gr Type'), headerTooltip: this.l('Related Gr Type'), field: 'relatedGrType', flex: 1 },
            { headerName: this.l('Related Gr Transaction Type'), headerTooltip: this.l('Related Gr Transaction Type'), field: 'relatedGrTransactionType', flex: 1 },
            { headerName: this.l('In-House Part Quantity Receive'), headerTooltip: this.l('In-House Part Quantity Receive'), field: 'inHousePartQuantityReceive', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Final Price'), headerTooltip: this.l('Final Price'), field: 'finalPrice', flex: 1, type: 'rightAligned' },
            { headerName: this.l('P-Sms Code'), headerTooltip: this.l('P-Sms Code'), field: 'psmsCode', flex: 1 },
            { headerName: this.l('Gi Uom'), headerTooltip: this.l('Gi Uom'), field: 'giUom', flex: 1 },
            { headerName: this.l('Ending Of Record'), headerTooltip: this.l('Ending Of Record'), field: 'endingOfRecord', flex: 1 },
          
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

    show(postingDateFrom?: any,postingDateTo?: any): void {
        this.rowData = [];
        this.postingDateFrom = postingDateFrom;
        this.postingDateTo = postingDateTo;
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM02Validate(
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvInterfaceFQF3MM02Validate(
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

        this._service.getValidate_FQF3MM02ToExcel(
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
