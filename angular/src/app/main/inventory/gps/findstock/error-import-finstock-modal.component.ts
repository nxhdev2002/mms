import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, GetPartListGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy, InvGpsFinStockServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { CellClickedEvent } from '@ag-grid-enterprise/all-modules';
import { NumericEditor } from '@app/shared/common/numeric-cell-editor/NumericEditor';
import { DatePipe, formatDate } from '@angular/common';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { ImportInvGpsFinstockComponent } from './import-finstock-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';


@Component({
    selector: 'error-import-finstock-modal',
    templateUrl: './error-import-finstock-modal.component.html',
    styleUrls: ['./error-import-finstock-modal.component.less']
})
export class ErrorImportFinstockModalComponent extends AppComponentBase {
    @ViewChild('errorListModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdPartListDto = new InvCkdPartListDto();
    partListColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pipe = new DatePipe('en-US');
    datasEdit: GetGradebyPartListDto[] = [];
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    data: any[] = [];
    guid;
    pending: string = '';
    disable: boolean = false;
    isClone: boolean = true;
    rowSelection = 'multiple';
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    checkValidate: boolean = true;
    checkAddNew: boolean = true;

    defaultColDef = {
        resizable: true,
        //  sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
        // floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };
    gridParams: GridParams;
    constructor(
        injector: Injector,
        //private _service: InvCkdPartListServiceProxy,
        private _service: InvGpsFinStockServiceProxy,
        private _import : ImportInvGpsFinstockComponent,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);
        this.partListColDef = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                field: 'roW_NO',
                width:50
            },
            {
                headerName: this.l('ErrorDescription'),
                flex: 1.5,
                headerTooltip: this.l('ErrorDescription'),
                field: 'errorDescription',
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1,
            },
            {
                headerName: this.l('Qty'),
                headerTooltip: this.l('Qty'),
                field: 'qty',
                flex: 1,
            },
            {
                headerName: this.l('Location'),
                headerTooltip: this.l('Location'),
                field: 'location',
                flex: 1
            },
           
          
        ]
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
            agDatepickerRendererComponent: AgDatepickerRendererComponent
        };
    }

  

    show(guid): void {
        this.guid = guid;
        this._service.getMessageErrorImport(guid)
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.data = result.items ?? [];
            });
        this.modal.show();
    }

    close(): void {
        this.modal.hide();
        this.modalClose.emit(null);
        this._import.close();
    }

    exportToExcel(): void {
        this.pending = 'pending';
        this.disable = true;
        var v_guid = this.guid
        this._service.getInvGpsFinStockErrToExcel(v_guid)
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.pending = '';
                this.disable = false;
            });
    }
}
