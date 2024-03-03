import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstWptWorkingTypeDto, MstWptWorkingTypeServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-workingtype-modal',
    templateUrl: './create-or-edit-workingtype-modal.component.html',
})
export class CreateOrEditWorkingTypeModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalWorkingType', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();


    rowdata: CreateOrEditMstWptWorkingTypeDto = new CreateOrEditMstWptWorkingTypeDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptWorkingTypeService: MstWptWorkingTypeServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstWptWorkingTypeDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstWptWorkingTypeDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
        this.saving = true;
        this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
        this._mstwptWorkingTypeService.createOrEdit(this.rowdata)
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
