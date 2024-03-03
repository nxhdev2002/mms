import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {  CreateOrEditMstInvGpsMaterialCategoryDto, MstInvGpsMaterialCategoryServiceProxy } from '@shared/service-proxies/service-proxies';



@Component({
    selector: 'create-or-edit-gpsmasterialcategory-modal',
    templateUrl: './create-or-edit-gpsmasterialcategory-modal.component.html',
})
export class CreateOrEditGpsMasterialCategoryModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsMtCategory', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstInvGpsMaterialCategoryDto = new CreateOrEditMstInvGpsMaterialCategoryDto();

    active: boolean = false;
    saving: boolean = false;
    name: string = '';
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstService1: MstInvGpsMaterialCategoryServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstInvGpsMaterialCategoryDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstInvGpsMaterialCategoryDto();
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
        this._mstService1.createOrEdit(this.rowdata)
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
