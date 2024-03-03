import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdPhysicalConfirmLotDto, InvCkdPhysicalConfirmLotServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportPhysicalConfirmLotComponent } from './import-physical-confirm-lot.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';


@Component({
    templateUrl: './physicalconfirmlot.component.html',
})
export class PhysicalConfirmLotComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportPhysicalConfirmLotComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPhysicalConfirmLotDto = new InvCkdPhysicalConfirmLotDto();
    saveSelectedRow: InvCkdPhysicalConfirmLotDto = new InvCkdPhysicalConfirmLotDto();
    datas: InvCkdPhysicalConfirmLotDto = new InvCkdPhysicalConfirmLotDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    	modelCode : string = '' ;
	prodLine : string = '' ;
	grade : string = '' ;
	lotNo
	noInLot
	processDate

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
        private _service: InvCkdPhysicalConfirmLotServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _formStoringService: FormStoringService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 60,},
            {headerName: this.l('Model Code'),headerTooltip: this.l('Model Code'),field:  'modelCode',flex: 1},
            {headerName: this.l('Prod Line'),headerTooltip: this.l('Prod Line'),field: 'prodLine',flex: 1},
			{headerName: this.l('Grade'),headerTooltip: this.l('Grade'),field: 'grade',flex: 1},
			{headerName: this.l('Start Lot'),headerTooltip: this.l('Start Lot'),field: 'startLot',flex: 1, type: 'rightAligned'},
			{headerName: this.l('Start Run'),headerTooltip: this.l('Start Run'),field: 'startRun',flex: 1, type: 'rightAligned'},
			{headerName: this.l('Start Process Date'),headerTooltip: this.l('Start Process Date'),field: 'startProcessDate',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.startProcessDate, 'dd/MM/yyyy')},
            {headerName: this.l('End Lot'),headerTooltip: this.l('End Lot'),field: 'endLot',flex: 1, type: 'rightAligned'},
			{headerName: this.l('End Run'),headerTooltip: this.l('End Run'),field: 'endRun',flex: 1, type: 'rightAligned'},
			{headerName: this.l('End Process Date'),headerTooltip: this.l('End Process Date'),field: 'endProcessDate',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.endProcessDate, 'dd/MM/yyyy')},
            {headerName: this.l('PeriodId'),headerTooltip: this.l('PeriodId'),field: 'periodId',flex: 1, type: 'rightAligned'},
        ];
		this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.modelCode,
			this.prodLine,
			this.grade,
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
        this.modelCode = '',
		this.prodLine = '',
		this.grade = '',
		this.lotNo = '',
		this.noInLot = '',
		this.processDate = '',
        this.searchDatas();
    }
    
    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
 			this.modelCode,
			this.prodLine,
			this.grade,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPhysicalConfirmLotDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPhysicalConfirmLotDto();
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
