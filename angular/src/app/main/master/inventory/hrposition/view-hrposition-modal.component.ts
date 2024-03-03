import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import {  MstInvHrPositionDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'view-hrposition-modal',
    templateUrl: './view-hrposition-modal.component.html',
    })
export class ViewHrPositionModalComponent extends AppComponentBase {
    @ViewChild('viewModalHrPosition', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstInvHrPositionDto = new MstInvHrPositionDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
    )
    {
        super(injector);
    }

    show(rowdata?: MstInvHrPositionDto): void {
       if (!rowdata) this.rowdata = new MstInvHrPositionDto();
        else this.rowdata = rowdata;
        this._isActive = (this.rowdata.isActive == "1")? true: false;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
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
