import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmLookupDto, MstCmmLookupServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-lookup-modal',
    templateUrl: './create-or-edit-lookup-modal.component.html',
})
export class CreateOrEditLookupModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalLookup', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmLookupDto = new CreateOrEditMstCmmLookupDto();

    active: boolean = false;
    saving: boolean = false;
    _isUse: boolean;
    _isRestrict: boolean;

    constructor(
        injector: Injector,
        private _mstwptLookupService: MstCmmLookupServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCmmLookupDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCmmLookupDto();
        else this.rowdata = rowdata;
        this._isUse = (this.rowdata.isUse == "N") ? false : true;
        this._isRestrict = (this.rowdata.isRestrict == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.isUse = (this._isUse == true) ? 'Y' : 'N'
        this.rowdata.isRestrict = (this._isRestrict == true) ? 'Y' : 'N'
        this._mstwptLookupService.createOrEdit(this.rowdata)
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
