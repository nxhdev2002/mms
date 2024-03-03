import { Component, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, PaginationParamsModel, FrameworkComponent, GridParams } from '@app/shared/common/models/base.model';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdContainerListServiceProxy, MstCmmExchangeRateServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { ValueGetterParams } from '@ag-grid-community/core';

@Component({
    templateUrl: './view-diffexchangerate-modal.component.html',
    selector: 'view-diffexchangerate-modal'
})
export class ViewDiffExchangeRateComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewContainerIntransit', { static: true }) modal: ModalDirective | undefined;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    viewColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;
    dataParamsView: GridParams | undefined;
    pipe = new DatePipe('en-US');
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
    };

    constructor( injector: Injector,
        private _service: MstCmmExchangeRateServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService
    ) {
        super(injector);

        this.viewColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,pinned: 'left'},
            {headerName: this.l('Exchange Date'),headerTooltip: this.l('Exchange Date'),field: 'exchangeDate',width:100,
                valueGetter: (params: ValueGetterParams) => this.pipe.transform(params.data?.exchangeDate, 'dd/MM/yyyy') ,
            },
            {headerName: this.l('Major Currency'),headerTooltip: this.l('Major Currency'),field: 'majorCurrency',width:100,pinned: 'left',},
            { headerName: this.l('Minor Currency'),headerTooltip: this.l('Minor Currency'),field: 'minorCurrency',width:100,pinned: 'left',},
            { headerName: this.l('Ceiling Rate'),headerTooltip: this.l('Ceiling Rate'),field: 'ceilingRateNew',flex: 1},
            { headerName: this.l('Svb Rate'),headerTooltip: this.l('Svb Rate'),field: 'svbRateNew',flex: 1},
            { headerName: this.l('Floor Rate'),headerTooltip: this.l('Floor Rate'),field: 'floorRateNew',flex: 1},
            { headerName: this.l('Buying Od'),headerTooltip: this.l('Buying Od'),field: 'buyingOdNew',flex: 1},
            { headerName: this.l('Buying Tt'),headerTooltip: this.l('Buying Tt'),field: 'buyingTtNew',flex: 1},
            { headerName: this.l('Agv Rate'),headerTooltip: this.l('Agv Rate'),field: 'agvRateNew',flex: 1},
            { headerName: this.l('Selling Tt Od'),headerTooltip: this.l('Selling Tt Od'),field: 'sellingTtOdNew',flex: 1},
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void{}

    // autoSizeAll() {
    //     const allColumnIds: string[] = [];
    //     this.dataParamsView.columnApi!.getAllColumns()!.forEach((column) => {
    //       if (column.getId().toString() != "checked" 
    //       && column.getId().toString() != "stt"
    //       && column.getId().toString() != "group"){
    //         allColumnIds.push(column.getId());
    //       }
    //     });
    //     this.dataParamsView.columnApi!.autoSizeColumns(allColumnIds);
    // }

    // resetGridView(){
    //     console.log(this.dataParamsView)
    //     this.dataParamsView.columnApi!.sizeColumnsToFit({
        //     suppressColumnVirtualisation: true, 
        // });
    //     setTimeout(()=>{
    //         this.autoSizeAll();
    //     },1000)
    // }


    show(guid): void {
        console.log('TEST' + guid);
        this.isLoading =  true;
        this._service.getDataDiffExchangeRate(guid)
        .subscribe(result => {
            this.rowData = result.items;
         //   console.log(this.rowData);
            
           // this.resetGridView();
            this.isLoading = false;
        });
        this.modal.show();
    }

    close(): void {
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
