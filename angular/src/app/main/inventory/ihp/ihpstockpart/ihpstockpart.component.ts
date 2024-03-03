import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvIhpPartListDto, InvIhpPartListServiceProxy, InvIhpStockPartDto, InvIhpStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './ihpstockpart.component.html',
})
export class IhpStockPartComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    partListColDefs: CustomColDef[] = [];
    partGradeColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
 

    selectedRow: InvIhpStockPartDto = new InvIhpStockPartDto();
    datas: InvIhpPartListDto = new InvIhpPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowDataPartList: any[] = [];
    rowDataPartGrade: any[] = [];
    dataParamsCustoms: GridParams | undefined;

    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    supplierType: string = '';
    supplierCd: string = '';
    model: string = '';

    cfc: string = '';
    materialCode: string = '';
    materialSpec: string = '';
    partSpec: string = '';
    partSize: string = '';
    sourcing: string = '';
    cutting: string = '';
    PartNo: string;
    WorkingDateFrom;
    WorkingDateTo;
    Model: string;
    fn: CommonFunction = new CommonFunction();

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
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
        private _service: InvIhpStockPartServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
        
    ) {
        super(injector);
        this.partListColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
           // { headerName: this.l('DrmMaterialId'), headerTooltip: this.l('DrmMaterialId'), field: 'drmMaterialId', flex: 1 },
            { headerName: this.l('Part Code'), headerTooltip: this.l('PartCode'), field: 'partCode', flex: 1,  },
            { headerName: this.l('Working Date'), headerTooltip: this.l('WorkingDate'), field: 'workingDate', flex: 1,   valueFormatter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'), },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('PartNo5Digits'), headerTooltip: this.l('PartNo5Digits'), field: 'partNo5Digits', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Model'), headerTooltip: this.l('Model'), field: 'model', flex: 1 },
            { headerName: this.l('Grade Name'), headerTooltip: this.l('Grade Name'), field: 'gradeName', flex: 1 },
            { headerName: this.l('Material Code'), headerTooltip: this.l('Material Code'), field: 'materialCode', flex: 1 },
            { headerName: this.l('MaterialSpec'), headerTooltip: this.l('MaterialSpec'), field: 'materialSpec', flex: 1 },
            { headerName: this.l('Use Press'), headerTooltip: this.l('Use Press'), field: 'usePress', flex: 1 },
            { headerName: this.l('Press'), headerTooltip: this.l('Press'), field: 'press', flex: 1 },
            { headerName: this.l('IhpOh'), headerTooltip: this.l('IhpOh'), field: 'ihpOh', flex: 1 },
            { headerName: this.l('Press Broken'), headerTooltip: this.l('Press Broken'), field: 'pressBroken', flex: 1 },
            { headerName: this.l('Hand'), headerTooltip: this.l('Hand'), field: 'hand', flex: 1 },
            { headerName: this.l('HandOh'), headerTooltip: this.l('HandOh'), field: 'handOh', flex: 1 },
            { headerName: this.l('Hand Broken'), headerTooltip: this.l('Hand Broken'), field: 'handBroken', flex: 1 },
            { headerName: this.l('Material In'), headerTooltip: this.l('Material In'), field: 'materialIn', flex: 1 },
            { headerName: this.l('Material In Addition'), headerTooltip: this.l('Material In Addition'), field: 'materialInAddition', flex: 1 },
            { headerName: this.l('Shift'), headerTooltip: this.l('Shift'), field: 'shift', flex: 1 },
            { headerName: this.l('PartId'), headerTooltip: this.l('PartId'), field: 'partId', flex: 1 },
        ];
       
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.fn.setHeight_notFullHeight();
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
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }


    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.PartNo,
            this._dateTimeService.convertToDatetime(this.WorkingDateFrom),
            this._dateTimeService.convertToDatetime(this.WorkingDateTo),
            this.Model,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowDataPartList = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.autoSizeDetails();
            });
    }

    clearTextSearch() {
            this.PartNo = '';
            this.Model = '';
            this.WorkingDateFrom = '';
            this.WorkingDateTo = '';
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.PartNo,
            this._dateTimeService.convertToDatetime(this.WorkingDateFrom),
            this._dateTimeService.convertToDatetime(this.WorkingDateTo),
            this.Model,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvIhpPartListDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        // if (selected) {
        //     this.I = selected.id;
        // }
        // this.selectedRow = Object.assign({}, selected);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowDataPartList = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.autoSizeDetails();
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
                this.rowDataPartList = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                this.autoSizeDetails();
            });
    }

    
    


    

}
