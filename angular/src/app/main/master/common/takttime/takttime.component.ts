import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmTaktTimeDto, MstCmmTaktTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditTakttimeModalComponent } from './create-or-edit-takttime-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './takttime.component.html',
})
export class TakttimeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalTaktTime', { static: true }) createOrEditModalTaktTime: | CreateOrEditTakttimeModalComponent | undefined;
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

    selectedRow: MstCmmTaktTimeDto = new MstCmmTaktTimeDto();
    saveSelectedRow: MstCmmTaktTimeDto = new MstCmmTaktTimeDto();
    datas: MstCmmTaktTimeDto = new MstCmmTaktTimeDto();
    isLoading: boolean = false;


    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    prodLine: string = '';
    groupCd: string = '';
    taktTimeSecond: number = 0;
    taktTime: string = '';
    isActive: string = '';

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
        private _service: MstCmmTaktTimeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1
            },
            {
                headerName: this.l('Group Cd'),
                headerTooltip: this.l('Group Cd'),
                field: 'groupCd',
                flex: 1
            },
            {
                headerName: this.l('Takt Time Second'),
                headerTooltip: this.l('Takt Time Second'),
                field: 'taktTimeSecond',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Takt Time'),
                headerTooltip: this.l('Takt Time'),
                field: 'taktTime',
              //  type: 'rightAligned',
                flex: 1
            },
            /* {
                 headerName: this.l('Is Active'),
                 headerTooltip: this.l('Is Active'),
                 field: 'isActive', cellClass: ['text-center'],
                 width: 120,
                 cellRenderer: 'agCellButtonComponent',
                 buttonDefTwo:
                 {
                     text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                     iconName: 'fa fa-circle',
                     className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                 },
             }*/
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
            this.prodLine,
            this.groupCd,
            this.taktTimeSecond,
            this.taktTime,
            this.isActive,
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
        this.prodLine = '',
            this.groupCd = '',
            this.taktTimeSecond = 0,
            this.taktTime = '',
            this.isActive = '',
            this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.prodLine,
            this.groupCd,
            this.taktTimeSecond,
            this.taktTime,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmTaktTimeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmTaktTimeDto();
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
            });
    }

    deleteRow(system: MstCmmTaktTimeDto): void {
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
    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;

    //     this._service.getTaktTimeToExcel(
    //         this.prodLine,
    //         this.groupCd,
    //         this.taktTimeSecond,
    //         this.taktTime,
    //         this.isActive,
    //     )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getTaktTimeToExcel(
            this.prodLine,
            this.groupCd,
            this.taktTimeSecond,
            this.taktTime,
            this.isActive,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
