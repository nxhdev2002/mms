import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvGenBOMDataDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { formatDate } from '@angular/common';


@Component({
    selector: 'view-genbomdata-detail-modal',
    templateUrl: './view-genbomdata-detail-modal.component.html',
    })
export class ViewGenBomDataDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvGenBOMDataDto = new InvGenBOMDataDto();

    active = false;
    _periodMpp: any;

    constructor(injector: Injector) {
        super(injector);
    }

    show(rowdata?: InvGenBOMDataDto): void {
        if (!rowdata) this.rowdata = new InvGenBOMDataDto();
        else this.rowdata = rowdata;

        this._periodMpp = rowdata.periodMpp ? formatDate(new Date(rowdata.periodMpp?.toString()), 'dd/MM/yyyy', 'en-US') : null;

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
