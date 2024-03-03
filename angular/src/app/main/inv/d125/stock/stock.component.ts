import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvPeriodDto, InvStockDto, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { RequestModalComponent } from './request-modal.component';

@Component({
    templateUrl: './stock.component.html',
    styleUrls: ['./stock.component.less'],
})
export class StockComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('reportRequestModal', { static: true }) reportRequestModal: | RequestModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvStockDto = new InvStockDto();
    saveSelectedRow: InvStockDto = new InvStockDto();
    datas: InvStockDto = new InvStockDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    selectedRowPeriod: InvPeriodDto = new InvPeriodDto();
    saveSelectedRowPeriod: InvPeriodDto = new InvPeriodDto();
    datasPeriod: InvPeriodDto = new InvPeriodDto();

    periodId: number = 0;
	partNo : string = '' ;
	source : string = '' ;
	carFamilyCode : string = '' ;
	lotNo : string = '' ;
    quantity: number = 0;
	customsDeclareNo: string = '';
	declareDate : any ;
	dcType : string = '' ;
	inStockByLot : string = '' ;
    cif: number = 0;
	tax: number = 0;
	inland: number = 0;
	cost: number = 0;
	amount: number = 0;
    cifVn: number = 0;
	taxVn: number = 0;
	inlandVn: number = 0;
	costVn: number = 0;
	amountVn: number = 0;
    cbbPeriod: any[] = [];
    description: string = '';
    fromDate: any;
    toDate: any;
    status: string = '';
    id: number = 0;

    periodColumdef: CustomColDef[] = [];
    domainColumdef: CustomColDef[] = [];

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
        private _serviceStock: InvStockServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.periodColumdef = [
            {
                headerName: this.l('Id'),
                headerTooltip: this.l('Id'),
                field: 'id',
                width: 50,
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
        ]
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 50,
            },
            {
                headerName: this.l('Period Id'),
                headerTooltip: this.l('Period Id'),
                field: 'periodId',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                width: 120,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Source'),
                headerTooltip: this.l('Source'),
                field: 'source',
                width: 80,
            },
			{
                headerName: this.l('Car Family Code'),
                headerTooltip: this.l('Car Family Code'),
                field: 'carFamilyCode',
                width: 130,
            },
			{
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 70,
            },
			{
                headerName: this.l('Quantity'),
                headerTooltip: this.l('Quantity'),
                field: 'quantity',
                width: 90,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Customs Declare No'),
                headerTooltip: this.l('Customs Declare No'),
                field: 'customsDeclareNo',
                width: 150,
            },
			{
                headerName: this.l('Declare Date'),
                headerTooltip: this.l('Declare Date'),
                field: 'declareDate',
                valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                width: 110,
            },
			{
                headerName: this.l('Dc Type'),
                headerTooltip: this.l('Dc Type'),
                field: 'dcType',
                width: 80,
            },
			{
                headerName: this.l('In Stock By Lot'),
                headerTooltip: this.l('In Stock By Lot'),
                field: 'inStockByLot',
                width: 120,
            },
			{
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Tax'),
                headerTooltip: this.l('Tax'),
                field: 'tax',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Inland'),
                headerTooltip: this.l('Inland'),
                field: 'inland',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Cost'),
                headerTooltip: this.l('Cost'),
                field: 'cost',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Amount'),
                headerTooltip: this.l('Amount'),
                field: 'amount',
                width: 100,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Cif Vn'),
                headerTooltip: this.l('Cif Vn'),
                field: 'cifVn',
                width: 100,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Tax Vn'),
                headerTooltip: this.l('Tax Vn'),
                field: 'taxVn',
                width: 80,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Inland Vn'),
                headerTooltip: this.l('Inland Vn'),
                field: 'inlandVn',
                width: 100,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Cost Vn'),
                headerTooltip: this.l('Cost Vn'),
                field: 'costVn',
                width: 100,
                type: 'rightAligned',
            },
			{
                headerName: this.l('Amount Vn'),
                headerTooltip: this.l('Amount Vn'),
                field: 'amountVn',
                width: 110,
                type: 'rightAligned',
            },
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceStock.getIdInvPeriod().subscribe((result) => {
            this.cbbPeriod = result.items;
        });

    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());


        this._serviceStock.getAll(
			this.id,
			this.partNo,
			this.source,
			this.carFamilyCode,
			this.lotNo,
			this.inStockByLot,
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
        this.id = 0,
		this.partNo = '',
		this.source = '',
		this.carFamilyCode = '',
		this.lotNo = '',
		this.inStockByLot = '',
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
        return this._serviceStock.getAll(
 			this.periodId,
			this.partNo,
			this.source,
			this.carFamilyCode,
			this.lotNo,
			this.inStockByLot,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvStockDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvStockDto();
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
        this._serviceStock.getStockToExcel(
            this.periodId,
			this.partNo,
			this.source,
			this.carFamilyCode,
			this.lotNo,
			this.inStockByLot,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }

    reportRequestDetails(){
        window.open("/app/main/cmm/reportrequest")
      }
}
