import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvGpsStockConceptDto, InvGpsStockConceptServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-invgpsstockconcept-modal',
    templateUrl: './create-or-edit-invgpsstockconcept-modal.component.html'
    })
export class CreateOrEditInvGpsStockConceptModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvGpsStockConcept', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsStockConceptDto = new CreateOrEditInvGpsStockConceptDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _monthStk:any;

    constructor(
        injector: Injector,
        private _mstwptInvGpsStockConceptService: InvGpsStockConceptServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsStockConceptDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvGpsStockConceptDto();
        else this.rowdata = rowdata;
        const dateValue = this.rowdata.monthStk ? new Date(this.rowdata.monthStk?.toString()) : null;
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
    this.rowdata.monthStk = this._dateTimeService.convertToDatetime(this._monthStk);
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
    this._mstwptInvGpsStockConceptService.createOrEdit(this.rowdata)
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
