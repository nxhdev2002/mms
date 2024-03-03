import { GridApi, GridReadyEvent, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvPartListOffServiceProxy, InvPioPartGradeInlDto, InvPioPartListInlDto, InvPioPartListInlServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './partlistoff.component.html',
    styleUrls: ['./partlistoff.component.less'],
})
export class PioPartListOffComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
   // @ViewChild('addPartListInlModal', { static: true }) addPartListInlModal:| InvPioPartListInlDto
   // | undefined;
    defaultColDefs: CustomColDef[] = [];
    partGradeColDef: CustomColDef[] = [];
    partPackingDetailColDef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
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
    orderPattern;
    listCfcAcitve: any[];
    checkClone: boolean = true;
    checkEdit: boolean = true;
    isRowGrade: boolean = false;


    changedRecordsPartList = [];
    changedRecordsPartGrade = [];
    selectedRow: InvPioPartListInlDto = new InvPioPartListInlDto();
    selectedRowGrade: InvPioPartGradeInlDto = new InvPioPartGradeInlDto();
    saveSelectedRow: InvPioPartListInlDto = new InvPioPartListInlDto();
    datas: InvPioPartListInlDto = new InvPioPartListInlDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    dataParamsGrade: GridParams | undefined;
    dataParamsPacking: GridParams | undefined;
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
    selectId;
    selectGradeId;
    partNo : string = '' ;
    partId: any;
	partNoNormalized : string = '' ;
	partName : string = '' ;
	supplierNo : string = '' ;
	supplierCd : string = '' ;
	colorSfx : string = '' ;
    model : string = '' ;
    grade : string = '' ;
    shop : string = '' ;
    p_partNo
	supplierId
	materialId
	isActive : string = 'Y' ;
    groupDefaultExpanded : number = 0;
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
    orderPatternList = [
        { value: '', label: "Mix" },
        { value: 'LOT', label: "LOT" },
        { value: 'PXP', label: "PXP" },
    ];
    cfcActiveList = [
        { value: '', label: "All" },
        { value: 'Y', label: "Active" },
        { value: 'N', label: "InActive" },

    ];
    isEci:boolean = true;
    constructor(
        injector: Injector,
        private _service: InvPartListOffServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('Cfc'),
                width:80,
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                valueGetter: (params: ValueGetterParams) => params.data.cfc ? 'Cfc : ' + params.data.cfc : this.l('Cfc : '),
            },
            {headerName: this.l('Part No'),flex:1,headerTooltip: this.l('Part No'),field: 'partNo'},
            {headerName: this.l('Supplier No'),flex:0.8,headerTooltip: this.l('Supplier No'),field: 'supplierNo'},
            {headerName: this.l('Part No Normalized'),flex:1,headerTooltip: this.l('Part No Normalized'),field: 'partNoNormalized'},
            {headerName: this.l('Part No Normalized S4'),flex:1,headerTooltip: this.l('Part No Normalized S4'),field: 'partNoNormalizedS4'},
			{headerName: this.l('Part Name'),flex:2,headerTooltip: this.l('Part Name'),field: 'partName'},
			{headerName: this.l('Color Sfx'),flex:0.6,headerTooltip: this.l('Color Sfx'),field: 'colorSfx'},
            {headerName: this.l('OrderPattern'),flex:0.6,headerTooltip: this.l('OrderPattern'),field: 'orderPattern'},
        ];

        this.partGradeColDef = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationGradeParams.pageSize * (this.paginationGradeParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),flex:2,headerTooltip: this.l('Part No'),field: 'partNo'},
			{headerName: this.l('Model'),flex:1,headerTooltip: this.l('Model'),field: 'model'},
            {headerName: this.l('Grade'),flex:1,headerTooltip: this.l('Grade'),field: 'grade'},
           // {headerName: this.l('IdLine'),flex:1,headerTooltip: this.l('IdLine'),field: 'idLine'},
            {headerName: this.l('Shop'),flex:1,headerTooltip: this.l('Shop'),field: 'shop',
            valueGetter: (params) => { if(params.data.shop == 'V'){ return 'VLD'}}},
            {headerName: this.l('Body Color'),flex:1,headerTooltip: this.l('Body Color'),field: 'bodyColor'},
            {headerName: this.l('Usage Qty'),flex:1,headerTooltip: this.l('Usage Qty'),field: 'usageQty', type: 'rightAligned'},
            {headerName: this.l('Start Lot'),flex:1,headerTooltip: this.l('Start Lot'),field: 'startLot'},
            {headerName: this.l('End Lot'),flex:1,headerTooltip: this.l('End Lot'),field: 'endLot'},
            {headerName: this.l('Start Run'),flex:1,headerTooltip: this.l('Start Run'),field: 'startRun'},
            {headerName: this.l('End Run'),flex:1,headerTooltip: this.l('End Run'),field: 'endRun'},
            {headerName: this.l('Ef From Part'),flex:1,headerTooltip: this.l('Ef From Part'),field: 'efFromPart', valueGetter: (params) => this.pipe.transform(params.data?.efFromPart, 'dd-MM-yyyy')},
            {headerName: this.l('Ef To Part'),flex:1,headerTooltip: this.l('Ef To Part'),field: 'efToPart', valueGetter: (params) => this.pipe.transform(params.data?.efToPart, 'dd-MM-yyyy')},
            {headerName: this.l('OrderPattern'),flex:1,headerTooltip: this.l('OrderPattern'),field: 'orderPattern'},
            {headerName: this.l('Remark'),flex:1, headerTooltip: this.l('Remark'),field: 'remark'},
        ];


        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationGradeParams = { pageNum: 1, pageSize: 500, totalCount: 0 }

        this.fetchChangedRecords();
        this._service.getListCfcs().subscribe(res => {
            this.listCfcAcitve = res;
        })

        this.checkEdit = true;
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsPartList = result.partList;
            this.changedRecordsPartGrade = result.partGrade;
        })
    }

    // autoSize() {
    //     const allColumnIds: string[] = [];
    //     this.dataParamsGrade.columnApi!.getAllColumns()!.forEach((column) => {
    //         if (column.getId().toString() != "checked" ) {
    //             allColumnIds.push(column.getId());
    //         }
    //     });
    //     this.dataParamsGrade.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {
    //     setTimeout(() => {
    //         this.autoSize();
    //     }, 500)
    // }
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
        .pipe(finalize(() =>{}))
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
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
                this.rowDataPartGrade = [];
                this.rowDataPartPackingDetail = [];
                this.isRowGrade = false;
                this.isEci = true;
                this.isLoading = false;
            }
        });
    }


    clearTextSearch() {
        this.partNo = '',
        this.cfc = '',
        this.model = '',
        this.grade = '',
        this.shop = '',
		this.supplierNo = '',
        this.orderPattern = '',
        this.isActive = '',
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

    reloadSearch(_partNo, _cfc,_partlistId) {
        this.partNo = _partNo;
        this.cfc = _cfc;
        this.searchDatas();
        this.searchDatasGrade(_partlistId);
    }

    getDatasGrade(paginationParams?: PaginationParamsModel){
        return this._service.getPartGradeInl(
            this.selectPartId,
			'',
            this.paginationGradeParams.skipCount,
            this.paginationGradeParams.pageSize
        );
    }

    searchDatasGrade(p_partid): void {
        this.isLoading = true;

        this._service.getPartGradeInl(
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
            }
            else {
                this.rowDataPartGrade = [];
                this.isLoading = false;
                this.isRowGrade = false;
                this.isEci = true;
            }
        });
    }




    onChangeRowSelection(params: { api: { getSelectedRows: () => InvPioPartListInlDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.selectPartId = selected.partId;
            this.selectId = selected.id;
            this.searchDatasGrade(selected.partId);
            this.checkEdit = false;
            var cfcExist = (this.listCfcAcitve.filter(s => s.cfc == selected.cfc)).length;
            this.checkClone = cfcExist > 0 ? false : true;
            this.isRowGrade = false;
            this.isEci = true;

        }
        this.fetchChangedRecords();
       this.selectedRow = Object.assign({}, selected);

    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isRowGrade = false;
            this.isEci = true;
            this.isLoading = false;
        });
    }
    changePageGrade(paginationParams) {
        this.paginationGradeParams = paginationParams;
        this.paginationGradeParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatasGrade(this.paginationGradeParams).subscribe((result) => {
            this.paginationGradeParams.totalCount = result.totalCount;
            this.rowDataPartGrade = result.items;
            this.paginationGradeParams.totalPage = ceil(result.totalCount / (this.paginationGradeParams.pageSize ?? 0));
          //  this.resetGridView();
          this.isRowGrade = false;
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
            .pipe(finalize(() =>{}))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isRowGrade = false;
                this.isEci = true;
                this.isLoading = false;
             //   this.resetGridView();
            });
    }

    onChangeRowSelectionGrade(params: { api: { getSelectedRows: () => InvPioPartGradeInlDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
       this.selectedRowGrade = Object.assign({}, selected);
       this.selectGradeId = this.selectedRowGrade.id;
       this.isRowGrade = (this.selectedRowGrade.id != null ? true : false);
       this.isEci = (this.selectedRowGrade.isECI != null ? true :false);
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
           // this.resetGridView();
           this.isRowGrade = false;
           this.isEci = true;
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

    deletePartGrade(id){

        this.message.confirm(this.l('Delete this Grade'), this.l('Are You Sure?'), (isConfirmed) => {
            if (isConfirmed) {
                this.isLoading = true;
                this._service.partGradeDel(id)
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this.notify.success(this.l('Delete Part Grade Successfully'));
                    this.isLoading = false;
                })
            }}
        )
        
    }


    exportToExcel(): void {

        this.isLoading = true;

        this._service.getPioPartExportToFile(
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

        this._service.getPioPartGroupBodyColorExportToFile(
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
}
