import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmVehicleCBUDto, MstCmmVehicleCBUServiceProxy, UpdateCmmVehicleCBUCreateMaterial } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { ConfirmDialogComponent } from './confirm-dialog/confirm-dialog.component';


@Component({
    templateUrl: './vehiclecbu.component.html',
})
export class VehicleCBUComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal: | ViewMaterialByIdModalComponent | undefined;
    @ViewChild('confirmDialog', { static: true }) confirmDialog: | ConfirmDialogComponent | undefined;
    

    defaultColDefs: CustomColDef[] = [];
    vehicleCBUColDefs: any;
    vehicleCBUColorColDefs: any;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    paginationParamsColor: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    changedRecordsVehicleCBU: number[] = [];
    selectId;
    selectedRow: MstCmmVehicleCBUDto = new MstCmmVehicleCBUDto();
    datas: MstCmmVehicleCBUDto = new MstCmmVehicleCBUDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsColor: GridParams | undefined;
    rowData: any[] = [];
    rowDataColor: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    vehicleType: string = '';
    model: string = '';
    marketingCode: string = '';
    katashiki: string = '';
    p_id: any;
    selectedIdMaterial;

    _pagesizecolor = 1000000000;

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
        private _service: MstCmmVehicleCBUServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.vehicleCBUColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,pinned: 'left' },
            { headerName: this.l('Vehicle Type'), headerTooltip: this.l('Vehicle Type'), field: 'vehicleType', flex: 1,pinned: 'left' },
            { headerName: this.l('Model'), headerTooltip: this.l('Model'), field: 'model', flex: 1 },
            { headerName: this.l('Marketing Code'), headerTooltip: this.l('Marketing Code'), field: 'marketingCode', flex: 1,pinned: 'left' },
            { headerName: this.l('Production Code'), headerTooltip: this.l('Production Code'), field: 'productionCode', flex: 1,pinned: 'left' },
            { headerName: this.l('Katashiki'), headerTooltip: this.l('Katashiki'), field: 'katashiki', flex: 1 },

            { headerName: this.l('Material Type'), headerTooltip: this.l('Material Type'), field: 'materialType', flex: 1 },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Industry Sector'), headerTooltip: this.l('Industry Sector'), field: 'industrySector', flex: 1 },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description', flex: 1 },
            { headerName: this.l('Material Group'), headerTooltip: this.l('Material Group'), field: 'materialGroup', flex: 1 },
            { headerName: this.l('Base Unit Of Measure'), headerTooltip: this.l('Base Unit Of Measure'), field: 'baseUnitOfMeasure', flex: 1 },
            { headerName: this.l('Deletion Flag'), headerTooltip: this.l('Deletion Flag'), field: 'deletionFlag', flex: 1 },
            { headerName: this.l('Plant'), headerTooltip: this.l('Plant'), field: 'plant', flex: 1 },
            { headerName: this.l('Storage Location'), headerTooltip: this.l('StorageLocation'), field: 'storageLocation', flex: 1 },
            { headerName: this.l('Production Group'), headerTooltip: this.l('Production Group'), field: 'productionGroup', flex: 1 },
            { headerName: this.l('Production Purpose'), headerTooltip: this.l('Production Purpose'), field: 'productionPurpose', flex: 1 },
            { headerName: this.l('Production Type'), headerTooltip: this.l('Production Type'), field: 'productionType', flex: 1 },
            { headerName: this.l('Profit Center'), headerTooltip: this.l('Profit Center'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Batch Management'), headerTooltip: this.l('Batch Management'), field: 'batchManagement', flex: 1 },
            { headerName: this.l('Reserved Stock'), headerTooltip: this.l('Reserved Stock'), field: 'reservedStock', flex: 1 },
            { headerName: this.l('Lot Code M'), headerTooltip: this.l('Lot Code M'), field: 'lotCodeM', flex: 1 },
            { headerName: this.l('Mrp Group'), headerTooltip: this.l('Mrp Group'), field: 'mrpGroup', flex: 1 },
            { headerName: this.l('Mrp Type'), headerTooltip: this.l('Mrp Type'), field: 'mrpType', flex: 1 },
            { headerName: this.l('Procurement Type'), headerTooltip: this.l('Procurement Type'), field: 'procurementType', flex: 1 },
            { headerName: this.l('Special Procurement'), headerTooltip: this.l('Special Procurement'), field: 'specialProcurement', flex: 1 },
            { headerName: this.l('Production Storage Location'), headerTooltip: this.l('Production Storage Location'), field: 'productionStorageLocation', flex: 1 },
            { headerName: this.l('Repet Manufacturing'), headerTooltip: this.l('Repet Manufacturing'), field: 'repetManufacturing', flex: 1 },
            { headerName: this.l('Rem Profile'), headerTooltip: this.l('Rem Profile'), field: 'remProfile', flex: 1 },
            { headerName: this.l('Do Not Cost'), headerTooltip: this.l('Do Not Cost'), field: 'doNotCost', flex: 1 },
            { headerName: this.l('Variance Key'), headerTooltip: this.l('Variance Key'), field: 'varianceKey', flex: 1 },
            { headerName: this.l('Costing Lot Size'), headerTooltip: this.l('Costing Lot Size'), field: 'costingLotSize', flex: 1 },
            { headerName: this.l('Production Version'), headerTooltip: this.l('Production Version'), field: 'productionVersion', flex: 1 },
            { headerName: this.l('Special Procurement Ctg View'), headerTooltip: this.l('Special Procurement Ctg View'), field: 'specialProcurementCtgView', flex: 1 },
            { headerName: this.l('Valuation Category'), headerTooltip: this.l('Valuation Category'), field: 'valuationCategory', flex: 1 },
            { headerName: this.l('Valuation Type'), headerTooltip: this.l('Valuation Type'), field: 'valuationType', flex: 1 },
            { headerName: this.l('Valuation Class'), headerTooltip: this.l('Valuation Class'), field: 'valuationClass', flex: 1 },
            { headerName: this.l('Price Determination'), headerTooltip: this.l('Price Determination'), field: 'priceDetermination', flex: 1 },
            { headerName: this.l('Price Control'), headerTooltip: this.l('Price Control'), field: 'priceControl', flex: 1 },
            { headerName: this.l('Standard Price'), headerTooltip: this.l('Standard Price'), field: 'standardPrice', flex: 1 },
            { headerName: this.l('Moving Price'), headerTooltip: this.l('Moving Price'), field: 'movingPrice', flex: 1 },
            { headerName: this.l('With Qty Structure'), headerTooltip: this.l('With Qty Structure'), field: 'withQtyStructure', flex: 1 },
            { headerName: this.l('Material Origin'), headerTooltip: this.l('Material Origin'), field: 'materialOrigin', flex: 1 },
            { headerName: this.l('Origin Group'), headerTooltip: this.l('Origin Group'), field: 'originGroup', flex: 1 },
            { headerName: this.l('Authorization Group'), headerTooltip: this.l('Authorization Group'), field: 'authorizationGroup', flex: 1 },
            { headerName: this.l('Mat Src'), headerTooltip: this.l('Mat Src'), field: 'matSrc', flex: 1 },
            {
                headerName: this.l('Effective Date From'), headerTooltip: this.l('Effective Date From'), field: 'effectiveDateFrom', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Effective Date To'), headerTooltip: this.l('Effective Date To'), field: 'effectiveDateTo', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/yyyy')
            },
        ];
        this.vehicleCBUColorColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsColor.pageSize * (this.paginationParamsColor.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Ext Color'), headerTooltip: this.l('Ext Color'), field: 'extColor', flex: 1 },
            { headerName: this.l('Int Color'), headerTooltip: this.l('Int Color'), field: 'intColor', flex: 1 }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsColor = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsVehicleCBU= result;
            console.log("result =", result);
        })
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
        },100)
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getVehicleCBUSearch(
            this.vehicleType,
            this.model,
            this.marketingCode,
            this.katashiki,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowDataColor = [];
                    this.paginationParamsColor.totalCount = result.totalCount;
                    this.paginationParamsColor.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                } else {
                    this.searchDatasColor(result.items[0].id);
                }
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.vehicleType = '';
        this.model = '';
        this.marketingCode = '';
        this.katashiki = '';
        this.paginationParamsColor = { pageNum: 1, pageSize: this._pagesizecolor, totalCount: 0 };
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getVehicleCBUSearch(
            this.vehicleType,
            this.model,
            this.marketingCode,
            this.katashiki,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmVehicleCBUDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_id = selected.id;
            this.paginationParamsColor.pageNum = 1;
            this.paginationParamsColor.skipCount = 0;
            this.searchDatasColor(selected.id);
            this.selectedIdMaterial = selected.id;
        }
        this.selectedRow = Object.assign({}, selected);
        this.selectId = this.selectedRow.id;

    }
    getViewDataMaterial() {
        if (this.selectedIdMaterial != null) {
            this.viewMaterialModal.show(this.selectedIdMaterial);
        }

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getVehicleCBUToExcel(
            this.vehicleType,
            this.model,
            this.marketingCode,
            this.katashiki
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    searchDatasColor(p_id): void {
        this._service.getVehicleCBUColorById(
            p_id,
            '',
            this.paginationParamsColor.skipCount,
            this.paginationParamsColor.pageSize
        )
            .subscribe((result) => {
                this.paginationParamsColor.totalCount = result.totalCount;
                this.rowDataColor = result.items;
                this.paginationParamsColor.totalPage = ceil(result.totalCount / (this.paginationParamsColor.pageSize ?? 0));
            });
    }

    getDatasColor(paginationParamsColor?: PaginationParamsModel) {
        return this._service.getVehicleCBUColorById(
            this.p_id,
            '',
            this.paginationParamsColor.skipCount,
            this.paginationParamsColor.pageSize
        );
    }

    changePageColor(paginationParamsColor) {
        this.paginationParamsColor = paginationParamsColor;
        this.paginationParamsColor.skipCount = (paginationParamsColor.pageNum - 1) * paginationParamsColor.pageSize;
        this.getDatasColor(this.paginationParamsColor).subscribe((result) => {
            this.paginationParamsColor.totalCount = result.totalCount;
            this.rowDataColor = result.items;
            this.paginationParamsColor.totalPage = ceil(result.totalCount / (this.paginationParamsColor.pageSize ?? 0));
        });

        this._pagesizecolor = this.paginationParamsColor.pageSize;
    }


    //

    exportToExcelColor(e): void {
        this.fn.exportLoading(e, true);
        this._service.getVehicleCBUColorToExcel(this.p_id)
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    validate(e) {
        this.fn.exportLoading(e, true);
        this._service.validateCheckingRule()
            .subscribe(() => {
                setTimeout(() => {
                    this.notify.success(this.l('Checking Rule Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    CreateMat() {
        let sufix = "";
        if (this.selectedRow.productionCode) sufix = " + " + this.selectedRow.productionCode;
        
        this.confirmDialog.show("Confirm", "Xác nhận tạo material cho <" +  this.selectedRow.katashiki + sufix + ">?");
    }
    confirmYes() {
        console.log(this.selectedRow.id);
        let body = new UpdateCmmVehicleCBUCreateMaterial();
        body.id = this.selectedRow.id; 
        this.isLoading = true;
        this._service.updateCreateMaterial(body)
        .subscribe(() => {
            this.isLoading = false;
            
            this.selectedRow.createMaterial = "Y"; 
            this.notify.success(this.l('Create Material. Updated Successfully!'));
            // this.reload();
            this.rowData.forEach(x=> {
                if (x.id == this.selectedRow.id)
                    x.createMaterial = "Y";  
            });
        });
    }

    confirmNo() {
        
    }
  
}
