import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstWptPatternHDto, MstWptPatternHServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditPatternHModalComponent } from './create-or-edit-patternh-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './patternh.component.html', 
})
export class PatternHComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalPatternH', { static: true }) createOrEditModalPatternH: | CreateOrEditPatternHModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    domainColumdef: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstWptPatternHDto = new MstWptPatternHDto();
    saveSelectedRow: MstWptPatternHDto = new MstWptPatternHDto();
    datas: MstWptPatternHDto = new MstWptPatternHDto();
    isLoading: boolean = false;
 

    dataParams: GridParams | undefined;
    rowData: MstWptPatternHDto[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    type: number = 0;
    startDate: any;
    endDate: any;
    description: string = '';
    isActive: string = '';
    frameworkComponents: FrameworkComponent;
    domainCode: string = 'PATTERN';
    itemValue: number = 0;
    itemCode: string = '';
    cbbDomain: any[] = [];

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

    constructor(
        injector: Injector,
        private _service: MstWptPatternHServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        this.domainColumdef = [
            {
                headerName: 'Row No',
                headerTooltip: 'Row No',
                field: 'rowNo',
                cellClass: ['text-center'],

                flex: 1
            },
            {
                headerName: 'Pattern Type ',
                headerTooltip: 'Pattern Type',
                field: 'itemCode',
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: 'No Of Shift',
                headerTooltip: 'No Of Shift',
                field: 'itemValue',
                cellClass: ['text-center'],
                flex: 1
            },
        ];

        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 60,
            },
            {
                headerName: this.l('Type'),
                headerTooltip: this.l('Type'),
                field: 'type',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Start Date'),
                headerTooltip: this.l('Start Date'),
                field: 'startDate',
                valueGetter: (params) => this.pipe.transform(params.data?.startDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('End Date'),
                headerTooltip: this.l('End Date'),
                field: 'endDate',
                valueGetter: (params) => this.pipe.transform(params.data?.endDate, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },
                width: 120,
            },
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };

        this._service.getsByDomainCode(
            this.domainCode,
            '',
            '',
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize)
            .subscribe((result) => {
                this.cbbDomain = result.items;
            });
    }

    searchDatas(): void {
        this._service.getAll(
            this.itemValue,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
            this.description,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }
    searchDatasByStatus(status: string): void {
        this._service.getAll(
            this.itemValue,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
            this.description,
            status,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        this.itemValue = 0,
            this.startDate = '',
            this.endDate = '',
            this.description = '',
            this.isActive = '',
            this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.type,
            this._dateTimeService.convertToDatetime(this.startDate),
            this._dateTimeService.convertToDatetime(this.endDate),
            this.description,
            this.isActive,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstWptPatternHDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstWptPatternHDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.getDatas(this.paginationParams)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    deleteRow(system: MstWptPatternHDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;

    //     this._service.getPatternHToExcel(
    //         this.type,
    //         this.startDate,
    //         this.isActive,
    //     )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void { 

        this.fn.exportLoading(e, true);
         
        this._service.getPatternHToExcel(
            this.type,
            this.startDate,
            this.isActive,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));
             
        });
    }


    buttonAction(i: number, status: string) {
 
        let _btnUncheck = document.querySelector('.actionButton_w'+i+'.active');
        if(_btnUncheck){
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            this.searchDatasByStatus('');
        }
        else {
            let objbtn = document.querySelectorAll('.groupBtn');
            for (let i = 0; objbtn[i]; i++) { objbtn[i].classList.remove('active'); }

            let _btn = document.querySelector('.actionButton_w'+i);
            if(_btn) _btn.classList.add('active');

            this.searchDatasByStatus(status);
        } 
    } 
}
