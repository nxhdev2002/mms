import { ColDef, GridApi, GridReadyEvent } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmLookupDto, MstCmmLookupServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { CreateOrEditLookupModalComponent } from './create-or-edit-lookup-modal.component';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { EventBusService } from '@app/shared/services/event-bus.service';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    selector: 'lookup',
    templateUrl: './lookup.component.html',
})
export class LookupComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModalLookup', { static: true }) createOrEditModalLookup: | CreateOrEditLookupModalComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;


    columnDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    // newInvoiceParam: GridParams;

    selectedRow: MstCmmLookupDto = new MstCmmLookupDto();
    saveSelectedRow: MstCmmLookupDto = new MstCmmLookupDto();
    datas: MstCmmLookupDto = new MstCmmLookupDto();
    isLoading: boolean = false;


    dataParams: GridParams | undefined;
    rowData: MstCmmLookupDto[] = [];
    gridApi!: GridApi;
    rowSelection = 'multiple';
    // filter: string = '';
    pipe = new DatePipe('en-US');

    domainCode: string = '';
    itemCode: string = '';
    itemValue: string = '';
    itemOrder: number = 0;
    description: string = '';
    isUse: string = '';
    isRestrict: string = '';
    changedRecordsCmmLookup: number[] = [];
    selectId;
    frameworkComponents: FrameworkComponent;

    defaultColDefs = {
        resizable: true,
        sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) { return null; }
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    constructor(
        injector: Injector,
        private _service: MstCmmLookupServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private eventBus: EventBusService,
    ) {
        super(injector);

        this.columnDefs = [
            {
                headerName: "",
                headerTooltip: "",
                field: "checked",
                headerClass: ["align-checkbox-header"],
                cellClass: ["check-box-center"],
                checkboxSelection: true,
                headerCheckboxSelection: true,
                headerCheckboxSelectionFilteredOnly: true,
                pinned: true,
                width: 37,
            },
            {
                headerName: this.l('STT'),
                headerTooltip: this.l('STT'),
                cellRenderer: (params) => params.rowIndex + 1 + this.paginationParams.pageSize * (this.paginationParams.pageNum - 1),
                cellClass: ['text-center'], width: 60,
                pinned: true,
            },
            {
                headerName: this.l('Domain'),
                headerTooltip: this.l('Domain'),
                // width: 1800,
                children: [
                    {
                        headerName: this.l('Domain'),
                        headerTooltip: this.l('Domain'),
                        field: 'domainCode',
                        flex: 1,  //Test AutoReSize
                      //  editable: true,
                        pinned: true,
                        // rowGroup: true,
                    },
                    {
                        headerName: this.l('Item Code'),
                        headerTooltip: this.l('Item Code'),
                        field: 'itemCode',
                        flex: 1,//Test AutoReSize
                        pinned: true,
                      //  editable: true,
                    }
                ]
            },
            /*
                comparator: this._formStoringService.dateComparator,
                //comparator: +Sử dụng cho export excel mặc định của Gridview
                cellRenderer: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                //cellRenderer: +Format value ĐÚNG
                    //-Gridview Sorting SAI: Datetime/Money
                    //+Sử dụng cho Total pinned bottom
                    //-KHÔNG sử dụng được Sum, Count Gridview cung cấp
                valueGetter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                //valueFormatter: +Format value ĐÚNG
                    //-Gridview Sorting SAI: Datetime/Money
                    //+Sử dụng cho Total pinned bottom
                    //+Sử dụng được Sum, Count Gridview cung cấp
                valueFormatter: (params) => this.pipe.transform(params.data?.declareDate, 'dd/MM/yyyy'),
                //valueFormatter: +Format value ĐÚNG
                    //+Gridview Sorting ĐÚNG: Datetime/Money
                    //-KHÔNG sử dụng được Total pinned bottom
                    //-KHÔNG sử dụng được Sum, Count Gridview cung cấp
            */
            {
                headerName: this.l('Item Value'),
                headerTooltip: this.l('Item Value'),
                field: 'itemValue',
                flex: 1,//Test AutoReSize
                pinned: true,
             //   editable: true,
                cellRenderer: (params) => params.data?.itemValue,
                aggFunc: 'count' //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Order'),
                headerTooltip: this.l('Order'),
                field: 'itemOrder',
                cellRenderer: (params) => params.data?.itemOrder,
                flex: 1,//Test AutoReSize
                type: 'rightAligned',
             //   editable: true,
                aggFunc: 'sum' //sum, min, max, count, avg, first, last
            },
            {
                headerName: this.l('Description'),
                headerTooltip: this.l('Description'),
                field: 'description', editable: true,
                flex: 1//Test AutoReSize
            },
            {
                headerName: this.l('Is Use'),
                headerTooltip: this.l('Is Use'),
                field: 'isUse',
                width: 120,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isUse === 'Y') ? 'isUse' : 'InUse',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isUse === 'Y') ? 'btnActive' : 'btnInActive',
                },
            },
            {
                headerName: this.l('Is Restrict'),
                headerTooltip: this.l('Is Restrict'),
                field: 'isRestrict',
                width: 120,
                cellClass: ['text-center'],
                cellRenderer: 'agCellButtonComponent',
                buttonDefTwo: {
                    text: params => (params.data?.isRestrict === 'Y') ? 'Restrict' : 'InRestrict',
                    iconName: 'fa fa-circle',
                    className: params => (params.data?.isRestrict === 'Y') ? 'btnActive' : 'btnInActive',
                },

            },
        ];


        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
        };

    }

    ngOnInit(): void {
        this.paginationParams = { pageNum: 1, pageSize: 1000000000, totalCount: 0 };
        this.fetchChangedRecords();
    }

    fetchChangedRecords() {
        this._service.getChangedRecords().subscribe((result) => {
            this.changedRecordsCmmLookup = result;
            console.log("result =", result);
        })
    }
    SumFunction() {
        var sum = 0;
        if (this.rowData) {
            for (let i = 0; this.rowData[i]; i++) {
                try { sum += Number(this.rowData[i].itemOrder); }
                catch (ex) { }
            }
        }
        return sum;
    }

    CountItem() {
        var _count = 0;
        if (this.rowData) {
            _count = this.rowData.length;
        }
        return _count;
    }
    createRow(count: number): any[] {
        let result: any[] = [];
        let _sum = this.SumFunction();
        let _count = this.CountItem();
        for (var i = 0; i < count; i++) {
            result.push({

                domainCode: 'Grand Total',

                itemValue: _count,
                itemOrder: _sum,

            });
        }
        return result;
    }

    searchDatas(): void {
        // this.paginator.changePage(this.paginator.getPage());
        this._service.getAll(
            this.domainCode,
            this.itemCode,
            this.itemValue,
            this.itemOrder,
            this.description,
            this.isUse,
            this.isRestrict,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        )
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);

                var rows = this.createRow(1);
                // this.dataParams!.api.setPinnedTopRowData(rows);
                this.dataParams!.api.setPinnedBottomRowData(rows);

                // let lastrow = this.dataParams!.api.getDisplayedRowAtIndex(this.dataParams!.api.getLastDisplayedRow());
                // console.log(lastrow);
                // console.log(lastrow.parent);

            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items;

                // this.dataParams!.api.setPinnedTopRowData(this.rowData[this.rowData.length-1]);

                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                // this.resetGridView();
            });
    }



    clearTextSearch() {
        this.domainCode = '',
            this.itemCode = '',
            this.itemValue = '',
            this.itemOrder = 0,
            this.description = '',
            this.isUse = '',
            this.isRestrict = '',
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

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getAll(
            this.domainCode,
            this.itemCode,
            this.itemValue,
            this.itemOrder,
            this.description,
            this.isUse,
            this.isRestrict,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    // onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmLookupDto[] } }) {
    //     this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmLookupDto();
    //     this.selectedRow = Object.assign({}, this.saveSelectedRow);
    // }

    changePage(paginationParams) {
        this.isLoading = true;
        this.paginationParams = paginationParams;
        //fix: PAGE_COUNT < 0;
        this.paginationParams.skipCount = ((paginationParams.pageNum - 1) * paginationParams.pageSize) < 0 ? 0 : ((paginationParams.pageNum - 1) * paginationParams.pageSize);
        // this.maxResultCount = paginationParams.pageSize;
        this.getDatas(this.paginationParams).subscribe((result) => {
            this.paginationParams.totalCount = result.totalCount;
            this.rowData = result.items;
            this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
            this.isLoading = false;

            // this.resetGridView();
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
            .pipe(finalize(() => {
                this.gridTableService.selectFirstRow(this.dataParams!.api);

                var rows = this.createRow(1);
                // this.dataParams!.api.setPinnedTopRowData(rows);
                this.dataParams!.api.setPinnedBottomRowData(rows);

                // let lastrow = this.dataParams!.api.getDisplayedRowAtIndex(this.dataParams!.api.getLastDisplayedRow());
                // console.log(lastrow);
                // console.log(lastrow.parent);

            }))
            .subscribe((result) => {
                this.paginationParams.totalCount = result.totalCount;
                this.rowData = result.items ?? [];
                this.paginationParams.totalPage = ceil(result.totalCount / (this.paginationParams.pageSize ?? 0));
                this.isLoading = false;

                // this.resetGridView();
            });
    }

    deleteRow(system: MstCmmLookupDto): void {
        this.message.confirm(this.l('AreYouSureToDelete'), 'Delete Row', (isConfirmed) => {
            if (isConfirmed) {
                this._service.delete(system.id).subscribe(() => {
                    this.callBackDataGrid(this.dataParams!);
                    this.notify.success(this.l('SuccessfullyDeleted'));
                    this.notify.info(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }
    // exportToExcel(): void {
    //     this.pending = 'pending';
    //     this.disable = true;

    //     this._service.getLookupToExcel(
    //         this.domainCode,
    //         this.itemCode,
    //         this.itemValue,
    //         this.itemOrder,
    //         this.description,
    //         this.isUse,
    //         this.isRestrict,
    //     )
    //         .subscribe((result) => {
    //             this._fileDownloadService.downloadTempFile(result);
    //             this.pending = '';
    //             this.disable = false;

    //         });
    // }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {

        this.fn.exportLoading(e, true);

        this._service.getLookupToExcel(
            this.domainCode,
            this.itemCode,
            this.itemValue,
            this.itemOrder,
            this.description,
            this.isUse,
            this.isRestrict,
        ).subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));
            }, this.fn.exportLoading(e));

        });
    }





    onChangeRowSelection(params: { api: { getSelectedRows: () => MstCmmLookupDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new MstCmmLookupDto();
        this.selectedRow = Object.assign({}, this.saveSelectedRow);
        this.selectId = this.selectedRow.id;

    }


    onChangeSelection(params) {
        this.selectedRow =
            params.api.getSelectedRows()[0] ?? new MstCmmLookupDto();

        this.selectedRow = Object.assign({}, this.selectedRow);
        this.spinnerService.show();
        // this._service.getInvoiceSearchDetail(this.selectedRow.id,this.statusText)
        //     .pipe(finalize(() => {
        //         this.spinnerService.hide();
        //     }))
        //     .subscribe(res => {
        //         this.listDetailItem = res;
        //     });
    }

    onCellValueChanged() {

    }
    SAVE_ALL() {

        // this.rowData

        // this._service.saveAll(this.rowData)
        // .pipe(finalize(() =>
        //         this.spinnerService.hide()
        //     ))
        //     .subscribe((result) => {
        //         this.notify.info(this.l('SavedSuccessfully'));
        //         this.resetGridView();
        //     });

    }


    /*

        this._mstwptLookupService.createOrEdit(this.rowdata)
        .pipe(finalize(() => this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            this.modalSave.emit(this.rowdata);
        });
        this.saving = false;
    */
    save() {
        // if (!this.checkValidate()) return;
        this.spinnerService.show();
        // this.selectedRow.totalPaymentAmount = this.totalActual;
        // this.selectedRow.invoiceAmountPO = this.totalPOActual;
        // this.selectedRow.invoiceDetailList = this.listItem;
        // this.selectedRow.vendorName = this.vendorList.find(e => e.value == this.selectedRow?.vendorId)?.label;
        // this.selectedRow.vendorNumber = this.vendorList.find(e => e.value == this.selectedRow?.vendorId)?.code;
        // this.selectedRow.currencyCode = this.currencyList.find(e => e.value == this.selectedRow?.currencyId)?.label;
        // this._service.saveInvoice(this.selectedRow)
        //     .pipe(finalize(() => {
        //         if (this.error) {
        //             this.spinnerService.hide();
        //             return;
        //         }
        //         this.spinnerService.hide();
        //         this.closeModel();
        //         this.modalSave.emit(null);
        //     }))
        //     .subscribe(res => {
        //         if (res.includes('Error:') || res.includes('error:')) {
        //             this.notify.error(res);
        //             this.error = true;
        //         }
        //         else {
        //             this.notify.success("Save success");
        //             this.attach.saveAttachFile(res);
        //         }
        //     })
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

        setTimeout(() => {
            this.dataParams.columnApi!.sizeColumnsToFit({
                suppressColumnVirtualisation: true,
            });
            this.autoSizeAll();
        }, 100)
    }

    OpenTab() {
        // window.location.href = "/app/main/home?s=SHOP"
        // get tab code

        this.eventBus.emit({
            type: 'openComponent',
            functionCode: "MASTER_WORKING_PATTERN_SHOP",
            tabHeader: "Shop",
            params: { type: 'lookuup' },
        });
    }

}
