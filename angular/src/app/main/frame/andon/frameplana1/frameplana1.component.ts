import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { FrmAdoFramePlanA1Dto, FrmAdoFramePlanA1ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditFramePlanA1ModalComponent } from './create-or-edit-frameplana1-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportFramePlanA1Component } from './import-frameplana1-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './frameplana1.component.html',
    styleUrls: ['./frameplana1.component.less'],
})
export class FramePlanA1Component extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalFramePlanA1', { static: true }) createOrEditModalFramePlanA1: | CreateOrEditFramePlanA1ModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportFramePlanA1Component| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: FrmAdoFramePlanA1Dto = new FrmAdoFramePlanA1Dto();
    saveSelectedRow: FrmAdoFramePlanA1Dto = new FrmAdoFramePlanA1Dto();
    datas: FrmAdoFramePlanA1Dto = new FrmAdoFramePlanA1Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    no: number = 0;
    model: string = '';
    lotNo: string = '';
    noInLot: number = 0;
    bodyNo: string = '';
    color: string = '';
    vinNo: string = '';
    frameId: string = '';
    status: string = '';
    planMonth: any; // planMonth: any;
    planDate: any;
    grade: string = '';
    version: string = '';
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
        private _service: FrmAdoFramePlanA1ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService
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
                headerName: this.l('No'),
                headerTooltip: this.l('No'),
                field: 'no',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex: 1
            },
            {
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                flex: 1,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Body No'),
                headerTooltip: this.l('Body No'),
                field: 'bodyNo',
                flex: 1
            },
            {
                headerName: this.l('Color'),
                headerTooltip: this.l('Color'),
                field: 'color',
                flex: 1
            },
            {
                headerName: this.l('Vin No'),
                headerTooltip: this.l('Vin No'),
                field: 'vinNo',
                width: 150
            },
            {
                headerName: this.l('Frame Id'),
                headerTooltip: this.l('Frame Id'),
                field: 'frameId',
                flex: 1
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                flex: 1
            },
            {
                headerName: this.l('Plan Month'),
                headerTooltip: this.l('Plan Month'),
                field: 'planMonth', valueGetter: (params) => this.pipe.transform(params.data?.planMonth, 'MM/yyyy'),
                width: 90
            },
            {
                headerName: this.l('Plan Date'),
                headerTooltip: this.l('Plan Date'),
                field: 'planDate', valueGetter: (params) => this.pipe.transform(params.data?.planDate, 'dd/MM/yyyy'),
                width: 90
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1
            },
            {
                headerName: this.l('Version'),
                headerTooltip: this.l('Version'),
                field: 'version',
                flex: 1,
                type: 'rightAligned',
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
    getMonthNow() {
        var today = new Date();
        var date = (today.getMonth() + 1) + '/' + today.getFullYear();
        return date;
    }

    formatMonth(m: number){
        if(m < 10) return "0" + m;
        else return m;
    }
    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        let _date = "";
        if(this.planMonth != "" && this.planMonth != undefined && this.planMonth != null) {
            _date = this.planMonth.getFullYear() + '-' + (this.formatMonth(this.planMonth.getMonth() + 1));
        }

        this._service.getAll(
            this.model,
            this.lotNo,
            this.bodyNo,
            this.vinNo,
            _date,
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
            this.model = '',
            this.lotNo = '',
            this.bodyNo = '',
            this.vinNo = '',
            this.planMonth = '',
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
        let _date = "";
        if(this.planMonth != "" && this.planMonth != undefined && this.planMonth != null) {
            _date = this.planMonth.getFullYear() + '-' + (this.formatMonth(this.planMonth.getMonth() + 1));
        }

        return this._service.getAll(
            this.model,
            this.lotNo,
            this.bodyNo,
            this.vinNo,
            _date,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => FrmAdoFramePlanA1Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new FrmAdoFramePlanA1Dto();
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

    deleteRow(system: FrmAdoFramePlanA1Dto): void {
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
        let _date = "";
        if(this.planMonth != "" && this.planMonth != undefined && this.planMonth != null) {
            _date = this.planMonth.getFullYear() + '-' + (this.formatMonth(this.planMonth.getMonth() + 1));
        }    
        this._service.getFramePlanA1ToExcel(
            this.model,
            this.lotNo,
            this.bodyNo,
            this.vinNo,
            _date
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }


}