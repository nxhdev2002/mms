import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { InvCkdPhysicalStockIssuingDto, InvCkdPhysicalStockIssuingServiceProxy, MstInvSupplierListDto } from '@shared/service-proxies/service-proxies';
import { DataFormatService } from '@app/shared/common/services/data-format.service';


@Component({
    selector: 'view-modal',
    templateUrl: 'view-modal-supplierlist.component.html',
    })
export class ViewSupplierListModalComponent extends AppComponentBase {
    @ViewChild('viewSupplierList', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerWorkingDate', { static: false }) datepickerWorkingDate!: BsDatepickerDirective;
    @ViewChild('datepickerTransactionDatetime', { static: false }) datepickerTransactionDatetime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstInvSupplierListDto = new MstInvSupplierListDto();

    active = false;
    saving: boolean = false;
    _isActive: boolean;
    _transactionDatetime;
    pipe: any;


    constructor(
        injector: Injector,
        private _mstwptPartReceivingService: InvCkdPhysicalStockIssuingServiceProxy,
        private _dateTimeService: DateTimeService,
        private _fm: DataFormatService,
        ) {
        super(injector);

    }

    show(rowdata?: MstInvSupplierListDto): void {
        if (!rowdata) this.rowdata = new MstInvSupplierListDto();
        else this.rowdata = rowdata;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === 'Escape') {
            this.close();
        }
    }
}
