import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvOutLineOffDto, InvOutLineOffServiceProxy, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { RequestModalComponent } from './request-modal.component';

@Component({
    templateUrl: './invoutlineoff.component.html',
    styleUrls: ['./invoutlineoff.component.less'],
})
export class InvOutLineOffComponent extends AppComponentBase implements OnInit {

    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('reportRequest3Modal', { static: true }) reportRequest3Modal: | RequestModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvOutLineOffDto = new InvOutLineOffDto();
    saveSelectedRow: InvOutLineOffDto = new InvOutLineOffDto();
    datas: InvOutLineOffDto = new InvOutLineOffDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    periodId
    partNo: string = '';
    carFamilyCode: string = '';
    usageQty
    sumCif
    sumTax
    sumInland
    amount
    sumCifVn
    sumTaxVn
    sumInlandVn
    amountVn
    customsDeclareNo
    declareDate: any;
    dcType: string = '';
    inStockByLot: string = '';
    id: number = 0;

    periodColumdef: CustomColDef[] = [];
    domainColumdef: CustomColDef[] = [];
    cbbPeriod: any[] = [];

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
        private _service: InvOutLineOffServiceProxy,
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
                   cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                   cellClass: ['text-center'],
                    width: 50,
            },
            {
                headerName: this.l('Period Id'),
                 headerTooltip: this.l('Period Id'),
                  field: 'periodId',
                   width: 80
             },
            {
                headerName: this.l('Part No'),
                 headerTooltip: this.l('Part No'),
                 field: 'partNo',
                  width: 80
            },
            {
                headerName: this.l('Car Family Code'),
                headerTooltip: this.l('Car Family Code'),
                 field: 'carFamilyCode',
                  width: 120
            },
            {
                headerName: this.l('Usage Qty'),
                headerTooltip: this.l('Usage Qty'),
                field: 'usageQty',
                width: 90
            },
            {
                headerName: this.l('Sum Cif'),
                headerTooltip: this.l('Sum Cif'),
                field: 'sumCif',
                width: 80
            },
            {
                headerName: this.l('Sum Tax'),
                headerTooltip: this.l('Sum Tax'),
                field: 'sumTax',
                width: 80
            },
            {
                headerName: this.l('Sum Inland'),
                headerTooltip: this.l('Sum Inland'),
                field: 'sumInland',
                width: 90
            },
            {
                headerName: this.l('Amount'),
                headerTooltip: this.l('Amount'),
                field: 'amount',
                width: 80
            },
            {
                headerName: this.l('Sum Cif Vn'),
                headerTooltip: this.l('Sum Cif Vn'),
                field: 'sumCifVn',
                width: 90
            },
            {
                headerName: this.l('Sum Tax Vn'),
                headerTooltip: this.l('Sum Tax Vn'),
                field: 'sumTaxVn',
                width: 100
            },
            {
                headerName: this.l('Sum Inland Vn'),
                headerTooltip: this.l('Sum Inland Vn'),
                field: 'sumInlandVn',
                width: 110
             },
            {
                headerName: this.l('Amount Vn'),
                headerTooltip: this.l('Amount Vn'),
                field: 'amountVn',
                width: 90
            },
            {
                headerName: this.l('Customs Declare No'),
                headerTooltip: this.l('Customs Declare No'),
                field: 'customsDeclareNo',
                width: 140
            },
            {
                headerName: this.l('Declare Date'),
                headerTooltip: this.l('Declare Date'),
                field: 'declareDate', valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                width: 100
            },
            {
                headerName: this.l('Dc Type'),
                headerTooltip: this.l('Dc Type'),
                field: 'dcType',
                width:100
            },
            {
                headerName: this.l('In Stock By Lot'),
                headerTooltip: this.l('In Stock By Lot'),
                field: 'inStockByLot',
                width:110
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceStock.getIdInvPeriod()
        .subscribe((result) => {
            this.cbbPeriod = result.items;


        });
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.id,
            this.partNo,
            this.carFamilyCode,
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
            this.carFamilyCode = '',
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
        return this._service.getAll(
            this.id,
            this.partNo,
            this.carFamilyCode,
            this.inStockByLot,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvOutLineOffDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvOutLineOffDto();
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
        this._service.getInvOutLineOffToExcel(
            this.id,
            this.partNo,
            this.carFamilyCode,
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
