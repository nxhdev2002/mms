import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvGpsScreenSettingDto, MstInvGpsScreenSettingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditGpsScreenSettingModalComponent } from './create-or-edit-gpsscreensetting-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './gpsscreensetting.component.html',
})
export class GpsScreenSettingComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalGpsScreenSetting', { static: true }) createOrEditModalGpsScreenSetting: | CreateOrEditGpsScreenSettingModalComponent | undefined;
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

    selectedRow: MstInvGpsScreenSettingDto = new MstInvGpsScreenSettingDto();
    saveSelectedRow: MstInvGpsScreenSettingDto = new MstInvGpsScreenSettingDto();
    datas: MstInvGpsScreenSettingDto = new MstInvGpsScreenSettingDto();
    isLoading = false;
    pending: string = '';
    disable: boolean = false;

    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    screenName = '';
    screenType = '';
    screenValue = '';
    description = '';


    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        //filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) {
                return null;
            }
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service1: MstInvGpsScreenSettingServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Screen Name'), headerTooltip: this.l('Screen Name'), field: 'screenName', flex: 1 },
            { headerName: this.l('Screen Type'), headerTooltip: this.l('Screen Type'), field: 'screenType', flex: 1 },
            { headerName: this.l('Screen Value'), headerTooltip: this.l('Screen Value'), field: 'screenValue', flex: 1 },
            { headerName: this.l('Description'), headerTooltip: this.l('Description'), field: 'description', flex: 1 },
            { headerName: this.l('Barcode Id'), headerTooltip: this.l('Barcode Id'), field: 'barcodeId', flex: 1 },
            {
                headerName: this.l('Is Active'), headerTooltip: this.l('Is Active'), field: 'isActive', cellClass: ['text-center'], width: 120, cellRenderer: 'agCellButtonComponent',
                // eslint-disable-next-line eqeqeq
                buttonDefTwo: { text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive', iconName: 'fa fa-circle', className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive', },
            }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service1.getAll(
            this.screenName,
            this.screenType,
            this.screenValue,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        this.screenName = '';
        this.screenType = '';
        this.screenValue = '';
        this.description = '';
        this.searchDatas();
    }



    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service1.getAll(
            this.screenName,
            this.screenType,
            this.screenValue,

            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstInvGpsScreenSettingDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstInvGpsScreenSettingDto();
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    deleteRow(system: MstInvGpsScreenSettingDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service1.delete(system.id).subscribe(() => {
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

        this._service1.getGpsScreenSettingToExcel(
            this.screenName,
            this.screenType,
            this.screenValue,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
		.subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
