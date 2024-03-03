import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IF_FQF3MM_LV2Dto, IF_FQF3MM_LV2ServiceProxy, InvCkdVehicleDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'view-fqf3mm_lv2-modal',
    templateUrl: './view-fqf3mm_lv2-modal.component.html',
})
export class ViewDetailLV2ModalComponent extends AppComponentBase {
    @ViewChild('viewModalFQF3MM_LV2', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerwInDateActual', { static: false }) datepickerwInDateActual!: BsDatepickerDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    isLoading: boolean = false;
    rowdata: IF_FQF3MM_LV2Dto = new IF_FQF3MM_LV2Dto();

    active = false;
    saving: boolean = false;
    isActive: boolean;

    constructor(
        injector: Injector,
        private _service: IF_FQF3MM_LV2ServiceProxy,
    ) {
        super(injector);
    }

    show(id): void {
        console.log(id);
        this.isLoading = true;
        this._service.getFQF3MMLV2byId(id)
        .subscribe((result) => {
            this.rowdata = result.items[0];
            this.isLoading = false;
        });
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
