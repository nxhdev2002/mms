import { CellClassParams, CellValueChangedEvent, ColDef, EditableCallbackParams, GridApi, GridOptions, RowNode, ValueGetterParams } from '@ag-grid-enterprise/all-modules';
import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { CustomColDef, GridParams, PaginationParamsModel, FrameworkComponent } from '@app/shared/common/models/base.model';
import { GridTableService } from '@app/shared/common/services/grid-table.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvDrmStockPartExcelDetailDto,  InvDrmStockPartExcelServiceProxy, MstCmmLookupDto, MstCmmLookupServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/internal/operators/finalize';
import { DatePipe } from '@angular/common';
import { FileDownloadService } from '@shared/utils/file-download.service';
import ceil from 'lodash-es/ceil';
import { Paginator } from 'primeng/paginator';

import { ImportDrmStockPartExcelComponent } from './import-drmstockpartexcel.component';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgCellTextRendererComponent } from '@app/shared/common/grid/ag-cell-text-renderer/ag-cell-text-renderer.component';


@Component({
    selector: 'drmstockpartexcel',
    templateUrl: './drmstockpartexcel.component.html',
    styleUrls: ['./drmstockpartexcel.component.less']
})
export class DrmStockPartExcelComponent extends AppComponentBase implements OnInit {
    @ViewChild('importExcelModal', { static: true }) importExcelModal: | ImportDrmStockPartExcelComponent | undefined;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    defaultColDefs: CustomColDef[] = [];
    paginationParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 1000000000,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    selectedRow: InvDrmStockPartExcelDetailDto = new InvDrmStockPartExcelDetailDto();
    saveSelectedRow: InvDrmStockPartExcelDetailDto = new InvDrmStockPartExcelDetailDto();
    datasEdit: InvDrmStockPartExcelDetailDto[] = [];
    valueChange: string ="";
    columnChange: string ="";
    isLoading: boolean = false;
    pending: string = '';
    disable: boolean = false;

    rowData: any[] = [];
    rowDataUpdate: any[] = [];
    lookupData: MstCmmLookupDto[] = [];

    dataParams: GridParams | undefined;
    gridApi: GridApi | undefined;
    gridColumnApi: any;

    rowSelection = 'multiple';
    filter: string = '';
    pipe = new DatePipe('en-US');
    frameworkComponents: FrameworkComponent;
    workingDate: any;
    Model: string = '';
    PartCode: string = '';
    MaterialCode: string = '';
    PartNo: string = '';


    gridOptions: GridOptions = {
        columnTypes: {
          editableColumn: {

            editable: (params: EditableCallbackParams) => { return this.isCellEditable(params); },
            valueParser: 'Number(newValue)',
            cellRenderer: 'agAnimateShowChangeCellRenderer',
            filter: 'agNumberColumnFilter',

            cellClass: (params)  => { return this.cellEditGetsClass(params); }, //['cell-edit','number-cell', 'cell-edit-disable'];
            // cellEditor: 'syncValidateValueSetter',
            valueGetter: (params) => { return this.syncValidateValueGetter(params) },
            valueSetter: (params) => { return this.syncValidateValueSetter(params) },
          },
          valueColumn: {
            editable: true,
            valueParser: 'Number(newValue)',
            cellClass: 'number-cell',
            cellRenderer: 'agAnimateShowChangeCellRenderer',
            filter: 'agNumberColumnFilter',
          },
        },
      };

    defaultColDef = {
        resizable: true,
        sortable: true, enableGroupEdit: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        filter: true,
        floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
        autoGroupColumnDef: {
        minWidth: 400,
        cellRendererParams: {
            suppressCount: true,
            checkbox: false,
        },
        pinned:true,
        },
        suppressAggFuncInHeader: true,
        enableCellChangeFlash: true,
        animateRows: true,
        // onCellValueChanged: this.onCellValueChanged,
    };

    // autoGroupColumnDef: ColDef = {
    //     // headerValueGetter: (params) => `${params.colDef.headerName} Group Column`,
    //     minWidth: 220,
    //     cellRendererParams: {
    //       suppressCount: true,
    //       checkbox: false,
    //     },
    //   };

