import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvDrmLocalPlanDto, InvDrmLocalPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    templateUrl: './drmlocalplan.component.html',
})
export class DrmLocalPlanComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvDrmLocalPlanDto = new InvDrmLocalPlanDto();
    saveSelectedRow: InvDrmLocalPlanDto = new InvDrmLocalPlanDto();
    datas: InvDrmLocalPlanDto = new InvDrmLocalPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    supplierNo : string = '' ;
	deliveryDate
	shipmentNo : string = '' ;
	cfc : string = '' ;
	partCode : string = '' ;
	materialCode : string = '' ;
	materialSpec : string = '' ;
	qty
	deliveryMonth
	delayDelivery
	remark : string = '' ;

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
        private _service: InvDrmLocalPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo',flex: 1},
			{headerName: this.l('Delivery Date'),headerTooltip: this.l('Delivery Date'),field: 'deliveryDate',flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.deliveryDate, 'dd/MM/yyyy')},
			{headerName: this.l('Shipment No'),headerTooltip: this.l('Shipment No'),field: 'shipmentNo',flex: 1},
			{headerName: this.l('Cfc'),headerTooltip: this.l('Cfc'),field: 'cfc',flex: 1},
			{headerName: this.l('Part Code'),headerTooltip: this.l('Part Code'),field: 'partCode',flex: 1},
			{headerName: this.l('Material Code'),headerTooltip: this.l('Material Code'),field: 'materialCode',flex: 1},
			{headerName: this.l('Material Spec'),headerTooltip: this.l('Material Spec'),field: 'materialSpec',flex: 1},
			{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1,type: 'rightAligned'},
			{headerName: this.l('Delivery Month'),headerTooltip: this.l('Delivery Month'),field: 'deliveryMonth',flex: 1},
			{headerName: this.l('Delay Delivery'),headerTooltip: this.l('Delay Delivery'),field: 'delayDelivery',flex: 1},
			{headerName: this.l('Remark'),headerTooltip: this.l('Remark'),field: 'remark',flex: 1},
        ];
		this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service.getAll(
			this.supplierNo,
			this.shipmentNo,
			this.cfc,
			this.partCode,
			this.materialCode,
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
        this.supplierNo = '',
		this.shipmentNo = '',
		this.cfc = '',
		this.partCode = '',
		this.materialCode = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
			this.supplierNo,
			this.shipmentNo,
			this.cfc,
			this.partCode,
			this.materialCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmLocalPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmLocalPlanDto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvDrmLocalPlanExportToFile(
            this.supplierNo,
			this.shipmentNo,
			this.cfc,
			this.partCode,
			this.materialCode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

}
