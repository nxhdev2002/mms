import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGpsStockDto } from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';

@Component({
    selector: 'view-gps-stockreceiving-detail-modal',
    templateUrl: './view-gpsstockpart-detail-modal.component.html',
    })
export class ViewGPSStockPartDetailModalComponent extends AppComponentBase {
    @ViewChild('viewModalGpsStock', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvGpsStockDto = new InvGpsStockDto();

    active = false;
    _workingDate;

    constructor(
        injector: Injector
        ) {
        super(injector);

    }

    show(rowdata?: InvGpsStockDto): void {
        this._workingDate = rowdata.workingDate ? formatDate(new Date(rowdata.workingDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        if (!rowdata) this.rowdata = new InvGpsStockDto();
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

