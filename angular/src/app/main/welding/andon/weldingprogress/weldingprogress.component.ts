import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WldAdoWeldingProgressDto, WldAdoWeldingProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditWeldingProgressModalComponent } from './create-or-edit-weldingprogress-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './weldingprogress.component.html',
    styleUrls: ['./weldingprogress.component.less'],
})
export class WeldingProgressComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalWeldingProgress', { static: true }) createOrEditModalWeldingProgress: | CreateOrEditWeldingProgressModalComponent | undefined;
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

    selectedRow: WldAdoWeldingProgressDto = new WldAdoWeldingProgressDto();
    saveSelectedRow: WldAdoWeldingProgressDto = new WldAdoWeldingProgressDto();
    datas: WldAdoWeldingProgressDto = new WldAdoWeldingProgressDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    scanningId
    bodyNo: string = '';
    color: string = '';
    colorOrg: string = '';
    scanTypeCd: string = '';
    scanLocation: string = '';
    scanTime: any;
    scanTimeFrom: any;
    scanTimeTo: any;
    scanValue: string = '';
    mode: string = '';
    processGroup: number;
    processSubgroup: number;
    processSeq: number;
    conveyerStatus: number;
    lastConveyerRun: any;
    tcStatus: number;
    model: string = '';
    lotNo: string = '';
    noInLot: number;
    defectDesc: string = '';
    targetRepair: string = '';
    location: string = '';
    duplicateLot: string = '';
    weldTransfer: string = '';
    rescanBodyNo: string = '';
    rescanLotNo: string = '';
    rescanMode: string = '';
    sequenceNo: string = '';
    errorCd: string = '';
    isRescan: string = '';
    isPaintOut: string = '';
    isActive: string = '';
    frameworkComponents: FrameworkComponent;

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
        private _service: WldAdoWeldingProgressServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Scanning Id'),
                headerTooltip: this.l('Scanning Id'),
                field: 'scanningId',
                width: 90,
            },
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
                headerName: this.l('Color Org'),
                headerTooltip: this.l('Color Org'),
                field: 'colorOrg',
                width: 90,
            },
            {
                headerName: this.l('Scan Type Cd'),
                headerTooltip: this.l('Scan Type Cd'),
                field: 'scanTypeCd',
                width: 110,
            },
            {
                headerName: this.l('Scan Location'),
                headerTooltip: this.l('Scan Location'),
                field: 'scanLocation',
                width: 110,
            },
            {
                headerName: this.l('Scan Time'),
                headerTooltip: this.l('Scan Time'),
                field: 'scanTime', valueGetter: (params) => this.pipe.transform(params.data?.scanTime, 'dd/MM/yyyy HH:mm:ss'),
                width: 90,
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
                width: 110,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Process Subgroup'),
                headerTooltip: this.l('Process Subgroup'),
                field: 'processSubgroup',
                width: 130,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Process Seq'),
                headerTooltip: this.l('Process Seq'),
                field: 'processSeq',
                width: 110,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Conveyer Status'),
                headerTooltip: this.l('Conveyer Status'),
                field: 'conveyerStatus',
                width: 140,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Last Conveyer Run'),
                headerTooltip: this.l('Last Conveyer Run'),
                field: 'lastConveyerRun',
                valueGetter: (params) => this.pipe.transform(params.data?.lastConveyerRun,'dd/MM/yyyy'),
                width: 130,
            },
            {
                headerName: this.l('Tc Status'),
                headerTooltip: this.l('Tc Status'),
                field: 'tcStatus',
                width: 90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
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
                headerName: this.l('Defect Desc'),
                headerTooltip: this.l('Defect Desc'),
                field: 'defectDesc',
                width: 100,
            },
            {
                headerName: this.l('Target Repair'),
                headerTooltip: this.l('Target Repair'),
                field: 'targetRepair',
                width: 110,
            },
            {
                headerName: this.l('Location'),
                headerTooltip: this.l('Location'),
                field: 'location',
                width: 90,
            },
            {
                headerName: this.l('Duplicate Lot'),
                headerTooltip: this.l('Duplicate Lot'),
                field: 'duplicateLot',
                width: 110,
            },
            {
                headerName: this.l('Weld Transfer'),
                headerTooltip: this.l('Weld Transfer'),
                field: 'weldTransfer',
                width: 120,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.weldTransfer == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.weldTransfer == "Y") ? 'btnActive' : 'btnInActive',
                },

            },
            {
                headerName: this.l('Rescan Body No'),
                headerTooltip: this.l('Rescan Body No'),
                field: 'rescanBodyNo',
                width: 120,
            },
            {
                headerName: this.l('Rescan Lot No'),
                headerTooltip: this.l('Rescan Lot No'),
                field: 'rescanLotNo',
                width: 110,
            },
            {
                headerName: this.l('Rescan Mode'),
                headerTooltip: this.l('Rescan Mode'),
                field: 'rescanMode',
                width: 100,
            },
            {
                headerName: this.l('Sequence No'),
                headerTooltip: this.l('Sequence No'),
                field: 'sequenceNo',
                width: 110,
            },
            {
                headerName: this.l('Error Cd'),
                headerTooltip: this.l('Error Cd'),
                field: 'errorCd',
                width: 90,
            },
            {
                headerName: this.l('Is Rescan'),
                headerTooltip: this.l('Is Rescan'),
                field: 'isRescan',
                width: 120,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isRescan == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isRescan == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isRescan == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isRescan == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },
            {
                headerName: this.l('Is Paint Out'),
                headerTooltip: this.l('Is Paint Out'),
                field: 'isPaintOut',
                width: 120,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isPaintOut == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isPaintOut == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isPaintOut == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isPaintOut == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },

            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                width: 90,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                //cellRenderer: (params: any) => `<input type="checkbox" class="checkbox" disabled="true" ${ params.data.isActive ? 'checked' : ''} />`,
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
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

    searchDatas(): void {
        this._service.getAll(
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.model,
            this.lotNo,
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
        this.bodyNo = '',
        this.scanTimeFrom= '',
        this.scanTimeTo = '',
        this.model = '',
        this.lotNo = '',
        this.processGroup = undefined,
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


    searchDatas_WeldIn(_processGroup: number, i: number) {

        let _btnUncheck = document.querySelector('.actionButton_w'+i+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.processGroup = undefined;

        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+i);
            if(_btn) _btn.classList.add('active');

            this.processGroup = _processGroup
        }

        this.searchDatas();
    }




    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            //this.scanningId,
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.model,
            this.lotNo,
            this.processGroup,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => WldAdoWeldingProgressDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new WldAdoWeldingProgressDto();
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

    deleteRow(system: WldAdoWeldingProgressDto): void {
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
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getWeldingProgressToExcel(
            this.bodyNo,
            this._dateTimeService.convertToDatetime(this.scanTimeFrom),
            this._dateTimeService.convertToDatetime(this.scanTimeTo),
            this.model,
            this.lotNo,
            this.processGroup,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
            });
    }


}