import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockIssuingTransactionDto } from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';

@Component({
    selector: 'view-stockissuingtrans',
    templateUrl: './view-stockissuingtrans-modal.component.html',
})
export class ViewGpsStockIssuingTransactionModalComponent extends AppComponentBase {
    @ViewChild('viewGpsStockIssuingTrans', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvGpsStockIssuingTransactionDto = new InvGpsStockIssuingTransactionDto();

    active = false;
    _workingDate;

    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    show(rowdata: InvGpsStockIssuingTransactionDto): void {
        this.rowdata = rowdata;

        this._workingDate = rowdata.workingDate ? formatDate(new Date(rowdata.workingDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;

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
