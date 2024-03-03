import { Component, ElementRef, EventEmitter, HostListener, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileDto, ImportDevanDto, InvCkdContainerListServiceProxy } from '@shared/service-proxies/service-proxies';
import ceil from 'lodash-es/ceil';

@Component({
    templateUrl: './view-importdevan.component.html',
    selector: 'view-importdevan'
})
export class ViewImportDevanComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewImportDevanComponent', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();


    @Input() rowDataImport: any;
    importdevanColDefs: CustomColDef[] = [];
    paginationParamsViewImport: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    pending;
    disable;
    loading;
    _isActive: boolean = false;
    frameworkComponents: FrameworkComponent;
    saving: boolean = false;
    isActive: boolean;
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    pipe = new DatePipe('en-US');
    active = false;
    containerNo: string = '';
    renban: string = '';
    supplierNo: string = '';
    haisenNo: string = '';
    billOfLadingNo: string = '';
    invoiceNo: string = '';
    ordertypeCode: string = '';
    goodstypeCode: string = '';
    portDateFrom;
    receiveDate;
    lotNo;
    portDateTo;
    receiveDateFrom;
    receiveDateTo;
    rowData: any[] = [];
    dau: any[]
    billDateFrom: any;
    billDateTo: any;
    radio: string = '';
    ckdPio: string = '';
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
        private _service: InvCkdContainerListServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);

        this.importdevanColDefs = [
            { headerName: this.l('Record Id'), headerTooltip: this.l('Record Id'),field: 'noNumber', cellRenderer: (params) => (params.data.noNumber).toString().padStart(7, '0'), cellClass: ['text-center'], width: 90 },
            { headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo' },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban' },
            { headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo' },
            { headerName: this.l(' Module No'), headerTooltip: this.l('Module No'), field: 'moduleCaseNo' },
            { headerName: this.l('Devan Date'), headerTooltip: this.l('Devan Date'), field: 'devanningDateS4' },
            { headerName: this.l(' Plant'), headerTooltip: this.l('Plant'), field: 'plant' },
            { headerName: this.l('Cancel Flag'), headerTooltip: this.l('CancelFlag'), field: 'cancelFlag' },

        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {

        // if (this.rowDataImport == undefined) {
        //     this.rowDataImport = {
        //         containerNo: this.containerNo = '',
        //         renban: this.renban = '',
        //         supplierNo: this.supplierNo = '',
        //         haisenNo: this.haisenNo = '',
        //         billOfLadingNo: this.billOfLadingNo = '',
        //         invoiceNo: this.invoiceNo = '',
        //         ordertypeCode: this.ordertypeCode = '',
        //         portDateFrom: this.portDateFrom = '',
        //         receiveDateFrom: this.receiveDateFrom = '',
        //         receiveDateTo: this.receiveDateTo = '',
        //         goodstypeCode: this.goodstypeCode = '',
        //         radio: this.radio = '',
        //     }
        // }
    }

    show(containerNo, renban, supplierNo,
        haisenNo, billOfLadingNo, invoiceNo,
        ordertypeCode, portDateFrom, portDateTo, receiveDateFrom, receiveDateTo,
        goodstypeCode, radio,
        billDateFrom, billDateTo, ckdPio

    ): void {
        this.rowData = [];
        this.loading = true;

        this.containerNo = containerNo;
        this.renban = renban;
        this.supplierNo = supplierNo;
        this.haisenNo = haisenNo;
        this.billOfLadingNo = billOfLadingNo;
        this.invoiceNo = invoiceNo;
        this.ordertypeCode = ordertypeCode;
        this.portDateFrom = portDateFrom;
        this.portDateTo = portDateTo;
        this.receiveDateFrom = receiveDateFrom;
        this.receiveDateTo = receiveDateTo;
        this.goodstypeCode = goodstypeCode;
        this.radio = radio;
        this.billDateFrom = billDateFrom;
        this.billDateTo = billDateTo;
        this.ckdPio = ckdPio;

        this.paginationParamsViewImport = { pageNum: 1, pageSize: 500, totalCount: 0 , skipCount:0};

        this._service.getImportDevan(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
            '',
            this.paginationParamsViewImport.skipCount,
            this.paginationParamsViewImport.pageSize
        ).subscribe(result => {

            this.paginationParamsViewImport.totalCount = result.totalCount;
            this.paginationParamsViewImport.totalPage = ceil(result.totalCount / (this.paginationParamsViewImport.pageSize ?? 0));

            this.rowData = result.items ?? [];
            this.loading = false;

        });
        this.modal.show();
    }

    // getData(paginationParams?: PaginationParamsModel) {
    //     return this._service.getAll(
    //         this.containerNo,
    //         this.renban,
    //         this.supplierNo,
    //         this.haisenNo,
    //         this.billOfLadingNo,
    //         this.invoiceNo,
    //         this.ordertypeCode,
    //         this._dateTimeService.convertToDatetime(this.portDateFrom),
    //         this._dateTimeService.convertToDatetime(this.portDateTo),
    //         this._dateTimeService.convertToDatetime(this.receiveDateFrom),
    //         this._dateTimeService.convertToDatetime(this.receiveDateTo),
    //         this.goodstypeCode,
    //         this.radio,
    //         this._dateTimeService.convertToDatetime(this.billDateFrom),
    //         this._dateTimeService.convertToDatetime(this.billDateTo),
    //         '',
    //         this.paginationParamsViewImport.skipCount,
    //         this.paginationParamsViewImport.pageSize
    //     );
    // }


    // callBackDataGrid(params: GridParams) {
    //     this.isLoading = true;
    //     this.dataParams = params;
    //     params.api.paginationSetPageSize(this.paginationParamsViewImport.pageSize);
    //     this.paginationParamsViewImport.skipCount =
    //         ((this.paginationParamsViewImport.pageNum ?? 1) - 1) * (this.paginationParamsViewImport.pageSize ?? 0);
    //     this.paginationParamsViewImport.pageSize = this.paginationParamsViewImport.pageSize;
    //     this.getData(this.paginationParamsViewImport);
    //     this.isLoading = false;
    // }


    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }
    changeActive(event) {
        this._isActive = event.target.checked;
    }

    exportDevanToExcel(): void {
        this.pending = 'pending'
        this.disable = true
        this._service.getImPortDeVanExcel(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this.ckdPio
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = ''
                this.disable = false
            });
    }

    changePage(paginationParams) {
        this.loading = true;
        this.paginationParamsViewImport.skipCount =
            ((this.paginationParamsViewImport.pageNum ?? 1) - 1) * (this.paginationParamsViewImport.pageSize ?? 0);
        this._service.getImportDevan(
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.haisenNo,
            this.billOfLadingNo,
            this.invoiceNo,
            this.ordertypeCode,
            this._dateTimeService.convertToDatetime(this.portDateFrom),
            this._dateTimeService.convertToDatetime(this.portDateTo),
            this._dateTimeService.convertToDatetime(this.receiveDateFrom),
            this._dateTimeService.convertToDatetime(this.receiveDateTo),
            this.goodstypeCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.lotNo,
            '',
            this.paginationParamsViewImport.skipCount,
            this.paginationParamsViewImport.pageSize
        ).subscribe(result => {

            this.paginationParamsViewImport.totalCount = result.totalCount;
            this.paginationParamsViewImport.totalPage = ceil(result.totalCount / (this.paginationParamsViewImport.pageSize ?? 0));

            this.rowData = result.items ?? [];
            this.loading = false;

        });
    }


    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
