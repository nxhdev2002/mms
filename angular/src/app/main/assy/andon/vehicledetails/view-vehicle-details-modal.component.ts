import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { AsyAdoVehicleDetailsDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { formatDate } from '@angular/common';


@Component({
    selector: 'view-vehicle-details-modal',
    templateUrl: './view-vehicle-details-modal.component.html'
})
export class ViewVehicleDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerwInDateActual', { static: false }) datepickerwInDateActual!: BsDatepickerDirective;


    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: AsyAdoVehicleDetailsDto = new AsyAdoVehicleDetailsDto();

    active = false;
    saving: boolean = false;
    _model;
    _isProject;
    _wInDateActual;

    _tInPlanDatetime;
    _aInDateActual;
    _updatedDate;
    _insLineOutVp4DateActual;
    _insOutDateActual;
    _shippingTime;
    _printedQrcodeDate;
    _lineoffDatetime;
    _lineoffDate;
    _pdiDatetime;
    _pdiDate;
    _pioActualDate;
    _pioActualDatetime;
    _salesActualDatetime;
    _salesActualDate;
    _paintInTime;
    _salesActualTime;
    _lineoffTime;
    _pdiTime;
    _pioActualTime;

    goshiCar;
    isActive: boolean;

    constructor(
        injector: Injector,
    ) {
        super(injector);
    }

    show(rowdata?: AsyAdoVehicleDetailsDto): void {
        //convert date view
        this._wInDateActual = rowdata.wInDateActual ? formatDate(new Date(rowdata.wInDateActual?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._aInDateActual = rowdata.aInDateActual ? formatDate(new Date(rowdata.aInDateActual?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._updatedDate = rowdata.updatedDate ? formatDate(new Date(rowdata.updatedDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._insLineOutVp4DateActual = rowdata.insLineOutVp4DateActual ? formatDate(new Date(rowdata.insLineOutVp4DateActual?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._tInPlanDatetime = rowdata.tInPlanDatetime ? formatDate(new Date(rowdata.tInPlanDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._insOutDateActual = rowdata.insOutDateActual ? formatDate(new Date(rowdata.insOutDateActual?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._shippingTime = rowdata.shippingTime ? formatDate(new Date(rowdata.shippingTime?.toString()), 'HH:mm:ss', 'en-US') : null;
        this._printedQrcodeDate = rowdata.printedQrcodeDate ? formatDate(new Date(rowdata.printedQrcodeDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._lineoffDatetime = rowdata.lineoffDatetime ? formatDate(new Date(rowdata.lineoffDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._lineoffDate = rowdata.lineoffDate ? formatDate(new Date(rowdata.lineoffDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pdiDatetime = rowdata.pdiDatetime ? formatDate(new Date(rowdata.pdiDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._pdiDate = rowdata.pdiDate ? formatDate(new Date(rowdata.pdiDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pioActualDate = rowdata.pioActualDate ? formatDate(new Date(rowdata.pioActualDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pioActualDatetime = rowdata.pioActualDatetime ? formatDate(new Date(rowdata.pioActualDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._salesActualDatetime = rowdata.salesActualDatetime ? formatDate(new Date(rowdata.salesActualDatetime?.toString()), 'dd/MM/yyyy HH:mm:ss', 'en-US') : null;
        this._salesActualDate = rowdata.salesActualDate ? formatDate(new Date(rowdata.salesActualDate?.toString()), 'dd/MM/yyyy', 'en-US') : null;
        this._pioActualTime = rowdata.pioActualTime ? rowdata.pioActualTime.substring(0,8) : null;
        this._salesActualTime = rowdata.salesActualTime ? rowdata.salesActualTime.substring(0,8) : null;
        this._isProject = (rowdata.isProject == "0") ? false : true;

        if (!rowdata) this.rowdata = new AsyAdoVehicleDetailsDto();
        else this.rowdata = rowdata;
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
