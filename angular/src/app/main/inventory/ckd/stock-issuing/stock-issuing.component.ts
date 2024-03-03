import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockIssuingDto, InvCkdStockIssuingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe, formatDate } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewStockIssuingDetailModalComponent } from './view-stockissuing-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ValidateIssuingModalComponent } from './validate-issuing-modal.component';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ViewTransferLocComponent } from './view-transfer-sloc-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './stock-issuing.component.html',
})
export class StockIssuingComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewStockIssuingDetailModal', { static: true }) viewStockIssuingDetailModal:| ViewStockIssuingDetailModalComponent| undefined;
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal:| ViewMaterialByIdModalComponent| undefined;
    @ViewChild('validateIssuing', { static: true }) validateIssuing:| ValidateIssuingModalComponent;
    @ViewChild('viewTransferLoc', { static: true }) viewTransferLoc:| ViewTransferLocComponent;
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

    StockIssuingId : number;
    changedRecords: number[] = [];
    selectedRow: InvCkdStockIssuingDto = new InvCkdStockIssuingDto();
    saveSelectedRow: InvCkdStockIssuingDto = new InvCkdStockIssuingDto();
    datas: InvCkdStockIssuingDto = new InvCkdStockIssuingDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    partNo : string = '' ;
	partNoNormalized : string = '' ;
	partName : string = '' ;
	partNoNormalizedS4 : string = '' ;
	colorSfx : string = '' ;
    vinNo: string = '';
    lotNo: string = '';
    noInLot
	partListId
	partListGradeId
	materialId
	qty
	transactionDatetime
	referenceId
	workingDate
	periodId
	isActive : string = '' ;
    workingDateFrom: any;
    workingDateTo: any;
    selectedIdMaterial;
    date = new Date();
    cfc;
    supplierNo;
    partType = 'IMP';

    listPartType = [
        {value: 'ALL' , label: "ALL"},
        {value: 'IMP' , label: "IMP"},
        {value: 'LSP' , label: "LSP"},
        {value: 'PIO' , label: "PIO"},
        {value: 'IHP' , label: "IHP"},
    ];

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
        private _service: InvCkdStockIssuingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService,
        private eventBus: EventBusService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNoNormalizedS4',flex: 1},
            {headerName: this.l('Color Sfx'),headerTooltip: this.l('Color Sfx'),field: 'colorSfx',flex: 1},
            {headerName: this.l('Cfc'),headerTooltip: this.l('Cfc'),field: 'cfc',flex: 1},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo',flex: 1},
            {headerName: this.l('Lot No'),headerTooltip: this.l('Lot No'),field: 'lotNo',flex: 1},
            {headerName: this.l('No In Lot'),headerTooltip: this.l('No In Lot'),field: 'noInLot',flex: 1},
            {headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1, comparator: this._formStoringService.decimalComparator, valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.qty),type: 'rightAligned',
            aggFunc: this.SumA,},
            {headerName: this.l('Vin No'),headerTooltip: this.l('Vin No'),field: 'vinNo',flex: 1},
            {headerName: this.l('Body No'),headerTooltip: this.l('Body No'),field: 'bodyNo',flex: 1},
            {headerName: this.l('Color'),headerTooltip: this.l('Color'),field: 'color',flex: 1},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName',flex: 1},
			{headerName: this.l('Transaction Datetime'),headerTooltip: this.l('Transaction Datetime'),field: 'transactionDatetime',flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.transactionDatetime, 'dd/MM/yyyy HH:mm:ss'),},
            {headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'),field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),flex: 1},
			{headerName: this.l('Material Id'),headerTooltip: this.l('MaterialId'),field: 'materialId',flex: 1,type: 'rightAligned'},

        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        //set date
       // this.date.setDate(this.date.getDate() - 3);

        let urlParams = new URLSearchParams(window.location.search);
        this.partNo = urlParams.get('p');

        if(this.partNo == null || this.partNo == undefined ) this.partNo = "";
        if(this.partNo != "" ){

                this.workingDateFrom = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, 1);
                this.workingDateTo = new Date(Number(urlParams.get('y')), Number(urlParams.get('m'))-1, Number(urlParams.get('d')));
                this.cfc = urlParams.get('c');
                if(this.cfc == null || this.cfc == undefined ) this.cfc = "";
                this.supplierNo = urlParams.get('s')
                if(this.supplierNo == null || this.supplierNo == undefined ) this.supplierNo = "";
                this.searchDatas();

        }

        this.workingDateFrom = this.date;
        this.workingDateTo = this.date;
        this.searchDatas();
        this.fetchChangedRecords();

      //
        // Receive PartNo and Data
        // this.date.setDate(this.date.getDate() - 3);
        // var lastDay = new Date();
        // this.eventBus.getData().subscribe((data) => {

        //     if (data && data != null) {
        //         if (data.params && data.params != null) {

        //             this.partNo = data.params?.partNo;
        //             lastDay = new Date(formatDate(data.params?.workingDate, 'yyyy-MM-dd', 'en-US'));
        //             this.workingDateFrom = new Date(lastDay.getFullYear(), lastDay.getMonth(), 1);
        //             this.workingDateTo = lastDay;

        //             if (data.params?._event == "Search") {
        //                 this.searchDatas();
        //             }

        //         }
        //     }

        // })

        // this.partNo = String(this.eventBus.getData().subscribe((data) => {data.partNo}))
        // this.workingDateFrom = new Date(lastDay.getFullYear(), lastDay.getMonth(), 1);
        // this.workingDateTo = lastDay;


    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecords = result;
        })
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" ) {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }


    resetGridView() {
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSize();
        }, 500)
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(

            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
            this.partType,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() =>{}))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            var grandTotal=0;
            if(result.totalCount>0) {
                grandTotal = result.items[0].grandTotal;
                var rows = this.createRow(1,grandTotal);
                this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
                this.gridTableService.selectFirstRow(this.dataParams!.api);
            }
            else{
                this.dataParams!.api.setPinnedBottomRowData(null);
            }
            this.isLoading = false;
        });
    }

    clearTextSearch() {
         this.partNo = '',
         this.colorSfx = '',
         this.supplierNo = '',
         this.vinNo = '',
         this.cfc = '',
         this.lotNo = '',
         this.noInLot = '',
         this.workingDateFrom = this.date;
         this.workingDateTo = this.date;
         this.partType = 'IMP',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
            this.partType,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdStockIssuingDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected){
            this.selectedIdMaterial = selected.materialId;
            this.StockIssuingId = selected.id;
        }
        this.selectedRow = Object.assign({}, selected);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            // var grandTotal=0;
            // if(result.totalCount>0) grandTotal = result.items[0].grandTotal;
            // var rows = this.createRow(1,grandTotal);
            // this.dataParams!.api.setPinnedBottomRowData(rows);
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
    }

    getViewDataDetail(){
        this.viewStockIssuingDetailModal.show(this.selectedRow);
    }
    getViewDataTranFerLoc(){
        this.viewTransferLoc.show(this.workingDateFrom,this.workingDateTo,this.partType);
    }

    getViewMaterial(){
        if(this.selectedIdMaterial != null){
            this.viewMaterialModal.show(this.selectedIdMaterial);
        }

    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockIssuingToExcel(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
            this.partType,
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }

    exportbyMaterialToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockIssuingByMaterialToExcel(
            this.partNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDateFrom),
            this._dateTimeService.convertToDatetime(this.workingDateTo),
            this.supplierNo,
            this.vinNo,
            this.cfc,
            this.lotNo,
            this.noInLot,
            this.partType
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }


    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

    createRow(count: number, grandTotal:number): any[] {
        let result: any[] = [];

        for (var i = 0; i < count; i++) {
            result.push({
                partNoNormalizedS4: 'Grand Total',
                qty: grandTotal,
            });
        }
        return result;
    }
}
