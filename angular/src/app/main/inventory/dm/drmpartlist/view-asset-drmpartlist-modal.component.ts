import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { InvDrmPartListDto, InvDrmPartListServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'view-asset-drmpartlist-modal',
    templateUrl: './view-asset-drmpartlist-modal.component.html'
})
export class ViewAsAssetModalComponent extends AppComponentBase {
    @ViewChild('viewModalAsAsset', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvDrmPartListDto = new InvDrmPartListDto();

    saving: boolean = false;


    constructor(injector: Injector,
        private _service: InvDrmPartListServiceProxy,) {
        super(injector);
    }

    show(id): void {
        this._service.getViewAsAsset(id)
        .subscribe((result) => {
            this.rowdata = result.items[0];

        });
        this.modal.show();
    }

    close(): void {
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
