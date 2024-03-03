import { GridApi, GridReadyEvent, SideBarDef, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdVehhicleServiceProxy, InvCkdVehicleDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';

import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ViewInterfaceComponent } from './view-interface.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { EditCKDVehicleDetailModalComponent } from './edit-ckd-vehicle-modal.component';

@Component({
    templateUrl: './ckd-vehicle.component.html',
    styleUrls: ['./ckd-vehicle.component.less'],
})
export class CkdVehicleComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewInterfaceComponent', { static: true }) viewModalInterface: ViewInterfaceComponent;
    @ViewChild('editCkdVehicleComponent', { static: true }) editCkdVehicleComponent: EditCKDVehicleDetailModalComponent;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdVehicleDto = new InvCkdVehicleDto();

    saveSelectedRow: InvCkdVehicleDto = new InvCkdVehicleDto();
    datas: InvCkdVehicleDto = new InvCkdVehicleDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    vinNo;
    typeDate: string = '';
    start_time
    end_time

    _selectDate = -1;
    listDate = [
        { value: -1, label: "" },
        { value: 1, label: "Filter by W In" },
        { value: 2, label: "Filter by W Out" },
        { value: 3, label: "Filter by T In" },
        { value: 4, label: "Filter by T Out" },
        { value: 5, label: "Filter by A In" },
        { value: 6, label: "Filter by A Out" },
        { value: 7, label: "Filter by Lineoff" },
        { value: 8, label: "Filter by Pdi Date" },
        { value: 9, label: "Filter by PIO Date" },
        { value: 10, label: "Filter by Sales Date" }
    ];

    bodyNo: string = '';
    lotNo: string = '';
    noInLot: any;
    lotNo1: string = '';
    noInLot1: any;
    vin: string = '';
    cfc;
    sequenceNo;
    _type;
    color: string = '';
    goshiCar: boolean = false;
    dateFrom: any;
    dateTo: any;
    date = new Date();
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] }
    defaultColDef = {
        enableValue: true,
        enableRowGroup: true,
        enablePivot: true,

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
        private _service: InvCkdVehhicleServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => {
                    return params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1);
                },
                cellClass: ['text-center'],
                width: 55,
                pinned: 'left'
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                width: 80,
                pinned: 'left',
                //enablePivot: true - cho phép chỉnh sửa true/false trên grid
                //enableRowGroup: true - cho phép chỉnh sửa true/false trên grid
                //enableValue: true - thay đổi giá trị //sum, min, max, count, avg, first, last
                //filter: true : chỉ chọn checkbox được
                enableRowGroup: true, enablePivot: true, enableValue: true,
                //filter: true,
                //
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 80,
                pinned: 'left',
                enableRowGroup: true, enablePivot: true, enableValue: true,

            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 80,
                pinned: 'left',
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 80,
                pinned: 'left',
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 80,
                type: 'rightAligned',
                pinned: 'left',
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'),
                field: 'sequenceNo',
                width: 100,
                pinned: 'left',
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Vin'),
                headerTooltip: this.l('Vin'),
                field: 'vin',
                width: 140,
                pinned: 'left',
                //filter: true,
                enableRowGroup: true, enablePivot: true, enableValue: true,
                aggFunc: 'count'
            },

            {
                headerName: this.l('W In Actual Date'),
                headerTooltip: this.l('W In Actual Date'),
                field: 'wInActualDate_DDMMYYYY',
                cellRenderer: (params) => params.data?.wInActualDate_DDMMYYYY,
                aggFunc: this.CountActual, //sum, min, max, count, avg, first, last
                // enablePivot: true, pivot: true,
                enableRowGroup: true, enablePivot: true, enableValue: true,
                width: 120,
            },
            {
                headerName: this.l('W Out Actual Date'),
                headerTooltip: this.l('W Out Actual Date'),
                field: 'wOutActualDate_DDMMYYYY',
                cellRenderer: (params) => params.data?.wOutActualDate_DDMMYYYY,
                aggFunc: this.CountActual, //sum, min, max, count, avg, first, last
                enableRowGroup: true, enablePivot: true, enableValue: true,
                width: 120,

            },
            {
                headerName: this.l('T In Actual Date'),
                headerTooltip: this.l('T In Actual Date'),
                field: 'tInActualDate_DDMMYYYY',
                cellRenderer: (params) => params.data?.tInActualDate_DDMMYYYY,
                width: 130,
                aggFunc: this.CountActual, //sum, min, max, count, avg, first, last
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('T Out Actual Date'),
                headerTooltip: this.l('T Out Actual Date'),
                field: 'tOutActualDate_DDMMYYYY',
                cellRenderer: (params) => params.data?.tOutActualDate_DDMMYYYY,
                enableRowGroup: true, enablePivot: true, enableValue: true,
                width: 120,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last

            },
            {
                headerName: this.l('A In Actual Date'),
                headerTooltip: this.l('A In Actual Date'),
                field: 'aInActualDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.aInActualDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('A Out Actual Date'),
                headerTooltip: this.l('A Out Actual Date'),
                field: 'aOutActualDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.aOutActualDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Lineoff Date'),
                headerTooltip: this.l('Lineoff Date'),
                field: 'lineoffDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.lineoffDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Pdi Date'),
                headerTooltip: this.l('Pdi Date'),
                field: 'pdiDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.pdiDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('PIO Date'),
                headerTooltip: this.l('PIO Date'),
                field: 'pioActualDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.pioActualDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Sales Date'),
                headerTooltip: this.l('Sales Date'),
                field: 'salesActualDate_DDMMYYYY',
                enableRowGroup: true, enablePivot: true, enableValue: true,
                cellRenderer: (params) => params.data?.salesActualDate_DDMMYYYY,
                width: 110,
                aggFunc: this.CountActual  //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Engine Id'),
                headerTooltip: this.l('Engine Id'),
                field: 'engineId',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Trans Id'),
                headerTooltip: this.l('Trans Id'),
                field: 'transId',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Sales Sfx'),
                headerTooltip: this.l('Sales Sfx'),
                field: 'salesSfx',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Color Type'),
                headerTooltip: this.l('Color Type'),
                field: 'colorType',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Description '),
                headerTooltip: this.l('Description '),
                field: 'name',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Indent Line'),
                headerTooltip: this.l('Indent Line'),
                field: 'indentLine',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Ss No'),
                headerTooltip: this.l('Ss No'),
                field: 'ssNo',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Ed Number'),
                headerTooltip: this.l('Ed Number'),
                field: 'edNumber',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },
            {
                headerName: this.l('Goshi Car'),
                headerTooltip: this.l('Goshi Car'),
                field: 'goshiCar',
                width: 110,
                enableRowGroup: true, enablePivot: true, enableValue: true,
            },


        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };


    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.date.setDate(this.date.getDate());
        this.dateFrom = this.date;
    }


    CountActual(values) {
        var _count = 0;
        values.forEach(function (value) {
            _count = _count + ((value != null && value != undefined && value != "") ? 1 : 0);
        });
        return _count;
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }))
            .subscribe((result) => {
                this.isLoading = false;
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                if(this.rowData.length > 0){
                    var rows = this.createRow(1);
                    this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                }else{
                    this.dataParams!.api.setPinnedBottomRowData(null);
                }
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.vin = '';
        this.lotNo = '';
        this.noInLot = '';
        this.cfc = '';
        this.bodyNo = '';
        this.color = '';
        this.sequenceNo = '';
        this.date.setDate(this.date.getDate());
        this.dateFrom = this.date;
        this.dateTo = null;
        this._selectDate = -1;
        this.selectedRow.goshiCar = null;
        this.goshiCar = null;
        this.searchDatas();

    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    // _CountWin:number = 0;
    createRow(count: number): any[] {
        let result: any[] = [];
        var _CountWin = 0;
        var _CountWout = 0;
        var _CountTin = 0;
        var _CountTout = 0;
        var _CountAin = 0;
        var _CountAout = 0;
        var _CountLineOut = 0;
        var _CountPdiDate = 0;
        var _CountPioDate = 0;
        var _CountSalesDate = 0;

        if (this.rowData) {
            if (this.rowData.length > 0) {
                _CountWin = this.rowData[0].countWin;
                _CountWout = this.rowData[0].countWout;
                _CountTin = this.rowData[0].countTin;
                _CountTout = this.rowData[0].countTout;
                _CountAin = this.rowData[0].countAin;
                _CountAout = this.rowData[0].countAout;
                _CountLineOut = this.rowData[0].countLineOut;
                _CountPdiDate = this.rowData[0].countPdiDate;
                _CountPioDate = this.rowData[0].countPioDate;
                _CountSalesDate = this.rowData[0].countSalesDate;
            }
        }

        result.push({
            cfc: 'Grand Total',

            wInActualDate_DDMMYYYY: _CountWin,
            wOutActualDate_DDMMYYYY: _CountWout,
            tInActualDate_DDMMYYYY: _CountTin,
            tOutActualDate_DDMMYYYY: _CountTout,
            aInActualDate_DDMMYYYY: _CountAin,
            aOutActualDate_DDMMYYYY: _CountAout,

            lineoffDate_DDMMYYYY: _CountLineOut,
            pdiDate_DDMMYYYY: _CountPdiDate,
            pioActualDate_DDMMYYYY: _CountPioDate,
            salesActualDate_DDMMYYYY: _CountSalesDate
        });


        return result;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdVehicleDto[] } }) {

        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdVehicleDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        if (this.selectedRow) {
            this.lotNo1 = this.selectedRow.lotNo;
            this.noInLot1 = this.selectedRow.noInLot;
            this.vinNo = this.selectedRow.vin;
        }

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
            .pipe(finalize(() => {
                var rows = this.createRow(1);
                this.dataParams!.api.setPinnedBottomRowData(rows);
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }))
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
        this._service.getInvVehicleToExcel(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportToExcelGIVehicle(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCKDGIVehicleToExcel(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportToExcelVehicleOutDetails(e, partType): void {
        this.exportLoading('exportExcelPDI', e, true)
        this.lostForcusPDI()
        this._service.getInvVehicleDetailOutPartExcel(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            partType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                },
                this.exportLoading('exportExcelPDI', e, false)
                );
            });
    }

    exportToExcelDetailsOutPartByVehicle(e, partType): void {
        this.exportLoading('exportExcelVehicle', e, true)
        this.lostForcusOutPart()
        this._service.getDetailsOutPartByVehicleToExcel(
            this.lotNo1,
            this.noInLot1,
            partType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                },
                //this.fn.exportLoading(e)
                this.exportLoading('exportExcelVehicle', e, false)
                );
            });
    }

    exportToExcelVehicleOutDetailsWIP(e, partType): void {
        this.exportLoading('exportExcelWIP', e, true)
        this.lostForcusWIP()
        this._service.getInvVehicleDetailOutPartWIPExcel(
            this.vin,
            this.lotNo,
            this.noInLot,
            this.cfc,
            this.bodyNo,
            this.color,
            this.sequenceNo,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
            this.typeDate,
            this._selectDate,
            this.goshiCar == true ? 'Y' : '',
            partType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                },
                this.exportLoading('exportExcelWIP', e, false)
                );
            });
    }

    buttonAction(e: string, _type: number) {

        let _btnUncheck = document.querySelector('.actionButton_w' + e + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.typeDate = '';
            this._type = 0;
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + e);
            if (_btn) _btn.classList.add('active');

            this.typeDate = _type.toString();
            this._type = _type;
        }
        this.searchDatas();

    }

    exportReportByDay(e): void {
        if (this.dateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if (this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if (_dateTo < _dateFrom) {
            this.notify.warn("Date From < Date To!");
            return;
        }

        this.exportLoading('exportExcelSummary', e, true)
        this.lostForcus();
        this._service.getExportReportDaily(
            this._type,
            this._dateTimeService.convertToDate(this.dateFrom),
            this._dateTimeService.convertToDate(this.dateTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.exportLoading('exportExcelSummary', e, false)
            });
    }


    exportReportByMonth(e): void {
        if (this.dateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if (this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if (_dateTo < _dateFrom) {
            this.notify.warn("Date From < Date To!");
            return;
        }
        this.exportLoading('exportExcelSummary', e, true)
        this.lostForcus();
        this._service.getExportReportMonthly(
            this._type,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.exportLoading('exportExcelSummary', e, false)
            });
    }


    exportReportColorByDay(e): void {
        if (this.dateFrom == undefined) {
            this.notify.warn("Date From not blank!");
            return;
        }

        if (this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if (_dateTo < _dateFrom) {
            this.notify.warn("Date From < Date To!");
            return;
        }
        this.exportLoading('exportExcelSummary', e, true)
        this.lostForcus()
        this._service.getExportReportColorDaily(
            this._type,
            this._dateTimeService.convertToDate(this.dateFrom),
            this._dateTimeService.convertToDate(this.dateTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.exportLoading('exportExcelSummary', e, false)
            });
    }


    exportReportColorByMonth(e): void {
        if (this.dateFrom == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        if (this.dateTo == undefined) {
            this.notify.warn("Date To not blank!");
            return;
        }

        var _dateFrom = this._dateTimeService.convertToDate(this.dateFrom);
        var _dateTo = this._dateTimeService.convertToDate(this.dateTo);
        if (_dateTo < _dateFrom) {
            this.notify.warn("Date From < Date To!");
            return;
        }
        this.exportLoading('exportExcelSummary', e, true)
        this.lostForcus();
        this._service.getExportReportColorMonthly(
            this._type,
            this._dateTimeService.convertToDatetime(this.dateFrom),
            this._dateTimeService.convertToDatetime(this.dateTo),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.exportLoading('exportExcelSummary', e, false)
            });
    }
    prodSumaryOption() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcus() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
    }

    outPartByVehicle() {
        var outPart = document.querySelector('.btn.out-part') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
        else outPart.classList.add("active");
    }

    lostForcusOutPart() {
        var outPart = document.querySelector('.btn.out-part') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
    }

    outPartByPDI() {
        var outPart = document.querySelector('.btn.out-part-pdi') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
        else outPart.classList.add("active");
    }

    lostForcusPDI() {
        var outPart = document.querySelector('.btn.out-part-pdi') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
    }

    outPartByWIP() {
        var outPart = document.querySelector('.btn.out-part-wip') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
        else outPart.classList.add("active");
    }

    lostForcusWIP() {
        var outPart = document.querySelector('.btn.out-part-wip') as HTMLElement; // .exel-dropdown
        if (outPart.classList.contains('active')) outPart.classList.remove("active");
    }


    viewIF() {
        this.viewModalInterface.show(this.dateFrom, this.dateTo);
    }

    exportLoading(id, e, _isLoad?:boolean) {
        console.log(e);
        if (_isLoad) {
            this.start_time = new Date();
            document.getElementById(id).classList.add('exportExcel');
        }
        else {
            this.end_time = new Date();
            let s = this.end_time.getSeconds() - this.start_time.getSeconds();
            let ms = this.end_time.getMilliseconds() - this.start_time.getMilliseconds();
            let coundown = 2000-ms; coundown = (coundown > 0) ? coundown: 0;
            setTimeout(() => {
                document.getElementById(id).classList.remove('exportExcel');
            }, coundown);
            return coundown;
         }
    }
}
