import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgwEciPartDto, MstLgwEciPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditEciPartModalComponent } from './create-or-edit-ecipart-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportEciPartComponent } from './import-ecipart-modal.component';

@Component({
    templateUrl: './ecipart.component.html',
    styleUrls: ['./ecipart.component.less'],
})
export class EciPartComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalEciPart', { static: true }) createOrEditModalEciPart:| CreateOrEditEciPartModalComponent| undefined;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportEciPartComponent| undefined;
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

    selectedRow: MstLgwEciPartDto = new MstLgwEciPartDto();
    saveSelectedRow: MstLgwEciPartDto = new MstLgwEciPartDto();
    datas: MstLgwEciPartDto = new MstLgwEciPartDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    moduleNo : string = '' ;
	partNo : string = '' ;
	supplierNo : string = '' ;
	moduleNoEci : string = '' ;
	partNoEci : string = '' ;
    supplierNoEci : string = '' ;
	startEciSeq : string = '' ;
	startEciRenban : string = '' ;
	startEciModule : string = '' ;
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
        private _service: MstLgwEciPartServiceProxy,
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
                headerName: this.l('Module No'),
                headerTooltip: this.l('Module No'),field: 'moduleNo',
                flex: 1
            },
			{
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                width:100,
            },
			{
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
			{
                headerName: this.l('Module No Eci'),
                headerTooltip: this.l('Module No Eci'),
                field: 'moduleNoEci',
                flex: 1
            },
			{
                headerName: this.l('Part No Eci'),
                headerTooltip: this.l('Part No Eci'),
                field: 'partNoEci',
                flex: 1
            },
			{
                headerName: this.l('Supplier No Eci'),
                headerTooltip: this.l('Supplier No Eci'),
                field: 'supplierNoEci',
                flex: 1
            },
			{
                headerName: this.l('Start Eci Seq'),
                headerTooltip: this.l('Start Eci Seq'),
                field: 'startEciSeq',
                flex: 1
            },
			{
                headerName: this.l('Start Eci Renban'),
                headerTooltip: this.l('Start Eci Renban'),
                field: 'startEciRenban',
                flex: 1
            },
			{
                headerName: this.l('Start Eci Module'),
                headerTooltip: this.l('Start Eci Module'),
                field: 'startEciModule',
                flex: 1
            },

            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },
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
			this.moduleNo,
			this.partNo,
			this.supplierNo,
			this.moduleNoEci,
			this.partNoEci,
			this.supplierNoEci,
			this.startEciSeq,
			this.startEciRenban,
			this.startEciModule,
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
        this.moduleNo = '',
		this.partNo = '',
		this.supplierNo = '',
		this.moduleNoEci = '',
		this.partNoEci = '',
        this.supplierNoEci = '',
		this.startEciSeq = '',
		this.startEciRenban = '',
		this.startEciModule = '',
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
 			this.moduleNo,
			this.partNo,
			this.supplierNo,
			this.moduleNoEci,
			this.partNoEci,
			this.supplierNoEci,
			this.startEciSeq,
			this.startEciRenban,
			this.startEciModule,
			this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgwEciPartDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgwEciPartDto();
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

    deleteRow(system: MstLgwEciPartDto): void {
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
        this._service.getEciPartToExcel(
			this.moduleNo,
			this.partNo,
			this.supplierNo,
			this.moduleNoEci,
			this.partNoEci,
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }
    
    
}
