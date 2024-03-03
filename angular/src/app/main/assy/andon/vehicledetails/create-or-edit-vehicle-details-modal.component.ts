import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { AsyAdoVehicleDetailsDto, MstCmmLotCodeGradeServiceProxy, MstCmmLotCodeGradeTDto } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-vehicle-details-modal',
    templateUrl: './create-or-edit-vehicle-details-modal.component.html',
    })
export class CreateOrEditVehicleDetailModalComponent extends AppComponentBase {
    @ViewChild('createOrEditVehicleDetails', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: AsyAdoVehicleDetailsDto = new AsyAdoVehicleDetailsDto();

    active = false;
    saving: boolean = false;
    _model;
    goshiCar;
    isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptLotCodeGradeService: MstCmmLotCodeGradeServiceProxy
    ) {
        super(injector);
    }

    show(rowdata?: AsyAdoVehicleDetailsDto): void {
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
