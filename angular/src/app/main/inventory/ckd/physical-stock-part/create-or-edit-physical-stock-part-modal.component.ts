import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { catchError, finalize } from 'rxjs/operators';
import { CreateOrEditInvCkdPhysicalStockPartDto, InvCkdPhysicalStockPartServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
import { DateTime } from 'luxon';



@Component({
    selector: 'create-or-edit-physicalstockpart-modal',
    templateUrl: './create-or-edit-physical-stock-part-modal.component.html',
})
export class CreateOrEditPhysicalStockPartModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPhysicalStockPart', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvCkdPhysicalStockPartDto = new CreateOrEditInvCkdPhysicalStockPartDto();

    pipe = new DatePipe('en-US');
    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    disable = false;
    _lastCalDatetime;
    _actualQty: number;
    errorInsert;


    constructor(
        injector: Injector,
        private _mstwptPhysicalStockPartService: InvCkdPhysicalStockPartServiceProxy,
        private _dateTimeService: DateTimeService,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvCkdPhysicalStockPartDto): void {

        if (!rowdata) {
            this.rowdata = new CreateOrEditInvCkdPhysicalStockPartDto();
            this.disable = false
        }
        else {
            this.rowdata = rowdata;
            this.disable = true;
        }
        this._actualQty = this.rowdata.actualQty;
        // this._lastCalDatetime = moment(this.rowdata.lastCalDatetime?.toString()).format().slice(0, 11) + moment(this.rowdata.lastCalDatetime?.toString()).format('hh:mm').slice(0, 5);
        this._lastCalDatetime = new Date().toISOString().slice(0, 16);
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        if(this._isActive == false && this._actualQty == 0)
        {
            this.message.confirm(this.l('Please confirm inactive (obsolete) following part?'), this.l('Are You Sure?'), (isConfirmed) => {
                if (isConfirmed) {
                    this.saving = true;
                    this.rowdata.actualQty = this._actualQty == null ? 0 : this._actualQty;
                    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
                    this.rowdata.lastCalDatetime = DateTime.fromISO(this._lastCalDatetime);
                    this._mstwptPhysicalStockPartService.createOrEdit(this.rowdata)
                        .pipe(
                            catchError((error) => {
                                const errorMessage = error?.error?.ErrorMessage || 'Đã xảy ra lỗi.';
                                this.message.error(errorMessage);
                                throw error;
                            }))
                        .subscribe((result) => {
                            if (result == "") {
                                this.modalSave.emit(this.rowdata)
                                this.close();
                                this.modal?.hide();
                                this.notify.success('Update Success')
                            }
                            else {
                                this.errorInsert = result;
                                if (result == 'success') {
                                    this.insertSuccess();
                                }
                                else {
                                    this.exceptionInsert();
                                }
                            }
                        })
                        .add(() => {
                            this.saving = false;
                        });
                }
                });
        }else{
            this.saving = true;
            this.rowdata.actualQty = this._actualQty == null ? 0 : this._actualQty;
            this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
            this.rowdata.lastCalDatetime = DateTime.fromISO(this._lastCalDatetime);
            this._mstwptPhysicalStockPartService.createOrEdit(this.rowdata)
                .pipe(
                    catchError((error) => {
                        const errorMessage = error?.error?.ErrorMessage || 'Đã xảy ra lỗi.';
                        this.message.error(errorMessage);
                        throw error;
                    }))
                .subscribe((result) => {
                    console.log(result);
                    
                    if (result == "") {
                        this.modalSave.emit(this.rowdata)
                        this.close();
                        this.modal?.hide();
                        this.notify.success('Update Success')
                    }
                    else {
                        this.errorInsert = result;
                        if (result == 'success') {
                            this.insertSuccess();
                        }
                        else {
                            this.exceptionInsert();
                        }
                    }
                })
                .add(() => {
                    this.saving = false;
                });
        }
            
    }
    insertSuccess() {
        this.notify.success('Thêm mới thành công')
        this.close();
        this.modal?.hide();
    }

    exceptionInsert() {
        this.notify.warn(this.errorInsert)
        this.close();
        this.modal?.hide();
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
