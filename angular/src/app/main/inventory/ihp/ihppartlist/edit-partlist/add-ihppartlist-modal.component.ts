import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetEditIhpPartGradeDto, GetEditIhpPartListDto, InvIhpPartListDto, InvIhpPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { AgCellButtonRendererComponent } from '@app/shared/common/grid/ag-cell-button-renderer/ag-cell-button-renderer.component';
import { AgDropdownRendererComponent } from '@app/shared/common/grid/ag-dropdown-renderer/ag-dropdown-renderer.component';
import { DatePipe, formatDate } from '@angular/common';
import { AgDatepickerRendererComponent } from '@app/shared/common/grid/ag-datepicker-renderer/ag-datepicker-renderer.component';
import { IhpPartListComponent } from '../ihppartlist.component';

@Component({
    selector: 'addIhpPartList',
    templateUrl: './add-ihppartlist-modal.component.html',
    styleUrls: ['./add-ihppartlist-modal.component.less']
})
export class AddIhpPartListModalComponent extends AppComponentBase {
    @ViewChild('addPartListModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvIhpPartListDto = new InvIhpPartListDto();
    pipe = new DatePipe('en-US');
    partListColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _grade;
    _usageQty;
    _partNo;
    _partName;
    _supplierNo;
    _materialCode;
    _materialSpec;
    _supplierType;
    _supplierCd;
    _sourcing;
    _cutting;
    _packing;
    _sheetWeight;
    _yiledRation;

    cbbCfcList = [{ value: '', label: '' }];
    gradeList = [];
    gradeLists1 = [];
    gradeLists1_temp = [];
    gradeListbyCfc = [];
    _selectCfc;
    _listGradeCheck: GetEditIhpPartGradeDto[] = [];
    _partId;

    rowSelection = 'multiple';
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    defaultColDef = {
        resizable: true,
        floatingFilterComponentParams: { suppressFilterButton: true },
        suppressHorizontalScroll: true,
        textFormatter: function (r: any) {
            if (r == null) return null;
            return r.toLowerCase();
        },
        tooltip: (params) => params.value,
    };

    gridParams: GridParams;

    partNoList = [{ value: '', label: '' }];
    checkValidate: boolean = true;
    listMaterial = [];
    drmPartId;
    partSpec;

    constructor(
        injector: Injector,
        private _service: InvIhpPartListServiceProxy,
        private _component: IhpPartListComponent
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
            { headerName: this.l('Grade'), headerTooltip: this.l('Grade'), field: 'grade', flex: 1 },
            {
                headerName: this.l('Usage Qty'), headerTooltip: this.l('Usage Qty'), field: 'usageQty', flex: 1, type: 'rightAligned', editable: true,
                cellStyle: { 'background-color': "white", 'border': "1px solid red", 'border-radius': '5px' }
            },
            {
                headerName: this.l('First Day Product'), headerTooltip: this.l('First Day Product'), field: 'firstDayProduct', flex: 1, editable: true,
                valueGetter: (params) => this.pipe.transform(params.data?.firstDayProduct, 'dd/MM/yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
            },
            {
                headerName: this.l('Last Day Product'), headerTooltip: this.l('Last Day Product'), field: 'lastDayProduct', flex: 1, editable: true,
                valueGetter: (params) => this.pipe.transform(params.data?.lastDayProduct, 'dd/MM/yyyy'),
                cellClass: ['cell-clickable', 'cell-border', 'text-center'],
                cellRenderer: 'agDatepickerRendererComponent',
            }
        ];
        this.frameworkComponents = {
            agSelectRendererComponent: AgDropdownRendererComponent,
            agCellButtonComponent: AgCellButtonRendererComponent,
            agDatepickerRendererComponent: AgDatepickerRendererComponent,
        };
    }

    ngOnInit(): void {
        // list cfc
        this._service.getListCfc().subscribe(res => {
            res.forEach(e => {
                if (e.cfc != null) this.cbbCfcList.push({ value: e.cfc, label: e.cfc });
            });
        });

        // list grade
        this._service.getListGrade().subscribe(resultGrades => {
            resultGrades.forEach(e => {
                this.gradeList.push({ value: e.grade, label: e.grade, cfc: e.cfc });
            });
        });

        this._service.getListInhousePartNo().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.partNoList.push({ value: e.partNo, label: e.partNo });
                })
            }
        })
    }

    show(rowdata?: InvIhpPartListDto): void {
        this.gradeLists1 = [];

        setTimeout(() => {
            if (!rowdata) {
                this.rowdata = new InvIhpPartListDto();
                this._partNo = '';
                this._partName = '';
                this._supplierNo = '';
                this._selectCfc = '';
                this._partId = '';
                this._materialCode = '';
                this._materialSpec = '';
                this._supplierType = '';
                this._supplierCd = '';
                this._sourcing = '';
                this._cutting = '';
                this._packing = undefined;
                this._sheetWeight = '';
                this._yiledRation = '';
                this.listMaterial = [];
            }
            else {
                this.rowdata = rowdata;
                this._partNo = rowdata.partNo;
                this._partName = rowdata.partName;
                this._selectCfc = rowdata.cfc;
                this.changeCfc(this._selectCfc);
                this._materialCode = this.rowdata.materialCode;
                this._materialSpec = this.rowdata.materialSpec;
                this._supplierType = this.rowdata.supplierType;
                this._supplierCd = this.rowdata.supplierCd;
                this._sourcing = this.rowdata.sourcing;
                this._cutting = this.rowdata.cutting;
                this._packing = this.rowdata.packing;
                this._sheetWeight = this.rowdata.sheetWeight;
                this._yiledRation = this.rowdata.yiledRation;
                this.drmPartId = this.rowdata.drmPartListId;
                this.gradeListbyCfc = this.gradeList.filter(x => x.cfc === rowdata.cfc);

                //partgrade
                this._service.getDataPartGradebyId(
                    rowdata.id,
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
            this.modal.show();

            setTimeout(() => {
                this.selectAll();
            }, 500);
        }, 700);

    }

    changeCfc(ev){
        this.listMaterial = [];
        this._service.getListMaterialByCfc(ev).subscribe(result => {
            result.forEach(e => {
                this.listMaterial.push({id: e.id, value: e.materialCode, label: e.materialCode, matspec: e.materialSpec, supcd: e.supplierCd, suptype: e.supplierType, partspec: e.partSpec});
            })
        })
    }

    changeMaterial(ev){
        this._materialSpec = this.listMaterial.filter(e => e.value == ev)[0].matspec;
        this._supplierCd = this.listMaterial.filter(e => e.value == ev)[0].supcd;
        this._supplierType = this.listMaterial.filter(e => e.value == ev)[0].suptype;
        this.drmPartId = this.listMaterial.filter(e => e.value == ev)[0].id;
        this.partSpec = this.listMaterial.filter(e => e.value == ev)[0].partspec;
    }

    changeValue() {
        this.gradeLists1 = [];
        this.gradeLists1_temp = [];
        this.gradeListbyCfc = this.gradeList.filter(x => x.cfc == this._selectCfc);
        this.gradeListbyCfc.forEach(item => {
            this.gradeLists1.push({ grade: item.value, usageQty: 1, firstDayProduct: formatDate(new Date(), 'MM/dd/yyyy', 'en-US') });
        });
        setTimeout(() => {
            this.gradeLists1_temp = this.gradeLists1;
            this.selectAll();
        }, 300);
    }

    selectAll() {
        this.gridParams.api.forEachNode((e, idx) => {
            this.gridParams.api.getRowNode(`${e.rowIndex}`)?.setSelected(true);
            this.gridParams.api.setFocusedCell(e.rowIndex,
                this.gridParams.api.getColumnDefs()[0]['checked']);
            this.gridParams.api.redrawRows();
        });
    }

    onChangeRowSelection(params: { api: { getSelectedRows: () => GetEditIhpPartGradeDto[] } }) {
        this._listGradeCheck = params.api.getSelectedRows();
    }

    callBackDataGrid1(params: GridParams) {
        this.gridParams = params;
        setTimeout(() => {
            this.selectAll();
        }, 500);
    }

    save(): void {
        this.checkValidate = true;

        if (this._partNo == null || this._partNo == '') {
            this.notify.warn(this.l('PartNo is Required!'));
            document.getElementById('partNo').focus();
            return;
        } else if (this._partNo.length > 20) {
            this.notify.warn(this.l('PartNo max length is 20!'));
            document.getElementById('partNo').focus();
            return;
        }

        if (this._partName == null || this._partName == '') {
            this.notify.warn(this.l('PartName is Required!'));
            document.getElementById('partName').focus();
            return;
        }

        if (this._selectCfc == null || this._selectCfc == '') {
            this.notify.warn(this.l('Cfc is Required!'));
            document.getElementById('cfc').focus();
            return;
        }

        if (this._materialCode == null || this._materialCode == '') {
            this.notify.warn(this.l('MaterialCode is Required!'));
            document.getElementById('materialCode').focus();
            return;
        } else if (this._materialCode.length > 40) {
            this.notify.warn(this.l('MaterialCode max length is 40!'));
            document.getElementById('materialCode').focus();
            return;
        }

        if (this._materialSpec == null || this._materialSpec == '') {
            this.notify.warn(this.l('MaterialSpec is Required!'));
            document.getElementById('materialSpec').focus();
            return;
        } else if (this._materialSpec.length > 40) {
            this.notify.warn(this.l('MaterialSpec max length is 40!'));
            document.getElementById('materialSpec').focus();
            return;
        }

        if (this._supplierType == null || this._supplierType == '') {
            this.notify.warn(this.l('SupplierType is Required!'));
            document.getElementById('supplierType').focus();
            return;
        } else if (this._supplierType.length > 20) {
            this.notify.warn(this.l('SupplierType max length is 20!'));
            document.getElementById('supplierType').focus();
            return;
        }

        if (this._supplierCd == null || this._supplierCd == '') {
            this.notify.warn(this.l('SupplierCd is Required!'));
            document.getElementById('supplierCd').focus();
            return;
        } else if (this._supplierCd.length > 20) {
            this.notify.warn(this.l('SupplierCd max length is 20!'));
            document.getElementById('supplierCd').focus();
            return;
        }

        const intPattern = /^[0-9]*$/;
        if (this._packing == null || this._packing == '') {
            this.notify.warn(this.l('Packing is Required!'));
            document.getElementById('packing').focus();
            return;
        } else if (!intPattern.test(this._packing)) {
            this.notify.warn(this.l('Packing must be a positive integer!'));
            document.getElementById('packing').focus();
            return;
        }

        if (this._sheetWeight == null || this._sheetWeight == '') {
            this.notify.warn(this.l('SheetWeight is Required!'));
            document.getElementById('sheetWeight').focus();
            return;
        } else if (isNaN(this._sheetWeight)) {
            this.notify.warn(this.l('SheetWeight must be a number!'));
            document.getElementById('sheetWeight').focus();
            return;
        } else if (this._sheetWeight < 0) {
            this.notify.warn(this.l('SheetWeight must be greater than 0!'));
            document.getElementById('sheetWeight').focus();
            return;
        }

        if (this._yiledRation == null || this._yiledRation == '') {
            this.notify.warn(this.l('YiledRation is Required!'));
            document.getElementById('yiledRation').focus();
            return;
        } else if (isNaN(this._yiledRation)) {
            this.notify.warn(this.l('YiledRation must be a number!'));
            document.getElementById('yiledRation').focus();
            return;
        } else if (this._yiledRation < 0) {
            this.notify.warn(this.l('YiledRation must be greater than 0!'));
            document.getElementById('yiledRation').focus();
            return;
        }

        const arr = [];
        for (var i = 0; i < this._listGradeCheck.length; i++) {
            if (this._listGradeCheck[i].firstDayProduct == null) {
                this.gridParams.api.setFocusedCell(i, "firstDayProduct");
                this.notify.warn(this.l('FirstDayProduct is required!'));
                this.checkValidate = false;
                break;
            }
            if (this._listGradeCheck[i].lastDayProduct != null && this._listGradeCheck[i].firstDayProduct != null
                && this._listGradeCheck[i].lastDayProduct < this._listGradeCheck[i].firstDayProduct) {
                this.gridParams.api.setFocusedCell(i, "lastDayProduct");
                this.notify.warn(this.l('LastDayProduct cannot be less than FirstDayProduct!'));
                this.checkValidate = false;
                break;
            }
            let obj = Object.assign(new GetEditIhpPartGradeDto(), {
                grade: this._listGradeCheck[i].grade,
                usageQty: this._listGradeCheck[i].usageQty,
                firstDayProduct: formatDate(new Date(this._listGradeCheck[i].firstDayProduct.toString()), 'dd/MM/yyyy', 'en-US'),
                lastDayProduct: this._listGradeCheck[i].lastDayProduct == undefined ? undefined : formatDate(new Date(this._listGradeCheck[i].lastDayProduct.toString()), 'dd/MM/yyyy', 'en-US')
            });
            arr.push(obj);
        };

        if (this.checkValidate) {
            let input = Object.assign(new GetEditIhpPartListDto(), {
                supplierType: this._supplierType,
                supplierCd: this._supplierCd,
                cfc: this._selectCfc,
                partNo: this._partNo,
                partName: this._partName,
                materialCode: this._materialCode,
                materialSpec: this.partSpec,
                sourcing: this._sourcing,
                cutting: this._cutting,
                packing: this._packing,
                sheetWeight: this._sheetWeight,
                yiledRation: this._yiledRation,
                drmPartListId: this.drmPartId,
                listGrade: arr
            });

            this._service.checkExistPart(this._partNo, this._selectCfc)
                .subscribe((result) => {
                    if (!result) {
                        this.isLoading = true;
                        this._service.insertIhpPartList(input)
                            .pipe(finalize(() => this.isLoading = false))
                            .subscribe(() => {
                                this.notify.success(this.l('SavedSuccessfully'));
                                this._component.searchDatas();
                                this.close();
                            });
                    }
                    else {
                        this.message.warn(this.l('Inhouse Part already exists!'));
                    }
                });
        }
    }

    close(): void {
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    onCellValueChanged(event) {
        const usageQtyPattern = /^[1-9][0-9]*$/;
        //Trường UsageQty phải là số > 0
        if (event.column.colId == 'usageQty') {
            if (event.newValue == null || event.newValue == '') {
                this.message.warn(this.l('UsageQty cannot null!'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
            }
            else if (isNaN(event.data.usageQty)) {
                this.message.warn(this.l('UsageQty must be a number!'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
            }
            else if (!usageQtyPattern.test(event.data.usageQty)) {
                this.message.warn(this.l('UsageQty must be greater than 0!'));
                event.data.usageQty = event.oldValue;
                event.api.refreshCells({ columns: ["usageQty"] });
            }
        }
        else if (event.column.colId == 'firstDayProduct') {
            if (event.newValue == null || event.newValue == '') {
                this.message.warn(this.l('FirstDayProduct cannot null!'));
                event.data.firstDayProduct = formatDate(new Date(event.oldValue), 'dd/MM/yyyy', 'en-US');
                event.api.refreshCells({ columns: ["firstDayProduct"] });
            }
        }
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
