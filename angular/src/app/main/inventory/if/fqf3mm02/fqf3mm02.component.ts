import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM02Dto, IF_FQF3MM02ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ViewFqf3mm02ValidateModalComponent } from './view-fqf3mm02-validate-modal.component';
import { ViewFqf3mm01ValidateResultModalComponent } from '../fqf3mm01/view-fqf3mm01-validate-result-modal.component';
import { ChangeDetectorRef } from '@angular/core';

@Component({
    templateUrl: './fqf3mm02.component.html',
})
export class FQF3MM02Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ViewFqf3mm02Validate', { static: true }) ViewFqf3mm02Validate: ViewFqf3mm02ValidateModalComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM02Dto = new IF_FQF3MM02Dto();
    saveSelectedRow: IF_FQF3MM02Dto = new IF_FQF3MM02Dto();
    datas: IF_FQF3MM02Dto = new IF_FQF3MM02Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    fn: CommonFunction = new CommonFunction();

    productionDate: any;
    postingDate: any;
    postingDateTo: any;
    postingDateFrom: any;
    materialCode;
    partCode;
    lineOffDatetime: any;

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
        private _service: IF_FQF3MM02ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Record Id(M)'), headerTooltip: this.l('Record Id'), field: 'recordId', flex: 1 },
            { headerName: this.l('Company Code(M)'), headerTooltip: this.l('Company Code'), field: 'companyCode', flex: 1 },
            { headerName: this.l('Plant Code(M)'), headerTooltip: this.l('Plant Code'), field: 'plantCode', flex: 1 },
            { headerName: this.l('Maru Code(M)'), headerTooltip: this.l('Maru Code'), field: 'maruCode', flex: 1 },
            { headerName: this.l('Receiving Stock Line(O)'), headerTooltip: this.l('Receiving Stock Line'), field: 'receivingStockLine', flex: 1 },
            { headerName: this.l('Production Date(O)'), headerTooltip: this.l('Production Date'), field: 'productionDate', flex: 1 },
            { headerName: this.l('Posting Date(M)'), headerTooltip: this.l('Posting Date'), field: 'postingDate', flex: 1 },
            { headerName: this.l('Part Code(M)'), headerTooltip: this.l('Part Code'), field: 'partCode', flex: 1 },
            { headerName: this.l('Quantity(M)'), headerTooltip: this.l('Quantity'), field: 'quantity', flex: 1, type: 'rightAligned'},
            { headerName: this.l('Spoiled Parts Quantity(O)'), headerTooltip: this.l('Spoiled Parts Quantity'), field: 'spoiledPartsQuantity', flex: 1 , type: 'rightAligned'},
            { headerName: this.l('Spoiled Material Quantity(O)'), headerTooltip: this.l('Spoiled Material Quantity'), field: 'spoiledMaterialQuantity1', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Material Code(M)'), headerTooltip: this.l('Material Code'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Free Shot Quantity(O)'), headerTooltip: this.l('Free Shot Quantity'), field: 'freeShotQuantity', flex: 1, type: 'rightAligned'},
            { headerName: this.l('Recycled Quantity(O)'), headerTooltip: this.l('Recycled Quantity'), field: 'recycledQuantity', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Cost Center(M)'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Normal Cancel Flag(M)'), headerTooltip: this.l('Normal Cancel Flag'), field: 'normalCancelFlag', flex: 1 },
            { headerName: this.l('Grgi No(M)'), headerTooltip: this.l('Grgi No'), field: 'grgiNo', flex: 1 },
            { headerName: this.l('Grgi Type(O)'), headerTooltip: this.l('Grgi Type'), field: 'grgiType', flex: 1 },
            { headerName: this.l('Material Doc Type(M)'), headerTooltip: this.l('Material Doc Type'), field: 'materialDocType', flex: 1 },
            { headerName: this.l('Material Quantity(O)'), headerTooltip: this.l('Material Quantity'), field: 'materialQuantity', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Spoiled Material Quantity(O)'), headerTooltip: this.l('Spoiled Material Quantity'), field: 'spoiledMaterialQuantity2', flex: 1 , type: 'rightAligned'},
            { headerName: this.l('Related Part Receive No(M)'), headerTooltip: this.l('Related Part Receive No'), field: 'relatedPartReceiveNo', flex: 1 },
            { headerName: this.l('Related Gr Type(M)'), headerTooltip: this.l('Related Gr Type'), field: 'relatedGrType', flex: 1 },
            { headerName: this.l('Related Gr Transaction Type(M)'), headerTooltip: this.l('Related Gr Transaction Type'), field: 'relatedGrTransactionType', flex: 1 },
            { headerName: this.l('In-House Part Quantity Receive(M)'), headerTooltip: this.l('In-House Part Quantity Receive'), field: 'inHousePartQuantityReceive', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Related Part Issue No(O)'), headerTooltip: this.l('Related Part Issue No'), field: 'relatedPartIssueNo', flex: 1 },
            { headerName: this.l('Related Gi Type(O)'), headerTooltip: this.l('Related Gi Type'), field: 'relatedGiType', flex: 1 },
            { headerName: this.l('Related Gi Transaction Type(O)'), headerTooltip: this.l('Related Gi Transaction Type'), field: 'relatedGiTransactionType', flex: 1 },
            { headerName: this.l('Related In-House Part Quantity Issued(O)'), headerTooltip: this.l('Related In-House Part Quantity Issued'), field: 'relatedInHousePartQuantityIssued', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Related Spoiled Part Quantity Issued(O)'), headerTooltip: this.l('Related Spoiled Part Quantity Issued'), field: 'relatedSpoiledPartQuantityIssued', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Wip(O)'), headerTooltip: this.l('Wip'), field: 'wip', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Production Id(O)'), headerTooltip: this.l('Production Id'), field: 'productionId', flex: 1 },
            { headerName: this.l('Final Price(M)'), headerTooltip: this.l('Final Price'), field: 'finalPrice', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Wbs(O)'), headerTooltip: this.l('Wbs'), field: 'wbs', flex: 1 },
            { headerName: this.l('Earmarked Fund(O)'), headerTooltip: this.l('Earmarked Fund'), field: 'earmarkedFund', flex: 1 },
            { headerName: this.l('Earmarked Fund Item(O)'), headerTooltip: this.l('Earmarked Fund Item'), field: 'earmarkedFundItem', flex: 1 },
            { headerName: this.l('P-Sms Code(M)'), headerTooltip: this.l('P-Sms Code'), field: 'psmsCode', flex: 1 },
            { headerName: this.l('Gi Uom(M)'), headerTooltip: this.l('Gi Uom'), field: 'giUom', flex: 1 },
            { headerName: this.l('Ending Of Record(M)'), headerTooltip: this.l('Ending Of Record'), field: 'endingOfRecord', flex: 1 },
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
            if (column.getId().toString() != "checked"
                && column.getId().toString() != "stt"
                && column.getId().toString() != "Request") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        this.autoSizeAll();
        this.cdr.detectChanges();
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.partCode,
            this.materialCode,
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
                setTimeout(() => {
                    this.isLoading = false;
                }, 5000);
            });
    }

    clearTextSearch() {
        this.productionDate = '',
        this.postingDateFrom = '',
        this.postingDateTo = '',
        this.partCode = '',
        this.materialCode = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.partCode,
            this.materialCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM02Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM02Dto();
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
            setTimeout(() => {
                this.isLoading = false;
            }, 5000);
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
                setTimeout(() => {
                    this.isLoading = false;
                }, 5000);
            });
    }


    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM02ToExcel(
            this._dateTimeService.convertToDatetime(this.postingDateFrom),
            this._dateTimeService.convertToDatetime(this.postingDateTo),
            this.partCode,
            this.materialCode,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.fn.exportLoading(e);
            });
    }
    reCreateData(e): void {
        if (this.postingDateFrom == null || this.postingDateFrom == '') {
            this.message.warn(this.l('Hãy nhập Posting Date From trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM02(
                this._dateTimeService.convertToDatetime(this.postingDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewValidate(){
        this.ViewFqf3mm02Validate.show(this.postingDateFrom,this.postingDateTo);
    }

    viewValidateData() {
        this.ViewFqf3mm02Validate.show(this.postingDateFrom,this.postingDateTo);
    }
}
