import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener, Renderer2, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvDemDetDaysDto, MstInvDemDetDaysServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-demdetdays-modal',
    templateUrl: './create-or-edit-demdetdays-modal.component.html',
    styleUrls: ['./create-or-edit-demdetdays-modal.component.less']
})
export class CreateOrEditDemDetDaysModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalDemDetDays', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvDemDetDaysDto = new CreateOrEditMstInvDemDetDaysDto();

    active: boolean = false;
    saving: boolean = false;
    combineDEMDET: boolean;
    isReadOnly: boolean = true;
    _: boolean;
    expList = [];
    carrierList = [];
    minwidth0 = "minwidth0";
    _exp;
    _carrier;
    constructor(
        injector: Injector,
        private _mstinvDemDetdaysService: MstInvDemDetDaysServiceProxy,
        private el: ElementRef,
        private renderer: Renderer2
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvDemDetDaysDto): void {
        if (!rowdata) {
            this.rowdata = new CreateOrEditMstInvDemDetDaysDto();
            this.isReadOnly = false
        }
        else {
            this.rowdata = rowdata;
            this.isReadOnly = (this.rowdata.combineDEMDET == "N")? false: true;
        }
        this.combineDEMDET = (this.rowdata.combineDEMDET == "N")? false: true;

        this._mstinvDemDetdaysService.getListExp().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.expList.push({ value: e.exp, label: e.exp });
                })
            }
        })
        this._mstinvDemDetdaysService.getListCarrier().subscribe(res => {
            if (res) {
                res.map(e => {
                    this.carrierList.push({ value: e.carrier, label: e.carrier });
                })
            }
        })
        this.modal.show();

    }

    changeActive(event) {
        this.combineDEMDET = event.target.checked;
    }

    save(): void {
        if (this.validateD()) {
            this.saving = true;
            this.rowdata.combineDEMDET = (this.combineDEMDET == true) ? 'Y' : 'N';
            this._mstinvDemDetdaysService.createOrEdit(this.rowdata)
                .pipe(finalize(() => this.saving = false))
                .subscribe(() => {
                    this.notify.info(this.l('SavedSuccessfully'));
                    this.close();
                    this.modal?.hide();
                    this.modalSave.emit(this.rowdata);
                });
            this.saving = false;
        }
    }
    validateD() {

        if (Number.isInteger(this.rowdata.freeDEM) && Number.isInteger(this.rowdata.freeDET) && Number.isInteger(this.rowdata.combineDEMDETFree)) {
            if (this.rowdata.freeDEM < 0) {
                this.message.warn(this.l('Free Dem must be positve'));
                return false;
            } else if (this.rowdata.freeDET < 0) {
                this.message.warn(this.l('Free Det must be positve'));
                return false;
            } else if (this.rowdata.combineDEMDETFree < 0) {
                this.message.warn(this.l('Combine Dem Det Free must be positve'));
                return false;
            }
        } else {
            this.message.warn(this.l('Free Dem/ Free Det  / Combine Dem Det must be positve integer'));
            return false;
        }

        return true;
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
