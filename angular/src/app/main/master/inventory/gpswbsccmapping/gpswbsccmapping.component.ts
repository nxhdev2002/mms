import {  MstGpsWbsCCMappingDto, MstGpsWbsCCMappingServiceProxy } from './../../../../../shared/service-proxies/service-proxies';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { CreateOrEditGpsWbsCCMappingModalComponent } from './create-or-edit-gpswbsccmapping-modal.component';
import { DatePipe } from '@angular/common';

@Component({
    templateUrl: './gpswbsccmapping.component.html',
})
export class GpsWbsCCMappingComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalGpsWbsCCMapping', { static: true }) createOrEditModal: | CreateOrEditGpsWbsCCMappingModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstGpsWbsCCMappingDto = new MstGpsWbsCCMappingDto();
    saveSelectedRow: MstGpsWbsCCMappingDto = new MstGpsWbsCCMappingDto();

    isLoading: boolean = false;
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    costCenterFrom: string = '';
    wbsFrom: string = '';
    rowSelection = 'multiple';
    selectId;
    changedRecordsMaterialList;

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
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
        private _service: MstGpsWbsCCMappingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            {
                headerName: this.l('Cost Center From'),
                headerTooltip: this.l('Cost Center From'),
                field: 'costCenterFrom', flex: 1
            },
            {
                headerName: this.l('Wbs From'),
                headerTooltip: this.l('Wbs From'),
                field: 'wbsFrom', flex: 1
            },
            {
                headerName: this.l('Cost Center To'),
                headerTooltip: this.l('Cost Center To'),
                field: 'costCenterTo', flex: 1
            },
            {
                headerName: this.l('Wbs To'),
                headerTooltip: this.l('Wbs To'),
                field: 'wbsTo', flex: 1
            },
            {
                headerName: this.l('Effective From Date'),
                headerTooltip: this.l('Effective From Date'),
                field: 'tcFrom',flex: 1, 
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveFromDate, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Effective From To'),
                headerTooltip: this.l('Effective From To'),
                field: 'tcFrom',flex: 1, 
                valueGetter: (params) => this.pipe.transform(params.data?.effectiveFromTo, 'dd/MM/yyyy')
            },
            {
                headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsMaterialList = result;
            console.log("result =", result);
        })
    }
    searchDatas(): void {
        this._service.getAll(
            this.costCenterFrom,
            this.wbsFrom,
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
        this.costCenterFrom = '',
        this.wbsFrom = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.costCenterFrom,
            this.wbsFrom,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstGpsWbsCCMappingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstGpsWbsCCMappingDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.pageNum = (this.paginationParams.pageNum <= 0) ? 1: this.paginationParams.pageNum;
        
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

    deleteRow(system: MstGpsWbsCCMappingDto): void {
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

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getMstGpsWbsCCMappingToExcel(
            this.costCenterFrom,
            this.wbsFrom
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    getdisable() {
        if(this.selectedRow.id) {
             return !this.changedRecordsMaterialList.includes(this.selectedRow.id);
        }
        else return false;
    }
}