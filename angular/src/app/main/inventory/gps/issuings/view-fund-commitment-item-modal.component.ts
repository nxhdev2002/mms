import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { GetIF_FundCommitmentItemDMExportDto, IF_FQF3MM07ServiceProxy, InvCkdContainerListServiceProxy, InvGpsIssuingsServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import ceil from 'lodash-es/ceil';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs';
import { ViewRequestCmmSelectedComponent } from './view-request-cmm-selected-modal';

@Component({
    templateUrl: './view-fund-commitment-item-modal.component.html',
    selector: 'view-fund-commitment-item'
})
export class ViewFundCommmitmentItemDMComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewFundCommitmentItem', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('viewRequestSelectModal', { static: true }) viewRequestSelectModal: ViewRequestCmmSelectedComponent | undefined;
    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: GetIF_FundCommitmentItemDMExportDto = new GetIF_FundCommitmentItemDMExportDto();
    saveSelectedRow: GetIF_FundCommitmentItemDMExportDto = new GetIF_FundCommitmentItemDMExportDto();
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    idFund:number;
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
    logId;
    constructor( injector: Injector,
        private _service: InvGpsIssuingsServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);

        this.viewColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('FundCommitmentHeaderType'),headerTooltip: this.l('FundCommitmentHeaderType'),field: 'fundCommitmentHeaderType'},
            {headerName: this.l('FundCommitmentHeaderId'),headerTooltip: this.l('FundCommitmentHeaderId'),field: 'fundCommitmentHeaderId'},
            {headerName: this.l('FundCommitmentItemId'),headerTooltip: this.l('FundCommitmentItemId'),field: 'fundCommitmentItemId'},
            {headerName: this.l('DocumentNo'),headerTooltip: this.l('DocumentNo'),field: 'documentNo'},
            {headerName: this.l('LineNo'),headerTooltip: this.l('LineNo'),field: 'lineNo'},
            {headerName: this.l('Closed'),headerTooltip: this.l('Closed'),field: 'closed'},
            {headerName: this.l('ReferenceDocumentNo'),headerTooltip: this.l('ReferenceDocumentNo'),field: 'referenceDocumentNo'},
			{headerName: this.l('ReferenceDocumentLineItemNo'),headerTooltip: this.l('ReferenceDocumentLineItemNo'),field: 'referenceDocumentLineItemNo',type: 'rightAligned'},
            {headerName: this.l('ItemCode'),headerTooltip: this.l('ItemCode'),field: 'itemCode'},
            {headerName: this.l('ItemDescription'),headerTooltip: this.l('ItemDescription'),field: 'itemDescription'},
            {headerName: this.l('PartCategory'),headerTooltip: this.l('PartCategory'),field: 'partCategory'},
            {headerName: this.l('MaterialType'),headerTooltip: this.l('MaterialType'),field: 'materialType'},

            {headerName: this.l('SupplierCode'),headerTooltip: this.l('SupplierCode'),field: 'supplierCode'},
            {headerName: this.l('Asset'),headerTooltip: this.l('Asset'),field: 'asset'},
            {headerName: this.l('WbsElement'),headerTooltip: this.l('WbsElement'),field: 'wbsElement'},
			{headerName: this.l('CostCenterCharger'),headerTooltip: this.l('CostCenterCharger'),field: 'costCenterCharger'},
            {headerName: this.l('TotalAmount'),headerTooltip: this.l('TotalAmount'),field: 'totalAmount'},
            {headerName: this.l('Quantity'),headerTooltip: this.l('Quantity'),field: 'quantity'},
            {headerName: this.l('Uom'),headerTooltip: this.l('Uom'),field: 'uom'},
            {headerName: this.l('JournalSource'),headerTooltip: this.l('JournalSource'),field: 'journalSource'},

            {headerName: this.l('GlAccount'),headerTooltip: this.l('GlAccount'),field: 'glAccount'},
            {
                headerName: this.l('SubmitDate'),
                headerTooltip: this.l('SubmitDate'),
                flex: 1,
                field: 'submitDate',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
            },
            {headerName: this.l('Action'),headerTooltip: this.l('Action'),field: 'action'},
			{headerName: this.l('MarkAsSapTransfer'),headerTooltip: this.l('MarkAsSapTransfer'),field: 'markAsSapTransfer'},
            {headerName: this.l('LatestSapSuccessTransferDate'),headerTooltip: this.l('LatestSapSuccessTransferDate'),field: 'latestSapSuccessTransferDate'},
            {headerName: this.l('LatestSapTransferUserId'),headerTooltip: this.l('LatestSapTransferUserId'),field: 'latestSapTransferUserId'},
            {
                headerName: this.l('LatestSapTransferDate'),
                headerTooltip: this.l('LatestSapTransferDate'),
                flex: 1,
                field: 'latestSapTransferDate',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
            },
            {headerName: this.l('LatestSapTransferMessage'),headerTooltip: this.l('LatestSapTransferMessage'),field: 'latestSapTransferMessage'},

            {headerName: this.l('MarkAsLegacyTransfer'),headerTooltip: this.l('MarkAsLegacyTransfer'),field: 'markAsLegacyTransfer'},
            {
                headerName: this.l('LatestLegacySuccessTransferDate'),
                headerTooltip: this.l('LatestLegacySuccessTransferDate'),
                flex: 1,
                field: 'latestLegacySuccessTransferDate',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
            },
			{headerName: this.l('LatestLegacyTransferUserId'),headerTooltip: this.l('LatestLegacyTransferUserId'),field: 'latestLegacyTransferUserId'},
            {headerName: this.l('LatestLegacyTransferDate'),headerTooltip: this.l('LatestLegacyTransferDate'),field: 'latestLegacyTransferDate'
            ,valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),},
            {headerName: this.l('LatestLegacyTransferMessage'),headerTooltip: this.l('LatestLegacyTransferMessage'),field: 'latestLegacyTransferMessage'},

            {headerName: this.l('EarmarkedFundsDocument'),headerTooltip: this.l('EarmarkedFundsDocument'),field: 'earmarkedFundsDocument'},
            {headerName: this.l('EarmarkedFundsDocumentItem'),headerTooltip: this.l('EarmarkedFundsDocumentItem'),field: 'earmarkedFundsDocumentItem'},
            {headerName: this.l('FundCommitmentHeaderNo'),headerTooltip: this.l('FundCommitmentHeaderNo'),field: 'fundCommitmentHeaderNo'},
            {headerName: this.l('BudgetCodeOld'),headerTooltip: this.l('BudgetCodeOld'),field: 'budgetCodeOld'},
            {headerName: this.l('CostCenterOld'),headerTooltip: this.l('CostCenterOld'),field: 'costCenterOld'},
            {headerName: this.l('GlAccountOld'),headerTooltip: this.l('GlAccountOld'),field: 'glAccountOld'},

            {headerName: this.l('MessageType'),headerTooltip: this.l('MessageType'),field: 'messageType'},
            {headerName: this.l('MessageID'),headerTooltip: this.l('MessageID'),field: 'messageID'},
            {headerName: this.l('MessageNo'),headerTooltip: this.l('MessageNo'),field: 'messageNo'},
            {headerName: this.l('Message'),headerTooltip: this.l('Message'),field: 'message'},
            {headerName: this.l('LoggingId'),headerTooltip: this.l('LoggingId'),field: 'loggingId'},

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

    show(idFund): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this.idFund = idFund;
        this._service.getViewFundCommmitmentItemDMIssuing(
            idFund,
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

    // searchDatas(): void {
    //     this.isLoading = true;
    //     this._service.getViewFundCommmitmentItemDM(
    //         this.documentNo,
    //         this._dateTimeService.convertToDatetime(this.documentDateFrom),
    //         this._dateTimeService.convertToDatetime(this.documentDateTo),
    //         this._dateTimeService.convertToDatetime(this.postingDateFrom),
    //         this._dateTimeService.convertToDatetime(this.postingDateTo),
    //         '',
    //         this.paginationParams.skipCount,
    //         this.paginationParams.pageSize
    //     )
    //         .pipe(finalize(() =>{}))
    //         .subscribe((result) => {
    //             this.paginationParams.totalCount = result.totalCount;
    //             this.rowData = result.items;
    //             this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
    //             this.resetGridView();
    //             this.isLoading = false;
    //         });
    // }
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getViewFundCommmitmentItemDMIssuing(
            this.idFund,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
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
            .pipe(finalize(() => {}))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }
    onChangeRowSelection(params: { api: { getSelectedRows: () => GetIF_FundCommitmentItemDMExportDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new GetIF_FundCommitmentItemDMExportDto();
        this.logId = this.saveSelectedRow.loggingId;
        this.idFund = this.saveSelectedRow.id;
        this.selectedRow = Object.assign({}, this.saveSelectedRow);


    }

    showRequest(){
        console.log('func');
        
        this.viewRequestSelectModal.show(this.logId);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
