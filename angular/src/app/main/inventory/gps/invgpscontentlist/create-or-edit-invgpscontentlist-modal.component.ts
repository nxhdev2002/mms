import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditInvGpsContentListDto, InvGpsContentListServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'create-or-edit-invgpscontentlist-modal',
    templateUrl: './create-or-edit-invgpscontentlist-modal.component.html',
    styleUrls: ['./create-or-edit-invgpscontentlist-modal.component.less']
    })
export class CreateOrEditInvGpsContentListModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalInvGpsContentList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerOrDatetime', { static: false }) datepickerOrDatetime!: BsDatepickerDirective;
    @ViewChild('datepickerEst', { static: false }) datepickerEst!: BsDatepickerDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditInvGpsContentListDto = new CreateOrEditInvGpsContentListDto();

    active: boolean = false;
    saving: boolean = false;
    _isPalletOnly
    _isAdhocReceiving
    isAdhocReceiving
    _unpackStatus
    _isActive: boolean;
    _workingDate: any;
    _orderDatetime: any;
    _estArrivalDatetime:any;



    constructor(
        injector: Injector,
        private _mstwptInvGpsContentListService: InvGpsContentListServiceProxy,
        private _dateTimeService: DateTimeService
    ) 
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvGpsContentListDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditInvGpsContentListDto();
        else this.rowdata = rowdata;
        const dateValue1 = this.rowdata.workingDate ? new Date(this.rowdata.workingDate?.toString()) : null;
        this.datepicker?.bsValueChange.emit(dateValue1);
        const dateValue2 = this.rowdata.orderDatetime ? new Date(this.rowdata.orderDatetime?.toString()) : null;
        this.datepickerOrDatetime?.bsValueChange.emit(dateValue2);
        const dateValue3 = this.rowdata.estArrivalDatetime ? new Date(this.rowdata.estArrivalDatetime?.toString()) : null;
        this.datepickerEst?.bsValueChange.emit(dateValue3);
        this._unpackStatus = (this.rowdata.unpackStatus == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this._isAdhocReceiving = (this.rowdata.isAdhocReceiving == "N")? false : true;
        this._isPalletOnly = (this.rowdata.isPalletOnly == "N") ?  false : true; 
        this.active = true;
        this.modal.show();
    } 

    changeActive(event){
        this._isPalletOnly
        this._isAdhocReceiving
        this._unpackStatus
        this._isActive = event.target.checked;
    }
 
    save(): void {
    this.saving = true; 
    this.rowdata.workingDate = this._dateTimeService.convertToDatetime(this._workingDate),
    this.rowdata.orderDatetime = this._dateTimeService.convertToDatetime(this._orderDatetime),
    this.rowdata.estArrivalDatetime = this._dateTimeService.convertToDatetime(this._estArrivalDatetime),
    this.rowdata.unpackStatus = (this._unpackStatus == true) ? 'Y' : 'N'
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this.rowdata.isAdhocReceiving = (this._isAdhocReceiving == true) ? 'Y' : 'N'
    this.rowdata.isPalletOnly = (this._isPalletOnly == true) ? 'Y' : 'N'

    this._mstwptInvGpsContentListService.createOrEdit(this.rowdata)
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
