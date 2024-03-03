import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PtsAdoTotalDelayDto, PtsAdoTotalDelayServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditTotalDelayModalComponent } from './create-or-edit-totaldelay-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './totaldelay.component.html',
    styleUrls: ['./totaldelay.component.less'],
})
export class TotalDelayComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalTotalDelay', { static: true }) createOrEditModalTotalDelay: | CreateOrEditTotalDelayModalComponent | undefined;
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

    selectedRow: PtsAdoTotalDelayDto = new PtsAdoTotalDelayDto();
    saveSelectedRow: PtsAdoTotalDelayDto = new PtsAdoTotalDelayDto();
    datas: PtsAdoTotalDelayDto = new PtsAdoTotalDelayDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    bodyNo: string = '';
    lotNo: string = '';
    color: string = '';
    mode: string = '';
    targetRepair: string = '';
    startRepair: any;
    location: string = '';
    aInPlanDate: any;
    edInAct: any;
    repairIn: any;
    leadtime: any;
    leadtimePlus: any;
    etd: any;
    recoatIn: any;
    isActive: string = '';
    wipId: any;
    progressId: any;

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
        private _service: PtsAdoTotalDelayServiceProxy,
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
                width: 60,
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 75,

            },
            {
                  headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 80,

            },
            {
                 headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                width: 80,

            },
            {
                 headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 80,

            },
            {
                 headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                width: 80,

            },
            {
                 headerName: this.l('Mode'),
                headerTooltip: this.l('Mode'),
                field: 'mode',
                width: 80,

            },
            {
                 headerName : this.l('Target Repair'),
                headerTooltip: this.l('Target Repair'),
                field: 'targetRepair',
                width: 120,
                type: 'rightAligned',

            },
            {
                 headerName: this.l('Start Repair'),
                headerTooltip: this.l('Start Repair'),
                field: 'startRepair',
                valueGetter: (params) => this.pipe.transform(params.data?.startRepair,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                 headerName: this.l('Location'),
                headerTooltip: this.l('Location'),
                field: 'location',
                width: 80,

            },
            {
                 headerName: this.l('A In Plan Date'),
                headerTooltip: this.l('A In Plan Date'),
                field: 'aInPlanDate',
                valueGetter: (params) => this.pipe.transform(params.data?.aInPlanDate,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                 headerName: this.l('Ed In Act'),
                headerTooltip: this.l('Ed In Act'),
                field: 'edInAct', valueGetter: (params) => this.pipe.transform(params.data?.edInAct,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                 headerName: this.l('Repair In'),
                headerTooltip: this.l('Repair In'),
                field: 'repairIn', valueGetter: (params) => this.pipe.transform(params.data?.repairIn,
                'dd/MM/yyyy hh:MM'),
               width: 140
            },
            {
                headerName: this.l('Leadtime'),
                headerTooltip: this.l('Leadtime'),
                field: 'leadtime', valueGetter: (params) => this.pipe.transform(params.data?.leadtime,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                headerName: this.l('Leadtime Plus'),
                headerTooltip: this.l('Leadtime Plus'),
                field: 'leadtimePlus',
                valueGetter: (params) => this.pipe.transform(params.data?.leadtimePlus,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                headerName: this.l('Etd'),
                headerTooltip: this.l('Etd'),
                field: 'etd', valueGetter: (params) => this.pipe.transform(params.data?.etd,
                'dd/MM/yyyy hh:MM'),
               width: 140
            },
            {
                headerName: this.l('Recoat In'),
                headerTooltip: this.l('Recoat In'),
                field: 'recoatIn', valueGetter: (params) => this.pipe.transform(params.data?.recoatIn,
                'dd/MM/yyyy hh:MM'),
               width: 140

            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 90,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                },
            }
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
            this.bodyNo,
            this.lotNo,
            this.color,
            this.mode,
            this.targetRepair,
            this.startRepair,
            this.location,
            this.aInPlanDate,
            this.edInAct,
            this.repairIn,
            this.leadtime,
            this.leadtimePlus,
            this.etd,
            this.recoatIn,
            this.isActive,
            "",
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
            this.lotNo = '',
            this.bodyNo = '',
            this.lotNo = '',
            this.color = '',
            this.mode = '',
            this.targetRepair = '',
            this.startRepair = '',
            this.location = '',
            this.aInPlanDate = '',
            this.edInAct = '',
            this.repairIn = '',
            this.leadtime = '',
            this.leadtimePlus = '',
            this.etd = '',
            this.recoatIn = '',
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
            this.bodyNo,
            this.lotNo,
            this.color,
            this.mode,
            this.targetRepair,
            this.startRepair,
            this.location,
            this.aInPlanDate,
            this.edInAct,
            this.repairIn,
            this.leadtime,
            this.leadtimePlus,
            this.etd,
            this.recoatIn,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PtsAdoTotalDelayDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PtsAdoTotalDelayDto();
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

    deleteRow(system: PtsAdoTotalDelayDto): void {
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
        this._service.getTotalDelayToExcel(
            this.bodyNo,
            this.lotNo,
            this.mode,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
    
    
}
