import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmVehicleNameDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'view-vehiclename-modal',
    templateUrl: './view-vehiclename-modal.component.html'
})
export class viewVehicleNameModalComponent extends AppComponentBase {
    @ViewChild('viewVehicleName', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmVehicleNameDto = new MstCmmVehicleNameDto();

    isActive: boolean;


    constructor(injector: Injector,) {
        super(injector);
    }

    show(rowdata?: MstCmmVehicleNameDto): void {
        if (!rowdata) this.rowdata = new MstCmmVehicleNameDto();
        else this.rowdata = rowdata;
        this.modal.show();
    }

    close(): void {
        this.isActive = false;
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
