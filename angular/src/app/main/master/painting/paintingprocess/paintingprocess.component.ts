import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstPtsPaintingProcessDto, MstPtsPaintingProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPaintingProcessModalComponent } from './create-or-edit-paintingprocess-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';


@Component({
    templateUrl: './paintingprocess.component.html',
    styleUrls: ['./paintingprocess.component.less',],
})
export class PaintingProcessComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPaintingProcess', { static: true })
    createOrEditModalPaintingProcess: CreateOrEditPaintingProcessModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = { pageNum: 1, pageSize: 500, totalCount: 0, skipCount: 0, sorting: '', totalPage: 1 };
    selectedRow: MstPtsPaintingProcessDto = new MstPtsPaintingProcessDto();
    saveSelectedRow: MstPtsPaintingProcessDto = new MstPtsPaintingProcessDto();
    datas: MstPtsPaintingProcessDto = new MstPtsPaintingProcessDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    filter: string = '';
    workingType: string = '';
    workingStatus: string = '';
    processCode: string = '';
    processName: string = '';
    today: Date = new Date();
    pipe = new DatePipe('en-US');
    gridApi!: GridApi;
    rowSelection = 'multiple';
    workingDate: any;
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
    };

    constructor(
        injector: Injector,
        private _mstwptPaintingProcessService: MstPtsPaintingProcessServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,

    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1,
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Process Seq'),
                headerTooltip: this.l('Process Seq'), field: 'processSeq', flex: 1, type: 'rightAligned',
            },
            {
                headerName: this.l('Process Code'),
                headerTooltip: this.l('Process Code'), field: 'processCode', flex: 1,
            },
            {
                headerName: this.l('Process Name'),
                headerTooltip: this.l('Process Name'), field: 'processName', flex: 1,
            },
            {
                headerName: this.l('Process Desc'),
                headerTooltip: this.l('Process Desc'), field: 'processDesc', flex: 1,
            },
            {
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'), field: 'processGroup', flex: 1,type: 'rightAligned',
            },
            {
                headerName: this.l('Group Name'),
                headerTooltip: this.l('Group Name'), field: 'groupName', flex: 1,
            },
            {
                headerName: this.l('Process Subgroup'),
                headerTooltip: this.l('Process Subgroup'), field: 'processSubgroup', flex: 1,type: 'rightAligned',
            },
            {
                headerName: this.l('Subgroup Name'),
                headerTooltip: this.l('Subgroup Name'), field: 'subgroupName', flex: 1,
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
        this._mstwptPaintingProcessService.getAll(this.processCode, this.processName, '', '', '', '', this.paginationParams.sorting, this.paginationParams.skipCount, this.paginationParams.pageSize)
            .subscribe((result) => {
                this.rowData = result.items;
            });
    }

    clearTextSearch() {
        this.processCode = '';
        this.processName = '';
        this.searchDatas();
    }

    onSelectionChanged(params) {
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRow, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRow.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString +=
                ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector(
            '#selectedRows'
        ) as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._mstwptPaintingProcessService.getAll(this.processCode, this.processName, '', '', '', '', this.paginationParams.sorting, this.paginationParams.skipCount, this.paginationParams.pageSize);
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstPtsPaintingProcessDto[]; }; }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstPtsPaintingProcessDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount =
            (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(
                result.totalCount / this.paginationParams.pageSize
            );
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
            .pipe(finalize(() =>
                this.gridTableService.selectFirstRow(this.dataParams!.api)
            ))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(
                    result.totalCount / (this.paginationParams.pageSize ?? 0)
                );
                this.isLoading = false;

            })
    }

    deleteRow(system: MstPtsPaintingProcessDto): void {
        this.message.confirm(
            this.l('AreYouSureToDelete'), "Delete Row",
            isConfirmed => {
                if (isConfirmed) {
                    this._mstwptPaintingProcessService.delete(system.id).subscribe(() => {
                        this.callBackDataGrid(this.dataParams!);
                        this.notify.success(this.l('SuccessfullyDeleted'));
                        this.notify.info(this.l('SuccessfullyDeleted'));
                    });
                }
            }
        );
    }


    exportToExcel(): void {
        this.isLoading = true;
        this._mstwptPaintingProcessService.getPaintingProcessToExcel(0, '', '', '', 0, '', 0, '', '')
            .subscribe(result => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
    
    
}
