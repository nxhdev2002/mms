import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdShippingScheduleDetailsFirmDto, InvCkdShippingScheduleFirmDto, InvCkdShippingScheduleFirmServiceProxy, ICreateOrEditInvCkdShippingScheduleDetailsFirmDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { isNumber } from 'lodash-es';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import * as moment from 'moment';


@Component({
    selector: 'create-or-edit-shippingscheduledetailsfirm-modal',
    templateUrl: './create-or-edit-shippingscheduledetailsfirm-modal.component.html',
    styleUrls: ['./create-or-edit-shippingscheduledetailsfirm-modal.component.less']
})
export class CreateOrEditShippingScheduleDetailsFirmModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalShippingScheduleDetailsFirm', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerPackingDate', { static: false }) datepickerPackingDate!: BsDatepickerDirective;
    @ViewChild('datepickerPackingMonth', { static: false }) datepickerPackingMonth!: BsDatepickerDirective;
    @ViewChild('datepickerShippingMonth', { static: false }) datepickerShippingMonth!: BsDatepickerDirective;
    @ViewChild('datepickerVesselEtd1st', { static: false }) datepickerVesselEtd1st!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    cbxPartList = [];
    rowdata: CreateOrEditInvCkdShippingScheduleDetailsFirmDto = new CreateOrEditInvCkdShippingScheduleDetailsFirmDto();
    minwidth0 = "minwidth0";
    active: boolean = false;
    saving: boolean = false;
    _packingDate
    _packingMonth
    _shippingMonth
    _vesselEtd1st;
    _: boolean;
    colClass = 'col-4';
    chunks = [];
    numberFields;
    itemsPerRow;
    exceptFieldsFromRender;

    fn = new CommonFunction();
    constructor(
        injector: Injector,
        private _service: InvCkdShippingScheduleFirmServiceProxy,
        private _dateTimeService: DateTimeService,
    )
    {
        super(injector);

        this.itemsPerRow = 3;
        this.exceptFieldsFromRender = [
            'id',
            'valuationTypeFrom',
            'revisionNo'
        ]

        this.numberFields = [
            'orderLot', 'scheduleQty', 'containerSize', 'm3OfModule', 'grossWeightOfModule'
        ];
    }



    show(rowdata?, headerId?): void {
        this.cbxPartList = [];
        this.rowdata = rowdata ? rowdata : new CreateOrEditInvCkdShippingScheduleDetailsFirmDto();
        // trim các property của rowdata
        Object.keys(this.rowdata).forEach(key => typeof rowdata[key] === 'string' ? rowdata[key] = rowdata[key].trim() : null);

        this.rowdata.shipmentHeaderId = headerId;


        const packingDate = this.rowdata.packingDate ? moment(this.rowdata.packingDate?.toString(), "YYYYMMDD").toDate() : null;
        this.datepickerPackingDate?.bsValueChange.emit(packingDate);

        const packingMonth = this.rowdata.packingMonth ? moment(this.rowdata.packingMonth?.toString(), "YYYYMM").toDate() : null;
        this.datepickerPackingMonth?.bsValueChange.emit(packingMonth);

        const shippingMonth = this.rowdata.shippingMonth ? moment(this.rowdata.shippingMonth?.toString(), "YYYYMM").toDate() : null;
        this.datepickerShippingMonth?.bsValueChange.emit(shippingMonth);

        const vesselEtd1st = this.rowdata.vesselEtd1st ? moment(this.rowdata.vesselEtd1st?.toString(), "YYYYMMDD").toDate() : null;
        this.datepickerVesselEtd1st?.bsValueChange.emit(vesselEtd1st);
        // this.renderForm();

        this._service.getListPartNo().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.cbxPartList.push({ value: e.partNo, label: e.partNo });
                })
            }
        })
        /// CÔNG NGHỆ MỚI ĐANG NGHIÊN CỨU
        // this.parseFields();
        this.modal.show();
    }

    parseFields() {
        this.chunks = [];
        let i = 0;
        // tách mảng các properties của object thành mảng gồm các mảng con có itemsPerRow phần tử
        // mục đích để lặp từng mảng con render thành các row
        let allRenderFields = Object.keys(this.rowdata).filter(
            x => !this.exceptFieldsFromRender.includes(x)
        );

        while (i < allRenderFields.length) {
            this.chunks.push(allRenderFields.slice(i, i + this.itemsPerRow));
            i += this.itemsPerRow;
        }

        // tính col class để style bootstrap
        this.colClass = "col-" + 12 / this.itemsPerRow;
    }


    normalizeString(inputStr) {
        // Hàm chuyển tên biến, property thành chữ đọc được. VD: lotNo -> Lot No
        // Use regular expression to find camelCase or PascalCase words and numbers
        var words = inputStr.match(/[A-Z]?[a-z0-9]+|[A-Z]+(?=[A-Z0-9]|$)|\d+/g);

        // Join the words with spaces and capitalize the first letter of each word
        var result = words.map(word => {
            if (!isNaN(word)) {
                return word;  // If the word is a number, keep it as it is
            }
            return word.charAt(0).toUpperCase() + word.slice(1); // Capitalize the first letter of non-numeric words
        }).join(' ');

        return result;
    }

    getObjectProperties(obj): { name: string, value: string }[] {
        const properties = [];

        for (const prop in obj) {
          if (obj.hasOwnProperty(prop)) {
            properties.push({ name: prop, value: obj[prop] });
          }
        }

        return properties;
      }


    changeActive(event){
        this._ = event.target.checked;
    }

    save(): void {
        if(this.validate()){
            console.log(this._packingDate);
            this.rowdata.packingDate = this._packingDate ? moment(this._packingDate).format("YYYYMMDD") : null;
            this.rowdata.packingMonth = this._packingMonth ? moment(this._packingMonth).format("YYYYMM") : null;
            this.rowdata.shippingMonth = this._shippingMonth ? moment(this._shippingMonth).format("YYYYMM") : null;
            this.rowdata.vesselEtd1st = this._vesselEtd1st ? moment(this._vesselEtd1st).format("YYYYMMDD") : null;

            this.saving = true;
            this._service.createOrEdit(this.rowdata)
                .pipe( finalize(() =>  this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            this.saving = false;
        }
    }

    validate() {
        // validate các property của this.rowdata là số sử dụng regex: orderLot, scheduleQty
        // nếu không phải số thì trả về false
        // nếu là số thì trả về true
        let isValidated = true;

        const numberPattern = /^[1-9]\d*$/;

        this.numberFields.forEach(field => {
            if (!numberPattern.test(this.rowdata[field]) && this.rowdata[field] != null) {
                this.notify.warn(this.l('Trường ' + this.normalizeString(field) + ' phải là số dương'));
                isValidated = false;
            }
        });

        if (this._packingDate && this._packingMonth) {
            if (this._packingDate.getFullYear() !== this._packingMonth.getFullYear() || this._packingDate.getMonth() !== this._packingMonth.getMonth()) {
                this.notify.warn(this.l('Packing Date phải trong thời gian Packing Month'));
                isValidated = false;
            }
        }

        if (this._packingMonth && this._shippingMonth) {
            if (moment(this._packingMonth).toDate() > moment(this._shippingMonth).toDate()) {
                this.notify.warn(this.l('Trường Packing Month phải <= Shipping Month'));
                isValidated = false;
            }
        }

        return isValidated;
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
