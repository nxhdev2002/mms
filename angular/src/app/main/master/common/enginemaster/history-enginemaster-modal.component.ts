import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core'
import { ModalDirective } from 'ngx-bootstrap/modal'
import { AppComponentBase } from '@shared/common/app-component-base'
import { finalize } from 'rxjs/operators'
import { InvGpsMaterialServiceProxy, MstCmmEngineMasterServiceProxy} from '@shared/service-proxies/service-proxies'
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker'
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model'
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component'
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component'
import { ceil, forEach } from 'lodash-es'
import { FileDownloadService } from '@shared/utils/file-download.service'
import { DatePipe } from '@angular/common'
import { GridOptions } from '@ag-grid-enterprise/all-modules'
import { DataFormatService } from '@app/shared/common/services/data-format.service'
import { CommonFunction } from '@app/main/commonfuncton.component'



@Component({
    selector: 'history-enginemaster-modal',
    templateUrl: './history-enginemaster-modal.component.html'
})
export class ViewHistoryEngineMasterModalComponent extends AppComponentBase {
    @ViewChild('viewHistoryEngineMatersModal', { static: true }) modal: ModalDirective | undefined
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
        'Id',
        'LastModificationTime'
    ]
    fn: CommonFunction = new CommonFunction();
    rowdata: any[];
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
        private _service: MstCmmEngineMasterServiceProxy,
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

    show(id): void {
        console.log(id);
        this.id =id;
        this.isLoading = true
        this.allRowHeaders = [];
        this.getData(id,'MstCmmEngineMaster')
    }

    getData(_id,_tableName){
        this._service.getMstCmmEngineMasterHistory(
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

        console.log("Result = ", result);
        console.log("Json = ", JSON);
        console.log("Rowdata = ", this.rowdata);
        console.log("AllRowHeaders = ", this.allRowHeaders);
    }
    // renderGrid(result: string[]) {
    //     this.rowdata = [];
    //     result.forEach(item => {
    //         let obj = JSON.parse(item);
    //         this.rowdata.push(obj) // từ Json string parse sang Object JS
    //         // for từng property của object đó -> thêm vào object allRowHeaders -> mục đích để lấy được số header tối đa
    //         Object.keys(obj).forEach(key => {
    //             if (!this.allRowHeaders.includes(key)) this.allRowHeaders.push(key)
    //         })
    //     });


    //     this.historyColDef = []
    //     this.allRowHeaders.forEach(header => {
    //         let headerDefObj =  {
    //             headerName: this.l(header),
    //             headerTooltip: this.l(header),
    //             field: header,
    //             flex: 1,
    //             valueFormatter: (params) => this.cellFormatValue(params),
    //         };
    //         if (header == 'UpdatedDateTime') {
    //             headerDefObj['sort'] = 'asc';
    //             headerDefObj['rowGroup'] = true;
    //             headerDefObj['pinned'] = 'left';
    //         }

    //         this.historyColDef.push(headerDefObj);
    //     })

    // }

    selectAll() {
            this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true)
            this.gridParams.api.setFocusedCell(e.rowIndex,
            this.gridParams.api.getColumnDefs()[0]['checked'])
            this.gridParams.api.redrawRows()
        })
    }
    SumA(values) {
        var sum = 0;
        values.forEach(function (value) { sum += Number(value); });
        return sum;
    }
    cellFormatValue(params) {
        console.log("Param=",params)
        let result;
        const dateTime = [
            'CreationTime',
            'UpdatedDateTime'
        ];

        // const date =[
        //     'BillDate',
        //     'CdDate',
        //     'ShippingDate',
        //     'PortDate',
        //     'ReceiveDate',
        //     'PortDateActual',
        //     'PortTransitDate',
        //     'DevanningDate',
        //     'GateinDate',
        //     'TransitPortReqDate',
        //     'LocationDate'
        // ]
        const money = [
            // 'Fob',
            // 'Freight',
            // 'Insurance',
            // 'Cif',
            // 'Tax',
            // 'Amount',
            'Price',
            'ConvertPrice'
        ]

        if(dateTime.includes(params.column.colId)){
            result = this.pipe.transform(params.value, 'dd/MM/yyyy hh:mm:ss');
        }
        // else if(date.includes(params.column.colId)){
        //     result = this.pipe.transform(params.value, 'dd/MM/yyyy')
        //     console.log(params.value)

        // }
        else if(money.includes(params.column.colId)){
            result = this._fm.formatMoney_decimal(params.value, 2)
            console.log(params.value)
        }

        switch (params.column.colId) {
            case 'Action':
                result = this.actionMapping[params.value];
                break;
            }
            return result;
        }

    

    getDatas(paginationParams?: PaginationParamsModel) {
		return this._service.getMstCmmEngineMasterHistory(
            this.id,
            'InvGpsMaterial',
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
    exportHistoricalData() {
        this.isLoading = true;
        this._service.getHistoricalDataToExcel(
            this.id,
            'MstCmmEngineMaster',
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
