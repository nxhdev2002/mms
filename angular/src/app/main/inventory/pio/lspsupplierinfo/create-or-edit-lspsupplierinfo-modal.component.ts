import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {  CreateOrEditMstLspSupplierInforDto, MstLspSupplierInforServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-lspsupplierinfo-modal',
    templateUrl: './create-or-edit-lspsupplierinfo-modal.component.html',
    })
export class CreateOrEditLspSupplierInfoModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLspSupplierInfo', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepicker2', { static: false }) datepicker2!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLspSupplierInforDto = new CreateOrEditMstLspSupplierInforDto();

    active: boolean = false;
    saving: boolean = false;
    _supplierPlantCode: boolean;
    _isActive: boolean;
    _tcFrom: any;
    _tcTo: any;

    constructor(
        injector: Injector,
        private _mstLspSupplierInfoService: MstLspSupplierInforServiceProxy,
        private _dateTimeService: DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLspSupplierInforDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLspSupplierInforDto();
        else this.rowdata = rowdata;

        const startDateValue = this.rowdata.tcFrom ? new Date(this.rowdata.tcFrom?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(startDateValue);

        const endDateValue = this.rowdata.tcTo ? new Date(this.rowdata.tcTo?.toString()) : new Date();
        this.datepicker2?.bsValueChange.emit(endDateValue);
        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this._supplierPlantCode = (this.rowdata.supplierPlantCode == "W")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
    this.rowdata.supplierPlantCode = (this._supplierPlantCode == true) ? 'A' : 'W';
    this.rowdata.tcFrom = this._dateTimeService.convertToDatetime(this._tcFrom);
    this.rowdata.tcTo = this._dateTimeService.convertToDatetime(this._tcTo);
    this._mstLspSupplierInfoService.createOrEdit(this.rowdata)
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
