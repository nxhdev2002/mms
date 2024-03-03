import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdStockPartDto } from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';

@Component({
    selector: 'view-stockreceiving-detail-modal',
    templateUrl: './view-stockpart-detail-modal.component.html',
    })
export class ViewStockPartDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdStockPartDto = new InvCkdStockPartDto();

    active = false;
    _isActive: boolean;
    _workingDate;
    _lastcalDateTime;

    constructor(
        injector: Injector
        ) {
        super(injector);

    }

    show(rowdata?: InvCkdStockPartDto): void {
        if (!rowdata) this.rowdata = new InvCkdStockPartDto();
        else this.rowdata = rowdata;

        this._workingDate = rowdata.workingDate ? formatDate(new Date(rowdata.workingDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;

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

