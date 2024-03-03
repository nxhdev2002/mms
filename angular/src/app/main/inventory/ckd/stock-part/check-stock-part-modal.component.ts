import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstFrmProcessDto, InvCkdStockPartServiceProxy, MstFrmProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import ceil from 'lodash-es/ceil';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
    selector: 'check-stock-part-modal',
    templateUrl: './check-stock-part-modal.component.html',
    })
export class CheckStockPartModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalProcess', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstFrmProcessDto = new CreateOrEditMstFrmProcessDto();
    isLoading: boolean = false;
    active: boolean = false;
    saving: boolean = false;
    byLotColDefs: CustomColDef[] = [];
    pending: string = '';
    disable: boolean = false;
    paginationParam: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    dataParams: GridParams | undefined;
    frameworkComponents: FrameworkComponent;
    rowData: any[] = [];
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
        cellStyle: function (params: any) {
            if (params.value == 'Shift No : 2') return {
                'background-color': 'yellow',
                'border-bottom': '1px Solid #c0c0c0',
                'border-right': '1px Solid #c0c0c0',
                'overflow': 'hidden',
                'border-top-width': '0',
                'border-left-width': '0',
                'padding': '3px 6px 4px',
                'position': 'absolute',
                'z-index': '-1',
            };
            // if (params.data?.shiftName == 'SHIFT 2') return {
            //     'background-color': 'yellow',
            //     'border-bottom': '1px Solid #c0c0c0',
            //     'border-right': '1px Solid #c0c0c0',
            //     'overflow': 'hidden',
            //     'border-top-width': '0',
            //     'border-left-width': '0',
            //     'padding': '3px 6px 4px',
            //     'position': 'absolute',
            //     'z-index': '-1',
            // };
            return { 'color': '' };
        },
    };
    constructor(
        injector: Injector,
        private _service: InvCkdStockPartServiceProxy,
        private _fileDownloadService: FileDownloadService,
    )
    {
        super(injector);
        this.byLotColDefs = [
            {
                headerName: this.l('STT'),headerTooltip: this.l('STT'),
                cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParam.pageSize * (this.paginationParam.pageNum - 1),
                cellClass: ['text-center'],
                width: 75
            },
            {
                headerName: this.l('Error Description'),
                headerTooltip: this.l('Error Description'),
                field: 'errDes',
                flex: 1
            },
            {
                headerName: this.l('PartNo'),
                headerTooltip: this.l('PartNo'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                flex: 1
            },
            


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        
    }



    show(): void {
        this.isLoading = true;
        this.paginationParam = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this._service.getCheckStockPart(
            '',
            this.paginationParam.skipCount,
            this.paginationParam.pageSize
        ).subscribe( (result) => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize));
            this.isLoading = true;
        })
        this.active = true;
        this.modal.show();
    }

   

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        this._service.getCheckStockPartToExcel()
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this._service.getCheckStockPart(
            '',
            this.paginationParam.skipCount,
            this.paginationParam.pageSize
        ).subscribe( (result) => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize));
            this.isLoading = true;
        })
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === "Escape") {
                this.close();
            }
        }
}
