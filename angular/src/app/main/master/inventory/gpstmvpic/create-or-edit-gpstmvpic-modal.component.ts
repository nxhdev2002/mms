import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstInvGpsTmvPicDto, MstInvGpsTmvPicServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-gpstmvpic-modal',
    templateUrl: './create-or-edit-gpstmvpic-modal.component.html',
})
export class CreateOrEditGpsTmvPicModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsTmvPic', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsTmvPicDto = new CreateOrEditMstInvGpsTmvPicDto();

    active = false;
    saving = false;
    _isMainPic: boolean;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptGpsTmvPicService: MstInvGpsTmvPicServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsTmvPicDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvGpsTmvPicDto();
        else this.rowdata = rowdata;

        this._isMainPic = this.rowdata.isMainPic === "N" ? false : true;

        this._isActive = (this.rowdata.isActive === "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isMainPic = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isMainPic = (this._isMainPic === true) ? 'Y' : 'N'
        this.rowdata.isActive = (this._isActive === true) ? 'Y' : 'N';
        this._mstwptGpsTmvPicService.createOrEdit(this.rowdata)
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
