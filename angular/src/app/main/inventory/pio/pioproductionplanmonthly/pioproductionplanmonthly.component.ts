import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdProductionPlanMonthlyServiceProxy, InvPioProductionPlanMonthlyDto, InvPioProductionPlanMonthlyServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ImportPioProductionPlanMonthlyComponent } from './import-pioproductionplanmonthly.component';

@Component({
    templateUrl: './pioproductionplanmonthly.component.html',
})
export class PioProductionPlanMonthlyComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportPioProductionPlanMonthlyComponent| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvPioProductionPlanMonthlyDto = new InvPioProductionPlanMonthlyDto();
    saveSelectedRow: InvPioProductionPlanMonthlyDto = new InvPioProductionPlanMonthlyDto();
    datas: InvPioProductionPlanMonthlyDto = new InvPioProductionPlanMonthlyDto();
    isLoading: boolean = false;
    disable: boolean = false;
    pending: string = '';
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    cfc : string = '' ;
	grade : string = '' ;
    prodMonth:any ;

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
        private _service: InvPioProductionPlanMonthlyServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Cfc'),headerTooltip: this.l('Cfc'),field: 'cfc',flex: 1},
			{headerName: this.l('Grade'),headerTooltip: this.l('Grade'),field: 'grade',flex: 1},
			{headerName: this.l('Prod Month'),headerTooltip: this.l('Production Month'),field: 'productionMonth',flex: 1, valueGetter: (params) => this.pipe.transform(params.data?.productionMonth, 'MM/yyyy')},
			{headerName: this.l('N - 3'),headerTooltip: this.l('N - 3'),field: 'n_3',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n_3)},
			{headerName: this.l('N - 2'),headerTooltip: this.l('N - 2'),field: 'n_2',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n_2)},
			{headerName: this.l('N - 1'),headerTooltip: this.l('N - 1'),field: 'n_1',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n_1)},
			{headerName: this.l('N'),headerTooltip: this.l('N'),field: 'n',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n)},
			{headerName: this.l('N + 1'),headerTooltip: this.l('N1'),field: 'n1',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n1)},
			{headerName: this.l('N + 2'),headerTooltip: this.l('N2'),field: 'n2',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n2)},
			{headerName: this.l('N + 3'),headerTooltip: this.l('N3'),field: 'n3',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n3)},
			{headerName: this.l('N + 4'),headerTooltip: this.l('N4'),field: 'n4',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n4)},
			{headerName: this.l('N + 5'),headerTooltip: this.l('N5'),field: 'n5',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n5)},
			{headerName: this.l('N + 6'),headerTooltip: this.l('N6'),field: 'n6',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n6)},
			{headerName: this.l('N + 7'),headerTooltip: this.l('N7'),field: 'n7',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n7)},
			{headerName: this.l('N + 8'),headerTooltip: this.l('N8'),field: 'n8',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n8)},
			{headerName: this.l('N + 9'),headerTooltip: this.l('N9'),field: 'n9',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n9)},
			{headerName: this.l('N + 10'),headerTooltip: this.l('N10'),field: 'n10',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n10)},
			{headerName: this.l('N + 11'),headerTooltip: this.l('N11'),field: 'n11',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n11)},
			{headerName: this.l('N + 12'),headerTooltip: this.l('N12'),field: 'n12',flex: 1,type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.n12)}
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
        allColumnIds.push(column.getId());
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.autoSize();
        }, 500)
    }

    searchDatas(): void {
        this._service.getAll(
            this.cfc,
            this.grade,
            this._dateTimeService.convertToDatetime(this.prodMonth),

			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => {
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            var rows = this.createRow(1);
            this.dataParams!.api.setPinnedBottomRowData(rows);
        }))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
           this.resetGridView();
        });
    }

    clearTextSearch() {
        this.cfc = '',
		this.grade = '',
        this.prodMonth = '',
        this.searchDatas();
    }

     createRow(count: number): any[] {
        let result: any[] = [];

        if (this.rowData.length > 0) {
            console.log(this.rowData[0]);

            result.push({
                cfc: 'Grand Total',
                n_3: this.rowData[0].total_N_3,
                n_2: this.rowData[0].total_N_2,
                n_1: this.rowData[0].total_N_1,
                n: this.rowData[0].total_N,
                n1: this.rowData[0].total_N1,
                n2: this.rowData[0].total_N2,
                n3: this.rowData[0].total_N3,
                n4: this.rowData[0].total_N4,
                n5: this.rowData[0].total_N5,
                n6: this.rowData[0].total_N6,
                n7: this.rowData[0].total_N7,
                n8: this.rowData[0].total_N8,
                n9: this.rowData[0].total_N9,
                n10: this.rowData[0].total_N10,
                n11: this.rowData[0].total_N11,
                n12: this.rowData[0].total_N12

            });
        }
        else {
            result.push({
                cfc: 'Grand Total',
                n_3: 0,
                n_2: 0,
                n_1: 0,
                n: 0,
                n1: 0,
                n2: 0,
                n3: 0,
                n4: 0,
                n5: 0,
                n6: 0,
                n7: 0,
                n8: 0,
                n9: 0,
                n10: 0,
                n11: 0,
                n12: 0
            });
        }
        return result;
    }


    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.cfc,
            this.grade,
            this._dateTimeService.convertToDatetime(this.prodMonth),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvPioProductionPlanMonthlyDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvPioProductionPlanMonthlyDto();
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
          //  this.resetGridView();
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
        .pipe(finalize(() => {
            var rows = this.createRow(1);
            this.dataParams!.api.setPinnedBottomRowData(rows);
            this.gridTableService.selectFirstRow(this.dataParams!.api);
        }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
                this.isLoading = false;
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvPioProdPlanMonthlyToExcel(
            this.cfc,
            this.grade,
            this._dateTimeService.convertToDatetime(this.prodMonth),
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
