import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { InvDrmStockRundownDto, InvDrmStockRundownServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './drmstockrundown.component.html'
})
export class DrmStockRundownComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvDrmStockRundownDto = new InvDrmStockRundownDto();
    saveSelectedRow: InvDrmStockRundownDto = new InvDrmStockRundownDto();
    datas: InvDrmStockRundownDto = new InvDrmStockRundownDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: InvDrmStockRundownDto[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    myDate = new Date();
    materialCode : string = '' ;
    materialSpec : string = '' ;
    A1 : string = '';
    A2 : string = '';
    A3 : string = '';
    A4 : string = '';
    A5 : string = '';
    A6 : string = '';
    A7 : string = '';
    A8 : string = '';
    A9 : string = '';
    A10 : string = '';
    A11 : string = '';
    A12 : string = '';
    A13 : string = '';
    A14 : string = '';
    A15 : string = '';
    A16 : string = '';
    A17 : string = '';
    A18 : string = '';
    A19 : string = '';
    A20 : string = '';
    A21 : string = '';
    A22 : string = '';
    A23 : string = '';
    A24 : string = '';
    A25 : string = '';
    A26 : string = '';
    A27 : string = '';
    A28 : string = '';
    A29 : string = '';
    A30 : string = '';
    A31 : string = '';
    A32 : string = '';
    A33 : string = '';
    A34 : string = '';
    A35 : string = '';
    A36 : string = '';
    A37 : string = '';
    A38 : string = '';
    A39 : string = '';
    A40 : string = '';
    A41 : string = '';
    A42 : string = '';
    A43 : string = '';
    A44 : string = '';
    A45 : string = '';
    A46 : string = '';
    A47 : string = '';
    A48 : string = '';
    A49 : string = '';
    A50 : string = '';
    A51 : string = '';
    A52 : string = '';
    A53 : string = '';
    A54 : string = '';
    A55 : string = '';
    A56 : string = '';
    A57 : string = '';
    A58 : string = '';
    A59 : string = '';
    A60 : string = '';
    A61 : string = '';



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
        private _service: InvDrmStockRundownServiceProxy,
        private gridTableService: GridTableService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,pinned: 'left'},
            {headerName:  this.l('Material Code'),headerTooltip: this.l('Material Code'),field: 'materialCode',width:100,pinned: 'left'},
			{headerName: this.l('Material Spec'),headerTooltip: this.l('Material Spec'),field: 'materialSpec',width:250,pinned: 'left'},
			//{headerName: this.l('Drm Material Id'),headerTooltip: this.l('Drm Material Id'),field: 'drmMaterialId',flex: 1},
			{headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo'},
			{headerName: this.l('Part Name'),headerTooltip: this.l('Part Name'),field: 'partName'},
			//{headerName: this.l('Part Id'),headerTooltip: this.l('Part Id'),field: 'partId',flex: 1},
			//{headerName: this.l('Material Id'),headerTooltip: this.l('Material Id'),field: 'materialId',flex: 1},
			//{headerName: this.l('Qty'),headerTooltip: this.l('Qty'),field: 'qty',flex: 1},
			//{headerName: this.l('Working Date'),headerTooltip: this.l('Working Date'),field: 'workingDate',flex: 1},
            { headerValueGetter: (params) => this.A1, field: 'a1', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A2, field: 'a2', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A3, field: 'a3', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A4, field: 'a4', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A5, field: 'a5', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A6, field: 'a6', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A7, field: 'a7', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A8, field: 'a8', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A9, field: 'a9', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A10, field: 'a10', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A11, field: 'a11', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A12, field: 'a12', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A13, field: 'a13', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A14, field: 'a14', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A15, field: 'a15', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A16, field: 'a16', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A17, field: 'a17', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A18, field: 'a18', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A19, field: 'a19', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A20, field: 'a20', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A21, field: 'a21', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A22, field: 'a22', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A23, field: 'a23', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A24, field: 'a24', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A25, field: 'a25', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A26, field: 'a26', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A27, field: 'a27', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A28, field: 'a28', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A29, field: 'a29', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A30, field: 'a30', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A31, field: 'a31', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A32, field: 'a32', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A33, field: 'a33', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A34, field: 'a34', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A35, field: 'a35', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A36, field: 'a36', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A37, field: 'a37', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A38, field: 'a38', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A39, field: 'a39', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A40, field: 'a40', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A41, field: 'a41', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A42, field: 'a42', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A43, field: 'a43', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A44, field: 'a44', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A45, field: 'a45', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A46, field: 'a46', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A47, field: 'a47', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A48, field: 'a48', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A49, field: 'a49', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A50, field: 'a50', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A51, field: 'a51', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A52, field: 'a52', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A53, field: 'a53', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A54, field: 'a54', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A55, field: 'a55', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A56, field: 'a56', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A57, field: 'a57', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A58, field: 'a58', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A59, field: 'a59', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A60, field: 'a60', type: 'rightAligned' },
            { headerValueGetter: (params) => this.A61, field: 'a61', type: 'rightAligned' },


        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.A1 = this.pipe.transform(this.myDate, 'dd/MM/yyyy');
        this.A2 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1)), 'dd/MM/yyyy');
        this.A3 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A4 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A5 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A6 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A7 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A8 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A9 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A10 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A11 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A12 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A13 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A14 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A15 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A16 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A17 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A18 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A19 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A20 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A21 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A22 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A23 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A24 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A25 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A26 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A27 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A28 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A29 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A30 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A31 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A32 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A33 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A34 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A35 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A36 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A37 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A38 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A39 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A40 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A41 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A42 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A43 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A44 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A45 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A46 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A47 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A48 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A49 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A50 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A51 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A52 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A53 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A54 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A55 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A56 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A57 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A58 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A59 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A60 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');
        this.A61 = this.pipe.transform((this.myDate.setDate(this.myDate.getDate() + 1 )), 'dd/MM/yyyy');


    }


    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if ( column.getId().toString() != "stt"
                && column.getId().toString() != "Material Code"
                && column.getId().toString() != "Material Spec") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        },200);
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.materialCode,
            this.materialSpec,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            console.log(this.rowData);

            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView()
        });
    }

    clearTextSearch() {
        this.materialCode = '',
        this.materialSpec = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
 			this.materialCode,
             this.materialSpec,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmStockRundownDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmStockRundownDto();
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
                this.resetGridView()
            });
    }
    calculatorRundown(){
        this._service.calculatorRundown()
        .subscribe((result) => {

        });
    }

}
