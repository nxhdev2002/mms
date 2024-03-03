import { DateTime } from 'luxon';
import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvTopsseInvoiceDetailsDto, InvTopsseInvoiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe, formatDate } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CellClassParams, CellValueChangedEvent, EditableCallbackParams, GridApi, GridOptions } from '@ag-grid-enterprise/all-modules';
import { AgCellTextRendererComponent } from '@app/shared/common/grid/ag-cell-text-renderer/ag-cell-text-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { TopsseInvoiceComponent } from './topsseinvoice.component';

@Component({
    templateUrl: './receive-topsseinvoice-modal.component.html',
    selector: 'receive-topsseinvoice'
})
export class ReceiveTopsseInvoiceComponent extends AppComponentBase implements OnInit {
    @ViewChild('receiveTopsseInvoice', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    datasEdit: InvTopsseInvoiceDetailsDto[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
    rowData: any[] = [];
    rowDataUpdate: any[] = [];
    dataParams: GridParams | undefined;
    gridApi: GridApi | undefined;
    gridColumnApi: any;

    p_id: any;
    date = new Date();
    _receiveDate: any;
    _invoiceNo: string = '';
    _date: string = '';

    processNoUpdate: boolean = false;
    valueChange: string = '';
    columnChange: string = '';

    gridOptions: GridOptions = {
        columnTypes: {
            editableColumn: {

                editable: (params: EditableCallbackParams) => { return true; },
                valueParser: 'Number(newValue)',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',

                cellClass: (params) => { return this.cellEditGetsClass(params); },
                valueGetter: (params) => { return this.syncValidateValueGetter(params) },
                valueSetter: (params) => { return this.syncValidateValueSetter(params) },
            },
            valueColumn: {
                editable: true,
                valueParser: 'Number(newValue)',
                cellClass: 'number-cell',
                cellRenderer: 'agAnimateShowChangeCellRenderer',
                filter: 'agNumberColumnFilter',
            },
        },
    };

    defaultColDef = {
        resizable: true,
        sortable: true, enableGroupEdit: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
        autoGroupColumnDef: {
            minWidth: 400,
            cellRendererParams: {
                suppressCount: true,
                checkbox: false,
            },
            pinned: true,
        },
        suppressAggFuncInHeader: true,
        enableCellChangeFlash: true,
        animateRows: true
    };

    constructor(injector: Injector,
        private _service: InvTopsseInvoiceServiceProxy,
        private _reload: TopsseInvoiceComponent,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);

        this.col_init();

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            AgCellTextComponent: AgCellTextRendererComponent
        };
    }

