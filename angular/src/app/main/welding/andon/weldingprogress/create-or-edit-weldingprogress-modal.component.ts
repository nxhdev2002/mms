import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditWldAdoWeldingProgressDto, WldAdoWeldingProgressServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-weldingprogress-modal',
    templateUrl: './create-or-edit-weldingprogress-modal.component.html',
    styleUrls: ['./create-or-edit-weldingprogress-modal.component.less']
})
export class CreateOrEditWeldingProgressModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalWeldingProgress', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;
    @ViewChild('datepickerScanTime', { static: false }) datepickerScanTime!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditWldAdoWeldingProgressDto = new CreateOrEditWldAdoWeldingProgressDto();

    active: boolean = false;
    saving: boolean = false;
    _weldTransfer: boolean;
    _isRescan: boolean;
    _isPaintOut: boolean;
    _errorCd: boolean;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptWeldingProgressService: WldAdoWeldingProgressServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditWldAdoWeldingProgressDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditWldAdoWeldingProgressDto();
        else
            this.rowdata = rowdata;
        const dateValueconveyerStatus = this.rowdata.conveyerStatus ? new Date(this.rowdata.lastConveyerRun?.toString()) : new Date();
        this.datepicker?.bsValueChange.emit(dateValueconveyerStatus);

        const dateValuescanTime = this.rowdata.scanTime ? new Date(this.rowdata.scanTime?.toString()) : new Date();
        this.datepickerScanTime?.bsValueChange.emit(dateValuescanTime);


        this._weldTransfer = (this.rowdata.weldTransfer == "N") ? false : true;
        this._isRescan = (this.rowdata.isRescan == "N") ? false : true;
        this._isPaintOut = (this.rowdata.isPaintOut == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.weldTransfer = (this._weldTransfer == true) ? 'Y' : 'N'
        this.rowdata.isRescan = (this._isRescan == true) ? 'Y' : 'N'
        this.rowdata.isPaintOut = (this._isPaintOut == true) ? 'Y' : 'N'
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : "N";
        this._mstwptWeldingProgressService.createOrEdit(this.rowdata)
            .pipe(finalize(() => this.saving = false))
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
