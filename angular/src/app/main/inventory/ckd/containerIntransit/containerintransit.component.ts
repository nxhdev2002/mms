import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerIntransitDto, InvCkdContainerIntransitServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { CommonFunction } from '@app/main/commonfuncton.component';


@Component({
    templateUrl: './containerintransit.component.html',
})
export class ContainerIntransitComponent extends AppComponentBase implements OnInit {
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

    selectedRow: InvCkdContainerIntransitDto = new InvCkdContainerIntransitDto();
    saveSelectedRow: InvCkdContainerIntransitDto = new InvCkdContainerIntransitDto();
    datas: InvCkdContainerIntransitDto = new InvCkdContainerIntransitDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    	containerNo : string = '' ;
	renban : string = '' ;
	supplierNo : string = '' ;
	shippingDate
	portDate
	transDate
	tmvDate
	cif
	tax
	inland : number ;
	thc
	fob
	freight
	insurance
	amount
	status : string = '' ;
	periodDate
	periodId
	cifVn
	taxVn
	inlandVn
	thcVn
	fobVn
	freightVn
	insuranceVn
	amountVn
	forwarder : string = '' ;
	generated : string = '' ;
	shop : string = '' ;
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
        private _service: InvCkdContainerIntransitServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _fm:DataFormatService,
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Container No'),headerTooltip: this.l('Container No'),field: 'containerNo'},
			{headerName: this.l('Renban'),headerTooltip: this.l('Renban'),field: 'renban'},
			{headerName: this.l('Supplier No'),headerTooltip: this.l('Supplier No'),field: 'supplierNo'},
			{headerName: this.l('ETD'),headerTooltip: this.l('ETD'),field: 'shippingDate',valueGetter: (params) => this.pipe.transform(params.data?.shippingDate, 'dd/MM/yyyy')},
			{headerName: this.l('ETA'),headerTooltip: this.l('ETA'),field: 'portDate',valueGetter: (params) => this.pipe.transform(params.data?.portDate, 'dd/MM/yyyy')},
			{headerName: this.l('Trans Date'),headerTooltip: this.l('TransDate'),field: 'transDate',valueGetter: (params) => this.pipe.transform(params.data?.transDate, 'dd/MM/yyyy')},
			{headerName: this.l('Tmv Date'),headerTooltip: this.l('TmvDate'),field: 'tmvDate',valueGetter: (params) => this.pipe.transform(params.data?.tmvDate, 'dd/MM/yyyy')},
			{headerName: this.l('Cif'),headerTooltip: this.l('Cif'),field: 'cif',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.cif, 4)},
			{headerName: this.l('Tax'),headerTooltip: this.l('Tax'),field: 'tax',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.tax, 4)},
			{headerName: this.l('Inland'),headerTooltip: this.l('Inland'),field: 'inland',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.inland, 4)},
			{headerName: this.l('Thc'),headerTooltip: this.l('Thc'),field: 'thc',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thc, 4)},
			{headerName: this.l('Fob'),headerTooltip: this.l('Fob'),field: 'fob',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fob, 4)},
			{headerName: this.l('Freight'),headerTooltip: this.l('Freight'),field: 'freight',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.frieght, 4)},
			{headerName: this.l('Insurance'),headerTooltip: this.l('Insurance'),field: 'insurance',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insurance, 4)},
			{headerName: this.l('Amount'),headerTooltip: this.l('Amount'),field: 'amount',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amount, 4)},
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status'},
			{headerName: this.l('Period Date'),headerTooltip: this.l('Period Date'),field: 'periodDate',valueGetter: (params) => this.pipe.transform(params.data?.periodDate, 'dd/MM/yyyy')},
			{headerName: this.l('Period Id'),headerTooltip: this.l('Period Id'),field: 'periodId'},
			{headerName: this.l('Cif Vn'),headerTooltip: this.l('Cif Vn'),field: 'cifVn',
            valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.cifVn, 4)},
			{headerName: this.l('Tax Vn'),headerTooltip: this.l('Tax Vn'),field: 'taxVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.taxVn, 4)},
			// {headerName: this.l('Inland Vn'),headerTooltip: this.l('Inland Vn'),field: 'inlandVn'},
			{headerName: this.l('Inland Vn').toLocaleString(),headerTooltip: this.l('Inland Vn'),field: 'inlandVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.inlandVn, 4) },
			{headerName: this.l('Thc Vn'),headerTooltip: this.l('Thc Vn'),field: 'thcVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thcVn, 4)},
			{headerName: this.l('Fob Vn'),headerTooltip: this.l('Fob Vn'),field: 'fobVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.fobVn, 4)},
			{headerName: this.l('Freight Vn'),headerTooltip: this.l('Freight Vn'),field: 'freightVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.freightVn, 4)},
			{headerName: this.l('Insurance Vn'),headerTooltip: this.l('Insurance Vn'),field: 'insuranceVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.insuranceVn, 4)},
			{headerName: this.l('Amount Vn'),headerTooltip: this.l('Amount Vn'),field: 'amountVn',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.amountVn, 4)},
			{headerName: this.l('Forwarder'),headerTooltip: this.l('Forwarder'),field: 'forwarder'},
			{headerName: this.l('Generated'),headerTooltip: this.l('Generated'),field:  'generated', cellClass: ['text-center'], width: 90 },

			{headerName: this.l('Shop'),headerTooltip: this.l('Shop'),field: 'shop'},
			// {headerName: this.l('Is Active'),headerTooltip: this.l('Is Active'),field:  'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
            // buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',iconName: 'fa fa-circle',className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',},}
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
			this.containerNo,
			this.renban,
			this.supplierNo,
			this._dateTimeService.convertToDatetime(this.shippingDate),
            this._dateTimeService.convertToDatetime(this.portDate),
            this._dateTimeService.convertToDatetime(this.transDate),
            this._dateTimeService.convertToDatetime(this.tmvDate),
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
        this.containerNo = '',
		this.renban = '',
		this.supplierNo = '',
		this.shippingDate = '',
		this.portDate = '',
		this.transDate = '',
		this.tmvDate = '',

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
 			this.containerNo,
			this.renban,
			this.supplierNo,
			this._dateTimeService.convertToDatetime(this.shippingDate),
            this._dateTimeService.convertToDatetime(this.portDate),
            this._dateTimeService.convertToDatetime(this.transDate),
            this._dateTimeService.convertToDatetime(this.tmvDate),
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdContainerIntransitDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdContainerIntransitDto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getContainerIntransitToExcel(
			this.containerNo,
			this.renban,
			this.supplierNo,
			this._dateTimeService.convertToDatetime(this.shippingDate),
            this._dateTimeService.convertToDatetime(this.portDate),
            this._dateTimeService.convertToDatetime(this.transDate),
            this._dateTimeService.convertToDatetime(this.tmvDate),
			this.cif,
			this.tax,
			this.inland,
			this.thc,
			this.fob,
			this.freight,
			this.insurance,
			this.amount,
			this.status,
			this.periodDate,
			this.periodId,
			this.cifVn,
			this.taxVn,
			this.inlandVn,
			this.thcVn,
			this.fobVn,
			this.freightVn,
			this.insuranceVn,
			this.amountVn,
			this.forwarder,
			this.generated,
			this.shop,
			this.isActive,
            )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
        });
    }


}
