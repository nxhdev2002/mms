import { formatDate } from "@angular/common";
import { Component, ElementRef, EventEmitter, HostListener, Injector, Output, ViewChild } from "@angular/core";
import { AppComponentBase } from "@shared/common/app-component-base";
import { InvCkdContainerListDto } from "@shared/service-proxies/service-proxies";
import { BsDatepickerDirective } from "ngx-bootstrap/datepicker/bs-datepicker.component";
import { ModalDirective } from "ngx-bootstrap/modal";


@Component({
    selector: 'view-detail-containerlist-modal',
    templateUrl: './view-detail-containerlist-modal.component.html',
})
export class ViewContainerListModalComponent extends AppComponentBase {
    @ViewChild('viewModalContainerList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdContainerListDto = new InvCkdContainerListDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _gateinDate: string;
    _devanningDate: string;
    _receiveDate: string;
    _shippingDate: string;
    _portDate: string;
    _cdDate: string;
    _portTransitDate: string;
    _transitPortReqDate: string;
    _locationDate: string;

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show(rowdata?: InvCkdContainerListDto): void {
        if (!rowdata) this.rowdata = new InvCkdContainerListDto();
        else this.rowdata = rowdata;

        this._cdDate = this.rowdata.cdDate ? formatDate(new Date(this.rowdata.cdDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._portDate = this.rowdata.portDate ? formatDate(new Date(this.rowdata.portDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._shippingDate = this.rowdata.shippingDate ? formatDate(new Date(this.rowdata.shippingDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._receiveDate = this.rowdata.receiveDate ? formatDate(new Date(this.rowdata.receiveDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._devanningDate = this.rowdata.devanningDate ? formatDate(new Date(this.rowdata.devanningDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._gateinDate = this.rowdata.gateinDate ? formatDate(new Date(this.rowdata.gateinDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._portTransitDate = this.rowdata.portTransitDate ? formatDate(new Date(this.rowdata.portTransitDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._transitPortReqDate = this.rowdata.transitPortReqDate ? formatDate(new Date(this.rowdata.transitPortReqDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;
        this._locationDate = this.rowdata.locationDate ? formatDate(new Date(this.rowdata.locationDate.toString().substring(0, 10)), 'dd/MM/yyyy', 'en-US') : null;


        // const dateValueTransactionDatetime =  this.rowdata.cdDate ? new Date(this.rowdata.cdDate?.toString()) : null;
        // this.dateValueCdDate ?.bsValueChange.emit(dateValueTransactionDatetime);
        // const dateValueTransactionDatetime =  this.rowdata.shippingDate? new Date(this.rowdata.shippingDate?.toString()) : null;
        // this.datepickerShippingDate?.bsValueChange.emit(dateValueTransactionDatetime);
        // const dateValueTransactionDatetime =  this.rowdata.portDate? new Date(this.rowdata.portDate?.toString()) : null;
        // this.datepickerPortDate?.bsValueChange.emit(dateValueTransactionDatetime);
        // const dateValueTransactionDatetime =  this.rowdata.receiveDate? new Date(this.rowdata.receiveDate?.toString()) : null;
        // this.datepickerReceiveDate?.bsValueChange.emit(dateValueTransactionDatetime);
        // const dateValueTransactionDatetime =  this.rowdata.devanningDate? new Date(this.rowdata.devanningDate?.toString()) : null;
        // this.datepickerDevanningDate?.bsValueChange.emit(dateValueTransactionDatetime);
        // const dateValueTransactionDatetime =  this.rowdata.gateinDate? new Date(this.rowdata.gateinDate?.toString()) : null;
        // this.datepickerGateInDate?.bsValueChange.emit(dateValueTransactionDatetime);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
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
