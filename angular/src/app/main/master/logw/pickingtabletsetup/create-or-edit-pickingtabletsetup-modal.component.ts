import { Component, ViewChild, Injector, ElementRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AppComponentBase } from '@shared/common/app-component-base';
import { finalize } from 'rxjs/operators';
import { CreateOrEditMstLgwPickingTabletProcessDto, MstLgwPickingTabletProcessServiceProxy } from '@shared/service-proxies/service-proxies';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';


@Component({
    selector: 'create-or-edit-pickingtabletsetup-modal',
    templateUrl: './create-or-edit-pickingtabletsetup-modal.component.html',
    styleUrls: ['./create-or-edit-pickingtabletsetup-modal.component.less']
    })
export class CreateOrEditPickingTabletSetupModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModalPickingTabletSetup', { static: true }) modal: ModalDirective | undefined;
    @ViewChild('nameInput') nameInput: ElementRef;
    @ViewChild('datepicker', { static: false }) datepicker!: BsDatepickerDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();

    rowdata: CreateOrEditMstLgwPickingTabletProcessDto = new CreateOrEditMstLgwPickingTabletProcessDto();

    active: boolean = false;
    saving: boolean = false;
    _hasModel: boolean;
    _isActive: boolean;


    constructor(
        injector: Injector,
        private _mstwptPickingTabletSetupService: MstLgwPickingTabletProcessServiceProxy,
    )
    {
        super(injector);
    }

    show(rowdata?: CreateOrEditMstLgwPickingTabletProcessDto): void {
        if (!rowdata) this.rowdata = new CreateOrEditMstLgwPickingTabletProcessDto();
        else this.rowdata = rowdata;

        this._isActive = (this.rowdata.isActive == "N")? false: true;
        this._hasModel = (this.rowdata.hasModel == "N")? false: true;

        this.active = true;
        this.modal.show();
    }


    save(): void {
    this.saving = true;
    this.rowdata.hasModel = (this._hasModel == true)? 'Y': 'N';
    this.rowdata.isActive = (this._isActive == true)? 'Y': 'N';
    this._mstwptPickingTabletSetupService.createOrEdit(this.rowdata)
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
