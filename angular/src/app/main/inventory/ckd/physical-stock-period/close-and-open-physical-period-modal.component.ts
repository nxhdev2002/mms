import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdPhysicalStockPeriodDto, InvCkdPhysicalStockPeriodServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { log } from 'console';
import { DatePipe } from '@angular/common';


@Component({
    selector: 'close-and-open-physicalperiod-modal',
    templateUrl: './close-and-open-physical-period-modal.component.html'
})
export class CloseAndOpenPhysicalPeriodModalComponent extends AppComponentBase {
    @ViewChild('closeOpenModalPhysicalPeriod', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerFrom', { static: false }) datepickerFrom!: BsDatepickerDirective;
    @ViewChild('datepickerTo', { static: false }) datepickerTo!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdPhysicalStockPeriodDto = new CreateOrEditInvCkdPhysicalStockPeriodDto();

    active: boolean = false;
    saving: boolean = false;
    isLoading;
    _description: string = '';
    _fromDate: any;
    _fromTime;
    _toDate: any;
    _toTime;
    pipe = new DatePipe('en-US');

    constructor(
        injector: Injector,
        private _mstwptPhysicalStockPeriodService: InvCkdPhysicalStockPeriodServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvCkdPhysicalStockPeriodDto): void {
        this.active = true;
        this.modal.show();
        this._toDate = '';
        this._fromDate = this.addDays(rowdata.toDate,1)
        this._fromTime = '07:00'
        this._toTime = '18:00'
        this._description = '';
    }

    addDays(theDate, days) {
        return new Date(theDate + 864E5 * days);
    }

    save(): void {
        this.saving = true;
        if (this._fromDate != null && this._toDate != null && this._description != '') {
            if (this._toDate < this._fromDate) {
                this.message.warn(this.l('To Date không được nhỏ hơn From Date!'), 'Warning');
            }
            else {
                this.message.confirm(this.l('Bạn chắc chắn muốn thay đổi không?'), 'Confirm Edit Stock Period', (isConfirmed) => {
                    if (isConfirmed) {
                        this.isLoading = true;
                        this._mstwptPhysicalStockPeriodService.getAllCreate(
                            this._description,
                            this._dateTimeService.convertToDatetime(this._fromDate),
                            this._fromTime,
                            this._dateTimeService.convertToDatetime(this._toDate),
                            this._toTime,
                        )
                            .pipe(finalize(() => this.saving = false))
                            .subscribe(() => {
                                this.isLoading = false;
                                this.notify.info(this.l('SavedSuccessfully'));
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
