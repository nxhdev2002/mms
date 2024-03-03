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
    templateUrl: './view-fqf3mm01-validate-result-modal.component.html',
    selector: 'view-fqf3mm01-validate-result-modal'
})
export class ViewFqf3mm01ValidateResultModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm01ValidateResult', { static: true }) modal: ModalDirective | undefined;
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
    lineOffDatetimeFrom;
    lineOffDatetimeTo;
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
        private _service: IF_FQF3MM01ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            { headerName: this.l('Record Id(M)'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Vin(M)'), headerTooltip: this.l('Vin'), field: 'vin', flex: 1 },
            { headerName: this.l('Spec Sheet No(M)'), headerTooltip: this.l('Spec Sheet No'), field: 'specSheetNo', flex: 1 },
            { headerName: this.l('Id Line(M)'), headerTooltip: this.l('Id Line'), field: 'idLine', flex: 1 },
            { headerName: this.l('Katashiki(M)'), headerTooltip: this.l('Katashiki'), field: 'katashiki', flex: 1 },
            { headerName: this.l('Sale Katashiki(M)'), headerTooltip: this.l('Sale Katashiki'), field: 'saleKatashiki', flex: 1 },
            { headerName: this.l('Sale Suffix(M)'), headerTooltip: this.l('Sale Suffix'), field: 'saleSuffix', flex: 1 },
            { headerName: this.l('Spec 200 Digits(M)'), headerTooltip: this.l('Spec 200 Digits'), field: 'spec200Digits', flex: 1 },
            { headerName: this.l('Production Suffix(M)'), headerTooltip: this.l('Production Suffix'), field: 'productionSuffix', flex: 1 },
            { headerName: this.l('Engine Prefix(M)'), headerTooltip: this.l('Engine Prefix'), field: 'enginePrefix', flex: 1 },
            { headerName: this.l('Engine No(M)'), headerTooltip: this.l('Engine No'), field: 'engineNo', flex: 1 },
            { headerName: this.l('Plant Code(M)'), headerTooltip: this.l('Plant Code'), field: 'plantCode', flex: 1 },
            { headerName: this.l('Current Status(M)'), headerTooltip: this.l('Current Status'), field: 'currentStatus', flex: 1 },
            { headerName: this.l('Line Off Datetime(M)'), headerTooltip: this.l('Line Off Datetime'), field: 'lineOffDatetime', flex: 1 },
            { headerName: this.l('Interior Color(M)'), headerTooltip: this.l('Interior Color'), field: 'interiorColor', flex: 1 },
            { headerName: this.l('Exterior Color(M)'), headerTooltip: this.l('Exterior Color'), field: 'exteriorColor', flex: 1 },
            { headerName: this.l('Destination Code(M)'), headerTooltip: this.l('Destination Code'), field: 'destinationCode', flex: 1 },
            { headerName: this.l('Ed Odno(M)'), headerTooltip: this.l('Ed Odno'), field: 'edOdno', flex: 1 },
            {headerName: this.l('Cancel Flag(M)'), headerTooltip: this.l('Cancel Flag'), field: 'cancelFlag', cellClass: ['text-center'], width: 120, flex: 1,},
			{ headerName: this.l('Sms Car Family Code(M)'), headerTooltip: this.l('Sms Car Family Code'), field: 'smsCarFamilyCode', flex: 1 },
            {headerName: this.l('Order Type(M)'), headerTooltip: this.l('Order Type'), field: 'orderType', flex: 1},
            {headerName: this.l('End Of Record(M)'), headerTooltip: this.l('End Of Record'), field: 'endOfRecord', flex: 1},
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

    show(lineOffDatetimeFrom?: any, lineOffDatetimeTo?: any): void {

        this.rowData = [];
        this.lineOffDatetimeFrom = lineOffDatetimeFrom;
        this.lineOffDatetimeTo = lineOffDatetimeTo;
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM01ValidateResult(
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
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
        return this._service.getInvInterfaceFQF3MM01ValidateResult(
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
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

        this._service.getValidateResult_FQF3MM01ToExcel(
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
        
    }
}
