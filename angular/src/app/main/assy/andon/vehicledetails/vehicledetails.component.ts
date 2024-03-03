import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AsyAdoVehicleDetailsDto, AsyAdoVehicleDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './vehicledetails.component.html',
})
export class VehicleDetailsComponent extends AppComponentBase implements OnInit {
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

    selectedRow: AsyAdoVehicleDetailsDto = new AsyAdoVehicleDetailsDto();
    saveSelectedRow: AsyAdoVehicleDetailsDto = new AsyAdoVehicleDetailsDto();
    datas: AsyAdoVehicleDetailsDto = new AsyAdoVehicleDetailsDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    no
    bodyNo: string = '';
    lotNo: string = '';
    cfc: string = '';
    noInLot: number = 0;
    sequenceNo: string = '';
    keyNo: string = '';
    vin: string = '';
    eng: string = '';
    trs: string = '';
    ecu: string = '';
    wInDateActual: any;
    tInPlanDatetime: any;
    paintInTime: string = '';
    aInDateActual: any;
    insOutDateActual: any;
    insLineOutVp4DateActual: any;
    driverAirBag: string = '';
    passengerAirBag: string = '';
    sideAirBagLh: string = '';
    sideAirBagRh: string = '';
    kneeAirBagLh: string = '';
    curtainSideAirBagLh: string = '';
    curtainSideAirBagRh: string = '';
    totalDelay: string = '';
    shippingTime: any;
    vehicleId: string = '';
    testNo: string = '';
    isPrintedQrcode: string = '';
    printedQrcodeDate: any;
    updatedDate: any;
    isProject: string = '';
    changedRecordsVehicleDetails: number[] = [];
    selectId;
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
        private _service: AsyAdoVehicleDetailsServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('No'),
                headerTooltip: this.l('No'),
                field: 'no',
                width: 80,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                width: 110
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 80
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 80

            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 80,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'),
                field: 'sequenceNo',
                width: 100
            },
            {
                headerName: this.l('Key No'),
                headerTooltip: this.l('Key No'),
                field: 'keyNo',
                width: 80,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Vin'),
                headerTooltip: this.l('Vin'),
                field: 'vin',
                width: 140
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                width: 60
            },
            {
                headerName: this.l('Eng'),
                headerTooltip: this.l('Eng'),
                field: 'eng',
                width: 90
            },
            {
                headerName: this.l('Trs'),
                headerTooltip: this.l('Trs'),
                field: 'trs',
                width: 130
            },
            {
                headerName: this.l('Ecu'),
                headerTooltip: this.l('Ecu'),
                field: 'ecu',
                width: 90
            },
            {
                headerName: this.l('W In Date Actual'),
                headerTooltip: this.l('W In Date Actual'),
                field: 'wInDateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.wInDateActual, 'dd/MM/yyyy HH:mm:ss'),
                width: 120
            },
            {
                headerName: this.l('T In Plan Datetime'),
                headerTooltip: this.l('T In Plan Datetime'),
                field: 'tInPlanDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.tInPlanDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 130
            },
            {
                headerName: this.l('Paint In Time'),
                headerTooltip: this.l('Paint In Time'),
                field: 'paintInTime',
                width: 110
            },
            {
                headerName: this.l('A In Date Actual'),
                headerTooltip: this.l('A In Date Actual'),
                field: 'aInDateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.aInDateActual, 'dd/MM/yyyy HH:mm:ss'),
                width: 120
            },
            {
                headerName: this.l('Ins Out Date Actual'),
                headerTooltip: this.l('Ins Out Date Actual'),
                field: 'insOutDateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.insOutDateActual, 'dd/MM/yyyy HH:mm:ss'),
                width: 130
            },
            {
                headerName: this.l('Ins Line Out Vp4 Date Actual'),
                headerTooltip: this.l('Ins Line Out Vp4 Date Actual'),
                field: 'insLineOutVp4DateActual',
                valueGetter: (params) => this.pipe.transform(params.data?.insLineOutVp4DateActual, 'dd/MM/yyyy HH:mm:ss'),
                width: 180
            },
            {
                headerName: this.l('Driver Air Bag'),
                headerTooltip: this.l('Driver Air Bag'),
                field: 'driverAirBag',
                width: 130
            },
            {
                headerName: this.l('Passenger Air Bag'),
                headerTooltip: this.l('Passenger Air Bag'),
                field: 'passengerAirBag',
                width: 130
            },
            {
                headerName: this.l('Side Air Bag Lh'),
                headerTooltip: this.l('Side Air Bag Lh'),
                field: 'sideAirBagLh',
                width: 120
            },
            {
                headerName: this.l('Side Air Bag Rh'),
                headerTooltip: this.l('Side Air Bag Rh'),
                field: 'sideAirBagRh',
                width: 120
            },
            {
                headerName: this.l('Knee Air Bag Lh'),
                headerTooltip: this.l('Knee Air Bag Lh'),
                field: 'kneeAirBagLh',
                width: 120
            },
            {
                headerName: this.l('Curtain Side Air Bag Lh'),
                headerTooltip: this.l('Curtain Side Air Bag Lh'),
                field: 'curtainSideAirBagLh',
                width: 150
            },
            {
                headerName: this.l('Curtain Side Air Bag Rh'),
                headerTooltip: this.l('Curtain Side Air Bag Rh'),
                field: 'curtainSideAirBagRh',
                width: 150
            },
            {
                headerName: this.l('Total Delay'),
                headerTooltip: this.l('Total Delay'),
                field: 'totalDelay',
                width: 90
            },
            {
                headerName: this.l('Shipping Time'),
                headerTooltip: this.l('Shipping Time'),
                field: 'shippingTime',
                valueGetter: (params) => this.pipe.transform(params.data?.shippingTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 120
            },
            {
                headerName: this.l('Vehicle Id'),
                headerTooltip: this.l('Vehicle Id'),
                field: 'vehicleId',
                width: 80
            },
            {
                headerName: this.l('Test No'),
                headerTooltip: this.l('Test No'),
                field: 'testNo',
                width: 80
            },
            {
                headerTooltip: this.l('Printed Qrcode Date'),
                field: 'printedQrcodeDate',
                valueGetter: (params) => this.pipe.transform(params.data?.printedQrcodeDate, 'dd/MM/yyyy HH:mm:ss'),
                width: 150
            },
            {
                headerName: this.l('Updated Date'),
                headerTooltip: this.l('Updated Date'),
                field: 'updatedDate', valueGetter: (params) => this.pipe.transform(params.data?.updatedDate, 'dd/MM/yyyy HH:mm:ss'),
                width: 110
            },
            {
                headerName: this.l('Is Projecte'),
                headerTooltip: this.l('Is Project'),
                field: 'isProject',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isProject == "1") {
                            return 'Project'
                        }
                        else if (params.data?.isProject == "0") {
                            return 'InProject'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isProject == "1") {
                            return 'btnActive'
                        }
                        else if (params.data?.isProject == "0") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },



            {
                headerName: this.l('Lineoff Datetime'),
                headerTooltip: this.l('LineoffDatetime'),
                field: 'lineoffDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.lineoffDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 110
            },
            {
                headerName: this.l('Lineoff Date'),
                headerTooltip: this.l('LineoffDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.lineoffDate, 'dd/MM/yyyy'),
                field: 'lineoffDate',
                width: 110
            },
            {
                headerName: this.l('Lineoff Time'),
                headerTooltip: this.l('LineoffTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.lineoffTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'lineoffTime',
                width: 110
            },
            {
                headerName: this.l('Pdi Datetime'),
                headerTooltip: this.l('PdiDatetime'),
                valueGetter: (params) => this.pipe.transform(params.data?.pdiDatetime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'pdiDatetime',
                width: 110
            },
            {
                headerName: this.l('Pdi Date'),
                headerTooltip: this.l('PdiDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.pdiDate, 'dd/MM/yyyy'),
                field: 'pdiDate',
                width: 110
            },
            {
                headerName: this.l('Pdi Time'),
                headerTooltip: this.l('PdiTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.pdiTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'pdiTime',
                width: 110
            },
            {
                headerName: this.l('PIO Actual Datetime'),
                headerTooltip: this.l('PIOActualDatetime'),
                valueGetter: (params) => this.pipe.transform(params.data?.pioActualDatetime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'pioActualDatetime',
                width: 110
            },
            {
                headerName: this.l('PIO Actual Date'),
                headerTooltip: this.l('PIOActualDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.pioActualDate, 'dd/MM/yyyy'),
                field: 'pioActualDate',
                width: 110
            },
            {
                headerName: this.l('PIO Actual Time'),
                headerTooltip: this.l('pIOActualTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.pdiTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'formatPIOActualTime',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Datetime'),
                headerTooltip: this.l('SalesActualDatetime'),
                valueGetter: (params) => this.pipe.transform(params.data?.salesActualDatetime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'salesActualDatetime',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Date'),
                headerTooltip: this.l('SalesActualDate'),
                valueGetter: (params) => this.pipe.transform(params.data?.salesActualDate, 'dd/MM/yyyy'),
                field: 'salesActualDate',
                width: 110
            },
            {
                headerName: this.l('Sales Actual Time'),
                headerTooltip: this.l('SalesActualTime'),
                //valueGetter: (params) => this.pipe.transform(params.data?.pdiTime, 'dd/MM/yyyy HH:mm:ss'),
                field: 'formatSalesActualTime',
                width: 110
            },

            {
                headerName: this.l('Engine Id'),
                headerTooltip: this.l('EngineId'),
                field: 'engineId',
                width: 110
            },
            {
                headerName: this.l('Trans Id'),
                headerTooltip: this.l('TransId'),
                field: 'transId',
                width: 110
            },
            {
                headerName: this.l('Sales Sfx'),
                headerTooltip: this.l('SalesSfx'),
                field: 'salesSfx',
                width: 110
            },
            {
                headerName: this.l('Color Type'),
                headerTooltip: this.l('ColorType'),
                field: 'colorType',
                width: 110
            },
            {
                headerName: this.l('Indent Line'),
                headerTooltip: this.l('IndentLine'),
                field: 'indentLine',
                width: 110
            },
            {
                headerName: this.l('Ss No'),
                headerTooltip: this.l('SsNo'),
                field: 'ssNo',
                width: 110
            },
            {
                headerName: this.l('Ed Number'),
                headerTooltip: this.l('EdNumber'),
                field: 'edNumber',
                width: 110
            },
            {
                headerName: this.l('Goshi Car'),
                headerTooltip: this.l('GoshiCar'),
                field: 'goshiCar',
                width: 110
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsVehicleDetails = result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this.isLoading = true;
        this._service.getAll(
            this.cfc,
            this.bodyNo,
            this.lotNo,
            this.vin,
            this.sequenceNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;

            });
    }

    clearTextSearch() {
            this.no = '',
            this.cfc ='',
            this.bodyNo = '',
            this.lotNo = '',
            this.vin = '',
            this.sequenceNo = '',
            this.searchDatas();

    }

    onSelectionMulti(params) {
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
            this.cfc,
            this.bodyNo,
            this.lotNo,
            this.vin,
            this.sequenceNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => AsyAdoVehicleDetailsDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new AsyAdoVehicleDetailsDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
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
            });
    }


    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getVehicleDetailsToExcel(
            this.cfc,
            this.bodyNo,
            this.lotNo,
            this.vin,
            this.sequenceNo,)
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
