import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { CreateOrEditInvGpsReceivingDto, InvGpsReceivingServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { ReceivingComponent } from './receiving.component';

@Component({
    selector: 'create-or-edit-receiving-modal',
    templateUrl: './create-or-edit-receiving-modal.component.html',
    })
export class CreateOrEditReceivingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalStockReceiving', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker_p', { static: false }) datepicker_p!: BsDatepickerDirective;
    @ViewChild('datepicker_e', { static: false }) datepicker_e!: BsDatepickerDirective;
    @ViewChild('datepicker_r', { static: false }) datepicker_r!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsReceivingDto = new CreateOrEditInvGpsReceivingDto();

    active: boolean = false;
    saving: boolean = false;
    checkEdit: boolean = true;
    _partNo;
    _prodDate: any;
    _expDate: any;
    _receivedDate: any;

    constructor(
        injector: Injector,
        private _mstwptReceivingService: InvGpsReceivingServiceProxy,
        private _dateTimeService: DateTimeService,
        private _service: ReceivingComponent)
    {super(injector);}

    show(rowdata?: CreateOrEditInvGpsReceivingDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvGpsReceivingDto();
        else this.rowdata = rowdata;
        const prodDateValue = this.rowdata.prodDate ? new Date(this.rowdata.prodDate?.toString()) : null;
        this.datepicker_p?.bsValueChange.emit(prodDateValue);
        const expDateValue = this.rowdata.expDate ? new Date(this.rowdata.expDate?.toString()) : null;
        this.datepicker_e?.bsValueChange.emit(expDateValue);
        const receivedDateValue = this.rowdata.receivedDate ? new Date(this.rowdata.receivedDate?.toString()) : null;
        this.datepicker_r?.bsValueChange.emit(receivedDateValue);
        this.active = true;
        this.modal.show();
    }

    save(): void {
        if (this.validate()) {
            this.saving = true;
            this.rowdata.prodDate = this._dateTimeService.convertToDatetime(this._prodDate);
            this.rowdata.expDate = this._dateTimeService.convertToDatetime(this._expDate);
            this.rowdata.receivedDate = this._dateTimeService.convertToDatetime(this._receivedDate);
            this._mstwptReceivingService.spCheckPartNoMaterial(this.rowdata.partNo).subscribe((e) => {
                if(e.existMaterial == '1'&& this.checkEdit){
                    this._mstwptReceivingService.createOrEdit(this.rowdata)
                    .pipe(finalize(() => this.saving = false))
                    .subscribe((res) => {
                        this.notify.success(this.l('SavedSuccessfully'));
                        this.close();
                        this.modal?.hide();
                        this.modalSave.emit(this.rowdata);
                        this._service.searchDatas();
                    });
                    this.saving = false;
                }
                else if(e.existMaterial == null){
                    this.notify.warn('PartNo không tồn tại !');
                    return;
                }
            })
        }
    }


    validate() {
        /// validate > 0
        // let validatePattern = /^[1-9]+$/
        // if (!validatePattern.test(Number(this.rowdata.boxqty).toString())) {
        //     this.message.warn(this.l('Trường BoxQty phải > 0'));
        //     return false;
        // }
        // if (!validatePattern.test(Number(this.rowdata.box).toString())) {
        //     this.message.warn(this.l('Trường Box phải > 0'));
        //     return false;
        // }
        // if (!validatePattern.test(Number(this.rowdata.qty).toString())) {
        //     this.message.warn(this.l('Trường Qty > 0'));
        //     return false;
        // }
        /// validate điều kiện qty
        // if (this.rowdata.boxqty !== null && this.rowdata.box !== null) {
        //     if (Number(this.rowdata.boxqty) * Number(this.rowdata.box) !== Number(this.rowdata.qty)) {
        //         this.message.warn(this.l('Trường Qty phải bằng BoxQty * Box'));
        //         return false;
        //     }
        // }
        /// validate ngày hết hạn, ngày nhập hàng
        // console.log(this._prodDate)
        // console.log(this._expDate)
        // console.log(this._receivedDate)
        if (this._prodDate !== null) {
            if (this._expDate !== null) {
                if (this._dateTimeService.convertToDatetime(this._prodDate) > this._dateTimeService.convertToDatetime(this._expDate)) {
                    this.message.warn(this.l('ExpDate phải sau ProdDate'));
                    return false;
                }
            }
            if (this._receivedDate !== null) {
                if (this._dateTimeService.convertToDatetime(this._prodDate) > this._dateTimeService.convertToDatetime(this._receivedDate)) {
                    this.message.warn(this.l('ReceivedDate phải sau ProdDate'));
                    return false;
                }
            }
        }

        return true;
    }

    changeBoxQty(ev){
        if(ev == '' || this.rowdata.box == undefined || this.rowdata.box == null || this.rowdata.box.toString() == '') this.rowdata.qty = null;
        else this.rowdata.qty = this.rowdata.box * Number(ev);
    }

    changeBox(ev){
        if(ev == '' || this.rowdata.boxqty == undefined || this.rowdata.boxqty == null || this.rowdata.boxqty.toString() == '') this.rowdata.qty = null;
        else this.rowdata.qty = this.rowdata.boxqty * Number(ev);
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
