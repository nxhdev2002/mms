import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgaLdsLotPlanDto, LgaLdsLotPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import * as moment from 'moment';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
 
@Component({
    selector: 'create-or-edit-lotplan-modal',
    templateUrl: './create-or-edit-lotplan-modal.component.html',
    styleUrls: ['./create-or-edit-lotplan-modal.component.less']
    })
export class CreateOrEditLotplanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLotPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerplanStartDatetime', { static: false }) datepickerplanStartDatetime!: BsDatepickerDirective;
    @ViewChild('startDatetime', { static: false }) startDatetime!: BsDatepickerDirective;
    @ViewChild('finishDatetime', { static: false }) finishDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgaLdsLotPlanDto = new CreateOrEditLgaLdsLotPlanDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate : any ;
	_planStartDatetime : any ;
    _startDatetime : any ;
	_finishDatetime : any ;

    constructor(
        injector: Injector,
        private _mstwptLotPlanService: LgaLdsLotPlanServiceProxy,
        private _dateTimeService: DateTimeService
        
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgaLdsLotPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgaLdsLotPlanDto();
        else this.rowdata = rowdata;
		const workingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : null;
		this.datepicker?.bsValueChange.emit(workingDate);

		const planStartDatetime =  this.rowdata.planStartDatetime ? new Date(this.rowdata.planStartDatetime?.toString()) : null;
		this.datepickerplanStartDatetime?.bsValueChange.emit(planStartDatetime);

		 const startDatetime =  this.rowdata.startDatetime ? new Date(this.rowdata.startDatetime?.toString()) : null;
		 this.startDatetime?.bsValueChange.emit(startDatetime);

		const finishDatetime =  this.rowdata.finishDatetime ? new Date(this.rowdata.finishDatetime?.toString()) : null;
		this.finishDatetime?.bsValueChange.emit(finishDatetime);



        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.planStartDatetime = this._dateTimeService.convertToDatetime(this._planStartDatetime),
    this.rowdata.startDatetime = this._dateTimeService.convertToDatetime(this._startDatetime),
    this.rowdata.finishDatetime = this._dateTimeService.convertToDatetime(this._finishDatetime),
    this._mstwptLotPlanService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            //this.modalSave.emit(this.rowdata);
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
