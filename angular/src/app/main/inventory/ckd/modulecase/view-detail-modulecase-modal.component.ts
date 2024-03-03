import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdModuleCaseDto } from '@shared/service-proxies/service-proxies';
import { formatDate, } from '@angular/common';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    selector: 'view-detail-modulecase-modal',
    templateUrl: './view-detail-modulecase-modal.component.html',
})
export class ViewModuleCaseModalComponent extends AppComponentBase {
    @ViewChild('viewModalModuleCase', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdModuleCaseDto = new InvCkdModuleCaseDto();

    active: boolean = false;
    v_devanningDate;
    v_unpackingDate;
    v_locationDate;
    v_cdDate;
    _plannedpackingdate;
    _unpackingDatetime;
    _unpackingTime;

    fn: CommonFunction = new CommonFunction();

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show(rowdata?: InvCkdModuleCaseDto): void {
        if (!rowdata) this.rowdata = new InvCkdModuleCaseDto();
        else this.rowdata = rowdata;

        this.v_devanningDate = rowdata.devanningDate ? formatDate(new Date(rowdata.devanningDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this.v_unpackingDate = rowdata.unpackingDate ? formatDate(new Date(rowdata.unpackingDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this.v_locationDate = rowdata.locationDate ? formatDate(new Date(rowdata.locationDate?.toString()), 'dd/MM/yyyy hh:mm:ss', 'en-US') : null;
        this.v_cdDate = rowdata.cdDate ? formatDate(new Date(rowdata.cdDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._plannedpackingdate = rowdata.plannedpackingdate ? formatDate(new Date(rowdata.plannedpackingdate?.toString()), 'dd/MM/yyyy ', 'en-US') : null;
        this._unpackingDatetime = rowdata.unpackingDatetime ? formatDate(new Date(rowdata.unpackingDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;

        this._unpackingTime = rowdata.unpackingTime ? rowdata.unpackingTime.slice(0,5) : null;

        this.active = true;
        this.modal.show();
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
