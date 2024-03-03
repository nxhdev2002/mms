import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { PlnCcrProductionPlanDto, PlnCcrProductionPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditProductionPlanModalComponent } from './create-or-edit-productionplan-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportPlnCcrProductionPlanComponent } from './import-productionplan-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    templateUrl: './productionplan.component.html',
    styleUrls: ['./productionplan.component.less'],
})
export class ProductionPlanComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalProductionPlan', { static: true }) createOrEditModalProductionPlan:| CreateOrEditProductionPlanModalComponent| undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportPlnCcrProductionPlanComponent| undefined;


    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: PlnCcrProductionPlanDto = new PlnCcrProductionPlanDto();
    saveSelectedRow: PlnCcrProductionPlanDto = new PlnCcrProductionPlanDto();
    datas: PlnCcrProductionPlanDto = new PlnCcrProductionPlanDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    planSequence
	shop : string = '' ;
	model : string = '' ;
	lotNo : string = '' ;
	noInLot : string = '' ;
	grade : string = '' ;
	body : string = '' ;
	dateIn : any ;
	timeIn : any ;
	dateTimeIn : any ;
	supplierNo : string = '' ;
	useLotNo : string = '' ;
	supplierNo2 : string = '' ;
	useLotNo2 : string = '' ;
	useNoInLot : string = '' ;

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
        private _service: PlnCcrProductionPlanServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService
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
                headerName: this.l('Plan Sequence'),
                headerTooltip: this.l('Plan Sequence'),
                field: 'planSequence',
                width:120,
            },
			{
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                width:100,
            },
			{
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field:  'model',
                width:100,

            },
			{
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                flex:1,
            },
			{
                headerName: this.l('No In Lot'),
                headerTooltip: this.l('No In Lot'),
                field: 'noInLot',
                width:100,
            },
			{
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width:100,
            },
			{
                headerName: this.l('Body'),
                headerTooltip: this.l('Body'),
                field: 'body',
                width:100,
            },
			{
                headerName: this.l('Date In'),
                headerTooltip: this.l('Date In'),
                field: 'dateIn',
                valueGetter: (params) => this.pipe.transform(params.data?.dateIn, 'dd/MM/yyyy'),
                flex:1,
            },
			{
                headerName: this.l('Time In'),
                headerTooltip: this.l('Time In'),
                field: 'timeIn',
                flex:1,
            },
			{
                headerName: this.l('Date Time In'),
                headerTooltip: this.l('Date Time In'),
                field: 'dateTimeIn',
                valueGetter: (params) => this.pipe.transform(params.data?.dateTimeIn, 'dd/MM/yyyy HH:mm'),
                flex:1,
            },
			{
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex:1,
            },
			{
                headerName: this.l('Use Lot No'),
                headerTooltip: this.l('Use Lot No'),
                field: 'useLotNo',
                flex:1,
            },
			{
                headerName: this.l('Supplier No 2'),
                headerTooltip: this.l('Supplier No 2'),
                field: 'supplierNo2',
                flex:1,
            },
			{
                headerName: this.l('Use Lot No 2'),
                headerTooltip: this.l('Use Lot No 2'),
                field: 'useLotNo2',
                flex:1,
            },
			{
                headerName: this.l('Use No In Lot'),
                headerTooltip: this.l('Use No In Lot'),
                field: 'useNoInLot',
                flex:1,
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
			this.shop,
			this.model,
			this.lotNo,
			this._dateTimeService.convertToDatetime(this.dateIn),
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
		this.shop = '',
		this.model = '',
		this.lotNo = '',
		this.dateIn = '',
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
			this.shop,
			this.model,
			this.lotNo,
			this._dateTimeService.convertToDatetime(this.dateIn),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => PlnCcrProductionPlanDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new PlnCcrProductionPlanDto();
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

    deleteRow(system: PlnCcrProductionPlanDto): void {
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
        this._service.getProductionPlanToExcel(
            this.shop,
			this.model,
			this.lotNo,
			this._dateTimeService.convertToDatetime(this.dateIn),
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
        });
    }





}
