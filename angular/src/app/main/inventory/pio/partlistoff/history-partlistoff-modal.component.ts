import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { AppComponentBase } from '@shared/common/app-component-base'
import { finalize } from 'rxjs/operators'
import { InvCkdPartListDto, InvCkdPartListServiceProxy, InvPartListOffServiceProxy, InvPioPartListInlDto, InvPioPartListInlServiceProxy} from '@shared/service-proxies/service-proxies'
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker'
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model'
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component'
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component'
import { ceil } from 'lodash-es'
import { FileDownloadService } from '@shared/utils/file-download.service'
import { DatePipe } from '@angular/common'
import { GridOptions } from '@ag-grid-enterprise/all-modules'



@Component({
    selector: 'history-partlistoff-modal',
    templateUrl: './history-partlistoff-modal.component.html'
})
export class ViewHistoryPartListOffModalComponent extends AppComponentBase {
    @ViewChild('viewHistoryPartListOffModal', { static: true }) modal: ModalDirective | undefined
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
    actionMapping = {
        '1': 'DELETE',
        '2': 'INSERT',
        '3': 'BEFORE UPDATE',
        '4': 'AFTER UPDATE'
    }
    rowdata: InvPioPartListInlDto[];
    pipe = new DatePipe('en-US');
    allRowHeaders = [];
    historyColDef: CustomColDef[] = []
    frameworkComponents: FrameworkComponent
    dataParams: GridParams | undefined
    isLoading: boolean = false
    saving: boolean = false
    rowSelection = 'multiple'
    id;
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


    gridParams: GridParams
    gridTableService: any
    notify: any
    constructor(
        injector: Injector,
        private _service: InvPartListOffServiceProxy,
        private _fileDownloadService: FileDownloadService,

    ) {
        super(injector)
        this.historyColDef = []
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        }
    }

    show(id,type): void {
        this.isLoading = true
        this.allRowHeaders = [];
        if(type == 'PL'){  this.getData(id,'InvPioPartListOff')}
        else if(type == 'PG'){ this.getData(id,'InvPioPartGradeOff')}       
    }

    getData(_id,_tableName){
        this._service.getPartListOffHistory(
            _id,
            _tableName,
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


        this.historyColDef = []
        this.allRowHeaders.forEach(header => {
            let headerDefObj =  {
                headerName: this.l(header),
                headerTooltip: this.l(header),
                field: header,
                flex: 1,
                valueFormatter: (params) => this.cellFormatValue(params),
            };
            if (header == 'UpdatedDateTime') {
                headerDefObj['sort'] = 'asc';
                headerDefObj['rowGroup'] = true;
                headerDefObj['pinned'] = 'left';
            }

            this.historyColDef.push(headerDefObj);
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
        let result;
        switch (params.column.colId) {
            case 'ActionId':
                result = this.actionMapping[params.value];
                break;
            case 'UpdatedDateTime':
                result = this.pipe.transform(params.value, 'dd/MM/yyyy hh:mm:ss')
                break;
            default:
                result = params.value;
        }
        return result;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
		return this._service.getPartListOffHistory(
            this.id,
            'InvPioPartGradeOff',
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

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close()
        }
    }
}
