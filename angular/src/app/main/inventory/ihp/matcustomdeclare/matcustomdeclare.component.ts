import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DataFormatService } from '@app/shared/common/services/data-format.service';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { FormStoringService } from '@app/shared/common/services/form-storing.service';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { InvIhpMatCustomDeclareServiceProxy, InvIphMatCustomDeclareDetailsDto, InvIphMatCustomDeclareDto } from '@shared/service-proxies/service-proxies';

@Component({
    templateUrl: './matcustomdeclare.component.html',
})
export class MATCustomsDeclareComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    customerColDefs: CustomColDef[] = [];
    detailsColDefs: CustomColDef[] = [];
    paginationParamsDeclare: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    paginationParamsDetails: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvIphMatCustomDeclareDto = new InvIphMatCustomDeclareDto();
    saveSelectedRow: InvIphMatCustomDeclareDto = new InvIphMatCustomDeclareDto();

    saveSelectedRowDetails: InvIphMatCustomDeclareDetailsDto = new InvIphMatCustomDeclareDetailsDto();
    selectedRowDetails: InvIphMatCustomDeclareDetailsDto = new InvIphMatCustomDeclareDetailsDto();


    datas: InvIphMatCustomDeclareDto = new InvIphMatCustomDeclareDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    dataParamsDetails: GridParams | undefined;

    rowDataDeclare: any[] = [];
    rowDataDeclareDetails: any[] = [];;
    gridApi!: GridApi;
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    id;
    sOTK
    nGAY_DK_FROM
    nGAY_DK_TO
    sO_HDTM
    vAN_DON
    pART_SPEC
    dToKhaiMDID
    nGAY_DK
    tEN_HANG
    PHI_BH
    phI_VC
    p_dToKhaiMDID : any;

    status: string = '';
    ordertypeCode: string = '';
    declareType: string = '';
    isActive: string = '';
    test: number = 0;


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
        private _service: InvIhpMatCustomDeclareServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _fm: DataFormatService,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
        this.customerColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDeclare.pageSize * (this.paginationParamsDeclare.pageNum - 1),
                cellClass: ['text-center'],
                flex: 1
            },
            {
                headerName: this.l('SOTK '),
                headerTooltip: this.l('SOTK'),
                field: 'sotk',
                flex: 1
            },
            {
                headerName: this.l('NGAY_DK'),
                headerTooltip: this.l('NGAY_DK'),
                field: 'ngaY_DK',
                valueFormatter: (params) => this.pipe.transform(params.data?.ngaY_DK, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('VAN_DON'),
                headerTooltip: this.l('VAN_DON'),
                field: 'vaN_DON',
                flex: 1
            },
            {
                headerName: this.l('SOHANG'),
                headerTooltip: this.l('SOHANG'),
                field: 'sohang',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('MA_NT'),
                headerTooltip: this.l('MA_NT'),
                field: 'mA_NT',
                flex: 1,

            },
            {
                headerName: this.l('TYGIA_VND'),
                headerTooltip: this.l('TYGIA_VND'),
                field: 'tygiA_VND',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tygiA_VND, 0),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('TONGTGKB'),
                headerTooltip: this.l('TONGTGKB'),
                field: 'tongtgkb',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tongtgkb, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('PHI_BH'),
                headerTooltip: this.l('PHI_BH'),
                field: 'phI_BH',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.phI_BH, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('PHI_VC'),
                headerTooltip: this.l('PHI_VC'),
                field: 'phI_VC',
                // valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.pHI_VC, 4),
                flex: 1,
                type: 'rightAligned'
            },

            {
                headerName: this.l('SO_KIEN'),
                headerTooltip: this.l('SO_KIEN'),
                field: 'sO_KIEN',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('SO_HDTM'),
                headerTooltip: this.l('SO_HDTM'),
                field: 'sO_HDTM',
                flex: 1
            },
            {
                headerName: this.l('NGAY_HDTM'),
                headerTooltip: this.l('NGAY_HDTM'),
                field: 'ngaY_HDTM',
                valueFormatter: (params) => this.pipe.transform(params.data?.ngaY_HDTM, 'dd/MM/yyyy'),
                flex: 1
            },
            {
                headerName: this.l('TONGTG_HDTM'),
                headerTooltip: this.l('TONGTG_HDTM'),
                field: 'tongtG_HDTM',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tongtG_HDTM, 4),
                flex: 1,
                type: 'rightAligned'

            },
            {
                headerName: this.l('THUE_VAT'),
                headerTooltip: this.l('THUE_VAT'),
                field: 'thuE_VAT',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.thuE_VAT, 0),
                flex: 1,
                type: 'rightAligned'

            },

            {
                headerName: this.l('THUE_XNK'),
                headerTooltip: this.l('THUE_XNK'),
                field: 'thuE_XNK',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.thuE_XNK, 0),
                flex: 1,
                type: 'rightAligned'

            },
            {
                headerName: this.l('TongTienThue'),
                headerTooltip: this.l('TongTienThue'),
                field: 'tongTienThue',
                valueFormatter: (params) => this._fm.formatMoney_decimal(params.data?.tongTienThue, 0),
                flex: 1,
                type: 'rightAligned'

            },


        ];
        this.detailsColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParamsDetails.pageSize * (this.paginationParamsDetails.pageNum - 1),
                cellClass: ['text-center'], width: 55,
            },
            {
                headerName: this.l('STTHANG'),
                headerTooltip: this.l('STTHANG'),
                field: 'stthang',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('PART_SPEC'),
                headerTooltip: this.l('PART_SPEC'),
                field: 'parT_SPEC',
                flex: 1,

            },
            {
                headerName: this.l('LUONG'),
                headerTooltip: this.l('LUONG'),
                field: 'luong',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('DGIA_KB'),
                headerTooltip: this.l('DGIA_KB'),
                field: 'dgiA_KB',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.dgiA_KB, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('TRIGIA_KB'),
                headerTooltip: this.l('TRIGIA_KB'),
                field: 'trigiA_KB',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.trigiA_KB, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('TRIGIA_TT'),
                headerTooltip: this.l('TRIGIA_TT'),
                field: 'tRIGIA_TT',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.trigiA_TT, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('TS_XNK'),
                headerTooltip: this.l('TS_XNK'),
                field: 'tS_XNK',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('TS_VAT'),
                headerTooltip: this.l('TS_VAT'),
                field: 'tS_VAT',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('THUE_XNK'),
                headerTooltip: this.l('THUE_XNK'),
                field: 'thuE_XNK',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thuE_XNK, 4),
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('THUE_VAT'),
                headerTooltip: this.l('THUE_VAT'),
                field: 'thuE_VAT',
                valueGetter: (params) => this._fm.formatMoney_decimal(params.data?.thuE_VAT, 4),
                flex: 1,
                type: 'rightAligned'
            },


        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.fn.setHeight_notFullHeight();
    }

    ngOnInit(): void {
        this.paginationParamsDeclare = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamsDetails = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    autoSize() {
        const allColumnIds: string[] = [];
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }
    autoSizeDetails() {
        const allColumnIds: string[] = [];
        this.dataParamsDetails.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParamsDetails.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSize();
            this.autoSizeDetails();
        }, 1000)
    }



    searchDatas(): void {
        this._service.getAllCustomDeclare(
            this.sOTK,
            this._dateTimeService.convertToDatetime(this.nGAY_DK_FROM),
            this._dateTimeService.convertToDatetime(this.nGAY_DK_TO),
            this.sO_HDTM,
            this.vAN_DON,
            this.pART_SPEC,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsDeclare.totalCount = result.totalCount;
                this.rowDataDeclare = result.items;
                this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
                this.isLoading = false;
            });
    }
    searchDatasDetails(p_dToKhaiMDID): void {

        this._service.getCustomDeclareDetails(
            p_dToKhaiMDID,
            this.pART_SPEC,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        )
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDeclareDetails = result.items;
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
            });
    }

    clearTextSearch() {
            this.sOTK = '',
            this.nGAY_DK_FROM = '',
            this.nGAY_DK_TO = '',
            this.sO_HDTM = '',
            this.vAN_DON = '',
            this.tEN_HANG = '',
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

    getAllCustomsDeclare(paginationParams?: PaginationParamsModel) {
        return this._service.getAllCustomDeclare(
            this.sOTK,
            this._dateTimeService.convertToDatetime(this.nGAY_DK_FROM),
            this._dateTimeService.convertToDatetime(this.nGAY_DK_TO),
            this.sO_HDTM,
            this.vAN_DON,
            this.pART_SPEC,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        );
    }

    getCustomDeclareDetails(paginationParams?: PaginationParamsModel) {
        return this._service.getCustomDeclareDetails(
            this.p_dToKhaiMDID,
            this.pART_SPEC,
            '',
            this.paginationParamsDetails.skipCount,
            this.paginationParamsDetails.pageSize
        );
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParamsDeclare = paginationParams;
        this.paginationParamsDeclare.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getAllCustomsDeclare(this.paginationParamsDeclare).subscribe((result) => {
            this.paginationParamsDeclare.totalCount = result.totalCount;
            this.rowDataDeclare = result.items;
            this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    changePageDetails(paginationParams) {
        this.paginationParamsDetails = paginationParams;
        this.paginationParamsDetails.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getCustomDeclareDetails(this.paginationParamsDetails).subscribe((result) => {
            this.paginationParamsDetails.totalCount = result.totalCount;
            this.rowDataDeclareDetails = result.items;
            this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
        });
    }

    // onChangeRowSelection(params: { api: { getSelectedRows: () => InvIphMatCustomDeclareDto[] } }) {

    //     const selected = params.api.getSelectedRows()[0];
    //     if (selected) {
    //         this.p_dToKhaiMDID = selected.dToKhaiMDID;
    //         this.searchDatasDetails(selected.dToKhaiMDID);
    //     }
    //     this.selectedRow = Object.assign({}, selected);

    // }
    onChangeRowSelection(params: { api: { getSelectedRows: () => InvIphMatCustomDeclareDto[] } }) {

        const selected = params.api.getSelectedRows()[0];
        if (selected) {
            this.p_dToKhaiMDID = selected.dToKhaiMDID;
            this.searchDatasDetails(selected.dToKhaiMDID);
        }
        this.selectedRow = Object.assign({}, selected);

    }

    onChangeRowSelectionDetails(params: { api: { getSelectedRows: () => InvIphMatCustomDeclareDetailsDto[] } }) {
        const selectedTable2 = params.api.getSelectedRows()[0];
        this.selectedRowDetails = Object.assign({}, selectedTable2);
    }

    callBackDataGrid(params: GridParams) {
        this.isLoading = true;
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParamsDeclare.pageSize);
        this.paginationParamsDeclare.skipCount =
            ((this.paginationParamsDeclare.pageNum ?? 1) - 1) * (this.paginationParamsDeclare.pageSize ?? 0);
        this.paginationParamsDeclare.pageSize = this.paginationParamsDeclare.pageSize;
        this.getAllCustomsDeclare(this.paginationParamsDeclare)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParamsDeclare.totalCount = result.totalCount;
                this.rowDataDeclare = result.items ?? [];
                this.paginationParamsDeclare.totalPage = ceil(result.totalCount / (this.paginationParamsDeclare.pageSize ?? 0));
                this.isLoading = false;
            });
    }
    callBackDataGridDetails(params: GridParams) {
        this.dataParamsDetails = params;
        params.api.paginationSetPageSize(this.paginationParamsDetails.pageSize);
        this.paginationParamsDetails.skipCount =
            ((this.paginationParamsDetails.pageNum ?? 1) - 1) * (this.paginationParamsDetails.pageSize ?? 0);
        this.paginationParamsDetails.pageSize = this.paginationParamsDetails.pageSize;
        this.getCustomDeclareDetails(this.paginationParamsDetails)
            .subscribe((result) => {
                this.paginationParamsDetails.totalCount = result.totalCount;
                this.rowDataDeclareDetails = result.items ?? [];
                this.paginationParamsDetails.totalPage = ceil(result.totalCount / (this.paginationParamsDetails.pageSize ?? 0));
            });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getInvIphMatCustomerDeclareToExcel(
            this.sOTK,
            this._dateTimeService.convertToDatetime(this.nGAY_DK_FROM),
            this._dateTimeService.convertToDatetime(this.nGAY_DK_TO),
            this.sO_HDTM,
            this.vAN_DON,
            this.pART_SPEC,
            '',
            this.paginationParamsDeclare.skipCount,
            this.paginationParamsDeclare.pageSize
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }


    exportDetailsToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getInvIphMatCustomerDeclareDetailsToExcel(
            this.p_dToKhaiMDID,
            this.pART_SPEC
        )
            .subscribe((result) => {
                setTimeout(() => {
                    this._fileDownloadService.downloadTempFile(result);
                    this.notify.success(this.l('Download Excel Successfully'));
                }, this.fn.exportLoading(e));
            });
    }


}

