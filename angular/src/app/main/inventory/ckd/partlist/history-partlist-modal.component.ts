import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize, filter } from 'rxjs/operators';
import { InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { ceil } from 'lodash-es';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DatePipe } from '@angular/common';
import { GridOptions } from '@ag-grid-enterprise/all-modules';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'history-partlist-modal',
    templateUrl: './history-partlist-modal.component.html',
    styleUrls: ['./history-partlist-modal.component.less']
})
export class ViewHistoryPartListModalComponent extends AppComponentBase {
    @ViewChild('viewHistoryPartListModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    tableMapping = {
        'PL': 'InvCkdPartList',
        'PG': 'InvCkdPartGrade',
        'PD': 'InvCkdPartPackingDetails'
    };
    actionTypeList = [
        { value: 1, label: 'Xóa' },
        { value: 2, label: 'Tạo mới' },
        { value: 3, label: 'Trước Update' },
        { value: 4, label: 'Sau Update' },
    ];
    action: number[] = [1, 2, 3, 4];
    actionMapping = {
        1: 'Xoá',
        2: 'Tạo mới',
        3: 'Trước Update',
        4: 'Sau Update'
    };
    headerMapping = {
        // 'LastModifierFullName': 'Modifier',
    };

    hiddenHeader = [
        'UpdateMask',
        'Id'
    ];

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) { return null; }
            return r.toLowerCase();
        },
        tooltip: params => params.value,
    };

    recordId: number;
    tableName: string;
    gridParams: GridParams;
    dataParams: GridParams | undefined;
    selectedRow: InvCkdPartListDto;
    notify: any;
    isFilterMode: boolean;
    partNo: string;
    fromDate: any;
    isLoading = false;
    saving = false;
    frameworkComponents: FrameworkComponent;
    listData: any;
    rowdata: any[];
    selectedRowGrade: InvCkdPartGradeDto;
    rowdataGrade: InvCkdPartGradeDto[];
    pipe = new DatePipe('en-US');
    allRowHeaders = [];
    historyColDef: CustomColDef[] = [];
    groupDefaultExpanded = 1;
    rowSelection: String = 'multiple';

    toDate: Date = new Date();
    fn: CommonFunction = new CommonFunction();
    constructor(
        injector: Injector,
        private _service: InvCkdPartListServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _datetimeService: DateTimeService,
    ) {
        super(injector);
        this.historyColDef = [];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    show(id, type, histMode?): void {
        // this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.recordId = id;
        this.tableName = this.tableMapping[type];
        this.isFilterMode = histMode ? true : false;
        this.isLoading = true;
        this.allRowHeaders = [];
        this.getData();
    }

    getData() {
        this._service.getPartListHistory(
            this.recordId,
            this.tableName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.renderGrid(result);
                // this.paginationParams.totalCount = result.length;
                // this.paginationParams.totalPage = ceil(result.length / (this.paginationParams.pageSize ?? 0));
                this.modal.show();
                this.isLoading = false;
                this.resetGridView();
            });
    }

    renderGrid(result: string[]) {
        this.rowdata = [];
        result.forEach(item => {
            let obj = JSON.parse(item);

            this.rowdata.push(obj); // từ Json string parse sang Object JS
            // for từng property của object đó -> thêm vào object allRowHeaders -> mục đích để lấy được số header tối đa
            Object.keys(obj).forEach(key => {
                if (!this.allRowHeaders.includes(key)) { this.allRowHeaders.push(key); }
            });
        });
        this.paginationParams.totalCount = this.rowdata.length;
        this.paginationParams.totalPage = ceil(this.rowdata.length / (this.paginationParams.pageSize ?? 0));

        this.rowdata.sort((a, b) => (a.UpdatedDateTime < b.UpdatedDateTime) ? 1 : -1);
        this.historyColDef = [];
        this.allRowHeaders.forEach(header => {
            if (!this.hiddenHeader.includes(header)) {
                let headerText = this.headerMapping[header] ?? this.fn.splitCamelCase(header);
                let headerDefObj = {
                    headerName: this.l(headerText),
                    headerTooltip: this.l(headerText),
                    field: header,
                    flex: 1,
                    valueFormatter: (params) => this.cellFormatValue(params),
                };
                if (header === 'Action') {
                    headerDefObj['pinned'] = 'left';
                }
                if (header === 'UpdatedDateTime') {
                    headerDefObj['sort'] = 'asc';
                    headerDefObj['rowGroup'] = true;
                    headerDefObj['pinned'] = 'left';
                    headerDefObj['hide'] = true;
                }

                this.historyColDef.push(headerDefObj);
            }
        });

    }

    clearTextSearch() {
        this.partNo = '';
        this.action = [1, 2, 3, 4];
        this.fromDate = '';
        this.toDate = new Date();
        this.searchDatas();
    }

    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked']);
            this.gridParams.api.redrawRows();
        });
    }

    cellFormatValue(params) {
        // console.log(params)
        let result;
        switch (params.column.colId) {
            case 'Action':
                result = this.actionMapping[params.value];
                break;
            case 'UpdatedDateTime':
                result = this.pipe.transform(params.value, 'dd/MM/yyyy hh:mm:ss');
                break;
            case 'CreationTime':
                result = this.pipe.transform(params.value, 'dd/MM/yyyy hh:mm:ss');
                break;
            default:
                result = params.value;
        }
        return result;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getPartListHistory(
            this.recordId,
            this.tableName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.gridParams.columnApi!.getAllColumns()!.forEach((column) => {
            allColumnIds.push(column.getId());
        });
        this.gridParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        setTimeout(() => {
            // this.gridParams.columnApi!.sizeColumnsToFit({
            //     suppressColumnVirtualisation: true,
            // })
            this.autoSize();
        }, 500);
    }

    callBackDataGrid(params: GridParams) {
        this.gridParams = params;
        // params.api.paginationSetPageSize(this.paginationParams.pageSize)
        setTimeout(() => {
            this.resetGridView();
            this.selectAll();
        }, 500);
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    exportHistoricalData() {
        this.isLoading = true;
        if (!this.isFilterMode) {
            this._service.getHistoricalDataToExcel(
                this.recordId,
                this.tableName,
            ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.isLoading = false;
            });
        } else {
            this._service.getHistoricalDataFilterToExcel(
                this.action,
                this.partNo,
                this._datetimeService.convertToDatetime(this.fromDate),
                this._datetimeService.convertToDatetime(this.toDate)
            ).subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
                this.isLoading = false;
            });
        }
    }

    searchDatas() {
        this.isLoading = true;
        this._service.getPartListHistoryFilter(
            this.action,
            this.partNo,
            this._datetimeService.convertToDatetime(this.fromDate),
            this._datetimeService.convertToDatetime(this.toDate),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.renderGrid(result);
                // this.paginationParams.totalCount = result.length;
                // this.paginationParams.totalPage = ceil(result.length / (this.paginationParams.pageSize ?? 0));
                this.modal.show();
                this.isLoading = false;
                this.resetGridView();
            });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === 'Escape') {
            this.close();
        }
    }
}
