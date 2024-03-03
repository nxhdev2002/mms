import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AsyAdoAInPlanDto } from '@shared/service-proxies/service-proxies';
import { formatDate } from '@angular/common';


@Component({
    selector: 'view-ainplan-detail-modal',
    templateUrl: './view-ainplan-detail-modal.component.html'
    })
export class ViewAInPlanDetailsModalComponent  extends AppComponentBase {
    @ViewChild('viewModalAInPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: AsyAdoAInPlanDto = new AsyAdoAInPlanDto();
    _aInDateActual: any;
    _aOutDateActual: any;
    _aInPlanDatetime: any;
    _aOutPlanDatetime: any;
    _lineoffDatetime: any;
    _lineoffDate: any;
    _pdiDatetime: any;
    _pdiDate: any;
    _pioActualDatetime: any;
    _pioActualDate: any;
    _salesActualDatetime: any;
    _salesActualDate: any;

    constructor( injector: Injector )
    { super(injector); }

    show(rowdata?: AsyAdoAInPlanDto): void {
        if (!rowdata) this.rowdata = new AsyAdoAInPlanDto();
        else this.rowdata = rowdata;

        this._aInDateActual = rowdata.aInDateActual ? formatDate(new Date(rowdata.aInDateActual?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._aOutDateActual = rowdata.aOutDateActual ? formatDate(new Date(rowdata.aOutDateActual?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._aInPlanDatetime = rowdata.aInPlanDatetime ? formatDate(new Date(rowdata.aInPlanDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._aOutPlanDatetime = rowdata.aOutPlanDatetime ? formatDate(new Date(rowdata.aOutPlanDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._lineoffDatetime = rowdata.lineoffDatetime ? formatDate(new Date(rowdata.lineoffDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._lineoffDate = rowdata.lineoffDate ? formatDate(new Date(rowdata.lineoffDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pdiDatetime = rowdata.pdiDatetime ? formatDate(new Date(rowdata.pdiDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._pdiDate = rowdata.pdiDate ? formatDate(new Date(rowdata.pdiDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pioActualDatetime = rowdata.pioActualDatetime ? formatDate(new Date(rowdata.pioActualDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._pioActualDate = rowdata.pioActualDate ? formatDate(new Date(rowdata.pioActualDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._salesActualDatetime = rowdata.salesActualDatetime ? formatDate(new Date(rowdata.salesActualDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._salesActualDate = rowdata.salesActualDate ? formatDate(new Date(rowdata.salesActualDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;

        this.modal.show();
    }

    close(): void {
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
