import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgwDvnContListDto, LgwDvnContListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTime } from 'luxon';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-contlist-modal',
    templateUrl: './create-or-edit-contlist-modal.component.html',
    styleUrls: ['./create-or-edit-contlist-modal.component.less']
})
export class CreateOrEditContListModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalContList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerworkingDate', { static: false }) datepickerworkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerplanDevanningDate', { static: false }) datepickerplanDevanningDate!: BsDatepickerDirective;
    @ViewChild('datepickeractDevanningDate', { static: false }) datepickeractDevanningDate!: BsDatepickerDirective;
    @ViewChild('datepickeractDevanningDateFinished', { static: false }) datepickeractDevanningDateFinished!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgwDvnContListDto = new CreateOrEditLgwDvnContListDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate:any;
    _planDevanningDate:any;
    _actDevanningDate:any;
    _actDevanningDateFinished:any;
    constructor(
        injector: Injector,
        private _mstwptContListService: LgwDvnContListServiceProxy,
        private _dateTimeService:DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgwDvnContListDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgwDvnContListDto();
        else this.rowdata = rowdata;
        const dateValueworkingDate = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);
        const dateValueplanDevanningDate = this.rowdata.planDevanningDate ? new Date(this.rowdata.planDevanningDate?.toString()) : new Date();
        this.datepickerplanDevanningDate?.bsValueChange.emit(dateValueplanDevanningDate);
        const dateValueactDevanningDate = this.rowdata.actDevanningDate ? new Date(this.rowdata.actDevanningDate?.toString()) : new Date();
        this.datepickeractDevanningDate?.bsValueChange.emit(dateValueactDevanningDate);
        const dateValueactDevanningDateFinished = this.rowdata.actDevanningDateFinished ? new Date(this.rowdata.actDevanningDateFinished?.toString()) : new Date();
        this.datepickeractDevanningDateFinished?.bsValueChange.emit(dateValueactDevanningDateFinished);

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
        this.rowdata.workingDate=this._dateTimeService.convertToDatetime(this._workingDate)
        this.rowdata.planDevanningDate=this._dateTimeService.convertToDatetime(this._planDevanningDate);
        this.rowdata.actDevanningDate=this._dateTimeService.convertToDatetime(this._actDevanningDate);
        this.rowdata.actDevanningDateFinished=this._dateTimeService.convertToDatetime(this._actDevanningDateFinished);
        
        this._mstwptContListService.createOrEdit(this.rowdata)
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
