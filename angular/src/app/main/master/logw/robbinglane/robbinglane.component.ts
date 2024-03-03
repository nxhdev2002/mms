import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwRobbingLaneDto, MstLgwRobbingLaneServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditRobbingLaneModalComponent } from './create-or-edit-robbinglane-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './robbinglane.component.html',
    styleUrls: ['./robbinglane.component.less'],
})
export class RobbingLaneComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalRobbingLane', { static: true }) createOrEditModalRobbingLane:| CreateOrEditRobbingLaneModalComponent| undefined;
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

    selectedRow: MstLgwRobbingLaneDto = new MstLgwRobbingLaneDto();
    saveSelectedRow: MstLgwRobbingLaneDto = new MstLgwRobbingLaneDto();
    datas: MstLgwRobbingLaneDto = new MstLgwRobbingLaneDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    laneNo : string = '' ;
	laneName : string = '' ;
	contNo : string = '' ;
	renban : string = '' ;
	supplierNo : string = '' ;
	showOnly : string = '' ;
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
        private _service: MstLgwRobbingLaneServiceProxy,
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
                width: 55,},
            {
                headerName: this.l('Lane No'),
                headerTooltip: this.l('Lane No'),
                field: 'laneNo',
                flex: 1
            },
			{
                headerName: this.l('Lane Name'),
                headerTooltip: this.l('Lane Name'),
                field: 'laneName',
                flex: 1
            },
			{
                headerName: this.l('Cont No'),
                headerTooltip: this.l('Cont No'),
                field: 'contNo',
                flex: 1
            },
			{
                headerName: this.l('Renban'),
                headerTooltip: this.l('Renban'),
                field: 'renban',
                flex: 1
            },
			{
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
			{
                headerName: this.l('Show Only'),
                headerTooltip: this.l('Show Only'),
                field:  'showOnly',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.showOnly == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.showOnly == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
			{
                headerName: this.l('Is Disabled'),
                headerTooltip: this.l('Is Disabled'),
                field:  'isDisabled',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isDisabled == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isDisabled == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
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
                flex: 1,
            },
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
			this.laneNo,
			this.laneName,
			this.contNo,
			this.renban,
			this.supplierNo,
			this.showOnly,
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
        this.laneNo = '',
		this.laneName = '',
		this.contNo = '',
		this.renban = '',
		this.supplierNo = '',
		this.showOnly = '',
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
 			this.laneNo,
			this.laneName,
			this.contNo,
			this.renban,
			this.supplierNo,
			this.showOnly,
			this.isDisabled,
			this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgwRobbingLaneDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgwRobbingLaneDto();
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

    deleteRow(system: MstLgwRobbingLaneDto): void {
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
        this._service.getRobbingLaneToExcel(
			this.laneNo,
			this.laneName,
			this.contNo,
			this.renban,
			this.supplierNo,
			this.showOnly,
			this.isDisabled,
			this.isActive,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }

    
    
}
