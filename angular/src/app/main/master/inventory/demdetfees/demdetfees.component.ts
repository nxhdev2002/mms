import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvDemDetFeesDto, MstInvDemDetFeesServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditDemDetFeesModalComponent } from './create-or-edit-demdetfees-modal.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';

@Component({
    templateUrl: './demdetfees.component.html',
})
export class DemDetFeesComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalDemDetFees', { static: true }) createOrEditModalDemDetFees:| CreateOrEditDemDetFeesModalComponent| undefined;
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
    selectedRow: MstInvDemDetFeesDto = new MstInvDemDetFeesDto();
    saveSelectedRow: MstInvDemDetFeesDto = new MstInvDemDetFeesDto();
    datas: MstInvDemDetFeesDto = new MstInvDemDetFeesDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    sourceList = [];
    carrierList = [];
    minwidth0 = "minwidth0";
    source;
    carrier;
    contType;
    noOfDayOVF;
    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
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
        private _service: MstInvDemDetFeesServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm:DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Source'),headerTooltip: this.l('Source'),field: 'source',flex: 1},
			{headerName: this.l('Carrier'),headerTooltip: this.l('Carrier'),field: 'carrier',flex: 1},
            {headerName: this.l('ContType'),headerTooltip: this.l('ContType'),field: 'contType',type: 'rightAligned',flex: 1},
            // {headerName: this.l('No of Day OVF'),headerTooltip: this.l('No of Day OVF'),type: 'rightAligned',flex: 1,valueGetter:(params) =>(params.data?.isMax === 'Y'?'>=':'') + params.data?.noOfDayOVF },
            {headerName: this.l('MinDay'),headerTooltip: this.l('MinDay'),field: 'minDay',type: 'rightAligned',flex: 1,},
            {headerName: this.l('MaxDay'),headerTooltip: this.l('MaxDay'),field: 'maxDay',type: 'rightAligned',flex: 1,},

            {headerName: this.l('DEM fee'),headerTooltip: this.l('DEM fee'),field: 'demFee',type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.demFee, 0),flex: 1},
            {headerName: this.l('DET fee'),headerTooltip: this.l('DET fee'),field: 'detFee',type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.detFee, 0),flex: 1},
            {headerName: this.l('DemAndDetFee'),headerTooltip: this.l('DemAndDetFee'),field: 'demAndDetFee',type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.demAndDetFee, 0),flex: 1},
            {headerName: this.l('EffectiveDateFrom'),headerTooltip: this.l('EffectiveDateFrom'),field: 'effectiveDateFrom',type: 'rightAligned',valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateFrom, 'dd/MM/yyyy'),flex: 1},
            {headerName: this.l('EffectiveDateTo'),headerTooltip: this.l('EffectiveDateTo'),field: 'effectiveDateTo',type: 'rightAligned',valueGetter: (params) => this.pipe.transform(params.data?.effectiveDateTo, 'dd/MM/yyyy'),flex: 1},
            {
                headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            }
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._service.getListSource().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.sourceList.push({ value: e.source, label: e.source });
                })
            }
        })
        this._service.getListCarrier().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.carrierList.push({ value: e.carrier, label: e.carrier });
                })
            }
        })
    }

    searchDatas(): void {
        this._service.getAll(
			this.source,
			this.carrier,

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
        this.source = '',
		this.carrier = '',

        this.searchDatas();
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){

        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSize();
        },1000)
    }
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
			this.source,
			this.carrier,

			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }
    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvDemDetFeesDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvDemDetFeesDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView;
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
                this.resetGridView;
                this.isLoading = false;
            });
    }

    deleteRow(system: MstInvDemDetFeesDto): void {
        this.message.confirm(this.l('Are You Sure To Delete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

           this._service.getMstInvDemDetFeesToExcel(
			this.source,
			this.carrier,

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
