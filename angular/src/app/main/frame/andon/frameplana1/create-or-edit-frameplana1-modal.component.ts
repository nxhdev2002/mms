import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditFrmAdoFramePlanA1Dto, FrmAdoFramePlanA1ServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-frameplana1-modal',
    templateUrl: './create-or-edit-frameplana1-modal.component.html',
    styleUrls: ['./create-or-edit-frameplana1-modal.component.less']
})
export class CreateOrEditFramePlanA1ModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalFramePlanA1', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerplanDate', { static: false }) datepickerplanDate!: BsDatepickerDirective;
    @ViewChild('datepickerplanMonth', { static: false }) datepickerplanMonth!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditFrmAdoFramePlanA1Dto = new CreateOrEditFrmAdoFramePlanA1Dto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _planDate : any;

    constructor(
        injector: Injector,
        private _mstwptFramePlanA1Service: FrmAdoFramePlanA1ServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditFrmAdoFramePlanA1Dto): void {
        if (!rowdata) this.rowdata = new CreateOrEditFrmAdoFramePlanA1Dto();
        else this.rowdata = rowdata;
        const dateValueplanDate = this.rowdata.planDate ? new Date(this.rowdata.planDate?.toString()) : new Date();
        this.datepickerplanDate?.bsValueChange.emit(dateValueplanDate);
        const dateValueplanMonth = this.rowdata.planMonth ? new Date(this.rowdata.planMonth?.toString()) : new Date();
        this.datepickerplanMonth?.bsValueChange.emit(dateValueplanMonth);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.planDate = this._dateTimeService.convertToDatetime(this._planDate),
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptFramePlanA1Service.createOrEdit(this.rowdata)
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
