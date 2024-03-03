import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvCkdRentalWarehouseDto, MstInvCkdRentalWarehouseServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-rentalwarehouse-modal',
    templateUrl: './create-or-edit-rentalwarehouse-modal.component.html',
})
export class CreateOrEditMstInvCkdRentalWarehouseModalComponent extends AppComponentBase {
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('createOrEditMstInvCkdRentalWarehouse', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker_f', { static: false }) datepicker_f!: BsDatepickerDirective;
    @ViewChild('datepicker_t', { static: false }) datepicker_t!: BsDatepickerDirective;
    @ViewChild('datepicker_devaningplan', { static: false }) datepicker_devaningplan!: BsDatepickerDirective;
    @ViewChild('datepicker_datetime', { static: false }) datepicker_datetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvCkdRentalWarehouseDto = new CreateOrEditMstInvCkdRentalWarehouseDto();

    active: boolean = false;
    saving: boolean = false;
    _fromDate: any;
    _toDate: any;
    _isSendEmail;
    _isActive;
    _isMainPic;

    constructor(
        injector: Injector,
        private _mstGpsSupplierPicService: MstInvCkdRentalWarehouseServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvCkdRentalWarehouseDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvCkdRentalWarehouseDto();
        else this.rowdata = rowdata;
        const fromDateValue = this.rowdata.fromDate ? new Date(this.rowdata.fromDate?.toString()) : null;
        this.datepicker_f?.bsValueChange.emit(fromDateValue);
        const toDateValue = this.rowdata.toDate ? new Date(this.rowdata.toDate?.toString()) : null;
        this.datepicker_t?.bsValueChange.emit(toDateValue);


        this._isActive = (this.rowdata.isActive == 'N') ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        if (this.validate()) {
        this.saving = true;
        this.rowdata.fromDate = this._dateTimeService.convertToDatetime(this._fromDate);
        this.rowdata.toDate = this._dateTimeService.convertToDatetime(this._toDate);
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstGpsSupplierPicService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
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
       
        /// validate ngày hết hạn, ngày nhập hàng
        if (this._fromDate !== null) {
            if (this._toDate !== null) {
                if (this._dateTimeService.convertToDatetime(this._fromDate) > this._dateTimeService.convertToDatetime(this._toDate)) {
                    this.message.warn(this.l('ToDate phải sau FromDate'));
                    return false;
                }
            }
        }

        return true;
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
