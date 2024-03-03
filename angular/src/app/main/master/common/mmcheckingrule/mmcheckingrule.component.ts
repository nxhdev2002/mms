import { GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmMMCheckingRuleDto, MstCmmMMCheckingRuleServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './mmcheckingrule.component.html',
})
export class MMCheckingRuleComponent extends AppComponentBase implements OnInit {
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

    changedRecords: number[] =[]; 
    selectId;
    selectedRow: MstCmmMMCheckingRuleDto = new MstCmmMMCheckingRuleDto();
    saveSelectedRow: MstCmmMMCheckingRuleDto = new MstCmmMMCheckingRuleDto();
    datas: MstCmmMMCheckingRuleDto = new MstCmmMMCheckingRuleDto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;

    ruleCode : string = '' ;
	ruleDescription : string = '' ;
	ruleItem : string = '' ;
	fieldName : string = '' ;
    resultfield : string = '';
    isActive: string = '';


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
        private _service: MstCmmMMCheckingRuleServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('Rule Code'),headerTooltip: this.l('Rule Code'),field: 'ruleCode',flex: 1},
			{headerName: this.l('Rule Description'),headerTooltip: this.l('Rule Description'),field: 'ruleDescription',flex: 1},
			{headerName: this.l('Rule Item'),headerTooltip: this.l('Rule Item'),field: 'ruleItem',flex: 1},
			{headerName: this.l('Field 1 Name'),headerTooltip: this.l('Field 1 Name'),field: 'field1Name',flex: 1},
			{headerName: this.l('Field 1 Value'),headerTooltip: this.l('Field 1 Value'),field: 'field1Value',flex: 1},
			{headerName: this.l('Field 2 Name'),headerTooltip: this.l('Field 2 Name'),field: 'field2Name',flex: 1},
			{headerName: this.l('Field 2 Value'),headerTooltip: this.l('Field 2 Value'),field: 'field2Value',flex: 1},
			{headerName: this.l('Field 3 Name'),headerTooltip: this.l('Field 3 Name'),field: 'field3Name',flex: 1},
			{headerName: this.l('Field 3 Value'),headerTooltip: this.l('Field 3 Value'),field: 'field3Value',flex: 1},
			{headerName: this.l('Field 4 Name'),headerTooltip: this.l('Field 4 Name'),field: 'field4Name',flex: 1},
			{headerName: this.l('Field 4 Value'),headerTooltip: this.l('Field 4 Value'),field: 'field4Value',flex: 1},
			{headerName: this.l('Field 5 Name'),headerTooltip: this.l('Field 5 Name'),field: 'field5Name',flex: 1},
			{headerName: this.l('Field 5 Value'),headerTooltip: this.l('Field 5 Value'),field: 'field5Value',flex: 1},
			{headerName: this.l('Option'),headerTooltip: this.l('Option'),field: 'option',flex: 1},
			{headerName: this.l('Resultfield'),headerTooltip: this.l('Resultfield'),field: 'resultfield',flex: 1},
			{headerName: this.l('Expectedresult'),headerTooltip: this.l('Expectedresult'),field: 'expectedresult',flex: 1},
			{headerName: this.l('Checkoption'),headerTooltip: this.l('CheckOption'),field: 'checkOption',flex: 1},
			{headerName: this.l('Errormessage'),headerTooltip: this.l('Errormessage'),field: 'errormessage',flex: 1},
            {
                headerName: this.l('Is Active'),
                headerTooltip: this.l('Is Active'),
                field: 'isActive',
                flex: 1,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'Active'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'InActive'
                        }
                        return '';
                    },
                    iconName: 'fa fa-circle',
                    className: function (params: any) {
                        if (params.data?.isActive == "Y") {
                            return 'btnActive'
                        }
                        else if (params.data?.isActive == "N") {
                            return 'btnInActive'
                        }
                        return 'btnUndefined';
                    }
                },
            }
        ];
		this.frameworkComponents = {

            agCellButtonComponent: AgCellButtonRendererComponent,
        };
        this.resetGridView();
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecords = result;
            console.log("result =", result);
        })
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.ruleCode,
			this.ruleItem,
			this.fieldName,
            this.resultfield,
            this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.resetGridView();
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
        });
    }
    toggleIsActive() {
        this.isActive = this.isActive === 'Y' ? '' : 'Y';
      }
    clearTextSearch() {
        this.ruleCode  = '' ;
        this.ruleItem  = '' ;
        this.fieldName  = '' ;
        this.resultfield = '',
        this.isActive  = '' ;
        this.searchDatas();
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.ruleCode,
			this.ruleItem,
			this.fieldName,
            this.resultfield,
            this.isActive,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmMMCheckingRuleDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmMMCheckingRuleDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;
    }


    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        this.paginationParams.skipCount = (paginationParams.pageNum - 1) * paginationParams.pageSize;
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.resetGridView();
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
                this.resetGridView();
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }



    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getCmmMMCheckingRuleToExcel(
			this.ruleCode,
			this.ruleItem,
			this.fieldName,
            this.resultfield,
            this.isActive,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }
}
