import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditPtsAdoLineEfficiencyDto, PtsAdoLineEfficiencyServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-lineefficiency-modal',
    templateUrl: './create-or-edit-lineefficiency-modal.component.html',
    styleUrls: ['./create-or-edit-lineefficiency-modal.component.less']
    })
export class CreateOrEditLineEfficiencyModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLineEfficiency', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditPtsAdoLineEfficiencyDto = new CreateOrEditPtsAdoLineEfficiencyDto();

    active: boolean = false;
    saving: boolean = false;
    _shift : boolean;
    _isActive: boolean;
    _workingDate : any;

    constructor(
        injector: Injector,
        private _service: PtsAdoLineEfficiencyServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditPtsAdoLineEfficiencyDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditPtsAdoLineEfficiencyDto();
        else this.rowdata = rowdata;
		const dateValue =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValue);


        this._shift = (this.rowdata.shift == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;


        this.active = true;
        this.modal.show();
    }


    save(): void {
    this.saving = true;
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.shift
    this.rowdata.shift = (this._shift == true) ? 'Y' : 'N'
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._service.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
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
