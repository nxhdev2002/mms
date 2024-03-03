import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvPioPartListDto, InvDrmPartListServiceProxy, InvPioPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { formatDate } from '@angular/common';


@Component({
    selector: 'create-or-edit-partlist',
    templateUrl: './create-or-edit-partlist.component.html'

    })
export class CreateOrEditPartListModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvPioPartList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerStartDate', { static: false }) datepickerStartDate!: BsDatepickerDirective;
    @ViewChild('datepickerEndDate', { static: false }) datepickerEndDate: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvPioPartListDto = new CreateOrEditInvPioPartListDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    _startDate;
    _endDate;
    _errorCheck: CreateOrEditInvPioPartListDto[] = [];
    checkVar: boolean = true;
    checkQty: boolean = true;

    constructor(
        injector: Injector,
        private _service: InvPioPartListServiceProxy,
        private _dateTimeService: DateTimeService,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvPioPartListDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvPioPartListDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "1") ? true : false;
        const startDate = this.rowdata.startDate ? new Date(this.rowdata.startDate?.toString()) : null;
        this.datepickerStartDate?.bsValueChange.emit(startDate);

        const endDate = this.rowdata.endDate ? new Date(this.rowdata.endDate?.toString()) : null;
        this.datepickerEndDate?.bsValueChange.emit(endDate);

        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
        this.checkQty = true;
        this.saving = true;
        if(!/^[1-9]+/.test(Number(this.rowdata.boxSize).toString()))
        {
            this.message.warn(this.l('boxsize > 0'));
            this.checkQty = false;
        }

        if(this.checkQty && this.validate())
        {
            this.rowdata.isActive = (this._isActive == true) ? '1' : '0'
            this.rowdata.startDate = this._dateTimeService.convertToDatetime(this._startDate)
            this.rowdata.endDate = this._dateTimeService.convertToDatetime(this._endDate)
            this.saving = true;
            this._service.createOrEdit(this.rowdata)
                .pipe( finalize(() =>  this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            this.saving = true;
        }
    }

    validate() {

        /// validate endDate phải sau startDate
        if (this._startDate !== null) {
            if (this._endDate !== null) {
                if (this._dateTimeService.convertToDatetime(this._startDate) >= this._dateTimeService.convertToDatetime(this._endDate)) {
                    this.message.warn(this.l('EndDate phải sau StartDate'));
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
