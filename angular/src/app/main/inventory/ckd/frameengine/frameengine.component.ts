import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdFrameEngineDto, InvCkdFrameEngineServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './frameengine.component.html',
    styleUrls: ['./frameengine.component.less'],
})
export class FrameEngineComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvCkdFrameEngineDto = new InvCkdFrameEngineDto();
    saveSelectedRow: InvCkdFrameEngineDto = new InvCkdFrameEngineDto();
    datas: InvCkdFrameEngineDto = new InvCkdFrameEngineDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    invoiceId ;
	frameEngineNo;
	type: string = '' ;
	supplierNo : string = '' ;
	billofladingNo : string = '' ;
	invoiceNo : string = '' ;
	invoiceDateFrom : any ;
	invoiceDateTo : any ;

    ordertypeCode: string = '';
    ordertypeCodeList = [
        { value: '', label: "All" },
        { value: 'R', label: "Regular" },
        { value: 'C', label: "CPO" },
        { value: 'S', label: "SPO" },
        { value: 'C&S', label: "CPO&SPO" },
    ];

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
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
        private _service: InvCkdFrameEngineServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Part No'),headerTooltip: this.l('Part No'),field: 'partNo',flex: 1},
            {headerName: this.l('Supplier No'),headerTooltip: this.l('supplier No'),field: 'supplierNo',flex: 1},
			{headerName: this.l('Bill Of Lading No'),headerTooltip: this.l('billoflading No'),field: 'billofladingNo',flex: 1},
			{headerName: this.l('Invoice No'),headerTooltip: this.l('Invoice No'),field: 'invoiceNo',flex: 1},
			{headerName: this.l('Lot No'),headerTooltip: this.l('Lot No'),field: 'lotNo',flex: 1},
			{headerName: this.l('Lot Code'),headerTooltip: this.l('Lot Code'),field: 'lotCode',flex: 1},
			{headerName: this.l('Invoice Date'),headerTooltip: this.l('Invoice Date'),field: 'invoiceDate',valueGetter: (params) => this.pipe.transform(params.data?.invoiceDate, 'dd/MM/yyyy'),flex: 1},
			{headerName: this.l('Frame No'),headerTooltip: this.l('Frame No'),field: 'frameNo',flex: 1},
			{headerName: this.l('Engine No'),headerTooltip: this.l('Engine No'),field: 'engineNo',flex: 1},
            {headerName: this.l('Case/Module No'),headerTooltip: this.l('Case/Module No'),field: 'module',flex: 1},
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
        this._service.getDataFrameEngine(
			this.invoiceId,
			this.frameEngineNo,
			this.type,
			this.supplierNo,
			this.billofladingNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
            this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.ordertypeCode,
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

    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    clearTextSearch() {
    this.invoiceId= '',
    this.frameEngineNo= '',
    this.type= '',
    this.supplierNo= '',
    this.billofladingNo= '',
    this.invoiceNo= '',
    this.invoiceDateFrom= '',
    this.invoiceDateTo= '',
    this.ordertypeCode = '';
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
        return this._service.getDataFrameEngine(
 			this.invoiceId,
			this.frameEngineNo,
			this.type,
			this.supplierNo,
			this.billofladingNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
            this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.ordertypeCode,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdFrameEngineDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdFrameEngineDto();
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
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFrameEngineToExcel(
			this.invoiceId,
			this.frameEngineNo,
			this.type,
			this.supplierNo,
			this.billofladingNo,
			this.invoiceNo,
            this._dateTimeService.convertToDatetime(this.invoiceDateFrom),
            this._dateTimeService.convertToDatetime(this.invoiceDateTo),
            this.ordertypeCode
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }

    buttonAction(e:string, _type:number){



        let _btnUncheck = document.querySelector('.actionButton_w'+e+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.type = '';
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+e);
            if(_btn) _btn.classList.add('active');
            this.type = _type.toString();
        }
        this.searchDatas();

    }
}
