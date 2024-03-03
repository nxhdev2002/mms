import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize, debounceTime } from 'rxjs/operators';
import { CreateOrEditPlnCcrProductionPlanDto, PlnCcrProductionPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-productionplan-modal',
    templateUrl: './create-or-edit-productionplan-modal.component.html',
    styleUrls: ['./create-or-edit-productionplan-modal.component.less']
    })
export class CreateOrEditProductionPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalProductionPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('dateTimeIn', { static: false }) dateTimeIn!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditPlnCcrProductionPlanDto = new CreateOrEditPlnCcrProductionPlanDto();

    active: boolean = false;
    saving: boolean = false;
    _model: boolean;
    _dateTimeIn: any;
    _dateIn: any;

    constructor(
        injector: Injector,
        private _mstwptProductionPlanService: PlnCcrProductionPlanServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditPlnCcrProductionPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditPlnCcrProductionPlanDto();
        else this.rowdata = rowdata;
		const dateIn =  this.rowdata.dateIn ? new Date(this.rowdata.dateIn?.toString()) : new Date();
		this.datepicker?.bsValueChange.emit(dateIn);
		const dateTimeIn =  this.rowdata.dateTimeIn ? new Date(this.rowdata.dateTimeIn?.toString()) : new Date();
		this.dateTimeIn?.bsValueChange.emit(dateTimeIn);

        this._model = (this.rowdata.model == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._model = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.dateIn = this._dateTimeService.convertToDatetime(this._dateIn),
    this.rowdata.dateTimeIn = this._dateTimeService.convertToDatetime(this._dateTimeIn),
    this.rowdata.model = (this._model == true) ? 'Y' : 'N'
    this._mstwptProductionPlanService.createOrEdit(this.rowdata)
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
