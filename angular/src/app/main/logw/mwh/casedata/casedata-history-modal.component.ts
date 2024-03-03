
import { DatePipe } from '@angular/common';
import { Component, ElementRef, EventEmitter, Injector, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwMwhCaseDataDto, LgwMwhCaseDataServiceProxy } from '@shared/service-proxies/service-proxies';
import ceil from 'lodash-es/ceil';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


@Component({
    selector: 'casedata-history-modal',
    templateUrl: './casedata-history-modal.component.html',
    styleUrls: ['./casedata-history-modal.component.less']
})
export class CaseDataHistoryComponent extends AppComponentBase {

    @ViewChild('imgInput', { static: false }) InputVar: ElementRef;
    @ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() close = new EventEmitter();
    @Output() end = new EventEmitter();


    defaultColDefs: CustomColDef[] = [];
    pipe = new DatePipe('en-US');
    saveSelectedRow: LgwMwhCaseDataDto = new LgwMwhCaseDataDto();
    selectedRow: LgwMwhCaseDataDto = new LgwMwhCaseDataDto();
    loading: boolean;
    isLoading: boolean = false;
    caseNo: string = '';
    lotNo: string = '';
    grade: string = '';
    model: string = '';
    caseQty: number = 0;
    containerNo: string = '';
    renban: string = '';
    supplierNo: string = '';
    orderType: string = '';
    casePrefix: string = '';
    prodLine: string = '';
    pxpCaseId
    contScheduleId
    status: string = '';
    devanningDate: any;
    date: Date = new Date();
    startDevanningDate: any;
    finishDevanningDate: any;
    zoneCd: string = '';
    areaCd: string = '';
    locId
    locCd: string = '';
    locDate: any;
    locBy: string = '';
    shop: string = '';
    isBigpart: string = '';
    isActive: string = '';
    rowSelection = 'multiple';

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

    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    rowData = [];

    dataParams: GridParams | undefined;

    constructor(
        injector: Injector,
        private _service: LgwMwhCaseDataServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService : DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
                width: 95,
            },
            {
                headerName: this.l('Zone Cd'),
                headerTooltip: this.l('Zone Cd'),
                field: 'zoneCd',
                width: 95
            },
            {
                headerName: this.l('Area Cd'),
                headerTooltip: this.l('Area Cd'),
                field: 'areaCd',
                width: 95
            },
            {
                headerName: this.l('Loc Cd'),
                headerTooltip: this.l('Loc Cd'),
                field: 'locCd',
                width: 95
            },
            {
                headerName: this.l('Loc Date'),
                headerTooltip: this.l('Loc Date'),
                field: 'locDate', valueGetter: (params) => this.pipe.transform(params.data?.locDate, 'dd/MM/yyyy hh:MM:ss'),
                width: 130
            },
        ];
        { }
    }
    ngOnInit() {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.searchDatas();
    }

    show(val?: any | undefined) {

        this.caseNo = val;
        this.searchDatas();
        this.modal.show();
    }

    closeModal() {
        this.modal.hide();
    }

    searchDatas(): void {
        // this.startDevanningDate = this.date;
        // this.finishDevanningDate = this.date;
        this._service.getCaseDataHisByCaseNo(
            this._dateTimeService.convertToDatetime(this.startDevanningDate),
            this._dateTimeService.convertToDatetime(this.finishDevanningDate),
            this.caseNo,       
            this.renban,
            this.zoneCd,
            this.areaCd,       
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        // this.startDevanningDate = this.date;
        // this.finishDevanningDate = this.date;
        return this._service.getCaseDataHisByCaseNo(
            this._dateTimeService.convertToDatetime(this.startDevanningDate),
            this._dateTimeService.convertToDatetime(this.finishDevanningDate),
            this.caseNo,       
            this.renban,
            this.zoneCd,
            this.areaCd,   
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwMwhCaseDataDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwMwhCaseDataDto();
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

}
