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
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { IF_FQF3MM04Dto, IF_FQF3MM04ServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewFqf3mm04ValidateModalComponent } from './view-fqf3mm04-validate-modal.component';
import { ViewFqf3mm04ValidateResultModalComponent } from './view-fqf3mm04-validate-result-modal.component';

@Component({
    templateUrl: './fqf3mm04.component.html',
})
export class FQF3MM04Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ViewFqf3mm04Validate', { static: true }) ViewFqf3mm04Validate: ViewFqf3mm04ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm04ValidateResult', { static: true }) ViewFqf3mm04ValidateResult: ViewFqf3mm04ValidateResultModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM04Dto = new IF_FQF3MM04Dto();
    saveSelectedRow: IF_FQF3MM04Dto = new IF_FQF3MM04Dto();
    datas: IF_FQF3MM04Dto = new IF_FQF3MM04Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    recordId
	invoiceNo : string = '' ;
	renban : string = '' ;
	containerNo : string = '' ;
	moduleNo : string = '' ;
	devaningDateFrom : any;
    devaningDateTo : any;
	plant : string = '' ;
	cancelFlag : string = '' ;
	endOfRecord : string = '' ;

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
        private _service: IF_FQF3MM04ServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Record Id (M)'),headerTooltip: this.l('Record Id (M)'),field: 'recordId',flex: 1},
			{headerName: this.l('Invoice No (M)'),headerTooltip: this.l('Invoice No (M)'),field: 'invoiceNo',flex: 1},
			{headerName: this.l('Renban (M)'),headerTooltip: this.l('Renban (M)'),field: 'renban',flex: 1},
			{headerName: this.l('Container No (M)'),headerTooltip: this.l('Container No (M)'),field: 'containerNo',flex: 1},
			{headerName: this.l('Module No (O)'),headerTooltip: this.l('Module No (O)'),field: 'moduleNo',flex: 1},
			{headerName: this.l('Devaning Date (M)'),headerTooltip: this.l('Devaning Date (M)'),field: 'devaningDate',flex: 1,valueFormatter: (params) => this.pipe.transform(params.data?.devaningDate, 'dd/MM/yyyy')},
			{headerName: this.l('Plant (M)'),headerTooltip: this.l('Plant (M)'),field: 'plant',flex: 1},
			{headerName: this.l('Cancel Flag (M)'),headerTooltip: this.l('Cancel Flag (M)'),field:  'cancelFlag',flex: 1},
			{headerName: this.l('End Of Record (M)'),headerTooltip: this.l('End Of Record (M)'),field:  'endOfRecord',flex: 1},
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service.getAll(
			this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo),
            this.invoiceNo,
            this.renban,
            this.containerNo,
            this.moduleNo,
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
        this.recordId = '',
        this.devaningDateFrom = '',
        this.devaningDateTo = '',
        this.invoiceNo= '',
        this.renban= '',
        this.containerNo = '';
        this.moduleNo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo),
            this.invoiceNo,
            this.renban,
            this.containerNo,
            this.moduleNo,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM04Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM04Dto();
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM04ToExcel(
            this._dateTimeService.convertToDatetime(this.devaningDateFrom),
            this._dateTimeService.convertToDatetime(this.devaningDateTo),
            this.invoiceNo,
            this.renban,
            this.containerNo,
            this.moduleNo
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
    reCreateData(e): void {
        if (this.devaningDateFrom == null || this.devaningDateFrom == '') {
            this.message.warn(this.l('Hãy nhập Devanning Date trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM04(
                this._dateTimeService.convertToDatetime(this.devaningDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }
    viewValidate(){
        this.ViewFqf3mm04Validate.show(this.devaningDateFrom, this.devaningDateTo);
    }
    
    viewValidateData() {
        this.ViewFqf3mm04ValidateResult.show(this.devaningDateFrom, this.devaningDateTo);
    }
}

