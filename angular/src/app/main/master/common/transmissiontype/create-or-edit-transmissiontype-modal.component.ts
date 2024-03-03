import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {  MstCmmTransmissionTypeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-transmissiontype-modal',
    templateUrl: './create-or-edit-transmissiontype-modal.component.html',
    })
export class CreateOrEditTransmissionTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalTransmissionType', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    // rowdata: CreateOrEditMstCmmTransmissionTypeDto = new CreateOrEditMstCmmTransmissionTypeDto();
    active = false;
    // active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptTransmissionTypeService: MstCmmTransmissionTypeServiceProxy,
    )
    {
        super(injector);
    }

    // show(rowdata?: CreateOrEditMstCmmTransmissionTypeDto): void {
    //     if (!rowdata) this.rowdata = new CreateOrEditMstCmmTransmissionTypeDto();
    //     else this.rowdata = rowdata;

    //     this._isActive = (this.rowdata.isActive == "N")? false: true;
    //     this.active = false;
    //     this.modal.show();
    // }



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
