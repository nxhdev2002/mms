import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwMwhCaseDataDto, LgwMwhCaseDataServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonTransparentRendererComponent } from '@app/shared/common/grid/ag-cell-button-transparent-renderer/ag-cell-button-transparent-renderer.component';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AutoComplete } from 'primeng/autocomplete';
import { CaseDataHistoryComponent } from './casedata-history-modal.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './casedata.component.html',
    styleUrls: ['./casedata.component.less'],
})
export class CaseDataComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('contactHisModal') contactHisModal!: CaseDataHistoryComponent;
    alwaysShowHorizontalScroll: true;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: LgwMwhCaseDataDto = new LgwMwhCaseDataDto();
    saveSelectedRow: LgwMwhCaseDataDto = new LgwMwhCaseDataDto();
    datas: LgwMwhCaseDataDto = new LgwMwhCaseDataDto();
    isLoading = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    caseNo = '';
    devanningDateFrom = new Date();
    devanningDateTo = new Date();
    lotNo = '';
    grade = '';
    model = '';
    caseQty = 0;
    containerNo = '';
    renban = '';
    supplierNo = '';
    orderType = '';
    casePrefix = '';
    prodLine = '';
    pxpCaseId;
    contScheduleId;
    status = '';
    devanningDate: any;
    startDevanningDate: any;
    finishDevanningDate: any;
    zoneCd = '';
    areaCd = '';
    locId;
    locCd = '';
    locDate: any;
    locBy = '';
    shop = '';
    isBigpart = '';
    isActive = '';
    loc = '';
    locDetails = '';

    defaultColDef = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
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
        private _service: LgwMwhCaseDataServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
        // this.frameworkComponents = { agCellButtonComponent: AgCellButtonRendererComponent };
        // this.frameworkComponents = { agCellButtonTransparentComponent: AgCellButtonTransparentRendererComponent };
        this.defaultColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'],
                width: 55,
            },
            {

                headerName: this.l('Case No'),
                headerTooltip: this.l('Case No'),
                field: 'caseNo',
                cellRenderer: 'agCellButtonTransparentComponent',
                cellClass: ['text-center'],
                buttonDefTwo: {
                    useRowData: true,
                    text: params => params.data?.caseNo,
                    function: this.openContactHistoryModal.bind(this),
                },
                width: 90,

            },
            {
                headerName: this.l('Lot No'),
                headerTooltip: this.l('Lot No'),
                field: 'lotNo',
                width: 90
            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                width: 90
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                width: 90
            },
            {
                headerName: this.l('Case Qty'),
                headerTooltip: this.l('Case Qty'),
                field: 'caseQty',
                width: 90,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Container No'),
                headerTooltip: this.l('Container No'),
                field: 'containerNo',
                width: 120
            },
            {
                headerName: this.l('Renban'),
                headerTooltip: this.l('Renban'),
                field: 'renban',
                width: 90
            },
            {
                headerName: this.l('Supplier No'),
                headerTooltip: this.l('Supplier No'),
                field: 'supplierNo',
                width: 90
            },
            {
                headerName: this.l('Order Type'),
                headerTooltip: this.l('Order Type'),
                field: 'orderType',
                width: 90
            },
            {
                headerName: this.l('Case Prefix'),
                headerTooltip: this.l('Case Prefix'),
                field: 'casePrefix',
                width: 90
            },
            {
                headerName: this.l('Prod Line'),
                headerTooltip: this.l('Prod Line'),
                field: 'prodLine',
                width: 90
            },
            {
                headerName: this.l('Pxp Case Id'),
                headerTooltip: this.l('Pxp Case Id'),
                field: 'pxpCaseId',
                width: 120,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Cont Schedule Id'),
                headerTooltip: this.l('Cont Schedule Id'),
                field: 'contScheduleId',
                width: 120,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Status'),
                headerTooltip: this.l('Status'),
                field: 'status',
                width: 90
            },
            {
                headerName: this.l('Devanning Date'),
                headerTooltip: this.l('Devanning Date'),
                field: 'devanningDate', valueGetter: (params) => this.pipe.transform(params.data?.devanningDate, 'dd/MM/yyyy'),
                width: 120
            },
            {
                headerName: this.l('Start Devanning Date'),
                headerTooltip: this.l('Start Devanning Date'),
                field: 'startDevanningDate', valueGetter: (params) => this.pipe.transform(params.data?.startDevanningDate, 'dd/MM/yyyy hh:MM:ss'),
                width: 150
            },
            {
                headerName: this.l('Finish Devanning Date'),
                headerTooltip: this.l('Finish Devanning Date'),
                field: 'finishDevanningDate', valueGetter: (params) => this.pipe.transform(params.data?.finishDevanningDate, 'dd/MM/yyyy hh:MM:ss'),
                width: 150
            },
            {
                headerName: this.l('Zone Cd'),
                headerTooltip: this.l('Zone Cd'),
                field: 'zoneCd',
                width: 90
            },
            {
                headerName: this.l('Area Cd'),
                headerTooltip: this.l('Area Cd'),
                field: 'areaCd',
                width: 90
            },
            {
                headerName: this.l('Loc Id'),
                headerTooltip: this.l('Loc Id'),
                field: 'locId',
                width: 90,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Loc Cd'),
                headerTooltip: this.l('Loc Cd'),
                field: 'locCd',
                width: 90
            },
            {
                headerName: this.l('Loc Date'),
                headerTooltip: this.l('Loc Date'),
                field: 'locDate', valueGetter: (params) => this.pipe.transform(params.data?.locDate, 'dd/MM/yyyy hh:MM:ss'),
                width: 90
            },
            {
                headerName: this.l('Loc By'),
                headerTooltip: this.l('Loc By'),
                field: 'locBy',
                width: 90
            },
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                width: 90
            },
            {
                headerName: this.l('Is Bigpart'),
                headerTooltip: this.l('Is Bigpart'),
                field: 'isBigpart',
                width: 90
            },
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                cellClass: ['text-center'],
                width: 120,
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    // eslint-disable-next-line eqeqeq
                    text: (params: { data: { isActive: string } }) => (params.data?.isActive == 'Y') ? 'Active' : 'Inactive',
                    iconName: 'fa fa-circle',
                    // eslint-disable-next-line eqeqeq
                    className: (params: { data: { isActive: string } }) => (params.data?.isActive == 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            agCellButtonTransparentComponent: AgCellButtonTransparentRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this._dateTimeService.convertToDatetime(this.devanningDateFrom ),
			this._dateTimeService.convertToDatetime(this.devanningDateTo ),
            this.caseNo,
            this.renban,
            this.zoneCd,
            this.areaCd,
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
        // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
            this.caseNo = '',
            this.renban = '',
            this.devanningDateFrom = new Date();
            this.devanningDateTo = new Date();
         // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
            this.zoneCd = '',
            this.areaCd = '',
            this.searchDatas();
    }

    onSelectionMulti(params) {
        let selectedRows = this.gridApi.getSelectedRows();
        let selectedRowsString = '';
        let maxToShow = 5;
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
            let othersCount = selectedRows.length - maxToShow;
            selectedRowsString += ' and ' + othersCount + ' other' + (othersCount !== 1 ? 's' : '');
        }
        (document.querySelector('#selectedRows') as any).innerHTML = selectedRowsString;
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this._dateTimeService.convertToDatetime(this.devanningDateFrom ),
			this._dateTimeService.convertToDatetime(this.devanningDateTo ),
            this.caseNo,
            this.renban,
            this.zoneCd,
            this.areaCd,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => LgwMwhCaseDataDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new LgwMwhCaseDataDto();
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

    // deleteRow(system: LgwMwhCaseDataDto): void {
    //     this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
    //         if (isConfirmed) {
    //             this._service.delete(system.id).subscribe(() => {
    //                 this.callBackDataGrid(this.dataParams!);
    //                 this.notify.success(this.l('SuccessfullyDeleted'));
    //                 this.notify.info(this.l('SuccessfullyDeleted'));
    //             });
    //         }
    //     });
    // }
    exportToExcel(): void {
        this._service.getCaseDataToExcel(
            this._dateTimeService.convertToDatetime(this.devanningDateFrom ),
			this._dateTimeService.convertToDatetime(this.devanningDateTo ),
            this.caseNo,
            this.renban,
            this.zoneCd,
            this.areaCd,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }

    exportWhBigCaseDataToExcel(): void {
        // this._service.getMwLayOutGetActiveAllToExcel()
        //     .subscribe((result) => {
        //         this._fileDownloadService.downloadTempFile(result);
        //     });
    }

    exportRobbingDataToExcel(): void {
        // this._service.getMwLayOutTmcGetActiveAllToExcel()
        //     .subscribe((result) => {
        //         this._fileDownloadService.downloadTempFile(result);
        //     });
    }

    openContactHistoryModal(params: { data: { caseNo: string } }) {
        console.log('Hello');

        this.contactHisModal.show(params.data.caseNo);

    }
}
