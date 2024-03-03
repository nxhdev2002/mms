import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdDto, InvCkdSmqdServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
// import { ImportInvCkdSmqdComponent } from './import-smqdmanagement.component';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { ImportInvCkdSmqdPxpInOutComponent } from './import-smqd-pxpinout.component';

@Component({
    templateUrl: './smqdmanagement.component.html',
    styleUrls: ['./smqdmanagement.component.less'],
})
export class SmqdComponent extends AppComponentBase implements OnInit {
   // @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportInvCkdSmqdComponent | undefined;
    @ViewChild('importPxPInOutExcelModal', { static: true }) importPxPInOutExcelModal: | ImportInvCkdSmqdPxpInOutComponent | undefined;
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

    selectedRow: InvCkdSmqdDto = new InvCkdSmqdDto();
    saveSelectedRow: InvCkdSmqdDto = new InvCkdSmqdDto();
    datas: InvCkdSmqdDto = new InvCkdSmqdDto();
    isLoading: boolean = false;
    fn: CommonFunction = new CommonFunction();
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    cfc: string = '';
    lotNo: string = '';
    supplierNo: string = '';
    partNo: string = '';
    smqdDateFrom: any;
    smqdDateTo: any;
    radioValue: string = '2';
    radioMapping = {
        '1': 'SMQD',
        '2': 'PxP In',
        '3': 'PxP Out',
        '4': 'PxP Return',
        '5': 'In Other',
        '6': 'Out Other',
    }
    checkbutuon = true;
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
        private _service: InvCkdSmqdServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private _formStoringService: FormStoringService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Run No'), headerTooltip: this.l('Run No'), field: 'runNo', flex: 1 },
            { headerName: this.l('Cfc'), headerTooltip: this.l('Cfc'), field: 'cfc', flex: 1 },
            { headerName: this.l('Lot No'), headerTooltip: this.l('Lot No'), field: 'lotNo', flex: 1 },
            { headerName: this.l('Check Model'), headerTooltip: this.l('Check Model'), field: 'checkModel', flex: 1 },
            { headerName: this.l('Supplier No'), headerTooltip: this.l('Supplier No'), field: 'supplierNo', flex: 1 },
            { headerName: this.l('Part No'), headerTooltip: this.l('Part No'), field: 'partNo', flex: 1 },
            { headerName: this.l('Part Name'), headerTooltip: this.l('Part Name'), field: 'partName', flex: 1 },
            { headerName: this.l('Order No'), headerTooltip: this.l('Order Status'), field: 'orderStatus', flex: 1 },
            { headerName: this.l('Qty'), headerTooltip: this.l('Qty'), field: 'qty', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Invoice'), headerTooltip: this.l('Invoice'), field: 'invoice', flex: 1 },
            {
                headerName: this.l('Received Date'), headerTooltip: this.l('Received Date'), field: 'receivedDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.returnDate, 'dd/MM/yyyy'),
                comparator: this._formStoringService.dateComparator
            },
            { headerName: this.l('Effect Qty'), headerTooltip: this.l('Effect Qty'), field: 'effectQty', flex: 1, type: 'rightAligned' },
            { headerName: this.l('Reason Code'), headerTooltip: this.l('Reason Code'), field: 'reasonCode', flex: 1 },

            { headerName: this.l('Order Status'), headerTooltip: this.l('Order Status'), field: 'orderStatus', flex: 1 },
            { headerName: this.l('Return Qty'), headerTooltip: this.l('Return Qty'), field: 'returnQty', flex: 1, type: 'rightAligned' },
            {
                headerName: this.l('Return Date'), headerTooltip: this.l('Return Date'), field: 'returnDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.returnDate, 'dd/MM/yyyy'),
                comparator: this._formStoringService.dateComparator
            },

            {
                headerName: this.l('Smqd Date'), headerTooltip: this.l('Smqd Date'), field: 'smqdDate', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.smqdDate, 'dd/MM/yyyy'),
                comparator: this._formStoringService.dateComparator
            },
            { headerName: this.l('Remark'), headerTooltip: this.l('Remark'), field: 'remark', flex: 1 },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

        let _btn = document.querySelector('.actionButton_w' + 1);
        if (_btn) _btn.classList.add('active');
        this.checkbutuon = (this.radioValue != '') ? false : true;
    }

    setvalradio(i: string) {
        var tabindex = Number(i) - 1;
        let _btnUncheck = document.querySelector('.actionButton_w' + tabindex + '.active');
        // if (_btnUncheck) {
        //     let objbtn = document.querySelectorAll('.groupBtn');
        //     for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

        //     this.radioValue = '';
        // }
        if(!_btnUncheck) {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w' + tabindex);
            if (_btn) _btn.classList.add('active');
            this.radioValue = i;
        }
        this.checkbutuon = (this.radioValue != '') ? false : true;
        this.searchDatas();

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

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.lotNo,
            this.radioValue,
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
        this.cfc = '',
            this.lotNo = '',
            this.supplierNo = '',
            this.partNo = '',
            this.smqdDateFrom = '',
            this.smqdDateTo = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.lotNo,
            this.radioValue,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCkdSmqdDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCkdSmqdDto();
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

    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvCkdSmqdToExcel(
            this._dateTimeService.convertToDatetime(this.smqdDateFrom),
            this._dateTimeService.convertToDatetime(this.smqdDateTo),
            this.partNo,
            this.cfc,
            this.supplierNo,
            this.lotNo,
            this.radioValue
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }

    showImportExcel() {
        // if (this.radioValue == "1") {
        //     this.importExcelModal.show();
        // } else {
        //     this.importPxPInOutExcelModal.show(this.radioMapping[this.radioValue]);
        // }
    }

    prodSumaryOption() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
        else optionSummary.classList.add("active");
    }

    lostForcus() {
        var optionSummary = document.querySelector('.btn.sumary-prod') as HTMLElement; // .exel-dropdown
        if (optionSummary.classList.contains('active')) optionSummary.classList.remove("active");
    }

}
