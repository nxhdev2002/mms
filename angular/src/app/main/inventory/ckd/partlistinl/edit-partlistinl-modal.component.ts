import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, GetPartListGradeDto, GetPartListGradePartListInlDto, InvCkdPartGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy, InvPioPartListInlServiceProxy, MstLgaBp2PartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { NumericEditor } from '@app/shared/common/numeric-cell-editor/NumericEditor';
import { CellClickedEvent, CellDoubleClickedEvent } from '@ag-grid-enterprise/all-modules';
import { PioPartListInlComponent } from './partlistinl.component';
import { DatePipe, formatDate } from '@angular/common';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';



@Component({
    selector: 'edit-partlistinl-modal',
    templateUrl: './edit-partlistinl-modal.component.html',
    styleUrls: ['./edit-partlistinl-modal.component.less']
})
export class EditPartListInlModalComponent extends AppComponentBase {
    @ViewChild('editPartListInlModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    gridParams: GridParams;
    minwidth0 = "minwidth0";
    rowdata: InvCkdPartListDto = new InvCkdPartListDto();
    partListColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    datasEdit: GetGradebyPartListDto[] = [];
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _shop;
    _grade;
    _usageQty;
    _partNo;
    _model;
    _partName;
    _supplierNo;
    _orderPattern = 'PXP';
    checkEdit: boolean = true;
    datasEdits: GetGradebyPartListDto[] = [];
    orderPattern
    orderPatternList = [
        { value: 'PXP', label: "PXP" },


    ];
    gridOrderPatternList = [
        { key: 'PXP', value: "PXP" },

    ];
    shopList = [
        { key: 'W', value: "W" },
        { key: 'T', value: "T" },
        { key: 'A', value: "A" },
    ];



    cfcList = [{ value: '', label: '' }];
    cbbCfcList = [{ value: '', label: '' }];
    supplierList = [{ value: '', label: '' }];
    cbbSupplierList = [{ value: '', label: '' }];
    gradeList = [];
    gradeLists1 = [];
    gradeLists1_temp = [];
    colorList = [];
    gradeListbyCfc = [];
    gradeListOther = [];
    gradeListLoad = [];
    gradeListCfc = [];
    _selectCfc;
    _test;
    _listGradeCheck: GetGradebyPartListDto[] = [];
    item;
    _partId;
    isCreate: boolean = true;
    checkClone: boolean = true;
    checkValidate: boolean = true;
    rowSelection = 'multiple';
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    pipe = new DatePipe('en-US');
    defaultColDef = {
        resizable: true,
        //  sortable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        // filter: true,
        // floatingFilter: true,
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };
    _partlistId;

    constructor(
        injector: Injector,
        private _service: InvPioPartListInlServiceProxy,
        private _partListComp: PioPartListInlComponent,

    ) {
        super(injector);
        this.partListColDef = [
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
                width: 35
            },
            {
                headerName: this.l('ID'),
                headerTooltip: this.l('ID'),
                field: 'id',
                hide: true,
            },
            {
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1.5,
            },
            {
                headerName: this.l('Model'),
                flex: 1, headerTooltip: this.l('Model'),
                field: 'model',

            },
            {
                headerName: this.l('Grade'),
                headerTooltip: this.l('Grade'),
                field: 'grade',
                flex: 1
            },
            {
                headerName: this.l('Usage Qty'),

                headerTooltip: this.l('Usage Qty'),
                field: 'usageQty',
                type: 'rightAligned',
                flex: 1,
                editable: true,
                cellEditor: NumericEditor,
            },
            {
                headerName: this.l('Shop'),
                headerTooltip: this.l('Shop'),
                field: 'shop',
                cellRenderer: 'agSelectRendererComponent',
                list: this.shopList,
                cellClass: ['RendererCombobox', 'text-center'],
                flex: 1,
                editable: true,
            },
            {
                headerName: this.l('Body Color'),
                headerTooltip: this.l('Body Color'),
                field: 'bodyColor',
                flex: 1,
            },

            {
                headerName: this.l('Start Lot'),
                flex: 1,
                headerTooltip: this.l('Start Lot'),
                field: 'startLot',
                editable: true,
                cellEditor: NumericEditor,
                onCellClicked: (event: CellClickedEvent) => this.onStartEdit(event),
            },
            {
                headerName: this.l('End Lot'),
                flex: 1,
                headerTooltip: this.l('End Lot'),
                field: 'endLot',
                editable: true,
                cellEditor: NumericEditor,
                onCellClicked: (event: CellClickedEvent) => this.onStartEdit(event),
            },
            {
                headerName: this.l('Start Run'),
                flex: 1,
                headerTooltip: this.l('Start Run'),
                field: 'startRun',
                editable: true,
                cellEditor: NumericEditor,
            },
            {
                headerName: this.l('End Run'),
                flex: 1,
                headerTooltip: this.l('End Run'),
                field: 'endRun',
                editable: true,
                cellEditor: NumericEditor,
            },
            {
                headerName: this.l('Ef From Part'),
                width: 150,
                headerTooltip: this.l('Ef From Part'),
                field: 'efFromPart',
                editable: true,
                valueGetter: (params) => this.pipe.transform(params.data?.efFromPart, 'dd-MM-yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
            },
            {
                headerName: this.l('Ef To Part'),
                width: 150,
                headerTooltip: this.l('Ef To Part'),
                field: 'efToPart',
                editable: true,
                valueGetter: (params) => this.pipe.transform(params.data?.efToPart, 'dd-MM-yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
            },
            {
                headerName: this.l('OrderPattern'),
                headerTooltip: this.l('OrderPattern'),
                field: 'orderPattern',
                cellRenderer: 'agSelectRendererComponent',
                list: this.gridOrderPatternList,
                cellClass: ['RendererCombobox', 'text-center'],
                flex: 1,
                editable: true,
            },
            {
                headerName: this.l('Remark'),
                flex: 1,
                headerTooltip: this.l('Remark'),
                field: 'remark',
                editable: true
            },
        ]
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
            agDatepickerRendererComponent: AgDatepickerRendererComponent

        };
    }

    onStartEdit(event) {
        console.log(event)
        if (event.column.colId == 'startLot' && event.column.startLot !== null) {
            event.data.startLot = event.data.startLot.substring(event.data.startLot.indexOf("-") + 1, event.data.startLot.length);
            event.api.refreshCells({ columns: ["startLot"] });
        }
        if (event.column.colId == 'endLot') {
            event.data.endLot = event.data.endLot.substring(event.data.endLot.indexOf("-") + 1, event.data.endLot.length);
            event.api.refreshCells({ columns: ["endLot"] });
        }
        this.gridParams.api.startEditingCell({
            rowIndex: event.rowIndex,
            colKey: event.column.colId
        });
    }

    onCellValueChanged(event) {
        const usageQtyPattern = /^[1-9][0-9]*$/
        const lotPattern = /^[0-9]{1,4}$/
        const runPattern = /^(?:[1-9]|10)$/
        console.log(event.newValue);

        //Trường UsageQty phải là số > 0
        if (event.column.colId == 'usageQty') {
            if (event.data.usageQty != null && Number(event.data.usageQty) == 0) {
                this.message.error(this.l('Trường UsageQty phải lớn hơn 0'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.data.usageQty == null || event.data.usageQty == '') {
                this.message.error(this.l('Trường UsageQty phải nhập'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
                //this.checkValidate = false;
                return;
            }
        }

        //startLot
        else if (event.column.colId == 'startLot') {
            if (event.newValue == null || event.newValue == '') {
                event.data.startRun = null;
                event.api.refreshCells({ columns: ["startRun"] });
            }
            else if (event.newValue != null && (!lotPattern.test(event.data.startLot) || Number(event.data.startLot) == 0)) {
                this.message.error(this.l('Trường StartLot phải là số trong khoảng 1 - 9999'));
                event.data.startLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["startLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null && event.data.endLot != null && event.data.startLot != null && (Number(event.data.startLot) > Number(event.data.endLot.substring(event.data.endLot.indexOf("-") + 1, event.data.endLot.length)))) {
                this.message.error(this.l('Trường StartLot phải nhỏ hơn hoặc bằng trường EndLot'));
                event.data.startLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["startLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null) {
                // if((Number(event.data.startLot) == Number(event.data.endLot.substring(event.data.endLot.indexOf("-") + 1, event.data.endLot.length)))
                //     && Number(event.data.startRun) > Number(event.data.endRun)){
                //         event.data.startRun = event.data.endRun;
                //         event.api.refreshCells({ columns: ["startRun"] });
                // }

                event.data.startLot = `${event.data.grade}-${event.newValue}`;
                event.api.refreshCells({ columns: ["startLot"] });
                // this.checkValidate = true;
            }
        }

        //endLot
        else if (event.column.colId == 'endLot') {
            if (event.newValue == null || event.newValue == '') {
                event.data.endRun = null;
                event.api.refreshCells({ columns: ["endRun"] });
            }
            else if (event.newValue != null && ((!lotPattern.test(event.data.endLot)) || Number(event.data.endLot) == 0)) {
                this.message.error(this.l('Trường EndLot phải là số trong khoảng 1 - 9999'));
                event.data.endLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["endLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null && event.data.endLot != null && event.data.startLot != null && (Number(event.data.endLot) < Number(event.data.startLot.substring(event.data.startLot.indexOf("-") + 1, event.data.startLot.length)))) {
                this.message.error(this.l('Trường EndLot phải lớn hơn hoặc bằng trường StartLot'));
                event.data.endLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["endLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null) {
                // if((Number(event.data.endLot) == Number(event.data.startLot.substring(event.data.startLot.indexOf("-") + 1, event.data.startLot.length)))
                //     && Number(event.data.startRun) > Number(event.data.endRun)){
                //         event.data.endRun = event.data.startRun;
                //         event.api.refreshCells({ columns: ["endRun"] });
                // }

                event.data.endLot = `${event.data.grade}-${event.newValue}`;
                event.api.refreshCells({ columns: ["endLot"] });
                // this.checkValidate = true;
            }
        }
        //startRun
        else if (event.column.colId == 'startRun') {
            if (event.newValue != null && (event.data.startLot == null || event.data.startLot == '')) {
                this.message.error(this.l('Phải nhập StartLot trước khi nhập trường StartRun'));
                event.data.startRun = null;
                event.api.refreshCells({ columns: ["startRun"] });
                this.gridParams.api.setFocusedCell(event.rowIndex, "startLot");
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (!runPattern.test(event.data.startRun) || Number(event.data.startRun) == 0)) {
                this.message.error(this.l('Trường StartRun phải là số trong khoảng 1 - 10'));
                event.data.startRun = event.oldValue;
                event.api.refreshCells({ columns: ["startRun"] });
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (event.data.startLot == event.data.endLot) && event.data.endRun != null && (Number(event.data.startRun) > Number(event.data.endRun))) {
                this.message.error(this.l('Trường StartRun phải nhỏ hơn hoặc bằng trường EndRun khi cùng Lot'));
                event.data.startRun = event.oldValue;
                event.api.refreshCells({ columns: ["startRun"] });
                // this.checkValidate = false;
                return;
            }
            // else {
            //     this.checkValidate = true;
            // }
        }

        //endRun
        else if (event.column.colId == 'endRun') {
            if (event.newValue != null && (event.data.endLot == null || event.data.endLot == '')) {
                this.message.error(this.l('Phải nhập EndLot trước khi nhập trường EndRun'));
                event.data.endRun = null;
                event.api.refreshCells({ columns: ["endRun"] });
                this.gridParams.api.setFocusedCell(event.rowIndex, "endLot");
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (!runPattern.test(event.data.endRun) || Number(event.data.endRun) == 0)) {
                this.message.error(this.l('Trường End phải là số trong khoảng 1 - 10'));
                event.data.endRun = event.oldValue;
                event.api.refreshCells({ columns: ["endRun"] });
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (event.data.startLot == event.data.endLot) && event.data.startRun != null && Number(event.data.startRun) > Number(event.data.endRun)) {
                this.message.error(this.l('Trường EndRun phải lớn hơn hoặc bằng trường StartRun khi cùng Lot'));
                event.data.endRun = event.oldValue;
                event.api.refreshCells({ columns: ["endRun"] });
                // this.checkValidate = false;
                return;
            }
            // else {
            //     this.checkValidate = true;
            // }
        }
    }

    show(rowdata?: any): void {
        this.checkValidate = true;
        this.cfcList = [];
        this.gradeList = [];
        this.colorList = [];
        this.gradeLists1 = [];
        this.gradeListLoad = [];
        this.gradeListOther = [];
        this.supplierList = [];
        this.gradeListbyCfc = [];
        //InvCkdPartGradeDto

        this._partlistId = rowdata.partId;

        // list cfc
        this.cbbCfcList = [{ value: '', label: '' }];
        this._service.getListCfcs().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.cbbCfcList.push({ value: e.cfc, label: e.cfc });
                })
            }
        })


        // list Supp
        this.cbbSupplierList = [{ value: '', label: '' }];
        this._service.getListSupplierNos().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.cbbSupplierList.push({ value: e.supplierNo, label: e.supplierNo });
                })
            }
        })

