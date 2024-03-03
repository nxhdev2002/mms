import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { InvCkdSmqdOrderLeadTimeDto} from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';

@Component({
    selector: 'view-orderleadtime-modal',
    templateUrl: './view-modal-smqdorderleadtime.html',
    })
export class ViewOrderLeadTimeModalComponent extends AppComponentBase {
    @ViewChild('viewModalSmqdOrderLeadTime', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvCkdSmqdOrderLeadTimeDto = new InvCkdSmqdOrderLeadTimeDto();

    active = false;


    constructor(
        injector: Injector
        ) {
        super(injector);

    }

    show(rowdata?: InvCkdSmqdOrderLeadTimeDto): void {
        if (!rowdata) this.rowdata = new InvCkdSmqdOrderLeadTimeDto();
        else this.rowdata = rowdata;
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
        if (event.key === 'Escape') {
            this.close();
        }
    }
}

