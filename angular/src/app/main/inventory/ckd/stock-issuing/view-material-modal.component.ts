import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmMaterialMasterDto, MstCmmMaterialMasterServiceProxy} from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'view-material-modal',
    templateUrl: './view-material-modal.component.html',
    })
export class ViewMaterialByIdModalComponent extends AppComponentBase {
    @ViewChild('viewModalMaterialMaster', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmMaterialMasterDto = new MstCmmMaterialMasterDto();

    saving: boolean = false;
    isActive: boolean;


    constructor(injector: Injector,
        private _service: MstCmmMaterialMasterServiceProxy,) {
        super(injector);
    }

    show(id): void {
        this._service.getDataMaterialbyId(id)
        .subscribe((result) => {
            this.rowdata = result.items[0];

        });
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


