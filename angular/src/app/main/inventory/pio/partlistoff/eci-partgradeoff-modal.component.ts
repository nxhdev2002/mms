import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { GetGradebyPartListDto, InvCkdPartGradeDto, InvCkdPartListServiceProxy, InvPartListOffServiceProxy, InvPioPartGradeInlDto, InvPioPartGradeOffDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CustomColDef, FrameworkComponent, GridParams, PaginationParamsModel } from '@app/shared/common/models/base.model';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { formatDate } from '@angular/common';
import { PioPartListOffComponent } from './partlistoff.component';




@Component({
    selector: 'eci-partgradeoff-modal',
    templateUrl: './eci-partgradeoff-modal.component.html',
    styleUrls: ['./eci-partgradeoff-modal.component.less']
})
export class EciPartGradeOffModalComponent extends AppComponentBase {
    @ViewChild('eciPartGradeOffModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerFrom', { static: false }) datepickerFrom!: BsDatepickerDirective;
    @ViewChild('datepickerTo', { static: false }) datepickerTo!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvPioPartGradeOffDto = new InvPioPartGradeOffDto();
    partListColDef: CustomColDef[] = [];
    frameworkComponents: FrameworkComponent;
    isLoading: boolean = false;
    dataseci: GetGradebyPartListDto[] = [];
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
    datasecis: GetGradebyPartListDto[] = [];
    orderPattern;
    _efFromPart;
    _efToPart;
    bodyColorArray = [];
    bodyColorList;
    checkeci: boolean = true;

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
    _endLotNumber;
    //by Viet

    constructor(
        injector: Injector,
        private _service: InvPartListOffServiceProxy,
        private _dateTimeService: DateTimeService,
        private _partListComp: PioPartListOffComponent,
    ) {
        super(injector);

    }
    show(rowdata?: InvPioPartGradeOffDto): void {
        this.bodyColorList = [];
        this.gradeListc = [];
        this.colorList = [];
        this.gradeLists1 = [];
        //InvCkdPartGradeDto
        console.log(this.rowdata.endLot);


        setTimeout(() => {
            this.isCreate = true;
            this.rowdata = rowdata;
            if (this.rowdata.endLot != null) {
                var endLotNumber = Number(this.rowdata.endLot.substring(this.rowdata.endLot.indexOf("-") + 1, this.rowdata.endLot.length));
                if(endLotNumber <9999){
                this._endLotNumber = endLotNumber;
                // this.rowdata.startLot = Number(this.rowdata.endRun) == 10 ? (this.rowdata.grade + '-' + (endLotNumber+1).toString()) : this.rowdata.endLot;
                this._startLot = Number(this.rowdata.endRun) == 10 ? (endLotNumber + 1).toString() : this.rowdata.endLot.slice(this.rowdata.endLot.indexOf('-') + 1);
                // this.rowdata.startRun = Number(this.rowdata.endRun) == 10 ? '1' :  (Number(this.rowdata.endRun) + 1).toString();
                this._startRun = Number(this.rowdata.endRun) == 10 ? 1 : (this.rowdata.endRun != null ? (Number(this.rowdata.endRun) + 1) : (Number(this.rowdata.startRun) + 1)).toString();
                //this._startRun = Number(this.rowdata.endRun) == 10 ? 1 : (Number(this.rowdata.endRun) + 1).toString();
                const dateValue1 = null;
                this.datepickerFrom?.bsValueChange.emit(dateValue1);
                const dateValue2 = null;
                this.datepickerTo?.bsValueChange.emit(dateValue2);
                this._endLot = '';
                this._endRun = '';
                this.rowdata.remark = '';
                this._usageQty = this.rowdata.usageQty;
                }
            } else {
                this._startLot = '';
                this._startRun = '';
                const dateValue1 = null;
                this.datepickerFrom?.bsValueChange.emit(dateValue1);
                const dateValue2 = null;
                this.datepickerTo?.bsValueChange.emit(dateValue2);
                this._endLot = '';
                this._endRun = '';
                this.rowdata.remark = '';
                this._usageQty  = '';
            }

            this.active = true;
            this.modal.show();
        }, 500);

        // this.checkeci = true;
        // this.rowdata = rowdata;
        // this._efFromPart = this.rowdata.efFromPart ? formatDate(new Date(this.rowdata.efFromPart.toString()), 'dd/MM/yyyy', 'en-US'): undefined;
        // this._efToPart = this.rowdata.efToPart ? formatDate(new Date(this.rowdata.efToPart.toString()), 'dd/MM/yyyy', 'en-US'): undefined;
        // this._endLot = this.rowdata.endLot == null ? null : this.rowdata.endLot.slice(this.rowdata.endLot.indexOf('-') + 1);
        // this._startLot = this.rowdata.startLot == null ? null : this.rowdata.startLot.slice(this.rowdata.startLot.indexOf('-') + 1);
        // this._startRun = this.rowdata.startRun;
        // this.modal.show();
    }

    clearGradeStart() {
        if (this.rowdata.startLot != null) {
            this.rowdata.startLot = this.rowdata.startLot.substring(this.rowdata.startLot.indexOf("-") + 1, this.rowdata.startLot.length);
        }
    }

    validateStartLot(e) {
        if (e == '0') {
            this.notify.warn('Phải nhập StartLot > 0');
            document.getElementById('startLot1').focus();
        }
        else if (e == '') {
            this._startRun = '';
        }
    }

