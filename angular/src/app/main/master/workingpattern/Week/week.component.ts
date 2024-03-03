import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    FrameworkComponent,
    GridParams,
    PaginationParamsModel,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptWeekDto, MstWptWeekServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditWeekModalComponent } from './create-or-edit-week-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './week.component.html',
})
export class WeekComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalWeek', { static: true }) createOrEditModalWeek:
        | CreateOrEditWeekModalComponent
        | undefined;
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

    selectedRow: MstWptWeekDto = new MstWptWeekDto();
    saveSelectedRow: MstWptWeekDto = new MstWptWeekDto();
    datas: MstWptWeekDto = new MstWptWeekDto();
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    workingYear: number;
    weekNumber: number;
    workingDays: number = 0;
    isActive: string = '';
    frameworkComponents: FrameworkComponent;

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
        private _service: MstWptWeekServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 60,
            },
            {
                headerName: this.l('Working Year'),
                headerTooltip: this.l('Working Year'),
                field: 'workingYear',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Week Number'),
                headerTooltip: this.l('Week Number'),
                field: 'weekNumber',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Working Days'),
                headerTooltip: this.l('Working Days'),
                field: 'workingDays',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isActive == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isActive == 'Y' ? 'btnActive' : 'btnInActive'),
                },
                width: 120,
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service.getAll(
            this.workingYear,
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
        this.workingYear = 0;
        this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.workingYear,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptWeekDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptWeekDto();
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

    deleteRow(system: MstWptWeekDto): void {
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

    //     this._service.getWeekToExcel(this.workingYear)
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void { 

        this.fn.exportLoading(e, true);
         
        this._service.getWeekToExcel(this.workingYear
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }

}
