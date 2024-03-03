import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstPtsBmpPartListDto, MstPtsBmpPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditBmpPartListModalComponent } from './create-or-edit-bmppartlist-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';

@Component({
    templateUrl: './bmppartlist.component.html',
    styleUrls: ['./bmppartlist.component.less'],
})
export class BmpPartListComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalBmpPartList', { static: true }) createOrEditModalBmpPartList: | CreateOrEditBmpPartListModalComponent | undefined;
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

    selectedRow: MstPtsBmpPartListDto = new MstPtsBmpPartListDto();
    saveSelectedRow: MstPtsBmpPartListDto = new MstPtsBmpPartListDto();
    datas: MstPtsBmpPartListDto = new MstPtsBmpPartListDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    model: string = '';
    cfc: string = '';
    grade: string = '';
    backNo: string = '';
    prodLine: string = '';
    partTypeCode: string = '';
    partTypeId
    process: string = '';
    pkProcess: string = '';
    isPunch: string = '';
    specialColor: string = '';
    signalId
    signalCode: string = '';
    remark: string = '';
    isActive: string = '';
    isBumber;

    BumberList = [
        {value: 'Y' , label: "Bumber"},
        {value: 'N' , label: "InBumber"},
    ];

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
        private _service: MstPtsBmpPartListServiceProxy,
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
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model', flex: 1
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc', flex: 1
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade', flex: 1
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
                headerName: this.l('Part Type Code'),
                headerTooltip: this.l('Part Type Code'),
                field: 'partTypeCode',
                flex: 1
            },
            {
                headerName: this.l('Part Type Id'),
                headerTooltip: this.l('Part Type Id'),
                field: 'partTypeId',
                flex: 1
            },
            {
                headerName: this.l('Process'),
                headerTooltip: this.l('Process'),
                field: 'process',
                flex: 1
            },
            {
                headerName: this.l('Pk Process'),
                headerTooltip: this.l('Pk Process'),
                field: 'pkProcess',
                flex: 1
            },
            {
                headerName: this.l('Is Punch'),
                headerTooltip: this.l('Is Punch'),
                field: 'isPunch',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo:
                {
                    text: params => (params.data?.isPunch == 'Y') ?'Active': 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isPunch == 'Y') ?'btnActive': 'btnInActive',
                },
            },
			{
                headerName: this.l('Special Color'),
                headerTooltip: this.l('Special Color'),
                field: 'specialColor',
                flex: 1
            },
            {
                headerName: this.l('Signal Id'),
                headerTooltip: this.l('Signal Id'),
                field: 'signalId',
                flex: 1
            },
            {
                headerName: this.l('Signal Code'),
                headerTooltip: this.l('Signal Code'),
                field: 'signalCode',
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
                    text: params => (params.data?.isActive == "Y") ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isActive == "Y") ? 'btnActive' : 'btnInActive',
                },
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
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.model,
            this.cfc,
            this.grade,
            this.prodLine,
            this.partTypeCode,
            this.isBumber,
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
        this.model = '',
        this.cfc = '',
        this.grade = '',
        this.prodLine = '',
        this.partTypeCode = '',
        this.isBumber = '',
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
            this.model,
            this.cfc,
            this.grade,
            this.prodLine,
            this.partTypeCode,
            this.isBumber,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstPtsBmpPartListDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstPtsBmpPartListDto();
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

    deleteRow(system: MstPtsBmpPartListDto): void {
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
        this._service.getBmpPartListToExcel(
            this.model,
            this.cfc,
            this.grade,
            this.prodLine,
            this.partTypeCode,
            this.isBumber,
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }


}
