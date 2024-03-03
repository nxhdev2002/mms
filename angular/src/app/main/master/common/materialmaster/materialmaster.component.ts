import { GridApi, GridReadyEvent, SideBarDef } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmMaterialMasterDto, MstCmmMaterialMasterServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { viewMaterialMasterModalComponent } from './view-materialmaster-modal.component';
import { ValidateMaterialMasterModalComponent } from './validate-materialmaster-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './materialmaster.component.html',
})
export class MaterialMasterComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewModalMaterialMaster', { static: true }) ciewModalMaterialMaster: | viewMaterialMasterModalComponent | undefined;
    @ViewChild('validateMaterial', { static: true }) validateMaterial: | ValidateMaterialMasterModalComponent | undefined;
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
    changedRecordsMaterialMaster: number[] = [];
    selectId;
    selectedRow: MstCmmMaterialMasterDto = new MstCmmMaterialMasterDto();
    saveSelectedRow: MstCmmMaterialMasterDto = new MstCmmMaterialMasterDto();
    datas: MstCmmMaterialMasterDto = new MstCmmMaterialMasterDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    materialType: string = '';
    materialCode: string = '';
    description: string = '';
    materialGroup: string = '';
    baseUnitOfMeasure: string = '';
    plant: string = '';
    storageLocation: string = '';
    productionGroup: string = '';
    productionPurpose: string = '';
    productionType: string = '';
    profitCenter: string = '';
    batchManagement: string = '';
    reservedStock: string = '';
    lotCode: string = '';
    mrpGroup: string = '';
    mrpType: string = '';
    specialProcurement: string = '';
    productionStorageLocation: string = '';
    remProfile: string = '';
    varianceKey: string = '';
    productionVersion: string = '';
    specialProcurementCtgView: string = '';
    valuationType: any;
    valuationClass: string = '';
    originGroup: any;
    authorizationGroup: string = '';
    matSrc: string = '';
    industrySector: string = '';
    deletionFlag: string = '';
    procurementType: string = '';
    repetManufacturing: string = '';
    doNotCost: string = '';
    costingLotSize: string = '';
    valuationCategory: string = '';
    materialOrigin: string = '';
    movingPrice: any;
    standardPrice: any;
    effectiveDateFrom: any;
    effectiveDateTo: any;
    priceDetermination: any;
    priceControl: string = '';
    withQtyStructure: string = '';
    isActive: Boolean = false;
    workingDate: any;
    workingDateFrom: any;
    workingDateTo: any;
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters


    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
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
        private _service: MstCmmMaterialMasterServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
                pinned: 'left'
            },
            {
                headerName: this.l('Material Type'),
                headerTooltip: this.l('Material Type'),
                field: 'materialType',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                pinned: 'left'
            },
            {
                headerName: this.l('Material Code'),
                headerTooltip: this.l('Material Code'),
                field: 'materialCode',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                pinned: 'left'

            },
            {
                headerName: this.l('Industry Sector'),
                headerTooltip: this.l('Industry Sector'),
                field: 'industrySector',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Material Group'),
                headerTooltip: this.l('Material Group'),
                field: 'materialGroup',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Base Unit Of Measure'),
                headerTooltip: this.l('Base Unit Of Measure'),
                field: 'baseUnitOfMeasure',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Deletion Flag'),
                headerTooltip: this.l('Deletion Flag'),
                field: 'deletionFlag',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Plant'),
                headerTooltip: this.l('Plant'),
                field: 'plant',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Storage Location'),
                headerTooltip: this.l('Storage Location'),
                field: 'storageLocation',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Production Group'),
                headerTooltip: this.l('Production Group'),
                field: 'productionGroup',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Production Purpose'),
                headerTooltip: this.l('Production Purpose'),
                field: 'productionPurpose',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Production Type'),
                headerTooltip: this.l('Production Type'),
                field: 'productionType',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                width: 120
            },
            {
                headerName: this.l('Profit Center'),
                headerTooltip: this.l('Profit Center'),
                field: 'profitCenter',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Batch Management'),
                headerTooltip: this.l('Batch Management'),
                field: 'batchManagement',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Reserved Stock'),
                headerTooltip: this.l('Reserved Stock'),
                field: 'reservedStock',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Lot Code'),
                headerTooltip: this.l('Lot Code'),
                field: 'lotCode',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Mrp Group'),
                headerTooltip: this.l('Mrp Group'),
                field: 'mrpGroup',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Mrp Type'),
                headerTooltip: this.l('Mrp Type'),
                field: 'mrpType',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Procurement Type'),
                headerTooltip: this.l('Procurement Type'),
                field: 'procurementType',
            },
            {
                headerName: this.l('Special Procurement'),
                headerTooltip: this.l('Special Procurement'),
                field: 'specialProcurement',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Production Storage Location'),
                headerTooltip: this.l('Production Storage Location'),
                field: 'productionStorageLocation',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Repet Manufacturing'),
                headerTooltip: this.l('Repet Manufacturing'),
                field: 'repetManufacturing',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Rem Profile'),
                headerTooltip: this.l('Rem Profile'),
                field: 'remProfile'
            },
            {
                headerName: this.l('Do Not Cost'),
                headerTooltip: this.l('Do Not Cost'),
                field: 'doNotCost',
                cellClass: ['text-center'],
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                width: 120,
            },
            {
                headerName: this.l('Variance Key'),
                headerTooltip: this.l('Variance Key'),
                field: 'varianceKey',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Costing Lot Size'),
                headerTooltip: this.l('Costing Lot Size'),
                field: 'costingLotSize',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Production Version'),
                headerTooltip: this.l('Production Version'),
                field: 'productionVersion',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Special Procurement Ctg View'),
                headerTooltip: this.l('Special Procurement Ctg View'),
                field: 'specialProcurementCtgView',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Valuation Category'),
                headerTooltip: this.l('Valuation Category'),
                field: 'valuationCategory',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Valuation Type'),
                headerTooltip: this.l('Valuation Type'),
                field: 'valuationType',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Valuation Class'),
                headerTooltip: this.l('Valuation Class'),
                field: 'valuationClass',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Price Determination'),
                headerTooltip: this.l('Price Determination'),
                field: 'priceDetermination',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Price Control'),
                headerTooltip: this.l('Price Control'),
                field: 'priceControl',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Standard Price'),
                headerTooltip: this.l('Standard Price'),
                field: 'standardPrice',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Moving Price'),
                headerTooltip: this.l('Moving Price'),
                field: 'movingPrice',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('With Qty Structure'),
                headerTooltip: this.l('With Qty Structure'),
                field: 'withQtyStructure',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Material Origin'),
                headerTooltip: this.l('Material Origin'),
                field: 'materialOrigin',
                cellClass: ['text-center'],
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                width: 120
            },
            {
                headerName: this.l('Origin Group'),
                headerTooltip: this.l('Origin Group'),
                field: 'originGroup',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
                width: 120
            },
            {
                headerName: this.l('Authorization Group'),
                headerTooltip: this.l('Authorization Group'),
                field: 'authorizationGroup',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Mat Src'),
                headerTooltip: this.l('Mat Src'),
                field: 'matSrc',
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Effective Date From'),
                headerTooltip: this.l('Effective Date From'),
                field: 'effectiveDateFrom', valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/yyyy'),
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            {
                headerName: this.l('Effective Date To'),
                headerTooltip: this.l('Effective Date To'),
                field: 'effectiveDateTo', valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/yyyy'),
                enableRowGroup: true, enablePivot: true, enableValue: true ,
            },
            // {
            //     headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            // }
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
            this.changedRecordsMaterialMaster = result;
            console.log("result =", result);
        })
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.materialType,
            this.materialGroup,
            this.valuationType,
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

    clearTextSearch() {
        this.materialType = '',
            this.materialGroup = '',
            this.valuationType = '',
            this.searchDatas();
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.materialType,
            this.materialGroup,
            this.valuationType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmMaterialMasterDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmMaterialMasterDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
        console.log(this.selectedRow);

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;

        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
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

        this._service.getMaterialMasterToExcel(
            this.materialType,
            this.materialGroup,
            this.valuationType,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    validate(e){
        this.fn.exportLoading(e, true);
        this._service.validateCheckingRule().subscribe(() => {
            setTimeout(() => {
                this.notify.success(this.l('Checking Rule Successfully'));
            }, this.fn.exportLoading(e));
        });
    }


}
