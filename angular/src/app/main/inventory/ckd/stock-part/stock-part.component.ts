import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockPartDto, InvCkdStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewStockPartDetailModalComponent } from './view-stockpart-detail-modal.component';
import { ViewMaterialByIdModalComponent } from './view-material-modal.component';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { CheckStockPartModalComponent } from './check-stock-part-modal.component';
import { DateTime } from 'luxon';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './stock-part.component.html',
})
export class StockPartComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewStockPartDetailModal', { static: true }) viewStockPartDetailModal:| ViewStockPartDetailModalComponent| undefined;
    @ViewChild('viewMaterialModal', { static: true }) viewMaterialModal:| ViewMaterialByIdModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('checkStockPartModal', { static: true }) checkStockPartModal:| CheckStockPartModalComponent| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdStockPartDto = new InvCkdStockPartDto();
    saveSelectedRow: InvCkdStockPartDto = new InvCkdStockPartDto();
    datas: InvCkdStockPartDto = new InvCkdStockPartDto();
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
	partListId
	materialId
	qty
	workingDate = new Date();
	periodId
	lastCalDatetime
	isActive : string = '' ;
    selectedIdMaterial;
    date = new Date();
    negativeStock:boolean = false;
    cfc;
    supplierNo;

    tabDataParams = { partNo: "", workingDate: undefined, _event: "Search" }

    _selectId;
    hasEdit = false;

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
        private _service: InvCkdStockPartServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService,
        private eventBus: EventBusService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNoNormalizedS4',flex: 1},
            {headerName: this.l('Color Sfx'),headerTooltip: this.l('Color Sfx'),field: 'colorSfx',flex: 1},
            {headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            {headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
           // {headerName: this.l('Part No Normalized'),headerTooltip: this.l('Part No Normalized'),field: 'partNoNormalized',flex: 1},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName',flex: 1},
			{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',type: 'rightAligned',flex: 1, comparator: this._formStoringService.decimalComparator, valueFormatter: (params) => params.data?.qty == 0 ? 0 : this._fm.formatMoney_decimal(params.data?.qty),aggFunc: this.SumA},
            {headerName: this.l('Working Date'), headerTooltip: this.l('Working Date'),field: 'workingDate', valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/yyyy'),flex: 1,},
			{headerName: this.l('Last Cal Datetime'),headerTooltip: this.l('Last Cal Datetime'),field: 'lastCalDatetime',flex: 1,valueGetter: (params) => this.pipe.transform(params.data?.lastCalDatetime, 'dd/MM/yyyy HH:mm:ss'),},
			{headerName: this.l('MaterialId'),headerTooltip: this.l('MaterialId'),field: 'materialId',flex: 1,type: 'rightAligned'},
            {
                headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},
            },
            // {
            //     headerName: this.l('Is Deleted'),headerTooltip: this.l('Is Deleted'),field: 'isDeleted', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            //     buttonDefTwo: { text: params => (params.data?.isDeleted == 1) ? 'Deleted' : 'NotDeleted',iconName: 'fa-solid fa-trash',className: params => (params.data?.isDeleted == 1) ? 'btnActive' : 'btnInActive',},
            // }
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.negativeStock,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            var grandTotal=0;
            if(result.totalCount>0) {
                grandTotal = result.items[0].grandTotal;
                var rows = this.createRow(1,grandTotal);
                this.dataParams!.api.setPinnedBottomRowData(rows);
            }else{
                this.dataParams!.api.setPinnedBottomRowData(null);
                this.hasEdit = false;
            }
            this.isLoading = false;
        });
    }

    clearTextSearch() {
        this.workingDate = this.date;
        this.negativeStock = false;
        this.partNo = '',
        this.colorSfx= "";
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.negativeStock,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdStockPartDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdStockPartDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        //get materialId
        this.selectedIdMaterial = this.saveSelectedRow.materialId;
        this.tabDataParams.partNo = this.selectedRow.partNo;
        this.tabDataParams.workingDate = this.selectedRow.workingDate;

        this._selectId = this.saveSelectedRow.id;
        if(this._selectId != null){
            if(this.saveSelectedRow.isDeleted) this.hasEdit = false;
            else this.hasEdit = true;
        }else this.hasEdit = false;
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
            // var grandTotal=0;
            // if(result.totalCount>0) grandTotal = result.items[0].grandTotal;
            // var rows = this.createRow(1,grandTotal);
            // this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
            // this.gridTableService.selectFirstRow(this.dataParams!.api)
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
                this.gridTableService.selectFirstRow(this.dataParams!.api);
                // var rows = this.createRow(1);
                // this.dataParams!.api.setPinnedBottomRowData(rows);
                this.isLoading = false;
            });
    }

    getViewDataDetail(){
        this.viewStockPartDetailModal.show(this.selectedRow);
    }

    getViewMaterial(){
        if(this.selectedIdMaterial != null){
            this.viewMaterialModal.show(this.selectedIdMaterial);
        }

    }

    getCheckStock(){
        this.checkStockPartModal.show();
    }

    openTab(tabKey: string, tabName: string) {
        this.eventBus.emit({
            type: 'openComponent',
            functionCode: tabKey,
            tabHeader: tabName,
            params: this.tabDataParams,
        });
        // this.eventBus.setData(this.data);
    }

    getDataDetailbyStockPart(){
        // this.saveSelectedRow.w

        window.open("/app/main/inventory/ckd/stock-receiving?p=" + this.saveSelectedRow.partNo +"&c="+ this.saveSelectedRow.cfc + "&s="+ this.saveSelectedRow.supplierNo + "&m="+ (this.workingDate.getMonth()+1) + "&y=" + this.workingDate.getFullYear() + "&d="+ this.workingDate.getDate(), "stock-receiving", "")
    }
    getstockbalance(){
        window.open("/app/main/inventory/ckd/stock-balance?p=" + this.saveSelectedRow.partNo +"&c="+ this.saveSelectedRow.cfc + "&s="+ this.saveSelectedRow.supplierNo +"&m="+ (this.workingDate.getMonth()+1) + "&y=" + this.workingDate.getFullYear()+ "&d="+ this.workingDate.getDate(), "stock-balance", "")
    }
    getstockissuing(){
        window.open("/app/main/inventory/ckd/stock-issuing?p=" + this.saveSelectedRow.partNo +"&c="+ this.saveSelectedRow.cfc + "&s="+ this.saveSelectedRow.supplierNo + "&m="+ (this.workingDate.getMonth()+1) + "&y=" + this.workingDate.getFullYear()+ "&d="+ this.workingDate.getDate(), "stock-issuing", "")
    }
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getStockPartToExcel(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.negativeStock
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
        this._service.getStockPartByMaterialToExcel(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.negativeStock
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
