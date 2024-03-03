import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, InvCkdPartGradeDto, InvCkdPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { CkdPartListComponent } from './partlist.component';



@Component({
    selector: 'edit-partgrade-modal',
    templateUrl: './edit-partgrade-modal.component.html',
    styleUrls: ['./edit-partgrade-modal.component.less']
})
export class EditPartGradeModalComponent extends AppComponentBase {
    @ViewChild('editPartGradeModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerFrom', { static: false }) datepickerFrom!: BsDatepickerDirective;
    @ViewChild('datepickerTo', { static: false }) datepickerTo!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdPartGradeDto = new InvCkdPartGradeDto();
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
    checkQty: boolean = true;
    _partName;
    _supplierNo;
    _orderPattern;
    _bodyColor = [{ value: '', label: '' }];
    datasEdits: GetGradebyPartListDto[] = [];
    orderPattern;
    _efFromPart;
    _efToPart;
    bodyColorArray = [];
    bodyColorList;
    checkEdit: boolean = true;

    orderPatternList = [
        { value: 'LOT', label: "LOT" },
        { value: 'PXP', label: "PXP" },
    ];
    shopList = [
        { value: 'W', label: "W" },
        { value: 'T', label: "T" },
        { value: 'A', label: "A" },
    ];
    cbbOrderPattern = [
        { value: 'PXP', label: "PXP" },
        { value: 'LOT', label: "LOT" },
    ]
    gradeList = [];
    gradeListc = [{ value: '', label: '' }];
    cbbGradeList = [{ value: '', label: '' }];

    gradeLists1 = [];
    gradeLists1_temp = [];
    colorList = [{ value: '', label: '' }];
    cbbcolorList = [{ value: '', label: '' }];
    gradeListbyCfc = [];
    _selectCfc;
    _listGradeCheck: GetGradebyPartListDto[] = [];
    item;
    _partId;
    v_efFromPart;
    v_efToPart;
    colors = [];
    isCreate: boolean = true;
    rowSelection = 'multiple';
    paginationGradeParams: PaginationParamsModel = {
        pageNum: 1,
        pageSize: 20,
        totalCount: 0,
        skipCount: 0,
        sorting: '',
        totalPage: 1,
    };

    //old value
    _startLotOld
    _endLotOld;
    _startRunOld;
    _endRunOld;
    _fromOld;
    _toOld;

    dStartLot;
    dEndLot;

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

    //by Viet
    _startLot;
    _startRun;
    _endLot;
    _endRun;
    _remark;

    _eciFromPart;
    _eciToPart;
    //by Viet

    constructor(
        injector: Injector,
        private _service: InvCkdPartListServiceProxy,
        private _dateTimeService: DateTimeService,
        private _partListComp: CkdPartListComponent,
    ) {
        super(injector);

    }
    show(rowdata?: InvCkdPartGradeDto): void {
        this.bodyColorList = [];
        this.gradeListc = [];
        this.colorList = [];
        this.gradeLists1 = [];
        //InvCkdPartGradeDto

        const dateValue1 = rowdata.efFromPart ? new Date(rowdata.efFromPart?.toString()) : null;
        this.datepickerFrom?.bsValueChange.emit(dateValue1);
        const dateValue2 = rowdata.efToPart ? new Date(rowdata.efToPart?.toString()) : null;
        this.datepickerTo?.bsValueChange.emit(dateValue2);

        this.rowdata = rowdata;

        this.cbbGradeList = [{ value: '', label: '' }];
        this._service.getListGradesByCfc(rowdata.cfc).subscribe(resultGrades => {
            resultGrades.forEach(e => {
                this.cbbGradeList.push({ value: e.grade, label: e.grade });
            })



        })

        // list color
        this.cbbcolorList = [];
        this._service.getListColors(rowdata.cfc, rowdata.grade).subscribe(resultColors => {
            if (resultColors) {
                resultColors.map(e => {
                    this.cbbcolorList.push({ value: e.color, label: e.color });
                })
            }

        })

        setTimeout(() => {

            this.bodyColorArray = [];
            this.bodyColorArray.push(rowdata.bodyColor);
            this._grade = rowdata.grade;
            this._remark = this.rowdata.remark;
            this._orderPattern = this.rowdata.orderPattern;
            this._usageQty = this.rowdata.usageQty;
            this._shop = this.rowdata.shop;
            this._endLot = this.rowdata.endLot == null ? null : this.rowdata.endLot.slice(this.rowdata.endLot.indexOf('-') + 1);
            this._startLot = this.rowdata.startLot == null ? null : this.rowdata.startLot.slice(this.rowdata.startLot.indexOf('-') + 1);
            this._startRun = this.rowdata.startRun;
            this._endRun = this.rowdata.endRun;
            this._startLotOld = this._startLot;
            this._endLotOld = this._endLot;
            this._startRunOld = this.rowdata.startRun;
            this._endRunOld = this.rowdata.endRun;
            // this._fromOld = this.rowdata.efFromPart;
            // this._toOld = this.rowdata.efToPart;
            this._eciFromPart = this.rowdata.eciFromPart;
            this._eciToPart = this.rowdata.eciToPart;
            this.checkEdit = true;
            this.modal.show();
        }, 500);

    }

