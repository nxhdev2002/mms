import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvGpsScreenSettingDto, MstInvGpsScreenSettingServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-gpsscreensetting-modal',
    templateUrl: './create-or-edit-gpsscreensetting-modal.component.html',
})
export class CreateOrEditGpsScreenSettingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsScreenSetting', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsScreenSettingDto = new CreateOrEditMstInvGpsScreenSettingDto();

    active = false;
    saving = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstGpsScreenSettingService: MstInvGpsScreenSettingServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsScreenSettingDto): void {
        if (!rowdata) {
            this.rowdata = new CreateOrEditMstInvGpsScreenSettingDto();
        } else {
            this.rowdata = rowdata;
        }

        // eslint-disable-next-line eqeqeq
        this._isActive = (this.rowdata.isActive == 'N') ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        // eslint-disable-next-line eqeqeq
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N';
        this._mstGpsScreenSettingService.createOrEdit(this.rowdata)
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

    // eslint-disable-next-line @typescript-eslint/member-ordering
    @HostListener('document:keydown', ['$event'])
    onKeydownHandler(event: KeyboardEvent) {
        if (event.key === 'Escape') {
            this.close();
        }
    }
}
