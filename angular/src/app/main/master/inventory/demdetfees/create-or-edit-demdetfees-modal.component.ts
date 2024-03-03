import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCkdCustomsLeadtimeDto, CreateOrEditMstInvDemDetFeesDto, MstCkdCustomsLeadtimeServiceProxy, MstInvDemDetFeesServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-demdetfees-modal',
    templateUrl: './create-or-edit-demdetfees-modal.component.html'
    })
export class CreateOrEditDemDetFeesModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalDemDetFees', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker_f', { static: false }) datepicker_f!: BsDatepickerDirective;
    @ViewChild('datepicker_t', { static: false }) datepicker_t!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvDemDetFeesDto = new CreateOrEditMstInvDemDetFeesDto();
    sourceList = [];
    carrierList = [];
    minwidth0 = "minwidth0";
    _source;
    _carrier;
    active: boolean = false;
    saving: boolean = false;
    _: boolean;
    _isActive;
    _effectiveDateFrom: any;
    _effectiveDateTo: any;

    constructor(
        injector: Injector,
        private _mstinvDemDetfeesService: MstInvDemDetFeesServiceProxy,
        private _dateTimeService: DateTimeService,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvDemDetFeesDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvDemDetFeesDto();
        else this.rowdata = rowdata;
        this._mstinvDemDetfeesService.getListSource().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.sourceList.push({ value: e.source, label: e.source });
                })
            }
        })
        this._mstinvDemDetfeesService.getListCarrier().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.carrierList.push({ value: e.carrier, label: e.carrier });
                })
            }
        })
        const effectiveDateFromValue = this.rowdata.effectiveDateFrom ? new Date(this.rowdata.effectiveDateFrom?.toString()) : null;
        this.datepicker_f?.bsValueChange.emit(effectiveDateFromValue);
        const effectiveDateToValue = this.rowdata.effectiveDateTo ? new Date(this.rowdata.effectiveDateTo?.toString()) : null;
        this.datepicker_t?.bsValueChange.emit(effectiveDateToValue);

        this._isActive = (this.rowdata.isActive == 'N') ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._ = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.effectiveDateFrom = this._dateTimeService.convertToDatetime(this._effectiveDateFrom);
    this.rowdata.effectiveDateTo = this._dateTimeService.convertToDatetime(this._effectiveDateTo);
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstinvDemDetfeesService.createOrEdit(this.rowdata)
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
