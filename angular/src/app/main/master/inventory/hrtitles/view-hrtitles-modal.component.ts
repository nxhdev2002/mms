import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstInvHrTitlesDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'view-hrtitles-modal',
    templateUrl: './view-hrtitles-modal.component.html',
    })
export class ViewHrTitlesModalComponent extends AppComponentBase {
    @ViewChild('viewHrTitles', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstInvHrTitlesDto = new MstInvHrTitlesDto();
    active = false;
    _isActive: boolean;

    constructor(injector: Injector) {
        super(injector);
    }

    show(rowdata: MstInvHrTitlesDto): void {
        this.rowdata = rowdata;
        this._isActive = (this.rowdata.isActive == "1")? true: false;
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
