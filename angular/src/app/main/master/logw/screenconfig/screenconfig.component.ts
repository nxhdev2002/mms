import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwScreenConfigDto, MstLgwScreenConfigServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditScreenConfigModalComponent } from './create-or-edit-screenconfig-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './screenconfig.component.html',
    styleUrls: ['./screenconfig.component.less'],
})
export class ScreenConfigComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalScreenConfig', { static: true }) createOrEditModalScreenConfig: | CreateOrEditScreenConfigModalComponent | undefined;
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

    selectedRow: MstLgwScreenConfigDto = new MstLgwScreenConfigDto();
    saveSelectedRow: MstLgwScreenConfigDto = new MstLgwScreenConfigDto();
    datas: MstLgwScreenConfigDto = new MstLgwScreenConfigDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    unpackDoneColor: string = '';
    needToUnpackColor: string = '';
    needToUnpackFlash
    confirmFlagColor: string = '';
    confirmFlagSound: string = '';
    confirmFlagPlaytime: number = 0;
    confirmFlagFlash
    delayUnpackColor: string = '';
    delayUnpackSound: string = '';
    delayUnpackPlaytime: number = 0;
    delayUnpackFlash
    callLeaderColor: string = '';
    callLeaderSound: string = '';
    callLeaderPlaytime: number = 0;
    callLeaderFlash
    totalColumnOldShift: number = 0;
    totalColumnSeqA1: number = 0;
    totalColumnSeqA2: number = 0;
    beforeTacktimeColor: string = '';
    beforeTacktimeSound: string = '';
    beforeTacktimePlaytime: number = 0;
    beforeTacktimeFlash
    tackCaseA1: number = 0;
    tackCaseA2: number = 0;
    isActive: string = '';

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
        private _service: MstLgwScreenConfigServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer:
                    (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Unpack Done Color'),
                headerTooltip: this.l('Unpack Done Color'),
                field: 'unpackDoneColor',
                width: 130
            },
            {
                headerName: this.l('Need To Unpack Color'),
                headerTooltip: this.l('Need To Unpack Color'),
                field: 'needToUnpackColor',
                width: 150
            },
            {
                headerName: this.l('Need To Unpack Flash'),
                headerTooltip: this.l('Need To Unpack Flash'),
                field: 'needToUnpackFlash',
                width: 150
            },
            {
                headerName:this.l('Confirm Flag Color'),
                headerTooltip: this.l('Confirm Flag Color'),
                field: 'confirmFlagColor',
                width: 130
            },
            {
                headerName:this.l('Confirm Flag Sound'),
                headerTooltip: this.l('Confirm Flag Sound'),
                field: 'confirmFlagSound',
                width: 150
            },

            {
                headerName: this.l('Confirm Flag Playtime'),
                headerTooltip: this.l('Confirm Flag Playtime'),
                field: 'confirmFlagPlaytime',
                width: 150
            },
            {
                headerName: this.l('Confirm Flag Flash'),
                headerTooltip: this.l('Confirm Flag Flash'),
                field: 'confirmFlagFlash',
                width: 130
            },
            {
                headerName: this.l('Delay Unpack Color'),
                headerTooltip: this.l('Delay Unpack Color'),
                field: 'delayUnpackColor',
                width: 150
            },
            {
                headerName:this.l('Delay Unpack Sound'),
                headerTooltip: this.l('Delay Unpack Sound'),
                field: 'delayUnpackSound',
                width: 150
            },
            {
                headerName: this.l('Delay Unpack Playtime'),
                headerTooltip: this.l('Delay Unpack Playtime'),
                field: 'delayUnpackPlaytime',
                width: 150
            },
            {
                headerName: this.l('Delay Unpack Flash'),
                headerTooltip: this.l('Delay Unpack Flash'),
                field: 'delayUnpackFlash',
                width: 150
            },
            {
                headerName: this.l('Call Leader Color'),
                headerTooltip: this.l('Call Leader Color'),
                field: 'callLeaderColor',
                width: 130
            },
            {
                headerName: this.l('Call Leader Sound'),
                headerTooltip: this.l('Call Leader Sound'),
                field: 'callLeaderSound',
                width: 130
            },
            {
                headerName: this.l('Call Leader Playtime'),
                headerTooltip: this.l('Call Leader Playtime'),
                field: 'callLeaderPlaytime',
                width: 150
            },
            {
                headerName: this.l('Call Leader Flash'),
                headerTooltip: this.l('Call Leader Flash'),
                field: 'callLeaderFlash',
                width: 150
            },
            {
                headerName: this.l('Total Column Old Shift'),
                headerTooltip: this.l('Total Column Old Shift'),
                field: 'totalColumnOldShift',
                width: 150
            },
            {
                headerName: this.l('Total Column Seq A1'),
                headerTooltip: this.l('Total Column Seq A1'),
                field: 'totalColumnSeqA1',
                width: 150
            },
            {
                headerName: this.l('Total Column Seq A2'),
                headerTooltip: this.l('Total Column Seq A2'),
                field: 'totalColumnSeqA2',
                width: 150
            },
            {
                headerName: this.l('Before Tacktime Color'),
                headerTooltip: this.l('Before Tacktime Color'),
                field: 'beforeTacktimeColor',
                width: 150
            },
            {
                headerName: this.l('Before Tacktime Sound'),
                headerTooltip: this.l('Before Tacktime Sound'),
                field: 'beforeTacktimeSound',
                width: 180
            },
            {
                headerName: this.l('Before Tacktime Playtime'),
                headerTooltip: this.l('Before Tacktime Playtime'),
                field: 'beforeTacktimePlaytime',
                width: 180
            },
            {
                headerName: this.l('Before Tacktime Flash'),
                headerTooltip: this.l('Before Tacktime Flash'),
                field: 'beforeTacktimeFlash',
                width: 150
            },
            {
                headerName: this.l('Tack Case A1'),
                headerTooltip: this.l('Tack Case A1'),
                field: 'tackCaseA1',
                width: 130
            },
            {
                headerName: this.l('Tack Case A2'),
                headerTooltip: this.l('Tack Case A2'),
                field: 'tackCaseA2',
                width: 130
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
                width: 90,
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.unpackDoneColor,
            this.needToUnpackColor,

            this.confirmFlagColor,
            this.confirmFlagSound,
            this.confirmFlagPlaytime,

            this.delayUnpackColor,
            this.delayUnpackSound,
            this.delayUnpackPlaytime,

            this.callLeaderColor,
            this.callLeaderSound,
            this.callLeaderPlaytime,

            this.totalColumnOldShift,
            this.totalColumnSeqA1,
            this.totalColumnSeqA2,
            this.beforeTacktimeColor,
            this.beforeTacktimeSound,
            this.beforeTacktimePlaytime,

            this.tackCaseA1,
            this.tackCaseA2,
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
        this.unpackDoneColor = '',
            this.needToUnpackColor = '',
            this.needToUnpackFlash = '',
            this.confirmFlagColor = '',
            this.confirmFlagSound = '',
            this.confirmFlagPlaytime = 0,
            this.confirmFlagFlash = '',
            this.delayUnpackColor = '',
            this.delayUnpackSound = '',
            this.delayUnpackPlaytime = 0,
            this.delayUnpackFlash = '',
            this.callLeaderColor = '',
            this.callLeaderSound = '',
            this.callLeaderPlaytime = 0,
            this.callLeaderFlash = '',
            this.totalColumnOldShift = 0,
            this.totalColumnSeqA1 = 0,
            this.totalColumnSeqA2 = 0,
            this.beforeTacktimeColor = '',
            this.beforeTacktimeSound = '',
            this.beforeTacktimePlaytime = 0,
            this.beforeTacktimeFlash = '',
            this.tackCaseA1 = 0,
            this.tackCaseA2 = 0,
            this.isActive = '',
            this.searchDatas();
    }

    onSelectionMulti(params) {
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
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.unpackDoneColor,
            this.needToUnpackColor,

            this.confirmFlagColor,
            this.confirmFlagSound,
            this.confirmFlagPlaytime,

            this.delayUnpackColor,
            this.delayUnpackSound,
            this.delayUnpackPlaytime,

            this.callLeaderColor,
            this.callLeaderSound,
            this.callLeaderPlaytime,

            this.totalColumnOldShift,
            this.totalColumnSeqA1,
            this.totalColumnSeqA2,
            this.beforeTacktimeColor,
            this.beforeTacktimeSound,
            this.beforeTacktimePlaytime,

            this.tackCaseA1,
            this.tackCaseA2,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgwScreenConfigDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgwScreenConfigDto();
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

    deleteRow(system: MstLgwScreenConfigDto): void {
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
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getScreenConfigToExcel(
             this.unpackDoneColor,
             this.needToUnpackColor,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
    
    
}
