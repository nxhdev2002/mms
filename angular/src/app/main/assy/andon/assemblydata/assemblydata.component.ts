import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
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
import { AsyAdoAssemblyDataDto, AsyAdoAssemblyDataServiceProxy } from '@shared/service-proxies/service-proxies';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './assemblydata.component.html',
})
export class AssemblyDataComponent extends AppComponentBase implements OnInit {
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

    selectedRow: AsyAdoAssemblyDataDto = new AsyAdoAssemblyDataDto();
    saveSelectedRow: AsyAdoAssemblyDataDto = new AsyAdoAssemblyDataDto();
    datas: AsyAdoAssemblyDataDto = new AsyAdoAssemblyDataDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    workingDate: any;
    noindate: number = 0;
    requestDateTo;
    requestDateFrom;
    shift: string = '';
    seqNo: string = '';
    line: string = '';
    process: string = '';
    model: string = '';
    body: string = '';
    seqno: string = '';
    grade: string = '';
    lotno: string = '';
    noinlot: number = 0;
    noinshift;
    lotnoindex: number = 0;
    color: string = '';
    createdate: any;

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
        private _service: AsyAdoAssemblyDataServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
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
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',
                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                flex: 1
            },

            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                flex: 1
            },
            {
                headerName: this.l('No In Date'),
                headerTooltip: this.l('No In Date'),
                field: 'noInDate',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Line'),
                headerTooltip: this.l('Line'),
                field: 'line',
                flex: 1
            },
            {
                headerName: this.l('Process'),
                headerTooltip: this.l('Process'),
                field: 'process',
                flex: 1
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1
            },
            {
                headerName: this.l('Body'),
                headerTooltip: this.l('Body'),
                field: 'body',
                flex: 1
            },
            {
                headerName: this.l('Seq No'),
                headerTooltip: this.l('Seqno'),
                field: 'seqNo',
                flex: 1
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lotno'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('Noinlot'),
                field: 'noInLot',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Lot No Index'),
                headerTooltip: this.l('Lot No Index'),
                field: 'lotNoIndex',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('No In Shift'),
                headerTooltip: this.l('No In Shift'),
                field: 'noInShift',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                flex: 1
            },
            {
                headerName: this.l('Create Date'),
                headerTooltip: this.l('Createdate'),
                field: 'createDate',
                valueGetter : (params) => this.pipe.transform(params.data?.createDate, 'dd/MM/yyyy  HH:mm:ss'),
                flex: 1
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    getDateNow() {
        var today = new Date();
        var date = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        return date;
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.line,
            this.shift,
            this.seqNo,
            this.process,
            this.body,
            this.lotno,
            this.noinshift,
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
        this.requestDateFrom = '',
        this.requestDateTo = '',
        this.line = '',
        this.shift = '',
        this.seqNo  = '',
        this.process = '',
        this.body = '',
        this.lotno = '',
        this.noinlot = 0,
        this.searchDatas();
    }

    // autoSizeAll() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //       if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
    //         allColumnIds.push(column.getId());
    //       }
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView(){
    //     setTimeout(()=>{
    //         this.autoSizeAll();
    //     },1000)
    // }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.line,
            this.shift,
            this.seqNo,
            this.process,
            this.body,
            this.lotno,
            this.noinshift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => AsyAdoAssemblyDataDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new AsyAdoAssemblyDataDto();
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
              //  this.resetGridView();
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getAssemblyDataToExcel(
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            this.line,
            this.shift,
            this.seqNo,
            this.process,
            this.body,
            this.lotno,
            this.noinshift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
