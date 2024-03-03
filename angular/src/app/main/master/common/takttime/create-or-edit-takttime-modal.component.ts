import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstCmmTaktTimeDto, MstCmmTaktTimeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';



@Component({
    selector: 'create-or-edit-takttime-modal',
    templateUrl: './create-or-edit-takttime-modal.component.html',
})
export class CreateOrEditTakttimeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalTaktTime', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstCmmTaktTimeDto = new CreateOrEditMstCmmTaktTimeDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _service: MstCmmTaktTimeServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstCmmTaktTimeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstCmmTaktTimeDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event) {
        this._isActive = event.target.checked;
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._service.createOrEdit(this.rowdata)
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
