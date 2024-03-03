import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { MstCmmMaterialMasterDto } from '@shared/service-proxies/service-proxies';


@Component({
    selector: 'view-materialmaster-modal',
    templateUrl: './view-materialmaster-modal.component.html'
})
export class viewMaterialMasterModalComponent extends AppComponentBase {
    @ViewChild('viewModalMaterialMaster', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmMaterialMasterDto = new MstCmmMaterialMasterDto();

    _isActive: boolean = false;
    saving: boolean = false;
    isActive: boolean;
    

    constructor(injector: Injector,) {
        super(injector);
    }

    show(rowdata?: MstCmmMaterialMasterDto): void {
        if (!rowdata) this.rowdata = new MstCmmMaterialMasterDto();
        else this.rowdata = rowdata;
        this.isActive == false ? "N" : "Y";
  
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
