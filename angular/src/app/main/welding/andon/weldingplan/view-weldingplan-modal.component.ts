import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { WldAdoWeldingPlanDto } from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';



@Component({
    selector: 'view-weldingplan-modal',
    templateUrl: './view-weldingplan-modal.component.html'
    })
export class ViewWeldingPlanModalComponent extends AppComponentBase {
    @ViewChild('viewModalWeldingPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: WldAdoWeldingPlanDto = new WldAdoWeldingPlanDto();

    active: boolean = false;
    _requestDate: any;
    _tInPlanDatetime: any;

    constructor(
        injector: Injector,
    )
    {
        super(injector);
    }

    show(rowdata: WldAdoWeldingPlanDto): void {
        this.rowdata = rowdata;

        this._requestDate = rowdata.requestDate ? formatDate(new Date(rowdata.requestDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._tInPlanDatetime = rowdata.tInPlanDatetime ? formatDate(new Date(rowdata.tInPlanDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;

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
