import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, GetPartListGradeDto, InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy, InvIhpStockPartServiceProxy, InvPioPartListInlServiceProxy, MstLgaBp2PartListServiceProxy, ValidatePartListDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { ceil } from 'lodash-es';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DatePipe } from '@angular/common';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'view-if-ihpstockpart-modal',
    templateUrl: './view-if-ihpstockpart-modal.component.html'
})
export class ViewIfIhpStockPartModalComponent extends AppComponentBase {
    @ViewChild('viewValidatePartListModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
	paginationParams: PaginationParamsModel = {
		pageNum: 1,
		pageSize: 500,
		totalCount: 0,
		skipCount: 0,
		sorting: '',
		totalPage: 1,
	};
    rowdata: any[] = [];
    validateColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
    isLoading: boolean = false;
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    rowSelection = 'multiple';
    pipe = new DatePipe('en-US');
    defaultColDef = {
        resizable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };
    WorkingDateFrom;
    WorkingDateTo;
    gridParams: GridParams;
    gridTableService: any;
    constructor(
        injector: Injector,
        private _service: InvIhpStockPartServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
   
    ) {
        super(injector);
        this.validateColDef = [
            {   headerName: this.l('STT'), 
                headerTooltip: this.l('STT'), 
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), 
                cellClass: ['text-center'], 
                width: 55, 
                pinned: 'left'
            },
            // {
            //     headerName: this.l('DrmMaterialId'),
            //     headerTooltip: this.l('DrmMaterialId'),
            //     field: 'drmMaterialId',
                
            // },
            {
                headerName: this.l('PartCode'),
                headerTooltip: this.l('PartCode'),
                field: 'partCode',
                
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                
            },
            {
                headerName: this.l('Press'),
                headerTooltip: this.l('Press'),
                field: 'press',
                
            },
            {
                headerName: this.l('Working Date'),
                headerTooltip: this.l('Working Date'),
                field: 'workingDate',

                valueGetter: (params) => this.pipe.transform(params.data?.workingDate, 'dd/MM/YYYY'),
                

            },
            {
                headerName: this.l('Supplier Type'),
                headerTooltip: this.l('Supplier Type'),
                field: 'supplierType',
                
            },
            {
                headerName: this.l('Supplier Cd'),
                headerTooltip: this.l('Supplier Cd'),
                field: 'supplierCd',
                
            },
          
            {
                headerName: this.l('Cfc'),
                 headerTooltip: this.l('Cfc'),
                field: 'cfc',

            },
            {
                headerName: this.l('Material Code'),
                 headerTooltip: this.l('Material Code'),
                field: 'materialCode',

            },
            {
                headerName: this.l('MaterialSpec'),
                 headerTooltip: this.l('MaterialSpec'),
                field: 'materialSpec',

            },
            {
                headerName: this.l('Fin Material Number'),
                 headerTooltip: this.l('Fin Material Number'),
                field: 'finMaterialNumber',

            },
            {
                headerName: this.l('Fin Material Code'),
                headerTooltip: this.l('Fin Material Code'),
                field: 'finMaterialCode',
                
            },
            {
                headerName: this.l('Fin Material Fin Spec'),
                headerTooltip: this.l('Fin Material Fin Spec'),
                field: 'finMaterialFinSpec',
                
            },
            {
                headerName: this.l('Fin Part Size'),
                headerTooltip: this.l('Fin Part Size'),
                field: 'FinPartSize',
                editable: true,
            },
            {
                headerName: this.l('Fin Part Price'),
                headerTooltip: this.l('Fin Part Price'),
                field: 'finPartPrice',
                
                editable: true,
            },
            {
                headerName: this.l('Part Spec'),
                headerTooltip: this.l('Part Spec'),
                field: 'partSpec',
                
            },
            {
                headerName: this.l('Size Code'),
                
                headerTooltip: this.l('Size Code'),
                field: 'sizeCode',
                editable: true
            },
            {
                headerName: this.l('Part Size'),
                
                headerTooltip: this.l('Part Size'),
                field: 'partSize',
                editable: true
            },
            {
                headerName: this.l('Box Qty'),
                
                headerTooltip: this.l('Box Qty'),
                field: 'boxQty',
                editable: true
            },
            {
                headerName: this.l('First Day Product'),
                
                headerTooltip: this.l('First Day Product'),
                field: 'firstDayProduct',
                valueGetter: (params) => this.pipe.transform(params.data?.firstDayProduct, 'dd/MM/YYYY'),
                editable: true
            },


            {
                headerName: this.l('Last Day Product'),
                headerTooltip: this.l('Last Day Product'),
                valueGetter: (params) => this.pipe.transform(params.data?.firstDayProduct, 'dd/MM/YYYY'),
                field: 'lastDayProduct',
                
            },
            {
                headerName: this.l('Sourcing'),
                
                headerTooltip: this.l('Sourcing'),
                field: 'sourcing',
                editable: true
            },
            {
                headerName: this.l('Cutting'),
                
                headerTooltip: this.l('Cutting'),
                field: 'cutting',
                editable: true
            },
            {
                headerName: this.l('Packing'),
                
                headerTooltip: this.l('Packing'),
                field: 'packing',
                editable: true
            },
            {
                headerName: this.l('Sheet Weight'),
                
                headerTooltip: this.l('Sheet Weight'),
                field: 'sheetWeight',
                editable: true
            },

            {
                headerName: this.l('Yiled Ration'),
                
                headerTooltip: this.l('Yiled Ration'),
                field: 'yiledRation',
                editable: true
            },
            {
                headerName: this.l('Model'),
                
                headerTooltip: this.l('Model'),
                field: 'model',
                editable: true
            },
            {
                headerName: this.l('Grade Name'),
                
                headerTooltip: this.l('Grade Name'),
                field: 'gradeName',
                editable: true
            },
            {
                headerName: this.l('Model Code'),
                
                headerTooltip: this.l('Model Code'),
                field: 'modelCode',
                editable: true
            }
        ]
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    show(WorkingDateFrom?: any,WorkingDateTo?: any): void {       
        this.isLoading = true;
        this.WorkingDateFrom = WorkingDateFrom;
        this.WorkingDateTo = WorkingDateTo;
        this._service.getIFView(
            this._dateTimeService.convertToDatetime(this.WorkingDateFrom),
            this._dateTimeService.convertToDatetime(this.WorkingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .subscribe((result) => {
            console.log(this.rowdata);
            
            this.rowdata = result.items;
            this.paginationParams.totalCount = result.totalCount;       
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));           
  
           this.resetGridView();
           this.isLoading = false;
        });
        this.modal.show();
        
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
		}, 500)
	}

    

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getIFView(
        
            this._dateTimeService.convertToDatetime(this.WorkingDateFrom),
            this._dateTimeService.convertToDatetime(this.WorkingDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    changePage(paginationParams) {
		this.isLoading = true;
		this.paginationParams = paginationParams;
		this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
		this.getDatas(this.paginationParams)
        .pipe(finalize(() => {
            if(this.rowdata.length > 0)  this.gridTableService.selectFirstRow(this.dataParams!.api)

        }))
        .subscribe((result) => {
			this.paginationParams.totalCount = result.totalCount;
			this.rowdata = result.items;
			this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
			this.resetGridView();
		});
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
