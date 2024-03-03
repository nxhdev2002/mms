import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstLgaBarUserDto, MstLgaBarUserServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-baruser-modal',
    templateUrl: './create-or-edit-baruser-modal.component.html',
    styleUrls: ['./create-or-edit-baruser-modal.component.less']
    })
export class CreateOrEditBarUserModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalBarUser', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgaBarUserDto = new CreateOrEditMstLgaBarUserDto();

    active: boolean = false;
    saving: boolean = false;
    _isNeedPass
    _isActive: boolean;

    constructor(
        injector: Injector,
        private _mstwptBarUserService: MstLgaBarUserServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgaBarUserDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLgaBarUserDto();
        else this.rowdata = rowdata;

        this._isNeedPass = (this.rowdata.isNeedPass == "N")? false: true;
        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this.active = true;
        this.modal.show();
    }

    changeActive(event){
        this._isNeedPass = event.target.checked;
        this._isActive = event.target.checked;
    }

    save(): void {
    this.saving = true;
    this.rowdata.isNeedPass= (this._isNeedPass== true) ? 'Y' : 'N'
    this.rowdata.isActive= (this._isActive== true) ? 'Y' : 'N'
    this._mstwptBarUserService.createOrEdit(this.rowdata)
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
