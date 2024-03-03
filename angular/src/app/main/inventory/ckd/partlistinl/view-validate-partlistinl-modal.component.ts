import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, GetInvInpIfViewDto, GetPartListGradeDto, InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy, InvPioPartListInlServiceProxy, MstLgaBp2PartListServiceProxy, ValidatePartListDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { ceil } from 'lodash-es';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { FileDownloadService } from '@shared/utils/file-download.service';



@Component({
    selector: 'view-validate-partlistinl-modal',
    templateUrl: './view-validate-partlistinl-modal.component.html'
})
export class ViewValidatePartListInlModalComponent extends AppComponentBase {
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

    gridParams: GridParams;
    gridTableService: any;
    constructor(
        injector: Injector,
        private _service: InvPioPartListInlServiceProxy,
        private _fileDownloadService: FileDownloadService,

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
            {
                headerName: this.l('PartListId'),
                headerTooltip: this.l('PartListId'),
                field: 'partListId',

            },
            {
                headerName: this.l('ErrorDescription'),
                headerTooltip: this.l('ErrorDescription'),
                field: 'errorDescription',

            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',

            },
            {
                headerName: this.l('PartNoNormalizedS4'),
                headerTooltip: this.l('PartNoNormalizedS4'),
                field: 'partNoNormalizedS4',

            },
            {
                headerName: this.l('PartName'),
                headerTooltip: this.l('PartName'),
                field: 'partName',

            },
            // {
            //     headerName: this.l('SupplierNo'),
            //     headerTooltip: this.l('SupplierNo'),
            //     field: 'supplierNo',

            // },
            {
                headerName: this.l('Model'),
                 headerTooltip: this.l('Model'),
                field: 'model',

            },
            {
                headerName: this.l('Cfc'),
                 headerTooltip: this.l('Cfc'),
                field: 'cfc',

            },
            {
                headerName: this.l('MaterialId'),
                 headerTooltip: this.l('MaterialId'),
                field: 'materialId',

            },
            {
                headerName: this.l('OrderPattern'),
                 headerTooltip: this.l('OrderPattern'),
                field: 'orderPattern',

            },
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',

            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',

            },
            {
                headerName: this.l('Usage Qty'),
                headerTooltip: this.l('Usage Qty'),
                field: 'usageQty',
                type: 'rightAligned',

                editable: true,
            },
            {
                headerName: this.l('Body Color'),
                headerTooltip: this.l('Body Color'),
                field: 'bodyColor',

            },
            {
                headerName: this.l('Start Lot'),

                headerTooltip: this.l('Start Lot'),
                field: 'startLot',
                editable: true
            },
            {
                headerName: this.l('End Lot'),

                headerTooltip: this.l('End Lot'),
                field: 'endLot',
                editable: true
            },
            {
                headerName: this.l('Start Run'),

                headerTooltip: this.l('Start Run'),
                field: 'startRun',
                editable: true
            },
            {
                headerName: this.l('End Run'),

                headerTooltip: this.l('End Run'),
                field: 'endRun',
                editable: true
            }
        ]
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    show(): void {
        this.isLoading = true;
        this._service.getValidateInvPioPartList(
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .subscribe((result) => {
            this.rowdata = result.items;
            console.log(this.rowdata);
            this.paginationParams.totalCount = result.totalCount;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
           this.modal.show();
           this.resetGridView();
           this.isLoading = false;
        });


    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
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

    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked']);
            this.gridParams.api.redrawRows();
        });
    }

    getDatas(paginationParams?: PaginationParamsModel) {
		return this._service.getValidateInvPioPartList(
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

    callBackDataGrid(params: GridParams) {
        this.gridParams = params;
        setTimeout(() => {
            this.selectAll();
            this.resetGridView();
        }, 500)
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getValidateInvPioPartListExcel(
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });

    }
}
