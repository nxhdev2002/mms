import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdReceivingPhysicalStockDto, InvCkdReceivingPhysicalStockServiceProxy} from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs';

@Component({
    selector: 'view-physicalstockreceiving-detail-modal',
    templateUrl: './view-physicalstockreceiving-detail-modal.component.html',
    })
export class ViewPhysicalStockReceivingDetailModalComponent extends AppComponentBase {
    @ViewChild('viewPhysicalStockReceivingDetailModal', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerWorkingDate', { static: false }) datepickerWorkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerTransactionDatetime', { static: false }) datepickerTransactionDatetime!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdReceivingPhysicalStockDto = new InvCkdReceivingPhysicalStockDto();

    active = false;
    // eslint-disable-next-line @typescript-eslint/no-inferrable-types
    saving: boolean = false;
    _isActive: boolean;
    _workingDate;
    _transactionDatetime;

    constructor(
        injector: Injector,
        private _mstwptPartReceivingService: InvCkdReceivingPhysicalStockServiceProxy,
        private _dateTimeService: DateTimeService
        ) {
        super(injector);

    }

    show(rowdata?: InvCkdReceivingPhysicalStockDto): void {
        if (!rowdata) this.rowdata = new InvCkdReceivingPhysicalStockDto();
        else this.rowdata = rowdata;
        const dateValueWorkingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : null;
		this.datepickerWorkingDate?.bsValueChange.emit(dateValueWorkingDate);
        const dateValueTransactionDatetime =  this.rowdata.transactionDatetime ? new Date(this.rowdata.transactionDatetime?.toString()) : null;
		this.datepickerTransactionDatetime?.bsValueChange.emit(dateValueTransactionDatetime);
        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
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
