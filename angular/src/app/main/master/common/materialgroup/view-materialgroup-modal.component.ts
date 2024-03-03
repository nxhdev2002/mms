import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { MstCmmMaterialGroupDto } from '@shared/service-proxies/service-proxies';



@Component({
    selector: 'view-materialgroup-modal',
    templateUrl: './view-materialgroup-modal.component.html'
})
export class viewMaterialGroupModalComponent extends AppComponentBase {
    @ViewChild('viewModalMaterialGroup', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmMaterialGroupDto = new MstCmmMaterialGroupDto();

    _isActive: boolean = false;
    saving: boolean = false;
    isActive: boolean;
    

    constructor(injector: Injector,) {
        super(injector);
    }

    show(rowdata?: MstCmmMaterialGroupDto): void {
        if (!rowdata) this.rowdata = new MstCmmMaterialGroupDto();
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
