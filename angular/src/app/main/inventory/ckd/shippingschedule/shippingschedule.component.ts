import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { InvCkdShipmentDetailsDto, InvCkdShipmentServiceProxy, InvCkdShippingScheduleDto, InvCkdShippingScheduleServiceProxy, InvIhpMatCustomDeclareServiceProxy, InvIphMatCustomDeclareDetailsDto, InvIphMatCustomDeclareDto } from '@shared/service-proxies/service-proxies';
import { format } from 'path';
import { ImportShippingScheduleComponent } from './import-shippingschedule.component';
import { ChangeDetectorRef } from '@angular/core';

import * as moment from 'moment';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { ButtonRendererComponent } from './renderer/button-renderer.component';


@Component({
    templateUrl: './shippingschedule.component.html',
    styleUrls: ['./shippingschedule.component.less']
})
export class ShippingScheduleComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportShippingScheduleComponent | undefined;
 
    customerColDefs: CustomColDef[] = [];
    detailsColDefs: CustomColDef[] = [];
 
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    selectedRow: InvCkdShippingScheduleDto = new InvCkdShippingScheduleDto();
    saveSelectedRow: InvCkdShippingScheduleDto = new InvCkdShippingScheduleDto();

    saveSelectedRowDetails: InvCkdShipmentDetailsDto = new InvCkdShipmentDetailsDto();
    selectedRowDetails: InvCkdShipmentDetailsDto = new InvCkdShipmentDetailsDto();

    rowsEdited: number[] = [];

    datas: InvCkdShippingScheduleDto = new InvCkdShippingScheduleDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;
    originalRowData: any[] = []; // data sau khi pivot
    totalData: any[] = []; // data nhan tu server
    rowData: any[] = [];
    rowDataDetails: any[] = [];;
    gridApi!: GridApi;
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: any;
    id;
    seller = '';
    buyer = '';
    partNo = '';
    moduleNo = '';
    cfc = '';
    renbanNo = '';
    shipmentNo;
    packingMonth;
    shippingMonthFrom;
    shippingMonthTo;
    exporter;
    valuationType: string = 'A';
    vesselEtd1St;

    valuationTypeList = [
        { value: 'A', label: "ALL" },
        { value: 'MSP', label: "MSP" },
        { value: 'JSP', label: "JSP" },
    ];
    ekanbanList = [
        { value: 'A', label: "ALL" },
        { value: 'P', label: "PAMS" },
        { value: 'E', label: "E-KANBAN" },
    ]
    status: string = '';
    ordertypeCode: string = '';
    declareType: string = '';
    isActive: string = '';
    test: number = 0;
    SellerId;
    BuyerId;
    DateSs;
    VesselNo1st;
    portOfLoading;
    vesselEtd1st;
    vesselName1st;
    vesselNo1st;
    ekanbanFlag = 'A';
    revisionNo;
    portOfDischarge;
    start_time
    end_time
    detailsStatus = '';
    detailsStatusList = [
        { value: '', label: "ALL" },
        { value: 'NEW', label: "NEW" },
        { value: 'SENT', label: "SENT" },
        { value: 'UPDATED', label: "UPDATED" },
    ];
    fn: CommonFunction = new CommonFunction();
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

    constructor(
        injector: Injector,
        private _service: InvCkdShippingScheduleServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef

    ) {
        super(injector);
        this.detailsColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDetails.pageSize * (this.paginationParamsDetails.pageNum - 1),
                cellClass: ['text-center'], width: 55,
            },
            {
                field: 'id',
                hide: true,
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status'

            },
            {
                headerName: this.l('Source'),
                headerTooltip: this.l('Source'),
                field: 'valuationTypeFrom'

            },


            // {
            //     headerName: this.l('Segment'),
            //     headerTooltip: this.l('Segment'),
            //     field: 'segment',
            //     flex: 1,

            // },
            {
                headerName: this.l('Update Date'),
                headerTooltip: this.l('Update Date'),
                field: 'dateSs',
                valueGetter: (params) => params.data?.dateSs ? this.pipe.transform(moment(params.data?.dateSs, 'YYYYMMDD').toDate(), 'dd-MMM-yyyy') : '',
                flex: 1,

            },
            {
                headerName: this.l('Exporter'),
                headerTooltip: this.l('Exporter'),
                field: 'seller',
                flex: 1,
                
            },
            {
                headerName: this.l('ETD'),
                headerTooltip: this.l('ETD'),
                field: 'etd',
                minWidth: 100,
                editable: true,
                headerClass: ["headerClass_shippingEditable"],
                type: 'editableColumn',
                valueGetter: (params) => this.pipe.transform(params.data?.etd, 'dd-MM-yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent', 
                cellRendererParamsCustom: {
                    code: "top"
                }
            },
            {
                headerName: this.l('ETA'),
                headerTooltip: this.l('ETA'),
                field: 'eta',
                minWidth: 100,
                editable: true,
                headerClass: ["headerClass_shippingEditable"],
                type: 'editableColumn',
                valueGetter: (params) => this.pipe.transform(params.data?.eta, 'dd-MM-yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
                cellRendererParamsCustom: {
                    code: "top"
                }
            },
            // {
            //     headerName: this.l('Buyer'),
            //     headerTooltip: this.l('Buyer'),
            //     field: 'buyer',
            //     flex: 1,

            // },
            // {
            //     headerName: this.l('Packing Date'),
            //     headerTooltip: this.l('Packing Date'),
            //     field: 'packingDate',
            //     flex: 1,
            // },

            // {
            //     headerName: this.l('Shipping Month'),
            //     headerTooltip: this.l('Shipping Month'),
            //     field: 'shippingMonth',
            //     flex: 1,

            // },
            // {
            //     headerName: this.l('Order Lot'),
            //     headerTooltip: this.l('Order Lot'),
            //     field: 'orderLot',
            //     type: 'rightAligned',
            //     flex: 1,

            // },
            // {
            //     headerName: this.l('Car Family Code'),
            //     headerTooltip: this.l('Car Family Code'),
            //     field: 'carFamilyCode',
            //     flex: 1,

            // },
            // {
            //     headerName: this.l('Importer Code'),
            //     headerTooltip: this.l('Importer Code'),
            //     field: 'importerCode',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Ekanban Flag'),
            //     headerTooltip: this.l('Ekanban Flag'),
            //     field: 'ekanbanFlag',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Port Of Loading'),
            //     headerTooltip: this.l('Port Of Loading'),
            //     field: 'portOfLoading',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Port Of Discharge'),
            //     headerTooltip: this.l('Port Of Discharge'),
            //     field: 'portOfDischarge',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Vessel Etd1 st'),
            //     headerTooltip: this.l('Vessel Etd 1st'),
            //     field: 'vesselEtd1st',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Vessel Name 1st'),
            //     headerTooltip: this.l('Vessel Name 1st'),
            //     field: 'vesselName1st',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Vessel No 1st'),
            //     headerTooltip: this.l('Vessel No 1st'),
            //     field: 'vesselNo1st',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Exporter Code'),
            //     headerTooltip: this.l('Exporter Code'),
            //     field: 'exporterCode',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Order Control No'),
            //     headerTooltip: this.l('Order Control No'),
            //     field: 'orderControlNo',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Renban No'),
            //     headerTooltip: this.l('Renban No'),
            //     field: 'renbanNo',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Container Size'),
            //     headerTooltip: this.l('Container Size'),
            //     field: 'containerSize',
            //     flex: 1,
            // },
            {
                headerName: this.l('Model Code'),
                headerTooltip: this.l('Model Code'),
                field: 'carFamilyCode',
                flex: 1,
            },
            // {
            //     headerName: this.l('Lot No'),
            //     headerTooltip: this.l('Lot No'),
            //     field: 'lotNo',
            //     flex: 1,
            // },

            // {
            //     headerName: this.l('Case No'),
            //     headerTooltip: this.l('Case No'),
            //     field: 'caseNo',
            //     flex: 1,
            // },

            // {
            //     headerName: this.l('Module No'),
            //     headerTooltip: this.l('Module No'),
            //     field: 'moduleNo',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('FormD Renban'),
            //     headerTooltip: this.l('FormD Renban'),
            //     field: 'formDRenban',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('M3OfModule'),
            //     headerTooltip: this.l('M3OfModule'),
            //     field: 'm3OfModule',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Gross Weight Of Module'),
            //     headerTooltip: this.l('Gross Weight Of Module'),
            //     field: 'grossWeightOfModule',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Re Export Code'),
            //     headerTooltip: this.l('Re Export Code'),
            //     field: 'reExportCode',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('SsNo'),
            //     headerTooltip: this.l('SsNo'),
            //     field: 'ssNo',
            //     flex: 1,
            // },
            {
                headerName: this.l('Line Code'),
                headerTooltip: this.l('Line Code'),
                field: 'lineCode',
                flex: 1,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1,

            },
            // TODO: ADD Part name
            {

                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
            },
            {

                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'scheduleQty',
                editable: true,
                headerClass: ["headerClass_shippingEditable"],
                type: 'editableColumn',
            },
            {
                headerName: this.l('Packing Month'),
                headerTooltip: this.l('Packing Month'),
                field: 'packingMonth',
                flex: 1,
            },
            {
                headerName: this.l('ETA Delay'),
                headerTooltip: this.l('ETA Delay'),
                field: 'etaDelay',
                editable: true,
                headerClass: ["headerClass_shippingEditable"],
                type: 'editableColumn',
                valueGetter: (params) => this.pipe.transform(params.data?.etaDelay, 'dd-MM-yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
                cellRendererParamsCustom: {
                    code: "top"
                }
            },
            {
                headerName: this.l('Revision No'),
                headerTooltip: this.l('Revision No'),
                field: 'revisionNo',
                flex: 1,
            },
            {
                headerName: this.l('Remark'),
                headerTooltip: this.l('Remark'),
                field: 'remark',
                editable: true,
                headerClass: ["headerClass_shippingEditable"],
                type: 'editableColumn',
                minWidth: 250,
            },
            {
                headerName: this.l('Manual Update Status'),
                headerTooltip: this.l('Manual Update Status'),
                field: 'manualUpdateStatus',
                flex: 1,
            },
            {
                headerName: this.l('Manual Update Confirm'),
                headerTooltip: this.l('Manual Update Confirm'),
                width: 100,
                pinned: 'right',
                field: 'manualUpdateConfirm',
                cellRenderer: 'buttonRenderer',
                cellRendererParams: {
                    customParam: (params) => {
                        return {
                            onClick: this.SubmitManualUpdate.bind(this),
                            disabled: !this.rowsEdited.includes(params.data.id)
                        };
                    }
                },
                cellClass: ['text-center'],
             
            },
             
        ];
 
      
 
        this.frameworkComponents = {
            buttonRenderer: ButtonRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent, 
            agDatepickerRendererComponent: AgDatepickerRendererComponent
        };
 
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.fn.setHeight_notFullHeight();
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
    }

    renderGridByHoang() {

        this.customerColDefs = [];
        this.customerColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                flex: 1,
            },
            {
                headerName: this.l('Id'),
                headerTooltip: this.l('Id'),
                field: 'id',
                flex: 1,
                hide: true
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1,
            },
            {
                headerName: this.l('Source'),
                headerTooltip: this.l('Source'),
                field: 'valuationTypeFrom',
                flex: 1,

            },
            {
                headerName: this.l('Revision No'),
                headerTooltip: this.l('Revision No'),
                field: 'revisionNo',
                flex: 1,
            },
            // {
            //     headerName: this.l('ShipmentNo '),
            //     headerTooltip: this.l('ShipmentNo'),
            //     field: 'shipmentNo',
            //     flex: 1,
            // },
            {
                headerName: this.l('Update Date'),
                headerTooltip: this.l('Update Date'),
                valueFormatter: (params) => params.data?.dateSs ? moment(params.data?.dateSs, 'YYYYMMDD').format("DD-MMM-YY"):'',
                field: 'dateSs',
                flex: 1
            },
            {
                headerName: this.l('Exporter'),
                headerTooltip: this.l('Exporter'),
                field: 'seller',
                flex: 1
            },
            // {
            //     headerName: this.l('Segment '),
            //     headerTooltip: this.l('Segment'),
            //     field: 'segment',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Buyer'),
            //     headerTooltip: this.l('Buyer'),
            //     field: 'buyer',
            //     flex: 1,
            // },
            {
                headerName: this.l('Packing Month'),
                headerTooltip: this.l('Packing Month'),
                field: 'shippingMonth',
                valueFormatter: (params) => moment(params.data?.shippingMonth, 'YYYYMMDD').format("MMM-YY"),
                flex: 1,
            },
            // {
            //     headerName: this.l('Ekanban Flag'),
            //     headerTooltip: this.l('Ekanban Flag'),
            //     field: 'ekanbanFlag',
            //     flex: 1,
            // },
            // {
            //     headerName: this.l('Port Of Loading'),
            //     headerTooltip: this.l('Port Of Loading'),
            //     field: 'portOfLoading',
            //     flex: 1,
            // },
        ];

        /// lấy bản ghi có nhiều lần xuất hiện nhất dựa theo key là DateSs, Seller, Buyer và ShippingMonth
        const mostRepeatedName = this.rowData.reduce((acc, curr) => {
            acc[curr.dateSs + curr.seller + curr.buyer + curr.shippingMonth] = (acc[curr.dateSs + curr.seller + curr.buyer + curr.shippingMonth] || 0) + 1;
            return acc;
          }, {});

        /// lấy số lần xuất hiện của bản ghi có nhiều lần đó
        const totalEtdRows = Math.max(...Object.values<number>(mostRepeatedName));
        for (let index = 1; index <= totalEtdRows; index++) {
            // this.customerColDefs.push({
            //         headerName: this.l(`${this.renderNumberSuffix(index)}`),
            //         headerTooltip: this.l(`${this.renderNumberSuffix(index)}`),
            //         children: [{
            //                 headerName: this.l(`Etd ${this.renderNumberSuffix(index)}`),
            //                 headerTooltip: this.l(`Etd ${this.renderNumberSuffix(index)}`),
            //                 field: 'etd' + index,
            //                 flex: 1,
            //             },
            //             {
            //                 headerName: this.l(`Eta ${this.renderNumberSuffix(index)}`),
            //                 headerTooltip: this.l(`Eta ${this.renderNumberSuffix(index)}`),
            //                 field: 'eta' + index,
            //                 flex: 1,
            //             },
            //         ],
            //     });

                this.customerColDefs.push({
                    headerName: this.l(`Etd ${this.renderNumberSuffix(index)}`),
                    headerTooltip: this.l(`Etd ${this.renderNumberSuffix(index)}`),
                    field: 'etd' + index,
                    valueFormatter: (params) => params.data['etd' + index] ? moment(params.data['etd' + index], 'YYYYMMDD').format("DD-MMM-YY") : '',
                    flex: 1,
                },
                {
                    headerName: this.l(`Eta ${this.renderNumberSuffix(index)}`),
                    headerTooltip: this.l(`Eta ${this.renderNumberSuffix(index)}`),
                    field: 'eta' + index,
                    valueFormatter: (params) => params.data['eta' + index] ? moment(params.data['eta' + index], 'YYYYMMDD').format("DD-MMM-YY") : '',
                    flex: 1,
                },);
        }

        this.customerColDefs.push(
            // {
            //     headerName: this.l('Port Of Discharge'),
            //     headerTooltip: this.l('Port Of Discharge'),
            //     field: 'portOfDischarge',
            //     flex: 1,
            // },
            {
                headerName: this.l('Total No Of Modules'),
                headerTooltip: this.l('Total No Of Modules'),
                field: 'totalOfCargoes',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Total M3 Of Modules '),
                headerTooltip: this.l('Total M3 Of Modules'),
                field: 'totalM3OfCargoes',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('TotalContainers20'),
                headerTooltip: this.l('TotalContainers20'),
                field: 'totalContainers20',
                type: 'rightAligned',
                //valueFormatter: (params) => this.pipe.transform(params.data?.ngaY_DK, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('TotalContainers40'),
                headerTooltip: this.l('TotalContainers40'),
                field: 'totalContainers40',
                type: 'rightAligned',
                flex: 1
            },
            // {
            //     headerName: this.l('VesselBookingRefNo'),
            //     headerTooltip: this.l('VesselBookingRefNo'),
            //     field: 'vesselBookingRefNo',
            //     flex: 1,

            // },
        )

        this.dataParams.api!.setColumnDefs([]);
        this.dataParams.api!.setColumnDefs(this.customerColDefs);



        let rsData = [];
        let viewState = [];
        try {
            this.rowData.reverse().forEach(item => {
                let index = rsData.findIndex(x => (x.dateSs === item.dateSs && x.seller === item.seller && x.buyer == item.buyer && x.shippingMonth === item.shippingMonth && x.valuationTypeFrom === item.valuationTypeFrom));
                if (index == -1) {
                    item['etd1'] = item.vesselEtd1St;
                    item['eta1'] = item.vesselEta1st;

                    /// list ids tổng hợp các bản ghi, dùng để search details.
                    item['ids'] = [item.id];
                    viewState.push({
                        item: `${item.dateSs}${item.seller}${item.buyer}${item.shippingMonth}${item.valuationTypeFrom}`,
                        currentState: 1
                    })
                    rsData.push(item);
                } else {
                    let state = viewState.find(x => x.item === `${item.dateSs}${item.seller}${item.buyer}${item.shippingMonth}${item.valuationTypeFrom}`);
                    rsData[index]['etd' + (state.currentState + 1)] = item.vesselEtd1St;
                    rsData[index]['eta' + (state.currentState + 1)] = item.vesselEta1st;
                    rsData[index]['totalOfCargoes'] = Number(rsData[index]['totalOfCargoes']) + Number(item.totalOfCargoes);
                    rsData[index]['totalM3OfCargoes'] = Number(rsData[index]['totalM3OfCargoes']) + Number(item.totalM3OfCargoes);
                    rsData[index]['totalContainers20'] = Number(rsData[index]['totalContainers20']) + Number(item.totalContainers20);
                    rsData[index]['totalContainers40'] = Number(rsData[index]['totalContainers40']) + Number(item.totalContainers40);
                    rsData[index]['ids'].push(item.id);
                    state.currentState += 1;
                }
            })
        } catch (error) {
            console.log(error)
        }

        this.rowData = rsData.reverse();

        if (this.originalRowData.length == 0) {
            this.originalRowData = rsData;
            this.paginationParams.totalCount = rsData.length;
        }
        this.resetGridView();
    }

    renderNumberSuffix(number) {
        let suffix = "";
        if ([11, 12, 13].includes(number % 100)) {
            suffix = "th";
        } else {
            const lastDigit = number % 10;
            suffix = {1: "st", 2: "nd", 3: "rd"}[lastDigit] || "th";
        }

        return number + suffix;
    }


    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    autoSizeDetails() {
        const allColumnIds: string[] = [];
        this.dataParamsDetails.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsDetails.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        this.cdr.detectChanges();
        this.dataParamsDetails.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        this.autoSize();
        this.autoSizeDetails();
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSize();
            this.autoSizeDetails();
        }, 1)
    }

    // searchAll() {
    //     this.searchDatas()
    //     this.searchDatasDetails()
    // }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.moduleNo,
            this.cfc,
            this.renbanNo,
            this.seller,
            this.buyer,
            this._dateTimeService.convertToDatetime(this.packingMonth),
            // this._dateTimeService.convertToDatetime(this.shippingMonthFrom),
            // this._dateTimeService.convertToDatetime(this.shippingMonthTo),
            this.valuationType,
            this.portOfLoading,
            this.vesselEtd1st,
            this.vesselName1st,
            this.vesselNo1st,
            this.portOfDischarge,
            this.ekanbanFlag,
            this.revisionNo,
            this.shipmentNo,
            this.detailsStatus,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.items.length > 0) {
                    this.originalRowData = [];
                    this.totalData = result.items;
                    this.paginationParams.totalCount = result.totalCount;
                    this.rowData = result.items;
                    this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                    this.isLoading = false;
                    this.renderGridByHoang();
                } else {
                    this.totalData = [];
                    this.rowData = [];
                    this.rowDataDetails = [];
                    this.isLoading = false;
                    this.resetGridView();
                }
            });
    }
    searchDatasDetails(): void {
        this.isLoading = true;
        if (this.selectedRow['ids'] && this.selectedRow['ids'].length > 0)
            this._service.getAllDetails(
                // this.partNo,
                // this.moduleNo,
                // this.cfc,
                // this.renbanNo,
                // this.seller,
                // this.buyer,
                // this._dateTimeService.convertToDatetime(this.shippingMonthFrom),
                // this._dateTimeService.convertToDatetime(this.shippingMonthTo),
                // this.valuationType,
                // this.portOfLoading,
                // this.vesselEtd1st,
                // this.vesselName1st,
                // this.vesselNo1st,
                // this.portOfDischarge,
                // this.ekanbanFlag,
                // this.revisionNo,
                // this.shipmentNo,
                this.selectedRow['ids'],
                this.partNo,
                this.cfc,
                this.detailsStatus,
                '',
                this.paginationParamsDetails.skipCount,
                this.paginationParamsDetails.pageSize
            ).subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDetails = result.items;
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.rowData = [];
        this.originalRowData = [];
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };

        this.cfc = '';
        this.partNo = '';
        this.seller = '';
        this.buyer = '';
        this.valuationType = 'A';
        this.detailsStatus = '';
        this.packingMonth = '';
        this.revisionNo = '';
        this.shipmentNo = '';
        this.searchDatas();
        // this.searchDatasDetails();
    }



    getAllShipingSchedule(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.moduleNo,
            this.cfc,
            this.renbanNo,
            this.seller,
            this.buyer,
            this._dateTimeService.convertToDatetime(this.packingMonth),
            // this._dateTimeService.convertToDatetime(this.shippingMonthFrom),
            // this._dateTimeService.convertToDatetime(this.shippingMonthTo),
            this.valuationType,
            this.portOfLoading,
            this.vesselEtd1st,
            this.vesselName1st,
            this.vesselNo1st,
            this.portOfDischarge,
            this.ekanbanFlag,
            this.revisionNo,
            this.shipmentNo,
            this.detailsStatus,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    getShipingScheduleDetails(paginationParams?: PaginationParamsModel) {
        if (this.selectedRow['ids'] && this.selectedRow['ids'].length > 0)
            return this._service.getAllDetails(
                // this.partNo,
                // this.moduleNo,
                // this.cfc,
                // this.renbanNo,
                // this.seller,
                // this.buyer,
                // this._dateTimeService.convertToDatetime(this.shippingMonthFrom),
                // this._dateTimeService.convertToDatetime(this.shippingMonthTo),
                // this.valuationType,
                // this.portOfLoading,
                // this.vesselEtd1st,
                // this.vesselName1st,
                // this.vesselNo1st,
                // this.portOfDischarge,
                // this.ekanbanFlag,
                // this.revisionNo,
                // this.shipmentNo,
                this.selectedRow['ids'],
                this.partNo,
                this.cfc,
                this.detailsStatus,
                '',
                this.paginationParamsDetails.skipCount,
                this.paginationParamsDetails.pageSize
            );
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;

        this.rowData = this.originalRowData.slice(this.paginationParams.skipCount, Number(this.paginationParams.pageSize) + this.paginationParams.skipCount)
        this.renderGridByHoang();
        this.isLoading = false;

        // this.isLoading = true;
        // this.paginationParams = paginationParams;
        // this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.getAllShipingSchedule(this.paginationParams).subscribe((result) => {
        //     this.paginationParams.totalCount = result.totalCount;
        //     this.rowData = result.items;
        //     this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        //     this.isLoading = false;
        //     this.renderGridByHoang();
        // });
    }

    changePageDetails(paginationParams) {
        this.isLoading = true;
        this.paginationParamsDetails = paginationParams;
        this.paginationParamsDetails.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getShipingScheduleDetails(this.paginationParamsDetails).subscribe((result) => {
            this.paginationParamsDetails.totalCount = result.totalCount;
            this.rowDataDetails = result.items;
            this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdShippingScheduleDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectedRow = Object.assign({}, selected);
        if (selected) {
            this.SellerId = selected.seller;
            this.BuyerId = selected.buyer;
            this.vesselEtd1St = selected.vesselEtd1St;
            this.VesselNo1st = selected.vesselNo1st;
            this.DateSs = selected.dateSs;
            // this.revisionNo = selected.revisionNo;
            this.searchDatasDetails();
        }
    }

    onChangeRowSelectionDetails(params: { api: { getSelectedRows: () => InvCkdShipmentDetailsDto[] } }) {
        const selectedTable2 = params.api.getSelectedRows()[0];
        this.selectedRowDetails = Object.assign({}, selectedTable2);
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getAllShipingSchedule(this.paginationParams)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.totalData = result.items ?? [];
                this.originalRowData = [];
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.renderGridByHoang();
            });
    }

    callBackDataGridDetails(params: GridParams) {
        this.isLoading = true;
        this.dataParamsDetails = params;
        params.api.paginationSetPageSize(this.paginationParamsDetails.pageSize);
        this.paginationParamsDetails.skipCount =
            ((this.paginationParamsDetails.pageNum ?? 1) - 1) * (this.paginationParamsDetails.pageSize ?? 0);
        this.paginationParamsDetails.pageSize = this.paginationParamsDetails.pageSize;
        this.getShipingScheduleDetails(this.paginationParamsDetails)
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDetails = result.items ?? [];
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }

    onCellValueChanged(e) {
        if (e.oldValue != e.newValue) {
        // add to list edited
            if (!this.rowsEdited.includes(e.data.id)) {
                this.rowsEdited.push(e.data.id);
            }
            this.dataParamsDetails.api.redrawRows();
            // this.dataParamsDetails.api.refreshInfiniteCache();
        }
    }

    // markAsDirty(e) {
    //     e.colDef.cellClass = (p) =>
    //       p.rowIndex.toString() === e.node.id ? "ag-cell-dirty" : "";

    //     e.api.refreshCells({
    //       force: true
    //     });
    //   }


    exportShippingMSP(e) {
        this.exportLoading(e, true);
        this._service.getInvCkdShippingToExcelNew(
            this._dateTimeService.convertToDatetime(this.packingMonth),
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.exportLoading(e, false));
            });

    }

    isCancelDisable() {
        return this.rowDataDetails.filter(e => e.status === 'UPDATED').length == 0
    }

    exportLoading(e, _isLoad?: boolean) {
        console.log(e);
        if (_isLoad) {
            this.lostForcus();
            this.start_time = new Date();
            document.getElementById("exportExcel").classList.add('exportExcel');
        }
        else {
            this.end_time = new Date();
            let s = this.end_time.getSeconds() - this.start_time.getSeconds();
            let ms = this.end_time.getMilliseconds() - this.start_time.getMilliseconds();
            let coundown = 2000 - ms; coundown = (coundown > 0) ? coundown : 0;
            setTimeout(() => {
                document.getElementById("exportExcel").classList.remove('exportExcel');
            }, coundown);
            return coundown;
        }
    }

    exportShippingJSP(e) {
        this.exportLoading(e, true);
        this._service.getInvCkdShippingJSPToExcelNew(
            this._dateTimeService.convertToDatetime(this.packingMonth),
        )
            .subscribe((result) => {
                this.exportLoading(e, false);
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            });
    }

    firmShippingSchedule(e, firmType: string) {
        if (this.selectedRow['ids'] && this.selectedRow['ids'].length > 0) {
            this.message.confirm(this.l('AreYouSureToFirm'), 'Firm Shipment', (isConfirmed) => {
                if (isConfirmed) {
                    this.fn.exportLoading(e, true);
                    this._service.getInvCkdShippingToFirm(
                        this.selectedRow['ids'],
                        firmType
                    ).subscribe(() => {
                        this.callBackDataGrid(this.dataParams!);
                        this.notify.success(this.l('SuccessfullyFirmed'));
                        this.notify.info(this.l('SuccessfullyFirmed'));
                        this.fn.exportLoading(e, false);
                    });
                }
            });
        }
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

    cancelShipment() {
        console.log(this.dataParams.api.getFocusedCell());

        /// get value
        var selectedData = this.selectedRow[this.dataParams.api.getFocusedCell().column.getColId()];

        // lay ra row data tu originalRowData dua theo selectedRow
        var selectedRowData = this.totalData.filter((x) => x.eta1 == selectedData || x.etd1 == selectedData);
        console.log(selectedRowData);
        // this.originalRowData.filter((x) => x.id == this.selectedRow.id);
        // this.message.confirm(this.l('AreYouSureToCancel'), 'Cancel Shipment', (isConfirmed) => {
        //     if (isConfirmed) {
        //         this._service.getInvCkdShippingJSPToCancel(
        //             this.selectedRow.shipmentNo,
        //             this.selectedRow.shippingMonth,
        //             this.selectedRow.vesselEtd1St,
        //             this.selectedRow.index
        //         ).subscribe(() => {
        //             this.callBackDataGrid(this.dataParams!);
        //             this.notify.success(this.l('SuccessfullyCancelled'));
        //             this.notify.info(this.l('SuccessfullyCancelled'));
        //         });
        //     }
        // });
    }

    cancelManualUpdate() {
        this.message.confirm(this.l('Are you sure to cancel all manual update?. This action cannot rollback.'), 'Cancel Manual Update', (isConfirmed) => {
            if (isConfirmed) {
                this._service.cancelManualUpdate().subscribe(() => {
                    this.callBackDataGridDetails(this.dataParamsDetails!);
                    this.notify.success(this.l('SuccessfullyCancelled'));
                    this.notify.info(this.l('SuccessfullyCancelled'));
                    this.rowsEdited = [];
                });
            }
        });
}


    SubmitManualUpdate(event){
        /// vì trường eta, etd là lazy loading nên có thể lúc submit 2 trường đó chưa đc load
        /// -> check xem 2 trường eta, etd đã được render thành DateTime chưa

        let eta = event.rowData.eta.hasOwnProperty('ts') ? event.rowData.eta : this._dateTimeService.convertToDate(event.rowData.eta);
        let etd = event.rowData.etd.hasOwnProperty('ts') ? event.rowData.etd : this._dateTimeService.convertToDate(event.rowData.etd);
        ///
        this._service.getShipmentDetailsEdit(
            event.rowData.id,
            eta,
            etd,
            this._dateTimeService.convertToDate(event.rowData.etaDelay),
            event.rowData.scheduleQty,
            event.rowData.remark ?? ''
        ).subscribe(() => {
            this.searchDatasDetails();
            this.notify.success(this.l('SuccessfullyUpdated'));
            this.notify.info(this.l('SuccessfullyUpdated'));
        });
    }
}

