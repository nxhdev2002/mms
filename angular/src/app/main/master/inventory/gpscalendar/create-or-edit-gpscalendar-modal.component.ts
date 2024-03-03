import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvGpsCalendarDto, MstInvGpsCalendarServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-gpscalendar-modal',
    templateUrl: './create-or-edit-gpscalendar-modal.component.html',
    })
export class CreateOrEditGpsCalendarModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsCalendar', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsCalendarDto = new CreateOrEditMstInvGpsCalendarDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate:any;
    constructor(
        injector: Injector,
        private _mstwptGpsCalendarService: MstInvGpsCalendarServiceProxy,
        private _dateTimeService:DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsCalendarDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvGpsCalendarDto();
        else this.rowdata = rowdata;
        const dateValue = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
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
    this._mstwptGpsCalendarService.createOrEdit(this.rowdata)
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
