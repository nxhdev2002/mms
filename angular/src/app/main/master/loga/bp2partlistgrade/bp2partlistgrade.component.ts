import { GridParams } from '@app/shared/common/models/base.model';
import { DatePipe } from '@angular/common';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CustomColDef, FrameworkComponent, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstLgaBp2PartListGradeDto, MstLgaBp2PartListGradeServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { ceil, result } from 'lodash-es';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { GridApi } from '@ag-grid-enterprise/all-modules';
import { CreateOrEditBp2PartListGradeModalComponent } from './create-or-edit-bp2partlistgrade-modal.component';
import { ImportBp2PartListGradeComponent } from './import-bp2partlistgrade-modal.component';
import { Bp2PartListGradeExportComponent } from './bp2partlistgrade-export-modal.component';


@Component({
    templateUrl: './bp2partlistgrade.component.html',
    styleUrls: ['./bp2partlistgrade.component.less']
})
export class Bp2PartListGradeComponent extends AppComponentBase implements OnInit {

    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('importExcelModal', { static: true }) importExcelModal:| ImportBp2PartListGradeComponent| undefined;
    @ViewChild('exportExcelModal', { static: true }) exportExcelModal:| Bp2PartListGradeExportComponent| undefined;
    @ViewChild('createOrEditModalBp2PartListGrade', { static: true }) createOrEditModalBp2PartListGrade:| CreateOrEditBp2PartListGradeModalComponent| undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: MstLgaBp2PartListGradeDto = new MstLgaBp2PartListGradeDto();
    saveSelectedRow: MstLgaBp2PartListGradeDto = new MstLgaBp2PartListGradeDto();
    datas: MstLgaBp2PartListGradeDto = new MstLgaBp2PartListGradeDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    partNo: string = '';
    partName: string = '';
    partListId: number;
    prodLine: string = '';
    grade: string = '';
    pikLocType: string = '';
    pikAddress: string = '';
    pikAddressDisplay: string = '';
    delLocType: string = '';
    delAddress: string = '';
    delAddressDisplay: string = '';
    sorting: string ='';
    isActive: string = '';
    model: string = '';

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
    }
    constructor(
        injector: Injector,
        private _service: MstLgaBp2PartListGradeServiceProxy,
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
                width: 110,
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'partName',
                width: 220,
            },
            {
                headerName: this.l('Part List Id'),
                headerTooltip: this.l('Part List Id'),
                field: 'partListId',
                width: 90,
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90,
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 90,
            },

            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 90,
            },
            {
                headerName: this.l('UsageQty'),
                headerTooltip: this.l('UsageQty'),
                field: 'usageQty',
                type: 'rightAligned',
                width: 90,
            },
            {
                headerName: this.l('Pik Loc Type'),
                headerTooltip: this.l('Pik Loc Type'),
                field: 'pikLocType',
                width: 120,
            },
            {
                headerName: this.l('Pik Address'),
                headerTooltip: this.l('Pik Address'),
                field: 'pikAddress',
                width: 100,
            },
            {
                headerName: this.l('Pik Address Display'),
                headerTooltip: this.l('Pik Address Display'),
                field: 'pikAddressDisplay',
                width: 150,
            },
            {
                headerName: this.l('Del Loc Type'),
                headerTooltip: this.l('Del Loc Type'),
                field: 'delLocType',
                width: 100,
            },
            {
                headerName: this.l('Del Address'),
                headerTooltip: this.l('Del Address'),
                field: 'delAddress',
                width: 100,
            },
            {
                headerName: this.l('Del Address Display'),
                headerTooltip: this.l('Del Address Display'),
                field: 'delAddressDisplay',
                width: 150,
            },
            {
                headerName: this.l('Sorting'),
                headerTooltip: this.l('Sorting'),
                field: 'sorting',
                type: 'rightAligned',
                width: 90,
            },
            {
                headerName: this.l('Remark'),
                headerTooltip: this.l('Remark'),
                field: 'remark',
                width: 90,
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
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        }
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service.getAll(
            this.partNo,
            this.prodLine,
            this.grade,
            this.model,
            this.partName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize,
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        });
    }

    clearTextSearch() {
        this.partNo = '',
            this.prodLine = '',
            this.grade = '',
            this.model = '',
            this.partName = '',
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel)
    {
        return this._service.getAll(
            this.partNo,
            this.prodLine,
            this.grade,
            this.model,
            this.partName,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize,
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstLgaBp2PartListGradeDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstLgaBp2PartListGradeDto();
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

    deleteRow(system: MstLgaBp2PartListGradeDto): void
    {
        this.message.confirm(this.l('AreYouSureToDelete'),'Delete Row', (isConfirmed) => {
            if(isConfirmed)
            {
                this._service.delete(system.id).subscribe(() =>{
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    exportToExcel(): void{
        this.isLoading = true;
        this._service.getBp2PartListGradeToExcel(
            this.model,
        ).subscribe((result) => {
            this._fileDownloadService.downloadTempFile(result);
            this.isLoading = false;
        });
    }
    
    
	______
}
