
import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvSppCostOfSaleSummaryServiceProxy, InvSppShippingServiceProxy, InvTopsseInvoiceDetailsDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe, formatDate } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CellClassParams, CellValueChangedEvent, EditableCallbackParams, GridApi, GridOptions } from '@ag-grid-enterprise/all-modules';
import { AgCellTextRendererComponent } from '@app/shared/common/grid/ag-cell-text-renderer/ag-cell-text-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './excel-summary-modal.component.html',
    selector: 'excel-summary-modal'
})
export class ExcelSummaryModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('excelSummaryModal', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    rowDataUpdate: any[] = [];
    dataParams: GridParams | undefined;
    gridApi: GridApi | undefined;
    gridColumnApi: any;
    fn: CommonFunction = new CommonFunction();

    stockList = [
        {value: 'A' , label: "A"},
        {value: 'N' , label: "N"},
        {value: 'C' , label: "C"},
        {value: 'S' , label: "S"},
    ];

    reportTypeList = [
        {value: '1' , label: "Sale Parts"},
        {value: '2' , label: "Sale C&A"},
        {value: '3' , label: "Sale Chemical (CHE)"},
        {value: '4' , label: "Sale Optional warranty (OPT)"},
        {value: '5' , label: "Sale Export"},
        {value: '6' , label: "Onhand Adjustment"},
        {value: '7' , label: "Internal"},
        {value: '8' , label: "Others"},
    ];

    date = new Date();
    fromMonthYear: any = '';
    toMonthYear: any = '';
    reportType = '';
    stock = '';
    exportType: number;
    processNoUpdate: boolean = false;
    valueChange: string = '';
    columnChange: string = '';

    constructor(injector: Injector,
        private _shippingService: InvSppShippingServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            AgCellTextComponent: AgCellTextRendererComponent
        };
    }

    ngOnInit(): void {
        const currentDate = new Date();
        currentDate.setMonth(currentDate.getMonth() - 1);
        currentDate.setDate(1);
        this.fromMonthYear = currentDate;
    }

    export(e) {
        switch(this.exportType) {
            case 0: // cost of sale (usd)
                this.exportCostOfSaleUsd(e)
                break
            case 1: // cost of sale (vnd)
                this.exportCostOfSaleVnd(e)
                break
            case 2: // balance report
                this.exportBalanceReport(e)
                break
        }
    }


    exportCostOfSaleUsd(e) {
        // this.fn.exportLoading(e, true);
        // this._shippingService.getShippingCostOfSaleSummaryToExcel(
        //     this.reportType,
        //     this.stock,
        //     0,
        //     this._dateTimeService.convertToDatetime(this.fromMonthYear),
        //     this._dateTimeService.convertToDatetime(this.toMonthYear),
        // ) .subscribe((result) => {
        //     setTimeout(() => {
        //         this._fileDownloadService.downloadTempFile(result);
        //         this.notify.success(this.l('Download Excel Successfully'));
        //     }, this.fn.exportLoading(e));
        // });
    }

    exportCostOfSaleVnd(e) {
        // this.fn.exportLoading(e, true);
        // this._shippingService.getShippingCostOfSaleSummaryToExcel(
        //     this.reportType,
        //     this.stock,
        //     1,
        //     this._dateTimeService.convertToDatetime(this.fromMonthYear),
        //     this._dateTimeService.convertToDatetime(this.toMonthYear),
        // ) .subscribe((result) => {
        //     setTimeout(() => {
        //         this._fileDownloadService.downloadTempFile(result);
        //         this.notify.success(this.l('Download Excel Successfully'));
        //     }, this.fn.exportLoading(e));
        // });
    }

    exportBalanceReport(e) {
        // this.fn.exportLoading(e, true)
        // this._shippingService.getShippingBalanceReportToExcel(
        //     this.reportType,
        //     this.stock,
        //     1,
        //     this._dateTimeService.convertToDatetime(this.fromMonthYear),
        //     this._dateTimeService.convertToDatetime(this.toMonthYear),
        // ) .subscribe((result) => {
        //     setTimeout(() => {
        //         this._fileDownloadService.downloadTempFile(result);
        //         this.notify.success(this.l('Download Excel Successfully'));
        //     }, this.fn.exportLoading(e));
        // });
    }


    show(exportType: number): void {
        this.exportType = exportType
        this.modal.show();
    }

    close(): void {
        this.valueChange = "";
        this.columnChange = "";
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
