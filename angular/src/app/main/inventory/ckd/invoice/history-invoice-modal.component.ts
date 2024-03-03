import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { AppComponentBase } from '@shared/common/app-component-base'
import { finalize } from 'rxjs/operators'
import { InvCkdInvoiceDto, InvCkdInvoiceServiceProxy, InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies'
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker'
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model'
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component'
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component'
import { ceil } from 'lodash-es'
import { FileDownloadService } from '@shared/utils/file-download.service'
import { DatePipe } from '@angular/common'
import { GridOptions } from '@ag-grid-enterprise/all-modules'
import { CommonFunction } from '@app/main/commonfuncton.component'
import { DataFormatService } from '@app/shared/common/services/data-format.service'



@Component({
    selector: 'history-invoice-modal',
    templateUrl: './history-invoice-modal.component.html'
})
export class ViewHistoryInvoiceModalComponent extends AppComponentBase {
    @ViewChild('viewHistoryInvoiceModal', { static: true }) modal: ModalDirective | undefined
    @ViewChild('nameInput') nameInput: ElementRef
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>()
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>()
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    }
    listData: any;
    selectedRow: InvCkdInvoiceDto;
    rowdata: any[];
    rowdataGrade: InvCkdPartGradeDto[];
    pipe = new DatePipe('en-US');
    allRowHeaders = [];
    historyColDef: CustomColDef[] = []
    groupDefaultExpanded = 1;
    frameworkComponents: FrameworkComponent
    dataParams: GridParams | undefined
    isLoading: boolean = false
    saving: boolean = false
    rowSelection = 'multiple'
    statustype: string;

    defaultColDef = {
        resizable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null
            return r.toLowerCase()
        },
        tooltip: (params) => params.value,
    }

    recordId: number;
    tableName: string;

    actionMapping = {
        '1': 'Xoá',
        '2': 'Tạo mới',
        '3': 'Trước Update',
        '4': 'Sau Update'
    }

    headerMapping = {
        // 'LastModifierFullName': 'Modifier',
    }

    hiddenHeader = [
        'UpdateMask',
        'Id'
    ]

    fn: CommonFunction = new CommonFunction();
    gridParams: GridParams
    gridTableService: any
    notify: any
    constructor(
        injector: Injector,
        private _service: InvCkdInvoiceServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
    ) {
        super(injector)
        this.historyColDef = []
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        }
    }

    show(id, type): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 }
        this.recordId = id;
        if (type == 'Inv'){
            this.tableName = 'InvCkdInvoice';
            this.statustype = type;
        }else if(type == 'InvDe'){
            this.tableName = 'InvCkdInvoiceDetails';
            this.statustype = type;
        }
        
        this.isLoading = true
        this.allRowHeaders = [];
        this.getData()
    }

    getData() {
        this._service.getInvoiceHistory(
            this.recordId,
            this.tableName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this.renderGrid(result);
                this.paginationParams.totalCount = result.length
                this.paginationParams.totalPage = ceil(result.length / (this.paginationParams.pageSize ?? 0))
                this.modal.show();
                this.isLoading = false;
                this.resetGridView();
            })
    }

    renderGrid(result: string[]) {
        this.rowdata = [];
        result.forEach(item => {
            let obj = JSON.parse(item);
            this.rowdata.push(obj) // từ Json string parse sang Object JS
            // for từng property của object đó -> thêm vào object allRowHeaders -> mục đích để lấy được số header tối đa
            Object.keys(obj).forEach(key => {
                if (!this.allRowHeaders.includes(key)) this.allRowHeaders.push(key)
            })
        });

        this.rowdata.sort((a, b) => (a.UpdatedDateTime < b.UpdatedDateTime) ? 1 : -1)
        this.historyColDef = []
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
                if (header == 'Action') {
                    headerDefObj['pinned'] = 'left';
                }
                if (header == 'UpdatedDateTime') {
                    headerDefObj['sort'] = 'asc';
                    headerDefObj['rowGroup'] = true;
                    headerDefObj['pinned'] = 'left';
                    headerDefObj['hide'] = true;
                }

                this.historyColDef.push(headerDefObj);
            }
        })

    }

    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true)
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked'])
            this.gridParams.api.redrawRows()
        })
    }

    

    cellFormatValue(params) {
        // console.log(params)
        let moneycol;
        let datecol;
        let result;
        const date = [
            'CreationTime',
            'UpdatedDateTime'
        ]
        const money = [
            'Cif',
        ]
        if(money.includes(params.column.colId)){
            moneycol = params.column.colId
        }else if (date.includes(params.column.colId)){
            datecol = params.column.colId
        }
        switch (params.column.colId) {
            case 'Action':
                result = this.actionMapping[params.value];
                break;
            case datecol:
                result = this.pipe.transform(params.value, 'dd/MM/yyyy hh:mm:ss')
                break;
            case moneycol:
                result = this._fm.formatMoney_decimal(params.value, 4)
                break;
            default:
                result = params.value;
        }
        return result;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvoiceHistory(
            this.recordId,
            this.tableName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
    }

    autoSize() {
        const allColumnIds: string[] = []
        this.gridParams.columnApi!.getAllColumns()!.forEach((column) => {
            allColumnIds.push(column.getId())
        })
        this.gridParams.columnApi!.autoSizeColumns(allColumnIds)
    }

    resetGridView() {
        setTimeout(() => {
            // this.gridParams.columnApi!.sizeColumnsToFit({
            //     suppressColumnVirtualisation: true,
            // })
            this.autoSize()
        }, 500)
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {

            result.forEach(item => {
                let obj = JSON.parse(item);
                this.rowdata.push(obj)
            });
            let data = result
            // let data = this.rowdata.sort((a, b) => new Date(a.UpdatedDateTime).getTime() - new Date(b.UpdatedDateTime).getTime());
            // let str = data.map(item => JSON.stringify(item))
            let start = (this.paginationParams.pageNum - 1) * this.paginationParams.pageSize;
            let end = this.paginationParams.pageNum * this.paginationParams.pageSize;
            let pagedData = data.slice(start, end);
            this.renderGrid(pagedData)

            this.paginationParams.totalCount = result.length;
            this.paginationParams.totalPage = ceil(result.length / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView()
        });

    }

    callBackDataGrid(params: GridParams) {
        this.gridParams = params
        // params.api.paginationSetPageSize(this.paginationParams.pageSize)
        setTimeout(() => {
            this.resetGridView();
            this.selectAll()
        }, 500)
    }

    close(): void {
        this.modal?.hide()
        this.modalClose.emit(null)
    }

    exportHistoricalData() {
        this.isLoading = true;
        this._service.getHistoricalDataToExcel(
            this.recordId,
            this.tableName,
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.notify.success(this.l('Download Excel Successfully'));
            this.isLoading = false;
        })

    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close()
        }
    }
}
