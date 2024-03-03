import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptSeasonMonthDto, MstWptSeasonMonthServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { Time } from '@angular/common';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-seasonmonth-modal',
    templateUrl: './create-or-edit-seasonmonth-modal.component.html',
})
export class CreateOrEditSeasonmonthModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalSeasonmonth', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstWptSeasonMonthDto = new CreateOrEditMstWptSeasonMonthDto();

    startTime: Time;
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _seasonMonth: any;

    constructor(
        injector: Injector,
        private _mstwptSeasonMonthService: MstWptSeasonMonthServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstWptSeasonMonthDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptSeasonMonthDto();
        else this.rowdata = rowdata;
        const dateValue = this.rowdata.seasonMonth ? new Date(this.rowdata.seasonMonth?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.rowdata.seasonMonth = this._dateTimeService.convertToDatetime(this._seasonMonth),
            this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptSeasonMonthService.createOrEdit(this.rowdata)
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