    onChangeDlrSelect(e) {
        this.bodyColorList = e.filter(element => {
            return element !== null;
        });;
        this.bodyColorList = e;
    }

    validateStartLot(e) {
        if (e == '0') {
            this.notify.warn('Phải nhập StartLot > 0');
            document.getElementById('startLot').focus();
        }
        else if (e == '') {
            this._startRun = '';
        }
    }

    validateEndLot(e) {
        if (e == '') {
            this._endRun = '';
        }
        else if (e == '0') {
            this.notify.warn('Phải nhập endLot > 0');
            document.getElementById('endLot').focus();
        }
    }

    validateStartRun(e) {
        if (e != '') {
            if (Number(e) > 10) {
                this.notify.warn('Phải nhập startRun < 11');
                document.getElementById('startRun').focus();
            }
            else if (e == '0') {
                this.notify.warn('Phải nhập startRun > 0');
                document.getElementById('startRun').focus();
            }
        }
    }

    validateEndRun(e) {
        if (e != '') {
            if (Number(e) > 10) {
                this.notify.warn('Phải nhập endRun < 11');
                document.getElementById('endRun').focus();
            }
            else if (e == '0') {
                this.notify.warn('Phải nhập endRun > 0');
                document.getElementById('endRun').focus();
            }
        }

    }


    checkStartLot() {
        if (this._startLot == '' || this._startLot == null) {
            this.notify.warn('Phải nhập StartLot trước khi nhập trường StartRun');
            document.getElementById('startLot').focus();
        }
    }
    checkEndLot() {
        if (this._endLot == '' || this._endLot == null) {
            this.notify.warn('Phải nhập EndLot trước khi nhập trường EndRun');
            document.getElementById('endLot').focus();
        }
    }

    checkValUsageQty(e) {
        if (e == '' || Number(e) == 0) {
            this.notify.warn('Phải nhập Qty > 0');
            document.getElementById('usageQty').focus();
        }
    }