    ngOnInit(): void {
        this._date = formatDate(new Date(this.date.toString()), 'yyyy/MM/dd', 'en-US').replace(/\//g, '');
    }

    col_init() {
        this.viewColDefs = [
            {
                headerName: this.l('Case No'), headerTooltip: this.l('Case No'), field: 'caseNo', width: 90,

            },
            {
                headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 90,

            },
            {
                headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 120,

            },
            {
                headerName: this.l('Order Qty'), headerTooltip: this.l('Order Qty'), field: 'partQty', flex: 1, type: 'rightAligned',

            },
            {
                headerName: this.l('Received Qty'), headerTooltip: this.l('Received Qty'), field: 'receivedQuantity', flex: 1, type: 'rightAligned',
            },
            {
                headerName: this.l('Remain Qty'), headerTooltip: this.l('Remain Qty'), field: 'remainQty', flex: 1, type: 'rightAligned',
            },


            {
                headerName: this.l('Receive Qty'), headerTooltip: this.l('Receive Qty'), field: 'receiveQuantity', flex: 1, type: 'editableColumn',
            },//cột cần edit


            {
                headerName: this.l('Unit Fob'), headerTooltip: this.l('Unit Fob'), field: 'unitFob', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.unitFob, 0),
            },
            {
                headerName: this.l('Amount'), headerTooltip: this.l('Amount'), field: 'receiveAmount', flex: 1, type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.receiveAmount, 0),
            },
        ];
    }

    syncValidateValueGetter(params) {

        try {
            if (params.data == undefined) return 0;

            let field = params.colDef.field;
            let v = params.data[field];
            if (v == null || v == undefined || Number.isNaN(v)) {    // không đúng định dạng
                return 0;
            }
            else {   //hợp lệ
                return v;
            }
        } catch (e) {
            return 0;
        }
    }

    syncValidateValueSetter(params) {
        try {
            if (params.data == undefined || params.data == null) return 0;

            let field = params.colDef.field;
            let v = params.newValue;

            if (v == null || v == undefined || Number.isNaN(v)) {

                params.data[field] = 0;
                params.data['receiveAmount'] = params.data['receiveQuantity'] * params.data['unitFob'];
                params.api.applyTransaction({ update: [params.data] });

                return 0;
            }
            else {  //hợp lệ

                params.newValue = ((params.newValue) < 0) ? params.oldValue : params.newValue;

                params.data[field] = params.newValue;

                if (params.newValue > params.data['remainQty']) {
                    this.message.warn(this.l('Receive Qty không được vượt quá Remain Qty!'), 'Warning');
                    params.data[field] = params.oldValue;
                }
                if (params.newValue == 0) params.data['receiveAmount'] = 0;

                params.api.applyTransaction({ update: [params.data] });

                return params.newValue;
            }

        } catch (e) {
            return 0;
        }

    }

    onCellValueChanged(params: CellValueChangedEvent) {
        params.data['receiveAmount'] = params.data['receiveQuantity'] * params.data['unitFob'];

        params.api.applyTransaction({ update: [params.data] });

        if (params.newValue == null || Number.isNaN(params.newValue) || params.newValue == undefined) return;

        let ischange = true;
        this.datasEdit.forEach((item) => {  // kiểm tra row change đã tồn tại trong mảng edit chưa
            if (item.keyRow == params.data.keyRow) ischange = false;
        });
        if (ischange && this.processNoUpdate != true) {
            this.datasEdit.push(params.data);
            this.valueChange = params.newValue;
            this.columnChange = params.column.getId();
        }

    }

    cellEditGetsClass(params) {
        if (params.data == undefined || params.data == null) return ['cell-edit', 'number-cell',];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    headerEditClass(params) {
        if (params.data == undefined || params.data == null) return [''];
        else return ['cell-edit', 'number-cell', 'cell-edit-edited'];
    }

    show(id, invoiceNo): void {
        this.p_id = id;
        this._invoiceNo = invoiceNo;
        this._service.getTopsseInvoiceDetailsToReceive(
            this.p_id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.rowData = result.items;

            const dateTo = ((this.rowData[0].receiveDate == undefined || this.rowData[0].receiveDate == null) ||
                (Number.parseInt(formatDate(new Date(this.rowData[0].receiveDate.toString()), 'yyyy/MM/dd', 'en-US').replace(/\//g, '')) < Number.parseInt(this._date)))
                ? this.date : new Date(this.rowData[0].receiveDate.toString());
            this.datepicker?.bsValueChange.emit(dateTo);
        });
        this.modal.show();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getTopsseInvoiceDetailsById(
            this.p_id,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;

        this.dataParams = params;
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;

        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;

        this.getDatas(this.paginationParams)
            .subscribe((result) => {
                this.rowData = result.items;
                this.isLoading = false;
            });
    }

    close(): void {
        this.datasEdit = [];
        this.valueChange = "";
        this.columnChange = "";
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    confirm(): void {
        var stringvalue = '';
        var _count = 0;
        if (Number.parseInt(formatDate(new Date(this._receiveDate.toString()), 'yyyy/MM/dd', 'en-US').replace(/\//g, ''))
            < Number.parseInt(this._date)) {
            this.message.warn(this.l('Receive Date không được trước ngày hiện tại!'), 'Warning');
        }
        else {
            if (this.datasEdit.length > 0) {
                for (let i = 0; i < this.datasEdit.length; i++) {
                    stringvalue += this.datasEdit[i].id + '-' + this.datasEdit[i].receiveQuantity + ';';
                    if (this.datasEdit[i].receiveQuantity == 0) _count++;
                }
            } else {
                for (let i = 0; i < this.rowData.length; i++) {
                    stringvalue += this.rowData[i].id + '-' + this.rowData[i].receiveQuantity + ';';
                    if (this.rowData[i].receiveQuantity == 0) _count++;
                }
            }
            if ((_count == this.rowData.length && this.rowData.length > 0) || (_count == this.datasEdit.length && this.datasEdit.length > 0))
                this.message.warn(this.l('Cần ít nhất 1 Receive Qty > 0!'), 'Warning');
            else {
                this.message.confirm(this.l('Xác nhận Receive?'), 'Confirm Receive Topsse Invoice', (isConfirmed) => {
                    if (isConfirmed) {
                        this._service.getDataReceiveTopsseInvoice(
                            stringvalue,
                            this._dateTimeService.convertToDatetime(this._receiveDate)
                        )
                            .subscribe((result) => {
                                this.datasEdit = [];
                                this.valueChange = "";
                                this.columnChange = "";
                                this.notify.info(this.l('Receive Topsse Invoice Successfully!'));
                                this.close();
                                this.modal?.hide();
                                this._reload.searchDatas();
                            });
                    }
                });
            }
        }
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
