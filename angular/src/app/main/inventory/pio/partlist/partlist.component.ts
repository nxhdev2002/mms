import { ColDef, GridApi, ValueGetterParams } from "@ag-grid-enterprise/all-modules";
import { DatePipe } from "@angular/common";
import { Component, Injector, OnInit, ViewChild } from "@angular/core";
import { CommonFunction } from "@app/main/commonfuncton.component";
import { AgCellButtonRendererComponent } from "@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component";
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from "@app/shared/common/models/base.model";
import { GridTableService } from "@app/shared/common/services/grid-table.service";
import { AppComponentBase } from "@shared/common/app-component-base";
import { InvPioPartListDto, InvPioPartListServiceProxy } from "@shared/service-proxies/service-proxies";
import { FileDownloadService } from "@shared/utils/file-download.service";
import { ceil } from "lodash-es";
import { Paginator } from "primeng/paginator";
import { finalize } from "rxjs";
import { CreateOrEditPartListModalComponent } from "./create-or-edit-partlist.component";
import { ViewHistoryPartListModalComponent } from "./history-partlist-modal.component";

@Component({
    templateUrl: './partlist.component.html',
})
export class PartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('createOrEditModalPartList', { static: true }) createOrEditModalPartList:| CreateOrEditPartListModalComponent| undefined;
    @ViewChild('viewHistoryPartListModal', { static: true }) viewHistoryPartListModal:| ViewHistoryPartListModalComponent| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvPioPartListDto = new InvPioPartListDto();
    saveSelectedRow: InvPioPartListDto = new InvPioPartListDto();
    datas: InvPioPartListDto = new InvPioPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();


    changedIds: any[] = [];
    partNo: string = '';
    partName: string = '';
    mktCode: string = '';
    vinNo: string = '';
    partType: string = '';
    shop: string = '';
    cartType: string = '';
    interiorColor: string = '';
    fullModel: string = '';
    prodSfx: string = '';
    remark: string = '';
    isActive = '1';

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

    autoGroupColumnDef: any = {
        headerName: 'Marketing Code',
        pinned: true,
    };

    constructor(
        injector: Injector,
        private _service: InvPioPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                field: 'mktCode',
                rowGroup: true,
                hide: true,
                cellClass: ['cell-border', 'cell-readonly',],
                flex: 1
            },
            { field: 'id' , hide: true},
            { headerName: this.l('Full Model'), headerTooltip: this.l('Full Model'), field: 'fullModel' , flex: 1},
            { headerName: this.l('Prod Sfx'), headerTooltip: this.l('Prod Sfx'), field: 'prodSfx' , flex: 1},
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo' , flex: 1},
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName' , flex: 1},
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'partType' , flex: 1},
            { headerName: this.l('Part Type Description'), headerTooltip: this.l('Part Type Description'), field: 'partDescription' , flex: 1},
            { headerName: this.l('Pio Type'), headerTooltip: this.l('Pio Type'), field: 'pioType' , flex: 1},
            { headerName: this.l('Box Size'), headerTooltip: this.l('Box Size'), field: 'boxSize' , flex: 1},
            {
                headerName: this.l('Start Date'), headerTooltip: this.l('Start Date'), field: 'startDate',flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.startDate, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('End Date'), headerTooltip: this.l('End Date'), field: 'endDate',flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.endDate, 'dd/MM/yyyy'),
            },


            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark' , flex: 1},
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "1") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "0") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "1") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "0") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._service.getChangedRecords().subscribe((res) => this.changedIds = res.partList);
    }

    toggleIsActive() {
        this.isActive = this.isActive === undefined ? '1' : undefined;
        if(this.paginationParams.pageNum > 1)
        {
            this.paginationParams.pageNum = 1;
            this.paginationParams.skipCount = 0;
        }
        this.searchDatas();
    }

    // autoSize() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //         if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
    //             allColumnIds.push(column.getId());
    //         }
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {
    //     setTimeout(() => {
    //         // this.dataParams.columnApi!.sizeColumnsToFit({
    //         //     suppressColumnVirtualisation: true,
    //         // });
    //         this.autoSize();
    //     }, 501)
    // }


    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.fullModel,
            this.mktCode,
            this.partNo,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            // .pipe(finalize(() => {
            //     this.gridTableService.selectFirstRow(this.dataParams!.api);
            // }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
               // this.resetGridView();
            });
    }

    clearTextSearch() {
        this.partNo = '';
        this.isActive = '1';
        this.mktCode = '';
        this.fullModel = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.fullModel,
            this.mktCode,
            this.partNo,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvPioPartListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvPioPartListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            // .pipe(finalize(() => {
            //     this.gridTableService.selectFirstRow(this.dataParams!.api);
            // }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                //this.resetGridView();
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
            // .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
                //this.resetGridView();
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPioPartListToExcel(
            this.fullModel,
            this.mktCode,
            this.partNo,
            this.isActive,
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    deleteRow(system): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
}
