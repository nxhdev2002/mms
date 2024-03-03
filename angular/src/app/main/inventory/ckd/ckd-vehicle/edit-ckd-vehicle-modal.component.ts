import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdVehhicleServiceProxy, InvCkdVehicleDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { formatDate } from '@angular/common';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { finalize } from 'rxjs/operators';
import * as moment from 'moment';
import { CkdVehicleComponent } from './ckd-vehicle.component';

@Component({
    selector: 'edit-ckd-vehicle-details-modal',
    templateUrl: './edit-ckd-vehicle-modal.component.html',
})
export class EditCKDVehicleDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerwInDateActual', { static: false }) datepickerwInDateActual!: BsDatepickerDirective;
    @ViewChild('datepickerPIODate', { static: false }) datepickerPIODate!: BsDatepickerDirective;
    @ViewChild('datepickerSalesDate', { static: false }) datepickerSalesDate!: BsDatepickerDirective;


   

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdVehicleDto = new InvCkdVehicleDto();

    active = false;
    saving: boolean = false;
    isActive: boolean;
    _WInActual: any;
    _WOutActual :any;
    _TInActual: any;
    _TOutActual: any;
    _AInActual: any;
    _AOutActual: any;
    _LineoffDateTime: any;
    _PdiDateTime: any;
    _PIODate: any;
    _SalesDate: any;
    Line
    constructor(
        injector: Injector,
        private _service: InvCkdVehhicleServiceProxy,
        private _dateTimeService: DateTimeService,
        private _serviceVehicle: CkdVehicleComponent,
    ) {
        super(injector);
    }

    show(rowdata?: InvCkdVehicleDto): void {
        if (!rowdata) this.rowdata = new InvCkdVehicleDto();
        else this.rowdata = rowdata;
        const dateSalesDate = this.rowdata.salesActualDate ? new Date(this.rowdata.salesActualDate?.toString()) : null;
        this.datepickerSalesDate?.bsValueChange.emit(dateSalesDate);

        const datePIODate1 = this.rowdata.pioActualDate ? new Date(this.rowdata.pioActualDate?.toString()) : null;
        this.datepickerPIODate?.bsValueChange.emit(datePIODate1);
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === 'Escape') {
            this.close();
        }
    }

    save(): void {
        //console.log(this.rowdata.pdiDateTime_YYYYDDMM)
            this.saving = true;
            this.rowdata.wInActualDateTime =this.rowdata.wInActualDateTime_YYYYDDMM ?  DateTime.fromISO(this.rowdata.wInActualDateTime_YYYYDDMM) : null;
            this.rowdata.wOutActualDateTime = this.rowdata.wOutActualDateTime_YYYYDDMM ? DateTime.fromISO(this.rowdata.wOutActualDateTime_YYYYDDMM): null;
            this.rowdata.tInActualDateTime = this.rowdata.tInActualDateTime_YYYYDDMM ? DateTime.fromISO(this.rowdata.tInActualDateTime_YYYYDDMM) : null;
            this.rowdata.tOutActualDateTime = this.rowdata.tOutActualDateTime_YYYYDDMM?  DateTime.fromISO(this.rowdata.tOutActualDateTime_YYYYDDMM) : null;
            this.rowdata.aInActualDateTime =this.rowdata.aInActualDateTime_YYYYDDMM? DateTime.fromISO(this.rowdata.aInActualDateTime_YYYYDDMM) : null;
            this.rowdata.aOutActualDateTime =this.rowdata.aOutActualDateTime_YYYYDDMM?  DateTime.fromISO(this.rowdata.aOutActualDateTime_YYYYDDMM): null;
            this.rowdata.lineoffDateTime =this.rowdata.lineoffDateTime_YYYYDDMM ?  DateTime.fromISO(this.rowdata.lineoffDateTime_YYYYDDMM) :null;
            this.rowdata.pdiDateTime =this.rowdata.pdiDateTime_YYYYDDMM?  DateTime.fromISO(this.rowdata.pdiDateTime_YYYYDDMM): null;
            this.rowdata.pioActualDate = this._dateTimeService.convertToDatetime(this._PIODate),
            this.rowdata.salesActualDate = this._dateTimeService.convertToDatetime(this._SalesDate),
            this._service.update(this.rowdata)
            .pipe(finalize(() => this.saving = false))
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modal?.hide();
                this.modalSave.emit(this.rowdata);
            });
        }
    
}
