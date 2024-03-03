import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsMaterialDto, InvGpsMaterialServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
// import { CreateOrEditInvGpsMaterialModalComponent } from './create-or-edit-material-modal.component.html';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTime } from 'luxon';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditInvGpsMaterialModalComponent } from './create-or-edit-material-modal.component';
import { ImportGpsMaterialComponent } from './import-gps-material-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { BlobOptions } from 'buffer';


@Component({
    templateUrl: './material.component.html',
})

export class InvGpsMaterialComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalInvGpsMaterial', { static: true }) createOrEditModalInvGpsMaterial: | CreateOrEditInvGpsMaterialModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importMaterial', { static: true }) importMaterial: | ImportGpsMaterialComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvGpsMaterialDto = new InvGpsMaterialDto();
    saveSelectedRow: InvGpsMaterialDto = new InvGpsMaterialDto();
    datas: InvGpsMaterialDto = new InvGpsMaterialDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    supplierCode: string = '';
    stkConcept: string = '';
    category: string = '';
    location: string = '';
    partNo: string = '';
    partName: string = '';
    partNameVn: string = '';
    partType: string = '';
    uses: string = '';
    spec: string = '';
    isExpDate: boolean = true;
    partGroup: string = '';
    price: number;
    currency: string = '';
    priceConvert: number;
    uom: string = '';
    supplierName: string = '';
    supplier: string = '';
    localImport: string = '';
    leadTime: number;
    leadTimeForecas: number;
    minLot: number;
    boxQty: number;
    remark: string = '';
    palletL: number;
    palletR: number;
    palletH: number;
    id;
    hasExpiryDate: boolean = true;
    isActive: boolean = true;
    selectId;
    changedRecordsMaterialList: number[] =[]
    locImpList = [
        { value: '', label: "All" },
        { value: 'LOCAL', label: "LOCAL" },
        { value: 'IMPORT', label: "IMPORT" },
    ];
    partTypeList = [
        {value: 'IDM' , label: "IDM"},
        {value: 'DM' , label: "DM"},
        ];
    categoryList = [
            {value: 'Indirect Materials' , label: "Indirect Materials"},
            {value: 'Direct Materials' , label: "Direct Materials"},
            {value: 'Spare parts' , label: "Spare parts"},
            {value: 'Stationery' , label: "Stationery"},
            {value: 'Other' , label: "Other"},
        ];
    locationList = [
            {value: 'GPS' , label: "GPS"},
            {value: 'Shop' , label: "Shop"},
            ];

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
        private _service: InvGpsMaterialServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo' },
            { headerName: this.l('Part No Normalized'), headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized' },
            { headerName: this.l('Color Sfx'), headerTooltip: this.l('Color Sfx'), field: 'colorSfx' },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName' },
            { headerName: this.l('Part Name VN'), headerTooltip: this.l('Part Name VN'), field: 'partNameVn' },

            //{ headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType' },
            { headerName: this.l('Category'), headerTooltip: this.l('Category'), field: 'category' },
            //{ headerName: this.l('Location'), headerTooltip: this.l('Location'), field: 'location' },
            { headerName: this.l('Purpose Of Use'), headerTooltip: this.l('Purpose Of Use'), field: 'purposeOfUse' },
            { headerName: this.l('Spec'), headerTooltip: this.l('Spec'), field: 'spec' },
            { headerName: this.l('Has Expiry Date'), headerTooltip: this.l('Has Expiry Date'), field: 'hasExpiryDate' },
            { headerName: this.l('Part Group'), headerTooltip: this.l('Part Group'), field: 'partGroup' },
            {
                headerName: this.l('Price'),
                headerTooltip: this.l('Price'),
                field: 'price',
                type: 'rightAligned',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.price, 2),
            },
            { headerName: this.l('Currency'), headerTooltip: this.l('Currency'), field: 'currency' },
            { headerName: this.l('Price VND'), headerTooltip: this.l('Price VND'), field: 'convertPrice',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.priceConvert, 2),type: 'rightAligned' },
            { headerName: this.l('UOM'), headerTooltip: this.l('UOM'), field: 'uom' },
            { headerName: this.l('Supplier Name'), headerTooltip: this.l('Supplier Name'), field: 'supplierName' },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo' },
            { headerName: this.l('LocalImport'), headerTooltip: this.l('Local Import'), field: 'localImport' },
            { headerName: this.l('LeadTime'), headerTooltip: this.l('Lead Time'), field: 'leadTime', type: 'rightAligned' },
            { headerName: this.l('LeadTimeForecast'), headerTooltip: this.l('Lead Time Forecast'), field: 'leadTimeForecast', type: 'rightAligned' },
            { headerName: this.l('Min Lot'), headerTooltip: this.l('Min Lot'), field: 'minLot', type: 'rightAligned' },
            { headerName: this.l('Box Qty'), headerTooltip: this.l('Box Qty'), field: 'boxQty', type: 'rightAligned' },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark' },
            { headerName: this.l('Pallet L'), headerTooltip: this.l('Pallet L'), field: 'palletL', type: 'rightAligned' },
            { headerName: this.l('Pallet R'), headerTooltip: this.l('Pallet R'), field: 'palletR', type: 'rightAligned' },
            { headerName: this.l('Pallet H'), headerTooltip: this.l('Pallet H'), field: 'palletH', type: 'rightAligned' },
            {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                    buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},
            }
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
            this.changedRecordsMaterialList = result;
            console.log("result =", result);
        })
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
        }, 1000)
    }

    searchDatas(): void {
        this._service.getAll(
            this.partNo,
            this.supplier,
            this.partType,
            this.hasExpiryDate == true ? 'Y' : 'N',
            this.partGroup,
            this.localImport,
            this.isActive == true ? 'Y' : 'N',
            this.partName,
            this.category,
            this.location,
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.supplier,
            this.partType,
            this.hasExpiryDate == true ? 'Y' : 'N',
            this.partGroup,
            this.localImport,
            this.isActive == true ? 'Y' : 'N',
            this.partName,
            this.category,
            this.location,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsMaterialDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsMaterialDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
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


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getMaterialToExcel(
            this.partNo,
            this.supplier,
            this.partType,
            this.hasExpiryDate == true ? 'Y' : 'N',
            this.partGroup,
            this.localImport,
            this.isActive == true ? 'Y' : 'N',
            this.partName,
            this.category,
            this.location,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    clearTextSearch() {
        this.partNo = '',
            this.supplier = '',
            this.partType = '',
            this.hasExpiryDate = true,
            this.partGroup = '',
            this.localImport = '',
            this.partName = '',
            this.category = '',
            this.location = '',
            this.isActive = true,
            this.searchDatas()
    }


}
