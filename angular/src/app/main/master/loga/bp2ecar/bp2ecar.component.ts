import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaBp2EcarDto, MstLgaBp2EcarServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBp2EcarModalComponent } from './create-or-edit-bp2ecar-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './bp2ecar.component.html',
    styleUrls: ['./bp2ecar.component.less'],
})
export class Bp2EcarComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBp2Ecar', { static: true }) createOrEditModalBp2Ecar: | CreateOrEditBp2EcarModalComponent | undefined;
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

    selectedRow: MstLgaBp2EcarDto = new MstLgaBp2EcarDto();
    saveSelectedRow: MstLgaBp2EcarDto = new MstLgaBp2EcarDto();
    datas: MstLgaBp2EcarDto = new MstLgaBp2EcarDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    code: string = '';
    ecarName: string = '';
    prodLine: string = '';
    ecarType: string = '';
    sorting: number = 0;
    isActive: string = '';

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
        private _service: MstLgaBp2EcarServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Code'),
                headerTooltip: this.l('Code'),
                field: 'code',
                flex: 1
            },
            {
                headerName: this.l('Ecar Name'),
                headerTooltip: this.l('Ecar Name'),
                field: 'ecarName',
                flex: 1
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1
            },
            {
                headerName: this.l('Ecar Type'),
                headerTooltip: this.l('Ecar Type'),
                field: 'ecarType',
                flex: 1
            },
            {
                headerName: this.l('Sorting'),
                headerTooltip: this.l('Sorting'),
                field: 'sorting',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            }
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
            this.code,
            this.ecarName,
            this.prodLine,
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
        this.code = '',
            this.ecarName = '',
            this.prodLine = '',
            this.ecarType = '',
            this.sorting = 0,
            this.isActive = '',
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
            this.code,
            this.ecarName,
            this.prodLine,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgaBp2EcarDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgaBp2EcarDto();
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

    deleteRow(system: MstLgaBp2EcarDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getBp2EcarToExcel(
            this.code,
            this.ecarName,
            this.prodLine,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }
 

}