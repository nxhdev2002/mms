import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgaEkbProgressDto, LgaEkbProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-progress-modal',
    templateUrl: './create-or-edit-progress-modal.component.html',
    styleUrls: ['./create-or-edit-progress-modal.component.less']
    })
export class CreateOrEditProgressModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalProgress', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepickerworkingDate', { static: false }) datepickerworkingDate!: BsDatepickerDirective;
    @ViewChild('datepickernewtaktDatetime', { static: false }) datepickernewtaktDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerstartDatetime', { static: false }) datepickerstartDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerfinishDatetime', { static: false }) datepickerfinishDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgaEkbProgressDto = new CreateOrEditLgaEkbProgressDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptProgressService: LgaEkbProgressServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgaEkbProgressDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgaEkbProgressDto();
        else this.rowdata = rowdata;
		const dateValueworkingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
		this.datepickerworkingDate?.bsValueChange.emit(dateValueworkingDate);
		const dateValuenewtaktDatetime =  this.rowdata.newtaktDatetime ? new Date(this.rowdata.newtaktDatetime?.toString()) : new Date();
		this.datepickernewtaktDatetime?.bsValueChange.emit(dateValuenewtaktDatetime);
		const dateValuestartDatetime =  this.rowdata.startDatetime ? new Date(this.rowdata.startDatetime?.toString()) : new Date();
		this.datepickerstartDatetime?.bsValueChange.emit(dateValuestartDatetime);
		const dateValuefinishDatetime =  this.rowdata.finishDatetime ? new Date(this.rowdata.finishDatetime?.toString()) : new Date();
		this.datepickerfinishDatetime?.bsValueChange.emit(dateValuefinishDatetime);

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
    this._mstwptProgressService.createOrEdit(this.rowdata)
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