    validateStartRun(e) {
        if (e != '') {
            if (Number(e) > 10) {
                this.notify.warn('Phải nhập startRun < 11');
                document.getElementById('startRun1').focus();
            }
            else if (e == '0') {
                this.notify.warn('Phải nhập startRun > 0');
                document.getElementById('startRun1').focus();
            }
        }
    }

    checkStartLot() {
        const reg = /^[1-9]\d{0,3}$/;
        if (this._startLot == '' || this._startLot == null || !(reg.test(this._startLot))) {
            this.notify.warn('Phải nhập StartLot trước khi nhập trường StartRun');
            document.getElementById('startLot1').focus();
        }
    }

    checkValUsageQty(e) {
        if (e == '' || Number(e) == 0) {
            this.notify.warn('Phải nhập Qty > 0');
            document.getElementById('usageQty').focus();
        }
    }

    save(): void {
        this.checkeci = true;
        // if (this._startRun != '' && this._endLot != null && this.rowdata.endRun != null
        //     && Number(this._startLot) == Number(this._endLot) && Number(this._startRun) != Number(this.rowdata.endRun)) {
        //     this.notify.warn('StartRun phải bẳng EndRun!');
        //     document.getElementById('startRun').focus();
        //     this.checkeci = false;
        //     return;
        // }
        if (this._startRun != '' && this._startLot == '') {
            this.notify.warn('Chưa nhập StartLot!');
            document.getElementById('startLot1').focus();
            this.checkeci = false;
            return;
        }
        else if(this._startLot != null && this._startLot != null && this._startLot == 0){
            this.notify.warn(this.l('StartLot phải lớn hơn 0'));
            document.getElementById('startLot1').focus();
            this.checkeci = false;
            return;
        }
        else if (this._startRun != '' && (Number(this._startRun) == 0 || Number(this._startRun) > 10)) {
            this.notify.warn('SatrtRun chỉ cho phép nhập từ 1 đến 10');
            document.getElementById('startRun1').focus();
            this.checkeci = false;
            return;
        }
        else if (this._startRun == '' && this._startLot == '') {
            this.notify.warn('StartLot,EndLot không được để trống');
            document.getElementById('startLot1').focus();
            this.checkeci = false;
            return;
        }

        else if (!this._usageQty) {
            this.notify.warn('UsageQty không được để trống');
            document.getElementById('usageQty').focus();
            this.checkeci = false;
            return;
        }else{
            this.checkeci = true;
        }

        if ((this._startLot != '' && this._startLot != null)
            && (this._startRun != '' && this._startRun != null)) {
            if ((this.rowdata.endRun != '' && this.rowdata.endRun != null)
                && (this.rowdata.endLot != '' && this.rowdata.endLot != null)
                && Number(this._startLot) < Number(this.rowdata.endLot.substring(this.rowdata.endLot.indexOf("-") + 1, this.rowdata.endLot.length))) {
                this.notify.warn('StartLotNew phải lớn hơn hoặc bằng EndLotOld');
                document.getElementById('startRun').focus();
                this.checkeci = false;
                return;
            } else if (Number(this._startLot) < Number(this.rowdata.startLot.substring(this.rowdata.startLot.indexOf("-") + 1, this.rowdata.startLot.length))) {
                this.notify.warn('StartLotNew phải lớn hơn hoặc bằng EndLotOld');
                document.getElementById('startRun').focus();
                this.checkeci = false;
                return;
            }
            else if ((this.rowdata.endRun != '' && this.rowdata.endRun != null)
                && (this.rowdata.endLot != '' && this.rowdata.endLot != null)
                && Number(this._startLot) == Number(this.rowdata.endLot.substring(this.rowdata.endLot.indexOf("-") + 1, this.rowdata.endLot.length))
                && Number(this._startRun) <= Number(this.rowdata.endRun)) {
                this.notify.warn('Cùng Lot thì StartRunNew phải lớn hơn EndRunOld');
                document.getElementById('startRun').focus();
                this.checkeci = false;
                return;
            } else if ((this.rowdata.startRun != '' && this.rowdata.startRun != null)
                && (this.rowdata.startLot != '' && this.rowdata.startLot != null)
                && Number(this._startLot) == Number(this.rowdata.startLot.substring(this.rowdata.startLot.indexOf("-") + 1, this.rowdata.startLot.length))
                && Number(this._startRun) <= Number(this.rowdata.startRun)) {
                this.notify.warn('Cùng Lot thì StartRunNew phải lớn hơn EndRunOld');
                document.getElementById('startRun').focus();
                this.checkeci = false;
                return;
            } else {
                this.checkeci = true;
            }

        }


        this.rowdata.startLot = this.rowdata.grade + '-' + this._startLot;
        this.rowdata.startRun = this._startRun;
        this.rowdata.usageQty = Number(this._usageQty);
        if (this.checkeci) {
            this.isLoading = true;
            this._service.eciPartGrade(this.rowdata.id, this.rowdata.startLot, this.rowdata.startRun, this.rowdata.grade,this.rowdata.usageQty )
                .pipe(finalize(() => this.isLoading = false))
                .subscribe(() => {
                    this.notify.success(this.l('Eci Successfully'));
                    this._partListComp.searchDatasGrade(this.rowdata.partId);
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                    this.isLoading = false;
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
