import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import {  CreateOrEditMstGpsMaterialCategoryMappingDto, MstGpsMaterialCategoryMappingServiceProxy } from '@shared/service-proxies/service-proxies';



@Component({
    selector: 'create-or-edit-gpsmasterialcategorymapping-modal',
    templateUrl: './create-or-edit-gpsmasterialcategorymapping-modal.component.html',
})
export class CreateOrEditGpsMasterialCategoryMappingModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalGpsMtCategoryMapping', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstGpsMaterialCategoryMappingDto = new CreateOrEditMstGpsMaterialCategoryMappingDto();

    active: boolean = false;
    saving: boolean = false;
    _isActive: boolean;
    _yvCategory;

    constructor(
        injector: Injector,
        private _mstService: MstGpsMaterialCategoryMappingServiceProxy,
    ) {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstGpsMaterialCategoryMappingDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstGpsMaterialCategoryMappingDto();
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
        this._mstService.createOrEdit(this.rowdata)
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
