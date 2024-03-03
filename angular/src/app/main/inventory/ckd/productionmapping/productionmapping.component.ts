import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdProductionMappingDto, InvCkdProductionMappingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ImportProductionMappingComponent } from './import-production-mapping.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './productionmapping.component.html',
    styleUrls: ['./productionmapping.component.less'],
})
export class ProductionMappingComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportProductionMappingComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdProductionMappingDto = new InvCkdProductionMappingDto();
    saveSelectedRow: InvCkdProductionMappingDto = new InvCkdProductionMappingDto();
    datas: InvCkdProductionMappingDto = new InvCkdProductionMappingDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    date = new Date();

    periodIdList: any[] = [];
    loadColumdef: CustomColDef[] = [];
    planSequence
    shop: string = '';
    model: string = '';
    lotNo: string = '';
    noInLot: string = '';
    grade: string = '';
    bodyNo: string = '';
    dateIn
    dateInFrom: any;
    dateInTo: any;
    timeIn: string = '';
    useLotNo: string = '';
    supplierNo: string = '';
    partId
    quantity
    periodId
    wipId
    inStockId
    mappingId
    id;
    changedProductionMapping: number[] = [];
    selectId;
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
        private _service: InvCkdProductionMappingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'from_Date',
                cellClass: ['text-center']
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'to_Date',
                cellClass: ['text-center'],
            },
        ];


        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Plan Sequence'), headerTooltip: this.l('Plan Sequence'), field: 'planSequence', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1 },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model', flex: 1

            },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', flex: 1 },
            { headerName: this.l('No In Lot'), headerTooltip: this.l('No In Lot'), field: 'noInLot', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade', flex: 1 },
            { headerName: this.l('Body No'), headerTooltip: this.l('Body No'), field: 'bodyNo', flex: 1 },
            {
                headerName: this.l('Date In'),
                headerTooltip: this.l('Date In'),
                flex: 1,
                field: 'dateIn',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
            },
            {
                headerName: this.l('Time In'), headerTooltip: this.l('Time In'), field: 'timeIn', flex: 1,
                valueGetter: (params) => params.data.timeIn.slice(0, 8)
            },
            { headerName: this.l('Use Lot No'), headerTooltip: this.l('Use Lot No'), field: 'useLotNo', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            //  { headerName: this.l('Part Id'), headerTooltip: this.l('Part Id'), field: 'partId',flex: 1 },
            //  { headerName: this.l('Quantity'), headerTooltip: this.l('Quantity'), field: 'quantity', type: 'rightAligned',flex: 1 },
            { headerName: this.l('Period Id'), headerTooltip: this.l('Period Id'), field: 'periodId', type: 'rightAligned', flex: 1 },
            //     { headerName: this.l('Wip Id'), headerTooltip: this.l('Wip Id'), field: 'wipId', type: 'rightAligned',flex: 1 },
            //     { headerName: this.l('In Stock Id'), headerTooltip: this.l('In Stock Id'), field: 'inStockId', type: 'rightAligned',flex: 1 },
            //     { headerName: this.l('Mapping Id'), headerTooltip: this.l('Mapping Id'), field: 'mappingId', type: 'rightAligned',flex: 1 },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.date.setDate(this.date.getDate() - 30);
        this.dateInFrom = this.date;
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedProductionMapping = result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.periodId,
            this.shop,
            this.bodyNo,
            this.lotNo,
            this.useLotNo,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.dateInFrom),
            this._dateTimeService.convertToDatetime(this.dateInTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                //   this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.id = '';
        this.periodId = '';
        this.shop = '';
        this.bodyNo = '';
        this.lotNo = '';
        this.date = new Date();
        this.date.setDate(this.date.getDate() - 30);
        this.dateInFrom = this.date;
        this.dateInTo = '';
        this.useLotNo = '';
        this.supplierNo = '';
        this.searchDatas();
    }


    // autoSize() {
    //     const allColumnIds: string[] = [];
    //     this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
    //         allColumnIds.push(column.getId());
    //     });
    //     this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView() {
    //     setTimeout(() => {
    //         this.autoSize();
    //     }, 500)
    // }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.periodId,
            this.shop,
            this.bodyNo,
            this.lotNo,
            this.useLotNo,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.dateInFrom),
            this._dateTimeService.convertToDatetime(this.dateInTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdProductionMappingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdProductionMappingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
        console.log("ID = "+ this.selectId)
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
            //  this.resetGridView();
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
                //   this.resetGridView();
            });
    }


    buttonAction(_shop: string, i: number) {


        let _btnUncheck = document.querySelector('.actionButton_w' + i + '.active');
        if (_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.shop = '';

        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + i);
            if (_btn) _btn.classList.add('active');

            this.shop = _shop;
        }

        this.searchDatas();
    }


    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getProductionMappingToExcel(
            this.periodId,
            this.shop,
            this.bodyNo,
            this.lotNo,
            this.useLotNo,
            this.supplierNo,
            this._dateTimeService.convertToDatetime(this.dateInFrom),
            this._dateTimeService.convertToDatetime(this.dateInTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));

            });
    }
}
