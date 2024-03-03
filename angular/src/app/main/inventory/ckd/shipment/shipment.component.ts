import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdShipmentDto, InvCkdShipmentServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './shipment.component.html',
})
export class ShipmentComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvCkdShipmentDto = new InvCkdShipmentDto();
    saveSelectedRow: InvCkdShipmentDto = new InvCkdShipmentDto();
    datas: InvCkdShipmentDto = new InvCkdShipmentDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    shipmentNo : string = '' ;
	shippingcompanyCode : string = '' ;
    supplierNo : string = '' ;
	buyer : string = '' ;
	fromPort : string = '' ;
	toPort : string = '' ;
	shipmentDate : string = '' ;
    etd : any ;
	eta : any ;
	ata : any ;
	feedervesselName : string = '' ;
	oceanvesselName : string = '' ;
    oceanvesselvoyageNo : string = '' ;
	noofinvoicewithinshipmentNo : number = 0 ;
	noof20Ftcontainers : number = 0 ;
	noof40Ftcontainers : number = 0 ;
	lclvolume : number = 0 ;
    atd : any ;
	status : string = '' ;
	isActive : string = '' ;
    pShipmentID;
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
        private _service: InvCkdShipmentServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            			{headerName: this.l('Shipment No'),headerTooltip: this.l('Shipment No'),field: 'shipmentNo',flex: 1},
			{headerName: this.l('Shippingcompany Code'),headerTooltip: this.l('Shippingcompany Code'),field: 'shippingcompanyCode',flex: 1},
			{headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo',flex: 1},
			{headerName: this.l('Buyer'),headerTooltip: this.l('Buyer'),field: 'buyer',flex: 1},
			{headerName: this.l('From Port'),headerTooltip: this.l('From Port'),field: 'fromPort',flex: 1},
			{headerName: this.l('To Port'),headerTooltip: this.l('To Port'),field: 'toPort',flex: 1},
			{headerName: this.l('Shipment Date'),headerTooltip: this.l('Shipment Date'),field: 'shipmentDate',flex: 1},
			{headerName: this.l('Etd'),headerTooltip: this.l('Etd'),field: 'etd',valueGetter: (params) => this.pipe.transform(params.data?.etd, 'dd/MM/yyyy'),flex: 1,
            comparator: this._formStoringService.dateComparator,
            },
			{headerName: this.l('Eta'),headerTooltip: this.l('Eta'),field: 'eta',valueGetter: (params) => this.pipe.transform(params.data?.eta, 'dd/MM/yyyy'),flex: 1,
            comparator: this._formStoringService.dateComparator,

            },
			{headerName: this.l('Ata'),headerTooltip: this.l('Ata'),field: 'ata',valueGetter: (params) => this.pipe.transform(params.data?.ata, 'dd/MM/yyyy'),flex: 1,
            comparator: this._formStoringService.dateComparator,

            },
			{headerName: this.l('Feedervessel Name'),headerTooltip: this.l('Feedervessel Name'),field: 'feedervesselName',flex: 1},
			{headerName: this.l('Oceanvessel Name'),headerTooltip: this.l('Oceanvessel Name'),field: 'oceanvesselName',flex: 1},
			{headerName: this.l('Oceanvesselvoyage No'),headerTooltip: this.l('Oceanvesselvoyage No'),field: 'oceanvesselvoyageNo',flex: 1},
			{headerName: this.l('Noofinvoicewithinshipment No'),headerTooltip: this.l('Noofinvoicewithinshipment No'),field: 'noofinvoicewithinshipmentNo',flex: 1,type: 'rightAligned'},
			{headerName: this.l('Noof20Ftcontainers'),headerTooltip: this.l('Noof20Ftcontainers'),field: 'noof20Ftcontainers',flex: 1,type: 'rightAligned'},
			{headerName: this.l('Noof40Ftcontainers'),headerTooltip: this.l('Noof40Ftcontainers'),field: 'noof40Ftcontainers',flex: 1,type: 'rightAligned'},
			{headerName: this.l('Lclvolume'),headerTooltip: this.l('Lclvolume'),field: 'lclvolume',flex: 1,type: 'rightAligned'},
			{headerName: this.l('Atd'),headerTooltip: this.l('Atd'),field: 'atd',valueGetter: (params) => this.pipe.transform(params.data?.atd, 'dd/MM/yyyy'),flex: 1,
            comparator: this._formStoringService.dateComparator,

            },
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status',flex: 1},
			// {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            // buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},}
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.shipmentNo,
			this.shippingcompanyCode,
			this.supplierNo,
			this.fromPort,
			this.toPort,
			this.shipmentDate,
            this.ckdPio,
            this.ordertypeCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
        });
    }


    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){

        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },1000)
    }


    clearTextSearch() {
        this.shipmentNo = '',
		this.shippingcompanyCode = '',
        this.supplierNo = '',
		this.fromPort = '',
		this.toPort = '',
		this.shipmentDate = '',
        this.ckdPio = 'C',
        this.ordertypeCode = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
 			this.shipmentNo,
			this.shippingcompanyCode,
			this.supplierNo,
			this.fromPort,
			this.toPort,
			this.shipmentDate,
            this.ckdPio,
            this.ordertypeCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdShipmentDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdShipmentDto();
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
                // alert(1);
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getShipmentToExcel(
			this.shipmentNo,
			this.shippingcompanyCode,
			this.supplierNo,
			this.fromPort,
			this.toPort,
			this.shipmentDate,
            this.ckdPio,
            this.ordertypeCode
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }




}
