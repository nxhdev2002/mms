import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { InvCkdPhysicalStockIssuingDto, InvCkdPhysicalStockIssuingServiceProxy } from '@shared/service-proxies/service-proxies';
import { DataFormatService } from '@app/shared/common/services/data-format.service';


@Component({
    selector: 'view-modal',
    templateUrl: './view-modal.component.html',
    })
export class ViewPhysicalStockIssuingDetailModalComponent extends AppComponentBase {
    @ViewChild('viewModalVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdPhysicalStockIssuingDto = new InvCkdPhysicalStockIssuingDto();

    active = false;
    saving: boolean = false;
    _isActive: boolean;
    _transactionDatetime;
    pipe: any;


    constructor(
        injector: Injector
        ) {
        super(injector);

    }

    show(rowdata?: InvCkdPhysicalStockIssuingDto): void {
        if (!rowdata) this.rowdata = new InvCkdPhysicalStockIssuingDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N")? false: true;
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
