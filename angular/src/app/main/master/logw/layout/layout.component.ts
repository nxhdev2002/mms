import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwLayoutDto, MstLgwLayoutServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';

import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.less'],
})
export class LayoutComponent extends AppComponentBase implements OnInit {

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

    selectedRow: MstLgwLayoutDto = new MstLgwLayoutDto();
    saveSelectedRow: MstLgwLayoutDto = new MstLgwLayoutDto();
    datas: MstLgwLayoutDto = new MstLgwLayoutDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    zoneCd : string = '' ;
	areaCd : string = '' ;
	rowId : number = 0 ;
	columnId : number = 0 ;
	rowName : string = '' ;
columnName : string = '' ;
	locationCd : string = '' ;
	locationName : string = '' ;
	locationTitle : string = '' ;
	isDisabled : string = '' ;
isActive : string = '' ;

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
        private _service: MstLgwLayoutServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Zone Cd'),
                headerTooltip: this.l('Zone Cd'),
                field: 'zoneCd',
                flex: 1
            },
			{
                headerName: this.l('Area Cd'),
                headerTooltip: this.l('Area Cd'),
                field: 'areaCd',
                flex: 1
            },
			{
                headerName: this.l('Row Id'),
                headerTooltip: this.l('Row Id'),
                field: 'rowId',
                cellClass: ['text-right'],
                flex: 1
            },
			{
                headerName: this.l('Column Id'),
                headerTooltip: this.l('Column Id'),
                field: 'columnId',
                cellClass: ['text-right'],
                flex: 1
            },
			{
                headerName: this.l('Row Name'),
                headerTooltip: this.l('Row Name'),
                field: 'rowName',
                cellClass: ['text-right'],
                flex: 1
            },
			{
                headerName: this.l('Column Name'),
                headerTooltip: this.l('Column Name'),
                field: 'columnName',
                flex: 1
            },
			{
                headerName: this.l('Location Cd'),
                headerTooltip: this.l('Location Cd'),
                field: 'locationCd',
                flex: 1
            },
			{
                headerName: this.l('Location Name'),
                headerTooltip: this.l('Location Name'),
                field: 'locationName',
                flex: 1
            },
			{
                headerName: this.l('Location Title'),
                headerTooltip: this.l('Location Title'),
                field: 'locationTitle',
                flex: 1
            },
            {
                headerName: this.l('Is Disabled'),
                headerTooltip: this.l('Is Disabled'),
                field: 'isDisabled',
                flex: 1,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isDisabled == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isDisabled == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isDisabled == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isDisabled == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            },
			{
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                flex: 1,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
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
			this.zoneCd,
			this.areaCd,
			this.rowId,
			this.columnId,
			this.rowName,
			this.columnName,
			this.locationCd,
			this.locationName,
			this.locationTitle,
			this.isDisabled,
			this.isActive,
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
        this.zoneCd = '',
		this.areaCd = '',
		this.rowId = 0,
		this.columnId = 0,
		this.rowName = '',
        this.columnName = '',
		this.locationCd = '',
		this.locationName = '',
		this.locationTitle = '',
		this.isDisabled = '',
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
 			this.zoneCd,
			this.areaCd,
			this.rowId,
			this.columnId,
			this.rowName,
			this.columnName,
			this.locationCd,
			this.locationName,
			this.locationTitle,
			this.isDisabled,
			this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgwLayoutDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgwLayoutDto();
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
        this._service.getLayoutToExcel(
			this.zoneCd,
			this.areaCd,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }
    
    
}
