import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptPatternHDto, MstWptPatternHServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-patternh-modal',
    templateUrl: './create-or-edit-patternh-modal.component.html',
})
export class CreateOrEditPatternHModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPatternH', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerStart', { static: false }) datepickerStart!: BsDatepickerDirective;
    @ViewChild('datepickerEnd', { static: false }) datepickerEnd!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstWptPatternHDto = new CreateOrEditMstWptPatternHDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _startDate: any;
    _endDate: any;

    constructor(
        injector: Injector,
        private _mstwptPatternHService: MstWptPatternHServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstWptPatternHDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptPatternHDto();
        else this.rowdata = rowdata;
        const startDateValue = this.rowdata.startDate ? new Date(this.rowdata.startDate?.toString()) : new Date();
        this.datepickerStart?.bsValueChange.emit(startDateValue);

        const endDateValue = this.rowdata.endDate ? new Date(this.rowdata.endDate?.toString()) : new Date();
        this.datepickerEnd?.bsValueChange.emit(endDateValue);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;

        this.active = true;
        this.modal.show();
    }



    save(): void {
        this.saving = true;
        this.rowdata.startDate = this._dateTimeService.convertToDatetime(this._startDate),
            this.rowdata.endDate = this._dateTimeService.convertToDatetime(this._endDate),
            this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptPatternHService.createOrEdit(this.rowdata)
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
