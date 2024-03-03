import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, InvGpsIssuingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { ViewRequestCmmSelectedComponent } from './view-request-cmm-selected-modal';
import { ViewRequestBudgetCheckComponent } from './view-request-budget-check-modal';


@Component({
    templateUrl: './view-logging-response-budget-check-modal.component.html',
    selector: 'viewLoggingResponseBudgetCheck'
})
export class ViewLoggingResponseBudgetCheckComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewLoggingResponseBudgetCheck', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('viewRequestSelectModal', { static: true }) viewRequestSelectModal: ViewRequestBudgetCheckComponent | undefined;

    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: LoggingResponseDetailsOnlBudgetCheckDto = new LoggingResponseDetailsOnlBudgetCheckDto();
    saveSelectedRow: LoggingResponseDetailsOnlBudgetCheckDto = new LoggingResponseDetailsOnlBudgetCheckDto();
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
    documentNo;
    constructor(injector: Injector,
        private _service: InvGpsIssuingsServiceProxy
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Logging Id'), headerTooltip: this.l('Logging Id'), field: 'loggingId' },
            { headerName: this.l('Document No'), headerTooltip: this.l('Document No'), field: 'documentNo' },
            { headerName: this.l('Available Budget WBS Master Data'), headerTooltip: this.l('Available Budget WBS Master Data'), field: 'availableBudgetWBSMasterData' },
            { headerName: this.l('Available Budget Fiscal Year'), headerTooltip: this.l('Available Budget Fiscal Year'), field: 'availableBudgetFiscalYear' },
            { headerName: this.l('Available Budget Available Amount'), headerTooltip: this.l('Available Budget Available Amount'), field: 'availableBudgetAvailableAmount', type: 'rightAligned' },
            { headerName: this.l('Available Budget Message Type'), headerTooltip: this.l('Available Budget Message Type'), field: 'availableBudgetMessageType' },
            { headerName: this.l('Available Budget Message ID'), headerTooltip: this.l('Available Budget Message ID'), field: 'availableBudgetMessageID' },
            { headerName: this.l('Available Budget Message No'), headerTooltip: this.l('Available Budget Message No'), field: 'availableBudgetMessageNo' },
            { headerName: this.l('Available Budget Message'), headerTooltip: this.l('Available Budget Message'), field: 'availableBudgetMessage' },
            { headerName: this.l('Data Validation WBS Master Data'), headerTooltip: this.l('Data Validation WBS Master Data'), field: 'dataValidationWBSMasterData' },
            { headerName: this.l('Data Validation Cost Center'), headerTooltip: this.l('Data Validation Cost Center'), field: 'dataValidationCostCenter' },
            { headerName: this.l('Data Validation Fixed Asset No'), headerTooltip: this.l('Data Validation Fixed Asset No'), field: 'dataValidationFixedAssetNo' },
            { headerName: this.l('Data Validation Result'), headerTooltip: this.l('Data Validation Result'), field: 'dataValidationResult' },
            { headerName: this.l('Data Validation Message Type'), headerTooltip: this.l('Data Validation Message Type'), field: 'dataValidationMessageType' },
            { headerName: this.l('Data Validation Message ID'), headerTooltip: this.l('Data Validation Message ID'), field: 'dataValidationMessageID' },
            { headerName: this.l('Data Validation Message No'), headerTooltip: this.l('Data Validation Message No'), field: 'dataValidationMessageNo' },
            { headerName: this.l('Data Validation Message'), headerTooltip: this.l('Data Validation Message'), field: 'dataValidationMessage' }
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

    show(documentNo): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this.documentNo = documentNo;
        this._service.getViewLoggingResponseDetailsOnlBudgetCheckIssuing(
            this.documentNo,
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
        return this._service.getViewLoggingResponseDetailsOnlBudgetCheckIssuing(
            this.documentNo,
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

    onChangeRowSelection(params: { api: { getSelectedRows: () => LoggingResponseDetailsOnlBudgetCheckDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LoggingResponseDetailsOnlBudgetCheckDto();
        this.logId = this.saveSelectedRow.loggingId;

        this.selectedRow = Object.assign({}, this.saveSelectedRow);
      
    }

    showRequest(){
        console.log('check');
       this.viewRequestSelectModal.show(this.logId);
    }
    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
