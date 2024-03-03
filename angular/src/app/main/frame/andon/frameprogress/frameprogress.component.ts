import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFrameProgressDto, FrmAdoFrameProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './frameprogress.component.html',
    styleUrls: ['./frameprogress.component.less'],
})
export class FrameProgressComponent extends AppComponentBase implements OnInit {
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

    selectedRow: FrmAdoFrameProgressDto = new FrmAdoFrameProgressDto();
    saveSelectedRow: FrmAdoFrameProgressDto = new FrmAdoFrameProgressDto();
    datas: FrmAdoFrameProgressDto = new FrmAdoFrameProgressDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    scanningId
    bodyNo: string = '';
    color: string = '';
    scanTypeCd: string = '';
    scanLocation: string = '';
    scanTime: any;
    scanTimeFrom: any;
    scanTimeTo: any;
    scanValue: string = '';
    mode: string = '';
    processGroup: number = 0;
    processSubgroup: number = 0;
    vinNo: string = '';
    frameNo: string = '';
    model: string = '';
    grade: string = '';
    lotNo: string = '';
    noInLot: number = 0;
    sequenceNo: string = '';
    location: string = '';
    andonTransfer: string = '';
    rescanBodyNo: string = '';
    rescanLotNo: string = '';
    rescanMode: string = '';
    errorCd: string = '';
    isRescan: string = '';
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
        private _service: FrmAdoFrameProgressServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55,
            },
            // {
            //     headerName: this.l('Scanning Id'),
            //     headerTooltip: this.l('Scanning Id'),
            //     field: 'scanningId',
            //     width: 120,
            //     type: 'rightAligned',
            // },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 90,
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                width: 90,
            },
            {
                headerName: this.l('Scan Type Cd'),
                headerTooltip: this.l('Scan Type Cd'),
                field: 'scanTypeCd',
                width: 120,
            },
            {
                headerName: this.l('Scan Location'),
                headerTooltip: this.l('Scan Location'),
                field: 'scanLocation',
                width: 120,
            },
            {
                headerName: this.l('Scan Time'),
                headerTooltip: this.l('Scan Time'),
                field: 'scanTime', valueGetter: (params) => this.pipe.transform(params.data?.scanTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 120,
            },
            {
                headerName: this.l('Scan Value'),
                headerTooltip: this.l('Scan Value'),
                field: 'scanValue',
                width: 90,
            },
            {
                headerName: this.l('Mode'),
                headerTooltip: this.l('Mode'),
                field: 'mode',
                width: 90,
            },
            {
                headerName: this.l('Process Group'),
                headerTooltip: this.l('Process Group'),
                field: 'processGroup',
                width: 120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Process Subgroup'),
                headerTooltip: this.l('Process Subgroup'),
                field: 'processSubgroup',
                width: 130,
            },
            {
                headerName: this.l('Vin No'),
                headerTooltip: this.l('Vin No'),
                field: 'vinNo',
                width: 140,
            },
            {
                headerName: this.l('Frame No'),
                headerTooltip: this.l('Frame No'),
                field: 'frameNo',
                width: 120,
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 90,
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 90,
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 90,
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width: 90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'),
                field: 'sequenceNo',
                width: 120,
            },
            {
                headerName: this.l('Location'),
                headerTooltip: this.l('Location'),
                field: 'location',
                width: 90,
            },
            {
                headerName: this.l('Andon Transfer'),
                headerTooltip: this.l('Andon Transfer'),
                field: 'andonTransfer',
                width: 150,
            },
            {
                headerName: this.l('Rescan Body No'),
                headerTooltip: this.l('Rescan Body No'),
                field: 'rescanBodyNo',
                width: 150,
            },
            {
                headerName: this.l('Rescan Lot No'),
                headerTooltip: this.l('Rescan Lot No'),
                field: 'rescanLotNo',
                width: 90,
            },
            {
                headerName: this.l('Rescan Mode'),
                headerTooltip: this.l('Rescan Mode'),
                field: 'rescanMode',
                width: 150,
            },
            {
                headerName: this.l('Error Cd'),
                headerTooltip: this.l('Error Cd'),
                field: 'errorCd',
                width: 130,
            },
            {
                headerName: this.l('Is Rescan'),
                headerTooltip: this.l('Is Rescan'),
                field: 'isRescan',
                width: 120,
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
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
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas_Frame(){
        this.scanLocation = 'FR_FR',
        this.processGroup = 101
        this.searchDatas();
    }

    searchDatas_Frame_A(){
        this.scanLocation = 'FR_C0',
        this.processGroup = 111
        this.searchDatas();
    }

    searchDatas_Frame_Chasis(){
        this.scanLocation = 'FR_C1',
        this.processGroup = 121
        this.searchDatas();
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.vinNo,
            this.frameNo,
            this.model,
            this.grade,
            this.scanLocation,
            this.processGroup,
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
        this.scanningId = '',
        this.scanTimeFrom = '',
        this.scanTimeTo = '',
            this.bodyNo = '',
            this.vinNo = '',
            this.frameNo = '',
            this.model = '',
            this.grade = '',
            this.scanLocation = '',
            this.processGroup = 0
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
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.vinNo,
            this.frameNo,
            this.model,
            this.grade,
            this.scanLocation,
            this.processGroup,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => FrmAdoFrameProgressDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new FrmAdoFrameProgressDto();
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

    exportToExcel(): void {
        this.isLoading = true;
        this._service.getFrameProgressToExcel(
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.vinNo,
            this.frameNo,
            this.model,
            this.grade,
            this.scanLocation,
            this.processGroup,
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }


}
