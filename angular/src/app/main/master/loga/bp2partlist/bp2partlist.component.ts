import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    GridParams,
    PaginationParamsModel,
    FrameworkComponent,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaBp2PartListDto, MstLgaBp2PartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBp2PartListModalComponent } from './create-or-edit-bp2partlist-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './bp2partlist.component.html',
    styleUrls: ['./bp2partlist.component.less'],
})
export class Bp2PartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBp2PartList', { static: true }) createOrEditModalBp2PartList:
        | CreateOrEditBp2PartListModalComponent
        | undefined;
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

    selectedRow: MstLgaBp2PartListDto = new MstLgaBp2PartListDto();
    saveSelectedRow: MstLgaBp2PartListDto = new MstLgaBp2PartListDto();
    datas: MstLgaBp2PartListDto = new MstLgaBp2PartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partName: string = '';
    shortName: string = '';
    prodLine: string = '';
    pikProcess: string = '';
    pikSorting: number = 0;
    delProcess: string = '';
    delSorting: number = 0;
    isActive: string = '';
    remark: string = '';

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
        private _service: MstLgaBp2PartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                width: 210,
            },
            {
                headerName: this.l('Short Name'),
                headerTooltip: this.l('Short Name'),
                field: 'shortName',
                flex: 1,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1,
            },
            {
                headerName: this.l('Pik Process'),
                headerTooltip: this.l('Pik Process'),
                field: 'pikProcess',
                flex: 1,
            },
            {
                headerName: this.l('Pik Sorting'),
                headerTooltip: this.l('Pik Sorting'),
                field: 'pikSorting',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Del Process'),
                headerTooltip: this.l('Del Process'),
                field: 'delProcess',
                type: 'rightAligned',
                flex: 1,
            },
            {
                headerName: this.l('Del Sorting'),
                headerTooltip: this.l('Del Sorting'),
                field: 'delSorting',
                flex: 1,
            },

            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isActive == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isActive == 'Y' ? 'btnActive' : 'btnInActive'),
                },
            },
            {
                headerName: this.l(' Remark '),
                headerTooltip: this.l('Remark'),
                field: 'remark',
                flex: 1,
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service
            .getAll(
                this.partName,
                this.shortName,
                this.prodLine,
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
        (this.partName = ''),
        (this.shortName = ''),
        (this.prodLine = ''),
        this.searchDatas();
    }

    onSelectionMulti(params) {
        var selectedRows = this.gridApi.getSelectedRows();
        var selectedRowsString = '';
        var maxToShow = 5;
        selectedRows.forEach(function (selectedRow, index) {
            if (index >= maxToShow) {
                return;
            }
            if (index > 0) {
                selectedRowsString += ', ';
            }
            selectedRowsString += selectedRow.athlete;
        });
        if (selectedRows.length > maxToShow) {
            var othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.partName,
            this.shortName,
            this.prodLine,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgaBp2PartListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgaBp2PartListDto();
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

    deleteRow(system: MstLgaBp2PartListDto): void {
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
    exportToExcel(): void {
        this.isLoading = true;
        this._service.getBp2PartListToExcel(
            this.partName,
            this.shortName,
            this.prodLine,
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.isLoading = false;
        });
    }
    
    
}
