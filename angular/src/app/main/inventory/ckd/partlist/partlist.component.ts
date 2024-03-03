import { style } from '@angular/animations';
import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy, InvPartPackingDetailDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportInvCkdPartListComponent } from './import-partlist.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './partlist.component.html',
    styleUrls: ['./partlist.component.less'],
})
export class CkdPartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportInvCkdPartListComponent | undefined;
    @ViewChild('addPartListModal', { static: true }) addPartListModal: | InvCkdPartListDto
        | undefined;
    defaultColDefs: CustomColDef[] = [];
    partGradeColDef: CustomColDef[] = [];
    partPackingDetailColDef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationPackingParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    checkTab = 'B';
    changedRecordsPartList: number[] = [];
    changedRecordsPartGrade: number[] = [];
    changeRecordsPartPacking: number[] = [];
    partGradeId;
    selectedRow: InvCkdPartListDto = new InvCkdPartListDto();
    selectedRowGrade: InvCkdPartGradeDto = new InvCkdPartGradeDto();
    saveSelectedRow: InvCkdPartListDto = new InvCkdPartListDto();
    datas: InvCkdPartListDto = new InvCkdPartListDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    dataParamsGrade: GridParams | undefined;
    dataParamsPacking: GridParams | undefined;
    originalRowData: any[] = [];
    rowData: any[] = [];
    rowDataPartGrade: any[] = [];
    rowDataPartPackingDetail: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponentsPartList: FrameworkComponent;
    frameworkComponents: FrameworkComponent;
    cfc;
    selectPartNo;
    selectPartId;
    partNo: string = '';
    partId: any;
    partNoNormalized: string = '';
    partName: string = '';
    supplierNo: string = '';
    supplierCd: string = '';
    colorSfx: string = '';
    model: string = '';
    grade: string = '';
    shop: string = '';
    p_partNo
    supplierId
    materialId
    orderPattern
    orderPatternList = [
        { value: '', label: "Mix" },
        { value: 'LOT', label: "LOT" },
        { value: 'PXP', label: "PXP" },
    ];
    cfcActiveList = [
        { value: '', label: "All" },
        { value: 'Y', label: "Active" },
        { value: 'N', label: "No Active" },

    ];
    isActive = 'Y';
    isRowGrade: boolean = false;
    isEci: boolean = true;
    //isActive: string = '';
    groupDefaultExpanded: number = 1;
    selectRowPacking: InvPartPackingDetailDto = new InvPartPackingDetailDto();;

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

    orderPatern: string;
    listCfcAcitve: any[];
    checkClone: boolean = true;
    checkEdit: boolean = true;
    PartPackingId;
    isSelectPacking: boolean = false;
    constructor(
        injector: Injector,
        private _service: InvCkdPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        // this.defaultColDefs = [
        //     //{headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
        //     {
        //         headerName: this.l('Cfc'),
        //         width: 80,
        //         headerTooltip: this.l('Cfc'),
        //         field: 'cfc',
        //         rowGroup: true,
        //         hide: true,
        //         cellClass: ['cell-border', 'cell-readonly',],
        //         valueGetter: (params: ValueGetterParams) => params.data.cfc ? 'Cfc : ' + params.data.cfc : this.l('Cfc : '),
        //     },
        //     { headerName: this.l('Part No'), flex: 1, headerTooltip: this.l('Part No'), field: 'partNo' },
        //     { headerName: this.l('Supplier No'), flex: 0.8, headerTooltip: this.l('Supplier No'), field: 'supplierNo' },
        //     { headerName: this.l('Part No Normalized'), flex: 1, headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized' },
        //     { headerName: this.l('Part No Normalized S4'), flex: 1, headerTooltip: this.l('Part No Normalized S4'), field: 'partNoNormalizedS4' },
        //     { headerName: this.l('Part Name'), flex: 2, headerTooltip: this.l('Part Name'), field: 'partName', cellRenderer: (params) => params.data?.partName.toUpperCase() },
        //     //{headerName: this.l('Supplier Cd'),flex:1,headerTooltip: this.l('Supplier Cd'),field: 'supplierCd'},
        //     { headerName: this.l('Color Sfx'), flex: 0.6, headerTooltip: this.l('Color Sfx'), field: 'colorSfx' },
        //     { headerName: this.l('OrderPattern'), flex: 0.6, headerTooltip: this.l('OrderPattern'), field: 'orderPattern' },
        // ];


        this.partGradeColDef = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationGradeParams.pageSize * (this.paginationGradeParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Model'), flex: 1, headerTooltip: this.l('Model'), field: 'model' },
            { headerName: this.l('Shop'), flex: 1, headerTooltip: this.l('Shop'), field: 'shop' },
            { headerName: this.l('Part No'), flex: 2, headerTooltip: this.l('Part No'), field: 'partNo' },
            { headerName: this.l('Part Name'), flex: 2, headerTooltip: this.l('Part Name'), field: 'partName', cellRenderer: (params) => params.data?.partName.toUpperCase() },
            { headerName: this.l('Exporter'), flex: 0.8, headerTooltip: this.l('Exporter'), field: 'supplierNo' },
            { headerName: this.l('Body Color'), flex: 1, headerTooltip: this.l('Body Color'), field: 'bodyColor' },
            { headerName: this.l('Box size'), flex: 1, headerTooltip: this.l('Box size'), field: 'boxsize' },
            { headerName: this.l('Grade'), flex: 1, headerTooltip: this.l('Grade'), field: 'grade' },
            { headerName: this.l('Usage Qty'), flex: 1, headerTooltip: this.l('Usage Qty'), field: 'usageQty', type: 'rightAligned' },
            { headerName: this.l('Start Lot'), flex: 1, headerTooltip: this.l('Start Lot'), field: 'startLot' },
            { headerName: this.l('Start no in Lot'), flex: 1, headerTooltip: this.l('Start no in Lot'), field: 'startRun' },
            { headerName: this.l('End Lot'), flex: 1, headerTooltip: this.l('End Lot'), field: 'endLot' },
            { headerName: this.l('End no in Lot'), flex: 1, headerTooltip: this.l('End no in Lot'), field: 'endRun' },
            { headerName: this.l('ECI from Part'), flex: 1, headerTooltip: this.l('ECI from Part'), field: 'eciFromPart' },
            { headerName: this.l('ECI to Part'), flex: 1, headerTooltip: this.l('ECI to Part'), field: 'eciToPart' },
            { headerName: this.l('Order Pattern'), flex: 1, headerTooltip: this.l('Order Pattern'), field: 'orderPattern' },
            { headerName: this.l('Update date'), flex: 1, headerTooltip: this.l('Update date'), field: 'userUpdate', valueGetter: (params) => this.pipe.transform(params.data?.userUpdate, 'dd-MM-yyyy') },
            { headerName: this.l('Update user'), flex: 1, headerTooltip: this.l('Update user'), field: 'userName' },

        ];

        this.partPackingDetailColDef = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationPackingParams.pageSize * (this.paginationPackingParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo',flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc',flex: 1 },
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade',flex: 1 },
            { headerName: this.l('Back No'), headerTooltip: this.l('Back No'), field: 'backNo',flex: 1 },
            { headerName: this.l('Module No'), headerTooltip: this.l('Module No'), field: 'moduleNo',flex: 1 },
            { headerName: this.l('Renban'), headerTooltip: this.l('Renban'), field: 'renban',flex: 1 },
            { headerName: this.l('Box Size'), headerTooltip: this.l('Box Size'), field: 'boxSize',flex: 1 },
            { headerName: this.l('Type'), headerTooltip: this.l('Type'), field: 'type',flex: 1 },
            { headerName: this.l('Common'), headerTooltip: this.l('Common'), field: 'common',flex: 1 },
            { headerName: this.l('Ico Flag'), headerTooltip: this.l('Ico Flag'), field: 'icoFlag',flex: 1 },
            { headerName: this.l('Re Export Cd'), headerTooltip: this.l('Re Export Cd'), field: 'reExportCd',flex: 1 },
            { headerName: this.l('Start Packing Month'), headerTooltip: this.l('Start Packing Month'), field: 'startPackingMonth', valueGetter: (params) => this.pipe.transform(params.data?.startPackingMonth, 'dd-MM-yyyy'),flex: 1 },
            { headerName: this.l('End Packing Month'), headerTooltip: this.l('End Packing Month'), field: 'endPackingMonth',flex: 1 },
            { headerName: this.l('Start Production Month'), headerTooltip: this.l('Start Production Month'), field: 'startProductionMonth', valueGetter: (params) => this.pipe.transform(params.data?.startProductionMonth, 'dd-MM-yyyy'),flex: 1 },
            { headerName: this.l('End Production Month'), headerTooltip: this.l('End Production Month'), field: 'endProductionMonth', flex: 1 },
        ]
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        this.paginationGradeParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationPackingParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
        this._service.getListCfcs().subscribe(res => {
            this.listCfcAcitve = res;
        })
    }

    renderGridByHoang() {
        /// set coldef
        this.defaultColDefs = [
            {
                headerName: this.l('Cfc'),
                width: 80,
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.cfc ? 'Cfc : ' + params.data.cfc : this.l('Cfc : '),
            },

            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', width: 60 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', width: 100 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', width: 100, cellRenderer: (params) => params.data?.partName.toUpperCase() },
            { headerName: this.l('Exporter'), headerTooltip: this.l('Exporter'), field: 'supplierNo', width: 80 },
            { headerName: this.l('Box Size'), headerTooltip: this.l('Box Size'), field: 'boxSize', width: 80 },
            { headerName: this.l('Body Color'), headerTooltip: this.l('Body Color'), field: 'bodyColor', width: 100 },
            // { headerName: this.l('Part No Normalized'),  headerTooltip: this.l('Part No Normalized'), field: 'partNoNormalized',width: 100 },
            // { headerName: this.l('Part No Normalized S4'),  headerTooltip: this.l('Part No Normalized S4'), field: 'partNoNormalizedS4',width: 100 },

            //     //{headerName: this.l('Supplier Cd'),flex:1,headerTooltip: this.l('Supplier Cd'),field: 'supplierCd'},
            // { headerName: this.l('Color Sfx'),  headerTooltip: this.l('Color Sfx'), field: 'colorSfx',width: 100 },
            { headerName: this.l('OrderPattern'), headerTooltip: this.l('OrderPattern'), field: 'orderPattern', width: 80 },
        ]



        let listGrade = Array.from(new Set(this.rowData.map((item: any) => item.grade)))
        const distinctKeys = new Set();

        this.rowData.forEach(obj => {
            Object.keys(obj).forEach(key => {
                if (key.length == 2 && !distinctKeys.has(key) && key != 'id') {
                    distinctKeys.add(key.toUpperCase());
                }
            });
        });

        listGrade = listGrade.concat(Array.from(distinctKeys));
        listGrade = [...new Set(listGrade)]

        listGrade.forEach(col => {
            if (col != null && col.length > 0)
                this.defaultColDefs.push(
                    { headerName: this.l(col), headerTooltip: this.l(col), field: `${col.toLowerCase()}`, flex: 1 },
                )
        })

        this.dataParams.api!.setColumnDefs(this.defaultColDefs);
        /// nhét đống data vào
        let rsData = []
        /// các phần tử trong rowData có partNo trùng nhau và khác grade thì gom lại thành 1
        /// với các grade khác nhau thì tạo ra các cột mới
        this.rowData.forEach(item => {
            let index = rsData.findIndex(x => x.partNo == item.partNo && x.cfc == item.cfc && x.supplierNo == item.supplierNo);
            if (index == -1) {
                if (item.qty != null)
                    item[item.grade.toLowerCase()] = Number(item.qty);
                rsData.push(item);
            } else {
                let grade = item.grade;
                let qty = item.qty;
                if (grade != null && qty != null) {
                    rsData[index][grade.toLowerCase()] = Number(item.qty);
                }

            }
        })
        this.rowData = rsData;

        if (this.originalRowData.length == 0) {
            this.originalRowData = rsData;
            this.paginationParams.totalCount = rsData.length;
        }



        // this.paginationParams.totalPage = ceil(rsData.length / (this.paginationParams.pageSize ?? 0));
        // this.resetGridView();
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParamsGrade.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsGrade.columnApi!.autoSizeColumns(allColumnIds);
    }

    autoSizeAllPartList() {

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
            this.dataParamsGrade.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            // this.dataParams.columnApi!.sizeColumnsToFit({
            //     suppressColumnVirtualisation: true,
            // });
            this.autoSizeAll();
            this.autoSizeAllPartList();
        }, 100)
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsPartList = result.partList;
            this.changedRecordsPartGrade = result.partGrade;
            this.changeRecordsPartPacking = result.partPacking;
        })
    }

    reloadSearch(_partNo, _cfc, _partlistId) {
        this.partNo = _partNo;
        this.cfc = _cfc;
        this.searchDatas();
        if (_partlistId != null) this.searchDatasGrade(_partlistId);
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                if (result.totalCount == 0) {
                    this.rowData = [];
                    this.rowDataPartGrade = [];
                    this.rowDataPartPackingDetail = [];
                    this.paginationGradeParams.totalCount = result.totalCount;
                    this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
                    this.isRowGrade = false;
                    this.isEci = true;
                    this.isLoading = false;
                }
                else {
                    //this.searchDatasPacking(result.items[0].partId);
                    this.originalRowData = [];
                    this.paginationParams.totalCount = result.totalCount;
                    this.rowData = result.items;
                    this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                    this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
                    this.rowDataPartGrade = [];
                    this.rowDataPartPackingDetail = [];
                    this.isRowGrade = false;
                    this.isEci = true;
                    this.isLoading = false;
                    this.renderGridByHoang();
                }
            });
    }


    clearTextSearch() {
        this.rowData = [];
        this.originalRowData = [];
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        this.partNo = '',
            this.cfc = '',
            this.model = '',
            this.grade = '',
            this.shop = '',
            this.supplierNo = '',
            this.orderPattern = '';
        this.isActive = 'Y';
        this.checkClone = false;
        this.checkEdit = false;
        this.isRowGrade = false;
        this.isEci = true;
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    getDatasGrade(paginationParams?: PaginationParamsModel) {
        return this._service.getPartGrade(
            this.selectPartId,
            '',
            this.paginationGradeParams.skipCount,
            this.paginationGradeParams.pageSize
        );
    }

    searchDatasGrade(p_partid): void {
        this.isLoading = true;
        this._service.getPartGrade(
            p_partid,
            '',
            this.paginationGradeParams.skipCount,
            this.paginationGradeParams.pageSize
        )
            .subscribe((result) => {
                if (result.totalCount != 0) {
                    this.paginationGradeParams.totalCount = result.totalCount;
                    this.rowDataPartGrade = result.items;
                    this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
                    this.isLoading = false;
                    this.isRowGrade = false;
                    this.isEci = true;
                    //  this.resetGridView();
                }
                else {
                    this.rowDataPartGrade = [];
                    this.isLoading = false;
                    this.isRowGrade = false;
                    this.isEci = true;
                }
            });
    }

    searchDatasPacking(p_partid,  packingId): void {
        this.isLoading = true;
        this._service.getPartPackingDetail(
            p_partid,
            packingId,
            '',
            this.paginationPackingParams.skipCount,
            this.paginationPackingParams.pageSize
        )
            .subscribe((result) => {
                if (result.totalCount != 0) {
                    this.paginationPackingParams.totalCount = result.totalCount;
                    this.rowDataPartPackingDetail = result.items;
                    this.paginationPackingParams.totalPage = ceil(result.totalCount / (this.paginationPackingParams.pageSize ?? 0));
                    this.isRowGrade = false;
                    this.isEci = true;
                    this.isLoading = false;
                }
                else {
                    this.rowDataPartPackingDetail = [];
                    this.isRowGrade = false;
                    this.isEci = true;
                    this.isLoading = false;
                }
            });
    }
    getDatasPacking(paginationParams?: PaginationParamsModel) {
        return this._service.getPartPackingDetail(
            this.selectPartId,
            this.orderPatern,
            '',
            this.paginationPackingParams.skipCount,
            this.paginationPackingParams.pageSize
        )
    }
    changePagePacking(paginationPackingParams) {
        this.paginationPackingParams = paginationPackingParams;
        this.paginationPackingParams.skipCount = (paginationPackingParams.pageNum - 1) * paginationPackingParams.pageSize;
        this.getDatasPacking(this.paginationPackingParams).subscribe((result) => {
            this.paginationPackingParams.totalCount = result.totalCount;
            this.rowDataPartPackingDetail = result.items;
            this.paginationPackingParams.totalPage = ceil(result.totalCount / (this.paginationPackingParams.pageSize ?? 0));
            this.isRowGrade = false;
            this.isEci = true;
            this.isLoading = false;
        });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPartListDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.orderPatern = selected.orderPattern;
            this.selectPartId = selected.partId != null ? selected.partId : 0;
            this.searchDatasGrade(this.selectPartId);
            this.searchDatasPacking(this.selectPartId, this.orderPatern);
            this.checkEdit = false;
            var cfcExist = (this.listCfcAcitve.filter(s => s.cfc == selected.cfc)).length;
            this.checkClone = cfcExist > 0 ? false : true;
            this.isRowGrade = false;
            this.isEci = true;

            console.log(selected);
        }
        this.selectedRow = Object.assign({}, selected);

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;

        this.rowData = this.originalRowData.slice(this.paginationParams.skipCount, Number(this.paginationParams.pageSize) + this.paginationParams.skipCount)
        this.renderGridByHoang();
        this.isLoading = false;
        // this.getDatas(this.paginationParams).subscribe((result) => {
        //     this.paginationParams.totalCount = result.totalCount;
        //     this.rowData = result.items;
        //     this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        //     this.isRowGrade = false;
        //     this.isEci = true;
        //     this.isLoading = false;
        //     this.renderGridByHoang();
        // });
    }
    changePageGrade(paginationParams) {
        this.paginationGradeParams = paginationParams;
        this.paginationGradeParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasGrade(this.paginationGradeParams).subscribe((result) => {
            this.paginationGradeParams.totalCount = result.totalCount;
            this.rowDataPartGrade = result.items;
            this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
            this.isRowGrade = false;
            // this.resetGridView();
            this.isEci = true;
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
            .subscribe((result) => {
                this.originalRowData = [];
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isRowGrade = false;
                this.isEci = true;
                this.isLoading = false;
                this.renderGridByHoang();
            });
    }

    onChangeRowSelectionGrade(params: { api: { getSelectedRows: () => InvCkdPartGradeDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectedRowGrade = Object.assign({}, selected);
        this.partGradeId = this.selectedRowGrade.id;
        this.isRowGrade = (this.selectedRowGrade.id != null ? true : false);
        this.isEci = (this.selectedRowGrade.isECI != null ? true : false);



    }

    onChangeRowSelectionPacking(params: { api: { getSelectedRows: () => InvPartPackingDetailDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        this.selectRowPacking = Object.assign({}, selected);
        this.PartPackingId = this.selectRowPacking.id;
        this.isSelectPacking = (this.selectRowPacking.id != null ? true : false);
    }
    callBackDataGradeGrid(params: GridParams) {
        this.dataParamsGrade = params;
        params.api.paginationSetPageSize(
            this.paginationGradeParams.pageSize
        );
        this.paginationGradeParams.skipCount =
            ((this.paginationGradeParams.pageNum ?? 1) - 1) * (this.paginationGradeParams.pageSize ?? 0);
        this.paginationGradeParams.pageSize = this.paginationGradeParams.pageSize;
        this.getDatasGrade(this.paginationGradeParams).subscribe((result) => {
            this.paginationGradeParams.totalCount = result.totalCount;
            this.rowDataPartGrade = result.items;
            this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
            this.isRowGrade = false;
            //  this.resetGridView();
            this.isEci = true;
        });
    }
    callBackDataPackingGrid(params: GridParams) {
        this.dataParamsPacking = params;
        params.api.paginationSetPageSize(
            this.paginationPackingParams.pageSize
        );
        this.paginationPackingParams.skipCount =
            ((this.paginationPackingParams.pageNum ?? 1) - 1) * (this.paginationPackingParams.pageSize ?? 0);
        this.paginationPackingParams.pageSize = this.paginationPackingParams.pageSize;
        this.getDatasPacking(this.paginationPackingParams).subscribe((result) => {
            this.paginationPackingParams.totalCount = result.totalCount;
            this.rowDataPartPackingDetail = result.items;
            this.paginationPackingParams.totalPage = ceil(result.totalCount / (this.paginationPackingParams.pageSize ?? 0));
            this.isRowGrade = false;
            this.isEci = true;
        });
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getCkdPartExportToFile(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })
    }

    exportToExcelGroupBodyColor(): void {
        this.isLoading = true;
        this._service.getCkdPartGroupBodyColorExportToFile(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })
    }

    deletePartGrade(rowdata?: InvCkdPartGradeDto) {
        this.message.confirm(this.l('Delete this Grade'), this.l('Are You Sure?'), (isConfirmed) => {
            if (isConfirmed) {
                this.isLoading = true;
                this._service.partGradeDel(rowdata.id)
                    .pipe(finalize(() => this.isLoading = false))
                    .subscribe((res) => {
                        console.log(res);
                        if (res == 1) {
                            this.notify.success(this.l('Delete Part Grade Successfully'));
                            this.searchDatasGrade(rowdata.partId);
                            this.isLoading = false;
                        } else {
                            this.notify.error(this.l('Delete Part Grade Not Successfully : Part Grade is being used'));
                        }


                    })
            }
        });
    }

    historyOption() {
        var optionSummary = document.querySelector('.btn.history-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcusHist() {
        var optionSummary = document.querySelector('.btn.history-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
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

    prodSumaryImportOption() {
        var optionSummary = document.querySelector('.btn.sumary-prod-import') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcusImport() {
        var optionSummary = document.querySelector('.btn.sumary-prod-import') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
    }

    prodSumaryImportOption1() {
        var optionSummary = document.querySelector('.btn.sumary-prod-import1') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcusImport1() {
        var optionSummary = document.querySelector('.btn.sumary-prod-import1') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
    }


    onCloseModal(type?) {
        switch (type) {
            case "PL":
                this.searchDatas();
                break;
            case "PG":
                this.searchDatasGrade(this.selectPartId);
                break;
            case "PP":
                this.searchDatasPacking(this.selectPartId,this.orderPatern);
                break;
        }

        setTimeout(() => {
            this.fetchChangedRecords();
        }, 5000);
    }

    exportDetailsToExcel() {
        this.isLoading = true;
        this._service.getCkdPartExportDetailsToFile(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern,
            this.isActive
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })
    }

    exportPackingDetailsToExcel() {
        this.isLoading = true;
        this._service.getCkdPartExportPackingDetails(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern,
            this.isActive
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })
    }
    exportNormalToExcel() {
        this.isLoading = true;
        this._service.getCkdPartExportNormalToFile(
            this.partNo,
            this.cfc,
            this.model,
            this.grade,
            this.shop,
            this.supplierNo,
            this.orderPattern,
            this.isActive
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })
    }


    checkActive() {
        var tab = document.querySelectorAll<HTMLElement>('li.nav-item.ng-star-inserted')
            if (tab[0].classList.toggle('active')){
                this.checkTab = 'A';
            }else{
                this.checkTab = 'B'
            }
    }
}
