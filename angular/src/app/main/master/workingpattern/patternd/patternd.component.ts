import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptPatternDDto, MstWptPatternDServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPatternDModalComponent } from './create-or-edit-patternd-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './patternd.component.html',
})
export class PatternDComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPatternD', { static: true }) createOrEditModalPatternD: | CreateOrEditPatternDModalComponent | undefined;
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

    selectedRow: MstWptPatternDDto = new MstWptPatternDDto();
    saveSelectedRow: MstWptPatternDDto = new MstWptPatternDDto();
    datas: MstWptPatternDDto = new MstWptPatternDDto();
    isLoading: boolean = false;
 

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    patternHId: number = 0;
    shiftNo: number = 0;
    shiftName: string = '';
    startTime: any;
    endTime: any;
    dayOfWeek: string = '';
    seasonType: string = '';
    isActive: string = '';
    frameworkComponents: FrameworkComponent

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
        private _service: MstWptPatternDServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer:
                    (params) => params.rowIndex + 1 + this.paginationParams.pageSize *
                        (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Pattern H Id'),
                headerTooltip: this.l('Pattern H Id'),
                field: 'patternHId',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Shift No'),
                headerTooltip: this.l('Shift No'),
                field: 'shiftNo',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Shift Name'),
                headerTooltip: this.l('Shift Name'),
                field: 'shiftName',
                flex: 1
            },
            {
                headerName: this.l('Start Time'),
                headerTooltip: this.l('Start Time'),
                field: 'startTime',
                flex: 1
            },
            {
                headerName: this.l('End Time'),
                headerTooltip: this.l('End Time'),
                field: 'endTime',
                flex: 1
            },
            {
                headerName: this.l('Day Of Week'),
                headerTooltip: this.l('Day Of Week'),
                field: 'dayOfWeek',
                flex: 1
            },
            {
                headerName: this.l('Season Type'),
                headerTooltip: this.l('Season Type'),
                field: 'seasonType',
                flex: 1,
                cellStyle: function (params: any) {
                    if (params.data.seasonType === 'High') {
                        return {
                            'background-color': 'Red',
                            'color': 'white',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            // 'border-left-width': '0',
                            // 'padding': '3px 6px 4px'
                        };
                    }
                    else if (params.data.seasonType === 'Low') {
                        return {
                            'background-color': 'Yellow',
                            'color': 'black',
                            'border-bottom': '1px Solid #c0c0c0',
                            'border-right': '1px Solid #c0c0c0',
                            'overflow': 'hidden',
                            'border-top-width': '0',
                            // 'border-left-width': '0',
                            // 'padding': '3px 6px 4px'
                        };
                    }
                    return '';
                },
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },
                flex: 1,
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
            this.patternHId,
            this.shiftNo,
            this.shiftName,
            this.startTime,
            this.endTime,
            this.dayOfWeek,
            this.seasonType,
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
        this.patternHId = 0,
            this.shiftNo = 0,
            this.shiftName = '',
            this.startTime = null,
            this.endTime = null,
            this.dayOfWeek = '',
            this.seasonType = '',
            this.isActive = '',
            this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.patternHId,
            this.shiftNo,
            this.shiftName,
            this.startTime,
            this.endTime,
            this.dayOfWeek,
            this.seasonType,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptPatternDDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptPatternDDto();
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

    deleteRow(system: MstWptPatternDDto): void {
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

    //     this._service.getPatternDToExcel(
    //         this.shiftName
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
         
        this._service.getPatternDToExcel(
            this.shiftName
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }

}
