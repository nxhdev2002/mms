import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, GetPartListGradeDto, InvCkdPartListDto, InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { CellClickedEvent } from '@ag-grid-enterprise/all-modules';
import { CkdPartListComponent } from './partlist.component';
import { NumericEditor } from '@app/shared/common/numeric-cell-editor/NumericEditor';
import { DatePipe, formatDate } from '@angular/common';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';


@Component({
    selector: 'create-partlist-modal',
    templateUrl: './create-partlist-modal.component.html',
    styleUrls: ['./create-partlist-modal.component.less']
})
export class CreatePartListModalComponent extends AppComponentBase {
    @ViewChild('addPartListModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdPartListDto = new InvCkdPartListDto();
    partListColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    pipe = new DatePipe('en-US');
    datasEdit: GetGradebyPartListDto[] = [];
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _shop;
    _grade;
    _usageQty;
    _partNo;
    _partName;
    _supplierNo;
    _orderPattern;
    checkEdit: boolean = true;
    partNoList = [];
    minwidth0 = "minwidth0";
    minwidth01 = "minwidth01";
    datasEdits: GetGradebyPartListDto[] = [];
    orderPattern
    orderPatternList = [
        { value: 'LOT', label: "LOT" },
        { value: 'PXP', label: "PXP" },
    ];
    shopList = [
        { key: 'W', value: "W" },
        { key: 'T', value: "T" },
        { key: 'A', value: "A" },
    ];
    gridOrderPatternList = [
        { key: 'LOT', value: "LOT" },
        { key: 'PXP', value: "PXP" },
    ];
    _partlistId;
    cfcList = [{ value: '', label: '' }];
    cbbCfcList = [{ value: '', label: '' }];
    supplierList = [{ value: '', label: '' }];
    cbbSupplierList = [{ value: '', label: '' }];
    gradeList = [];
    gradeLists1 = [];
    gradeLists1_temp = [];
    colorList = [];
    gradeListbyCfc = [];
    _selectCfc;
    _listGradeCheck: GetGradebyPartListDto[] = [];
    item;
    _partId;
    isClone: boolean = true;
    rowSelection = 'multiple';
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };
    checkValidate: boolean = true;
    checkAddNew: boolean = true;

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

