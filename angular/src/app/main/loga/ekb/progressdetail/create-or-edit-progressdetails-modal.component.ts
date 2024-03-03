import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgaEkbProgressDetailsDto, LgaEkbProgressDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-progressdetails-modal',
    templateUrl: './create-or-edit-progressdetails-modal.component.html',
    styleUrls: ['./create-or-edit-progressdetails-modal.component.less']
    })
export class CreateOrEditProgressDetailsModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalProgressDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerNewTakt', { static: false }) datepickerNewTakt!: BsDatepickerDirective;
    @ViewChild('datepickerPikStart', { static: false }) datepickerPikStart!: BsDatepickerDirective;
    @ViewChild('datepickerPikFinish', { static: false }) datepickerPikFinish!: BsDatepickerDirective;
    @ViewChild('datepickerDelStart', { static: false }) datepickerDelStart!: BsDatepickerDirective;
    @ViewChild('datepickerDelFinish', { static: false }) datepickerDelFinish!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgaEkbProgressDetailsDto = new CreateOrEditLgaEkbProgressDetailsDto();

    active: boolean = false;
    saving: boolean = false;
    _isZeroKb:boolean;
    _isActive: boolean;
    _workingDate:any;

    constructor(
        injector: Injector,
        private _mstwptProgressDetailsService: LgaEkbProgressDetailsServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgaEkbProgressDetailsDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgaEkbProgressDetailsDto();
        else this.rowdata = rowdata;
		const dateValueworkingDate =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValueworkingDate);

		const dateValuenewtaktDatetime =  this.rowdata.newtaktDatetime ? new Date(this.rowdata.newtaktDatetime?.toString()) : new Date();
		this.datepickerNewTakt?.bsValueChange.emit(dateValuenewtaktDatetime);

		const dateValuepikStartDatetime =  this.rowdata.pikStartDatetime ? new Date(this.rowdata.pikStartDatetime?.toString()) : new Date();
		this.datepickerPikStart?.bsValueChange.emit(dateValuepikStartDatetime);

		const dateValuepikFinishDatetime =  this.rowdata.pikFinishDatetime ? new Date(this.rowdata.pikFinishDatetime?.toString()) : new Date();
		this.datepickerPikFinish?.bsValueChange.emit(dateValuepikFinishDatetime);

		const dateValuedelStartDatetime =  this.rowdata.delStartDatetime ? new Date(this.rowdata.delStartDatetime?.toString()) : new Date();
		this.datepickerDelStart?.bsValueChange.emit(dateValuedelStartDatetime);

		const dateValuedelFinishDatetime =  this.rowdata.delFinishDatetime ? new Date(this.rowdata.delFinishDatetime?.toString()) : new Date();
		this.datepickerDelFinish?.bsValueChange.emit(dateValuedelFinishDatetime);

        this._isZeroKb = (this.rowdata.isZeroKb == "N")? false: true;
        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isZeroKb = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.isZeroKb = (this._isZeroKb == true) ? 'Y' : 'N'
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstwptProgressDetailsService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
           // this.modalSave.emit(this.rowdata);
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
