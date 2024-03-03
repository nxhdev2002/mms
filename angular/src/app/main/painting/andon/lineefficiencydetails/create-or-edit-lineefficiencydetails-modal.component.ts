import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditPtsAdoLineEfficiencyDetailsDto, PtsAdoLineEfficiencyDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-lineefficiencydetails-modal',
    templateUrl: './create-or-edit-lineefficiencydetails-modal.component.html',
    styleUrls: ['./create-or-edit-lineefficiencydetails-modal.component.less']
    })
export class CreateOrEditLineEfficiencyDetailsModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLineEfficiencyDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditPtsAdoLineEfficiencyDetailsDto = new CreateOrEditPtsAdoLineEfficiencyDetailsDto();

    active: boolean = false;
    saving: boolean = false;
    _status: boolean;
    _workingDate: any;

    constructor(
        injector: Injector,
        private _service: PtsAdoLineEfficiencyDetailsServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditPtsAdoLineEfficiencyDetailsDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditPtsAdoLineEfficiencyDetailsDto();
        else this.rowdata = rowdata;
		const dateValue =  this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateValue);

        this._status = (this.rowdata.status == "N")? false: true;
        this.active = true;
        this.modal.show();

    }

    changeActive(event){
        this._status = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.status = (this._status == true) ? 'Y' : 'N'
    this._service.createOrEdit(this.rowdata)
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
