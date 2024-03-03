import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {MstCmmBrandServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';

@Component({
    selector: 'create-or-edit-brand-modal',
    templateUrl: './create-or-edit-brand-modal.component.html',
})
export class CreateOrEditBrandModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBrand', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    //rowdata: CreateOrEditMstCmmBrandDto = new CreateOrEditMstCmmBrandDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptBrandService: MstCmmBrandServiceProxy,
    ) {
        super(injector);
    }

    // show(rowdata?: CreateOrEditMstCmmBrandDto): void {
    //     if (!rowdata) this.rowdata = new CreateOrEditMstCmmBrandDto();
    //     else this.rowdata = rowdata;

    //     this._isActive = (this.rowdata.isActive == "N") ? false : true;
    //     this.active = true;
    //     this.modal.show();
    // }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    close(): void {
        this.active = false;
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
