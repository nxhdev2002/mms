import { Component, ElementRef, EventEmitter, HostListener, Injector, OnInit, Output, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCpsSapAssetInput, InvCpsSapAssetMasterDto, InvDrmPartListDto, InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { DatePipe } from '@angular/common';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { ceil } from 'lodash-es';
import { finalize } from 'rxjs';
import { DrmPartListComponent } from './drmpartlist.component';

@Component({
    templateUrl: './add-asset-drmpartlist-modal.component.html',
    selector: 'addAssetDrmPartList',
    styleUrls: ['./add-asset-drmpartlist-modal.component.less']
})
export class AddAssetDrmPartListModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('addAssetDrmPartList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    validateColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    frameworkComponents: FrameworkComponent;
    saving: boolean = false;
    isActive: boolean;
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    pipe = new DatePipe('en-US');

    rowData: any = [];
    selectedRow: InvCpsSapAssetMasterDto = new InvCpsSapAssetMasterDto();
    saveSelectedRow: InvCpsSapAssetMasterDto = new InvCpsSapAssetMasterDto();
    partListRowData: InvDrmPartListDto = new InvDrmPartListDto();
    drmPartListId;
    v_assetId;
    rowData1: InvDrmPartListDto = new InvDrmPartListDto();
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
    checkAddAsset: boolean = true;

    constructor(injector: Injector,
        private _service: InvDrmPartListServiceProxy,
        private gridTableService: GridTableService,
        private _component: DrmPartListComponent
    ) {
        super(injector);

        this.validateColDefs = [
            { headerName: this.l('STT'), headerTooltip: this.l('STT'), cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1), cellClass: ['text-center'], width: 55 },
            //  { headerName: this.l('Asset Id'), headerTooltip: this.l('Asset Id'), field: 'id', flex: 1 },
            { headerName: this.l('Main Asset Number'), headerTooltip: this.l('Main Asset Number'), field: 'fixedAssetNumber', flex: 1 },
            { headerName: this.l('Sub Asset Number'), headerTooltip: this.l('Sub Asset Number'), field: 'subAssetNumber', flex: 1 },
            { headerName: this.l('WBS'), headerTooltip: this.l('WBS'), field: 'wbs', flex: 1 },
            { headerName: this.l('Cost Center'), headerTooltip: this.l('Cost Center'), field: 'costCenter', flex: 1 },
            { headerName: this.l('Responsible Cost Center'), headerTooltip: this.l('Responsible Cost Center'), field: 'responsibleCostCenter', flex: 1 },
            { headerName: this.l('Cost Of Asset'), headerTooltip: this.l('Cost Of Asset'), field: 'costOfAsset', flex: 1 },
            // { headerName: this.l('Is Use'), headerTooltip: this.l('Is Use'), field: 'isUse', flex: 1 }
        ];
        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }
    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
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
        // setTimeout(() => {
        //     this.dataParams.columnApi!.sizeColumnsToFit({
        //         suppressColumnVirtualisation: true,
        //     });

        //     this.autoSizeAll();
        // })
    }

    show(rowData, assetId): void {
        this.partListRowData = rowData;
        this.drmPartListId = this.partListRowData.id;
        if (assetId != null) {
            this._service.getViewAsAsset(assetId)
                .subscribe((result) => {
                    this.rowData1 = result.items[0];
                    this.v_assetId = this.partListRowData.assetId != null ? this.partListRowData.assetId : 0;
                });
        }
        this.v_assetId = this.partListRowData.assetId != null ? this.partListRowData.assetId : 0;
        this.checkAddAsset = assetId != null ? true : false;
        this.getSapAsset();
        this.modal.show();
    }

    save(): void {
        this.message.confirm(this.l('Bạn có chắc chắn muốn chọn asset này không? '), 'Are you sure you want to select this asset ?', (isConfirmed) => {
            if (isConfirmed) {
                let input = Object.assign(new InvCpsSapAssetInput(), {
                    currentLineItemId: this.partListRowData.assetId,
                    id: this.saveSelectedRow.id,
                    lineItem: this.saveSelectedRow.fixedAssetNumber,
                    subAssetNumber: this.saveSelectedRow.subAssetNumber,
                    wbs: this.saveSelectedRow.wbs,
                    costCenter: this.saveSelectedRow.costCenter,
                    responsibleCostCenter: this.saveSelectedRow.responsibleCostCenter,
                    costOfAsset: this.saveSelectedRow.costOfAsset,
                    drmPartListId: this.drmPartListId,
                });

                this.saving = true;
                this._service.addAssetDRMPartList(input)
                    .pipe(finalize(() => this.saving = false))
                    .subscribe(() => {
                        this.notify.info(this.l('SavedSuccessfully'));
                        this._component.searchDatas();
                        this.close();
                    });
            }
        });
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getListSapAsset(
            this.v_assetId
        );
    }

    getSapAsset() {
        this.isLoading = true;
        this._service.getListSapAsset(
            this.v_assetId
        ).pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe(result => {
                this.rowData = result;
                this.resetGridView();
                this.isLoading = false;
            });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvCpsSapAssetMasterDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvCpsSapAssetMasterDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
    }

    callBackGrid(params: GridParams) {
        this.dataParams = params;
        params.api.paginationSetPageSize(this.paginationParams.pageSize);
        this.paginationParams.skipCount =
            ((this.paginationParams.pageNum ?? 1) - 1) * (this.paginationParams.pageSize ?? 0);
        this.paginationParams.pageSize = this.paginationParams.pageSize;
        this.resetGridView();
    }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        this.getDatas(this.paginationParams).pipe(finalize(() => {
        })).subscribe((result) => {
            this.rowData = result;
            this.resetGridView();
            this.isLoading = false;
        });
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
