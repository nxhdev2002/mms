import { GridApi, GridReadyEvent, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdModuleCaseDto, InvCkdModuleCaseServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewModuleCaseModalComponent } from './view-detail-modulecase-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './modulecase.component.html',
    styleUrls: ['./modulecase.component.less'],
})
export class ModuleCaseComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewModalModuleCase', { static: true }) viewModalModuleCase: | ViewModuleCaseModalComponent | undefined;
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

    selectedRow: InvCkdModuleCaseDto = new InvCkdModuleCaseDto();
    saveSelectedRow: InvCkdModuleCaseDto = new InvCkdModuleCaseDto();
    datas: InvCkdModuleCaseDto = new InvCkdModuleCaseDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    billNo: string = '';
    moduleNo: string = '';
    caseNo: string = '';
    containerNo: string = '';
    renban: string = '';
    invoiceId
    lotNo: string = '';
    supplierNo: string = '';
    containerListId
    moduleCaseNo: string = '';
    invoiceNo: string = '';
    cdDate
    storageLocationCode: string = '';
    locationId
    locationDate
    locationBy: string = '';
    unpackingDate

    unpackingFromDate
    unpackingToDate

    unpackingTime
    unpackingDatetime

    devanningDate
    devanningFromDate
    devanningToDate
    devanningTime

    netWeight
    grossWeight
    measuarementM3
    lengthofcasemodule
    widthofcasemodule
    heightofcasemodule
    dummy: string = '';
    plannedpackingdate
    dgflag: string = '';
    packingType: string = '';
    rrType: string = '';
    freightPerInvoice
    insurancePerInvoice
    innerMaterialType1: string = '';
    innerMaterialType2: string = '';
    innerMaterialType3: string = '';
    innerMaterialType4: string = '';
    innerMaterialType5: string = '';
    innerMaterialType6: string = '';
    innerMaterialType7: string = '';
    innerMaterialType8: string = '';
    innerMaterialType9: string = '';
    innerMaterialType10: string = '';
    innerMaterialType11: string = '';
    innerMaterialType12: string = '';
    innerMaterialQuantity1
    innerMaterialQuantity2
    innerMaterialQuantity3
    innerMaterialQuantity4
    innerMaterialQuantity5
    innerMaterialQuantity6
    innerMaterialQuantity7
    innerMaterialQuantity8
    innerMaterialQuantity9
    innerMaterialQuantity10
    innerMaterialQuantity11
    innerMaterialQuantity12
    status
    isActive: string = '';
    billDateFrom: any;
    billDateTo: any;
    radio = '1';

    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters


    defaultColDef = {

        enableValue: true,
        enableRowGroup: true,
        enablePivot: true,
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    ckdPio = 'C';

    ckdPioList = [
        { value: 'C', label: "CKD" },
        { value: 'P', label: "PIO" },
    ];

    ordertypeCode: string = '';
    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];

    constructor(
        injector: Injector,
        private _service: InvCkdModuleCaseServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 45, pinned: 'left' },
            {
                headerName: this.l('Module Case No'), headerTooltip: this.l('Module Case No'), field: 'moduleCaseNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Container No'), headerTooltip: this.l('Container No'), field: 'containerNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', pinned: 'left',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Invoice No'), headerTooltip: this.l('Invoice No'), field: 'invoiceNo',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Cd Date'), headerTooltip: this.l('Cd Date'), field: 'cdDate', valueGetter: (params) => this.pipe.transform(params.data?.cdDate, 'dd/MM/yyyy'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },

            {   headerName: this.l('Bill Date'),headerTooltip: this.l('Bill Date'),field: 'billDate',
                valueFormatter: (params) => this.pipe.transform(params.data?.billDate, 'dd/MM/yyyy'),flex: 1,
            },

            // {
            //     headerName: 'Devanning Info',
            //     children: [
                    {
                        field: 'devanningDate',
                        headerClass: "cell-group-parent-child cell-parent-devanning-info",
                        valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                        enablePivot: true, enableRowGroup: true, enableValue: true,
                    },
                    {
                        field: 'devanningTime',
                        headerClass: "cell-group-parent-child cell-parent-devanning-info",
                        enablePivot: true, enableRowGroup: true, enableValue: true,
                    },
            //     ]
            // },
            // {
            //     headerName: 'Unpacking Info',
            //     children: [
                    {
                        field: 'unpackingDate',headerClass: "cell-group-parent-child cell-parent-unpacking-info",
                         valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy'),
                        enablePivot: true, enableRowGroup: true, enableValue: true,
                    },
                    {
                        field: 'unpackingTime',
                        headerClass: "cell-group-parent-child cell-parent-unpacking-info",
                        valueGetter: (params) => {
                            const inputTime = params.data?.unpackingTime;
                            if (inputTime) {
                              const timeParts = inputTime.split(':');
                              return `${timeParts[0]}:${timeParts[1]}`;
                            }
                            return '';
                        },
                        enablePivot: true,enableRowGroup: true,enableValue: true,
                    },
                    {
                        field: 'unpackingDatetime',
                        headerClass: "cell-group-parent-child cell-parent-unpacking-info",
                        enablePivot: true, enableRowGroup: true, enableValue: true,
                        valueFormatter: (params) => this.pipe.transform(params.data?.unpackingDatetime, 'dd/MM/yyyy HH:mm:ss')
                    },

            //     ]
            // },
            {
                headerName: this.l('Storage Location Code'), headerTooltip: this.l('Storage Location Code'), field: 'storageLocationCode',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Location Datetime'), headerTooltip: this.l('Location Datetime'), field: 'locationDate', valueGetter: (params) => this.pipe.transform(params.data?.locationDate, 'dd/MM/yyyy hh:mm:ss'),
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
            {
                headerName: this.l('Location By'), headerTooltip: this.l('Location By'), field: 'locationBy',
                enablePivot: true, enableRowGroup: true, enableValue: true,
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.moduleCaseNo,
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.invoiceNo,
            this.caseNo,
            this._dateTimeService.convertToDatetime(this.devanningFromDate),
            this._dateTimeService.convertToDatetime(this.devanningToDate),
            this._dateTimeService.convertToDatetime(this.unpackingFromDate),
            this._dateTimeService.convertToDatetime(this.unpackingToDate),
            this.storageLocationCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
            this.billNo,
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.resetGridView();
                this.isLoading = false
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
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
        }, 100)
    }

    clearTextSearch() {
        this.billNo = '';
        this.moduleCaseNo = '',
            this.containerNo = '',
            this.renban = '',
            this.supplierNo = '',
            this.invoiceNo = '',
            this.caseNo = '',

            this.devanningFromDate = '',
            this.devanningToDate = '',
            this.unpackingFromDate = '',
            this.unpackingToDate = '',

            this.devanningDate = '',
            this.devanningTime = '',
            this.unpackingDate = '',
            this.unpackingTime = '',
            this.storageLocationCode = '',
            this.billDateFrom = '',
            this.billDateTo = '',
            this.ckdPio = 'C',
            this.ordertypeCode = '';
            this.lotNo = '';
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        console.log(this.paginationParams.skipCount);

        return this._service.getAll(
            this.moduleCaseNo,
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.invoiceNo,
            this.caseNo,
            this._dateTimeService.convertToDatetime(this.devanningFromDate),
            this._dateTimeService.convertToDatetime(this.devanningToDate),
            this._dateTimeService.convertToDatetime(this.unpackingFromDate),
            this._dateTimeService.convertToDatetime(this.unpackingToDate),
            this.storageLocationCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
            this.billNo,
            this.lotNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdModuleCaseDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdModuleCaseDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
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
            this.isLoading = false;
            this.resetGridView();
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
                this.isLoading = false;
                this.resetGridView();
            });
    }


    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getModuleCaseToExcel(
            this.moduleCaseNo,
            this.containerNo,
            this.renban,
            this.supplierNo,
            this.invoiceNo,
            this.caseNo,
            this._dateTimeService.convertToDatetime(this.devanningFromDate),
            this._dateTimeService.convertToDatetime(this.devanningToDate),
            this._dateTimeService.convertToDatetime(this.unpackingFromDate),
            this._dateTimeService.convertToDatetime(this.unpackingToDate),
            this.storageLocationCode,
            this.radio,
            this._dateTimeService.convertToDatetime(this.billDateFrom),
            this._dateTimeService.convertToDatetime(this.billDateTo),
            this.ckdPio,
            this.ordertypeCode,
            this.billNo,
            this.lotNo,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    setvalradio(i: string) {

        let _btnUncheck = document.querySelector('.actionButton_w' + i + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.radio = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + i);
            if (_btn) _btn.classList.add('active');
            this.radio = i;
        }
        this.searchDatas();

    }
}
