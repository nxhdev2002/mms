import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvGpsSupplierPicDto, MstInvGpsSupplierPicServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-gpssupplierpic-modal',
    templateUrl: './create-or-edit-gpssupplierpic-modal.component.html',
})
export class CreateOrEditGpsSupplierPicModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsSupplierPic', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsSupplierPicDto = new CreateOrEditMstInvGpsSupplierPicDto();

    active: boolean = false;
    saving: boolean = false;
    _isSendEmail
    _isActive;
    _isMainPic;

    constructor(
        injector: Injector,
        private _mstGpsSupplierPicService: MstInvGpsSupplierPicServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsSupplierPicDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvGpsSupplierPicDto();
        else this.rowdata = rowdata;

        this._isSendEmail = (this.rowdata.isSendEmail == 'N') ? false : true;
        this._isMainPic = (this.rowdata.isMainPic == 'N') ? false : true;
        this._isActive = (this.rowdata.isActive == 'N') ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isSendEmail.isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isSendEmail = (this._isSendEmail == true) ? 'Y' : 'N'
        this.rowdata.isMainPic = (this._isMainPic == true) ? 'Y' : 'N'
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstGpsSupplierPicService.createOrEdit(this.rowdata)
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
