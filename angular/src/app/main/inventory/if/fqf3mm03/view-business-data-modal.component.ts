import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LoggingResponseDetailsOnlBudgetCheckDto, IF_FQF3MM07ServiceProxy, BusinessDataDto, IF_FQF3MM03ServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './view-business-data-modal.component.html',
    selector: 'viewBusinessData'
})
export class ViewBusinessDataComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewLoggingResponseBudgetCheck', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: BusinessDataDto = new BusinessDataDto();
    saveSelectedRow: BusinessDataDto = new BusinessDataDto();
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    idFund: number;
    logId;
    partNo;
    supplierCode;
    workingDateFrom;
    workingDateTo;

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
        private _service: IF_FQF3MM03ServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('RowNo'), headerTooltip: this.l('RowNo'), field: 'rowNo', type: 'rightAligned' },
            { headerName: this.l('ItemNo'), headerTooltip: this.l('ItemNo'), field: 'itemNo', type: 'rightAligned' },
            { headerName: this.l('OrderMonth'), headerTooltip: this.l('OrderMonth'), field: 'orderMonth',valueFormatter: (params) => this.pipe.transform(params.data?.orderMonth, 'dd/MM/yyyy') },
            { headerName: this.l('OrderWkDate'), headerTooltip: this.l('OrderWkDate'), field: 'orderWkDate', valueFormatter: (params) => this.pipe.transform(params.data?.orderWkDate, 'dd/MM/yyyy') },
            { headerName: this.l('ActArrivalWkDate'), headerTooltip: this.l('ActArrivalWkDate'), field: 'actArrivalWkDate',valueFormatter: (params) => this.pipe.transform(params.data?.actArrivalWkDate, 'dd/MM/yyyy') },
            { headerName: this.l('ActArrivalDateTime'), headerTooltip: this.l('ActArrivalDateTime'), field: 'actArrivalDateTime',valueFormatter: (params) => this.pipe.transform(params.data?.actArrivalDateTime, 'dd/MM/yyyy HH:mm') },
            { headerName: this.l('SupplierCode'), headerTooltip: this.l('SupplierCode'), field: 'supplierCode' },
            { headerName: this.l('SptCode'), headerTooltip: this.l('SptCode'), field: 'sptCode' },
            { headerName: this.l('SupplierAbbr'), headerTooltip: this.l('SupplierAbbr'), field: 'supplierAbbr' },
            { headerName: this.l('OrderNo'), headerTooltip: this.l('OrderNo'), field: 'orderNo' },
            { headerName: this.l('ContentNo'), headerTooltip: this.l('ContentNo'), field: 'contentNo' },
            { headerName: this.l('PartNo'), headerTooltip: this.l('PartNo'), field: 'partNo' },
            { headerName: this.l('PartName'), headerTooltip: this.l('PartName'), field: 'partName' },
            { headerName: this.l('Unit'), headerTooltip: this.l('Unit'), field: 'unit' },
            { headerName: this.l('UsageOrderQty'), headerTooltip: this.l('UsageOrderQty'), field: 'usageOrderQty', type: 'rightAligned' },
            { headerName: this.l('UsageActualQty'), headerTooltip: this.l('UsageActualQty'), field: 'usageActualQty', type: 'rightAligned' },
            { headerName: this.l('ActualQty'), headerTooltip: this.l('ActualQty'), field: 'actualQty', type: 'rightAligned' },
            { headerName: this.l('BoxSize'), headerTooltip: this.l('BoxSize'), field: 'boxSize', type: 'rightAligned' },
            { headerName: this.l('PackagingType'), headerTooltip: this.l('PackagingType'), field: 'packagingType' },
            { headerName: this.l('Shift'), headerTooltip: this.l('Shift'), field: 'shift' },
            { headerName: this.l('UlScanUserId'), headerTooltip: this.l('UlScanUserId'), field: 'ulScanUserId', type: 'rightAligned' },
            { headerName: this.l('IsActive'), headerTooltip: this.l('IsActive'), field: 'isActive' },
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

    show(workingDateFrom,workingDateTo): void {
        this.workingDateFrom = workingDateFrom;
        this.workingDateTo = workingDateTo;
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getViewBusinessData(
            this.partNo,
            this.supplierCode,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),      
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
        return this._service.getViewBusinessData(
            this.partNo,
            this.supplierCode,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
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
}
