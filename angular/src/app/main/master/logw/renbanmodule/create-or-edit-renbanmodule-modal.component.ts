import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstLgwRenbanModuleDto, MstLgwRenbanModuleServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-renbanmodule-modal',
    templateUrl: './create-or-edit-renbanmodule-modal.component.html',
    styleUrls: ['./create-or-edit-renbanmodule-modal.component.less']
    })
export class CreateOrEditRenbanModuleModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalRenbanModule', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgwRenbanModuleDto = new CreateOrEditMstLgwRenbanModuleDto();
    isUsePxpData : boolean = false;
    active: boolean = false;
    saving: boolean = false;
    _isUsePxpData : boolean;
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptRenbanModuleService: MstLgwRenbanModuleServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgwRenbanModuleDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLgwRenbanModuleDto();
        else this.rowdata = rowdata;
        this._isUsePxpData = (this.rowdata.isUsePxpData == "N") ? false : true;
        this._isActive = (this.rowdata.isActive == "N") ? false : true;
        this.isUsePxpData = true;
        this.active = true;
        this.modal.show();
    }

    save(): void {
    this.saving = true;
    this.rowdata.isUsePxpData
    this.rowdata.isUsePxpData = (this._isUsePxpData == true) ? 'Y' : 'N'
    this.rowdata.isActive = (this._isActive == true) ? 'Y' : 'N'
    this._mstwptRenbanModuleService.createOrEdit(this.rowdata)
        .pipe( finalize(() =>  this.saving = false))
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
