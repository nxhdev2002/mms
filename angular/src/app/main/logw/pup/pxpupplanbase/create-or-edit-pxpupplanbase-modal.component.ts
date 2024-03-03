import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgwPupPxPUpPlanBaseDto, LgwPupPxPUpPlanBaseServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-pxpupplanbase-modal',
    templateUrl: './create-or-edit-pxpupplanbase-modal.component.html',
    styleUrls: ['./create-or-edit-pxpupplanbase-modal.component.less']
    })
export class CreateOrEditPxPUpPlanBaseModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPxPUpPlanBase', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerworkingDate', { static: false }) datepickerworkingDate!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgwPupPxPUpPlanBaseDto = new CreateOrEditLgwPupPxPUpPlanBaseDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _workingDate:any;
    constructor(
        injector: Injector,
        private _mstwptPxPUpPlanBaseService: LgwPupPxPUpPlanBaseServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgwPupPxPUpPlanBaseDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgwPupPxPUpPlanBaseDto();
        else this.rowdata = rowdata;
        const dateValueworkingDate = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);

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
    const dateValueworkingDate = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
    this._workingDate=this._dateTimeService.convertToDatetime(this._workingDate)
    this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);
    this._mstwptPxPUpPlanBaseService.createOrEdit(this.rowdata)
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
