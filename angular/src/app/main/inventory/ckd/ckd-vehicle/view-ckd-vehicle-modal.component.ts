import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AsyAdoVehicleDetailsDto, InvCkdVehicleDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { formatDate } from '@angular/common';


@Component({
    selector: 'view-ckd-vehicle-details-modal',
    templateUrl: './view-ckd-vehicle-modal.component.html',
})
export class ViewCKDVehicleDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerwInDateActual', { static: false }) datepickerwInDateActual!: BsDatepickerDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdVehicleDto = new InvCkdVehicleDto();

    active = false;
    saving: boolean = false;
    isActive: boolean;

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show(rowdata?: InvCkdVehicleDto): void {
        if (!rowdata) this.rowdata = new InvCkdVehicleDto();
        else this.rowdata = rowdata;
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
