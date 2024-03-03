import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgwPupPxPUpPlanDto, LgwPupPxPUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-pxpupplan-modal',
    templateUrl: './create-or-edit-pxpupplan-modal.component.html',
    styleUrls: ['./create-or-edit-pxpupplan-modal.component.less']
})
export class CreateOrEditPxPUpPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPxPUpPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerworkingDate', { static: false }) datepickerworkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerunpackingStartDatetime', { static: false }) datepickerunpackingStartDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerunpackingFinishDatetime', { static: false }) datepickerunpackingFinishDatetime!: BsDatepickerDirective;

    @ViewChild('datepickerdelayConfirmFlag', { static: false }) datepickerdelayConfirmFlag!: BsDatepickerDirective;
    @ViewChild('datepickerinvoiceDate', { static: false }) datepickerinvoiceDate!: BsDatepickerDirective;
    @ViewChild('datepickerunpackingDate', { static: false }) datepickerunpackingDate!: BsDatepickerDirective;
    @ViewChild('datepickerunpackingDatetime', { static: false }) datepickerunpackingDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgwPupPxPUpPlanDto = new CreateOrEditLgwPupPxPUpPlanDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate:any;
    _unpackingStartDatetime:any;
    _unpackingFinishDatetime:any;
    _invoiceDate:any;
    _unpackingDate:any;
    _unpackingDatetime:any;
    _delayConfirmFlag:any;
    constructor(
        injector: Injector,
        private _mstwptPxPUpPlanService: LgwPupPxPUpPlanServiceProxy,
        private _dateTimeService: DateTimeService
    ) { 
        super(injector);
    }

    show(rowdata?: CreateOrEditLgwPupPxPUpPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgwPupPxPUpPlanDto();
        else this.rowdata = rowdata;
        const dateValueworkingDate = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);
        const dateValueunpackingStartDatetime = this.rowdata.unpackingStartDatetime ? new Date(this.rowdata.unpackingStartDatetime?.toString()) : new Date();
        this.datepickerunpackingStartDatetime?.bsValueChange.emit(dateValueunpackingStartDatetime);
        const dateValueunpackingFinishDatetime = this.rowdata.unpackingFinishDatetime ? new Date(this.rowdata.unpackingFinishDatetime?.toString()) : new Date();
        this.datepickerunpackingFinishDatetime?.bsValueChange.emit(dateValueunpackingFinishDatetime);

        const dateValuedelayConfirmFlag = this.rowdata.delayConfirmFlag ? new Date(this.rowdata.delayConfirmFlag?.toString()) : new Date();
        this.datepickerdelayConfirmFlag?.bsValueChange.emit(dateValuedelayConfirmFlag);
        const dateValueinvoiceDate = this.rowdata.invoiceDate ? new Date(this.rowdata.invoiceDate?.toString()) : new Date();
        this.datepickerinvoiceDate?.bsValueChange.emit(dateValueinvoiceDate);
        const dateValueunpackingDate = this.rowdata.unpackingDate ? new Date(this.rowdata.unpackingDate?.toString()) : new Date();
        this.datepickerunpackingDate?.bsValueChange.emit(dateValueunpackingDate);
        const dateValueunpackingDatetime = this.rowdata.unpackingDatetime ? new Date(this.rowdata.unpackingDatetime?.toString()) : new Date();
        this.datepickerunpackingDatetime?.bsValueChange.emit(dateValueunpackingDatetime);

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
        this.rowdata.workingDate=this._dateTimeService.convertToDatetime(this._workingDate);
        this.rowdata.unpackingStartDatetime=this._dateTimeService.convertToDatetime(this._unpackingStartDatetime);
        this.rowdata.unpackingFinishDatetime=this._dateTimeService.convertToDatetime(this._unpackingFinishDatetime);
        this.rowdata.delayConfirmFlag=this._dateTimeService.convertToDatetime(this._delayConfirmFlag);
        this.rowdata.invoiceDate=this._dateTimeService.convertToDatetime(this._invoiceDate);
        this.rowdata.unpackingDate=this._dateTimeService.convertToDatetime(this._unpackingDate);
        this.rowdata.unpackingDatetime=this._dateTimeService.convertToDatetime(this._unpackingDatetime)
        this._mstwptPxPUpPlanService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
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
