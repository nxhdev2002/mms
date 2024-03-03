import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WldAdoWeldingPlanDto, WldAdoWeldingPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditWeldingPlanModalComponent } from './create-or-edit-weldingplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { ImportWeldingPlanComponent } from './import-weldingplan-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewWeldingPlanModalComponent } from './view-weldingplan-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './weldingplan.component.html',
    styleUrls: ['./weldingplan.component.less'],
})
export class WeldingPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalWeldingPlan', { static: true }) createOrEditModalWeldingPlan: | CreateOrEditWeldingPlanModalComponent | undefined;
    @ViewChild('viewModalWeldingPlan', { static: true }) viewModalWeldingPlan: | ViewWeldingPlanModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportWeldingPlanComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    buttonChange;
    actionButton_w1;
    actionButton_w2;
    actionButton_w3;
    selectedRow: WldAdoWeldingPlanDto = new WldAdoWeldingPlanDto();
    saveSelectedRow: WldAdoWeldingPlanDto = new WldAdoWeldingPlanDto();
    datas: WldAdoWeldingPlanDto = new WldAdoWeldingPlanDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');

    model: string = '';
    lotNo: string = '';
    noInLot: number = 0;
    grade: string = '';
    prodLine: string = '';
    bodyNo: string = '';
    vinNo: string = '';
    color: string = '';
    planTime: string = '';
    requestDate: any;
    requestDateFrom: any;
    requestDateTo: any;
    shift: string = '';
    wInDate: any;
    wOutDate: any;
    edIn: any;
    tInPlanDatetime: any;
    vehicleId: number;
    cfc: string = '';
    weldingId: number;
    assemblyId: number;
    nut1: string = "nut1";
    changedRecordsWeldingPlan: number[] = [];
    selectId;

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
        private _service: WldAdoWeldingPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Model'), headerTooltip: this.l('Model'), field: 'model', width: 100 },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', width: 130 },
            { headerName: this.l('No In Lot'), headerTooltip: this.l('No In Lot'), field: 'noInLot', width: 130, type: 'rightAligned', },
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade', width: 130 },
            { headerName: this.l('Prod Line'), headerTooltip: this.l('Prod Line'), field: 'prodLine', width: 130 },
            { headerName: this.l('Body No'), headerTooltip: this.l('Body No'), field: 'bodyNo', width: 130 },
            { headerName: this.l('Vin No'), headerTooltip: this.l('Vin No'), field: 'vinNo', width: 150 },
            { headerName: this.l('Color'), headerTooltip: this.l('Color'), field: 'color', width: 130 },
            { headerName: this.l('Plan Time'), headerTooltip: this.l('Plan Time'), field: 'planTime', width: 130 },
            { headerName: this.l('Request Date'), headerTooltip: this.l('Request Date'), field: 'requestDate', valueGetter: (params) => this.pipe.transform(params.data?.requestDate, 'dd/MM/yyyy'), width: 130 },
            { headerName: this.l('Shift'), headerTooltip: this.l('Shift'), field: 'shift', width: 130 },
            { headerName: this.l('W In Date'), headerTooltip: this.l('W In Date'), field: 'wInDate', valueGetter: (params) => this.pipe.transform(params.data?.wInDate, 'dd/MM/yyyy'), width: 130 },
            { headerName: this.l('W Out Date'), headerTooltip: this.l('W Out Date'), field: 'wOutDate', valueGetter: (params) => this.pipe.transform(params.data?.wOutDate, 'dd/MM/yyyy'), width: 130 },
            { headerName: this.l('Ed In'), headerTooltip: this.l('Ed In'), field: 'edIn', valueGetter: (params) => this.pipe.transform(params.data?.edIn, 'dd/MM/yyyy'), width: 130 },
            { headerName: this.l('T In Plan Datetime'), headerTooltip: this.l('T In Plan Datetime'), field: 'tInPlanDatetime', valueGetter: (params) => this.pipe.transform(params.data?.tInPlanDatetime, 'dd/MM/yyyy HH:mm:ss'), width: 130 },
            { headerName: this.l('Vehicle Id'), headerTooltip: this.l('Vehicle Id'), field: 'vehicleId', width: 130 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', width: 130 },
            { headerName: this.l('Welding Id'), headerTooltip: this.l('Welding Id'), field: 'weldingId', width: 130 },
            { headerName: this.l('Assembly Id'), headerTooltip: this.l('Assembly Id'), field: 'assemblyId', width: 100 },
        ];
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsWeldingPlan = result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        this._service.getAll(
            this.model,
            this.lotNo,
            this.prodLine,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
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


    searchDatabyProdLine(prodLine): void {
        this._service.getAll(
            this.model,
            this.lotNo,
            prodLine,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
            });
    }


    getDateNow() {
        var today = new Date();
        var date = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        return date;
    }
    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 1000)
    }
    clearTextSearch() {
        this.model = '',
            this.lotNo = '',
            this.vinNo = '',
            this.requestDateFrom = null,
            this.requestDateTo = null,
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.model,
            this.lotNo,
            this.prodLine,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => WldAdoWeldingPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new WldAdoWeldingPlanDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            // this.resetGridView();
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
                // this.resetGridView();
            });
    }

    deleteRow(system: WldAdoWeldingPlanDto): void {
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
        this._service.getWeldingPlanToExcel(
            this.model,
            this.lotNo,
            this.prodLine,
            this.vinNo,
            this._dateTimeService.convertToDatetime(this.requestDateFrom),
            this._dateTimeService.convertToDatetime(this.requestDateTo),
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    buttonAction(i: number, prodLine: string) {


        let _btnUncheck = document.querySelector('.actionButton_w'+i+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.prodLine ='';
            this.searchDatas();
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+i);
            if(_btn) _btn.classList.add('active');
            this.searchDatabyProdLine(prodLine);
        }
    }

}
