import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockBalanceDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'view-stockbalance-modal',
    templateUrl: './view-stockbalance-modal.component.html',
    })
export class ViewStockBalanceModalComponent extends AppComponentBase {
    @ViewChild('viewModalStockBalance', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdStockBalanceDto = new InvCkdStockBalanceDto();

    active = false;

    constructor(
        injector: Injector,
        ) {
        super(injector);

    }

    show(rowdata: InvCkdStockBalanceDto): void {
        this.rowdata = rowdata;
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
