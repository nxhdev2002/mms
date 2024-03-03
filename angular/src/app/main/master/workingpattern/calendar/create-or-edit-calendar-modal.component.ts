import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptCalendarDto, MstWptCalendarServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-calendar-modal',
    templateUrl: './create-or-edit-calendar-modal.component.html',
    styleUrls: ['./create-or-edit-calendar-modal.component.less']
})
export class CreateOrEditCalendarModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalCalendar', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    @Output() valueChosen: EventEmitter<any> = new EventEmitter();

    rowdata: CreateOrEditMstWptCalendarDto = new CreateOrEditMstWptCalendarDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate: any;

    constructor(
        injector: Injector,
        private _mstwptCalendarService: MstWptCalendarServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    choose(value: string) {
        this.valueChosen.emit(value);
    }

    show(rowdata?: CreateOrEditMstWptCalendarDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptCalendarDto();
        else this.rowdata = rowdata;
        this.rowdata.workingType = this.rowdata.workingType == 'HOLIDAY' ? '0'
            : this.rowdata.workingType == 'ALL DAY' ? '123'
                : this.rowdata.workingType == 'SHIFT 1 WORK ONLY' ? '1'
                    : this.rowdata.workingType == 'SHIFT 2 WORK ONLY' ? '2'
                        : this.rowdata.workingType == 'SHIFT 3 WORK ONLY' ? '3'
                            : this.rowdata.workingType == 'SHIFT 1,2 WORK ONLY' ? '12'
                                : this.rowdata.workingType == 'SHIFT 1,3 WORK ONLY' ? '13'
                                    : this.rowdata.workingType == 'SHIFT 2,3 WORK ONLY' ? '23' : 'Unknown'
        const dateValue = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
            this.rowdata.seasonType = this.rowdata.seasonType == 'High' ? 'H'
                : this.rowdata.seasonType == 'Normal' ? 'N'
                    : this.rowdata.seasonType == 'Low' ? 'L' : 'Unknown'
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptCalendarService.createOrEdit(this.rowdata)
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
