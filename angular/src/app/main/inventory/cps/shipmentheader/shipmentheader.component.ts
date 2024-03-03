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
    InvCpsRcvShipmentHeadersDto,
    InvCpsRcvShipmentHeadersServiceProxy,
    MstCmmGradeColorDetailDto,
    MstCmmLotCodeGradeDto,
    MstCmmLotCodeGradeServiceProxy,
    MstCmmLotCodeGradeTDto,
    MstCmmModelDto,
    MstCmmModelServiceProxy,
} from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppConsts } from '@shared/AppConsts';
import { HttpClient } from '@angular/common/http';
import { FileDownloadService } from '@shared/utils/file-download.service';
import * as moment from 'moment';
import { CommonFunction } from '@app/main/commonfuncton.component';
ModuleRegistry.registerModules([MasterDetailModule]);
@Component({
    templateUrl: './shipmentheader.component.html',
})
export class ShipmentHeaderComponent extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    receiveHeaderColDefs: CustomColDef[] = [];
    receiveLinesColDefs: CustomColDef[] = [];

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
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    //

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



    selectedRow: InvCpsRcvShipmentHeadersDto = new InvCpsRcvShipmentHeadersDto();
    saveSelectedRow: InvCpsRcvShipmentHeadersDto = new InvCpsRcvShipmentHeadersDto();
    modelDto: MstCmmModelDto = new MstCmmModelDto();
    lotCodeGradeDto: MstCmmLotCodeGradeDto = new MstCmmLotCodeGradeDto();
    datas: MstCmmModelDto = new MstCmmModelDto();
    isLoading = false;

    dataParams: GridParams | undefined;
    rowDataModel: any[] = [];
    rowDataLotCodeGrade: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    rowColor: MstCmmGradeColorDetailDto = new MstCmmGradeColorDetailDto();
    p_receipt_num = '';
    p_invetory_group_id = 35;
    p_vendor_id = -1;
    model = '';
    selectId;
    p_from_date;
    p_to_date;

    p_part_no;
    p_po_number;
    p_part_name;
    p_id;
    gradeId;
    ListInventoryGroup: { value: number; label: string }[] = [];
    ListSuppliers: { value: number; label: string }[] = [];

    constructor(
        injector: Injector,
        private _service: InvCpsRcvShipmentHeadersServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _http: HttpClient,
        private _fileDownloadService: FileDownloadService,
    ) {
        super(injector);

        this.receiveHeaderColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamModels.pageSize * (this.paginationParamModels.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
            },
            {
                headerName: this.l('Receipt Number'),
                headerTooltip: this.l('Receipt Number'),
                field: 'receiptNum',
                flex: 1,
                cellRenderer: 'agGroupCellRenderer',
            },
            {
                headerName: this.l('Receive Date'),
                headerTooltip: this.l('Receive Date'),
                field: 'creationTime',
                valueGetter: (params) => this.pipe.transform(params.data?.creationTime, 'dd/MM/yyyy'),
                flex: 1,
            },
            {
                headerName: this.l('Inventory Group'),
                headerTooltip: this.l('Inventory Group'),
                field: 'productgroupname',
                flex: 1,
            },
            {
                headerName: this.l('Vendor Name'),
                headerTooltip: this.l('Vendor Name'),
                field: 'supplierName',
                flex: 1,
            },
        ];

        // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
        this.receiveLinesColDefs = [
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) =>
                    params.rowIndex + 1 + this.paginationParamLots.pageSize * (this.paginationParamLots.pageNum - 1),
                cellClass: ['text-center'],
                width: 65,
            },
            {
                headerName: this.l('PO Number'),
                headerTooltip: this.l('Po Number'),
                field: 'poNumber',
                flex: 1,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1,
            },
            {
                headerName: this.l('Part Name'),
                headerTooltip: this.l('Part Name'),
                field: 'itemDescription',
                flex: 1,
            },
            {
                headerName: this.l('Quantity Shipped'),
                headerTooltip: this.l('Quantity Shipped'),
                field: 'quantityShipped',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Quantity Received'),
                headerTooltip: this.l('Quantity Received'),
                field: 'quantityReceived',
                flex: 1,
                type: 'rightAligned'
            },
            {
                headerName: this.l('Unit'),
                headerTooltip: this.l('Unit'),
                field: 'unitOfMeasure',
                flex: 1,
            },
        ],
            this.frameworkComponents = {
                agCellButtonComponent: AgCellButtonRendererComponent,
            };
    }

    ngOnInit(): void {
        this.paginationParamModels = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.paginationParamLots = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        let date = new Date();
        this.searchTime();
        this.getMasterData();
        this.searchDatas();
    }

    getMasterData() {

        this._service.getShipmentHeaderData()
            .pipe(finalize(() => this.isLoading = false))
            .subscribe((result) => {
                this.ListInventoryGroup.push({ value: -1, label: '' });
                // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
                result.listInventoryGroup.forEach(e => {
                    this.ListInventoryGroup.push({ value: e.id, label: e.productgroupname });
                }),
                    this.ListSuppliers.push({ value: -1, label: '' });
                result.listSuppliers.forEach(e => {
                    this.ListSuppliers.push({ value: e.id, label: e.supplierName });
                });

            });

    }
    searchTime() {
        var date = new Date();
        date.setDate(date.getDate() - 30);
        this.p_from_date = date;
        this.p_to_date = new Date;
    }
    searchDatas(): void {
        this.isLoading = true;
        this._service
            .getAllInvCpsRcvShipmentHeaders(
                this.p_receipt_num,
                this.p_invetory_group_id ?? -1,
                this.p_vendor_id ?? -1,
                this._dateTimeService.convertToDatetime(this.p_from_date),
                this._dateTimeService.convertToDatetime(this.p_to_date),
                this.p_part_no,
                this.p_part_name,
                this.p_po_number,
                '',
                this.paginationParamModels.skipCount,
                this.paginationParamModels.pageSize
            )
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                // eslint-disable-next-line eqeqeq
                if (result.totalCount == 0) {
                    this.rowDataLotCodeGrade = [];
                    this.paginationParamLots.totalCount = result.totalCount;
                    this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
                } else if (result.items[0].id) {
                    this.searchDataReceiveLines(result.items[0].id);
                    this.selectId = result.items[0].id;
                }
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items;
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                this.isLoading=false;
            });
    }

    clearTextSearch() {
        this.searchTime();
        this.p_receipt_num = '',
            this.p_invetory_group_id = -1,
            this.p_vendor_id = -1,
            this.p_part_no = '',
            this.p_po_number = '';
        this.searchDatas();
    }

    getDataReceiveHeader(paginationParams?: PaginationParamsModel) {
        return this._service.getAllInvCpsRcvShipmentHeaders(
            this.p_receipt_num,
            this.p_invetory_group_id ?? -1,
            this.p_vendor_id ?? -1,
            this._dateTimeService.convertToDatetime(this.p_from_date),
            this._dateTimeService.convertToDatetime(this.p_to_date),
            this.p_part_no,
            this.p_part_name,
            this.p_po_number,
            '',
            this.paginationParamModels.skipCount,
            this.paginationParamModels.pageSize
        );
    }

    onChangeRowSelectionModel(params: { api: { getSelectedRows: () => InvCpsRcvShipmentHeadersDto[] } }) {
        const selected = params.api.getSelectedRows()[0];
        if (selected.id) {
            this.searchDataReceiveLines(selected.id);
            this.selectId = selected.id;
        }
        this.selectedRow = Object.assign({}, selected);
    }



    changePageModel(paginationParams) {
        this.isLoading = true;
        this.paginationParamModels = paginationParams;
        this.paginationParamModels.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataReceiveHeader(this.paginationParamModels).subscribe((result) => {
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
        this.getDataReceiveHeader(this.paginationParamModels)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParamModels.totalCount = result.totalCount;
                this.rowDataModel = result.items ?? [];
                this.paginationParamModels.totalPage = ceil(result.totalCount / (this.paginationParamModels.pageSize ?? 0));
                this.isLoading = false;
            });
    }


    //lotCodeGrade

    searchDataReceiveLines(p_Id): void {

        this._service.getAllInvCpsRcvShipmentHeadersDetail(
            p_Id,
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

    getDataReceiveLines(paginationParams?: PaginationParamsModel) {
        return this._service.getAllInvCpsRcvShipmentHeadersDetail(
            this.selectId,
            '',
            this.paginationParamLots.skipCount,
            this.paginationParamLots.pageSize
        );
    }

    onChangeRowSelectionLotCodeGrade(params: { api: { getSelectedRows: () => InvCpsRcvShipmentHeadersDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCpsRcvShipmentHeadersDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    changePageLotCodeGrade(paginationParams) {
        this.isLoading = true;
        this.paginationParamLots = paginationParams;
        this.paginationParamLots.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDataReceiveLines(this.paginationParamLots).subscribe((result) => {
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
        this.getDataReceiveLines(this.paginationParamLots)
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams?.api)))
            .subscribe((result) => {
                this.paginationParamLots.totalCount = result.totalCount;
                this.rowDataLotCodeGrade = result.items ?? [];
                this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
                this.isLoading = false;
            });
    }




    // changePageLotCodeGrade(paginationParams) {
    //     this.isLoading = true;
    //     this.paginationParamLots = paginationParams;
    //     this.paginationParamLots.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
    //     // this.maxResultCount = paginationParams.pageSize;
    //     this.getDataReceiveLines(this.paginationParamLots).subscribe((result) => {
    //         if (result.totalCount == 0) {
    //             this.rowDataLotCodeGrade = [];
    //         } else if (result.items[0].id) {
    //             this.searchDataReceiveLines(result.items[0].id);
    //             this.selectId = result.items[0].id;
    //         }
    //         this.isLoading = false;
    //         this.paginationParamLots.totalCount = result.totalCount;
    //         this.rowDataLotCodeGrade = result.items;
    //         this.paginationParamLots.totalPage = ceil(result.totalCount / (this.paginationParamLots.pageSize ?? 0));
    //     });
    //     this.searchDataReceiveLines(this.selectId);
    // }


    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.shipmentHeader_ExportExcel(
            this.p_receipt_num,
            this.p_invetory_group_id ?? -1,
            this.p_vendor_id ?? -1,
            this._dateTimeService.convertToDatetime(this.p_from_date),
            this._dateTimeService.convertToDatetime(this.p_to_date),
            this.p_part_no,
            this.p_part_name,
            this.p_po_number
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    exportLinesToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.shipmentLines_ExportExcel(
            this.selectId
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}


