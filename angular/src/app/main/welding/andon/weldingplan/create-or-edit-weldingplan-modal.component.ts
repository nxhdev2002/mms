import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditWldAdoWeldingPlanDto, WldAdoWeldingPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-weldingplan-modal',
    templateUrl: './create-or-edit-weldingplan-modal.component.html',
    })
export class CreateOrEditWeldingPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalWeldingPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerRequest', { static: false }) datepickerRequest!: BsDatepickerDirective;
    @ViewChild('datepickerwIn', { static: false }) datepickerwIn!: BsDatepickerDirective;
    @ViewChild('datepickerwOut', { static: false }) datepickerwOut!: BsDatepickerDirective;
    @ViewChild('datepickeredIn', { static: false }) datepickeredIn!: BsDatepickerDirective;
    @ViewChild('datepickeredPlan', { static: false }) datepickeredPlan!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditWldAdoWeldingPlanDto = new CreateOrEditWldAdoWeldingPlanDto();
    requestDate;
    wInDate;
    wOutDate;
    edIn;
    tInPlanDatetime;

    active: boolean = false;
    saving: boolean = false;
    _: boolean;

    constructor(
        injector: Injector,
        private _mstwptWeldingPlanService: WldAdoWeldingPlanServiceProxy,
        private _datetimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditWldAdoWeldingPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditWldAdoWeldingPlanDto();
        else this.rowdata = rowdata;
		const dateValuerequestDate =  this.rowdata.requestDate ? new Date(this.rowdata.requestDate?.toString()) : null;
		this.datepickerRequest?.bsValueChange.emit(dateValuerequestDate);

		const dateValuewInDate =  this.rowdata.wInDate ? new Date(this.rowdata.wInDate?.toString()) : null;
		this.datepickerwIn?.bsValueChange.emit(dateValuewInDate);

		const dateValuewOutDate =  this.rowdata.wOutDate ? new Date(this.rowdata.wOutDate?.toString()) : null;
		this.datepickerwOut?.bsValueChange.emit(dateValuewOutDate);

		const dateValue =  this.rowdata.edIn ? new Date(this.rowdata.edIn?.toString()) : null;
		this.datepickeredIn?.bsValueChange.emit(dateValue);

        const dateValuePlan =  this.rowdata.tInPlanDatetime ? new Date(this.rowdata.tInPlanDatetime?.toString()) : null;
		this.datepickeredPlan?.bsValueChange.emit(dateValuePlan);

        this.modal.show();
    }


    save(): void {
        let postData = new CreateOrEditWldAdoWeldingPlanDto();

        postData = Object.assign(this.rowdata, {
            requestDate: this._datetimeService.convertToDatetime(this.requestDate),
            wInDate: this._datetimeService.convertToDatetime(this.wInDate),
            wOutDate: this._datetimeService.convertToDatetime(this.wOutDate),
            edIn: this._datetimeService.convertToDatetime(this.edIn),
            tInPlanDatetime: this._datetimeService.convertToDatetime(this.tInPlanDatetime)
        })


        this.saving = true;
        this._mstwptWeldingPlanService.createOrEdit(postData)
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

    @HostListener('document:keydown', ['$event'])
        onKeydownHandler(event: KeyboardEvent) {
            if (event.key === "Escape") {
                this.close();
            }
        }
}
