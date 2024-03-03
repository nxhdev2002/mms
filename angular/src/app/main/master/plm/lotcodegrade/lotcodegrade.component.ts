import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstPlmLotCodeGradeDto, MstPlmLotCodeGradeServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditLotCodeGradeModalComponent } from './create-or-edit-lotcodegrade-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './lotcodegrade.component.html',
    styleUrls: ['./lotcodegrade.component.less'],
})
export class LotCodeGradeComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalLotCodeGrade', { static: true }) createOrEditModalLotCodeGrade:| CreateOrEditLotCodeGradeModalComponent| undefined;
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

    selectedRow: MstPlmLotCodeGradeDto = new MstPlmLotCodeGradeDto();
    saveSelectedRow: MstPlmLotCodeGradeDto = new MstPlmLotCodeGradeDto();
    datas: MstPlmLotCodeGradeDto = new MstPlmLotCodeGradeDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    modelId : string = '' ;
	lotCode : string = '' ;
	cfc : string = '' ;
	grade
	odering
	gradeName
	modeCode
	modelVin
	visStart : string = '' ;
	visEnd : string = '' ;
	maLotCode
	vehicleId
	testNo
	dab
	pab
	engCode
	lab
	rab
	kab
	isFcLabel
	isActive
	r
	g
	b
	clab
	charStr

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
        private _service: MstPlmLotCodeGradeServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Model Id'),headerTooltip: this.l('Model Id'),field: 'modelId',flex: 1},
			{headerName: this.l('Lot Code'),headerTooltip: this.l('Lot Code'),field: 'lotCode',flex: 1},
			{headerName: this.l('Cfc'),headerTooltip: this.l('Cfc'),field: 'cfc',flex: 1},
			{headerName: this.l('Grade'),headerTooltip: this.l('Grade'),field: 'grade',flex: 1},
			{headerName: this.l('Odering'),headerTooltip: this.l('Odering'),field: 'odering',flex: 1},
			{headerName: this.l('Grade Name'),headerTooltip: this.l('Grade Name'),field: 'gradeName',flex: 1},
			{headerName: this.l('Mode Code'),headerTooltip: this.l('Mode Code'),field: 'modeCode',flex: 1},
			{headerName: this.l('Model Vin'),headerTooltip: this.l('Model Vin'),field: 'modelVin',flex: 1},
			{headerName: this.l('Vis Start'),headerTooltip: this.l('Vis Start'),field: 'visStart',flex: 1},
			{headerName: this.l('Vis End'),headerTooltip: this.l('Vis End'),field: 'visEnd',flex: 1},
			{headerName: this.l('Ma Lot Code'),headerTooltip: this.l('Ma Lot Code'),field: 'maLotCode',flex: 1},
			{headerName: this.l('Vehicle Id'),headerTooltip: this.l('Vehicle Id'),field: 'vehicleId',flex: 1},
			{headerName: this.l('Test No'),headerTooltip: this.l('Test No'),field: 'testNo',flex: 1},
			{headerName: this.l('Dab'),headerTooltip: this.l('Dab'),field: 'dab',flex: 1},
			{headerName: this.l('Pab'),headerTooltip: this.l('Pab'),field: 'pab',flex: 1},
			{headerName: this.l('Eng Code'),headerTooltip: this.l('Eng Code'),field: 'engCode',flex: 1},
			{headerName: this.l('Lab'),headerTooltip: this.l('Lab'),field: 'lab',flex: 1},
			{headerName: this.l('Rab'),headerTooltip: this.l('Rab'),field: 'rab',flex: 1},
			{headerName: this.l('Kab'),headerTooltip: this.l('Kab'),field: 'kab',flex: 1},
			{headerName: this.l('Is Fc Label'),headerTooltip: this.l('Is Fc Label'),field: 'isFcLabel',flex: 1},
			{headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field: 'isActive',flex: 1},
			{headerName: this.l('R'),headerTooltip: this.l('R'),field: 'r',flex: 1},
			{headerName: this.l('G'),headerTooltip: this.l('G'),field: 'g',flex: 1},
			{headerName: this.l('B'),headerTooltip: this.l('B'),field: 'b',flex: 1},
			{headerName: this.l('Clab'),headerTooltip: this.l('Clab'),field: 'clab',flex: 1},
			{headerName: this.l('Char Str'),headerTooltip: this.l('Char Str'),field: 'charStr',flex: 1},
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

			this.lotCode,
			this.cfc,
			this.grade,

			this.modeCode,
			this.modelVin,

			this.maLotCode,

			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.resetGridView();
        });
    }
    autoSizeAll() {
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
            this.autoSizeAll();
        },1000)
    }

    clearTextSearch() {
        this.modelId = '',
		this.lotCode = '',
		this.cfc = '',
		this.grade = '',
		this.odering = '',
		this.gradeName = '',
		this.modeCode = '',
		this.modelVin = '',
		this.visStart = '',
		this.visEnd = '',
		this.maLotCode = '',
		this.vehicleId = '',
		this.testNo = '',
		this.dab = '',
		this.pab = '',
		this.engCode = '',
		this.lab = '',
		this.rab = '',
		this.kab = '',
		this.isFcLabel = '',
		this.isActive = '',
		this.r = '',
		this.g = '',
		this.b = '',
		this.clab = '',
		this.charStr = '',
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
            this.lotCode,
			this.cfc,
			this.grade,
			this.modeCode,
			this.modelVin,
			this.maLotCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstPlmLotCodeGradeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstPlmLotCodeGradeDto();
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
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    deleteRow(system: MstPlmLotCodeGradeDto): void {
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
        this._service.getLotCodeGradeToExcel(
            this.lotCode,
			this.cfc,
			this.grade,
			this.modeCode,
			this.modelVin,
			this.maLotCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
        });
    }
}