    constructor(
        injector: Injector,
        private _service: InvDrmStockPartExcelServiceProxy,
        private _servicelookup: MstCmmLookupServiceProxy,
        private gridTableService: GridTableService,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);

        this.workingDate =new Date(); //new Date(2023,6,7); //
        this.getDatasLookup();

        this.frameworkComponents = {
            agCellButtonComponent: AgCellButtonRendererComponent,
            AgCellTextComponent: AgCellTextRendererComponent,
        };
    }
    ngOnInit(): void {

     }

    col_init() {
        let days = new Date(this.d.getFullYear(), this.d.getMonth()+1, 0).getDate();

        this.defaultColDefs = [
            {   field: 'model', rowGroupIndex:1, rowGroup: true, hide: true, flex: 1,
                // cellRenderer: 'AgCellTextComponent', cellClass: ['cell-text-render'],
                // textCellDef: {
                //     text: params => (params.data?.model),
                //     // iconName: 'fa fa-circle',
                //     className: params => this.cellGetsClassHeader(params.data?.itemOrder),
                // },
            },
            {  field: 'partCode', headerName: this.l('Part Code'), headerTooltip: this.l('Part Code'), pinned:true, width: 70,
                cellRenderer: 'AgCellTextComponent', cellClass: ['cell-text-render'],
                textCellDef: {
                    text: params => (params.data?.partCode),
                    // iconName: 'fa fa-circle',
                    className: params => this.cellGetsClassHeader(params.data?.itemOrder),
                },
            },
            {  field: 'partNo', headerName: this.l('Part No'), headerTooltip: this.l('Part No'), pinned:true, width: 70,
                cellRenderer: 'AgCellTextComponent', cellClass: ['cell-text-render'],
                textCellDef: {
                    text: params => (params.data?.partNo),
                    // iconName: 'fa fa-circle',
                    className: params => this.cellGetsClassHeader(params.data?.itemOrder),
                },
            },
            {  field: 'grade', headerName: this.l('Grade'), headerTooltip: this.l('Grade'), pinned:true, width: 70,
                cellRenderer: 'AgCellTextComponent', cellClass: ['cell-text-render'],
                textCellDef: {
                    text: params => (params.data?.grade),
                    // iconName: 'fa fa-circle',
                    className: params => this.cellGetsClassHeader(params.data?.itemOrder),
                },
            },

             { field: 'itemCode', headerName: this.l('Item Code'), headerTooltip: this.l('Item Code'), pinned:true,
                cellRenderer: 'AgCellTextComponent', cellClass: ['cell-text-render'],
                textCellDef: {
                    text: params => (params.data?.itemCode),
                    // iconName: 'fa fa-circle',
                    className: params => this.cellGetsClass(params.data?.itemOrder),
                },
             },
            //  this.column(),
            this.columnDay(1, days),
            this.columnDay(2, days),
            this.columnDay(3, days),
            this.columnDay(4, days),
            this.columnDay(5, days),
            this.columnDay(6, days),
            this.columnDay(7, days),
            this.columnDay(8, days),
            this.columnDay(9, days),
            this.columnDay(10, days),
            this.columnDay(11, days),
            this.columnDay(12, days),
            this.columnDay(13, days),
            this.columnDay(14, days),
            this.columnDay(15, days),
            this.columnDay(16, days),
            this.columnDay(17, days),
            this.columnDay(18, days),
            this.columnDay(19, days),
            this.columnDay(20, days),
            this.columnDay(21, days),
            this.columnDay(22, days),
            this.columnDay(23, days),
            this.columnDay(24, days),
            this.columnDay(25, days),
            this.columnDay(26, days),
            this.columnDay(27, days),
            this.columnDay(28, days),
            this.columnDay(29, days),
            this.columnDay(30, days),
            this.columnDay(31, days),
         ];
    }

    d = new Date();
    day_shift = 0;
    column() {
        let monthheader =  "Tháng " + (this.d.getMonth()+1) + " Năm " + this.d.getFullYear();
        let days = new Date(this.d.getFullYear(), this.d.getMonth()+1, 0).getDate();

        return {
            headerName: monthheader,
            headerTooltip: monthheader,
            children: [
                this.columnDay(1, days),
                this.columnDay(2, days),
                this.columnDay(3, days),
                this.columnDay(4, days),
                this.columnDay(5, days),
                this.columnDay(6, days),
                this.columnDay(7, days),
                this.columnDay(8, days),
                this.columnDay(9, days),
                this.columnDay(10, days),
                this.columnDay(11, days),
                this.columnDay(12, days),
                this.columnDay(13, days),
                this.columnDay(14, days),
                this.columnDay(15, days),
                this.columnDay(16, days),
                this.columnDay(17, days),
                this.columnDay(18, days),
                this.columnDay(19, days),
                this.columnDay(20, days),
                this.columnDay(21, days),
                this.columnDay(22, days),
                this.columnDay(23, days),
                this.columnDay(24, days),
                this.columnDay(25, days),
                this.columnDay(26, days),
                this.columnDay(27, days),
                this.columnDay(28, days),
                this.columnDay(29, days),
                this.columnDay(30, days),
                this.columnDay(31, days),
            ]
        };
    }

    columnDay(_d, _days) {
        let isHide = (_d <= _days)? false: true;
        let _header_d = (_d.toString().length < 2) ? "0" + _d: _d;
        let _field_d = (_d.toString().length < 2) ? "n0" + _d: "n" + _d;
        if(this.lookupData) if (this.lookupData.length > 0)  this.day_shift = Number(this.lookupData[0].itemValue);

        return  {   headerName: _header_d,
                        headerTooltip: _header_d,
                        hide: isHide,
                        children: [
                            {
                                headerName: this.l('Ca 1'), field: _field_d +  '_Ca1',  headerClass: ["headerClass_ca1"],
                                headerTooltip: this.l('Ca 1'), width: 70, hide: (this.day_shift >= 1)? false: true,
                                type: 'editableColumn',
                            },
                            {
                                headerName: this.l('Ca 2'), field: _field_d +  '_Ca2',  headerClass: ["headerClass_ca2"],
                                headerTooltip: this.l('Ca 2'), width: 70,   hide: (this.day_shift >= 2)? false: true,
                                type: 'editableColumn',
                            },
                            {
                                headerName: this.l('Ca 3'), field: _field_d +  '_Ca3',  headerClass: ["headerClass_ca3"],
                                headerTooltip: this.l('Ca 3'), width: 70,  hide: (this.day_shift >= 3)? false: true,
                                type: 'editableColumn',
                            },
                        ]
                    };
    }

    syncValidateValueGetter(params) {

        try{
            // console.log('syncValidateValueGet');
            // let data = params.data;

            if(params.data == undefined) return '';

            let field = params.colDef.field;
            let v = params.data[field];
            // console.log(Number.isNaN(v));
            if(v == null || v== undefined || Number.isNaN(v) || v == '0') {    // fail
                // console.log(params);
                // params.data[field] = '';
                // params.api.applyTransaction({ update: [params.data] });
                return '';
            }
            else {   //hợp lệ
                // params.data[field] = params.newValue;
                //   params.api.applyTransaction({ update: [params.data] });
                return v;
            }
        }catch(e){
                console.log(e);
                console.log(params);
                console.log(params.data[params.colDef.field]);
                return '';
        }
     }

     syncValidateValueSetter(params){
        try{
                // if (!this.processNoUpdate) console.log('syncValidateValueSetter');


                // console.log(params);
                if(params.data == undefined ||  params.data == null) return '';

                let field = params.colDef.field;
                let v = params.newValue;

                // if(this.processNoUpdate) console.log('Setter update: ' + params.oldValue + ' > ' + params.newValue);
                // if(this.processNoUpdate) console.log(params);

                // console.log(Number.isNaN(v));
                // console.log(!this.fn.isInt(params.newValue));
                // if(this.fn.isInt(params.newValue)) {    // fail
                if(v == null || v== undefined || Number.isNaN(v) ) {

                    params.data[field] = 0;
                    params.api.applyTransaction({ update: [params.data] });

                    return '';
                }
                else {  //hợp lệ

                    params.newValue = ((params.newValue) < 0) ? 0: params.newValue;

                    params.data[field] = params.newValue;
                    params.api.applyTransaction({ update: [params.data] });

                    if (params.oldValue != undefined && params.oldValue != null && params.oldValue != "" &&  Number.isNaN(params.newValue) != true && params.newValue == '0' && this.processNoUpdate !=  true) {
                        // console.log(params);

                        this.datasEdit.push(params.data);
                        this.valueChange = '0';
                        this.columnChange = field;
                        this.SaveChange();
                    }

                    return params.newValue;
                }

        } catch(e) {
            console.log(e);
            console.log(params);
            console.log(params.data[params.colDef.field]);
            return '';
        }

     }

     onCellValueChanged(params: CellValueChangedEvent) {

        // if (!this.processNoUpdate) { console.log('onCellValueChanged');  console.log(params); }
        // if(this.processNoUpdate) console.log('Changed update: ' + params.oldValue + ' > ' + params.newValue);
        // if(this.processNoUpdate) console.log(params);

        params.api.applyTransaction({ update:  [params.data] });

        if(params.newValue == null || Number.isNaN(params.newValue) || params.newValue == undefined) return;

        let ischange = true;
        this.datasEdit.forEach((item) => {  // kiểm tra row change đã tồn tại trong mảng edit chưa
            if(item.keyRow == params.data.keyRow)  ischange = false;
        });
        if(ischange && this.processNoUpdate !=true) {
            this.datasEdit.push(params.data);
            this.valueChange = params.newValue;
            this.columnChange = params.column.getId();
            this.SaveChange();
        }
    }

    isCellEditable(params: EditableCallbackParams | CellClassParams) {
        // return params.data.year === 2000;
        if(params.data.itemOrder  == 8 ||
        params.data.itemOrder  == 3 ||
        params.data.itemOrder  == 6)  return false;
        else return true;
    }

    cellEditGetsClass(params)  {
        // console.log(params);
        if(params.data == undefined || params.data == null)  return ['cell-edit','number-cell', ];
        else if(params.data.itemOrder  == 8 ||
                    params.data.itemOrder  == 3 ||
                    params.data.itemOrder  == 6) return ['cell-edit','number-cell', 'cell-edit-disable' ];
        else if(params.data.itemOrder  == 1) return ['cell-edit','number-cell', 'cell-edit-itemOrder1', 'cell-edit-edited' ];
        else return ['cell-edit','number-cell', 'cell-edit-edited' ];
    }

    headerEditClass(params)  {
        // console.log(params);
        if(params.data == undefined || params.data == null)  return [''];
        else if(params.data.itemOrder  == 8 ||
                    params.data.itemOrder  == 3 ||
                    params.data.itemOrder  == 6) return ['cell-edit','number-cell', 'cell-edit-disable' ];
        else if(params.data.itemOrder  == 1) return ['cell-edit','number-cell', 'cell-edit-itemOrder1', 'cell-edit-edited' ];
        else return ['cell-edit','number-cell', 'cell-edit-edited' ];
    }

    cellGetsClass(itemOrder){
        if(itemOrder) {
            switch(itemOrder) {
                case '1': return 'item_code_1' //{ backgroundColor: '#FFFB8F' };
                case '2': return  'item_code_2' //{ backgroundColor: '#82FFB4' };
                case '3': return  'item_code_3' //{ backgroundColor: '#FF9DD2' };
                case '4': return  'item_code_4' //{ backgroundColor: '#CBB9FF' };
                case '5':  return  'item_code_5' //{ backgroundColor: '#8DEEFF' };
                case '6': return  'item_code_6' //{ backgroundColor: '#D1A691' };
                case '7': return  'item_code_7' //{ backgroundColor: '#EB4D22' };
                case '8': return  'item_code_8' //{ backgroundColor: '#0FD5E8' };
                case '9': return  'item_code_9' //{ backgroundColor: '#9BAB4B' };
                case '10': return 'item_code_10'; //lightBlue
                default: return  '';
            }
        }
        else return '';
    }

    cellGetsClassHeader(itemOrder){
        if(itemOrder) {
            switch(itemOrder) {
                case '1': return 'itemOrder1' //{ backgroundColor: '#FFFB8F' };
                default: return  '';
            }
        }
        else return '';
    }

    autoSizeAll() {
        const allColumnIds: string[] = [];
        // console.log(this.dataParams.columnApi!.getRowGroupColumns())
        // console.log(this.dataParams.columnApi!.getAllColumns())
        this.dataParams.columnApi!.getAllColumns()!.forEach((column) => {
            if (column.getId().toString() != "checked" &&
                column.getId().toString() != "stt"  ) {
                allColumnIds.push(column.getId());
            }
        });
        // this.dataParams.columnApi!.autoSizeColumns(this.dataParams.columnApi!.getRowGroupColumns(),false);
        this.dataParams.columnApi!.autoSizeColumns(allColumnIds);
    }

    resetGridView() {
        // setTimeout(() => {
        //     // this.dataParams.api!.sizeColumnsToFit();
        //     this.gridOptions.columnApi!.sizeColumnsToFit({
        //         // cellDataType: false,
        //         suppressColumnVirtualisation: true,
        //         // defaultMinWidth: 50,
        //         // columnLimits: [{ key: 'Model',}],
        //     });

        //     this.autoSizeAll();
        // }, 1000);
    }

    searchDatas(): void {
        this.isLoading = true;
        this._service.getInvDrmStockPartExcelDetailSearch(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.Model,
            this.PartCode,
            this.MaterialCode,
            this.PartNo,
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
        this.Model = "";
        this.PartCode = "";
        this.MaterialCode = "";
        this.PartNo = "";
        this.workingDate = new Date(),
            this.searchDatas();
    }

    getDatas(paginationParams?: PaginationParamsModel) {
        return this._service.getInvDrmStockPartExcelDetailSearch(
            this._dateTimeService.convertToDatetime(this.workingDate),
            this.Model,
            this.PartCode,
            this.MaterialCode,
            this.PartNo,
            '',
            this.paginationParams.skipCount,
            this.paginationParams.pageSize
        );
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => InvDrmStockPartExcelDetailDto[] } }) {
        this.saveSelectedRow = params.api.getSelectedRows()[0] ?? new InvDrmStockPartExcelDetailDto();
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
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
        console.log(this.dataParams);
        console.log(this.gridApi);
        console.log(this.gridColumnApi);


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

    getDatasLookup() {
        //DRM_EXPORT_DAY_SHIFT -gridview-edit - phuongdv
        //DRM_EXPORT_DAY_MONTH
        //DRM_EXPORT_COLUMN
        //              (8)Material Cuối ngày = hôm trc + (9) + (10) -(2)
        //              (3)Ihp Oh / (Stock dập)= hôm trc + (2) - (4) - (5)
        //              (6)Hand Oh / (Stock hand)= hôm trc + (5) - (1) - (7)
        this._servicelookup.getsByDomainCode("DRM_EXPORT_DAY_SHIFT")
        .subscribe((result) => {
            if(result) {
                this.lookupData = result ?? [];
                this.col_init();
            }
        });
    }

    fn: CommonFunction = new CommonFunction();
    exportToExcel(e): void {
        this.fn.exportLoading(e, true);
        this._service.getExportStockPartExcelToExcel(
            this._dateTimeService.convertToDatetime(this.workingDate)
        )
        .subscribe((result) => {
            setTimeout(() => {
                this._fileDownloadService.downloadTempFile(result);
                this.notify.success(this.l('Download Excel Successfully'));},
                this.fn.exportLoading(e));
        });
    }

    SaveChange() {
        // this.isLoading = true
        if(this.datasEdit) {
            if(this.datasEdit.length > 0) {
                // console.log(this.datasEdit);
                this._service.getInvDrmStockPartStockUpdateTrans(
                    this._dateTimeService.convertToDatetime(this.workingDate),
                    this.datasEdit[0].partId,
                    this.datasEdit[0].drmPartListId,
                    this.datasEdit[0].itemOrder,
                    this.columnChange,
                    this.valueChange
                )
                .subscribe((result) => {
                    // console.log('số bản ghi updated:' + result)
                    // console.log(result);

                    this.rowDataUpdate = result ?? [];
                    if(this.rowDataUpdate.length > 0) this.UpdateRow();

                    this.notify.success(this.l('SavedSuccessfully'));
                    this.datasEdit =  [];
                    this.valueChange = "";
                    this.columnChange = "";
                });
            }
        }
    }


    processNoUpdate: boolean = false;
    UpdateRow() {

        let keyRow8 = this.rowDataUpdate[0].keyRow + "_8";  //Qty
        let keyRow6 = this.rowDataUpdate[0].keyRow + "_6";  //IhpOh
        let keyRow3 = this.rowDataUpdate[0].keyRow + "_3";  //HandOh

        // if(this.rowDataUpdate.length > 0)
        let rowNode8;   //Qty
        let rowNode6;   //IhpOh
        let rowNode3;    //HandOh
        let allItems: RowNode[] = [];

        this.dataParams.api.forEachLeafNode(function (rowNode) {

            if(rowNode.data.keyRow == keyRow8) { rowNode8 = rowNode;   allItems.push(rowNode); }
            else if(rowNode.data.keyRow == keyRow6) { rowNode6 = rowNode;   allItems.push(rowNode);  }
            else if(rowNode.data.keyRow == keyRow3) { rowNode3 = rowNode;   allItems.push(rowNode);  }

            if( rowNode8 && rowNode6 && rowNode3) return;   // đã lấy được 3 row - kết thúc
            // allItems.push(rowNode);
        });
        // console.log(allItems);
        // if(rowNode8)  this.UpdateColumn(rowNode8, keyRow8);
        // if(rowNode6)  this.UpdateColumn(rowNode6, keyRow6);
        // if(rowNode3)  this.UpdateColumn(rowNode3, keyRow3);

        // console.log(this.rowData);
        // console.log(this.rowDataUpdate);
        this.processNoUpdate = true;


        for(let i=0; i < this.rowDataUpdate.length;i++) {

            let item = this.rowDataUpdate[i];

            if (rowNode8.data[item.colsId]  != item.itemCode_8 ) {
                // console.log(rowNode8.data[item.colsId] + ' = ' + item.itemCode_8);
                rowNode8.data[item.colsId] = Number(item.itemCode_8);
                // rowNode8.setDataValue(item.colsId,Number(item.itemCode_8));
                this.gridOptions.api.applyTransaction({ update: [rowNode8.data] });
                // console.log(rowNode8);
            }

            if (rowNode6.data[item.colsId].toString() != item.itemCode_6.toString()) {
                // console.log(rowNode6.data[item.colsId] + ' = ' + item.itemCode_6);
                rowNode6.data[item.colsId] = Number(item.itemCode_6);
                // rowNode6.setDataValue(item.colsId,Number(item.itemCode_6));
                this.gridOptions.api.applyTransaction({ update: [rowNode6.data] });
                // console.log(rowNode8);
            }

            if (rowNode3.data[item.colsId].toString() != item.itemCode_3.toString()) {
                // console.log(rowNode3.data[item.colsId] + ' = ' + item.itemCode_3);
                rowNode3.data[item.colsId] = Number(item.itemCode_3);
                // rowNode3.setDataValue(item.colsId,Number(item.itemCode_3));
                this.gridOptions.api.applyTransaction({ update: [rowNode3.data] });
                // console.log(rowNode8);
            }

            if( i == (this.rowDataUpdate.length -1)){
                setTimeout(() => {
                    this.processNoUpdate = false;   //set trạng thái cập nhật database
                    this.rowDataUpdate = [];
                }, 1000);
            }
        }
    }

    UpdateColumn(_rowNode, _keyRow) { console.log(_rowNode); }

}
