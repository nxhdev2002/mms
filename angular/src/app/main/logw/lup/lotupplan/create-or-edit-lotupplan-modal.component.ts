import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditLgwLupLotUpPlanDto, LgwLupLotUpPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DatePipe } from '@angular/common';
import * as moment from 'moment';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';




@Component({
    selector: 'create-or-edit-lotupplan-modal',
    templateUrl: './create-or-edit-lotupplan-modal.component.html',
    styleUrls: ['./create-or-edit-lotupplan-modal.component.less']
})
export class CreateOrEditLotUpPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLotUpPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerStartDatime', { static: false }) datepickerStartDatime!: BsDatepickerDirective;
    @ViewChild('datepickerFinishDatetime', { static: false }) datepickerFinishDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditLgwLupLotUpPlanDto = new CreateOrEditLgwLupLotUpPlanDto();
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    pipe = new DatePipe('en-US');
    _startDatetime: any;
    _finishDatetime: any;
    _unpackingActualFinishDatetime:any;
    _unpackingActualStartDatetime:any;
    _makingFinishDatetime: any;
    _upCalltime: any;
    _workingDate:any;
    constructor(
        injector: Injector,
        private _mstwptLotUpPlanService: LgwLupLotUpPlanServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditLgwLupLotUpPlanDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditLgwLupLotUpPlanDto();
        else this.rowdata = rowdata;
        const dateValue = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValue);

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
        this._startDatetime = moment(this.rowdata.unpackingStartDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.unpackingStartDatetime?.toString()).format('hh:mm').slice(0, 5);

        this._finishDatetime = moment(this.rowdata.unpackingFinishDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.unpackingFinishDatetime?.toString()).format('hh:mm').slice(0, 5);

        this._unpackingActualFinishDatetime = moment(this.rowdata.unpackingActualFinishDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.unpackingActualFinishDatetime?.toString()).format('hh:mm').slice(0, 5);

        this._unpackingActualStartDatetime = moment(this.rowdata.unpackingActualStartDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.unpackingActualStartDatetime?.toString()).format('hh:mm').slice(0, 5);

        this._makingFinishDatetime = moment(this.rowdata.makingFinishDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.makingFinishDatetime?.toString()).format('hh:mm').slice(0, 5);

        this._upCalltime = moment(this.rowdata.upCalltime?.toString()).format().slice(0, 11) + moment(this.rowdata.upCalltime?.toString()).format('hh:mm').slice(0, 5);

    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
        this.rowdata.unpackingStartDatetime = this._dateTimeService.convertToDatetime(this._startDatetime);
        this.rowdata.unpackingFinishDatetime = this._dateTimeService.convertToDatetime(this._finishDatetime);
        this.rowdata.upCalltime = this._dateTimeService.convertToDatetime(this._upCalltime);
        this.rowdata.unpackingActualFinishDatetime = this._dateTimeService.convertToDatetime(this._unpackingActualFinishDatetime);
        this.rowdata.unpackingActualStartDatetime = this._dateTimeService.convertToDatetime(this._unpackingActualStartDatetime);
        this.rowdata.makingFinishDatetime = this._dateTimeService.convertToDatetime(this._makingFinishDatetime);
        if (this.rowdata.unpackingStartDatetime <= this.rowdata.unpackingFinishDatetime) {
            this._mstwptLotUpPlanService.createOrEdit(this.rowdata)
                .pipe(finalize(() => this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    // this.modalSave.emit(this.rowdata);
                });
            this.saving = false;
        }
        else {
            alert("Ngày kết thúc phải sau hoặc bằng ngày bắt đầu!");
        }
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
