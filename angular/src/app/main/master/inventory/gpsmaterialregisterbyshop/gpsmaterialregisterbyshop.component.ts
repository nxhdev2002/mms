import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstGpsMaterialRegisterByShopDto, MstGpsMaterialRegisterByShopServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditGpsMaterialRegisterByShopModalComponent } from './create-or-edit-gpsmaterialregisterbyshop-modal.component';
import { ImportGpsMaterialRegisterByShopComponent } from './import-gpsmaterialregisterbyshop.component';

@Component({
    templateUrl: './gpsmaterialregisterbyshop.component.html',
    styleUrls: ['./gpsmaterialregisterbyshop.component.less']
})
export class GpsMaterialRegisterByShopComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditGpsMaterialRegisterByShop', { static: true }) createOrEditGpsMaterialRegisterByShop: | CreateOrEditGpsMaterialRegisterByShopModalComponent | undefined;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportGpsMaterialRegisterByShopComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstGpsMaterialRegisterByShopDto = new MstGpsMaterialRegisterByShopDto();
    saveSelectedRow: MstGpsMaterialRegisterByShopDto = new MstGpsMaterialRegisterByShopDto();
    datas: MstGpsMaterialRegisterByShopDto = new MstGpsMaterialRegisterByShopDto();
    pending: string = '';
    disable: boolean = false;
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo
    uom
    category = ''
    expenseAccount
    shop
    costCenter
    categoryList = [
        {value: '' , label: "ALL"},
        {value: 'Direct Materials' , label: "Direct Materials"},
        {value: 'Indirect Materials' , label: "Indirect Materials"},
    ];

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
        private _service1: MstGpsMaterialRegisterByShopServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description', width: 500},
            { headerName: this.l('Uom'), headerTooltip: this.l('Uom'), field: 'uom', flex: 1 },
            { headerName: this.l('Part Type'), headerTooltip: this.l('Part Type'), field: 'category', flex: 1 },
            { headerName: this.l('Expense Account'), headerTooltip: this.l('ExpenseAccount'), field: 'expenseAccount', flex: 1 },
            { headerName: this.l('Shop'), headerTooltip: this.l('Shop'), field: 'shop', flex: 1 },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('CostCenter'), field: 'costCenter', flex: 1 },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

        // this.getCategoryCbx();
    }

    // getCategoryCbx() {
    //     this.categoryList = []
    //     this.categoryList.push({value: '', label: 'ALL'})
    //     this._service1.getCbxCategory().subscribe((result) => {
    //         result.forEach(e => this.categoryList.push({ value: e.name, label: e.name }));
    //     });
    // }


    searchDatas(): void {
        this.paginationParams.pageNum = 1;
        this.paginationParams.skipCount = 0;

        this._service1.getAll(
            this.partNo,
            this.uom,
            this.category,
            this.expenseAccount,
            this.shop,
            this.costCenter,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView()
            });
    }

    clearTextSearch() {
        this.partNo = ''
        this.uom = ''
        this.expenseAccount = ''
        this.shop = ''
        this.costCenter = ''
        this.category = ''
        // this.getCategoryCbx()
        this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service1.getAll(
            this.partNo,
            this.uom,
            this.category,
            this.expenseAccount,
            this.shop,
            this.costCenter,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        // setTimeout(() => {
        //     this.dataParams.columnApi!.sizeColumnsToFit({
        //         suppressColumnVirtualisation: true,
        //     });
        //     this.autoSize();
        // }, 1000)
    }


    onChangeRowSelection(params: { api: { getSelectedRows: () => MstGpsMaterialRegisterByShopDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstGpsMaterialRegisterByShopDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {

        this.isLoading = true;
        this.paginationParams = paginationParams;

        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
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


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

       this.fn.exportLoading(e, true);


        this._service1.getMaterialRegisterByShopToExcel(
            this.partNo,
            this.uom,
            this.category,
            this.expenseAccount,
            this.shop,
            this.costCenter,
        )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    deleteRow(system: MstGpsMaterialRegisterByShopDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service1.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

}
