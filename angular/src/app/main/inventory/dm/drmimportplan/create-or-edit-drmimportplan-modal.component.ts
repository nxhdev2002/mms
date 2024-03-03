import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {   CreateOrEditInvDrmImportPlanDto, InvDrmImportPlanDto, InvDrmImportPlanServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';
import { formatDate } from '@angular/common';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';


@Component({
    selector: 'create-or-edit-drmimportplan-modal',
    templateUrl: './create-or-edit-drmimportplan-modal.component.html'
    })
export class CreateOrEditDrmImportPlanModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalDrmImportPlan', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: InvDrmImportPlanDto = new InvDrmImportPlanDto();
    rowdataSave : CreateOrEditInvDrmImportPlanDto = new CreateOrEditInvDrmImportPlanDto();

    active: boolean = false;
    saving: boolean = false;
    date = new Date();
    _: boolean;
    _eta;
    _etd;
    _packingMonth;
    _delayEtd;
    _delayEta;
    _ata
    constructor(
        injector: Injector,
        private _mstwptDrmImportPlanService: InvDrmImportPlanServiceProxy,
        private _dateTimeService : DateTimeService
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditInvDrmImportPlanDto): void {

        this._eta = rowdata.eta ? formatDate(new Date(rowdata.eta?.toString()), 'dd/MM/yyyy ', 'en-US') : null;
        this._etd = rowdata.etd ? formatDate(new Date(rowdata.etd?.toString()), 'dd/MM/yyyy ', 'en-US') : null;
        this._packingMonth = rowdata.packingMonth ? formatDate(new Date(rowdata.packingMonth?.toString()), 'dd/MM/yyyy ', 'en-US') : null;
        this._delayEtd = rowdata.delayEta ? formatDate(new Date(rowdata.delayEta?.toString()), 'dd/MM/yyyy ', 'en-US') : null;
        this._delayEta = rowdata.delayEtd ? formatDate(new Date(rowdata.delayEtd?.toString()), 'dd/MM/yyyy ', 'en-US') : null;

        const dateata = rowdata.ata ? new Date(rowdata.ata?.toString()) : this.date;
        this.datepicker?.bsValueChange.emit(dateata);

        if (!rowdata) {
            this.rowdata = new CreateOrEditInvDrmImportPlanDto();
        }  else {
            this.rowdata = rowdata;
            rowdata.status = (rowdata.status === 'S') ? "SCHEDULE" :
            (rowdata.status === 'F') ? "CONFIRM" :
            (rowdata.status === 'C') ? "CANCEL" :
            rowdata.status;
        }
        this.modal.show();
    }

    changeActive(event){
        this._ = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.ata = this._dateTimeService.convertToDatetime(this._ata)
    this._mstwptDrmImportPlanService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
        .subscribe(() => {
            this.notify.info(this.l('SavedSuccessfully'));
            this.close();
            this.modal?.hide();
            this.modalSave.emit(this.rowdata);
        });
    this.saving = false;
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
