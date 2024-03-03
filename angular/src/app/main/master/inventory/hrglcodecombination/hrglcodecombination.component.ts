import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvHrGlCodeCombinationDto, MstInvHrGlCodeCombinationServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './hrglcodecombination.component.html',
})
export class HrGlCodeCombinationComponent extends AppComponentBase implements OnInit {
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

    selectedRow: MstInvHrGlCodeCombinationDto = new MstInvHrGlCodeCombinationDto();
    saveSelectedRow: MstInvHrGlCodeCombinationDto = new MstInvHrGlCodeCombinationDto();
    datas: MstInvHrGlCodeCombinationDto = new MstInvHrGlCodeCombinationDto();
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

    chartOfAccountsId
    accountType: string = '';
    enabledFlag: string = '';
    segment1
    segment2: string = '';
    segment3: string = '';
    segment4: string = '';
    segment5: string = '';
    segment6: string = '';
    concatenatedsegments: string = '';
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
        private _service: MstInvHrGlCodeCombinationServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Account Type'), headerTooltip: this.l('Account Type'), field: 'accountType', flex: 1 },
            { headerName: this.l('Enabled Flag'), headerTooltip: this.l('Enabled Flag'), field: 'enabledFlag', flex: 1 },
            { headerName: this.l('Segment 1'), headerTooltip: this.l('Segment1'), field: 'segment1', flex: 1 },
            { headerName: this.l('Segment 2'), headerTooltip: this.l('Segment2'), field: 'segment2', flex: 1 },
            { headerName: this.l('Segment 3'), headerTooltip: this.l('Segment3'), field: 'segment3', flex: 1 },
            { headerName: this.l('Segment 4'), headerTooltip: this.l('Segment4'), field: 'segment4', flex: 1 },
            { headerName: this.l('Segment 5'), headerTooltip: this.l('Segment5'), field: 'segment5', flex: 1 },
            { headerName: this.l('Segment 6'), headerTooltip: this.l('Segment6'), field: 'segment6', flex: 1 },
            { headerName: this.l('Concatenated Segments'), headerTooltip: this.l('Concatenatedsegments'), field: 'concatenatedsegments', flex: 1 },
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
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.concatenatedsegments,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
             //   this.resetGridView();
            });
    }

    clearTextSearch() {
        this.concatenatedsegments = '',
            this.searchDatas();
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.concatenatedsegments,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    // autoSizeAll() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi?.getAllColumns()?.forEach((column) => {
    //         // eslint-disable-next-line eqeqeq
    //         if (column.getId().toString() != 'checked' && column.getId().toString() != 'stt') {
    //             allColumnIds.push(column.getId());
    //         }
    //     });
    //     this.dataParams.columnApi?.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {
    //     this.dataParams.columnApi!.sizeColumnsToFit({
        //     suppressColumnVirtualisation: true,
        // });
    //     setTimeout(() => {
    //         this.autoSizeAll();
    //     }, 1000);
    // }


    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvHrGlCodeCombinationDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvHrGlCodeCombinationDto();
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
          //  this.resetGridView();
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
             //   this.resetGridView();
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);


        this._service.getHrGlCodeCombinationToExcel(
            this.accountType,
            this.enabledFlag,
            this.segment1,
            this.segment2,
            this.segment3,
            this.segment4,
            this.segment5,
            this.segment6,
            this.concatenatedsegments,
            this.isActive,
        )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
