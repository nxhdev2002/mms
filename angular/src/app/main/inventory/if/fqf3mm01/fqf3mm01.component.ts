import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM01Dto, IF_FQF3MM01ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewFqf3mm01ValidateModalComponent } from './view-fqf3mm01-validate-modal.component';
import { ViewFqf3mm01ValidateResultModalComponent } from './view-fqf3mm01-validate-result-modal.component';
import { ChangeDetectorRef } from '@angular/core';

@Component({
    templateUrl: './fqf3mm01.component.html',
})
export class FQF3MM01Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ViewFqf3mm01Validate', { static: true }) ViewFqf3mm01Validate: ViewFqf3mm01ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm01ValidateResult', { static: true }) ViewFqf3mm01ValidateResult: ViewFqf3mm01ValidateResultModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM01Dto = new IF_FQF3MM01Dto();
    saveSelectedRow: IF_FQF3MM01Dto = new IF_FQF3MM01Dto();
    datas: IF_FQF3MM01Dto = new IF_FQF3MM01Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    vin: string = '';
    lotCode: string = '';
    smsCarFamilyCode: string = '';
    lineOffDatetimeFrom;
    lineOffDatetimeTo;
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
        private _service: IF_FQF3MM01ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Record Id(M)'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Vin(M)'), headerTooltip: this.l('Vin'), field: 'vin', flex: 1 },
            { headerName: this.l('Urn(O)'), headerTooltip: this.l('Urn'), field: 'urn', flex: 1 },
            { headerName: this.l('Spec Sheet No(M)'), headerTooltip: this.l('Spec Sheet No'), field: 'specSheetNo', flex: 1 },
            { headerName: this.l('Id Line(M)'), headerTooltip: this.l('Id Line'), field: 'idLine', flex: 1 },
            { headerName: this.l('Katashiki(M)'), headerTooltip: this.l('Katashiki'), field: 'katashiki', flex: 1 },
            { headerName: this.l('Sale Katashiki(M)'), headerTooltip: this.l('Sale Katashiki'), field: 'saleKatashiki', flex: 1 },
            { headerName: this.l('Sale Suffix(M)'), headerTooltip: this.l('Sale Suffix'), field: 'saleSuffix', flex: 1 },
            { headerName: this.l('Spec 200 Digits(M)'), headerTooltip: this.l('Spec 200 Digits'), field: 'spec200Digits', flex: 1 },
            { headerName: this.l('Production Suffix(M)'), headerTooltip: this.l('Production Suffix'), field: 'productionSuffix', flex: 1 },
            { headerName: this.l('Lot Code(O)'), headerTooltip: this.l('Lot Code'), field: 'lotCode', flex: 1 },
            { headerName: this.l('Engine Prefix(M)'), headerTooltip: this.l('Engine Prefix'), field: 'enginePrefix', flex: 1 },
            { headerName: this.l('Engine No(M)'), headerTooltip: this.l('Engine No'), field: 'engineNo', flex: 1 },
            { headerName: this.l('Plant Code(M)'), headerTooltip: this.l('Plant Code'), field: 'plantCode', flex: 1 },
            { headerName: this.l('Current Status(M)'), headerTooltip: this.l('Current Status'), field: 'currentStatus', flex: 1 },
            { headerName: this.l('Line Off Datetime(M)'), headerTooltip: this.l('Line Off Datetime'), field: 'lineOffDatetime', flex: 1 },
            { headerName: this.l('Interior Color(M)'), headerTooltip: this.l('Interior Color'), field: 'interiorColor', flex: 1 },
            { headerName: this.l('Exterior Color(M)'), headerTooltip: this.l('Exterior Color'), field: 'exteriorColor', flex: 1 },
            { headerName: this.l('Destination Code(M)'), headerTooltip: this.l('Destination Code'), field: 'destinationCode', flex: 1 },
            { headerName: this.l('Ed Odno(M)'), headerTooltip: this.l('Ed Odno'), field: 'edOdno', flex: 1 },
            {headerName: this.l('Cancel Flag(M)'), headerTooltip: this.l('Cancel Flag'), field: 'cancelFlag', cellClass: ['text-center'], width: 120, flex: 1,},
			{ headerName: this.l('Sms Car Family Code(M)'), headerTooltip: this.l('Sms Car Family Code'), field: 'smsCarFamilyCode', flex: 1 },
            {headerName: this.l('Order Type(M)'), headerTooltip: this.l('Order Type'), field: 'orderType', flex: 1},
			{ headerName: this.l('Katashiki Code(O)'), headerTooltip: this.l('Katashiki Code'), field: 'katashikiCode', flex: 1 },
            {headerName: this.l('End Of Record(M)'), headerTooltip: this.l('End Of Record'), field: 'endOfRecord', flex: 1}
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }


    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        this.cdr.detectChanges();
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        this.autoSizeAll();
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.lotCode,
            this.vin,
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
            this.smsCarFamilyCode,
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
                this.isLoading = false;
            });
    }

    clearTextSearch() {
            this.vin = '',
            this.lotCode = '',
            this.lineOffDatetimeFrom = '',
            this.lineOffDatetimeTo = '',
            this.smsCarFamilyCode = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.lotCode,
            this.vin,
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
            this.smsCarFamilyCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM01Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM01Dto();
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
            this.resetGridView();
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getIF_FQF3MM01ToExcel(
            this.lotCode,
            this.vin,
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom),
            this._dateTimeService.convertToDatetime(this.lineOffDatetimeTo),
            this.smsCarFamilyCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
    reCreateData(e): void {
        if (this.lineOffDatetimeFrom == null || this.lineOffDatetimeFrom == '') {
            this.message.warn(this.l('Hãy nhập Line Off Date Time trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM01(
                this._dateTimeService.convertToDatetime(this.lineOffDatetimeFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewValidate(){
        this.ViewFqf3mm01Validate.show(this.lineOffDatetimeFrom, this.lineOffDatetimeTo);
    }

    viewValidateData() {
        this.ViewFqf3mm01ValidateResult.show(this.lineOffDatetimeFrom, this.lineOffDatetimeTo);
    }
}
