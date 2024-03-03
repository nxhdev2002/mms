import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdSmqdOrderDto, InvCkdSmqdOrderServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-smqdorder-modal',
    templateUrl: './create-or-edit-smqdorder-modal.component.html'

    })
export class CreateOrEditInvCkdSmqdOderModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvCkdSmqdOrder', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerOrDatetime', { static: false }) datepickerOrDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerEtaRequest', { static: false }) datepickerEtaRequest!: BsDatepickerDirective;
    @ViewChild('datepickerEtaExpReply', { static: false }) datepickerEtaExpReply!: BsDatepickerDirective;
    @ViewChild('datepickerReceiveDate', { static: false }) datepickerReceiveDate!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdSmqdOrderDto = new CreateOrEditInvCkdSmqdOrderDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _smqdDate: any;
    _orderDate: any;
    _etarequest: any;
    _etaexpreply: any;
    _receivedate: any;
    _errorCheck: CreateOrEditInvCkdSmqdOrderDto[] = [];
    checkQty: boolean = true;
    supplierType;

    constructor(
        injector: Injector,
        private _mstwptInvCkdSmqdOrderService: InvCkdSmqdOrderServiceProxy,
        private _dateTimeService: DateTimeService,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvCkdSmqdOrderDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvCkdSmqdOrderDto();
        else this.rowdata = rowdata;
        const dateValue1 = this.rowdata.smqdDate ? new Date(this.rowdata.smqdDate?.toString()) : null;
        this.datepicker?.bsValueChange.emit(dateValue1);
        const dateValue2 = this.rowdata.orderDate ? new Date(this.rowdata.orderDate?.toString()) : null;
        this.datepickerOrDatetime?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowdata.etaRequest ? new Date(this.rowdata.etaRequest?.toString()) : null;
        this.datepickerEtaRequest?.bsValueChange.emit(dateValue3);
        const dateValue4 = this.rowdata.etaExpReply ? new Date(this.rowdata.etaExpReply?.toString()) : null;
        this.datepickerEtaExpReply?.bsValueChange.emit(dateValue4);
        const dateValue5 = this.rowdata.receiveDate ? new Date(this.rowdata.receiveDate?.toString()) : null;
        this.datepickerReceiveDate?.bsValueChange.emit(dateValue5);
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this._mstwptInvCkdSmqdOrderService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.rowdata.smqdDate = this._dateTimeService.convertToDatetime(this._smqdDate),
            this.rowdata.orderDate = this._dateTimeService.convertToDatetime(this._orderDate),
            this.rowdata.etaRequest = this._dateTimeService.convertToDatetime(this._etarequest),
            this.rowdata.etaExpReply = this._dateTimeService.convertToDatetime(this._etaexpreply),
            this.rowdata.receiveDate = this._dateTimeService.convertToDatetime(this._receivedate),
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
