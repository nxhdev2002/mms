import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgwAdoCallingLightStatusDto, LgwAdoCallingLightStatusServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-callinglightstatus-modal',
    templateUrl: './create-or-edit-callinglightstatus-modal.component.html',
    styleUrls: ['./create-or-edit-callinglightstatus-modal.component.less']
    })
export class CreateOrEditCallinglightstatusModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalCallingLightStatus', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerworkingDate', { static: false }) datepickerworkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerstartDate', { static: false }) datepickerstartDate!: BsDatepickerDirective;
    @ViewChild('datepickerfinshDate', { static: false }) datepickerfinshDate!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgwAdoCallingLightStatusDto = new CreateOrEditLgwAdoCallingLightStatusDto();

    active: boolean = false;
    saving: boolean = false;
    constructor(
        injector: Injector,
        private _mstwptCallingLightStatusService: LgwAdoCallingLightStatusServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgwAdoCallingLightStatusDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgwAdoCallingLightStatusDto();
        else this.rowdata = rowdata;
        const datepickerstartDate =  this.rowdata.startDate ? new Date(this.rowdata.startDate?.toString()) : null;
		this.datepickerstartDate?.bsValueChange.emit(datepickerstartDate);
        const datepickerfinshDate =  this.rowdata.finshDate ? new Date(this.rowdata.finshDate?.toString()) : null;
		this.datepickerfinshDate?.bsValueChange.emit(datepickerfinshDate);
        const dateValueworkingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : null;
		this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);
        this.active = true;
        this.modal.show();
    }

    save(): void {
    this.saving = true;
    this._mstwptCallingLightStatusService.createOrEdit(this.rowdata)
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
