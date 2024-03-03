import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdPhysicalStockPeriodDto, InvCkdPhysicalStockPeriodServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DatePipe } from '@angular/common';




@Component({
    selector: 'edit-physicalperiod-modal',
    templateUrl: './edit-physical-period-modal.component.html'
})
export class EditPhysicalPeriodModalComponent extends AppComponentBase {
    @ViewChild('closeOpenModalPhysicalPeriod', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerFrom', { static: false }) datepickerFrom!: BsDatepickerDirective;
    @ViewChild('datepickerTo', { static: false }) datepickerTo!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdPhysicalStockPeriodDto = new CreateOrEditInvCkdPhysicalStockPeriodDto();

    active: boolean = false;
    saving: boolean = false;
    _description: string = '';
    _fromDate: any;
    _toDate :any;
    fromDateTemp: any;
    pipe = new DatePipe('en-US');


    constructor(
        injector: Injector,
        private _mstwptPhysicalStockPeriodService: InvCkdPhysicalStockPeriodServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvCkdPhysicalStockPeriodDto): void {
        this.rowdata = rowdata;
        this._description = rowdata.description;
        const dateFrom = rowdata.fromDate ? new Date(rowdata.fromDate?.toString()) : null;
        this.datepickerFrom?.bsValueChange.emit(dateFrom);
        const dateTo = rowdata.toDate ? new Date(rowdata.toDate?.toString()) : null;
        this.datepickerTo?.bsValueChange.emit(dateTo);
        this.fromDateTemp = rowdata.fromDate ? new Date(rowdata.fromDate.toString()) : null;
        this.active = true;
        this.modal.show();
    }

    addDays(theDate, days) {
        return new Date(theDate + 864E5*days);
    }

    save(): void {
        this.saving = true;
        this._fromDate = this.fromDateTemp;
        this.rowdata.description = this._description;
        this.rowdata.fromDate = this._dateTimeService.convertToDatetime(this._fromDate);
        this.rowdata.toDate = this._dateTimeService.convertToDatetime(this._toDate);
        if (this._fromDate != null && this._toDate != null && this._description != '') {
            if (this._toDate < this._fromDate) {
                this.message.warn(this.l('To Date không được nhỏ hơn From Date!'), 'Warning');
            }
            else {
                this.message.confirm(this.l('Bạn chắc chắn muốn sửa?'), 'Confirm Edit Stock Period', (isConfirmed) => {
                    if (isConfirmed) {
                        this._mstwptPhysicalStockPeriodService.createOrEdit(this.rowdata)
                            .pipe(finalize(() => this.saving = false))
                            .subscribe(() => {
                                this.notify.info(this.l('EditSuccessfully'));
                                this.close();
                                this.modal?.hide();
                                this.modalSave.emit(this.rowdata);
                            });
                    }
                });

            }
        } else {
            this.message.warn(this.l('Description, To Date và From Date không được để trống!'), 'Warning');
        }

        this.saving = false;
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }
}
