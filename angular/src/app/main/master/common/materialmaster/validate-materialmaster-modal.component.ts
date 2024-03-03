import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockIssuingServiceProxy, InvCkdStockIssuingValidateDto, MstCmmMaterialMasterServiceProxy, MstCommonMMValidationResultServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { ceil, result } from 'lodash-es';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { DatePipe } from '@angular/common';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { finalize } from 'rxjs';

@Component({
    selector: 'validate-materialmaster-modal',
    templateUrl: './validate-materialmaster-modal.component.html',
})
export class ValidateMaterialMasterModalComponent extends AppComponentBase {
    @ViewChild('validateMaterial', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowDataValidate: InvCkdStockIssuingValidateDto = new InvCkdStockIssuingValidateDto();
    rowData: any[] = [];
    validateColDefs: CustomColDef[] = [];
    _isActive: boolean = false;
    saving: boolean = false;
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };


    partNo;
    supplierNo;
    cfc;
    lotNo;
    noInLot;
    vinNo;
    messagesError;
    dataParams: GridParams | undefined;
    isLoading;
    disable;
    pending;
    ruleCode: string = '';
    ruleDescription: string = '';
    ruleItem: string = '';
    materialCode: string = '';
    resultfield: string = '';
    materialGroup: string = '';
    fn: CommonFunction = new CommonFunction();
    paginationParam: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
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
    constructor(injector: Injector,
        private _service: MstCmmMaterialMasterServiceProxy,
        private _service_validationresult: MstCommonMMValidationResultServiceProxy,
        private _fileDownloadService: FileDownloadService) {
        super(injector);

        this.validateColDefs = [
            
            {
                headerName: this.l('STT'), headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1,
                cellClass: ['text-center'],
                width: 55
            },
            {
                headerName: this.l('Error Description'),
                headerTooltip: this.l('Error Description'),
                field: 'errorMessage',

            },
            // {
            //     headerName: this.l('Materia Id'),
            //     headerTooltip: this.l('Materia Id'),
            //     field: 'materiaId',

            // },
            {
                headerName: this.l('Material Code'),
                headerTooltip: this.l('Material Code'),
                field: 'materialCode',

            },
            {
                headerName: this.l('Material Name'),
                headerTooltip: this.l('Material Name'),
                field: 'materialName',

            },
            {
                headerName: this.l('Material Group'),
                headerTooltip: this.l('Material Group'),
                field: 'materialGroup',

            },
            {
                headerName: this.l('Valuation Class'),
                headerTooltip: this.l('Valuation Class'),
                field: 'valuationClass',

            },
            {
                headerName: this.l('Valuation Type'),
                headerTooltip: this.l('Valuation Type'),
                field: 'valuationType',
            },
            {
                headerName: this.l('Rule Id'),
                headerTooltip: this.l('Rule Id'),
                field: 'ruleId',

            },
            {
                headerName: this.l('Rule Code'),
                headerTooltip: this.l('Rule Code'),
                field: 'ruleCode',

            },
            {
                headerName: this.l('Rule Description'),
                headerTooltip: this.l('Rule Description'),
                field: 'ruleDescription',

            },
            {
                headerName: this.l('Rule Item'),
                headerTooltip: this.l('Rule Item'),
                field: 'ruleItem',

            },
            {
                headerName: this.l('Option'),
                headerTooltip: this.l('Option'),
                field: 'option',

            },
            {
                headerName: this.l('Result Field'),
                headerTooltip: this.l('Result Field'),
                field: 'resultField',

            },
            {
                headerName: this.l('Expected Result'),
                headerTooltip: this.l('Expected Result'),
                field: 'expectedResult',
                valueGetter: (params) => params.data?.expectedResult != '' ? params.data?.expectedResult : '(null)'

            },
            {
                headerName: this.l('Actual Result'),
                headerTooltip: this.l('Actual Result'),
                field: 'actualResult',
                valueGetter: (params) => params.data?.actualResult != '' ? params.data?.actualResult : '(null)'

            },
            {
                headerName: this.l('Last Validation Datetime'),
                headerTooltip: this.l('Last Validation Datetime'),
                field: 'lastValidationDatetime',
                valueGetter: (params) => this.pipe.transform(params.data?.lastValidationDatetime, 'dd/MM/yyyy HH:mm')

            },
            {
                headerName: this.l('Lastvalidationby'),
                headerTooltip: this.l('Lastvalidationby'),
                field: 'lastvalidationby',

            },
            // {
            //     headerName: this.l('Last Validation Id'),
            //     headerTooltip: this.l('Last Validation Id'),
            //     field: 'lastValidationId',

            // },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',

            },
            


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {

    }

    show(): void {
        this.getDataValidate();
        this.modal.show();
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }



    getDataValidate() {
        this.isLoading = true;
        this._service_validationresult.getAll(
            this.materialCode,
            this.materialGroup,
            this.ruleCode,
            this.ruleItem,
            this.resultfield,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        ).subscribe(result => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        })
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service_validationresult.getAll(
            this.materialCode,
            this.materialGroup,
            this.ruleCode,
            this.ruleItem,
            this.resultfield,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }


    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
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
            this.autoSizeAll();
        }, 100)
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    callBackGrid(params) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParam).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    // changePage(paginationParams) {
    //     this.isLoading = true;
    //     this.paginationParams = paginationParams;
    //     this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
    //     this.getDatas(this.paginationParams).subscribe((result) => {
    //         this.paginationParams.totalCount = result.totalCount;
    //         this.rowData = result.items;
    //         this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
    //         this.isLoading = false;
    //     });
    // }
    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParam = paginationParams;
        this.paginationParam.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParam).pipe(finalize(() => {
        })).subscribe((result) => {
            this.paginationParam.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParam.totalPage = ceil(result.totalCount / (this.paginationParam.pageSize ?? 0));
            this.isLoading = false;
        });
    }
    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
    exportToExcelValidate(e):void{
            this.fn.exportLoading(e, true);
            this._service_validationresult.getCmmMMValidationResultToExcel(
                this.materialCode,
                this.materialGroup,
                this.ruleCode,
                this.ruleItem,
                this.resultfield
            ).subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));

            });
        }

}