    save(): void {
        this.checkEdit = true;
        var efFromPart = this._dateTimeService.convertToDatetime(this._efFromPart);
        var efToPart = this._dateTimeService.convertToDatetime(this._efToPart);
        if (this._grade == '' || this._grade == null) {
            this.notify.warn(this.l('Chưa chọn Grade!'));
            document.getElementById('grade').focus();
            this.checkEdit = false;
            return;
        }
        else if (this._usageQty == '' || this._usageQty == null) {
            this.notify.warn(this.l('UsageQty phải nhập!'));
            document.getElementById('usageQty').focus();
            this.checkEdit = false;
            return;
        }
        else if (this._usageQty != '' && this._usageQty != null && this._usageQty == 0) {
            this.notify.warn(this.l('UsageQty phải nhập lớn hơn 0'));
            document.getElementById('usageQty').focus();
            this.checkEdit = false;
            return;
        }
        else if (this._shop == '' || this._shop == null) {
            this.notify.warn(this.l('Chưa chọn Shop!'));
            document.getElementById('shop').focus();
            this.checkEdit = false;
            return;
        }
        else if (this._startLot != null && this._startLot != '' && this._endLot != '' && this._endLot != null
            && (Number(this._endLot) < Number(this._startLot))) {
            this.notify.warn(this.l('EndLot phải lớn hơn StartLot'));
            document.getElementById('startLot').focus();
            this.checkEdit = false;
            return;
        }
        else if ((this._startLot == this._endLot) && this._startRun != '' && this._endRun != '' && this._startRun != null
            && this._endRun != null && Number(this._startRun) > Number(this._endRun)) {
            this.notify.warn(this.l('StartRun phải nhỏ hơn EndRun khi cùng Lot'));
            document.getElementById('startRun').focus();
            this.checkEdit = false;
            return;
        }

        else if (this._endLot != null && this._endLot != '' && this._endLot == 0) {
            this.notify.warn(this.l('EndLot phải lớn hơn 0'));
            document.getElementById('endLot').focus();
            this.checkEdit = false;
            return;
        }

        else if (this._startLot != null && this._startLot != '' && this._startLot == 0) {
            this.notify.warn(this.l('StartLot phải lớn hơn 0'));
            document.getElementById('startLot').focus();
            this.checkEdit = false;
            return;
        }

        else if (this._startLot != null && this._startLot != '' && (Number(this._startRun == 0) || Number(this._startRun > 10))) {
            this.notify.warn('StartRun trong khoảng từ 1 đến 10');
            document.getElementById('startRun').focus();
            this.checkEdit = false;
            return;
        }
        else if (this._endLot != null && this._endLot != '' && (Number(this._endRun == 0) || Number(this._endRun > 10))) {
            this.notify.warn('EndRun trong khoảng từ 1 đến 10');
            document.getElementById('endRun').focus();
            this.checkEdit = false;
            return;
        }

        else if (efFromPart == null && efToPart != null) {
            this.notify.warn('EfFromPart bị trống!');
            document.getElementById('efFromPart').focus();
            this.checkEdit = false;
            return;
        }
        else if (efFromPart != null && efToPart != null && efFromPart > efToPart) {
            this.notify.warn('EfFromPart không thể sau ngày EfToPart!');
            document.getElementById('efFromPart').focus();
            this.checkEdit = false;
            return;
        } else {
            this.checkEdit = true;
        }

        if (this.checkEdit) {
            var listColor;
            this.isLoading = true;
            if (this.bodyColorList != null) {
                listColor = this.bodyColorList.toString().charAt(0) == ',' ? this.bodyColorList.toString().substring(1, this.bodyColorList.toString().length) : this.bodyColorList.toString()
            }

            let input = Object.assign(new InvCkdPartGradeDto(), {
                id: this.rowdata.id,
                usageQty: Number(this._usageQty),
                shop: this._shop,
                grade: this._grade,
                bodyColor: (listColor == '' || listColor == null) ? null : listColor,
                startLot: (this._startLot == '' || this._startLot == null) ? null : this._grade + '-' + Number(this._startLot),
                endLot: (this._endLot == '' || this._endLot == null) ? null : this._grade + '-' + Number(this._endLot),
                startRun: (this._startRun == '' || this._startRun == null) ? null : Number(this._startRun),
                endRun: (this._endRun == '' || this._endRun == null) ? null : Number(this._endRun),
                // efFromPart: this._dateTimeService.convertToDatetime(this._efFromPart),
                // efToPart: this._dateTimeService.convertToDatetime(this._efToPart),
                eciFromPart: this._eciFromPart == '' ? null : this._eciFromPart,
                eciToPart: this._eciToPart == '' ? null : this._eciToPart,
                orderPattern: this._orderPattern,
                remark: this._remark == '' ? null : this._remark
            })

            this._service.partGradeEdit(input)
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this._partListComp.searchDatasGrade(this.rowdata.partId);
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
        }
    }

    close(): void {
        this._efFromPart = null;
        this._efToPart = null;
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
        this.bodyColorArray = [];
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
