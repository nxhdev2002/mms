import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { MstCmmLotCodeGradeTDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'view-vehicle-detail-modal',
    templateUrl: './view-vehicle-detail-modal.component.html',
    })
export class ViewVehicleDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: MstCmmLotCodeGradeTDto = new MstCmmLotCodeGradeTDto();

    active = false;
    // eslint-disable-next-line @typescript-eslint/no-inferrable-types
    saving: boolean = false;
    _model;
    goshiCar;
    _isActive: boolean;

    constructor(injector: Injector) {
        super(injector);
    }

    show(rowdata?: MstCmmLotCodeGradeTDto): void {
        if (!rowdata) this.rowdata = new MstCmmLotCodeGradeTDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal?.hide();
        this.modalClose.emit(null);
    }

    // eslint-disable-next-line @typescript-eslint/member-ordering
    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === 'Escape') {
            this.close();
        }
    }
}
