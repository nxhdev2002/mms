import {
    GridApi,
    MasterDetailModule,
    ModuleRegistry,
    SideBarDef,
} from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    GridParams,
    PaginationParamsModel,
    FrameworkComponent,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    MstCmmGradeColorDetailDto,
    MstCmmLotCodeGradeDto,
    MstCmmLotCodeGradeServiceProxy,
    MstCmmLotCodeGradeTDto,
    MstCmmMaterialMasterServiceProxy,
    MstCmmModelDto,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CreateOrEditVehicleModalComponent } from './create-or-edit-vehicle-modal.component';
import { ViewVehicleDetailModalComponent } from './view-vehicle-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
ModuleRegistry.registerModules([MasterDetailModule]);
import { CommonFunction } from '@app/main/commonfuncton.component';
import { Console } from 'console';

@Component({
    templateUrl: './vehicle.component.html',
})

export class VehicleComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('createOrEditVehicle', { static: true }) createOrEditVehicle: CreateOrEditVehicleModalComponent;
    @ViewChild('viewVehicleDetail', { static: true }) viewVehicleDetail: | ViewVehicleDetailModalComponent | undefined;
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal: | ViewMaterialByIdModalComponent | undefined;

    columnDefs: CustomColDef[] = [];
    detailCellRendererParams: any;
    paginationParamModels: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamLots: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    //
    gradeColDefs: any;
    colorGradeColDefs: any;


    selectedRow: MstCmmLotCodeGradeTDto = new MstCmmLotCodeGradeTDto();
    saveSelectedRow: MstCmmLotCodeGradeTDto = new MstCmmLotCodeGradeTDto();
    modelDto: MstCmmModelDto = new MstCmmModelDto();
    lotCodeGradeDto: MstCmmLotCodeGradeDto = new MstCmmLotCodeGradeDto();
    datas: MstCmmModelDto = new MstCmmModelDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    dataParamsLotCodeGrade: GridParams | undefined;
    dataParamsColor: GridParams | undefined;
    rowDataModel: any[] = [];
    rowDataLotCodeGrade: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    rowColor: MstCmmGradeColorDetailDto = new MstCmmGradeColorDetailDto();

    code: string = '';
    name: string = '';
    cfc: string = '';
    grade: string = '';
    modelCode: string = '';
    materialCode: string = '';
    productionGroup: string = '';
    valuationType: string = '';
    modelVin: string = '';
    isActive: string = '';
    model: string = '';
    selectGradeId;
    katashiki;
    CfcFilter;
    gradeId;
    selectedIdMaterial;
    sideBar: SideBarDef | string | string[] | boolean | null = { toolPanels: ['columns', 'filters'] } // ['columns','filters'] //filters

    lotCode: string = '';
    gradeName;
    idLine: string = '';
    spec200: string = '';
    ssNo: string = '';
    katashikiCtl: string = '';
    vehNameCd: string = '';
    marLotCode: string = '';
    testNo: string = '';
    vehicleId: string = '';
    prodSfx: string = '';
    salesSfx: string = '';
    brand: string = '';
    carSeries: string = '';
    transmissionType: string = '';
    engineType: string = '';
    fuelType: string = '';
    goshiCar: string = '';
    changedRecordsGradeColor: number[] =[];
    changedRecordsGradeColorDetail: number[] =[];
    selectId;
    defaultColDef = {
        enableValue: true,
        enableRowGroup: true,
        enablePivot: true,
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
        private _service: MstCmmLotCodeGradeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _service_validate: MstCmmMaterialMasterServiceProxy,
    ) {
        super(injector);

        this.gradeColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamModels.pageSize * (this.paginationParamModels.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
                pinned: 'left'
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
                pinned: 'left'
            },
            {
                headerName: this.l('Lotcode'),
                headerTooltip: this.l('Lotcode'),
                field: 'lotCode',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
                pinned: 'left'
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
                pinned: 'left'
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
                pinned: 'left'
            },
            {
                headerName: this.l('Grade Name'),
                headerTooltip: this.l('Grade Name'),
                field: 'gradeName',
                flex: 1,
                cellRenderer: 'agGroupCellRenderer',
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Model Code'),
                headerTooltip: this.l('Model Code'),
                field: 'modelCode',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Model Vin'),
                headerTooltip: this.l('Model Vin'),
                field: 'modelVin',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            // {
            //     headerName: this.l('Id Line'),
            //     headerTooltip: this.l('Id Line'),
            //     field: 'idLine',
            //     flex: 1,
            // },
            {
                headerName: this.l('Spec 200'),
                headerTooltip: this.l('Spec 200'),
                field: 'spec200',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Ss No'),
                headerTooltip: this.l('Ss No'),
                field: 'ssNo',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Katashiki Ctl'),
                headerTooltip: this.l('Katashiki Ctl'),
                field: 'katashikiCtl',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Veh Name Cd'),
                headerTooltip: this.l('Veh Name Cd'),
                field: 'vehNameCd',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Katashiki'),
                headerTooltip: this.l('Katashiki'),
                field: 'katashiki',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Mar Lot Code'),
                headerTooltip: this.l('Mar Lot Code'),
                field: 'marLotCode',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Test No'),
                headerTooltip: this.l('Test No'),
                field: 'testNo',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            // {
            //     headerName: this.l('Vehicle Id'),
            //     headerTooltip: this.l('Vehicle Id'),
            //     field: 'vehicleId',
            //     flex: 1,
            // },
            {
                headerName: this.l('Prod Sfx'),
                headerTooltip: this.l('Prod Sfx'),
                field: 'prodSfx',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Sales Sfx'),
                headerTooltip: this.l('Sales Sfx'),
                field: 'salesSfx',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Brand'),
                headerTooltip: this.l('Brand'),
                field: 'brand',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Car Series'),
                headerTooltip: this.l('Car Series'),
                field: 'carSeries',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Transmission Type'),
                headerTooltip: this.l('Transmission Type'),
                field: 'transmissionType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Engine Type'),
                headerTooltip: this.l('Engine Type'),
                field: 'engineType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Fuel Type'),
                headerTooltip: this.l('Fuel Type'),
                field: 'fuelType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Goshi Car'),
                headerTooltip: this.l('Goshi Car'),
                field: 'goshiCar',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Material Type'),
                headerTooltip: this.l('Material Type'),
                field: 'materialType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Material Code'),
                headerTooltip: this.l('Material Code'),
                field: 'materialCode',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Industry Sector'),
                headerTooltip: this.l('Industry Sector'),
                field: 'industrySector',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Material Group'),
                headerTooltip: this.l('Material Group'),
                field: 'materialGroup',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Base Unit Of Measure'),
                headerTooltip: this.l('Base Unit Of Measure'),
                field: 'baseUnitOfMeasure',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Deletion Flag'),
                headerTooltip: this.l('Deletion Flag'),
                field: 'deletionFlag',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Plant'),
                headerTooltip: this.l('Plant'),
                field: 'plant',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('StorageLocation'),
                headerTooltip: this.l('StorageLocation'),
                field: 'storageLocation',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Production Group'),
                headerTooltip: this.l('Production Group'),
                field: 'productionGroup',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Production Purpose'),
                headerTooltip: this.l('Production Purpose'),
                field: 'productionPurpose',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Production Type'),
                headerTooltip: this.l('Production Type'),
                field: 'productionType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Profit Center'),
                headerTooltip: this.l('Profit Center'),
                field: 'profitCenter',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Batch Management'),
                headerTooltip: this.l('Batch Management'),
                field: 'batchManagement',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Reserved Stock'),
                headerTooltip: this.l('Reserved Stock'),
                field: 'reservedStock',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Lot Code M'),
                headerTooltip: this.l('Lot Code M'),
                field: 'lotCodeM',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Mrp Group'),
                headerTooltip: this.l('Mrp Group'),
                field: 'mrpGroup',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Mrp Type'),
                headerTooltip: this.l('Mrp Type'),
                field: 'mrpType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },

            {
                headerName: this.l('Procurement Type'),
                headerTooltip: this.l('Procurement Type'),
                field: 'procurementType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Special Procurement'),
                headerTooltip: this.l('Special Procurement'),
                field: 'specialProcurement',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Production Storage Location'),
                headerTooltip: this.l('Production Storage Location'),
                field: 'productionStorageLocation',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Repet Manufacturing'),
                headerTooltip: this.l('Repet Manufacturing'),
                field: 'repetManufacturing',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Rem Profile'),
                headerTooltip: this.l('Rem Profile'),
                field: 'remProfile',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Do Not Cost'),
                headerTooltip: this.l('Do Not Cost'),
                field: 'doNotCost',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Variance Key'),
                headerTooltip: this.l('Variance Key'),
                field: 'varianceKey',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Costing Lot Size'),
                headerTooltip: this.l('Costing Lot Size'),
                field: 'costingLotSize',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Production Version'),
                headerTooltip: this.l('Production Version'),
                field: 'productionVersion',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Special Procurement Ctg View'),
                headerTooltip: this.l('Special Procurement Ctg View'),
                field: 'specialProcurementCtgView',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Valuation Category'),
                headerTooltip: this.l('Valuation Category'),
                field: 'valuationCategory',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Valuation Type'),
                headerTooltip: this.l('Valuation Type'),
                field: 'valuationType',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Valuation Class'),
                headerTooltip: this.l('Valuation Class'),
                field: 'valuationClass',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Price Determination'),
                headerTooltip: this.l('Price Determination'),
                field: 'priceDetermination',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Price Control'),
                headerTooltip: this.l('Price Control'),
                field: 'priceControl',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Standard Price'),
                headerTooltip: this.l('Standard Price'),
                field: 'standardPrice',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Moving Price'),
                headerTooltip: this.l('Moving Price'),
                field: 'movingPrice',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('With Qty Structure'),
                headerTooltip: this.l('With Qty Structure'),
                field: 'withQtyStructure',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Material Origin'),
                headerTooltip: this.l('Material Origin'),
                field: 'materialOrigin',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Origin Group'),
                headerTooltip: this.l('Origin Group'),
                field: 'originGroup',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Authorization Group'),
                headerTooltip: this.l('Authorization Group'),
                field: 'authorizationGroup',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('Mat Src'),
                headerTooltip: this.l('Mat Src'),
                field: 'matSrc',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('EffectiveDate From'),
                headerTooltip: this.l('EffectiveDate From'),
                field: 'effectiveDateFrom',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },
            {
                headerName: this.l('EffectiveDate To'),
                headerTooltip: this.l('EffectiveDate To'),
                field: 'effectiveDateTo',
                flex: 1,
                enablePivot: true,
                enableRowGroup: true,
                enableValue: true,
            },


        ];
        this.colorGradeColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamModels.pageSize * (this.paginationParamModels.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                flex: 1,
            },
            {
                headerName: this.l('Color Type'),
                headerTooltip: this.l('Color Type'),
                field: 'colorType',
                flex: 1,
            },
            {
                headerName: this.l('Name Color Type'),
                headerTooltip: this.l('Name Color  Type'),
                field: 'nameColorType',
                flex: 1,
            },

            {
                headerName: this.l('Name En'),
                headerTooltip: this.l('Name En'),
                field: 'nameEn',
                flex: 1,
            },
            {
                headerName: this.l('Name Vn'),
                headerTooltip: this.l('Name Vn'),
                field: 'nameVn',
                flex: 1,
            },
        ],
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParamModels = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamLots = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }
    // fetchChangedRecords() {
    //     this._service.getChangedRecords().subscribe((result) => {
    //         this.changedRecordsGradeColor = result;
    //         console.log("result =", result);
    //     })
    // }
    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsGradeColor = result.gradeColor;
            this.changedRecordsGradeColorDetail = result.gradeColorDetail;
            // console.log("result =", result);

        })
    }
    autoSizeAll() {
        const allColumnIds: string[] = [];
        const allColumnIds2: string[] = [];
        this.dataParamsLotCodeGrade.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });

        this.dataParamsLotCodeGrade.columnApi!.autoSizeColumns(allColumnIds);
        this.resetGridView();
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParamsLotCodeGrade.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },100)
    }



    searchDatas(): void {
        this.isLoading=true;
        this._service
            .getAllGradeColor(
                this.cfc,
                this.modelVin,
                this.modelCode,
                this.materialCode,
                this.productionGroup,
                this.valuationType,
                '',
                this.paginationParamModels.skipCount,
                this.paginationParamModels.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsLotCodeGrade?.api)))
            .subscribe((result) => {
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items;
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                if (result.totalCount == 0) {
                    this.rowDataLotCodeGrade = [];
                    this.paginationParamLots.totalCount = result.totalCount;
                    this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                }
                this.isLoading=false;
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.cfc = '',
            this.modelCode = '',
            this.modelVin = '',
            this.materialCode = '',
            this.productionGroup = '',
            this.valuationType = '',
            this.isActive = '',
            this.searchDatas();
    }


    getDataGradeColors(paginationParams?: PaginationParamsModel) {
        return this._service.getAllGradeColor(
            this.cfc,
            this.modelCode,
            this.modelVin,
            this.materialCode,
            this.productionGroup,
            this.valuationType,
            '',
            this.paginationParamModels.skipCount,
            this.paginationParamModels.pageSize
        );
    }

    onChangeRowSelectionModel(params: { api: { getSelectedRows: () => MstCmmLotCodeGradeTDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        
        if (selected) {
            this.searchDataLotCodeGrades(selected.id);
            this.selectGradeId = selected.id;
            this.selectedIdMaterial = selected.id;
        }
        else {
            this.rowDataLotCodeGrade = [];
        }
        this.selectedRow = Object.assign({}, selected);
        this.selectId = this.selectedRow.id;
        console.log("Id = ", this.selectId);
    }

    changePageModel(paginationParams) {
        this.isLoading = true;
        this.paginationParamModels = paginationParams;
        this.paginationParamModels.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataGradeColors(this.paginationParamModels).subscribe((result) => {
            this.paginationParamModels.totalCount = result.totalCount;
            this.rowDataModel = result.items;
            this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
        });
    }

    callBackDataGridModel(params: GridParams) {
        this.isLoading = true;
        this.dataParamsLotCodeGrade = params;
        params.api.paginationSetPageSize(this.paginationParamModels.pageSize);
        this.paginationParamModels.skipCount =
            ((this.paginationParamModels.pageNum ?? 1) - 1) * (this.paginationParamModels.pageSize ?? 0);
        this.paginationParamModels.pageSize = this.paginationParamModels.pageSize;
        this.getDataGradeColors(this.paginationParamModels)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsLotCodeGrade?.api)))
            .subscribe((result) => {
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items ?? [];
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                this.isLoading = false;
                this.resetGridView();
            });
    }


    //lotCodeGrade
    searchDataLotCodeGrades(gradeId): void {
        this._service.getAllGradeColorDetail(
            gradeId,
            '',
            this.paginationParamLots.skipCount,
            this.paginationParamLots.pageSize
        )
            .pipe(finalize(() => { }
            )).subscribe((result) => {
                this.paginationParamLots.totalCount = result.totalCount;
                this.rowDataLotCodeGrade = result.items;
                this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
            });
    }

    getDataGradeColorDetailGrades(paginationParams?: PaginationParamsModel) {
        this.isLoading = true;
        return this._service.getAllGradeColorDetail(
            this.gradeId,
            '',
            this.paginationParamLots.skipCount,
            this.paginationParamLots.pageSize
        );
    }

    onChangeRowSelectionLotCodeGrade(params: { api: { getSelectedRows: () => MstCmmLotCodeGradeTDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmLotCodeGradeTDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.gradeId = this.selectedRow.id;
        console.log("IdDetail = "+ this.gradeId);
    }

    changePageLotCodeGrade(paginationParams) {
        this.isLoading = true;
        this.paginationParamLots = paginationParams;
        this.paginationParamLots.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataGradeColorDetailGrades(this.paginationParamLots).subscribe((result) => {
            this.paginationParamLots.totalCount = result.totalCount;
            this.rowDataLotCodeGrade = result.items;
            this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGridLotCodeGrade(params: GridParams) {
        this.isLoading = true;
        this.dataParamsColor = params;
        params.api.paginationSetPageSize(this.paginationParamLots.pageSize);
        this.paginationParamLots.skipCount =
            ((this.paginationParamLots.pageNum ?? 1) - 1) * (this.paginationParamLots.pageSize ?? 0);
        this.paginationParamLots.pageSize = this.paginationParamLots.pageSize;
        this.getDataGradeColorDetailGrades(this.paginationParamLots)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParamsColor?.api)))
            .subscribe((result) => {
                this.paginationParamLots.totalCount = result.totalCount;
                this.rowDataLotCodeGrade = result.items ?? [];
                this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    createOrEditColors() {
        this.createOrEditVehicle.show(this.selectGradeId);
    }


    getViewMaterial() {
        if (this.selectedIdMaterial != null) {
            this.viewMaterialModal.show(this.selectedIdMaterial);
        }

    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getGradeColorToExcel(
            this.cfc,
            this.modelCode,
            this.modelVin,
            this.materialCode,
            this.productionGroup,
            this.valuationType,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    validate(){
        this.isLoading = true;
        this._service_validate.validateCheckingRule().subscribe(() => {
            setTimeout(() => {  
                this.notify.success(this.l('Check The Rule Successfully'));
            }, 1600)
            this.isLoading = false;                         
        });
    }

}