        // list grade
        this._service.getListGrades().subscribe(resultGrades => {
            this.gradeList = resultGrades;
        })

        setTimeout(() => {
            if (!rowdata) {
                this.rowdata = new InvCkdPartListDto();
                this.isCreate = false;
                this._partNo = '';
                this._partName = '';
                this._orderPattern = '';
                this._supplierNo = '';
                this._selectCfc = '';
                this._partId = '';
            }
            else {
                this.isCreate = true;
                this.rowdata = rowdata;
                this._partNo = rowdata.partNo;
                this._partName = rowdata.partName;
                this._orderPattern = rowdata.orderPattern;
                this._supplierNo = rowdata.supplierNo;
                this._selectCfc = rowdata.cfc;
                this._partId = rowdata.partId;
                this.gradeListbyCfc = this.gradeList.filter(x => x.cfc === rowdata.cfc);

                //partgrade
                this._service.getPartGradeInl(
                    rowdata.partId,
                    '',
                    this.paginationGradeParams.skipCount,
                    this.paginationGradeParams.pageSize
                )
                    .pipe(finalize(() => { }))
                    .subscribe((result) => {
                        this.gradeLists1 = result.items ?? [];
                        this.gradeLists1_temp = result.items ?? [];
                        this.gradeLists1.map(e => {
                            this.gradeListLoad.push(e.grade);
                        })

                        setTimeout(() => {
                            this.gradeListOther = this.gradeListbyCfc.filter(x => !this.gradeListLoad.includes(x.grade));
                        }, 300);
                    });

            }
            this.active = true;
            this.modal.show();

            setTimeout(() => {
                this.selectAll();
            }, 300)

        }, 500);
    }

    changeCfc() {
        this.gradeLists1 = [];
        this.gradeListbyCfc = [];

        this.gradeListbyCfc = this.gradeList.filter(x => x.cfc === this._selectCfc);
        this.gradeListbyCfc.forEach(item => {
            this.gradeLists1.push({ partNo: this._partNo, grade: item.grade, cfc: item.cfc })
        })
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => GetGradebyPartListDto[] } }) {
        this._listGradeCheck = params.api.getSelectedRows();
    }

    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked']);
            this.gridParams.api.redrawRows();
        });
    }

    onChangeCheckGrade() {

        this._service.getPartGradeInl(
            this._partId,
            '',
            this.paginationGradeParams.skipCount,
            this.paginationGradeParams.pageSize
        )
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                if (result.totalCount != 0) {
                    this.gradeLists1 = [];
                    this.gradeLists1 = result.items ?? [];
                    this.gradeListbyCfc.forEach(e => {
                        if (e.checks == true) {
                            this.gradeLists1.push({ partNo: this._partNo, model: e.model, grade: e.grade, usageQty: this.gradeLists1[0].usageQty, shop: this.gradeLists1[0].shop, cfc: this._selectCfc, partId: this.gradeLists1_temp[0].partId, orderPattern: this.gradeLists1[0].orderPattern })
                        }
                    });
                }
                else {
                    this.gradeLists1 = [];
                    this.gradeListbyCfc.forEach(e => {
                        if (e.checks == true) {
                            this.gradeLists1.push({ partNo: this._partNo, model: e.model, grade: e.grade, usageQty: 0, shop: '', cfc: this._selectCfc, partId: this._partId, orderPattern: this._orderPattern })
                        }
                    });
                }

            });

        setTimeout(() => {
            this.selectAll();


        }, 500)
        //
    }

    callBackDataGrid1(params: GridParams) {
        this.gridParams = params;
        setTimeout(() => {
            this.selectAll();
        }, 500)
    }

    save(): void {

        if (this._partName == null || this._partName == '') {
            this.notify.warn(this.l('PartName chưa nhập!'));
            document.getElementById('partName').focus();
            return;
        }

        if (this._supplierNo == null || this._supplierNo == '') {
            this.notify.warn(this.l('SupplierNo chưa chọn!'));
            document.getElementById('supplierNo').focus();
            return;
        }

        if (this._selectCfc == null || this._selectCfc == '') {
            this.notify.warn(this.l('Cfc chưa chọn!'));
            document.getElementById('cfc').focus();
            return;
        }

        if (this._orderPattern == null || this._orderPattern == '') {
            this.notify.warn(this.l('OrderPattern chưa chọn!'));
            document.getElementById('orderPattern').focus();
            return;
        }

        const arr = [];
        for (var i = 0; i < this._listGradeCheck.length; i++) {
            if(this._listGradeCheck[i].usageQty == null || this._listGradeCheck[i].usageQty == undefined){
                this.gridParams.api.setFocusedCell(i, "usageQty");
                this.notify.warn(this.l('UsageQty chưa nhập!'));
                this.checkValidate = false;
                break;
            }
            else if(this._listGradeCheck[i].usageQty == 0){
                this.gridParams.api.setFocusedCell(i, "usageQty");
                this.notify.warn(this.l('UsageQty chưa nhập!'));
                this.checkValidate = false;
                break;
            }

            else if(this._listGradeCheck[i].shop == null || this._listGradeCheck[i].shop == ''){
                this.gridParams.api.setFocusedCell(i, "shop");
                this.notify.warn(this.l('Shop chưa chọn!'));
                this.checkValidate = false;
                break;
            }
            // if (this._listGradeCheck[i].efFromPart == null && this._listGradeCheck[i].efToPart != null) {
            //     this.gridParams.api.setFocusedCell(i, "efFromPart");
            //     this.notify.warn(this.l('EfFromPart chưa nhập!'));
            //     this.checkValidate = false;
            //     break;
            // }
            else if (this._listGradeCheck[i].efFromPart != null && this._listGradeCheck[i].efToPart != null
                && this._listGradeCheck[i].efToPart < this._listGradeCheck[i].efFromPart) {
                this.gridParams.api.setFocusedCell(i, "efToPart");
                this.notify.warn(this.l('EfToPart không được trước ngày EfFromPart!'));
                this.checkValidate = false;
                break;
            }
            else if(this._listGradeCheck[i].startLot != null && this._listGradeCheck[i].startLot == ''
            && this._listGradeCheck[i].endLot != null && this._listGradeCheck[i].endLot == ''
            && this._listGradeCheck[i].startLot == this._listGradeCheck[i].endLot
            && this._listGradeCheck[i].startRun > this._listGradeCheck[i].endRun){
                this.gridParams.api.setFocusedCell(i, "endRun");
                this.notify.warn(this.l('Cùng Lot thì EndRun phải lớn hơn StartRun'));
                this.checkValidate = false;
                break;
            }
            else{
                this.checkValidate = true;
            }

            let obj = Object.assign(new GetGradebyPartListDto(), {
                id: this._listGradeCheck[i].id,
                partId: this._listGradeCheck[i].partId,
                partNo: this._listGradeCheck[i].partNo,
                model: this._listGradeCheck[i].model,
                grade: this._listGradeCheck[i].grade,
                cfc: this._selectCfc,
                usageQty: this._listGradeCheck[i].usageQty,
                shop: this._listGradeCheck[i].shop,
                bodyColor: this._listGradeCheck[i].bodyColor,
                startLot: this._listGradeCheck[i].startLot,
                endLot: this._listGradeCheck[i].endLot,
                startRun: this._listGradeCheck[i].startRun,
                endRun: this._listGradeCheck[i].endRun,
                efFromPart: this._listGradeCheck[i].efFromPart == undefined ? undefined : formatDate(new Date(this._listGradeCheck[i].efFromPart.toString()), 'dd/MM/yyyy', 'en-US'),
                efToPart: this._listGradeCheck[i].efToPart == undefined ? undefined : formatDate(new Date(this._listGradeCheck[i].efToPart.toString()), 'dd/MM/yyyy', 'en-US'),
                orderPattern: this._listGradeCheck[i].orderPattern,
                remark: this._listGradeCheck[i].remark
            })
            arr.push(obj);
        };

            // this.gridParams.api.forEachLeafNode(node => {
            //     let obj = Object.assign(new GetGradebyPartListDto(), {
            //         partNo:node.data.partNo,
            //         model:node.data.model,
            //         grade:node.data.grade,
            //         usageQty: node.data.usageQty,
            //         shop: node.data.shop,
            //         bodyColor: node.data.bodyColor,
            //         startLot: node.data.startLot,
            //         endLot: node.data.endLot,
            //         startRun: node.data.startRun,
            //         endRun: node.data.endRun
            //     })
            //     arr.push(obj);
            // })
        if (this.checkValidate) {
            let input = Object.assign(new GetPartListGradeDto(), {
                id: this.rowdata.id,
                partNo: this._partNo,
                partName: this._partName.toUpperCase(),
                supplierNo: this._supplierNo,
                cfc: this._selectCfc,
                orderPattern: this._orderPattern,
                listGrade: arr
            })

            this.isLoading = true;
            this._service.partListInlEdit(input)
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                    this._partListComp.reloadSearch(this._partNo, this._selectCfc,this._partlistId)
                    // this.isLoading = true;
                })
        }
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
