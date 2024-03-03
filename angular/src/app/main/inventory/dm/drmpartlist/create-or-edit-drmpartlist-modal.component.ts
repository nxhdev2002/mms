import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvDrmPartListDto, InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-drmpartlist-modal',
    templateUrl: './create-or-edit-drmpartlist-modal.component.html'

    })
export class CreateOrEditInvDrmPartListModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvDrmPartList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerOrDatetime', { static: false }) datepickerOrDatetime!: BsDatepickerDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvDrmPartListDto = new CreateOrEditInvDrmPartListDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _firtDate: any;
    _lastDate: any;
    _errorCheck: CreateOrEditInvDrmPartListDto[] = [];
    checkQty: boolean = true;
    supplierType;
    supplierTypeList = [
        {value: 'LOCAL' , label: "LOCAL"},
        {value: 'IMPORT' , label: "IMPORT"},
    ];

    constructor(
        injector: Injector,
        private _mstwptInvDrmPartListService: InvDrmPartListServiceProxy,
        private _dateTimeService: DateTimeService,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvDrmPartListDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvDrmPartListDto();
        else this.rowdata = rowdata;
        const dateValue1 = this.rowdata.firstDayProduct ? new Date(this.rowdata.firstDayProduct?.toString()) : null;
        this.datepicker?.bsValueChange.emit(dateValue1);
        const dateValue2 = this.rowdata.lastDayProduct ? new Date(this.rowdata.lastDayProduct?.toString()) : null;
        this.datepickerOrDatetime?.bsValueChange.emit(dateValue2);
        this.active = true;
        this.modal.show();

    }

    changeActive(event){
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        if(!/^[1-9]+/.test(Number(this.rowdata.boxQty).toString()))
        {
            this.message.warn(this.l('boxQty > 0'));
            this.checkQty = false;
        }

        if(this.checkQty)
        {
            this.saving = true;
            this._mstwptInvDrmPartListService.createOrEdit(this.rowdata)
                .pipe( finalize(() =>  this.saving = false))
                .subscribe(() => {

                    this.rowdata.firstDayProduct = this._dateTimeService.convertToDatetime(this._firtDate),
                    this.rowdata.lastDayProduct = this._dateTimeService.convertToDatetime(this._lastDate),
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            
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
