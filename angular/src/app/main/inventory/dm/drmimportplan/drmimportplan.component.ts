import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvDrmImportPlanDto, InvDrmImportPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditDrmImportPlanModalComponent } from './create-or-edit-drmimportplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';

@Component({
    templateUrl: './drmimportplan.component.html'
})
export class DrmImportPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalDrmImportPlan', { static: true }) createOrEditModalDrmImportPlan: | CreateOrEditDrmImportPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    listIdStatus = '';
    arrIdStatus: InvDrmImportPlanDto[] = [];
    RestrictList = [
        { key: 'S', value: "SCHEDULE" },
        { key: 'F', value: "CONFIRM" },
        { key: 'C', value: "CANCEL" },
    ];
    isShowConfirm;

    selectedRow: InvDrmImportPlanDto = new InvDrmImportPlanDto();
    saveSelectedRow: InvDrmImportPlanDto = new InvDrmImportPlanDto();
    pending: string = '';
    disable: boolean = false;
    datas: InvDrmImportPlanDto = new InvDrmImportPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    supplierNo: string = '';
    etd
    eta
    shipmentNo: string = '';
    cfc: string = '';
    partCode: string = '';
    partNo: string = '';
    partName: string = '';
    qty
    packingMonth
    delayEtd
    delayEta
    remark: string = '';
    ata

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service: InvDrmImportPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {

                // headerName: "checked",
                // headerTooltip: "checked",
                field: "checked",
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true,
                width: 37
            },
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',

                cellRenderer: 'agSelectRendererComponent',
                list: this.RestrictList,
                cellClass: ['RendererCombobox', 'text-center'],
            },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            {
                headerName: this.l('Etd'), headerTooltip: this.l('Etd'), field: 'etd',
                valueGetter: (params) => this.pipe.transform(params.data?.etd, 'dd/MM/yyyy'), flex: 1
            },
            {
                headerName: this.l('Eta'), headerTooltip: this.l('Eta'), field: 'eta',
                valueGetter: (params) => this.pipe.transform(params.data?.eta, 'dd/MM/yyyy'), flex: 1
            },
            {
                headerName: this.l('Ata'), headerTooltip: this.l('Ata'), field: 'ata',
                valueGetter: (params) => this.pipe.transform(params.data?.ata, 'dd/MM/yyyy'), flex: 1
            },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Shipment No'), headerTooltip: this.l('Shipment No'), field: 'shipmentNo', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Part Code'), headerTooltip: this.l('Part Code'), field: 'partCode', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            {
                headerName: this.l('Packing Month'), headerTooltip: this.l('Packing Month'), field: 'packingMonth', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.packingMonth, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Delay Etd'), headerTooltip: this.l('Delay Etd'), field: 'delayEtd',
                valueGetter: (params) => this.pipe.transform(params.data?.delayEtd, 'dd/MM/yyyy'), flex: 1
            },
            {
                headerName: this.l('Delay Eta'), headerTooltip: this.l('Delay Eta'), field: 'delayEta',
                valueGetter: (params) => this.pipe.transform(params.data?.delayEta, 'dd/MM/yyyy'), flex: 1
            },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 },

        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000)
    }
    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.etd),
            this._dateTimeService.convertToDatetime(this.eta),
            this.cfc,
            this.partCode,
            this.partNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.supplierNo = '',
            this.etd = '',
            this.eta = '',
            this.shipmentNo = '',
            this.cfc = '',
            this.partCode = '',
            this.partNo = '',
            this.partName = '',
            this.qty = '',
            this.packingMonth = '',
            this.delayEtd = '',
            this.delayEta = '',
            this.remark = '',
            this.ata = '',
            this.searchDatas();
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmImportPlanDto[] } }) {

        console.log('onChangeRowSelection');
        this.arrIdStatus = params.api.getSelectedRows();
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmImportPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        var myDate = new Date();
        if (this.selectedRow.ata) {
            var date1 = this.selectedRow.ata.toString().substring(0, 10);
            var date2 = this._dateTimeService.convertToDatetime(myDate).toString().substring(0, 10);
            this.isShowConfirm = !(this.selectedRow.id && (date1 >= date2))
        }
    }


    onSelectionMulti(params) {
        console.log('onSelectionMulti');
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRow, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRow.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.etd),
            this._dateTimeService.convertToDatetime(this.eta),
            this.cfc,
            this.partCode,
            this.partNo,
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
            // this.resetGridView();
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
                this.isLoading = false;
            });
    }
    onCellValueChanged(ev) {
        this._service.conFirmStatus(ev.data.id.toString(), ev.newValue).subscribe((result) => {

            if (result > 0) {
                this.callBackDataGrid(this.dataParams!);
                this.notify.success(this.l('Update Success'));
            }

            else {
                this.callBackDataGrid(this.dataParams!);
                this.notify.warn(this.l('Not Update'));
            }

        });

    }
    conFirmStartusMultiChk(system: InvDrmImportPlanDto): void {

        this.arrIdStatus.forEach(e => {
            this.listIdStatus += e.id + ',';
        })

        this._service.conFirmStatusMultiCkb(this.listIdStatus, 'F').subscribe(() => {
            this.callBackDataGrid(this.dataParams!);
            this.notify.success(this.l('SavedSuccessfully'));
            this.notify.info(this.l('SavedSuccessfully'));
        });
    }

    rowClickData: InvDrmImportPlanDto;
    onRowClick(params) {

        let _rows = document.querySelectorAll<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container .ag-row.ag-row-level-0.ag-row-position-absolute");
        for (let i = 0; _rows[i]; i++) { _rows[i].classList.remove("setcolor_background_rowclick"); }

        if (this.rowClickData && this.rowClickData.id == params.data.id) this.rowClickData = undefined;
        else {
            this.rowClickData = params.data;
            let _row = document.querySelector<HTMLElement>("body .ag-theme-alpine .ag-center-cols-container div[row-id='" + params.node.rowIndex + "'].ag-row.ag-row-level-0.ag-row-position-absolute");
            if (_row) _row.classList.add("setcolor_background_rowclick");
        }

    }

    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;
    //     this._service.getDrmImportPlanToExcel(
    //         this.supplierNo,
    // 		this.etd,
    // 		this.eta,
    // 		this.shipmentNo,
    // 		this.cfc,
    // 		this.partCode,
    // 		this.partNo,
    // 		this.partName,
    // 		this.qty,
    // 		this.packingMonth,
    // 		this.delayEtd,
    // 		this.delayEta,
    // 		this.remark,
    // 		this.ata
    //     )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;
    //         });
    // }

}
