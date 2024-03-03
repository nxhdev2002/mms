import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwPupPxPUpPlanDto, LgwPupPxPUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPxPUpPlanModalComponent } from './create-or-edit-pxpupplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportPxpUpPlanComponent } from './import-pxpupplan-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './pxpupplan.component.html',
    styleUrls: ['./pxpupplan.component.less'],
})
export class PxPUpPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPxPUpPlan', { static: true }) createOrEditModalPxPUpPlan: | CreateOrEditPxPUpPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportPxpUpPlanComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    saveSelectedRow: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    datas: LgwPupPxPUpPlanDto = new LgwPupPxPUpPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    prodLine: string = '';
    workingDate: any;
    workingDateFrom;
    workingDateTo;
    shift: string = '';
    noInShift: number = 0;
    seqLineIn: number = 0;
    caseNo: string = '';
    supplierNo: string = '';
    upTable: string = '';
    isNoPxpData: string = '';
    unpackingStartDatetime: any;
    unpackingFinishDatetime: any;
    unpackingTime: string = '';
    unpackingDate: any;
    unpackingDatetime: any;
    upLt: number = 0;
    status: string = '';
    delaySecond: number = 0;
    delayConfirmFlag: any;
    whLocation: string = '';
    invoiceDate: any;
    remarks: string = '';
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
        private _service: LgwPupPxPUpPlanServiceProxy,
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
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),
                width: 110
            },
            {
                headerName: this.l('Shift'),
                headerTooltip: this.l('Shift'),
                field: 'shift',
                width: 90
            },
            {
                headerName: this.l('No In Shift'),
                headerTooltip: this.l('No In Shift'),
                field: 'noInShift',
                width: 90
            },
            {
                headerName: this.l('Seq Line In'),
                headerTooltip: this.l('Seq Line In'),
                field: 'seqLineIn',
                width: 90
            },
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
                width: 90
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width: 90
            },
            {
                headerName: this.l('Up Table'),
                headerTooltip: this.l('Up Table'),
                field: 'upTable',
                width: 90
            },
            {
                headerName: this.l('Is No Pxp Data'),
                headerTooltip: this.l('Is No Pxp Data'),
                field: 'isNoPxpData',
                width: 120
            },
            {
                headerName: this.l('Unpacking Start Datetime'),
                headerTooltip: this.l('Unpacking Start Datetime'),
                field: 'unpackingStartDatetime', valueGetter: (params) => this.pipe.transform(params.data?.unpackingStartDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 170
            },
            {
                headerName: this.l('Unpacking Time'),
                headerTooltip: this.l('Unpacking Time'),
                field: 'unpackingTime',
                width: 120
            },
            {
                headerName: this.l('Unpacking Finish Datetime')
                , headerTooltip: this.l('Unpacking Finish Datetime'),
                field: 'unpackingFinishDatetime', valueGetter: (params) => this.pipe.transform(params.data?.unpackingFinishDatetime, 'dd/MM/yyyy HH:mm:ss'),
                width: 220
            },
            {
                headerName: this.l('Unpacking Date')
                , headerTooltip: this.l('Unpacking Date'),
                field: 'unpackingDate', valueGetter: (params) => this.pipe.transform(params.data?.unpackingDate, 'dd/MM/yyyy'),
                width: 170
            },
            {
                headerName: this.l('Unpacking Datetime')
                , headerTooltip: this.l('Unpacking Datetime'),
                field: 'unpackingDatetime', valueGetter: (params) => this.pipe.transform(params.data?.unpackingDatetime, 'dd/MM/yyyy hh:MM:ss'),
                width: 170
            },
            {
                headerName: this.l('Up Lt'),
                headerTooltip: this.l('Up Lt'),
                field: 'upLt',
                width: 90
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                width: 90
            },
            {
                headerName: this.l('Delay Second'),
                headerTooltip: this.l('Delay Second'),
                field: 'delaySecond',
                width: 100
            },
            {
                headerName: this.l('Delay Confirm Flag')
                , headerTooltip: this.l('Delay Confirm Flag'),
                field: 'delayConfirmFlag', valueGetter: (params) => this.pipe.transform(params.data?.delayConfirmFlag, 'dd/MM/yyyy'),
                width: 170
            },
            {
                headerName: this.l('Wh Location'),
                headerTooltip: this.l('Wh Location'),
                field: 'whLocation',
                width: 100
            },
            {
                headerName: this.l('Invoice Date')
                , headerTooltip: this.l('Invoice Date'),
                field: 'invoiceDate', valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                width: 170
            },
            {
                headerName: this.l('Remark'),
                headerTooltip: this.l('Remarks'),
                field: 'remarks',
                width: 90
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

    fn: CommonFunction = new CommonFunction();
    ngAfterViewInit() {
        this.fn.setHeight();
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.fn.setHeight();
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.prodLine,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });
    }

    clearTextSearch() {
        this.prodLine = '',
        this.workingDateFrom = '',
        this.workingDateTo = '',
        this.shift = '',
        this.searchDatas();
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){
        
            setTimeout(()=>{
                this.dataParams.columnApi!.sizeColumnsToFit({
                    suppressColumnVirtualisation: true, 
                });
                this.autoSizeAll();
            }, 1000)}

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
            this.prodLine,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }
    getDateNow() {

        var today = new Date();
        var date = today.getDate()+'/'+(today.getMonth()+1)+'/'+today.getFullYear();
        return date;
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwPupPxPUpPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwPupPxPUpPlanDto();
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
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    deleteRow(system: LgwPupPxPUpPlanDto): void {
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
        this.loadingExportVisible();
        this._service.getPxPUpPlanToExcel(
            this.prodLine,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.shift,
        ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.loadingExportHidden();
            });
    }

    loadingExportVisible(){
        document.querySelectorAll<HTMLElement>('.lds-hourglass')[0].style.visibility = "visible";
        (<HTMLInputElement> document.getElementById("exportToExcel")).disabled = true;
    }
    loadingExportHidden(){
        document.querySelectorAll<HTMLElement>('.lds-hourglass')[0].style.visibility = "hidden";
        (<HTMLInputElement> document.getElementById("exportToExcel")).disabled = false;
    }
}
