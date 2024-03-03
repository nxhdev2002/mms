import { ViewIF } from './../../../../../shared/service-proxies/service-proxies';
import { Component, ElementRef, EventEmitter, HostListener, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ceil } from 'lodash-es';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { FileDto, ImportDevanDto, InvCkdVehhicleServiceProxy } from '@shared/service-proxies/service-proxies';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './view-interface.component.html',
    selector: 'view-interface'
})
export class ViewInterfaceComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewInterfaceComponent', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();


    interfaceColDefs: CustomColDef[] = [];
    paginationParams1: PaginationParamsModel = {
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
    isLoading1: boolean = false;
    dataParamsIF: GridParams | undefined;
    pipe = new DatePipe('en-US');
    active = false;
    rowData: any[] = [];
    _totalcount: number;
    _fromdate;
    dateFrom;
    fn: CommonFunction = new CommonFunction();
    vinNo: string = '';
    dateTo;
    _todate;
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
        private _service: InvCkdVehhicleServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.interfaceColDefs = [
            { headerName: this.l('Record Id'), headerTooltip: this.l('Record Id'),  width: 90, field: 'recordId'},
            { headerName: this.l('Vin'), headerTooltip: this.l('Vin'), field: 'vin' },
            { headerName: this.l('Urn'), headerTooltip: this.l('Urn'), field: 'urn' },
            { headerName: this.l('Spec SheetNo'), headerTooltip: this.l('SpecSheetNo'), field: 'specSheetNo' },
            { headerName: this.l('Id Line'), headerTooltip: this.l('IdLine'), field: 'idLine' },
            { headerName: this.l('Katashiki'), headerTooltip: this.l('Katashiki'), field: 'katashiki' },
            { headerName: this.l('Sale Katashiki'), headerTooltip: this.l('SaleKatashiki'), field: 'saleKatashiki' },
            { headerName: this.l('Sale Suffix'), headerTooltip: this.l('SaleSuffix'), field: 'saleSuffix' },
            { headerName: this.l('Spec200 Digits'), headerTooltip: this.l('Spec200Digits'), field: 'spec200Digits', },
            { headerName: this.l('Production Suffix'), headerTooltip: this.l('ProductionSuffix'), field: 'productionSuffix' },
            { headerName: this.l('Lot Code'), headerTooltip: this.l('LotCode'), field: 'lotCode' },
            { headerName: this.l('Engine Prefix'), headerTooltip: this.l('EnginePrefix'), field: 'enginePrefix' },
            { headerName: this.l('EngineNo'), headerTooltip: this.l('EngineNo'), field: 'engineNo' },
            { headerName: this.l('Plant Code'), headerTooltip: this.l('PlantCode'), field: 'plantCode' },
            { headerName: this.l('Current Status'), headerTooltip: this.l('CurrentStatus'), field: 'currentStatus' },
            { headerName: this.l('LineOffDatetime'), headerTooltip: this.l('LineOffDatetime'), field: 'lineOffDatetime' },
            { headerName: this.l('Interior Color'), headerTooltip: this.l('InteriorColor'), field: 'interiorColor' },
            { headerName: this.l('Exterior Color'), headerTooltip: this.l('ExteriorColor'), field: 'exteriorColor' },
            { headerName: this.l('Destination Code'), headerTooltip: this.l('DestinationCode'), field: 'destinationCode' },
            { headerName: this.l('Ed Odno'), headerTooltip: this.l('Ed Odno'), field: 'edOdno' },
            { headerName: this.l('Cancel Flag'), headerTooltip: this.l('CancelFlag'), field: 'cancelFlag' },
            { headerName: this.l('SmsCarFamily Code'), headerTooltip: this.l('SmsCarFamilyCode'), field: 'smsCarFamilyCode' },
            { headerName: this.l('Order Type'), headerTooltip: this.l('OrderType'), field: 'orderType' },
            { headerName: this.l('Katashiki Code'), headerTooltip: this.l('KatashikiCode'), field: 'katashikiCode' },
            { headerName: this.l('End Of Record'), headerTooltip: this.l('EndOfRecord'), field: 'endOfRecord' },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
    }

    show(datefrom, dateto, vinNo?: any) {
        this.paginationParams1 = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._fromdate = datefrom;
        this._todate = dateto;
        this.vinNo = vinNo;
        this.isLoading1 = true;
        this._service.getViewIF(
            this._dateTimeService.convertToDate(this._fromdate),
            this._dateTimeService.convertToDate(this._todate),
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        ).subscribe(result => {
            this.paginationParams1.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
        });
        this.modal.show();
    }



    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }
    changeActive(event) {
        this._isActive = event.target.checked;
    }

    exportViewIFToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getViewIFExcel(
            this._dateTimeService.convertToDate(this._fromdate),
            this._dateTimeService.convertToDate(this._todate),
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }

    autoSizeAll() {

        const allColumnIds: string[] = [];
        this.dataParamsIF.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked"
                && column.getId().toString() != "stt"
                && column.getId().toString() != "Spec200 Digits") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsIF.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsIF.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000)
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading1 = true;
        this.dataParamsIF = params;
        params.api.paginationSetPageSize(this.paginationParams1.pageSize);
        this.paginationParams1.skipCount =
            ((this.paginationParams1.pageNum ?? 1) - 1) * (this.paginationParams1.pageSize ?? 0);
        this._service.getViewIF(
            this._dateTimeService.convertToDate(this._fromdate),
            this._dateTimeService.convertToDate(this._todate),
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        ).subscribe(result => {
            this.paginationParams1.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
            // this.resetGridView();
            this.isLoading1 = false;
        });
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getViewIF(
            this._dateTimeService.convertToDate(this._fromdate),
            this._dateTimeService.convertToDate(this._todate),
            '',
            this.paginationParams1.skipCount,
            this.paginationParams1.pageSize
        );
    }
    changePage(paginationParams) {
        this.isLoading1 = true;
        this.paginationParams1 = paginationParams;
        this.paginationParams1.skipCount = (this.rowData.length <= 0) ? 0 : ((paginationParams.pageNum - 1) * paginationParams.pageSize);
        this.getDatas(this.paginationParams1).subscribe((result) => {
            this.paginationParams1.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams1.totalPage = ceil(result.totalCount / (this.paginationParams1.pageSize ?? 0));
            this.isLoading1 = false;
        });
    }

}
