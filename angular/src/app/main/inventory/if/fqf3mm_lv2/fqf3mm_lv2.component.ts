import { GridApi } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel,FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM_LV2Dto, IF_FQF3MM_LV2ServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ViewDetailLV2ModalComponent } from './view-fqf3mm_lv2-modal.component';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    templateUrl: './FQF3MM_LV2.component.html',
})
export class FQF3MM_LV2Component extends AppComponentBase implements OnInit {
    @ViewChild('paginator', { static: true }) paginator: Paginator;
    @ViewChild('viewModalFQF3MM_LV2', { static: true }) viewModalFQF3MM_LV2:| ViewDetailLV2ModalComponent| undefined;
    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 500,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: IF_FQF3MM_LV2Dto = new IF_FQF3MM_LV2Dto();
    saveSelectedRow: IF_FQF3MM_LV2Dto = new IF_FQF3MM_LV2Dto();
    datas: IF_FQF3MM_LV2Dto = new IF_FQF3MM_LV2Dto();
    isLoading: boolean = false;
    dataParams: GridParams | undefined;
    rowData: any[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    filter: string = '';
	pipe = new DatePipe('en-US');
	frameworkComponents: FrameworkComponent;
    selectedIdDetail;

    fileId : string = '' ;
	fileDescription : string = '' ;
	content : string = '' ;
	interfaceDate: any;
	status : string = '' ;

	statusDateTime
	filePath : string = '' ;

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
        private _service: IF_FQF3MM_LV2ServiceProxy,
        private gridTableService: GridTableService,
        private _dateTimeService: DateTimeService,
        private _fileDownloadService: FileDownloadService
    ) {
        super(injector);
        this.defaultColDefs = [
            {headerName: this.l('STT'),headerTooltip: this.l('STT'),cellRenderer: (params) =>params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),cellClass: ['text-center'],width: 55,},
            {headerName: this.l('File Id'),headerTooltip: this.l('File Id'),field: 'fileId',flex: 1},
			{headerName: this.l('File Description'),headerTooltip: this.l('File Description'),field: 'fileDescription',flex: 1},
			{headerName: this.l('Content'),headerTooltip: this.l('Content'),field: 'content',flex: 1,cellRenderer: (params) => { return '<i class="fas fa-user-edit"></i>'; }},
			{headerName: this.l('Interface Date'),headerTooltip: this.l('Interface Date'),field: 'interfaceDate',flex: 1,valueFormatter: (params) => this.pipe.transform(params.data?.interfaceDate, 'dd/MM/yyyy')},
			{headerName: this.l('Status'),headerTooltip: this.l('Status'),field: 'status',flex: 1},
			{headerName: this.l('Status Date Time'),headerTooltip: this.l('Status Date Time'),field: 'statusDateTime',flex: 1,valueFormatter: (params) => this.pipe.transform(params.data?.statusDateTime, 'dd/MM/yyyy HH:mm:ss')},
			{headerName: this.l('File Path'),headerTooltip: this.l('File Path'),field: 'filePath',flex: 1},
        ];
		this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };
    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 500, totalCount: 0 };
    }

    searchDatas(): void {
        this.isLoading = true;
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
			this.fileDescription,
            this._dateTimeService.convertToDatetime(this.interfaceDate),
			this.status,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
        .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
        .subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;
        });
    }

    clearTextSearch() {
		this.fileDescription = '',
		this.interfaceDate = null,
		this.status = '',
        this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.fileDescription,
            this._dateTimeService.convertToDatetime(this.interfaceDate),
			this.status,
			'',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => IF_FQF3MM_LV2Dto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new IF_FQF3MM_LV2Dto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectedIdDetail = this.saveSelectedRow.id;
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
            .pipe(finalize(() => this.gridTableService.selectFirstRow(this.dataParams!.api)))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;
            });
    }

    getViewDetail(){
        if(this.selectedIdDetail != null){
            this.viewModalFQF3MM_LV2.show(this.selectedIdDetail);
        }

    }

    fn: CommonFunction = new CommonFunction();
    getExportContent(e): void {

        this.fn.exportLoading(e, true);

        this._service.getExportToText(
            this.selectedIdDetail
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }

}
