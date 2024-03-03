import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdContainerRentalWHPlanDto, InvCkdContainerRentalWHPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-containerrentalwhplan-modal',
    templateUrl: './create-or-edit-containerrentalwhplan-modal.component.html',
})
export class CreateOrEditContainerRentalWHPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalContainerRentalWHPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerRequestDate', { static: false }) datepickerRequestDate!: BsDatepickerDirective;
    @ViewChild('datepickerCdDate', { static: false }) datepickerCdDate!: BsDatepickerDirective;
    @ViewChild('datepickerDevanningDate', { static: false }) datepickerDevanningDate!: BsDatepickerDirective;
    @ViewChild('datepickerActualDevanningDate', { static: false }) datepickerActualDevanningDate!: BsDatepickerDirective;
    @ViewChild('datepickerGateInPlanTime', { static: false }) datepickerGateInPlanTime!: BsDatepickerDirective;
    @ViewChild('datepickerGateInActualDateTime', { static: false }) datepickerGateInActualDateTime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdContainerRentalWHPlanDto = new CreateOrEditInvCkdContainerRentalWHPlanDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _requestDate;
    _cdDate;
    _devanningDate;
    _actualDevanningDate;
    _gateInPlanTime;
    _gateInActualDateTime;
    _status;

    constructor(
        injector: Injector,
        private _mstwptContainerRentalWHPlanService: InvCkdContainerRentalWHPlanServiceProxy,
        private _dateTimeService: DateTimeService
        ) {
        super(injector);
    }

    ///////////
    RestrictList = [
        {value: 'R' , label: "REQUESTED"},
        {value: 'P' , label: "PENDING"},
        {value: 'F' , label: "CONFIRM"},
        {value: 'C' , label: "CANCEL"},
    ];



    show(rowdata?: CreateOrEditInvCkdContainerRentalWHPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvCkdContainerRentalWHPlanDto();
        else this.rowdata = rowdata;

        const dateRequestDate = this.rowdata.requestDate ? new Date(this.rowdata.requestDate?.toString()) : null;
        this.datepickerRequestDate?.bsValueChange.emit(dateRequestDate);

        const dateCdDate = this.rowdata.cdDate ? new Date(this.rowdata.cdDate?.toString()) : null;
        this.datepickerCdDate?.bsValueChange.emit(dateCdDate);

        const dateDevanningDate = this.rowdata.devanningDate ? new Date(this.rowdata.devanningDate?.toString()) : null;
        this.datepickerDevanningDate?.bsValueChange.emit(dateDevanningDate);


        const dateActualDevanningDate = this.rowdata.actualDevanningDate ? new Date(this.rowdata.actualDevanningDate?.toString()) : null;
        this.datepickerActualDevanningDate?.bsValueChange.emit(dateActualDevanningDate);

        const dateGateInPlanTime = this.rowdata.gateInPlanTime ? new Date(this.rowdata.gateInPlanTime?.toString()) : null;
        this.datepickerGateInPlanTime?.bsValueChange.emit(dateGateInPlanTime);

        const dateGateInActualDateTimee = this.rowdata.gateInActualDateTime ? new Date(this.rowdata.gateInActualDateTime?.toString()) : null;
        this.datepickerGateInActualDateTime?.bsValueChange.emit(dateGateInActualDateTimee);

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'

        this.rowdata.requestDate = this._dateTimeService.convertToDatetime(this._requestDate),
        this.rowdata.cdDate = this._dateTimeService.convertToDatetime(this._cdDate),
        this.rowdata.devanningDate = this._dateTimeService.convertToDatetime(this._devanningDate),
        this.rowdata.actualDevanningDate = this._dateTimeService.convertToDatetime(this._actualDevanningDate),
        this.rowdata.gateInPlanTime = this._dateTimeService.convertToDatetime(this._gateInPlanTime),
        this.rowdata.gateInActualDateTime = this._dateTimeService.convertToDatetime(this._gateInActualDateTime),

        this._mstwptContainerRentalWHPlanService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
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

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === "Escape") {
            this.close();
        }
    }
}
