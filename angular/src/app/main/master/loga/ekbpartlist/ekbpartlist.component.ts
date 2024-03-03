import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaEkbPartListDto, MstLgaEkbPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditEkbPartListModalComponent } from './create-or-edit-ekbpartlist-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './ekbpartlist.component.html',
    styleUrls: ['./ekbpartlist.component.less'],
})
export class EkbPartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalEkbPartList', { static: true }) createOrEditModalEkbPartList: | CreateOrEditEkbPartListModalComponent | undefined;
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

    selectedRow: MstLgaEkbPartListDto = new MstLgaEkbPartListDto();
    saveSelectedRow: MstLgaEkbPartListDto = new MstLgaEkbPartListDto();
    datas: MstLgaEkbPartListDto = new MstLgaEkbPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partNoNormanlized: string = '';
    partName: string = '';
    backNo: string = '';
    prodLine: string = '';
    supplierNo: string = '';
    model: string = '';
    processId
    processCode: string = '';
    usageQty: number = 0;
    boxQty: number = 0;
    pcAddress: string = '';
    pcSorting: number = 0;
    spsAddress: string = '';
    spsSorting: number = 0;
    remark: string = '';
    isActive: string = '';

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
        private _service: MstLgaEkbPartListServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1
            },
            {
                headerName: this.l('Part No Normanlized'),
                headerTooltip: this.l('Part No Normanlized'),
                field: 'partNoNormanlized',
                flex: 1
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                flex: 1
            },
            {
                headerName: this.l('Back No'),
                headerTooltip: this.l('Back No'),
                field: 'backNo',
                flex: 1
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                flex: 1
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                flex: 1
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1
            },
            // {
            //     headerName: this.l('Process Id'),
            //     headerTooltip: this.l('Process Id'),
            //     field: 'processId',
            //     type: 'rightAligned',
            //     flex: 1
            // },
            {
                headerName: this.l('Process Code'),
                headerTooltip: this.l('Process Code'),
                field: 'processCode',
                flex: 1
            },
            {
                headerName: this.l('Module Code'),
                headerTooltip: this.l('Module Code'),
                field: 'moduleCode',
                flex: 1
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                flex: 1
            },
            {
                headerName: this.l('ExporterBackNo'),
                headerTooltip: this.l('ExporterBackNo'),
                field: 'exporterBackNo',
                flex: 1
            },
            {
                headerName: this.l('Body Color'),
                headerTooltip: this.l('Body Color'),
                field: 'bodyColor',
                flex: 1
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1
            },
            {
                headerName: this.l('Usage Qty'),
                headerTooltip: this.l('Usage Qty'),
                field: 'usageQty',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Box Qty'),
                headerTooltip: this.l('Box Qty'),
                field: 'boxQty',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Pc Address'),
                headerTooltip: this.l('Pc Address'),
                field: 'pcAddress',
                flex: 1
            },
            {
                headerName: this.l('Pc Sorting'),
                headerTooltip: this.l('Pc Sorting'),
                field: 'pcSorting',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Sps Address'),
                headerTooltip: this.l('Sps Address'),
                field: 'spsAddress',
                flex: 1
            },
            {
                headerName: this.l('Sps Sorting'),
                headerTooltip: this.l('Sps Sorting'),
                field: 'spsSorting',
                type: 'rightAligned',
                flex: 1
            },
            {
                headerName: this.l('Remark'),
                headerTooltip: this.l('Remark'),
                field: 'remark',
                flex: 1
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                },
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.partNo,
            this.backNo,
            this.prodLine,
            this.supplierNo,
            this.model,
            this.processCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.resetGridView();
            });
    }
    autoSizeAll() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
          if (column.getId().toString() != "checked" && column.getId().toString() != "stt"){
            allColumnIds.push(column.getId());
          }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView(){
      
        setTimeout(()=>{
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true, 
            });
            this.autoSizeAll();
        },100)
    }
    clearTextSearch() {
        this.partNo = '',
            this.backNo = '',
            this.prodLine = '',
            this.supplierNo = '',
            this.model = '',
            this.processCode = '',
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
            this.partNo,
            this.backNo,
            this.prodLine,
            this.supplierNo,
            this.model,
            this.processCode,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgaEkbPartListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgaEkbPartListDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
            this.resetGridView();
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
                this.resetGridView();
            });
    }

    deleteRow(system: MstLgaEkbPartListDto): void {
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
        this._service.getEkbPartListToExcel(
            this.partNo,
            this.backNo,
            this.prodLine,
            this.supplierNo,
            this.model,
            this.processCode,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
                this.isLoading = false;
            });
    }


}
