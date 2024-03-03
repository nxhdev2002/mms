import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvGpsDailyOrderDto, InvGpsDailyOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { DateTime } from 'luxon';


@Component({
    selector: 'create-or-edit-invgpsdailyorder-modal',
    templateUrl: './create-or-edit-invgpsdailyorder-modal.component.html',
    styleUrls: ['./create-or-edit-invgpsdailyorder-modal.component.less']
    })
export class CreateOrEditInvGpsDailyOrderModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvGpsDailyOrder', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerWorkingDate', { static: false }) datepickerWorkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerOrderDatetime', { static: false }) datepickerOrderDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerEstArrivalDatetime', { static: false }) datepickerEstArrivalDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsDailyOrderDto = new CreateOrEditInvGpsDailyOrderDto();

    active = false;
    saving = false;
    _isActive: boolean;
    _workingDate;
    _orderDatetime;
    _estArrivalDatetime;

    constructor(
        injector: Injector,
        private _mstwptInvGpsDailyOrderService: InvGpsDailyOrderServiceProxy,
          private _dateTimeService: DateTimeService

    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsDailyOrderDto): void {
        if (!rowdata) {
        this.rowdata = new CreateOrEditInvGpsDailyOrderDto();
        } else {
        this.rowdata = rowdata;
        }
        const dateValueWorkingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : null;
		this.datepickerWorkingDate?.bsValueChange.emit(dateValueWorkingDate);
        this._orderDatetime = new Date().toISOString().slice(0, 16);
        this._estArrivalDatetime = new Date();

        // eslint-disable-next-line eqeqeq
        this._isActive = (this.rowdata.isActive == 'N') ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    // eslint-disable-next-line @typescript-eslint/no-unused-expressions, no-unused-expressions
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.orderDatetime = DateTime.fromISO(this._orderDatetime);
    this.rowdata.estArrivalDatetime = DateTime.fromISO(this._estArrivalDatetime);
    // eslint-disable-next-line eqeqeq
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
    this._mstwptInvGpsDailyOrderService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            this.modalSave.emit(this.rowdata);
        });
    this.saving = false;
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    // eslint-disable-next-line @typescript-eslint/member-ordering
    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === 'Escape') {
                this.close();
            }
        }
}
