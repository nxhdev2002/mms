import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditGpsMasterModalComponent } from './create-or-edit-invgpsmaster-modal.component';
import { InvGpsPartListByCategoryDto, InvGpsPartListByCategoryServiceProxy } from '@shared/service-proxies/service-proxies';
import { ImportGpsMasterComponent } from './import-gpsmaster.component';

@Component({
    templateUrl: './gpsmaster.component.html',
})
export class GpsMasterComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) createOrEditModal: | CreateOrEditGpsMasterModalComponent | undefined;
    @ViewChild('importModal', { static: true }) importModal: | ImportGpsMasterComponent | undefined;
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

    selectedRow: InvGpsPartListByCategoryDto = new InvGpsPartListByCategoryDto();
    saveSelectedRow: InvGpsPartListByCategoryDto = new InvGpsPartListByCategoryDto();
    datas: InvGpsPartListByCategoryDto = new InvGpsPartListByCategoryDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    item : string = '';
    category = '';
    location : string = '';
    partType : string = '';

    categoryList = [
        {value: '' , label: "ALL"},
        {value: 'Direct Materials' , label: "Direct Materials"},
        {value: 'Indirect Materials' , label: "Indirect Materials"},
        {value: 'Other' , label: "Other"},
        {value: 'Stationery' , label: "Stationery"},
        {value: 'Spare parts' , label: "Spare parts"},
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
        private _service: InvGpsPartListByCategoryServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'item', flex: 1 },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description', flex: 1 },
            { headerName: this.l('Uom'), headerTooltip: this.l('Uom'), field: 'uom', flex: 1 },
            { headerName: this.l('Category'), headerTooltip: this.l('Category'), field: 'category', flex: 1 },
            { headerName: this.l('Location'), headerTooltip: this.l('Location'), field: 'location', flex: 1 },
            { headerName: this.l('Expense Account'), headerTooltip: this.l('ExpenseAccount'), field: 'expenseAccount', flex: 1 },
            { headerName: this.l('Group'), headerTooltip: this.l('Group'), field: 'group', flex: 1 },
            { headerName: this.l('Current Category'), headerTooltip: this.l('CurrentCategory'), field: 'currentCategory', flex: 1 },
            { headerName: this.l('Part Type'), headerTooltip: this.l('PartType'), field: 'partType', flex: 1 },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
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

        // setTimeout(()=>{
        //     this.dataParams.columnApi!.sizeColumnsToFit({
        //         suppressColumnVirtualisation: true,
        //     });
        //     this.autoSizeAll();
        // },100)
    }
    searchDatas(): void {
        this._service.getAll(
            this.item,
            this.category,
            this.location,
            this.partType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    clearTextSearch() {
            this.item = '',
            this.category = '',
            this.location = '',
            this.partType = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.item,
            this.category,
            this.location,
            this.partType,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGpsPartListByCategoryDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGpsPartListByCategoryDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
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
    // getCategoryCbx() {
    //     this.isCategoryList = []
    //     this._service.getCbxCategory().subscribe((result) => {
    //         result.forEach(e => this.isCategoryList.push({ value: e.name, label: e.name }));
    //     });
    // }
    deleteRow(system: InvGpsPartListByCategoryDto): void {
        this.message.confirm(this.l('Are You Sure To Delete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvGpsPartListByCategoryToExcel(
            this.item,
            this.category,
            this.location,
            this.partType,
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
    
    onModalSave(savedData: any): void {
        this.searchDatas();
    }
}
