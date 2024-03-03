import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvImportByContDto, InvImportByContServiceProxy, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { RequestModalComponent } from './request-modal.component';


@Component({
    templateUrl: './importbycont.component.html',
    styleUrls: ['./importbycont.component.less'],
})
export class ImportByContComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('reportRequest5Modal', { static: true }) reportRequest5Modal: | RequestModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    periodColumdef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvImportByContDto = new InvImportByContDto();
    saveSelectedRow: InvImportByContDto = new InvImportByContDto();
    datas: InvImportByContDto = new InvImportByContDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    periodId
    containerNo: string = '';
    invoiceNo
    caseNo: string = '';
    lotNo: string = '';
    partNo: string = '';
    dateIn: any;
    invoiceDate: any;
    receiveDate: any;
    contSize: number = 0;
    eta: any;
    supplierNo: string = '';
    cbbPeriod: any[] = [];
    description: string = '';
    id: number = 0;

      //
      TotalFob: any;
      TotalCif: any;
      TotalImportTax: any;
      TotalInlandCharge: any;
      TotalAmount: any;

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
        private _serviceinvimport: InvStockServiceProxy,
        private _service12: InvImportByContServiceProxy,
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
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Period Id'),
                headerTooltip: this.l('Period Id'),
                field: 'periodId',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                width:100
            },
            {
                headerName: this.l('Invoice No'),
                headerTooltip: this.l('Invoice No'),
                field: 'invoiceNo',
                width:120
            },
            {
                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
                width:90
            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width:90
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                width:100
            },
            {
                headerName: this.l('Date In'),
                headerTooltip: this.l('Date In'),
                field: 'dateIn', valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
                width:90
            },
            {
                headerName: this.l('Fob'),
                headerTooltip: this.l('Fob'),
                field: 'fob',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cif'),
                headerTooltip: this.l('Cif'),
                field: 'cif',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Import Tax'),
                headerTooltip: this.l('Import Tax'),
                field: 'importTax',
                width:100,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Inland Charge'),
                headerTooltip: this.l('Inland Charge'),
                field: 'inlandCharge',
                width:110,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Amount'),
                headerTooltip: this.l('Amount'),
                field: 'amount',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Qty'),
                headerTooltip: this.l('Qty'),
                field: 'qty',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Price'),
                headerTooltip: this.l('Price'),
                field: 'price',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Fob Vn'),
                headerTooltip: this.l('Fob Vn'),
                field: 'fobVn',

                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Cif Vn'),
                headerTooltip: this.l('Cif Vn'),
                field: 'cifVn',

                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Import Tax Vn'),
                headerTooltip: this.l('Import Tax Vn'),
                field: 'importTaxVn',

                width:120,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Inland Charge Vn'),
                headerTooltip: this.l('Inland Charge Vn'),
                field: 'inlandChargeVn',
                width:150,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Amount Vn'),
                headerTooltip: this.l('Amount Vn'),
                field: 'amountVn',
                width:150,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Price Vn'),
                headerTooltip: this.l('Price Vn'),
                field: 'priceVn',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Invoice Date'),
                headerTooltip: this.l('Invoice Date'),
                field: 'invoiceDate', valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),
                width:100
            },
            {
                headerName: this.l('Receive Date'),
                headerTooltip: this.l('Receive Date'),
                field: 'receiveDate', valueGetter: (params) => this.pipe.transform(params.data?.receiveDate, 'dd/MM/yyyy'),
                width:100
            },
            {
                headerName: this.l('Cont Size'),
                headerTooltip: this.l('Cont Size'),
                field: 'contSize',
                width:90,
                type: 'rightAligned',
            },
            {
                headerName: this.l('Eta'),
                headerTooltip: this.l('Eta'),
                field: 'eta', valueGetter: (params) => this.pipe.transform(params.data?.eta, 'dd/MM/yyyy'),
                width:90
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width:90
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceinvimport.getIdInvPeriod()
        .subscribe((result) => {
            this.cbbPeriod = result.items;
         });
    }

    searchDatas(): void {

        this._service12.getAll(
            this.id,
            this.containerNo,
            this.caseNo,
            this.lotNo,
            this.partNo,
            this.supplierNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.TotalFob = this.rowData[0].totalFob;
                this.TotalCif = this.rowData[0].totalCif;
                this.TotalImportTax = this.rowData[0].totalImportTax;
                this.TotalInlandCharge = this.rowData[0].totalInlandCharge;
                this.TotalAmount = this.rowData[0].totalAmount;
            });
    }

    clearTextSearch() {
            this.id = 0,
            this.containerNo = '',
            this.caseNo = '',
            this.lotNo = '',
            this.partNo = '',
            this.supplierNo = '',
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
        return this._service12.getAll(
            this.id,
            this.containerNo,
            this.caseNo,
            this.lotNo,
            this.partNo,
            this.supplierNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvImportByContDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvImportByContDto();
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
            if(result.totalCount > 0){
                this.TotalFob = this.rowData[0].totalFob;
                this.TotalCif = this.rowData[0].totalCif;
                this.TotalImportTax = this.rowData[0].totalImportTax;
                this.TotalInlandCharge = this.rowData[0].totalInlandCharge;
                this.TotalAmount = this.rowData[0].totalAmount;
            }
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
                this.TotalFob = this.rowData[0].totalFob;
                this.TotalCif = this.rowData[0].totalCif;
                this.TotalImportTax = this.rowData[0].totalImportTax;
                this.TotalInlandCharge = this.rowData[0].totalInlandCharge;
                this.TotalAmount = this.rowData[0].totalAmount;
            });
    }

    exportToExcel(): void {
        this.isLoading = true;
        this._service12.getImportByContToExcel(
            this.id,
            this.containerNo,
            this.caseNo,
            this.lotNo,
            this.partNo,
            this.supplierNo
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
