import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdContainerTransitPortPlanDto, InvCkdContainerTransitPortPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import * as moment from 'moment';


@Component({
    selector: 'create-or-edit-containertransitportplan-modal',
    templateUrl: './create-or-edit-containertransitportplan-modal.component.html',
})
export class CreateOrEditContainerTransitPortPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalContainerTransitPortPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdContainerTransitPortPlanDto = new CreateOrEditInvCkdContainerTransitPortPlanDto();

    RestrictList = [
        { value: 'R', label: "REQUESTED" },
        { value: 'P', label: "PENDING" },
        { value: 'F', label: "CONFIRM" },
        { value: 'C', label: "CANCEL" },
    ];

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _requestTime;
    _requestDate;

    constructor(
        injector: Injector,
        private _mstwptContainerTransitPortPlanService: InvCkdContainerTransitPortPlanServiceProxy,
        private _dateTimeService: DateTimeService
    ) { super(injector); }

    show(rowdata?: CreateOrEditInvCkdContainerTransitPortPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvCkdContainerTransitPortPlanDto();
        else this.rowdata = rowdata;

        const dateValue = this.rowdata.requestDate ? new Date(this.rowdata.requestDate?.toString()) : null;

        this.datepicker?.bsValueChange.emit(dateValue);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }



    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.requestDate = this._dateTimeService.convertToDatetime(this._requestDate);

        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptContainerTransitPortPlanService.createOrEdit(this.rowdata)
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
