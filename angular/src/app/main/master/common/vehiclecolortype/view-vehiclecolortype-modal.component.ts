import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmVehicleColorTypeDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'view-vehiclecolortype-modal',
    templateUrl: './view-vehiclecolortype-modal.component.html'
})
export class viewVehicleColorTypeModalComponent extends AppComponentBase {
    @ViewChild('viewVehicleColorType', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmVehicleColorTypeDto = new MstCmmVehicleColorTypeDto();

    isActive: boolean;


    constructor(injector: Injector,) {
        super(injector);
    }

    show(rowdata?: MstCmmVehicleColorTypeDto): void {
        if (!rowdata) this.rowdata = new MstCmmVehicleColorTypeDto();
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
