import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockIssuingServiceProxy, InvCkdStockIssuingValidateDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ceil } from 'lodash-es';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { finalize } from 'rxjs';
import { GridTableService } from '@app/shared/common/services/grid-table.service';

@Component({
    selector: 'validate-issuing-modal',
    templateUrl: './validate-issuing-modal.component.html',
})
export class ValidateIssuingModalComponent extends AppComponentBase {
    @ViewChild('validateIssuing', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowDataValidate: InvCkdStockIssuingValidateDto = new InvCkdStockIssuingValidateDto();
    saveSelectedRow: InvCkdStockIssuingValidateDto = new InvCkdStockIssuingValidateDto();
    rowData: any[] = [];
    validateColDefs: CustomColDef[] = [];
    _isActive: boolean = false;
    saving: boolean = false;
    isActive: boolean;
    partNo;
    supplierNo;
    colorSfx : string = '' ;
    cfc;
    lotNo;
    noInLot;
    vinNo;
    messagesError;
    dataParams: GridParams | undefined;
    isLoading: boolean = false;;
    pending;
    disable;
    _totalcount: number;
    body;
    workingDateFrom: any;
    workingDateTo: any;
    selectedIdMaterial; 


    paginationParam: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    frameworkComponents: FrameworkComponent;
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
        private _service: InvCkdStockIssuingServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private gridTableService: GridTableService,
        private _dateTimeService : DateTimeService,
       ) {
        super(injector);

        this.validateColDefs = [
            {
                headerName: this.l('STT'), headerTooltip: this.l('STT'),
                cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParam.pageSize * (this.paginationParam.pageNum - 1),
                cellClass: ['text-center'],
                width: 55
            },
            {
                headerName: this.l('Error Description'),
                headerTooltip: this.l('Error Description'),
                field: 'messagesError',
                flex: 1
            },
            {
                headerName: this.l('PartNo'),
                headerTooltip: this.l('PartNo'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                flex: 1
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                flex: 1
            },
            {
                headerName: this.l('Vin No'),
                headerTooltip: this.l('Vin No'),
                field: 'vinNo',
                flex: 1
            },
            


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParam = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    show(workingDateFrom,workingDateTo): void {
        this.workingDateFrom = workingDateFrom;
        this.workingDateTo = workingDateTo;
        this.getDataValidate();
        this.modal.show();
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    getDataValidate() {
        this.isLoading = true;
        this._service.getValidateStockIssuing(
        this._dateTimeService.convertToDatetime(this.workingDateFrom),
        this._dateTimeService.convertToDatetime(this.workingDateTo),
        '',
        this.paginationParam.skipCount,
        this.paginationParam.pageSize)
        .subscribe(result => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize));
            this.isLoading = false;
            // this.resetGridView();
        })
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return  this._service.getValidateStockIssuing(
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            '',
            this.paginationParam.skipCount,
            this.paginationParam.pageSize
            );
    }

    exportValidateToExcel(): void {
        this.pending = 'pending'
        this.disable = true
        this._service.excelCheckValidateStockIssuing(
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = ''
                this.disable = false
            });
    }

    callBackGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParam.pageSize);
        this.paginationParam.skipCount =
            ((this.paginationParam.pageNum ?? 1) - 1) * (this.paginationParam.pageSize ?? 0);
        this.paginationParam.pageSize = this.paginationParam.pageSize;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParam = paginationParams;
        this.paginationParam.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParam).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize ?? 0));
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


