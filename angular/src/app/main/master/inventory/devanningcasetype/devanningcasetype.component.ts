import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvDevanningCaseTypeDto, MstInvDevanningCaseTypeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditDevanningCaseTypeModalComponent } from './create-or-edit-devanningcasetype-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './devanningcasetype.component.html',
    styleUrls: ['./devanningcasetype.component.less'],
})
export class DevanningCaseTypeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalDevanningCaseType', { static: true }) createOrEditModalDevanningCaseType:| CreateOrEditDevanningCaseTypeModalComponent| undefined;
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

    changedRecordsDevaningCaseType: number[] = [];
    selectId;
    selectedRow: MstInvDevanningCaseTypeDto = new MstInvDevanningCaseTypeDto();
    saveSelectedRow: MstInvDevanningCaseTypeDto = new MstInvDevanningCaseTypeDto();
    datas: MstInvDevanningCaseTypeDto = new MstInvDevanningCaseTypeDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    source : string = '' ;
    caseNo : string = '' ;
	carFamilyCode : string = '' ;
    shoptypeCode : string = '' ;
    type: string = '' ;
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
        private _service: MstInvDevanningCaseTypeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Source'),headerTooltip: this.l('Source'),field: 'source',flex: 1},
            {headerName: this.l('Case No'),headerTooltip: this.l('Case No'),field: 'caseNo',flex: 1},
            {headerName: this.l('Shoptype Code'),headerTooltip: this.l('Shoptype Code'),field: 'shoptypeCode',flex: 1},
            {headerName: this.l('Type'),headerTooltip: this.l('Type'),field: 'type',flex: 1},
            {headerName: this.l('Car Family Code'),headerTooltip: this.l('Car Family Code'),field: 'carFamilyCode',flex: 1},
			{headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            buttonDefTwo: {
            text: params => (params.data?.isActive === 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive === 'Y') ? 'btnActive' : 'btnInActive',},}
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsDevaningCaseType= result;
            console.log("result =", result);
        })
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.source,
			this.caseNo,
            this.carFamilyCode,
			this.shoptypeCode,
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
        this.caseNo = '',
		this.carFamilyCode = '',
        this.shoptypeCode = '',
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
 			this.caseNo,
            this.carFamilyCode,
			this.shoptypeCode,
			this.type,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvDevanningCaseTypeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvDevanningCaseTypeDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
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

    deleteRow(system: MstInvDevanningCaseTypeDto): void {
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
        this._service.getMstInvDevanningCaseTypeToExcel(
            this.source,
			this.caseNo,
            this.shoptypeCode,
            this.type,
			this.carFamilyCode,
			this.isActive,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }
}
