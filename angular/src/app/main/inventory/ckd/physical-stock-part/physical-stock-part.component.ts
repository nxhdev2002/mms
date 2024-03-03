import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditInvCkdPhysicalStockPartDto, InvCkdPhysicalStockPartDto, InvCkdPhysicalStockPartServiceProxy, InvCkdPhysicalStockPeriodServiceProxy, InvStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPhysicalStockPartModalComponent } from './create-or-edit-physical-stock-part-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ImportInvCkdPhysicalStockPartComponent } from './import-physical-stock-part.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ViewLotDetailsComponent } from './view-lot-details-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { error } from 'console';

@Component({
    templateUrl: './physical-stock-part.component.html',
})
export class PhysicalStockPartComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewLotDetails', { static: true }) viewLotDetails: ViewLotDetailsComponent;
    @ViewChild('createOrEditModalPhysicalStockPart', { static: true }) createOrEditModalPhysicalStockPart: | CreateOrEditPhysicalStockPartModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportInvCkdPhysicalStockPartComponent | undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvCkdPhysicalStockPartDto = new InvCkdPhysicalStockPartDto();
    saveSelectedRow: InvCkdPhysicalStockPartDto = new InvCkdPhysicalStockPartDto();
    datas: InvCkdPhysicalStockPartDto = new InvCkdPhysicalStockPartDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    disableexport: boolean = false;
    disable1: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partNoNormalized: string = '';
    partName: string = '';
    partNoNormalizedS4: string = '';
    colorSfx: string = '';
    lotNo: string = '';
    partListId
    materialId
    receiveQty
    actualQty
    beginQty
    issueQty
    transtype
    calculatorQty
    lastCalDatetime
    workingDate
    remark
    periodId: number = 0;
    isActive: string = '';
    negativeStock: boolean = false;
    supplierNo;
    p_mode = 0;
    Period: any;
    modelist = [
        {value: '0' , label: "Mix"},
        {value: '1' , label: "Lot"},
        {value: '2' , label: "Pxp"},
    ];
    cfc;
    cbbPeriod: any[] = [];
    id: any;
    loadColumdef: CustomColDef[] = [];
    periodIdList: any[] = [];

    _lotno: string = '';

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
        private _service: InvCkdPhysicalStockPartServiceProxy,
        private _serviceStock: InvCkdPhysicalStockPeriodServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
    ) {
        super(injector);
        this.loadColumdef = [
            {
                headerName: 'Description',
                headerTooltip: 'Description',
                field: 'description',
                cellClass: ['text-center'],
                width:100
            },
            {
                headerName: 'FromDate',
                headerTooltip: 'FromDate',
                field: 'fromDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.fromDate, 'dd/MM/yyyy'),
                flex:1
            },
            {
                headerName: 'ToDate',
                headerTooltip: 'ToDate',
                field: 'toDate',
                cellClass: ['text-center'],
                valueGetter: (params) => this.pipe.transform(params.data?.toDate, 'dd/MM/yyyy'),
                flex:1
            },

        ];
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo',width:150 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc',width:150 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo',width:150 },
            { headerName: this.l('Color Sfx'), headerTooltip: this.l('Color Sfx'), field: 'colorSfx',width:150 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName',width:150 },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo',width:150 },
            { headerName: this.l('Begin Qty'), headerTooltip: this.l('BeginQty'), field: 'beginQty', type: 'rightAligned' ,valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.beginQty), aggFunc: this.SumA,width:150},
            { headerName: this.l('Receive Qty'), headerTooltip: this.l('ReceiveQty'), field: 'receiveQty', type: 'rightAligned' ,valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.receiveQty),  aggFunc: this.SumA,width:150},
            { headerName: this.l('Issue Qty'), headerTooltip: this.l('IssueQty'), field: 'issueQty', type: 'rightAligned',valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.issueQty),  aggFunc: this.SumA,width:150},
            { headerName: this.l('Closing'), headerTooltip: this.l('Closing'), field: 'calculatorQty', type: 'rightAligned' ,valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.calculatorQty),  aggFunc: this.SumA,width:150},
            { headerName: this.l('Actual Qty'), headerTooltip: this.l('ActualQty'), field: 'actualQty', type: 'rightAligned' ,valueGetter: (params) => this._fm.formatMoney_decimal0(params.data?.actualQty),  aggFunc: this.SumA,width:150},
            { headerName: this.l('Diff'), headerTooltip: this.l('Diff'), field: 'diff', type: 'rightAligned' , width:150},
            { headerName: this.l('Last Cal Datetime'), headerTooltip: this.l('Last Cal Datetime'), field: 'lastCalDatetime',
             valueGetter: (params) => this.pipe.transform(params.data?.lastCalDatetime, 'dd/MM/yyyy HH:mm:ss'),width:150},
            { headerName: this.l('Material Id'), headerTooltip: this.l('Material Id'), field: 'materialId', type: 'rightAligned',width:100 },
            { headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive',width:100, valueGetter: (params) => params.data.isActive == 'Y' ? 'Active' : (params.data.isActive == 'N' ? 'InActive' : '') },

        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._serviceStock.getIdInvPhysicalStockPeriod()
            .subscribe((result) => {
                this.periodIdList = result.items;
                this.Period = (result.items.filter(s => s.status = 1))[0].infoPeriod;
            });

    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            allColumnIds.push(column.getId());
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
                this.autoSize();
        }, 1000);

    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => {
            this.gridTableService.selectFirstRow(this.dataParams!.api);
            var rows = this.createRow(1);
            this.dataParams!.api.setPinnedBottomRowData(rows); //add row total vào cuối grid
        }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.partNo = "",
            this.cfc = "",
            this.supplierNo = "",
            this.colorSfx = "",
            this.id = '',
            this.lotNo = '',
            this.p_mode = 0,
        this.searchDatas();
    }

    SumA(values) {
        var sum = 0;
        if (values) {
            values.forEach(function (value) { sum += parseFloat(value); });
        }
        return sum;
    }

     // _CountWin:number = 0;
     createRow(count: number): any[] {
        let result: any[] = [];

        if (this.rowData.length > 0) {
            result.push({
                cfc: 'Grand Total',
                beginQty: this.rowData[0].totalBeginQty,
                receiveQty: this.rowData[0].totalReceiveQty,
                issueQty: this.rowData[0].totalIssueQty,
                calculatorQty: this.rowData[0].totalCalculatorQty,
                actualQty: this.rowData[0].totalActualQty,
            });
        }
        else {
            result.push({
                cfc: 'Grand Total',
                beginQty: 0,
                receiveQty: 0,
                issueQty: 0,
                calculatorQty: 0,
                actualQty: 0,
            });
        }
        return result;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdPhysicalStockPartDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdPhysicalStockPartDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);

        this._lotno = this.saveSelectedRow.lotNo;
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
            this.gridTableService.selectFirstRow(this.dataParams!.api)
            this.isLoading = false;
            // this.resetGridView();
        });
    }

    callBackDataGrid(params: GridParams) {
        // this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
        .pipe(finalize(() => {
            var rows = this.createRow(1);
            this.dataParams!.api.setPinnedBottomRowData(rows);
            this.gridTableService.selectFirstRow(this.dataParams!.api);
        }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.isLoading = false;
                // this.resetGridView();
            });
    }

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPhysicalStockPartToExcel(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
    calculatorStockPart(e) {
        this.message.confirm(this.l('Calculate có thể diễn ra trong vài phút và sẽ xóa dữ liệu cũ, bạn có xác nhận Calculate?'), 'Confirm Calculate', (isConfirmed) => {
            if (isConfirmed) {
                var startTime = new Date().toISOString();
                this.fn.exportLoading(e, true);
                this._service.calculatorStockPart().subscribe(() => {
                    var endTime = new Date().toISOString(); ;
                    console.log('Start time:', startTime);
                    console.log('End time:', endTime);
                    this.fn.exportLoading(e);
                    this.notify.info(this.l('Calculator Successfully'));
                    this._service.sendMailCalCulate(startTime,endTime).subscribe(()=>{
                        this.message.success('Send mail success')
                    },(error)=>{
                        this.message.error('Send mail error',error)
                    })
                })
            }
        });
    }

    viewLotDetail() {
        if(this._lotno != null)
        {
            if(this._lotno.length == 6) this.viewLotDetails.show(this._lotno.replace('-','0'));
            if(this._lotno.length == 5) this.viewLotDetails.show(this._lotno.replace('-','00'));
            if(this._lotno.length == 4) this.viewLotDetails.show(this._lotno.replace('-','000'));
        }
    }

    exportToExcelLotDetails(e){
        this.fn.exportLoading(e, true);
        this._service.getPhysicalStockPartDetailsToExcel(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportToExcelGroup(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPhysicalStockPartToExcelGroup(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    exportSummaryStockByPart(e): void {
        this.fn.exportLoading(e, true);
        this._service.getPhysicalSummaryStockByPart(
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.colorSfx,
            this.id,
            this.lotNo,
            this.p_mode
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }
}
