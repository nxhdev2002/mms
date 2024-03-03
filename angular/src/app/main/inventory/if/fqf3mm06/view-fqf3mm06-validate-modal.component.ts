import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import ceil from 'lodash-es/ceil';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { GetIF_FQF3MM06_VALIDATE, IF_FQF3MM06ServiceProxy } from '@shared/service-proxies/service-proxies';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './view-fqf3mm06-validate-modal.component.html',
    selector: 'view-fqf3mm06-validate-modal'
})
export class ViewFqf3mm06ValidateModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('ViewFqf3mm06Validate', { static: true }) modal: ModalDirective | undefined;
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
    selectedRow: GetIF_FQF3MM06_VALIDATE = new GetIF_FQF3MM06_VALIDATE();
    saveSelectedRow: GetIF_FQF3MM06_VALIDATE = new GetIF_FQF3MM06_VALIDATE();
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
    createDateFrom;
    createDateTo;

    constructor(injector: Injector,
        private _service: IF_FQF3MM06ServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.viewColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Error Description'), headerTooltip: this.l('ErrorDescription'), field: 'errorDescription' },
            { headerName: this.l('Record Id (M)'), headerTooltip: this.l('Record Id (M)'), field: 'recordId', flex: 1 },
            { headerName: this.l('Authorization Group (M)'), headerTooltip: this.l('Authorization Group (M)'), field: 'authorizationGroup', flex: 1 },
            { headerName: this.l('Material Code (M)'), headerTooltip: this.l('Material Code (M)'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Industry Sector (M)'), headerTooltip: this.l('Industry Sector (M)'), field: 'industrySector', flex: 1 },
            { headerName: this.l('Material Description (M)'), headerTooltip: this.l('Material Description (M)'), field: 'materialDescription', flex: 1 },
            { headerName: this.l('Base Unit Of Measure (M)'), headerTooltip: this.l('Base Unit Of Measure (M)'), field: 'baseUnitOfMeasure', flex: 1 },
            { headerName: this.l('Plant (M)'), headerTooltip: this.l('Plant (M)'), field: 'plant', flex: 1 },
            { headerName: this.l('Storage Location (M)'), headerTooltip: this.l('Storage Location (M)'), field: 'storageLocation', flex: 1 },
            { headerName: this.l('Procurement Type (M)'), headerTooltip: this.l('Procurement Type (M)'), field: 'procurementType', flex: 1 },
            { headerName: this.l('Repet Manufacturing (M)'), headerTooltip: this.l('Repet Manufacturing (M)'), field: 'repetManufacturing', flex: 1 },
            { headerName: this.l('Rem Profile (M)'), headerTooltip: this.l('Rem Profile (M)'), field: 'remProfile', flex: 1 },
            { headerName: this.l('Basic Data Text (R)'), headerTooltip: this.l('Basic Data Text (R)'), field: 'basicDataText', flex: 1 },
            { headerName: this.l('Price Unit (M)'), headerTooltip: this.l('Price Unit (M)'), field: 'priceUnit', flex: 1 },
            { headerName: this.l('Ending Of Record (M)'), headerTooltip: this.l('Ending Of Record (M)'), field: 'endingOfRecord', flex: 1 },
           
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

    show(createDateFrom?: any, createDateTo?: any): void {
        this.rowData = [];
        this.createDateFrom = createDateFrom;
        this.createDateTo = createDateTo
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.isLoading = true;
        this._service.getInvInterfaceFQF3MM06Validate(
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo),
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
        return this._service.getInvInterfaceFQF3MM06Validate(
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo),
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
        this._service.getFQF3MM06VALIDATEToExcel(
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo)
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
}
