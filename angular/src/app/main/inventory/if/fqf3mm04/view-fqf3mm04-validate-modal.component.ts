import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, BusinessDataDto, IF_FQF3MM03ServiceProxy, GetIF_FQF3MM01_VALIDATE, IF_FQF3MM01ServiceProxy, GetIF_FQF3MM04_VALIDATE, IF_FQF3MM04ServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-fqf3mm04-validate-modal.component.html',
    selector: 'view-fqf3mm04-validate-modal'
})
export class ViewFqf3mm04ValidateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm04Validate', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: GetIF_FQF3MM04_VALIDATE = new GetIF_FQF3MM04_VALIDATE();
    saveSelectedRow: GetIF_FQF3MM04_VALIDATE = new GetIF_FQF3MM04_VALIDATE();
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
    devaningDateFrom : any;
    devaningDateTo : any;
    constructor(injector: Injector,
        private _service: IF_FQF3MM04ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            {headerName: this.l('Record Id (M)'),headerTooltip: this.l('Record Id (M)'),field: 'recordId',flex: 1},
			{headerName: this.l('Invoice No (M)'),headerTooltip: this.l('Invoice No (M)'),field: 'invoiceNo',flex: 1},
			{headerName: this.l('Renban (M)'),headerTooltip: this.l('Renban (M)'),field: 'renban',flex: 1},
			{headerName: this.l('Container No (M)'),headerTooltip: this.l('Container No (M)'),field: 'containerNo',flex: 1},
			//{headerName: this.l('Module No (O)'),headerTooltip: this.l('Module No (O)'),field: 'moduleNo',flex: 1},
			{headerName: this.l('Devaning Date (M)'),headerTooltip: this.l('Devaning Date (M)'),field: 'devaningDate',flex: 1,valueFormatter: (params) => this.pipe.transform(params.data?.devaningDate, 'dd/MM/yyyy')},
			{headerName: this.l('Plant (M)'),headerTooltip: this.l('Plant (M)'),field: 'plant',flex: 1},
			{headerName: this.l('Cancel Flag (M)'),headerTooltip: this.l('Cancel Flag (M)'),field:  'cancelFlag',flex: 1},
			{headerName: this.l('End Of Record (M)'),headerTooltip: this.l('End Of Record (M)'),field:  'endOfRecord',flex: 1},
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

    show(devaningDateFrom?: any,devaningDateTo?: any): void {
        this.rowData = [];
        this.devaningDateFrom = devaningDateFrom;
        this.devaningDateTo = devaningDateTo;
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM04Validate(
            this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo),
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
        return this._service.getInvInterfaceFQF3MM04Validate(
            this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo),
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

        this._service.getValidate_FQF3MM04ToExcel(
            this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo)
    
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
        
    }
}
