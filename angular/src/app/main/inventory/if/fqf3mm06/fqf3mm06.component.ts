import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM06Dto, IF_FQF3MM06ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewFqf3mm06ValidateModalComponent } from './view-fqf3mm06-validate-modal.component';
import { ChangeDetectorRef } from '@angular/core';
import { ViewFqf3mm06ValidateResultModalComponent } from './view-fqf3mm06-validate-result-modal.component';

@Component({
    templateUrl: './fqf3mm06.component.html',
})
export class FQF3MM06Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('ViewFqf3mm06Validate', { static: true }) ViewFqf3mm06Validate: ViewFqf3mm06ValidateModalComponent | undefined;
    @ViewChild('ViewFqf3mm06ValidateResult', { static: true }) ViewFqf3mm06ValidateResult: ViewFqf3mm06ValidateResultModalComponent | undefined;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM06Dto = new IF_FQF3MM06Dto();
    saveSelectedRow: IF_FQF3MM06Dto = new IF_FQF3MM06Dto();
    datas: IF_FQF3MM06Dto = new IF_FQF3MM06Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    createDateFrom;
    createDateTo;
    materialCode: string = '';
    materialDescription: string = '';

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
        private _service: IF_FQF3MM06ServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService : DateTimeService,
        private cdr: ChangeDetectorRef
    ) {
        super(injector);
        this.defaultColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55, },
            { headerName: this.l('Record Id (M)'), headerTooltip: this.l('Record Id (M)'), field: 'recordId', flex: 1 },
            { headerName: this.l('Authorization Group (M)'), headerTooltip: this.l('Authorization Group (M)'), field: 'authorizationGroup', flex: 1 },
            { headerName: this.l('Material Type (O)'), headerTooltip: this.l('Material Type (O)'), field: 'materialType', flex: 1 },
            { headerName: this.l('Material Code (M)'), headerTooltip: this.l('Material Code (M)'), field: 'materialCode', flex: 1 },
            { headerName: this.l('Industry Sector (M)'), headerTooltip: this.l('Industry Sector (M)'), field: 'industrySector', flex: 1 },
            { headerName: this.l('Material Description (M)'), headerTooltip: this.l('Material Description (M)'), field: 'materialDescription', flex: 1 },
            { headerName: this.l('Material Group (O)'), headerTooltip: this.l('Material Group (O)'), field: 'materialGroup', flex: 1 },
            { headerName: this.l('Base Unit Of Measure (M)'), headerTooltip: this.l('Base Unit Of Measure (M)'), field: 'baseUnitOfMeasure', flex: 1 },
            { headerName: this.l('Flag Deletion Plant Level (O)'), headerTooltip: this.l('Flag Deletion Plant Level (O)'), field: 'flagDeletionPlantLevel', flex: 1 },
            { headerName: this.l('Plant (M)'), headerTooltip: this.l('Plant (M)'), field: 'plant', flex: 1 },
            { headerName: this.l('Storage Location (M)'), headerTooltip: this.l('Storage Location (M)'), field: 'storageLocation', flex: 1 },
            { headerName: this.l('Product Group (O)'), headerTooltip: this.l('Product Group (O)'), field: 'productGroup', flex: 1 },
            { headerName: this.l('Product Purpose (O)'), headerTooltip: this.l('Product Purpose (O)'), field: 'productPurpose', flex: 1 },
            { headerName: this.l('Product Type (O)'), headerTooltip: this.l('Product Type (O)'), field: 'productType', flex: 1 },
            { headerName: this.l('Profit Center (O)'), headerTooltip: this.l('Profit Center (O)'), field: 'profitCenter', flex: 1 },
            { headerName: this.l('Batch Management (O)'), headerTooltip: this.l('Batch Management (O)'), field: 'batchManagement', flex: 1 },
            { headerName: this.l('Reserved Stock (O)'), headerTooltip: this.l('Reserved Stock (O)'), field: 'reservedStock', flex: 1 },
            { headerName: this.l('Residue (O)'), headerTooltip: this.l('Residue (O)'), field: 'residue', flex: 1 },
            { headerName: this.l('Lot Code (O)'), headerTooltip: this.l('Lot Code (O)'), field: 'lotCode', flex: 1 },
            { headerName: this.l('Mrp Group (O)'), headerTooltip: this.l('Mrp Group (O)'), field: 'mrpGroup', flex: 1 },
            { headerName: this.l('Mrp Type (O)'), headerTooltip: this.l('Mrp Type (O)'), field: 'mrpType', flex: 1 },
            { headerName: this.l('Procurement Type (M)'), headerTooltip: this.l('Procurement Type (M)'), field: 'procurementType', flex: 1 },
            { headerName: this.l('Special Procurement (O)'), headerTooltip: this.l('Special Procurement (O)'), field: 'specialProcurement', flex: 1 },
            { headerName: this.l('Prod Stor Location (O)'), headerTooltip: this.l('Prod Stor Location (O)'), field: 'prodStorLocation', flex: 1 },
            { headerName: this.l('Repet Manufacturing (M)'), headerTooltip: this.l('Repet Manufacturing (M)'), field: 'repetManufacturing', flex: 1 },
            { headerName: this.l('Rem Profile (M)'), headerTooltip: this.l('Rem Profile (M)'), field: 'remProfile', flex: 1 },
            { headerName: this.l('Do Not Cost (O)'), headerTooltip: this.l('Do Not Cost (O)'), field: 'doNotCost', flex: 1 },
            { headerName: this.l('Variance Key (O)'), headerTooltip: this.l('Variance Key (O)'), field: 'varianceKey', flex: 1 },
            { headerName: this.l('Costing Lot Size (O)'), headerTooltip: this.l('Costing Lot Size (O)'), field: 'costingLotSize', flex: 1 },
            { headerName: this.l('Production Version (O)'), headerTooltip: this.l('Production Version (O)'), field: 'productionVersion', flex: 1 },
            { headerName: this.l('Special Procurement Type (O)'), headerTooltip: this.l('Special Procurement Type (O)'), field: 'specialProcurementType', flex: 1 },
            { headerName: this.l('Valuation Category (O)'), headerTooltip: this.l('Valuation Category (O)'), field: 'valuationCategory', flex: 1 },
            { headerName: this.l('Valuation Type (O)'), headerTooltip: this.l('Valuation Type (O)'), field: 'valuationType', flex: 1 },
            { headerName: this.l('Valuation Class (O)'), headerTooltip: this.l('Valuation Class (O)'), field: 'valuationClass', flex: 1 },
            { headerName: this.l('Price Determination (O)'), headerTooltip: this.l('Price Determination (O)'), field: 'priceDetermination', flex: 1 },
            { headerName: this.l('Price Control (O)'), headerTooltip: this.l('Price Control (O)'), field: 'priceControl', flex: 1 },
            { headerName: this.l('Standard Price (O)'), headerTooltip: this.l('Standard Price (O)'), field: 'standardPrice', flex: 1 },
            { headerName: this.l('Moving Price (O)'), headerTooltip: this.l('Moving Price (O)'), field: 'movingPrice', flex: 1 },
            { headerName: this.l('With Qty Structure (O)'), headerTooltip: this.l('With Qty Structure (O)'), field: 'withQtyStructure', flex: 1 },
            { headerName: this.l('Material Origin (O)'), headerTooltip: this.l('Material Origin (O)'), field: 'materialOrigin', flex: 1 },
            { headerName: this.l('Origin Group (O)'), headerTooltip: this.l('Origin Group (O)'), field: 'originGroup', flex: 1 },
            { headerName: this.l('Basic Data Text (R)'), headerTooltip: this.l('Basic Data Text (R)'), field: 'basicDataText', flex: 1 },
            { headerName: this.l('Katashiki (O)'), headerTooltip: this.l('Katashiki (O)'), field: 'katashiki', flex: 1 },
            { headerName: this.l('Vehicle Control Katashiki (O)'), headerTooltip: this.l('Vehicle Control Katashiki (O)'), field: 'vehicleControlKatashiki', flex: 1 },
            { headerName: this.l('Toyota Or Non Toyota (O)'), headerTooltip: this.l('Toyota Or Non Toyota (O)'), field: 'toyotaOrNonToyota', flex: 1 },
            { headerName: this.l('Category Of Gear (O)'), headerTooltip: this.l('Category Of Gear (O)'), field: 'categoryOfGear', flex: 1 },
            { headerName: this.l('Goshi Car (O)'), headerTooltip: this.l('Goshi Car (O)'), field: 'goshiCar', flex: 1 },
            { headerName: this.l('Series Of Vehicles (O)'), headerTooltip: this.l('Series Of Vehicles (O)'), field: 'seriesOfVehicles', flex: 1 },
            { headerName: this.l('Deliver Power Of Driving Wheels (O)'), headerTooltip: this.l('Deliver Power Of Driving Wheels (O)'), field: 'deliverPowerOfDrivingWheels', flex: 1 },
            { headerName: this.l('Fuel Type (O)'), headerTooltip: this.l('Fuel Type (O)'), field: 'fuelType', flex: 1 },
            { headerName: this.l('Vehicle Name (O)'), headerTooltip: this.l('Vehicle Name (O)'), field: 'vehicleName', flex: 1 },
            { headerName: this.l('Price Unit (M)'), headerTooltip: this.l('Price Unit (M)'), field: 'priceUnit', flex: 1 },
            { headerName: this.l('Maru Code (O)'), headerTooltip: this.l('Maru Code (O)'), field: 'maruCode', flex: 1 },
            { headerName: this.l('Ending Of Record (M)'), headerTooltip: this.l('Ending Of Record (M)'), field: 'endingOfRecord', flex: 1 }
        ];
        this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
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

    searchDatas(): void {
        this.isLoading = true;
        this._service.getAll(
            this.materialCode,
            this.materialDescription,
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo),
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
                this.isLoading = false;
            });
    }

    clearTextSearch() {
        this.materialCode = '',
        this.materialDescription = '',
        this.createDateFrom = '',
        this.createDateTo = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.materialCode,
            this.materialDescription,
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo),
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM06Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM06Dto();
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
            this.resetGridView();
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
                this.resetGridView();
                this.isLoading = false;
            });
    }

    fn: CommonFunction = new CommonFunction();
        exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getFQF3MM06ToExcel(
            this.materialCode,
            this.materialDescription,
            this._dateTimeService.convertToDatetime(this.createDateFrom),
            this._dateTimeService.convertToDatetime(this.createDateTo),
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

    reCreateData(e): void {
        if (this.createDateFrom == null || this.createDateFrom == '') {
            this.message.warn(this.l('Hãy nhập Create Date From trước khi Re-create'), 'Warning');
        } else {
            this.fn.exportLoading(e, true);
            this._service.reCreateDataFQF3MM06(
                this._dateTimeService.convertToDatetime(this.createDateFrom)
            ).subscribe(() => {
                this.fn.exportLoading(e);
                this.notify.success(this.l('Re - Generate thành công!'));
                this.searchDatas();
            });
        }
    }

    viewValidate(){
        this.ViewFqf3mm06Validate.show(this.createDateFrom, this.createDateTo);
    }

    viewValidateData() {
        this.ViewFqf3mm06ValidateResult.show(this.createDateFrom, this.createDateTo);
    }
}
