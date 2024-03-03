import {
    GridApi,
    IDetailCellRendererParams,
    MasterDetailModule,
    ModuleRegistry,
} from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import {
    CustomColDef,
    GridParams,
    PaginationParamsModel,
    FrameworkComponent,
} from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import {
    MstCmmLotCodeGradeDto,
    MstCmmModelDto,
    MstCmmModelServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditModelModalComponent } from './create-or-edit-model-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
ModuleRegistry.registerModules([MasterDetailModule]);
@Component({
    templateUrl: './model.component.html',
    styleUrls: ['./model.component.less'],
})
export class ModelComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalModel', { static: true }) createOrEditModalModel:
        | CreateOrEditModelModalComponent
        | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    columnDefs: CustomColDef[] = [];
    detailCellRendererParams: any;
    paginationParamModels: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamLots: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    //
    modelColDefs: CustomColDef[] = [];
    lotCodeGradeColDefs: any;

    selectedRow: MstCmmModelDto = new MstCmmModelDto();
    saveSelectedRow: MstCmmModelDto = new MstCmmModelDto();
    modelDto: MstCmmModelDto = new MstCmmModelDto();
    lotCodeGradeDto: MstCmmLotCodeGradeDto = new MstCmmLotCodeGradeDto();
    datas: MstCmmModelDto = new MstCmmModelDto();
    isLoading: boolean = false;

    dataParams: GridParams | undefined;
    rowDataModel: any[] = [];
    rowDataLotCodeGrade: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    code: string = '';
    name: string = '';
    cfc: string = '';
    modelCode: string = '';
    grade : string = '';
    lotCode : string = '';
    gradeName : string = '';
    katashiki : string = '';
    materialCode: string = '';
    productionGroup: string = '';
    valuationType: string = '';
    modelVin: string = '';
    isActive: string = '';
    model: string = '';

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
        private _service: MstCmmModelServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);

        this.modelColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamModels.pageSize * (this.paginationParamModels.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
            },
            {
                headerName: this.l('Code'),
                headerTooltip: this.l('Code'),
                field: 'code',
                flex: 1,
                cellRenderer: 'agGroupCellRenderer',
            },
            {
                headerName: this.l('Name'),
                headerTooltip: this.l('Name'),
                field: 'name',
                flex: 1,
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 150,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: (params) => (params.data?.isActive == 'Y' ? 'Active' : 'Inactive'),
                    iconName: 'fa fa-circle',
                    className: (params) => (params.data?.isActive == 'Y' ? 'btnActive' : 'btnInActive'),
                },
            },
        ];

        this.lotCodeGradeColDefs = [
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1,
            },
            {
                headerName: this.l('Lot Code'),
                headerTooltip: this.l('Lot Code'),
                field: 'lotCode',
                flex: 1,
            },
            {
                headerName: this.l('Cfc'),
                headerTooltip: this.l('Cfc'),
                field: 'cfc',
                flex: 1,
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1,
            },
            {
                headerName: this.l('Grade Name'),
                headerTooltip: this.l('Grade Name'),
                field: 'gradeName',
                flex: 1,
            },
            {
                headerName: this.l('Model Code'),
                headerTooltip: this.l('Model Code'),
                field: 'modelCode',
                flex: 1,
            },
            {
                headerName: this.l('Model Vin'),
                headerTooltip: this.l('Model Vin'),
                field: 'modelVin',
                flex: 1,
            },
        ],
            this.frameworkComponents = {
                agCellButtonComponent: AgCellButtonRendererComponent,
            };
    }

    ngOnInit(): void {
        this.paginationParamModels = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamLots = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this._service
            .getAllModel(
                this.cfc,
                this.modelVin,
                this.modelCode,
                '',
                this.paginationParamModels.skipCount,
                this.paginationParamModels.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items;
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
            });
    }

    clearTextSearch() {
        this.cfc = '',
            this.modelCode = '',
            this.modelVin = '',
            this.searchDatas();
    }

    //
    getDataModels(paginationParams?: PaginationParamsModel) {
        return this._service.getAllModel(
            this.cfc,
            this.modelVin,
            this.modelCode,
            '',
            this.paginationParamModels.skipCount,
            this.paginationParamModels.pageSize
        );
    }

    onChangeRowSelectionModel(params: { api: { getSelectedRows: () => MstCmmModelDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.searchDataLotCodeGrades(selected.code);
        }
        this.selectedRow = Object.assign({}, selected);
        console.log(this.selectedRow);
    }

    changePageModel(paginationParams) {
        this.isLoading = true;
        this.paginationParamModels = paginationParams;
        this.paginationParamModels.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataModels(this.paginationParamModels).subscribe((result) => {
            this.paginationParamModels.totalCount = result.totalCount;
            this.rowDataModel = result.items;
            this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGridModel(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamModels.pageSize);
        this.paginationParamModels.skipCount =
            ((this.paginationParamModels.pageNum ?? 1) - 1) * (this.paginationParamModels.pageSize ?? 0);
        this.paginationParamModels.pageSize = this.paginationParamModels.pageSize;
        this.getDataModels(this.paginationParamModels)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items ?? [];
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                this.isLoading = false;
            });
    }


    //lotCodeGrade

    searchDataLotCodeGrades(code): void {
        this._service.getAllLotCodeGrade(
            this.cfc,
            code ?? '',
            this.modelVin,
            this.modelCode,
            '',
            this.paginationParamLots.skipCount,
            this.paginationParamLots.pageSize
        )
            .pipe(finalize(() => { }
            )).subscribe((result) => {
                this.paginationParamLots.totalCount = result.totalCount;
                this.rowDataLotCodeGrade = result.items;
                this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
            });
    }

    getDataLotCodeGrades(paginationParams?: PaginationParamsModel) {
        return this._service.getAllLotCodeGrade(
            this.cfc,
            this.code,
            this.modelVin,
            this.modelCode,
            '',
            this.paginationParamLots.skipCount,
            this.paginationParamLots.pageSize
        );
    }

    onChangeRowSelectionLotCodeGrade(params: { api: { getSelectedRows: () => MstCmmModelDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmModelDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePageLotCodeGrade(paginationParams) {
        this.isLoading = true;
        this.paginationParamLots = paginationParams;
        this.paginationParamLots.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataLotCodeGrades(this.paginationParamLots).subscribe((result) => {
            this.paginationParamLots.totalCount = result.totalCount;
            this.rowDataLotCodeGrade = result.items;
            this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    callBackDataGridLotCodeGrade(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamLots.pageSize);
        this.paginationParamLots.skipCount =
            ((this.paginationParamLots.pageNum ?? 1) - 1) * (this.paginationParamLots.pageSize ?? 0);
        this.paginationParamLots.pageSize = this.paginationParamLots.pageSize;
        this.getDataLotCodeGrades(this.paginationParamLots)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamLots.totalCount = result.totalCount;
                this.rowDataLotCodeGrade = result.items ?? [];
                this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    //

    deleteRow(system: MstCmmModelDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGridModel(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;

    //     this._service.getModelToExcel(
    //         this.code,
    //         this.name,
    //         this.cfc,
    //         this.modelCode,
    //         this.modelVin,
    //         this.isActive,)
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getModelToExcel(
            this.code,
            this.name,
            this.cfc,
            this.modelCode,
            this.modelVin,
            this.isActive
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    // exportLotCodeToFile(): void {
    //     this.pending1 = 'pending1';
    //     this.disable = true;

    //     this._service.getLotCodeGradeToExcel(
    //         this.cfc,
    //         this.modelCode,
    //         this.modelVin,
    //         this.materialCode,
    //         this.productionGroup,
    //         this.valuationType,
    //         '',
    //         this.paginationParamModels.skipCount,
    //         this.paginationParamModels.pageSize
    //         )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportLotCodeToFile(e): void {

        this.fn.exportLoading(e, true);

        this._service.getLotCodeGradeToExcel(
            this.cfc,
            this.modelCode,
            this.modelVin,
            this.materialCode,
            this.productionGroup,
            this.valuationType,
            '',
            this.paginationParamModels.skipCount,
            this.paginationParamModels.pageSize
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
