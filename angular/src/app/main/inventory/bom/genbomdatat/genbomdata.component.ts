import { GridApi } from '@ag-grid-enterprise/all-modules';
import { ChangeDetectorRef, Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGenBOMDataDto, InvGenBOMDataServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { finalize } from 'rxjs/internal/operators/finalize';
import { ViewGenBomDataDetailModalComponent } from './view-genbomdata-detail-modal.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './genbomdata.component.html',
})
export class GenBomDataComponent extends AppComponentBase implements OnInit {
    @ViewChild('viewGenBomDataDetailModal', { static: true }) viewGenBomDataDetailModal: | ViewGenBomDataDetailModalComponent | undefined;
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

    selectedRow: InvGenBOMDataDto = new InvGenBOMDataDto();
    saveSelectedRow: InvGenBOMDataDto = new InvGenBOMDataDto();
    datas: InvGenBOMDataDto = new InvGenBOMDataDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;

    typeMPP: string = '';
    periodMpp: any;

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
        private _service: InvGenBOMDataServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Discrimination Sign'), headerTooltip: this.l('Discrimination Sign'), field: 'discriminationSign', flex: 1 },
            { headerName: this.l('Order Cycle'), headerTooltip: this.l('Order Cycle'), field: 'orderCycle', flex: 1 },
            { headerName: this.l('PSC'), headerTooltip: this.l('PSC'), field: 'psc', flex: 1 },
            { headerName: this.l('EDNO'), headerTooltip: this.l('EDNO'), field: 'edno', flex: 1 },
            { headerName: this.l('URN'), headerTooltip: this.l('URN'), field: 'urn', flex: 1 },
            { headerName: this.l('Production SFX'), headerTooltip: this.l('Production SFX'), field: 'productionSFX', flex: 1 },
            { headerName: this.l('Export Type'), headerTooltip: this.l('Export Type'), field: 'exportType', flex: 1 },
            { headerName: this.l('ID Line Code'), headerTooltip: this.l('ID Line Code'), field: 'idLineCode', flex: 1 },
            { headerName: this.l('Production Year'), headerTooltip: this.l('Production Year'), field: 'productionYear', flex: 1 },
            { headerName: this.l('Production Month'), headerTooltip: this.l('Production Month'), field: 'productionMonth', flex: 1 },
            { headerName: this.l('Production Week'), headerTooltip: this.l('Production Week'), field: 'productionWeek', flex: 1 },
            { headerName: this.l('Production Day'), headerTooltip: this.l('ProductionDay'), field: 'productionDay', flex: 1 },
            { headerName: this.l('Frame Type Code'), headerTooltip: this.l('FrameTypeCode'), field: 'frameTypeCode', flex: 1 },
            { headerName: this.l('VIN'), headerTooltip: this.l('VIN'), field: 'vin', flex: 1 },
            { headerName: this.l('WMI'), headerTooltip: this.l('WMI'), field: 'wmi', flex: 1 },
            { headerName: this.l('VDS'), headerTooltip: this.l('VDS'), field: 'vds', flex: 1 },
            { headerName: this.l('Model Year Code'), headerTooltip: this.l('ModelYearCode'), field: 'modelYearCode', flex: 1 },
            { headerName: this.l('VINType'), headerTooltip: this.l('VINType'), field: 'vinType', flex: 1 },
            { headerName: this.l('Stamp Model Code'), headerTooltip: this.l('Stamp Model Code'), field: 'stampModelCode', flex: 1 },
            { headerName: this.l('Frame Sequence Number Digits'), headerTooltip: this.l('Frame Sequence Number Digits'), field: 'frameSequenceNumberDigits', flex: 1 },
            { headerName: this.l('SpecSheet Number'), headerTooltip: this.l('Spec Sheet Number'), field: 'specSheetNumber', flex: 1 },
            { headerName: this.l('KATASHIKI Code'), headerTooltip: this.l('KATASHIKI Code'), field: 'katashikiCode', flex: 1 },
            { headerName: this.l('Display KATASHIKI'), headerTooltip: this.l('Display KATASHIKI'), field: 'displayKATASHIKI', flex: 1 },
            { headerName: this.l('CTL KATASHIKI'), headerTooltip: this.l('CTLKATASHIKI'), field: 'ctlkatashiki', flex: 1 },
            { headerName: this.l('Line Off KATASHIKI Code'), headerTooltip: this.l('Line Off KATASHIKI Code'), field: 'lineOffKATASHIKICode', flex: 1 },
            { headerName: this.l('Line Off KATASHIKI'), headerTooltip: this.l('Line Off KATASHIKI'), field: 'lineOffKATASHIKI', flex: 1 },
            { headerName: this.l('Exterior Code'), headerTooltip: this.l('Exterior Code'), field: 'exteriorCode', flex: 1 },
            { headerName: this.l('Interior Code'), headerTooltip: this.l('Interior Code'), field: 'interiorCode', flex: 1 },
            { headerName: this.l('Production Spec'), headerTooltip: this.l('Production Spec'), field: 'productionSpec', flex: 1 },
            { headerName: this.l('Car Family Code'), headerTooltip: this.l('Car Family Code'), field: 'carFamilyCode', flex: 1 },
            { headerName: this.l('Destination Country Code'), headerTooltip: this.l('Destination Country Code'), field: 'destinationCountryCode', flex: 1 },
            { headerName: this.l('Destination Country Name'), headerTooltip: this.l('Destination Country Name'), field: 'destinationCountryName', flex: 1 },
            { headerName: this.l('KD Lot Code'), headerTooltip: this.l('KD Lot Code'), field: 'kDLotCode', flex: 1 },
            { headerName: this.l('KD Lot No'), headerTooltip: this.l('KD Lot No'), field: 'kdLotNo', flex: 1 },
            { headerName: this.l('KD Sub Type'), headerTooltip: this.l('KD Sub Type'), field: 'kdSubType', flex: 1 },
            { headerName: this.l('PAMS Lot SFX'), headerTooltip: this.l('PAMS Lot SFX'), field: 'pAMSLotSFX', flex: 1 },
            { headerName: this.l('Engine Basic KATASHIKI'), headerTooltip: this.l('EngineBasicKATASHIKI'), field: 'engineBasicKATASHIKI', flex: 1 },
            { headerName: this.l('Motor Basic KATASHIKI'), headerTooltip: this.l('Motor Basic KATASHIKI'), field: 'motorBasicKATASHIKI', flex: 1 },
            { headerName: this.l('Production Lot Size'), headerTooltip: this.l('ProductionLotSize'), field: 'productionLotSize', flex: 1 },
            { headerName: this.l('Maker Code'), headerTooltip: this.l('Maker Code'), field: 'makerCode', flex: 1 },
            { headerName: this.l('Packing Year Month'), headerTooltip: this.l('Packing Year Month'), field: 'packingYearMonth', flex: 1 },
            { headerName: this.l('Vehicle Name Code'), headerTooltip: this.l('Vehicle Name Code'), field: 'vehicleNameCode', flex: 1 },
            { headerName: this.l('Packing Style'), headerTooltip: this.l('Packing Style'), field: 'packingStyle', flex: 1 },
            { headerName: this.l('Destination Destinction'), headerTooltip: this.l('Destination Destinction'), field: 'destinationDestinction', flex: 1 },
            { headerName: this.l('Destination Details'), headerTooltip: this.l('Destination Details'), field: 'destinationDetails', flex: 1 },
            { headerName: this.l('Government Approval'), headerTooltip: this.l('Government Approval'), field: 'governmentApproval', flex: 1 },
            { headerName: this.l('VAR 1'), headerTooltip: this.l('VAR1'), field: 'vAR1', flex: 1 },
            { headerName: this.l('Spec Type'), headerTooltip: this.l('SpecType'), field: 'specType', flex: 1 },
            { headerName: this.l('Plant Code'), headerTooltip: this.l('Plant Code'), field: 'plantCode', flex: 1 },
            { headerName: this.l('Special Sign'), headerTooltip: this.l('Special Sign'), field: 'specialSign', flex: 1 },
            { headerName: this.l('Unit Type'), headerTooltip: this.l('Unit Type'), field: 'unitType', flex: 1 },
            { headerName: this.l('Second Assembly Line'), headerTooltip: this.l('Second Assembly Line'), field: 'secondAssemblyLine', flex: 1 },
            { headerName: this.l('Second Assembly Vehicle Type'), headerTooltip: this.l('Second Assembly Vehicle Type'), field: 'secondAssemblyVehicleType', flex: 1 },
            { headerName: this.l('Off Option Type'), headerTooltip: this.l('Off Option Type'), field: 'offOptionType', flex: 1 },
            { headerName: this.l('Production Request Year Month'), headerTooltip: this.l('ProductionRequestYearMonth'), field: 'productionRequestYearMonth', flex: 1 },
            { headerName: this.l('Distributer Code'), headerTooltip: this.l('Distributer Code'), field: 'distributerCode', flex: 1 },
            { headerName: this.l('Sales SFX'), headerTooltip: this.l('Sales SFX'), field: 'salesSFX', flex: 1 },
            { headerName: this.l('Import Duty Exemption'), headerTooltip: this.l('Import Duty Exemption'), field: 'importDutyExemption', flex: 1 },
            { headerName: this.l('Order No'), headerTooltip: this.l('Order No'), field: 'orderNo', flex: 1 },
            { headerName: this.l('Type Approval Number'), headerTooltip: this.l('Type Approval Number'), field: 'typeApprovalNumber', flex: 1 },
            { headerName: this.l('Dummy'), headerTooltip: this.l('Dummy'), field: 'dummy', flex: 1 },
            { headerName: this.l('Type MPP'), headerTooltip: this.l('Type MPP'), field: 'typeMPP', flex: 1 },
            {
                headerName: this.l('Period Mpp'), headerTooltip: this.l('Period Mpp'), field: 'periodMpp', flex: 1,
                valueGetter: (params) => this.pipe.transform(params.data?.periodMpp, 'dd/MM/yyyy'),
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
        this._service.getAll(
            this.typeMPP,
            this._dateTimeService.convertToDatetime(this.periodMpp),
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
            if (column.getId().toString() != "checked" && column.getId().toString() != "stt") {
                allColumnIds.push(column.getId());
            }
        });
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        this.cdr.detectChanges();
        this.dataParams.columnApi!.sizeColumnsToFit({
            suppressColumnVirtualisation: true,
        });
        this.autoSizeAll();
    }

    clearTextSearch() {
        this.typeMPP = '';
        this.periodMpp = '';
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.typeMPP,
            this._dateTimeService.convertToDatetime(this.periodMpp),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvGenBOMDataDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvGenBOMDataDto();
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

    getViewDataDetail() {
        this.viewGenBomDataDetailModal.show(this.selectedRow);
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getInvGenBOMDataToExcel(
            this.typeMPP,
            this._dateTimeService.convertToDatetime(this.periodMpp)
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
}