    gridParams: GridParams;
    constructor(
        injector: Injector,
        private _service: InvCkdPartListServiceProxy,
        private _partList: CkdPartListComponent
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
                headerName: this.l('Part No'),
                headerTooltip: this.l('Part No'),
                field: 'partNo',
                flex: 1.5,
            },
            {
                headerName: this.l('Model'),
                headerTooltip: this.l('Model'),
                field: 'model',
                flex: 1,
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
                //editable: true,
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
                //editable: true,
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

    show(rowdata?: InvCkdPartListDto): void {
        this.checkValidate = true;
        this.checkAddNew = (!rowdata) ? false : true;
        this.partNoList = [];
        this.cfcList = [];
        this.gradeList = [];
        this.colorList = [];
        this.gradeLists1 = [];
        this.supplierList = [];
        this._partlistId = !rowdata ? null : rowdata.partId;

        // list cfc
        this.cbbCfcList = [];
        this.cbbCfcList = [{ value: '', label: '' }];
        this.cfcList.push({ value: '', label: '' });
        this._service.getListCfcs().subscribe(res => {
            if (res) {
                res.map(e => {
                    // this.cfcList.push({ value: e.cfc, label: e.cfc });
                    // this.cbbCfcList = [...this.cfcList];
                    this.cbbCfcList.push({ value: e.cfc, label: e.cfc });
                })
            }
        })
        // list partno
        this._service.getListPartNo().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.partNoList.push({ value: e.partNo, label: e.partNo });
                })
            }
        })
        // list Supp
        this.cbbSupplierList = [{ value: '', label: '' }];
        this._service.getListSupplierNos().subscribe(res => {
            if (res) {
                res.map(e => {
                    // this.supplierList.push({ value: e.supplierNo, label: e.supplierNo });
                    // this.cbbSupplierList = [...this.supplierList];
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
                this.isClone = false;
                this._partNo = '';
                this._partName = '';
                this._orderPattern = '';
                this._supplierNo = '';
                this._selectCfc = '';
                this._partId = '';
                this.gradeLists1 = [];
                this.gradeLists1_temp = [];

            }
            else {
                this.isClone = true;
                this.rowdata = rowdata;
                this._partNo = rowdata.partNo;
                this._partName = rowdata.partName;
                this._orderPattern = rowdata.orderPattern;
                this._supplierNo = rowdata.supplierNo;
                this._selectCfc = rowdata.cfc;
                this._partId = rowdata.partId;
                this.gradeListbyCfc = this.gradeList.filter(x => x.cfc === rowdata.cfc);

                //partgrade
                this._service.getPartGrade(
                    rowdata.partId,
                    '',
                    this.paginationGradeParams.skipCount,
                    this.paginationGradeParams.pageSize
                )
                    .pipe(finalize(() => { }))
                    .subscribe((result) => {
                        this.gradeLists1 = result.items ?? [];
                        this.gradeLists1_temp = result.items ?? [];
                    });
            }

            this.active = true;
            this.modal.show();

            setTimeout(() => {
                this.selectAll();
            }, 300)
        }, 500);

    }


    changePartNo(partNo?) {
        this.gradeLists1 = [];
        this.gradeLists1_temp.forEach(item => {
            this.gradeLists1.push({
                partNo: partNo == null ? this._partNo : partNo,
                model: item.model,
                grade: item.grade,
                usageQty: item.usageQty,
                shop: item.shop,
                bodyColor: item.bodyColor,
                startLot: item.startLot,
                endLot: item.endLot,
                startRun: item.startRun,
                endRun: item.endRun,
                efFromPart: item.efFromPart,
                efToPart: item.efToPart,
                orderPattern: item.orderPattern,
                remark: item.remark,
                cfc: item.cfc
            })
        })

        // setTimeout(() => {
        //     this.selectAll();
        // }, 300)
    }

    changeCfc() {
        this.gradeLists1 = [];
        this.gradeListbyCfc = [];
        this.gradeLists1_temp = [];
        this.gradeListbyCfc = this.gradeList.filter(x => x.cfc === this._selectCfc);
        this.gradeListbyCfc.forEach(item => {
            this.gradeLists1.push({ partNo: this._partNo, grade: item.grade, cfc: item.cfc, model: item.model })
        })
        setTimeout(() => {
            this.gradeLists1_temp = this.gradeLists1;
            this.selectAll();
        }, 100);

    }

    changePattern() {
        this.gradeLists1 = [];
        this.gradeLists1_temp.forEach(item => {
            this.gradeLists1.push({
                partNo: item.partNo,
                model: item.model,
                grade: item.grade,
                usageQty: item.usageQty,
                shop: item.shop,
                bodyColor: item.bodyColor,
                startLot: item.startLot,
                endLot: item.endLot,
                startRun: item.startRun,
                endRun: item.endRun,
                efFromPart: item.efFromPart,
                efToPart: item.efToPart,
                orderPattern: this._orderPattern,
                remark: item.remark,
                cfc: item.cfc
            })
        })
        // setTimeout(() => {
        //     this.gradeLists1_temp = this.gradeLists1;
        //     this.selectAll();
        // }, 100);
    }


    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked']);
            this.gridParams.api.redrawRows();
        });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => GetGradebyPartListDto[] } }) {
        this._listGradeCheck = params.api.getSelectedRows();
    }

    onChangeCheckGrade(e) {
        this.gradeLists1 = [];
        this._service.getPartGrade(
            this._partId,
            '',
            this.paginationGradeParams.skipCount,
            this.paginationGradeParams.pageSize
        )
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.gradeLists1 = result.items ?? [];
                this.gradeListbyCfc.forEach(e => {
                    if (e.checks == true) {
                        this.gradeLists1.push({ partNo: this._partNo, grade: e.grade, cfc: e.cfc })
                    }
                });
            });
        //
    }

    onCellValueChanged(event) {
        const usageQtyPattern = /^[1-9][0-9]*$/
        const lotPattern = /^[0-9]{1,4}$/
        const runPattern = /^(?:[1-9]|10)$/

        //Trường UsageQty phải là số > 0
        if (event.column.colId == 'usageQty') {
            if (event.newValue == null || event.newValue == '') {
                this.message.error(this.l('Trường UsageQty phải nhập'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
                //this.checkValidate = false;
                return;
            }
            else if (isNaN(event.data.usageQty)) {
                this.message.warn(this.l('Trường UsageQty phải là số!'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
                // this.checkValidate = false;
                return;
            }
            else if (!usageQtyPattern.test(event.data.usageQty)) {
                this.message.error(this.l('Trường UsageQty phải lớn hơn 0'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
                // this.checkValidate = false;
                return;
            }
            // else {
            //     this.checkValidate = true;
            // }
        }

        //startLot
        if (event.column.colId == 'startLot') {
            if (event.newValue == null) {
                event.data.startRun = null;
                event.api.refreshCells({ columns: ["startRun"] });
            }
            else if (event.newValue != null && (!lotPattern.test(event.data.startLot) || Number(event.data.startLot) == 0)) {
                this.message.warn(this.l('Trường StartLot phải là số trong khoảng 1 - 9999'));
                event.data.startLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["startLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null && event.data.endLot != null && event.data.startLot != null && (Number(event.data.startLot) > Number(event.data.endLot.substring(event.data.endLot.indexOf("-") + 1, event.data.endLot.length)))) {
                this.message.warn(this.l('Trường StartLot phải nhỏ hơn hoặc bằng trường EndLot'));
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
        if (event.column.colId == 'endLot') {
            if (event.newValue == null) {
                event.data.endLot = null;
                event.api.refreshCells({ columns: ["endLot"] });
            }
            else if (event.newValue != null && ((!lotPattern.test(event.data.endLot)) || Number(event.data.endLot) == 0)) {
                this.message.warn(this.l('Trường EndLot phải là số trong khoảng 1 - 9999'));
                event.data.endLot = event.oldValue != null ? `${event.data.grade}-${event.oldValue}` : null;
                event.api.refreshCells({ columns: ["endLot"] });
                // this.checkValidate = false;
                return;
            }
            else if (event.newValue != null && event.data.endLot != null && event.data.startLot != null && (Number(event.data.endLot) < Number(event.data.startLot.substring(event.data.startLot.indexOf("-") + 1, event.data.startLot.length)))) {
                this.message.warn(this.l('Trường EndLot phải lớn hơn hoặc bằng trường StartLot'));
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
        if (event.column.colId == 'startRun') {
            if (event.newValue != null && event.data.startLot == null) {
                this.message.warn(this.l('Phải nhập StartLot trước khi nhập trường StartRun'));
                event.data.startRun = null;
                event.api.refreshCells({ columns: ["startRun"] });
                this.gridParams.api.setFocusedCell(event.rowIndex, "startLot");
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (!runPattern.test(event.data.startRun) || Number(event.data.startRun) == 0)) {
                this.message.warn(this.l('Trường StartRun phải là số trong khoảng 1 - 10'));
                event.data.startRun = event.oldValue;
                event.api.refreshCells({ columns: ["startRun"] });
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (event.data.startLot == event.data.endLot) && event.data.startRun != null && event.data.endRun != null && (Number(event.data.startRun) > Number(event.data.endRun))) {
                this.message.warn(this.l('Trường StartRun phải nhỏ hơn hoặc bằng trường EndRun khi cùng Lot'));
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
        if (event.column.colId == 'endRun') {
            if (event.newValue != null && event.data.endLot == null) {
                this.message.warn(this.l('Phải nhập EndLot trước khi nhập trường EndRun'));
                event.data.endRun = null;
                event.api.refreshCells({ columns: ["endRun"] });
                this.gridParams.api.setFocusedCell(event.rowIndex, "endLot");
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (!runPattern.test(event.data.endRun) || Number(event.data.endRun) == 0)) {
                this.message.warn(this.l('Trường End phải là số trong khoảng 1 - 10'));
                event.data.endRun = event.oldValue;
                event.api.refreshCells({ columns: ["endRun"] });
                // this.checkValidate = false;
                return;
            } else if (event.newValue != null && (event.data.startLot == event.data.endLot) && event.data.startRun != null && event.data.endRun != null && Number(event.data.startRun) > Number(event.data.endRun)) {
                this.message.warn(this.l('Trường EndRun phải lớn hơn hoặc bằng trường StartRun khi cùng Lot'));
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

    callBackDataGrid1(params: GridParams) {
        this.gridParams = params;
        setTimeout(() => {
            this.selectAll();
        }, 500)
    }

    save(): void {
        this.checkValidate = true;

        const arr = [];

        if (this._partNo == null || this._partNo == '') {
            this.notify.warn(this.l('PartNo chưa nhập!'));
            document.getElementById('partNo').focus();
            return;
        } else if (this._partNo.length != 12) {
            this.notify.warn(this.l('PartNo phải là 12 ký tự!'));
            document.getElementById('partNo').focus();
            return;
        }

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

        // if (this.rowdata.partNo != null && (this.rowdata.partNo.length < 12 || this.rowdata.partNo.length > 12)) {
        //     this.message.warn(this.l('partNo :' + this.rowdata.partNo + ' phải là 12 ký tự'));
        //     this.checkValidate = false;
        //     return;
        // }

        for (var i = 0; i < this._listGradeCheck.length; i++) {
            if (this._listGradeCheck[i].usageQty == null || this._listGradeCheck[i].usageQty == undefined) {
                this.gridParams.api.setFocusedCell(i, "usageQty");
                this.notify.warn(this.l('UsageQty chưa nhập!'));
                this.checkValidate = false;
                break;
            }
            else if (this._listGradeCheck[i].shop == null || this._listGradeCheck[i].shop == '') {
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
            else if (this._listGradeCheck[i].startLot != null && this._listGradeCheck[i].startLot != ''
                && this._listGradeCheck[i].endLot != null && this._listGradeCheck[i].endLot != ''
                && (this._listGradeCheck[i].startLot == this._listGradeCheck[i].endLot)
                && (Number(this._listGradeCheck[i].startRun) > Number(this._listGradeCheck[i].endRun))) {
                this.gridParams.api.setFocusedCell(i, "endRun");
                this.notify.warn(this.l('Cùng Lot thì EndRun phải lớn hơn StartRun'));
                this.checkValidate = false;
                break;
            } else {
                this.checkValidate = true;
            }

            let obj = Object.assign(new GetGradebyPartListDto(),
                {
                    partNo: this._listGradeCheck[i].partNo,
                    model: this._listGradeCheck[i].model,
                    grade: this._listGradeCheck[i].grade,
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
        }

        if (this.checkValidate) {
            let input = Object.assign(new GetPartListGradeDto(), {
                partNo: this._partNo,
                partName: this._partName,
                supplierNo: this._supplierNo,
                cfc: this._selectCfc,
                orderPattern: this._orderPattern,
                listGrade: arr
            })

            this.isLoading = true;
            this._service.partListInsert(input)
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this.notify.success(this.l('SavedSuccessfully'));
                    this._partList.reloadSearch(this._partNo, this._selectCfc, this._partlistId);
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                    //this._partList.reloadSearch(this._partNo, this._selectCfc, this._partlistId)
                    // this.isLoading = true;
                })
        }
    }

    close(): void {
        this._partNo = '',
            this._partName = '',
            this._supplierNo = '',
            this._selectCfc = '',
            this._orderPattern = '',
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
